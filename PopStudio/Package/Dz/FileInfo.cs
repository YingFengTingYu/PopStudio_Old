namespace PopStudio.Package.Dz
{
    /// <summary>
    /// 描述DTRZ中每个文件的基本信息的类
    /// </summary>
    internal class FileInfo
    {
        public int offset;
        public int size;
        public int size2;
        public CompressFlags compressMethod = CompressFlags.STORE;
        public int zsize;

        public void Write(BinaryStream bs)
        {
            size2 = size;
            bs.WriteInt32(offset);
            bs.WriteInt32(size);
            bs.WriteInt32(size2);
            bs.WriteInt32((int)compressMethod);
        }

        /// <summary>
        /// 读取FileInfo信息
        /// </summary>
        /// <param name="bs"></param>
        public FileInfo Read(BinaryStream bs)
        {
            offset = bs.ReadInt32();
            size = bs.ReadInt32();
            size2 = bs.ReadInt32();
            int k = bs.ReadInt32();
            if ((k & (k - 1)) != 0)
            {
                k = 0b1 << ((int)Math.Floor(Math.Log2(k)));
            }
            compressMethod = (CompressFlags)k;
            return this;
        }

        /// <summary>
        /// 用于比较FileInfo的类
        /// </summary>
        public class CompareFileInfoByOffsetAndUp : IComparer<FileInfo>
        {
            public int Compare(FileInfo? x, FileInfo? y)
            {
                return (x?.offset ?? 0) - (y?.offset ?? 0);
            }
        }
    }
}