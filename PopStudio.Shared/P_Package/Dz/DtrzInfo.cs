namespace PopStudio.P_Package.Dz
{
    internal class DtrzInfo
    {
        public static readonly string magic = "DTRZ";
        public static readonly ushort firstIndex = 0x1;

        public ushort fileNumber;
        public ushort folderNumber;
        public byte version = 0;
        public string[] fileNameLibrary;
        public string[] folderNameLibrary;
        public MatchInfo[] matchInfoLibrary;
        public ushort lastIndex;
        public FileInfo[] fileInfoLibrary;

        public void WritePart1(BinaryStream bs)
        {
            //初始化
            fileNumber = (ushort)fileNameLibrary.Length;
            folderNumber = (ushort)folderNameLibrary.Length;
            lastIndex = fileNumber;
            //写入
            bs.WriteString(magic);
            bs.WriteUInt16(fileNumber);
            bs.WriteUInt16(folderNumber);
            bs.WriteByte(version);
            for (int i = 0; i < fileNumber; i++)
            {
                bs.WriteStringByEmpty(fileNameLibrary[i]);
            }
            for (int i = 1; i < folderNumber; i++) //0是空
            {
                bs.WriteStringByEmpty(folderNameLibrary[i]);
            }
            for (int i = 0; i < matchInfoLibrary.Length; i++)
            {
                matchInfoLibrary[i].Write(bs);
            }
            bs.WriteUInt16(firstIndex);
            bs.WriteUInt16(lastIndex);
        }

        public void WritePart2(BinaryStream bs)
        {
            for (int i = 0; i < fileInfoLibrary.Length; i++)
            {
                fileInfoLibrary[i].Write(bs);
            }
        }

        /// <summary>
        /// 从dz中读取DTRZ所有信息
        /// </summary>
        /// <param name="bs"></param>
        /// <exception cref="WrongFileMagicException"></exception>
        /// <exception cref="FileCorruptionException"></exception>
        public DtrzInfo Read(BinaryStream bs)
        {
            bs.IdString(magic);
            fileNumber = bs.ReadUInt16();
            folderNumber = bs.ReadUInt16();
            version = bs.ReadUInt8();
            fileNameLibrary = new string[fileNumber];
            for (ushort i = 0; i < fileNumber; i++)
            {
                fileNameLibrary[i] = bs.ReadStringByEmpty();
            }
            folderNameLibrary = new string[folderNumber];
            folderNameLibrary[0] = string.Empty;
            for (ushort i = 1; i < folderNumber; i++)
            {
                folderNameLibrary[i] = bs.ReadStringByEmpty();
            }
            matchInfoLibrary = new MatchInfo[fileNumber];
            for (ushort i = 0; i < fileNumber; i++)
            {
                matchInfoLibrary[i] = new MatchInfo();
                matchInfoLibrary[i].Read(bs);
                matchInfoLibrary[i].fileIndex = i;
            }
            bs.IdUInt16(firstIndex);
            if ((lastIndex = bs.ReadUInt16()) < fileNumber)
            {
                throw new Exception(Str.Obj.DataMisMatch);
            }
            fileInfoLibrary = new FileInfo[lastIndex];
            FileInfo[] temp = new FileInfo[lastIndex];
            for (ushort i = 0; i < lastIndex; i++)
            {
                fileInfoLibrary[i] = new FileInfo();
                fileInfoLibrary[i].Read(bs);
                temp[i] = fileInfoLibrary[i]; //这个和fileInfoLibrary指向同一FileInfo，等会排序之后无伤大雅
            }
            Array.Sort(temp, new FileInfo.CompareFileInfoByOffsetAndUp());
            lastIndex--;
            for (ushort i = 0; i < lastIndex; i++)
            {
                temp[i].zsize = temp[i + 1].offset - temp[i].offset;
            }
            CompressFlags flag = temp[lastIndex].compressMethod;
            switch (flag)
            {
                case CompressFlags.MP3:
                case CompressFlags.JPEG:
                case CompressFlags.ZERO:
                case CompressFlags.STORE:
                    temp[lastIndex].zsize = temp[lastIndex].size;
                    break;
                default:
                    temp[lastIndex].zsize = (int)(bs.Length - temp[lastIndex].offset);
                    break;
            }
            return this;
        }
    }
}