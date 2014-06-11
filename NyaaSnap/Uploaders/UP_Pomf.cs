using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NyaaSnap.Uploaders
{
    class UP_Pomf : UploadBase
    {
        private string Name = "Pomf.se";
        private string UploadURL = "http://pomf.se/upload.php";
        private string RemoteURL = "http://a.pomf.se/";
        private string RemoteFieldName = "files[]";
        private int MaxFileSize = 1024 * 50;

        public override string Upload(string filePath)
        {
            if (new FileInfo(filePath).Length >= MaxFileSize)
            {
                NameValueCollection nvc = new NameValueCollection();
                string result = HttpUploadFile(UploadURL, filePath, RemoteFieldName, "binary/octet-stream", nvc);

                // Get the url
                if (result != null)
                {
                    dynamic json = JsonConvert.DeserializeObject(result);

                    if ((bool)json.success)
                    {
                        return RemoteURL + (string)json.files[0].url;
                    }
                    else
                        return null;
                }
            }
            else
                MessageBox.Show("The file is too big for this host :<");

            return null;
        }
    }
}
