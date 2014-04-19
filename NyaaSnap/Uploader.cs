using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NyaaSnap
{
    public class Uploader
    {
        public delegate string HostURLReslover(FileHost_S self, string data);
        private UrlUploaded UploadedWindow;

        public struct FileHost_S
        {
            public string Name;
            public string UploadURL;
            public string RemoteURL;
            public string RemoteFieldName;
            public NameValueCollection Nvc;
            public int MaxFileSize;
            public HostURLReslover URLReslover;
        };

        public Dictionary<String, FileHost_S> Hosts;

        public Uploader()
        {
            UploadedWindow = new UrlUploaded();

            Hosts = new Dictionary<String, FileHost_S>();

            // Add the file hosts

            // Pomf.se
            var host = new FileHost_S();
            host.Name = "Pomf.se";
            host.UploadURL = "http://pomf.se/upload.php";
            host.RemoteURL = "http://a.pomf.se/";
            host.RemoteFieldName = "files[]";
            host.Nvc = new NameValueCollection();
            host.MaxFileSize = 1024 * 50;

            host.URLReslover = (FileHost_S self, string data) =>
            {
                dynamic json = JsonConvert.DeserializeObject(data);

                if ((bool)json.success)
                {
                    return self.RemoteURL + (string)json.files[0].url;
                }
                else
                    return null;
            };

            Hosts.Add(host.Name, host);
        }

        public void UploadFile(string filePath, string host)
        {
            if (!Hosts.ContainsKey(host))
                return;

            var h = Hosts[host];

            if (new FileInfo(filePath).Length >= h.MaxFileSize)
            {
                NameValueCollection nvc = new NameValueCollection();
                string result = HttpUploadFile(h.UploadURL, filePath, h.RemoteFieldName, "binary/octet-stream", h.Nvc);

                if (result != null)
                {
                    string url = h.URLReslover(h, result);
                    Clipboard.SetText(url);
                    UploadedWindow.Show(url);
                }
            }
            else
                MessageBox.Show("The file is too big for this host :<");
        }

        public static string HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc)
        {
            string result = null;

            Debug.WriteLine(string.Format("Uploading {0} to {1}", file, url));
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/32.0.1700.102 Safari/537.36";
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            Stream rs = wr.GetRequestStream();

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in nvc.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, nvc[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, paramName, file, contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse wresp = null;
            try
            {
                wresp = wr.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                result = reader2.ReadToEnd();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error uploading file", ex);
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
            }
            finally
            {
                wr = null;
            }

            return result;
        }
    }
}
