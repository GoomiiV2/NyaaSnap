using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NyaaSnap
{
    // Multi threaded is a no go :<
    public struct CaptureThreadData
    {
        public CaptureThreadData(Point start, Size a, int sTime)
        {
            StartPos = start;
            Area = a;
            SleepTime = sTime;
        }

        public Point StartPos;
        public Size Area;
        public int SleepTime;
    };

    public class MultiThreadedScreenCaper
    {
        //private Bitmap ScreenSnap;
        //private Graphics gfx;
        private Thread[] CaptureThreads;
        private Bitmap[] ScreenShots;
        private Graphics[] gfx;

        public MultiThreadedScreenCaper(int width, int height, int numThreads)
        {
            //ScreenSnap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            //gfx = Graphics.FromImage(ScreenSnap);

            CaptureThreads = new Thread[numThreads];
            ScreenShots = new Bitmap[numThreads];
            gfx = new Graphics[numThreads];

            int h = height / numThreads;

            for (int i = 0; i < numThreads; i++)
            {
                ScreenShots[i] = new Bitmap(width, h, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                gfx[i] = Graphics.FromImage(ScreenShots[i]);
                CaptureThreads[i] = new Thread(ThreadWorker);
                CaptureThreads[i].Start(new CaptureThreadData(new Point(0, h*i), new Size(width, h), i));
            }
        }

        private void ThreadWorker(object data)
        {
            CaptureThreadData StartInfo = (CaptureThreadData)data;
            var watch = Stopwatch.StartNew();

            Debug.WriteLine("StartInfo: " + StartInfo.StartPos.ToString());

            long total = 0;
            int loops = 0;
            while (true)
            {
                watch.Restart();
                CaptureScreenArea(gfx[StartInfo.SleepTime], StartInfo.StartPos, StartInfo.StartPos, StartInfo.Area);
                watch.Stop();
                long elapsedMs = watch.ElapsedMilliseconds;

                total += elapsedMs;
                loops++;

                if (loops > 100)
                {
                    Debug.WriteLine("Capture Time Avg over (" + loops + "): " + total/loops);

                    total = 0;
                    loops = 0;
                }

                if (elapsedMs < StartInfo.SleepTime)
                {
                    //Thread.Sleep((int)(StartInfo.SleepTime - elapsedMs));
                }
            }
        }

        private void CaptureScreenArea(Graphics g, Point srcPos, Point destPos, Size area)
        {
            try
            {
                //lock (ScreenSnap)
                    g.CopyFromScreen(srcPos.X, srcPos.Y, destPos.X, destPos.Y, area, CopyPixelOperation.SourceCopy);

            }
            catch (Exception e)
            {

            }
        }
    }


}
