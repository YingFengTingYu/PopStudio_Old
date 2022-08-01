namespace PopStudio.Package.Dz
{
    internal class ChunkInfo
    {
        public ushort FolderNameIndex = 0;
        public ushort FileNameIndex = 0;
        public ushort ChunkIndex = 0;
        public int Offset = 0;
        public int ZSize_For_Dz = 0;
        public int Size = 0;
        public CompressFlags Flags = CompressFlags.STORE;
        public int ZSize_For_Compress = 0;
        public int MultiIndex = 0;
        public ushort ArchiveIndex = 0; //when ArchivesCount > 1

        public ChunkInfo(ushort folderNameIndex, ushort fileNameIndex, ushort chunkIndex, int multiIndex = 0)
        {
            FolderNameIndex = folderNameIndex;
            FileNameIndex = fileNameIndex;
            ChunkIndex = chunkIndex;
            MultiIndex = multiIndex;
        }

        public void ReadInfo(BinaryStream bs)
        {
            Offset = bs.ReadInt32();
            ZSize_For_Dz = bs.ReadInt32();
            Size = bs.ReadInt32();
            Flags = (CompressFlags)bs.ReadUInt16();
            ArchiveIndex = bs.ReadUInt16();
        }

        public void WriteInfo(BinaryStream bs)
        {
            bs.WriteInt32(Offset);
            bs.WriteInt32(ZSize_For_Dz);
            bs.WriteInt32(Size);
            bs.WriteUInt16((ushort)Flags);
            bs.WriteUInt16(ArchiveIndex);
        }
    }
}
