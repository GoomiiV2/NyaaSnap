namespace NyaaSnap
{
    partial class NyaaSnapMain
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NyaaSnapMain));
            this.Btt_Capture = new System.Windows.Forms.Button();
            this.previewHole = new System.Windows.Forms.Panel();
            this.LBL_TimeStamp = new System.Windows.Forms.Label();
            this.RecTimer = new System.Windows.Forms.Timer(this.components);
            this.PB_Rec = new System.Windows.Forms.PictureBox();
            this.CM_FileDests = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.LBL_Uploading = new System.Windows.Forms.Label();
            this.BTT_Save = new wyDay.Controls.SplitButton();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Rec)).BeginInit();
            this.SuspendLayout();
            // 
            // Btt_Capture
            // 
            this.Btt_Capture.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Btt_Capture.Location = new System.Drawing.Point(3, 3);
            this.Btt_Capture.Name = "Btt_Capture";
            this.Btt_Capture.Size = new System.Drawing.Size(75, 26);
            this.Btt_Capture.TabIndex = 0;
            this.Btt_Capture.Text = "Capture";
            this.Btt_Capture.UseVisualStyleBackColor = true;
            this.Btt_Capture.Click += new System.EventHandler(this.Btt_Capture_Click);
            // 
            // previewHole
            // 
            this.previewHole.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.previewHole.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.previewHole.Location = new System.Drawing.Point(0, 32);
            this.previewHole.Name = "previewHole";
            this.previewHole.Size = new System.Drawing.Size(640, 480);
            this.previewHole.TabIndex = 1;
            // 
            // LBL_TimeStamp
            // 
            this.LBL_TimeStamp.AutoSize = true;
            this.LBL_TimeStamp.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBL_TimeStamp.Location = new System.Drawing.Point(114, 3);
            this.LBL_TimeStamp.Name = "LBL_TimeStamp";
            this.LBL_TimeStamp.Size = new System.Drawing.Size(96, 26);
            this.LBL_TimeStamp.TabIndex = 3;
            this.LBL_TimeStamp.Text = "00:00:00";
            // 
            // RecTimer
            // 
            this.RecTimer.Interval = 1000;
            this.RecTimer.Tick += new System.EventHandler(this.RecTimer_Tick);
            // 
            // PB_Rec
            // 
            this.PB_Rec.BackgroundImage = global::NyaaSnap.Properties.Resources.RecordInactive;
            this.PB_Rec.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.PB_Rec.InitialImage = global::NyaaSnap.Properties.Resources.RecordInactive;
            this.PB_Rec.Location = new System.Drawing.Point(84, 3);
            this.PB_Rec.Name = "PB_Rec";
            this.PB_Rec.Size = new System.Drawing.Size(24, 26);
            this.PB_Rec.TabIndex = 2;
            this.PB_Rec.TabStop = false;
            // 
            // CM_FileDests
            // 
            this.CM_FileDests.Name = "CM_FileDesenations";
            this.CM_FileDests.Size = new System.Drawing.Size(61, 4);
            this.CM_FileDests.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.CM_FileDests_Selected);
            // 
            // LBL_Uploading
            // 
            this.LBL_Uploading.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LBL_Uploading.AutoSize = true;
            this.LBL_Uploading.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBL_Uploading.Location = new System.Drawing.Point(425, 6);
            this.LBL_Uploading.Name = "LBL_Uploading";
            this.LBL_Uploading.Size = new System.Drawing.Size(0, 18);
            this.LBL_Uploading.TabIndex = 6;
            this.LBL_Uploading.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BTT_Save
            // 
            this.BTT_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BTT_Save.AutoSize = true;
            this.BTT_Save.ContextMenuStrip = this.CM_FileDests;
            this.BTT_Save.Location = new System.Drawing.Point(517, 3);
            this.BTT_Save.Name = "BTT_Save";
            this.BTT_Save.Size = new System.Drawing.Size(121, 26);
            this.BTT_Save.SplitMenuStrip = this.CM_FileDests;
            this.BTT_Save.TabIndex = 5;
            this.BTT_Save.Text = "Save";
            this.BTT_Save.UseVisualStyleBackColor = true;
            this.BTT_Save.Click += new System.EventHandler(this.BTT_Save_Click);
            // 
            // NyaaSnapMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 512);
            this.Controls.Add(this.LBL_Uploading);
            this.Controls.Add(this.BTT_Save);
            this.Controls.Add(this.LBL_TimeStamp);
            this.Controls.Add(this.PB_Rec);
            this.Controls.Add(this.previewHole);
            this.Controls.Add(this.Btt_Capture);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "NyaaSnapMain";
            this.Text = "NyaaSnap";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.ResizeEnd += new System.EventHandler(this.Form1_ResizeEnd);
            this.LocationChanged += new System.EventHandler(this.NyaaSnapMain_LocationChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.NyaaSnapMain_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.PB_Rec)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Btt_Capture;
        private System.Windows.Forms.Panel previewHole;
        private System.Windows.Forms.PictureBox PB_Rec;
        private System.Windows.Forms.Label LBL_TimeStamp;
        private System.Windows.Forms.Timer RecTimer;
        private wyDay.Controls.SplitButton BTT_Save;
        private System.Windows.Forms.ContextMenuStrip CM_FileDests;
        private System.Windows.Forms.Label LBL_Uploading;
    }
}

