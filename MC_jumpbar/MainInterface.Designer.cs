namespace MC_CSPlugin_Test
{
    partial class MainInterface
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Panel = new System.Windows.Forms.Panel();
            this.displayArtistTrackName = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.noProgressTrack = new System.Windows.Forms.RadioButton();
            this.playlistProgress = new System.Windows.Forms.RadioButton();
            this.trackProgress = new System.Windows.Forms.RadioButton();
            this.enableCoverArt = new System.Windows.Forms.CheckBox();
            this.txtUserInfo = new System.Windows.Forms.TextBox();
            this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.Panel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Panel
            // 
            this.Panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Panel.Controls.Add(this.displayArtistTrackName);
            this.Panel.Controls.Add(this.groupBox1);
            this.Panel.Controls.Add(this.enableCoverArt);
            this.Panel.Location = new System.Drawing.Point(0, 0);
            this.Panel.Name = "Panel";
            this.Panel.Size = new System.Drawing.Size(780, 305);
            this.Panel.TabIndex = 0;
            this.Panel.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel_Paint);
            // 
            // displayArtistTrackName
            // 
            this.displayArtistTrackName.AutoSize = true;
            this.displayArtistTrackName.Location = new System.Drawing.Point(20, 47);
            this.displayArtistTrackName.Name = "displayArtistTrackName";
            this.displayArtistTrackName.Size = new System.Drawing.Size(240, 17);
            this.displayArtistTrackName.TabIndex = 8;
            this.displayArtistTrackName.Text = "Display Artist and Track Name in the Title Bar";
            this.displayArtistTrackName.UseVisualStyleBackColor = true;
            this.displayArtistTrackName.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.noProgressTrack);
            this.groupBox1.Controls.Add(this.playlistProgress);
            this.groupBox1.Controls.Add(this.trackProgress);
            this.groupBox1.Location = new System.Drawing.Point(14, 98);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 115);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Progress Indicator Type";
            // 
            // noProgressTrack
            // 
            this.noProgressTrack.AutoSize = true;
            this.noProgressTrack.Location = new System.Drawing.Point(6, 84);
            this.noProgressTrack.Name = "noProgressTrack";
            this.noProgressTrack.Size = new System.Drawing.Size(51, 17);
            this.noProgressTrack.TabIndex = 7;
            this.noProgressTrack.Text = "None";
            this.noProgressTrack.UseVisualStyleBackColor = true;
            this.noProgressTrack.CheckedChanged += new System.EventHandler(this.noProgressTrack_CheckedChanged);
            // 
            // playlistProgress
            // 
            this.playlistProgress.AutoSize = true;
            this.playlistProgress.Location = new System.Drawing.Point(6, 51);
            this.playlistProgress.Name = "playlistProgress";
            this.playlistProgress.Size = new System.Drawing.Size(57, 17);
            this.playlistProgress.TabIndex = 5;
            this.playlistProgress.Text = "Playlist";
            this.playlistProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.playlistProgress.UseVisualStyleBackColor = true;
            this.playlistProgress.CheckedChanged += new System.EventHandler(this.playlistProgress_CheckedChanged);
            // 
            // trackProgress
            // 
            this.trackProgress.AutoSize = true;
            this.trackProgress.Location = new System.Drawing.Point(6, 19);
            this.trackProgress.Name = "trackProgress";
            this.trackProgress.Size = new System.Drawing.Size(53, 17);
            this.trackProgress.TabIndex = 6;
            this.trackProgress.Text = "Track";
            this.trackProgress.UseVisualStyleBackColor = true;
            this.trackProgress.CheckedChanged += new System.EventHandler(this.trackProgress_CheckedChanged);
            // 
            // enableCoverArt
            // 
            this.enableCoverArt.AutoSize = true;
            this.enableCoverArt.Location = new System.Drawing.Point(20, 13);
            this.enableCoverArt.Name = "enableCoverArt";
            this.enableCoverArt.Size = new System.Drawing.Size(197, 17);
            this.enableCoverArt.TabIndex = 4;
            this.enableCoverArt.Text = "Enable Cover Art Thumbnail (Buggy)";
            this.enableCoverArt.UseVisualStyleBackColor = true;
            this.enableCoverArt.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged_1);
            // 
            // txtUserInfo
            // 
            this.txtUserInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtUserInfo.Location = new System.Drawing.Point(0, 271);
            this.txtUserInfo.Multiline = true;
            this.txtUserInfo.Name = "txtUserInfo";
            this.txtUserInfo.Size = new System.Drawing.Size(774, 93);
            this.txtUserInfo.TabIndex = 0;
            // 
            // BottomToolStripPanel
            // 
            this.BottomToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.BottomToolStripPanel.Name = "BottomToolStripPanel";
            this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.BottomToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // TopToolStripPanel
            // 
            this.TopToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.TopToolStripPanel.Name = "TopToolStripPanel";
            this.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.TopToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.TopToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // RightToolStripPanel
            // 
            this.RightToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.RightToolStripPanel.Name = "RightToolStripPanel";
            this.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.RightToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // LeftToolStripPanel
            // 
            this.LeftToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftToolStripPanel.Name = "LeftToolStripPanel";
            this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.LeftToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // ContentPanel
            // 
            this.ContentPanel.AutoScroll = true;
            this.ContentPanel.Size = new System.Drawing.Size(640, 273);
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.Panel);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(780, 370);
            this.mainPanel.TabIndex = 0;
            this.mainPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.mainPanel_Paint);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(504, 370);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // MainInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.pictureBox1);
            this.DoubleBuffered = true;
            this.Name = "MainInterface";
            this.Size = new System.Drawing.Size(780, 370);
            this.Panel.ResumeLayout(false);
            this.Panel.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Panel;
        private System.Windows.Forms.ToolStripPanel BottomToolStripPanel;
        private System.Windows.Forms.ToolStripPanel TopToolStripPanel;
        private System.Windows.Forms.ToolStripPanel RightToolStripPanel;
        private System.Windows.Forms.ToolStripPanel LeftToolStripPanel;
        private System.Windows.Forms.ToolStripContentPanel ContentPanel;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.TextBox txtUserInfo;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.Windows.Forms.CheckBox enableCoverArt;
        private System.Windows.Forms.RadioButton trackProgress;
        private System.Windows.Forms.RadioButton playlistProgress;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox displayArtistTrackName;
        private System.Windows.Forms.RadioButton noProgressTrack;

    }
}
