namespace Package.Plugin
{
    public class Dir
    {
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
            if (!toEnd) pthName = pthName[..pthName.LastIndexOf(Path.DirectorySeparatorChar)];
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
    }
}