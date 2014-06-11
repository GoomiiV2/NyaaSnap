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

        public delegate void closeCallback();
        private closeCallback callback = null;

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
            var loc = NyaaSnapMain.self.Location;
            var bounds = NyaaSnapMain.self.Bounds;
            var thisBounds = this.Bounds;
            this.Location = new Point(loc.X + ((bounds.Width / 2)) - (thisBounds.Width / 2),
                loc.Y + ((bounds.Height / 2) - (thisBounds.Height / 2)));

            PBIcon.Image = images[rand.Next(images.Length)];
            this.Show();
            LBL_URL.Text = url;
        }

        public void Show(string url, closeCallback cb)
        {
            this.Show(url);
            callback = cb;
        }

        private void BTT_OK_Click(object sender, EventArgs e)
        {
            this.Hide();

            if (callback != null)
                callback();
        }

        private void UrlUploaded_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();

            if (callback != null)
                callback();
        }

        private void LBL_URL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(LBL_URL.Text);
        }
    }
}
