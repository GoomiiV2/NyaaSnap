using NyaaSnap.Uploaders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace NyaaSnap
{
    class ScreenCapture
    {
        [DllImport("gdi32.dll", EntryPoint = "DeleteDC")]
        static extern bool DeleteDC([In] IntPtr hdc);

        [DllImport("kernel32.dll", EntryPoint = "CopyMemory")]
        static extern void CopyMemory(IntPtr destination, IntPtr source, uint length);

        public static int CaptureX = 0;
        public static int CaptureY = 0;
        public static int CaptureWidth = 0;
        public static int CaptureHeight = 0;
        public static bool IsRecording = false;

        private static VP8Encoder Encoder = new VP8Encoder();
        private const int maxEncodeThreads = 1;
        private static Thread[] EncodeThreads;
        private static int EncodeThreadsFinished = 0;
        private static Thread CaptureThread;
        private static Object CaptureLock = new Object();

        private static Bitmap Capture;
        private static Queue<Bitmap> CaptureQueue;
        private static Graphics GfxContext;

        // TODO: Maybe I can use threads to capture differn't sections of the screen and not have it blow up in my face?
        public static void CaptureScreen()
        {
            try
            {
                lock (GfxContext)
                {
                    GfxContext.CopyFromScreen(CaptureX, CaptureY, 0, 0, Capture.Size, CopyPixelOperation.SourceCopy);

                    return;
                    using (Bitmap bmp = new Bitmap(CaptureWidth, CaptureHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb))
                    {
                        using (Graphics g = Graphics.FromImage(bmp))
                        {
                            g.CopyFromScreen(CaptureX, CaptureY, 0, 0, bmp.Size, CopyPixelOperation.SourceCopy);
                        }

                        /*var temp = bmp.LockBits(new Rectangle(0, 0, CaptureWidth, CaptureHeight), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                        var temp2 = Capture.LockBits(new Rectangle(0, 0, CaptureWidth, CaptureHeight), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
                        uint size = (uint)temp.Stride;
                        size = size * (uint)temp.Height;
                        CopyMemory(temp2.Scan0, temp.Scan0, size);
                        bmp.UnlockBits(temp);
                        Capture.UnlockBits(temp2);*/
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("CaptureScreen Error: " + e.ToString());
                IsRecording = false;
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

            Capture = new Bitmap(CaptureWidth, CaptureHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            GfxContext = Graphics.FromImage(Capture);
            CaptureQueue = new Queue<Bitmap>(500);

            IsRecording = true;

            CaptureThread = new Thread(() =>
            {
                Encoder.InitSession(CaptureWidth, CaptureHeight, fps);
                Stopwatch watch = Stopwatch.StartNew();
                long loops = 100;
                long elapsed = 0;
                long total = 0;
                byte[] capPixels = new byte[(int)((CaptureWidth*3) * CaptureHeight)];
                Bitmap temp;

                try
                {
                    while (IsRecording)
                    {
                        watch.Restart();
                        CaptureScreen();
                        lock (GfxContext)
                        {
                            temp = (Bitmap)Capture.Clone(new Rectangle(0, 0, CaptureWidth, CaptureHeight), PixelFormat.Format24bppRgb);
                            //lock (CaptureQueue)
                            {
                                CaptureQueue.Enqueue(temp);
                            }
                        }

                        watch.Stop();
                        elapsed = watch.ElapsedMilliseconds;
                        total += elapsed;

                        loops++;
                        if (loops >= 100)
                        {
                            Debug.WriteLine("Average Capture Time: " + total / loops);
                            loops = total = 0;
                        }

                        if (elapsed < (1000 / fps))
                            Thread.Sleep((int)((1000 / fps) - elapsed));
                        else
                        {
                            Debug.WriteLine("Too slow");
                        }
                    }
                }
                catch (ThreadAbortException e)
                {
                       
                }
            });

            CaptureThread.Start();

            EncodeThreads = new Thread[maxEncodeThreads];
            EncodeThreadsFinished = 0;

            for (int i = 0; i < maxEncodeThreads; i++)
            {
                EncodeThreads[i] = new Thread(EncodeWorker);
                EncodeThreads[i].Start();
            }
        }

        private static void EncodeWorker()
        {
            Stopwatch watch = Stopwatch.StartNew();
            long loops = 0;
            long total = 0;
            bool hasStuffToEncode = false;
            Bitmap temp = null;
            int totalFrames = 0;

            try
            {
                while (true)
                {
                    // If encoding and not recording show the progress
                    if (totalFrames == 0 && !IsRecording && CaptureQueue.Count > 0)
                    {
                        NyaaSnapMain.onEncodeStart();
                        totalFrames = CaptureQueue.Count;
                    }

                    // Get a frame to encode
                    lock (CaptureQueue)
                    {
                        if (CaptureQueue.Count > 0)
                            temp = CaptureQueue.Dequeue(); //.Clone(new Rectangle(0, 0, CaptureWidth, CaptureHeight), PixelFormat.Format24bppRgb);
                    }

                    // Encode
                    if (temp != null)
                    {
                        watch.Restart();

                        Encoder.EncodeFrame(temp);

                        temp.Dispose();
                        temp = null;

                        watch.Stop();
                        total += watch.ElapsedMilliseconds;
                    }
                    else
                        Thread.Sleep(5);

                    loops++;
                    if (loops >= 100)
                    {
                        Debug.WriteLine("Average Encode Time: " + total / loops);
                        loops = total = 0;
                    }

                    // Update the progress
                    if (!IsRecording && CaptureQueue.Count != 0)
                        NyaaSnapMain.onEncodeProgress(CaptureQueue.Count, totalFrames);

                    if ((!IsRecording && CaptureQueue.Count == 0))
                    {
                        NyaaSnapMain.onEncodeProgress(0, 100);
                        break;
                    }
                }

                Debug.WriteLine("Encode thread finished!");

                EncodeThreadsFinished++;

                if (EncodeThreadsFinished >= maxEncodeThreads)
                {
                    Debug.WriteLine("All encoding threads have stoped.");
                    Encoder.Stop();
                }
            }
            catch (ThreadAbortException e)
            {

            }
        }

        public static void StopVP8Capture()
        {
            IsRecording = false;
        }

        public static void SaveVP8Capture(string filePath)
        {
            Encoder.Save(filePath);
        }

        public static void UploadVP8Capture(UploadManager uper, string host)
        {
            Encoder.Upload(uper, host);
        }

        public static void Flush()
        {
            Encoder.Flush();
        }

        public static void BenchMarkCapture(int width, int height)
        {
            Stopwatch watch = Stopwatch.StartNew();
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            Graphics gfx = Graphics.FromImage(bmp);
            int loops = 100;
            long total = 0;

            for (int i = 0; i <= loops; i++)
            {
                watch.Restart();
                gfx.CopyFromScreen(0, 0, 0, 0, bmp.Size, CopyPixelOperation.SourceCopy);
                watch.Stop();
                total += watch.ElapsedMilliseconds;
            }

            MessageBox.Show("Average capture time: " + total / loops);
        }
    }
}
