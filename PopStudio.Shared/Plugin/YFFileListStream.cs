namespace PopStudio.Plugin
{
    internal class YFFileListStream : IDisposable
    {
        public static YFFileListStream Singleten;

        public static void RegistPlatform(byte[] b) => _ = new YFFileListStream(b);

        public static byte[] GetFile(string str) => Singleten?[str];

        public static string[] GetStringList() => Singleten?.StringList;

        BinaryStream BaseStream;
        CIXFileInfo[] AllFileInfoList;
        Dictionary<string, int> StringIndexMap;
        string[] StringList;
        private bool disposedValue;

        public byte[] this[string name]
        {
            get
            {
                if (StringIndexMap.TryGetValue(name, out int index))
                {
                    CIXFileInfo info = AllFileInfoList[index];
                    BaseStream.Position = info.FilePos;
                    if ((info.Flags & 0b1) != 0)
                    {
                        //Deflate
                        using (DeflateStream ds = new DeflateStream(BaseStream, CompressionMode.Decompress, true))
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                ds.CopyTo(ms);
                                return ms.ToArray();
                            }
                        }
                    }
                    else
                    {
                        //Copy
                        return BaseStream.ReadBytes(info.FileSize);
                    }
                }
                return null;
            }
        }


        public YFFileListStream(byte[] s)
        {
            BaseStream = new BinaryStream(s);
            BaseStream.Position = 0;
            BaseStream.IdString("CIX\0");
            int fileCount = BaseStream.ReadInt32();
            BaseStream.ReadInt32(); //dummy
            BaseStream.ReadInt32(); //dummy
            AllFileInfoList = new CIXFileInfo[fileCount];
            StringIndexMap = new Dictionary<string, int>();
            StringList = new string[fileCount];
            for (int i = 0; i < fileCount; i++)
            {
                CIXFileInfo info = new CIXFileInfo();
                info.Name = BaseStream.GetStringByEmpty(BaseStream.ReadInt32());
                info.FilePos = BaseStream.ReadInt32();
                info.FileSize = BaseStream.ReadInt32();
                info.Flags = BaseStream.ReadInt32();
                AllFileInfoList[i] = info;
                StringIndexMap.Add(info.Name, i);
                StringList[i] = info.Name;
            }
            Singleten = this;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                }
                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                BaseStream.Dispose();
                // TODO: 将大型字段设置为 null
                BaseStream = null;
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~YFFileListStream()
        // {
        //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private struct CIXFileInfo
        {
            public string Name;
            public int FilePos;
            public int FileSize;
            public int Flags; //0b1 => Deflate
        }
    }
}
