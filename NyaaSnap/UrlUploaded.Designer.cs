namespace NyaaSnap
{
    partial class UrlUploaded
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UrlUploaded));
            this.PBIcon = new System.Windows.Forms.PictureBox();
            this.BTT_OK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.LBL_URL = new System.Windows.Forms.LinkLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.PBIcon)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PBIcon
            // 
            this.PBIcon.Location = new System.Drawing.Point(0, 0);
            this.PBIcon.Name = "PBIcon";
            this.PBIcon.Size = new System.Drawing.Size(210, 251);
            this.PBIcon.TabIndex = 0;
            this.PBIcon.TabStop = false;
            // 
            // BTT_OK
            // 
            this.BTT_OK.Location = new System.Drawing.Point(216, 207);
            this.BTT_OK.Name = "BTT_OK";
            this.BTT_OK.Size = new System.Drawing.Size(269, 37);
            this.BTT_OK.TabIndex = 1;
            this.BTT_OK.Text = "OK";
            this.BTT_OK.UseVisualStyleBackColor = true;
            this.BTT_OK.Click += new System.EventHandler(this.BTT_OK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(216, 9);
            this.label1.MaximumSize = new System.Drawing.Size(280, 300);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(278, 72);
            this.label1.TabIndex = 2;
            this.label1.Text = "Yay! \r\n\r\nYour video has been uploaded and the url has been copyied to your clipbo" +
    "ard.";
            // 
            // LBL_URL
            // 
            this.LBL_URL.AutoSize = true;
            this.LBL_URL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LBL_URL.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBL_URL.Location = new System.Drawing.Point(3, 16);
            this.LBL_URL.Name = "LBL_URL";
            this.LBL_URL.Size = new System.Drawing.Size(132, 18);
            this.LBL_URL.TabIndex = 4;
            this.LBL_URL.TabStop = true;
            this.LBL_URL.Text = "                               ";
            this.LBL_URL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LBL_URL_LinkClicked);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.LBL_URL);
            this.groupBox1.Location = new System.Drawing.Point(219, 161);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(266, 40);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "URL: ";
            // 
            // UrlUploaded
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 249);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BTT_OK);
            this.Controls.Add(this.PBIcon);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UrlUploaded";
            this.ShowInTaskbar = false;
            this.Text = "Yayifications!";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UrlUploaded_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.PBIcon)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox PBIcon;
        private System.Windows.Forms.Button BTT_OK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel LBL_URL;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}