namespace PopStudio.Package.Rsb
{
    internal class ChildRsgpInfo
    {
        public int index;
        public int ratio;
        public string language;

        public void Write(BinaryStream bs)
        {
            bs.WriteInt32(index);
            bs.WriteInt32(ratio);
            bs.WriteString(language, 4, bs.Endian);
            bs.WriteInt32(0);
        }

        public ChildRsgpInfo Read(BinaryStream bs)
        {
            index = bs.ReadInt32();
            ratio = bs.ReadInt32();
            language = bs.ReadString(4, bs.Endian).Replace("\0", "");
            bs.IdInt32(0);
            return this;
        }
    }

    internal class RsbCompositeInfo
    {
        public string ID;
        public ChildRsgpInfo[] child_Info = new ChildRsgpInfo[0x40];
        public int child_Number;

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
            bs.WriteInt32(child_Number);
        }

        public RsbCompositeInfo Read(BinaryStream bs)
        {
            ID = bs.ReadString(0x80).Replace("\0", "");
            for (int i = 0; i < 0x40; i++)
            {
                child_Info[i].Read(bs);
            }
            child_Number = bs.ReadInt32();
            return this;
        }
    }
}
