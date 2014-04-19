﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyaaSnap
{
    class VP8Encoder
    {
        private VP8.Encoder Encoder;
        private FileStream fs;
        private BinaryWriter bw;
        private int width = 0;
        private int height = 0;
        private int fps = 0;
        private int frameNum = 0;
        private int keyFrameEveryN = 90;
        private bool isEncoding = false;
        private string tempFilePath = "";

        public VP8Encoder()
        {

        }

        public void InitSession(int width, int height, int fps)
        {
            isEncoding = true;

            this.width = width;
            this.height = height;
            this.fps = fps;

            Encoder = new VP8.Encoder(width, height, fps);

            tempFilePath = Path.GetTempFileName();
            fs = File.Create(tempFilePath, 2048, FileOptions.SequentialScan);
            bw = new BinaryWriter(fs);

            WriteHeader();
        }

        public void WriteHeader()
        {
            bw.Write('D');
            bw.Write('K');
            bw.Write('I');
            bw.Write('F');
            bw.Write((short)0);
            bw.Write((short)32);
            bw.Write((int)0x30385056);
            bw.Write((short)width);
            bw.Write((short)height);
            bw.Write((int)fps);
            bw.Write((int)1);
            bw.Write((int)100);
            bw.Write((int)0);
        }

        public void WriteFrame(byte[] data)
        {
            bw.Write(data);
        }

        public void EncodeFrame(Bitmap frame)
        {
            if (!isEncoding)
                return;

            //if ((frameNum % keyFrameEveryN) == 0)
                //Encoder.ForceKeyframe();

            var data = Encoder.Encode(frame);
            bw.Write(data);

            frameNum++;
        }

        public void Stop()
        {
            isEncoding = false;

            bw.Seek(24, SeekOrigin.Begin);
            bw.Write((int)frameNum);
            bw.Flush();
            bw.Close();
            fs.Close();

            frameNum = 0;
        }

        private void EncodeToWebm(string filePath)
        {
            // Now convert to webm
            string mkvMergePath = Path.GetTempFileName() + ".exe";
            Stream output = File.OpenWrite(mkvMergePath);
            output.Write(NyaaSnap.Properties.Resources.mkvmerge, 0, NyaaSnap.Properties.Resources.mkvmerge.Length);
            output.Flush();
            output.Close();

            Process p = new Process();

            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            p.StartInfo.FileName = mkvMergePath;
            p.StartInfo.Arguments = "-o " + filePath + " " + tempFilePath;
            p.Start();
            p.WaitForExit();

            File.Delete(mkvMergePath);
            File.Delete(mkvMergePath.Substring(mkvMergePath.Length - 4));
        }

        public void Save(string filePath)
        {
            EncodeToWebm(filePath);
        }

        public void Upload(Uploader uper, string host)
        {
            string webm = Path.GetTempFileName();
            File.Delete(webm);
            webm += ".webm";

            EncodeToWebm(webm);

            uper.UploadFile(webm, host);

            File.Delete(webm);
        }

        public void Flush()
        {
            if (File.Exists(tempFilePath))
                File.Delete(tempFilePath);
        }
    }
}
