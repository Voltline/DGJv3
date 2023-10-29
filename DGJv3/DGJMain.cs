using BilibiliDM_PluginFramework;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace DGJv3
{
    public class DGJMain : DMPlugin
    {
        private readonly DGJWindow window;

        public DGJMain()
        {
            try
            {
                var info = Directory.CreateDirectory(Utilities.BinDirectoryPath);
                info.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            }
            catch (Exception) { }
            AppDomain.CurrentDomain.AssemblyResolve += OnResolveAssembly;

            PluginName = "点歌姬(Modified By Voltline)";
            PluginVer = BuildInfo.Version;
            PluginDesc = "使用弹幕点播歌曲";
            PluginAuth = "Genteure(Modified By Voltline)";
            PluginCont = "Voltline@163.com";

            try
            {
                Directory.CreateDirectory(Utilities.DataDirectoryPath);
            }
            catch (Exception) { }
            window = new DGJWindow(this);
        }

        public override void Admin()
        {
            window.Show();
            window.Activate();
        }

        public override void DeInit() => window.DeInit();

        private static Assembly OnResolveAssembly(object sender, ResolveEventArgs args)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            AssemblyName assemblyName = new AssemblyName(args.Name);

            var path = assemblyName.Name + ".dll";
            string filepath = Path.Combine(Utilities.BinDirectoryPath, path);

            if (assemblyName.CultureInfo?.Equals(CultureInfo.InvariantCulture) == false)
            { path = string.Format(@"{0}\{1}", assemblyName.CultureInfo, path); }

            using (Stream stream = executingAssembly.GetManifestResourceStream(path))
            {
                if (stream == null) { return null; }

                var assemblyRawBytes = new byte[stream.Length];
                stream.Read(assemblyRawBytes, 0, assemblyRawBytes.Length);
                try
                {
                    File.WriteAllBytes(filepath, assemblyRawBytes);
                }
                catch (Exception) { }
            }

            return Assembly.LoadFrom(filepath);
        }
    }
}
