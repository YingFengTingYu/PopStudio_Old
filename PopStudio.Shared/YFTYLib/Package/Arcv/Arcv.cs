namespace PopStudio.Package.Arcv
{
    internal static class Arcv
    {
        static string CheckExtension(string str)
        {
            return str switch
            {
                "RGCN" => ".NCGR", //Nitro Character Graphic Resource
                "RCSN" => ".NSCR", //Nitro Screen Resource
                "RLCN" => ".NCLR", //Nitro Color Resource
                "RNAN" => ".NANR", //Nitro Animation Resource
                "RECN" => ".NCER", //Nitro Cell Resource
                "RAMN" => ".NMAR", //Unknow
                "RCMN" => ".NMCR", //Unknow
                "RTFN" => ".NFTR", //Nitro Font Resource
                "SDAT" => ".sdat", //Sound Data
                "NARC" => ".narc", //Nintendo Archives
                _ => ".dat"
            };
        }

        /// <summary>
        /// It's so easy that I don't need to use any other class or struct to describe it
        /// </summary>
        /// <param name="inFile"></param>
        /// <param name="outFile"></param>
        public static void Unpack(string inFile, string outFile)
        {
            Dir.FormatAndDeleteEndPathSeparator(ref inFile);
            Dir.FormatAndDeleteEndPathSeparator(ref outFile);
            Dir.NewDir(outFile);
            outFile = outFile + Const.PATHSEPARATOR;// + "NDS";
            using (BinaryStream BS = new BinaryStream(inFile, FileMode.Open))
            {
                BS.IdString("ARCV");
                int fileNumber = BS.ReadInt32();
                BS.Position += 0x4;
                long tmpOffset;
                int size;
                long offset;
                long crc32;
                for (int i = 0; i < fileNumber; i++)
                {
                    offset = BS.ReadUInt32();
                    size = BS.ReadInt32();
                    crc32 = BS.ReadUInt32();
                    tmpOffset = BS.Position;
                    BS.Position = offset;
                    using (BinaryStream BS2 = new BinaryStream(outFile + crc32.ToString().PadLeft(10, '0') + CheckExtension(BS.PeekString(4)), FileMode.Create))
                    {
                        BS2.WriteBytes(BS.ReadBytes(size));
                    }
                    BS.Position = tmpOffset;
                }
            }
        }

        public static void Pack(string inFile, string outFile)
        {
            Dir.FormatAndDeleteEndPathSeparator(ref inFile);
            Dir.FormatAndDeleteEndPathSeparator(ref outFile);
            Dir.NewDir(outFile, false);
            string[] files = Dir.GetFiles(inFile);
            List<(long crc32, string filePath)> lst = new List<(long crc32, string filePath)>();
            int filearrNumber = files.Length;
            for (int i = 0; i < filearrNumber; i++)
            {
                try
                {
                    lst.Add((Convert.ToInt64(Path.GetFileNameWithoutExtension(files[i])), files[i]));
                }
                catch (Exception)
                {
                }
            }
            lst.Sort(((long crc32, string filePath) c1, (long crc32, string filePath) c2) => Math.Sign(c1.crc32 - c2.crc32));
            int fileNumber = lst.Count;
            ArcvFileInfo[] infos = new ArcvFileInfo[fileNumber];
            using (BinaryStream bs = new BinaryStream(outFile, FileMode.Create))
            {
                bs.WriteString("ARCV");
                bs.WriteInt32(fileNumber);
                bs.Position += 4;
                bs.Position += fileNumber * 12;
                for (int i = 0; i < fileNumber; i++)
                {
                    int p = (int)(bs.Position % 4);
                    if (p != 0)
                    {
                        for (int j = 4 - p; j > 0; j--)
                        {
                            bs.WriteByte(0xAC);
                        }
                    }
                    using (BinaryStream bs2 = new BinaryStream(lst[i].filePath, FileMode.Open))
                    {
                        infos[i] = new ArcvFileInfo(bs.Position, bs2.Length, lst[i].crc32);
                        bs2.CopyTo(bs);
                    }
                }
                bs.Position = 8;
                bs.WriteInt32((int)bs.Length);
                for (int i = 0; i < fileNumber; i++)
                {
                    infos[i].Write(bs);
                }
            }
        }

        class ArcvFileInfo
        {
            public int offset;
            public int size;
            public int crc32;

            public ArcvFileInfo()
            {

            }

            public ArcvFileInfo(long offset, long size, long crc32)
            {
                this.offset = (int)offset;
                this.size = (int)size;
                this.crc32 = (int)crc32;
            }

            public void Write(BinaryStream bs)
            {
                bs.WriteInt32(offset);
                bs.WriteInt32(size);
                bs.WriteInt32(crc32);
            }
        }
    }
}