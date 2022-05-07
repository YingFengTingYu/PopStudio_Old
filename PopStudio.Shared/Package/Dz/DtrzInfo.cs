namespace PopStudio.Package.Dz
{
    internal class DtrzInfo
    {
        public static readonly string Magic = "DTRZ";
        public static readonly byte Version = 0;

        public ushort FileNameNumber = 0;
        public ushort FolderNameNumber = 0;
        public string[] FileNameLibrary = null;
        public string[] FolderNameLibrary = null;
        public ChunkInfo[] Chunks = null;

        public void WritePart1(BinaryStream bs)
        {
            //初始化
            FileNameNumber = (ushort)FileNameLibrary.Length;
            FolderNameNumber = (ushort)FolderNameLibrary.Length;
            ////写入
            bs.WriteString(Magic);
            bs.WriteUInt16(FileNameNumber);
            bs.WriteUInt16(FolderNameNumber);
            bs.WriteByte(Version);
            for (int i = 0; i < FileNameNumber; i++)
            {
                bs.WriteStringByEmpty(FileNameLibrary[i]);
            }
            for (int i = 1; i < FolderNameNumber; i++) //0是空
            {
                bs.WriteStringByEmpty(FolderNameLibrary[i]);
            }
            int matchCount = Chunks.Length;
            Array.Sort(Chunks, (a, b) => a.FileNameIndex - b.FileNameIndex);
            for (int i = 0; i < matchCount; i++)
            {
                bs.WriteUInt16(Chunks[i].FolderNameIndex);
                bs.WriteUInt16(Chunks[i].ChunkIndex);
                bs.WriteUInt16(0xFFFF);
            }
            bs.WriteUInt16(1);
            bs.WriteUInt16((ushort)matchCount);
        }

        public void WritePart2(BinaryStream bs)
        {
            int matchCount = Chunks.Length;
            for (int i = 0; i < matchCount; i++)
            {
                Chunks[i].WriteInfo(bs);
            }
        }

        public void Read(BinaryStream bs)
        {
            bs.IdString(Magic);
            FileNameNumber = bs.ReadUInt16();
            FolderNameNumber = bs.ReadUInt16();
            bs.IdByte(Version);
            FileNameLibrary = new string[FileNameNumber];
            for (ushort i = 0; i < FileNameNumber; i++)
            {
                FileNameLibrary[i] = bs.ReadStringByEmpty();
            }
            FolderNameLibrary = new string[FolderNameNumber];
            FolderNameLibrary[0] = string.Empty;
            for (ushort i = 1; i < FolderNameNumber; i++)
            {
                FolderNameLibrary[i] = bs.ReadStringByEmpty();
            }
            List<ChunkInfo> tempChunks = new List<ChunkInfo>();
            int chunksCount = 0;
            for (ushort i = 0; i < FileNameNumber; i++)
            {
                ushort FolderIndex = bs.ReadUInt16();
                ushort Next;
                int index = 0;
                while ((Next = bs.ReadUInt16()) != 0xFFFF)
                {
                    tempChunks.Add(new ChunkInfo(FolderIndex, i, Next, index++));
                    if (Next > chunksCount) chunksCount = Next;
                }
            }
            chunksCount++;
            Chunks = new ChunkInfo[chunksCount];
            foreach (ChunkInfo item in tempChunks)
            {
                Chunks[item.ChunkIndex] = item;
            }
            bs.IdUInt16(1);
            bs.IdUInt16((ushort)chunksCount);
            for (int i = 0; i < chunksCount; i++)
            {
                Chunks[i].ReadInfo(bs);
            }
            tempChunks.Sort((a, b) => a.Offset - b.Offset);
            chunksCount--;
            for (int i = 0; i < chunksCount; i++)
            {
                tempChunks[i].ZSize_For_Compress = tempChunks[i + 1].Offset - tempChunks[i].Offset;
            }
            if ((tempChunks[chunksCount].Flags & CompressFlags.STORE) != 0)
            {
                tempChunks[chunksCount].ZSize_For_Compress = tempChunks[chunksCount].Size;
            }
            else
            {
                tempChunks[chunksCount].ZSize_For_Compress = (int)(bs.Length - tempChunks[chunksCount].Offset);
            }
        }
    }
}
