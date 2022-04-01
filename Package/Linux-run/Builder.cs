using SharpCompress.Writers;
using SharpCompress.Common;

namespace Package.Linux_run
{
    internal static class Builder
    {
        public static bool Build(string inFolder)
        {
            bool ans = false;
            string father = "PopStudio";
            while (Directory.Exists(father) || File.Exists(father)) father += "1";
            try
            {
                string inFile = father + Path.DirectorySeparatorChar + "PopStudio";
                Plugin.Dir.NewDir(inFile);
                //Info
                string InfoPath = inFile + Path.DirectorySeparatorChar + "Info";
                Plugin.Dir.NewDir(InfoPath);
                using (Stream stream = File.Open(InfoPath + Path.DirectorySeparatorChar + "popstudio_icon.png", FileMode.Create))
                {
                    stream.Write(Res.Icon, 0, Res.Icon.Length);
                }
                using (Stream stream = File.Open(InfoPath + Path.DirectorySeparatorChar + "popstudio.desktop", FileMode.Create))
                {
                    byte[] desktopbytes = System.Text.Encoding.UTF8.GetBytes(Install.Desktop);
                    stream.Write(desktopbytes, 0, desktopbytes.Length);
                }
                //Main
                string MainPath = inFile + Path.DirectorySeparatorChar + "Main";
                Plugin.Dir.NewDir(MainPath);
                CopyFolder(inFolder, MainPath);
                if (File.Exists(MainPath + Path.DirectorySeparatorChar + "PopStudio.GTK"))
                {
                    File.Move(MainPath + Path.DirectorySeparatorChar + "PopStudio.GTK", MainPath + Path.DirectorySeparatorChar + "PopStudio");
                }
                using (Stream stream = File.Open(Path.GetDirectoryName(inFolder) + Path.DirectorySeparatorChar + "PopStudio.run", FileMode.Create, FileAccess.ReadWrite))
                {
                    //WriteHead
                    byte[] scriptbytes = System.Text.Encoding.UTF8.GetBytes(Install.Script);
                    stream.SetLength(scriptbytes.Length);
                    stream.Write(scriptbytes, 0, scriptbytes.Length);
                    //WriteTar
                    using (IWriter tar = WriterFactory.Open(stream, ArchiveType.Tar, new WriterOptions(CompressionType.BZip2)))
                    {
                        tar.WriteAll(father, "*", SearchOption.AllDirectories);
                    }
                }
                ans = true;
            }
            catch (Exception)
            {

            }
            finally
            {
                Directory.Delete(father, true);
            }
            return ans;
        }

        public static void CopyFolder(string inFolder, string outFolder)
        {
            inFolder = inFolder.StartsWith('\"') ? inFolder[1..] : inFolder;
            inFolder = inFolder.EndsWith('\"') ? inFolder[..^1] : inFolder;
            string[] dest = Plugin.Dir.GetFiles(inFolder);
            int l = inFolder.Length;
            foreach (string s in dest)
            {
                string newpath = outFolder + Path.DirectorySeparatorChar + s[l..];
                Plugin.Dir.NewDir(newpath, false);
                File.Copy(s, newpath, true);
            }
        }
    }
}