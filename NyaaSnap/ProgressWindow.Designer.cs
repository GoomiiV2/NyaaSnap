namespace NyaaSnap
{
    partial class ProgressWindow
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
            this.PB_Progress = new System.Windows.Forms.ProgressBar();
            this.LB_Desc = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // PB_Progress
            // 
            this.PB_Progress.Location = new System.Drawing.Point(1, 28);
            this.PB_Progress.Name = "PB_Progress";
            this.PB_Progress.Size = new System.Drawing.Size(417, 23);
            this.PB_Progress.TabIndex = 0;
            // 
            // LB_Desc
            // 
            this.LB_Desc.AutoSize = true;
            this.LB_Desc.Location = new System.Drawing.Point(12, 9);
            this.LB_Desc.Name = "LB_Desc";
            this.LB_Desc.Size = new System.Drawing.Size(35, 13);
            this.LB_Desc.TabIndex = 1;
            this.LB_Desc.Text = "label1";
            // 
            // ProgressWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 52);
            this.Controls.Add(this.LB_Desc);
            this.Controls.Add(this.PB_Progress);
            this.Name = "ProgressWindow";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.ProgressWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar PB_Progress;
        private System.Windows.Forms.Label LB_Desc;
    }
}