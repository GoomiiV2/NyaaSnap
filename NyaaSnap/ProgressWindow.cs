using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NyaaSnap
{
    public partial class ProgressWindow : Form
    {
        public ProgressWindow()
        {
            InitializeComponent();
        }

        private void ProgressWindow_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
            PB_Progress.Minimum = 0;
            PB_Progress.Maximum = 100;
        }

        delegate void ShowCallback(bool show, string title, string desc);
        public void Show(bool show, string title, string desc)
        {
            if (this.InvokeRequired)
            {
                ShowCallback d = new ShowCallback(Show);
                this.Invoke(d, new object[] { show, title, desc });
            }
            else
            {
                var loc = NyaaSnapMain.self.Location;
                var bounds = NyaaSnapMain.self.Bounds;
                var thisBounds = this.Bounds;
                this.Location = new Point(loc.X + ((bounds.Width / 2)) - (thisBounds.Width / 2),
                    loc.Y + ((bounds.Height / 2) - (thisBounds.Height / 2)));

                if (show)
                    this.Show();
                else
                    this.Hide();

                this.Text = title;
                this.LB_Desc.Text = desc;
            }
        }

        delegate void SetPrecentCallback(int current, int max);
        public void SetPrecent(int current, int max)
        {
            if (this.PB_Progress.InvokeRequired)
            {
                SetPrecentCallback d = new SetPrecentCallback(SetPrecent);
                this.Invoke(d, new object[] { current, max });
            }
            else
            {
                if (max == 0)
                    return;

                PB_Progress.Maximum = max;
                PB_Progress.Value = current;
            }
        }

    }
}
