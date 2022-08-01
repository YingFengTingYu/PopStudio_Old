namespace PopStudio.Package.Rsb
{
    internal class ChildRsgpInfo
    {
        public uint index;
        public uint ratio;
        public string language;

        public void Write(BinaryStream bs)
        {
            bs.WriteUInt32(index);
            bs.WriteUInt32(ratio);
            bs.WriteString(language, 4, bs.Endian);
            bs.WriteInt32(0);
        }

        public ChildRsgpInfo Read(BinaryStream bs)
        {
            index = bs.ReadUInt32();
            ratio = bs.ReadUInt32();
            language = bs.ReadString(4, bs.Endian).Replace("\0", "");
            bs.IdInt32(0);
            return this;
        }
    }

    internal class RsbCompositeInfo
    {
        public string ID;
        public ChildRsgpInfo[] child_Info = new ChildRsgpInfo[0x40];
        public uint child_Number;

        public RsbCompositeInfo()
        {
            for (int i = 0; i < child_Info.Length; i++)
            {
                child_Info[i] = new ChildRsgpInfo();
            }
        }

        public void Write(BinaryStream bs)
        {
            bs.WriteString(ID, 0x80);
            for (int i = 0; i < 0x40; i++)
            {
                child_Info[i].Write(bs);
            }
            bs.WriteUInt32(child_Number);
        }

        public RsbCompositeInfo Read(BinaryStream bs)
        {
            ID = bs.ReadString(0x80).Replace("\0", "");
            for (int i = 0; i < 0x40; i++)
            {
                child_Info[i].Read(bs);
            }
            child_Number = bs.ReadUInt32();
            return this;
        }
    }
}
