using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NyaaSnap.Uploaders
{
    public class UploadManager
    {
        public Dictionary<String, UploadBase> Uploaders;
        private UrlUploaded UploadedWindow;
        private Thread UploadThread;

        public UploadManager()
        {
            UploadedWindow = new UrlUploaded();
            UploadedWindow.Show("");
            UploadedWindow.Hide();

            Uploaders = new Dictionary<String, UploadBase>();

            // Uploaders
            Uploaders.Add("Pomf.se", new UP_Pomf());
        }

        public String[] GetUploaders()
        {
            return Uploaders.Keys.ToArray<String>();
        }

        public void Upload(string host, string filePath)
        {
            if (Uploaders.ContainsKey(host))
            {
                UploadThread = new Thread(() =>
                {
                    var url = Uploaders[host].Upload(filePath);
                    Clipboard.SetText(url);

                    UploadedWindow.BeginInvoke((MethodInvoker)delegate() 
                    {
                        UploadedWindow.Show(url);
                    });

                    NyaaSnapMain.progressWindow.Show(false, "", "");

                    File.Delete(filePath);
                });

                UploadThread.SetApartmentState(ApartmentState.STA);
                UploadThread.Start();

                NyaaSnapMain.progressWindow.Show(true, "Uploading....", "Uploading to " + host);
            }
        }
    }
}
