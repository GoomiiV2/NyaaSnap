using NyaaSnap.Uploaders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NyaaSnap
{
    public partial class NyaaSnapMain : Form
    {
        [DllImport("user32")]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

        // Get that Aero crap the hell out of here, kills capture times D:
        const uint DWM_EC_DISABLECOMPOSITION = 0;
        const uint DWM_EC_ENABLECOMPOSITION = 1;

        [DllImport("dwmapi.dll", EntryPoint = "DwmEnableComposition")]
        extern static uint DwmEnableComposition(uint compositionAction);

        private const int WINDOWPOSCHANGING = 0x0046;

        private long recordStartTime;
        private Point lastDims;
        private Point windowOffset = new Point(16, 70); // window padding to add to previewWindow
        private Point maxSize = new Point(1080, 720);

        private string saveAction = "";

        public static NyaaSnapMain self;
        private bool hasARecording = false;

        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPOS
        {
            public IntPtr hwnd;
            public IntPtr hwndInsertAfter;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public int flags;
        }

        public static UploadManager UpMan;

        public static ProgressWindow progressWindow;

        public NyaaSnapMain()
        {
            self = this;

            InitializeComponent();
            UpMan = new UploadManager();

            progressWindow = new ProgressWindow();

            saveAction = "Save to file";
            BTT_Save.Text = saveAction;
            CM_FileDests.Items.Add(saveAction);

            foreach (string ety in UpMan.GetUploaders())
                CM_FileDests.Items.Add(ety);

            SetCaptureDims();

            if (MessageBox.Show("Temporaly set the window style to basic? \nBasic will improve the capture preformance by a whole lot so please say yes.", "Enable Windows Aero Basic?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                DwmEnableComposition(DWM_EC_DISABLECOMPOSITION);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WINDOWPOSCHANGING)
            {
                WINDOWPOS mwp = (WINDOWPOS)Marshal.PtrToStructure(m.LParam, typeof(WINDOWPOS));

                if (ScreenCapture.IsRecording)
                {
                    ScreenCapture.CaptureX = mwp.x + windowOffset.X;
                    ScreenCapture.CaptureY = mwp.y + windowOffset.Y;

                    mwp.cx = ScreenCapture.CaptureWidth + windowOffset.X;
                    mwp.cy = ScreenCapture.CaptureHeight + windowOffset.Y;
                    Marshal.StructureToPtr(mwp, m.LParam, false);
                }
                else
                    Marshal.StructureToPtr(EnforceAspect(mwp), m.LParam, false);
            }
            base.WndProc(ref m);
        }

        private WINDOWPOS EnforceAspect(WINDOWPOS wpos)
        {
            int w = wpos.cx - windowOffset.X;
            int h = wpos.cy - windowOffset.Y;

            if (wpos.cx == lastDims.X && wpos.cy == lastDims.Y)
            {

            }
            else
            {
                wpos.cx = ((((w) / 16) + 1) * 16) + windowOffset.X;
                wpos.cy = ((((h) / 16) + 1) * 16) + windowOffset.Y;

                if (wpos.cx > maxSize.X)
                    wpos.cx = maxSize.X;

                if (wpos.cy > maxSize.Y)
                    wpos.cy = maxSize.Y;

                lastDims = new Point(wpos.cx, wpos.cy);
            }

            return wpos;
        }

        bool recToggle = false;
        private void Btt_Capture_Click(object sender, EventArgs e)
        {
            hasARecording = true;

            if (!recToggle)
                ScreenCapture.StartVP8Capture(20);
            else
                ScreenCapture.StopVP8Capture();

            Btt_Capture.Text = !recToggle ? "Stop!" : "Capture";
            PB_Rec.BackgroundImage = !recToggle ? NyaaSnap.Properties.Resources.RecordActive : NyaaSnap.Properties.Resources.RecordInactive;

            recordStartTime = UnixTime();

            RecTimer.Enabled = !recToggle;

            recToggle = !recToggle;
        }

        private Int32 UnixTime()
        {
            return (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        private void SetCaptureDims()
        {
            var point = this.PointToScreen(previewHole.Location);
            ScreenCapture.CaptureX = point.X;
            ScreenCapture.CaptureY = point.Y;
            ScreenCapture.CaptureWidth = previewHole.Bounds.Width;
            ScreenCapture.CaptureHeight = previewHole.Bounds.Height;
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            SetCaptureDims();
        }

        private void NyaaSnapMain_LocationChanged(object sender, EventArgs e)
        {
            SetCaptureDims();
        }

        private void NyaaSnapMain_Paint(object sender, PaintEventArgs e)
        {
            SetCaptureDims();
        }

        static int GCD(int a, int b)
        {
            return (b == 0) ? a : GCD(b, a % b);
        }

        private bool IsVaildAspect(int width, int height)
        {
            int gdc = GCD(width, height);

            if ((width / gdc == 16 && height / gdc == 9) && (width % 16 == 0 && height % 16 == 0))
                return true;
            else
                return false;
        }

        private void RecTimer_Tick(object sender, EventArgs e)
        {
            long seconds = UnixTime() - recordStartTime;
            TimeSpan t = TimeSpan.FromSeconds(seconds);

            LBL_TimeStamp.Text = string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);
        }

        private void BTT_Save_Click(object sender, EventArgs e)
        {
            /*ScreenCapture.BenchMarkCapture(SystemInformation.PrimaryMonitorSize.Width, SystemInformation.PrimaryMonitorSize.Height);
            return;*/

            if (!hasARecording)
            {
                MessageBox.Show("Umm, you need to record something first.");
                return;
            }
            else if (ScreenCapture.IsRecording)
            {
                MessageBox.Show("Can't save while recording D:");
                return;
            }
            else if (saveAction == "Save to file")
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Webm Video (*.webm)|*.webm";
                sfd.DefaultExt = ".webm";
                sfd.AddExtension = true;
                sfd.ShowDialog();

                ScreenCapture.SaveVP8Capture(sfd.FileName);
            }
            else
            {
                LBL_Uploading.Text = "Uploading...";
                ScreenCapture.UploadVP8Capture(UpMan, saveAction);
                LBL_Uploading.Text = "";
            }
        }

        private void CM_FileDests_Selected(object sender, ToolStripItemClickedEventArgs e)
        {
            saveAction = e.ClickedItem.Text;
            BTT_Save.Text = saveAction;
        }

        private void CM_FileDests_Opening(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        public static void onEncodeStart()
        {
            progressWindow.Show(true, "Encodeing...", "Encoding.........");
        }

        public static void onEncodeProgress(int current, int max)
        {
            progressWindow.SetPrecent(current, max);

            if (current >= max)
                progressWindow.Show(false, "", "");
        }
    }
}
