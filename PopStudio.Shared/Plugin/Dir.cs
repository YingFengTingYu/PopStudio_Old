namespace PopStudio.Plugin
{
    public class Dir
    {
        public static void FormatAndDeleteEndPathSeparator(ref string str)
        {
            str = FormatPath(str);
            char t = str[str.Length - 1];
            if (t == '/' || t == '\\') str = str[0..^1];
        }

        public static void DeleteEndPathSeparator(ref string str)
        {
            char t = str[^1];
            if (t == '/' || t == '\\') str = str[0..^1];
        }
        /// <summary>
        /// 新建文件夹，若上级目录不存在则会新建上级目录
        /// </summary>
        /// <param name="pthName"></param>
        public static void NewDir(string pthName)
        {
            if (!Directory.Exists(pthName))
            {
                Directory.CreateDirectory(pthName);
            }
        }
        /// <summary>
        /// 新建文件夹，若上级目录不存在则会新建上级目录
        /// </summary>
        /// <param name="pthName"></param>
        /// <param name="toEnd"></param>
        public static void NewDir(string pthName, bool toEnd)
        {
            if (!toEnd) pthName = pthName[..pthName.LastIndexOf(Const.PATHSEPARATOR)];
            if (!Directory.Exists(pthName))
            {
                Directory.CreateDirectory(pthName);
            }
        }

        static string[] fileNameLib;
        static int fileNum;
        /// <summary>
        /// 获取一个目录下的所有文件，包含子目录里的文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string[] GetFiles(string path)
        {
            fileNameLib = new string[10000];
            fileNum = 0;
            GetFile(path);
            string[] ansLib = new string[fileNum];
            for (int i = 0; i < fileNum; i++)
            {
                ansLib[i] = fileNameLib[i];
                fileNameLib[i] = null;
            }
            fileNameLib = null;
            fileNum = 0;
            return ansLib;
        }
        /// <summary>
        /// 递归获取子目录下文件
        /// </summary>
        /// <param name="path"></param>
        static void GetFile(string path)
        {
            if (fileNameLib == null) return;
            string[] p = Directory.GetDirectories(path);
            for (int i = 0; i < p.Length; i++)
            {
                GetFile(p[i]);
            }
            p = Directory.GetFiles(path);
            for (int i = 0; i < p.Length; i++)
            {
                fileNameLib[fileNum++] = p[i];
            }
        }
        /// <summary>
        /// 按照文件系统方式格式化文件目录
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string FormatPath(string filePath)
        {
            return filePath.Replace(Const.UNUSEDPATHSEPARATOR, Const.PATHSEPARATOR).Replace(Const.DOUBLEPATHSEPARATOR, Const.PATHSEPARATOR).Replace(Const.EMPTYPATHSEPARATOR, Const.PATHSEPARATOR);
        }
        /// <summary>
        /// 按照Windows文件系统格式化文件目录
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string FormatWindowsPath(string filePath)
        {
            return filePath.Replace('/', '\\').Replace(@"\\", @"\").Replace(@" \", @"\");
        }
        /// <summary>
        /// 按照Linux文件系统格式化文件目录
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string FormatLinuxPath(string filePath)
        {
            return filePath.Replace('\\', '/').Replace("//", "/").Replace(" /", "/");
        }
    }
}