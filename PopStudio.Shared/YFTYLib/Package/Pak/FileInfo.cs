namespace PopStudio.Package.Pak
{
    internal class FileInfo
    {
        public string fileName;
        public int zsize;
        public int size;
        public long fileTime = 129146222018596744;

        public void Write(BinaryStream bs, bool? compress)
        {
            bs.WriteStringByUInt8Head(fileName);
            bs.WriteInt32(zsize);
            if (compress == true)
            {
                bs.WriteInt32(size);
            }
            bs.WriteInt64(fileTime);
        }

        /// <summary>
        /// 读取FileInfo信息
        /// </summary>
        /// <param name="bs"></param>
        public FileInfo Read(BinaryStream bs, bool? compress)
        {
            fileName = bs.ReadStringByUInt8Head();
            zsize = bs.ReadInt32();
            if (compress == true)
            {
                size = bs.ReadInt32();
            }
            fileTime = bs.ReadInt64();
            return this;
        }
    }
}
