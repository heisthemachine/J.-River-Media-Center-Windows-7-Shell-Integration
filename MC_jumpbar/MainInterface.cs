#region Libraries
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Taskbar;
using Microsoft.WindowsAPICodePack.Dialogs;
using Windows7.DesktopIntegration;
using Windows7.DesktopIntegration.Interop;
using System.Design;
using MediaCenter;
using System.Drawing.Drawing2D;
using Windows7.DesktopIntegration.WindowsForms;
using MC_Jumpbar.Properties;
using System.Threading;
using System.Diagnostics;

#endregion

namespace MC_CSPlugin_Test
{
    #region Interop Program ID Registration

    // This string must unique and match the String in the Inno Setup Script
    [System.Runtime.InteropServices.ProgId("MCPlugin.MC_jumpbar")]
    #endregion

    public partial class MainInterface : UserControl
    {
        #region Attributes

        private MediaCenter.MCAutomation mcRef;
        private TaskbarManager windowsTaskbar = TaskbarManager.Instance;
        private static string AppId = "MC_Jumpbar";
        private Win32WndProc newWndProc;
        public IntPtr oldWndProc = IntPtr.Zero;
        private Bitmap screen;
        private Rectangle windowsize;
        private bool windowMinimized = false;
        private StringBuilder oldWindowText = new StringBuilder();
        private IMJFileAutomation nowPlayingFile;
        private IMJPlaybackAutomation playback;
        private Settings s = new Settings();

        #endregion

        #region Constants
        public const int WM_DWMSENDICONICTHUMBNAIL = 0x0323;
        public const int WM_DWMSENDICONICLIVEPREVIEWBITMAP = 0x0326;
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MINIMIZE = 0xF020;
        public const int SC_RESTORE = 0xF120;
        private const int GWL_WNDPROC = -4;
        private const int WM_DESTROY = 0x0002;
        #endregion

        #region DLL Imports

