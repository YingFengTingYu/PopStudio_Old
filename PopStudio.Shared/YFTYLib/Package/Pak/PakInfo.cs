namespace PopStudio.Package.Pak
{
    internal class PakInfo
    {
        public static readonly int magic = -1161803072;
        public static readonly int version = 0x0;
        public static readonly byte infoEnd = 0x80;

        public List<FileInfo> fileInfoLibrary;
        public bool? compress = null; //0x10 is ps3, 0xC is PC or Xbox360
        public bool x360 = false; //true is x360 0x1000 for ptx (not mean the pak is from Xbox360!!!)
        public bool pc = false; //true is pc version
        public bool xmem = false; //true is using xmemcompress
        public bool win = true; //true is using windows PATHSEPARATOR

        public void Write(BinaryStream bs)
        {
            bs.WriteInt32(magic);
            bs.WriteInt32(version);
            if (fileInfoLibrary == null) return;
            for (int i = 0; i < fileInfoLibrary.Count; i++)
            {
                bs.WriteUInt8(0x0);
                fileInfoLibrary[i].Write(bs, compress);
            }
            bs.WriteUInt8(infoEnd);
        }

        public PakInfo Read(BinaryStream bs)
        {
            bs.IdInt32(magic);
            bs.IdInt32(version);
            try
            {
                fileInfoLibrary = new List<FileInfo>();
                while (true)
                {
                    if (bs.ReadByte() == infoEnd)
                    {
                        break;
                    }
                    if (compress == null) //必要性探路
                    {
                        long b = bs.Position;
                        bs.ReadStringByUInt8Head();
                        bs.Position += 0x10;
                        byte h = bs.ReadByte();
                        if (h == 0x0 || h == infoEnd)
                        {
                            compress = true;
                        }
                        else
                        {
                            compress = false;
                        }
                        bs.Position = b;
                    }
                    fileInfoLibrary.Add(new FileInfo().Read(bs, compress));
                }
            }
            catch (Exception)
            {
                bs.Position = 0x8;
                compress = false;
                fileInfoLibrary = new List<FileInfo>();
                fileInfoLibrary.Add(new FileInfo().Read(bs, compress));
            }
            foreach (FileInfo file in fileInfoLibrary)
            {
                if (file.fileName.IndexOf('/') > -1)
                {
                    win = false;
                    break;
                }
                if (file.fileName.IndexOf('\\') > -1)
                {
                    break;
                }
            }
            return this;
        }

        public static void Fill0x1000(BinaryStream bs)
        {
            //先确定要补多少
            int l = (int)(bs.Position & (0x1000 - 1));
            if (l == 0)
            {
                bs.WriteUInt16(0x1000 - 2);
                for (int i = 2; i < 0x1000; i++)
                {
                    bs.WriteByte(0x0);
                }
            }
            else if (l > 0x1000 - 2)
            {
                ushort w = (ushort)(0x2000 - 2 - l);
                bs.WriteUInt16(w);
                for (int i = 0; i < w; i++)
                {
                    bs.WriteByte(0x0);
                }
            }
            else
            {
                ushort w = (ushort)(0x1000 - 2 - l);
                bs.WriteUInt16(w);
                for (int i = 0; i < w; i++)
                {
                    bs.WriteByte(0x0);
                }
            }
        }

        public static void Fill(BinaryStream bs)
        {
            //先确定要补多少
            int l = (int)(bs.Position & 0b111);
            if (l == 0)
            {
                bs.WriteUInt16(0x06);
                for (int i = 0; i < 6; i++)
                {
                    bs.WriteByte(0x0);
                }
            }
            else if (l > 5)
            {
                ushort w = (ushort)(14 - l);
                bs.WriteUInt16(w);
                for (int i = 0; i < w; i++)
                {
                    bs.WriteByte(0x0);
                }
            }
            else
            {
                ushort w = (ushort)(6 - l);
                bs.WriteUInt16(w);
                for (int i = 0; i < w; i++)
                {
                    bs.WriteByte(0x0);
                }
            }
        }

        public static bool Jump(BinaryStream bs)
        {
            ushort jmp = bs.ReadUInt16();
            bs.Position += jmp;
            if (jmp > 8) return true;
            return false;
        }
    }
}
