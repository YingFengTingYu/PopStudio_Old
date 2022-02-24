namespace PopStudio.P_Package.Dz
{
    /// <summary>
    /// 描述DTRZ中文件和文件夹的对应关系的类
    /// </summary>
    internal class MatchInfo
    {
        public ushort fileIndex;
        public ushort fileIndexForFileInfo = 0xFFFF;
        public ushort folderIndex;

        public void Write(BinaryStream bs)
        {
            if (fileIndexForFileInfo == 0xFFFF)
            {
                fileIndexForFileInfo = fileIndex;
            }
            bs.WriteUInt16(folderIndex);
            bs.WriteUInt16(fileIndexForFileInfo);
            bs.WriteUInt16(0xFFFF);
        }

        /// <summary>
        /// 读取MatchInfo信息
        /// </summary>
        /// <param name="bs"></param>
        public MatchInfo Read(BinaryStream bs)
        {
            folderIndex = bs.ReadUInt16();
            fileIndexForFileInfo = bs.ReadUInt16();
            while (true)
            {
                if (bs.ReadUInt16() == 0xFFFF)
                {
                    break;
                }
            }
            return this;
        }
    }
}
