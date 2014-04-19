using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NyaaSnap
{
    public partial class UrlUploaded : Form
    {
        Bitmap[] images = null;
        Random rand = new Random();

        public UrlUploaded()
        {
            InitializeComponent();

            images = new Bitmap[] 
            {
                NyaaSnap.Properties.Resources.Konata,
                NyaaSnap.Properties.Resources.Tsumiki,
                NyaaSnap.Properties.Resources.Yui,
                NyaaSnap.Properties.Resources.Yuuko,
                NyaaSnap.Properties.Resources.Ui
            };

            PBIcon.SizeMode = PictureBoxSizeMode.StretchImage;
            PBIcon.Image = images[rand.Next(images.Length)];

            label1.AutoSize = true;

            this.Hide();
        }

        public void Show(string url)
        {
            var loc = PointToScreen(NyaaSnapMain.self.Location);
            this.Location = new Point(loc.X + 50, loc.Y + 50);

            PBIcon.Image = images[rand.Next(images.Length)];
            this.Show();
            LBL_URL.Text = url;
        }

        private void BTT_OK_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void UrlUploaded_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void LBL_URL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(LBL_URL.Text);
        }
    }
}