        [DllImport("user32")]
        private static extern bool GetWindowRect(IntPtr hWnd, ref Rectangle rect);
        [DllImport("user32")]
        private static extern int CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32")]
        private static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, Win32WndProc newProc);
        [DllImport("user32")]
        private static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr newProc);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetActiveWindow();
        [DllImport("user32.dll")]
        static extern int SetWindowText(IntPtr hWnd, string text);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        // For Windows Mobile, replace user32.dll with coredll.dll
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        // Find window by Caption only. Note you must pass IntPtr.Zero as the first parameter.
        [DllImport("user32.dll", EntryPoint="FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, IntPtr windowTitle);

        #endregion

        #region Delegates
        private delegate int Win32WndProc(IntPtr hWnd, int Msg,int wParam, int lParam);
        #endregion

        #region Constructor

        public MainInterface()
        {
            InitializeComponent();
            
            // Don't run unless the taskbar extensions are supported (currently Windows 7+).
            if (!TaskbarManager.IsPlatformSupported)
            {
                MessageBox.Show("This sample requires Windows 7 to run");
                Application.Exit();
            }
//            this.mcRef = mcRef;
            //mcRef.FireMJEvent += new IMJAutomationEvents_FireMJEventEventHandler(MJEvent);
            //initAll();

            // The application ID is used to group windows together
            windowsTaskbar.ApplicationId = AppId;
            
            //Init(mcRef);
        }
        #endregion

        #region Media Center Initialisation

        // Set all Components on the Panel to invisible
        private void setVisibility(Control ctlControl, Boolean blnVisible)
        {
            while (ctlControl != null)
            {
                ctlControl.Visible = blnVisible;

                ctlControl = mainPanel.GetNextControl(ctlControl, true);
            }
        }

        // Check the version of Media Center.
        // Don't support C# Plugins with versions lower than 11.1
        // Certain Events are not supported with older versions
        private Boolean checkVersion()
        {
            IMJVersionAutomation version;

            version = mcRef.GetVersion();

            if (version.Major >= 12 && version.Minor >= 0 && version.Build >= 213)
            {
                return true;
            }

            //setVisibility(Panel, false);

            Panel.Visible = true;
            txtUserInfo.Visible = true;

            return false;
        }

        public void Init(MediaCenter.MCAutomation mcRef)
        {
            try
            {

                //if (checkVersion())
                //{
                    // Tell MC to call our MJEvent Routine in case of evenst
                this.mcRef = mcRef;
                this.mcRef.FireMJEvent += new IMJAutomationEvents_FireMJEventEventHandler(MJEvent);
                // Init our plugin
                initAll();
                // This is the main entry for MC Automation
                txtUserInfo.Visible = true;
                addUserInfoText("Plugin Initiated OK");
                StartSubclass((IntPtr)mcRef.GetWindowHandle());
                windowsize = new Rectangle();
                GetWindowRect((IntPtr)mcRef.GetWindowHandle(), ref windowsize);
                screen = new Bitmap(windowsize.Width, windowsize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                screen = ScreenCapture.GrabWindowBitmap((IntPtr)mcRef.GetWindowHandle(), new Size(windowsize.Width, windowsize.Height));
                screen.RotateFlip(RotateFlipType.Rotate180FlipX);
                GetWindowText((IntPtr)mcRef.GetWindowHandle(), oldWindowText, oldWindowText.Capacity);
                backgroundWorker1.RunWorkerAsync();
                                
            }
            catch (Exception e)
            {
                errorHandler(e);
            }
        }

        private void initAll()
        {
            try
            {
                // Set some skin colors
                setSkinColors();
            }
            catch (Exception e)
            {
                errorHandler(e);
            }
        }

        #endregion

        #region ErrorHandler
        private void errorHandler(Exception e)
        {
            addUserInfoText("A Fatal error has occured: " + e.Message);
            addUserInfoText("The Failure Occured In Class Object: " + e.Source);
            addUserInfoText("when calling Method " + e.TargetSite);
            addUserInfoText("The following Inner Exception was caused" + e.InnerException);
            addUserInfoText("The Stack Trace Follows:\r\n" + e.StackTrace);

            txtUserInfo.Dock = DockStyle.Fill;

            this.Enabled = false;
        }
        #endregion

        #region Setting Skin Colors

        private void skinPlugin()
        {
            try
            {
                // MC 12 does not support skinning of C# plugins
                // We do some pseudo skinning by setting the colors used in the skin
                setSkinColors();
            }
            catch (Exception e)
            {
                errorHandler(e);
            }
        }

        // Get the Color for an item and attribute (i.e. "Tree", "BackColor"
        // Look in the wiki for the Metamorphis subject for Items and Attributes
        // http://wiki.jrmediacenter.com/index.php/Metamorphis
        private Color getColor(String strItem, String strAttribute)
        {
            Color colReturned = Color.LightGray;
            int intColor;
            int intR;
            int intG;
            int intB;

            try
            {
                intColor = mcRef.GetSkinInfo(strItem, strAttribute);

                // The color is represented as an int in MC. 
                // Windows requires a ARGB Color object
                // Using bitshifting and masking to get the R, G and B values
                if (intColor != -1)
                {
                    intR = intColor & 255;
                    intG = (intColor >> 8) & 255;
                    intB = (intColor >> 16) & 255;

                    colReturned = Color.FromArgb(intR, intG, intB);
                }
            }
            catch (Exception e)
            {
                errorHandler(e);
            }

            return colReturned;
        }

        // Setting the Backcolor of a Control (if different)
        private void setBackColor(Control control, Color color)
        {
            if (!control.BackColor.Equals(color))
            {
                control.BackColor = color;
            }
        }

        // Setting the Forecolor of a Control (if different)
        private void setForeColor(Control control, Color color)
        {
            if (!control.ForeColor.Equals(color))
            {
                control.ForeColor = color;
            }
        }

        // Setting the Fore- and Backcolors of all Controls (if different)
        private void setAllColors(Control control)
        {
            while (control != null)
            {
                setBackColor(control, getColor("Tree", "BackColor"));
                setForeColor(control, getColor("Tree", "TextColor"));

                control = mainPanel.GetNextControl(control, true);
            }
        }

        // Setting the Color for Maininterface
        private void setMainInterfaceColors()
        {
            this.BackColor = getColor("Tree", "BackColor");
            this.ForeColor = getColor("Tree", "TextColor");
        }

        // Pseude Skin our Plugin
        private void setSkinColors()
        {
            try
            {
                setMainInterfaceColors();
                setAllColors(Panel);
            }
            catch (Exception e)
            {
                errorHandler(e);
            }
        }

        #endregion

        #region Event Handling

        // Just for debugging purposes. Fill a text in the txtUserInfo Textbox
        private void addUserInfoText(String strText)
        {
            txtUserInfo.Text = txtUserInfo.Text + strText + "\r\n";
        }
        
        // This routine is called by MC in case of an Event
        // s1 - String containing the the MC Command.
        // s2 - String containing the the MC Event
        // s3 - String containing supplement information for the Event (not always filled)
        private void MJEvent(String strCommand, String strEvent, String strEventInfo)
        {
            // Debug info
            addUserInfoText(strCommand + "/" + strEvent + "/" + strEventInfo);
            //ScreenCapture.GrabWindowBitmap((IntPtr)mcRef.GetWindowHandle(), new Size(windowsize.Width, windowsize.Height), out screen);
            switch (strCommand)
            {
                case "MJEvent type: MCCommand":
                    switch (strEvent)
                    {
                        case "MCC: NOTIFY_TRACK_CHANGE":
                        case "MCC: NOTIFY_PLAYERSTATE_CHANGE":
                            backgroundWorker1.RunWorkerAsync();
                            break;

                        case "MCC: NOTIFY_PLAYLIST_ADDED":
                            // Your code goes here
                            break;

                        case "MCC: NOTIFY_PLAYLIST_INFO_CHANGED":
                            // Your code goes here
                            break;

                        case "MCC: NOTIFY_PLAYLIST_FILES_CHANGED":
                            // Your code goes here
                            break;

                        case "MCC: NOTIFY_PLAYLIST_REMOVED":
                            // Your code goes here
                            break;

                        case "MCC: NOTIFY_PLAYLIST_COLLECTION_CHANGED":
                            // Your code goes here
                            break;

                        case "MCC: NOTIFY_PLAYLIST_PROPERTIES_CHANGED":
                            // Your code goes here
                            break;

                        case "MCC: NOTIFY_SKIN_CHANGED":
                            skinPlugin();
                            break;
                    }

                    break;

                default:
                    break;
            }
        }


 /*           public void MCWin7Taskbar()
            {
                IMJPlaybackAutomation playback;
                playback = mcRef.GetPlayback();
                //this.mcRef = mcRef;
                //this.mcRef.FireMJEvent += new IMJAutomationEvents_FireMJEventEventHandler(MJEvent);
                try
                {

                          
                }
                catch (Exception e)
                {
                    errorHandler(e);
                }
            }*/

        // At the time this template was created the MCC: NOTIFY_SKIN_CHANGED
        // Event didn't function correctly. Doing it on a PAINT Event from Windows.
        private void mainPanel_Paint(object sender, PaintEventArgs e)
        {
            skinPlugin();
        }

        #endregion

        #region Window Proc stuff
        public void StartSubclass(IntPtr hWnd)
        {
            // delegate for the new wndproc
            newWndProc = new Win32WndProc(MyWndProc);

            if (oldWndProc.ToInt32() != 0)
                SetWindowLong(hWnd, GWL_WNDPROC, newWndProc);
            else
                oldWndProc = SetWindowLong(hWnd, GWL_WNDPROC, newWndProc);
        }

        /// <summary>
        /// After we have finished we need to 
        /// return the old handle, else we will
        /// never be able to exit clean from the
        /// program
        /// </summary>
        /// <param name="hWnd"></param>
        public void StopSubclass(IntPtr hWnd)
        {
            if (oldWndProc.ToInt32() != 0) SetWindowLong(hWnd, GWL_WNDPROC, oldWndProc);
        }

        private int MyWndProc(IntPtr hWnd, int Msg, int wParam, int lParam)
        {

            //switch (Msg)
            //{
            //    case WM_SYSCOMMAND:
            //        switch (wParam)
            //        {
            //            case SC_MINIMIZE:
            //                windowMinimized = true;
            //                break;
            //            case SC_RESTORE:
            //                windowMinimized = false;
            //                break;
            //        }
            //        break;
            //    case WM_DWMSENDICONICLIVEPREVIEWBITMAP:
            //        setWindowsPeak();
            //        break;

            //}
            return CallWindowProc(oldWndProc, hWnd, Msg, wParam, lParam);
        }
        #endregion

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtUserInfo_TextChanged(object sender, EventArgs e)
        {

        }

        private void setWindowTitle()
        {

            IMJPlaybackAutomation playback = mcRef.GetPlayback();

            if (playback.State != MJPlaybackStates.PLAYSTATE_STOPPED && displayArtistTrackName.Checked)
            {
                Windows7Taskbar.SetThumbnailTooltip((IntPtr)mcRef.GetWindowHandle(), nowPlayingFile.Artist + " - " + nowPlayingFile.Name);
                SetWindowText((IntPtr)mcRef.GetWindowHandle(), nowPlayingFile.Artist + " - " + nowPlayingFile.Name);
            }
            else
            {
                SetWindowText((IntPtr)mcRef.GetWindowHandle(), oldWindowText.ToString());
                Windows7Taskbar.SetThumbnailTooltip((IntPtr)mcRef.GetWindowHandle(), oldWindowText.ToString());
            }
        }

        private IntPtr getDisplayWindowHandle()
        {
            IntPtr displayHandle = FindWindow("J. River Display Container Window", "Display");
            if (displayHandle == IntPtr.Zero)
            {
                IntPtr MainWindow = FindWindowEx((IntPtr)mcRef.GetWindowHandle(), IntPtr.Zero, "MainUIWnd", IntPtr.Zero);
                IntPtr MCViewContainer = FindWindowEx(MainWindow, IntPtr.Zero, "MC View Container", IntPtr.Zero);
                IntPtr displayContainer = FindWindowEx(MCViewContainer, IntPtr.Zero, "J. River Display Window", IntPtr.Zero);
                displayHandle = FindWindowEx(displayContainer, IntPtr.Zero, "J. River Display Container Window", IntPtr.Zero);
            }

            return displayHandle;
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            playback = mcRef.GetPlayback();
            //customWindow = Windows7.DesktopIntegration.CustomWindowsManager.CreateWindowsManager((IntPtr)mcRef.GetWindowHandle(), IntPtr.Zero);
            //IMJFileAutomation nowPlayingFile;
            //nowPlayingFile = mcRef.GetCurPlaylist().GetFile(mcRef.GetCurPlaylist().Position);

            int i = 0;

            string prevFile = "";
            string currentFile = "";
            
            while (playback.State != MJPlaybackStates.PLAYSTATE_STOPPED)
            {
                nowPlayingFile = mcRef.GetCurPlaylist().GetFile(mcRef.GetCurPlaylist().Position);

                setWindowsPeak();
                if (enableCoverArt.Checked)
                {
                    Windows7Taskbar.EnableCustomWindowPreview((IntPtr)mcRef.GetWindowHandle());
                    currentFile = nowPlayingFile.GetImageFile(MJImageFileFlags.IMAGEFILE_THUMBNAIL_MEDIUM);
                    if (!string.IsNullOrEmpty(currentFile))
                    {
                        if (currentFile != prevFile)
                        {
                            setPreview(currentFile);
                        }
                    }
                    else
                    {
                        Windows7Taskbar.DisableCustomWindowPreview((IntPtr)mcRef.GetWindowHandle());
                    }

                    prevFile = currentFile;
                }
                else
                {
                    Windows7Taskbar.DisableCustomWindowPreview((IntPtr)mcRef.GetWindowHandle());
                }
                
                
                createThumbnail(getDisplayWindowHandle());
                if (trackProgress.Checked)
                {
                    if (playback.State == MJPlaybackStates.PLAYSTATE_PLAYING)
                    {
                        windowsTaskbar.SetProgressState(TaskbarProgressBarState.Normal);
                        addUserInfoText("Duration: " + playback.Duration);
                        if (nowPlayingFile.Duration <= 0) windowsTaskbar.SetProgressState(TaskbarProgressBarState.Indeterminate);
                        else
                        {
                            if (playback.Position >= 0) windowsTaskbar.SetProgressValue(playback.Position, nowPlayingFile.Duration);
                        }
                    }
                    else if (playback.State == MJPlaybackStates.PLAYSTATE_PAUSED) windowsTaskbar.SetProgressState(TaskbarProgressBarState.Paused);
                }

                else if (playlistProgress.Checked)
                {
                    windowsTaskbar.SetProgressState(playback.State != MJPlaybackStates.PLAYSTATE_PAUSED ? TaskbarProgressBarState.Normal : TaskbarProgressBarState.Paused);
                    windowsTaskbar.SetProgressValue(mcRef.GetCurPlaylist().Position, mcRef.GetCurPlaylist().GetNumberFiles());
                }

                else
                {
                    windowsTaskbar.SetProgressState(TaskbarProgressBarState.NoProgress);
                }
                setWindowTitle();
                System.Threading.Thread.Sleep(500);
                GC.Collect();
                i++;
            }
            //Windows7Taskbar.DisableCustomWindowPreview((IntPtr)mcRef.GetWindowHandle());
            windowsTaskbar.SetProgressState(TaskbarProgressBarState.NoProgress);
            return; 
        }

        //The following method is part of the lab. Create a thumbnail and register for the event so that
        //you can render its bitmap on demand. Use an offset from the main window to the black image.
        private void createThumbnail(IntPtr displayHandle)
        {
            if (displayHandle != IntPtr.Zero  && windowsTaskbar.TabbedThumbnail.GetThumbnailPreview(displayHandle) == null)
            {
                TabbedThumbnail preview = new TabbedThumbnail((IntPtr)mcRef.GetWindowHandle(), displayHandle);
                windowsTaskbar.TabbedThumbnail.AddThumbnailPreview(preview);

                windowsTaskbar.TabbedThumbnail.SetActiveTab(preview);
            }
        }

        //The following method is part of the lab. When a thumbnail is requested, provide it on demand
        //by loading an image from a resource.
        private void TabbedThumbnail_TabbedThumbnailBitmapRequested(object sender, TabbedThumbnailEventArgs e)
        {
            //if (_sneakySource == null)
            //{
            //    _sneakySource = new BitmapImage();
            //    _sneakySource.BeginInit();
            //    _sneakySource.DecodePixelHeight = 200;
            //    _sneakySource.UriSource = new Uri("pack://application:,,,/assets/SpeedyGonzales.jpg");
            //    _sneakySource.EndInit();
            ////}
            Bitmap preview = new Bitmap(nowPlayingFile.GetImageFile(MJImageFileFlags.IMAGEFILE_THUMBNAIL_MEDIUM));
            e.TabbedThumbnail.SetImage(preview);
            e.TabbedThumbnail.DisplayFrameAroundBitmap = false;
        }

        private void setWindowsPeak()
        {
            if (!windowMinimized)
            {
                GetWindowRect((IntPtr)mcRef.GetWindowHandle(), ref windowsize);
                ScreenCapture.GrabWindowBitmap((IntPtr)mcRef.GetWindowHandle(), windowsize.Size, out screen);
                screen.RotateFlip(RotateFlipType.Rotate180FlipX);
            }
            if (screen != null) Windows7Taskbar.SetPeekBitmap((IntPtr)mcRef.GetWindowHandle(), screen, false);
            GC.Collect();
        }

        private void setPreview(string fileLocation)
        {
            if (!string.IsNullOrEmpty(fileLocation))
            {
                using (Image coverArt = Image.FromFile(fileLocation))
                {
                    Windows7Taskbar.SetIconicThumbnail((IntPtr)mcRef.GetWindowHandle(), (Bitmap)coverArt.GetThumbnailImage(120, 120, null, IntPtr.Zero));
                }
                return;
            }
        }

        private void Panel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            s.enableCoverArt = ((CheckBox)sender).Checked;
            s.Save();
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            enableCoverArt.Checked = s.enableCoverArt;
            displayArtistTrackName.Checked = s.displayArtistTrackName;
            trackProgress.Checked = s.trackProgress;
            playlistProgress.Checked = s.playlistProgress;
            noProgressTrack.Checked = s.noProgressTrack;
            base.OnLoad(e);
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            // Listen for operating system messages.
            switch (m.Msg)
            {
                case WM_DESTROY:
                    StopSubclass((IntPtr)mcRef.GetWindowHandle());
                break;
            }
            base.WndProc(ref m);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            s.displayArtistTrackName = ((CheckBox)sender).Checked;
            s.Save();
        }

        private void trackProgress_CheckedChanged(object sender, EventArgs e)
        {
            s.trackProgress = ((RadioButton)sender).Checked;
            s.Save();
        }

        private void playlistProgress_CheckedChanged(object sender, EventArgs e)
        {
            s.playlistProgress = ((RadioButton)sender).Checked;
            s.Save();
        }

        private void noProgressTrack_CheckedChanged(object sender, EventArgs e)
        {
            s.noProgressTrack = ((RadioButton)sender).Checked;
            s.Save();
        }
    }

    
}
