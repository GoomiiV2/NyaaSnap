using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace NyaaSnap
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                string assemblyName = new AssemblyName(args.Name).Name;
                byte[] data = null;

                if (assemblyName == ("VP8.NET"))
                {
                    data = NyaaSnap.Properties.Resources.VP8_NET;
                }
                else if (assemblyName == ("Newtonsoft.Json"))
                {
                    data = NyaaSnap.Properties.Resources.Newtonsoft_Json;
                }
                else
                    return null;

                string dllName = assemblyName + ".dll";
                string dllFullPath = Path.Combine(Path.GetTempPath(), dllName);

                Stream output = File.Create(dllFullPath, 4096, FileOptions.WriteThrough);
                output.Write(data, 0, data.Length);
                output.Flush();
                output.Close();

                return Assembly.LoadFrom(dllFullPath);
            };

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new NyaaSnapMain());
        }

        static void OnProcessExit(object sender, EventArgs e)
        {
            ScreenCapture.Flush();
        }
    }
}
