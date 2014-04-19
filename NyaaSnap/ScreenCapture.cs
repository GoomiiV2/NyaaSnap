using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace NyaaSnap
{
    class ScreenCapture
    {
        public static int CaptureX = 0;
        public static int CaptureY = 0;
        public static int CaptureWidth = 0;
        public static int CaptureHeight = 0;
        public static bool IsRecording = false;

        private static VP8Encoder Encoder = new VP8Encoder();
        private static Thread EncodeThread;
        private static Thread CaptureThread;

        // TODO: Maybe I can use threads to capture differn't sections of the screen and not have it blow up in my face?
        public static Bitmap CaptureScreen()
        {
            try
            {
                Bitmap bmpScreenCapture = new Bitmap(CaptureWidth, CaptureHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                using (Graphics g = Graphics.FromImage(bmpScreenCapture))
                {
                    g.CopyFromScreen(CaptureX, CaptureY, 0, 0, bmpScreenCapture.Size, CopyPixelOperation.SourceCopy);
                    return bmpScreenCapture;
                }
            }
            catch (Exception e)
            {
                // This can't possabley hurt, right, right!?
                return CaptureScreen();
            }
        }

        private static Int32 UnixTime()
        {
            return (Int32)(DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
        }

        //static System.Timers.Timer aTimer;
        public static void StartVP8Capture(int fps)
        {
            Encoder.Flush();

            IsRecording = true;

            if (EncodeThread != null)
            {
                EncodeThread.Abort();
                EncodeThread = null;
            }

            if (CaptureThread != null)
            {
                CaptureThread.Abort();
                CaptureThread = null;
            }

            bool newCapture = false;
            Bitmap capture = null;

            CaptureThread = new Thread(() =>
            {
                try
                {
                    var watch = Stopwatch.StartNew();
                    while (true)
                    {
                        watch.Restart();

                        capture = CaptureScreen();
                        newCapture = true;

                        watch.Stop();
                        var elapsedMs = watch.ElapsedMilliseconds;

                        if (elapsedMs < (1000 / fps))
                        {
                            Thread.Sleep((int)((1000 / fps) - elapsedMs));
                        }
                        else
                        {
                            Debug.WriteLine("Capture is too slow D:");
                        }
                    }
                }
                catch (ThreadAbortException e)
                {
                       
                }
            });

            EncodeThread = new Thread(() =>
            {
                CaptureThread.Start();

                try
                {
                    Encoder.InitSession(CaptureWidth, CaptureHeight, fps);

                    var watch = Stopwatch.StartNew();
                    while (true)
                    {
                        if (newCapture)
                        {
                            Encoder.EncodeFrame(capture);
                            newCapture = false;
                        }
                    }
                }
                catch (ThreadAbortException e)
                {

                }
            });

            EncodeThread.Start();

            /*Encoder.InitSession(CaptureWidth, CaptureHeight, fps, filePath);

            aTimer = new System.Timers.Timer(1000 / fps);

            // Hook up the Elapsed event for the timer.
            aTimer.Elapsed += new ElapsedEventHandler(VP8CaptureFrame);

            aTimer.Enabled = true;*/
        }

        private static void VP8CaptureFrame()
        {
            var frame = CaptureScreen();
            Encoder.EncodeFrame(frame);
        }

        public static void StopVP8Capture()
        {
            EncodeThread.Abort();
            CaptureThread.Abort();
            IsRecording = false;
            Encoder.Stop();
        }

        public static void SaveVP8Capture(string filePath)
        {
            Encoder.Save(filePath);
        }

        public static void UploadVP8Capture(Uploader uper, string host)
        {
            Encoder.Upload(uper, host);
        }

        public static void Flush()
        {
            Encoder.Flush();
        }
    }
}
