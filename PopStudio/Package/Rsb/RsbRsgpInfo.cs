namespace PopStudio.Package.Rsb
{
    internal class RsbRsgpInfo
    {
        public string ID;
        public int offset;
        public int size;
        public int index;
        public int flags = 0b1;
        public int fileOffset;
        public int part0_Offset;
        public int part0_ZSize;
        public int part0_Size;
        public int part0_Size2;
        public int part1_Offset;
        public int part1_ZSize;
        public int part1_Size;
        public int ptx_Number;
        public int ptx_BeforeNumber;

        public void Write(BinaryStream bs)
        {
            bs.WriteString(ID, 0x80);
            bs.WriteInt32(offset);
            bs.WriteInt32(size);
            bs.WriteInt32(index);
            bs.WriteInt32(flags);
            bs.WriteInt32(fileOffset);
            bs.WriteInt32(part0_Offset);
            bs.WriteInt32(part0_ZSize);
            bs.WriteInt32(part0_Size);
            bs.WriteInt32(part0_Size2 == 0 ? part0_Size : part0_Size2);
            bs.WriteInt32(part1_Offset);
            bs.WriteInt32(part1_ZSize);
            bs.WriteInt32(part1_Size);
            bs.WriteInt32(0);
            bs.WriteInt32(0);
            bs.WriteInt32(0);
            bs.WriteInt32(0);
            bs.WriteInt32(0);
            bs.WriteInt32(ptx_Number);
            bs.WriteInt32(ptx_BeforeNumber);
        }

        public RsbRsgpInfo Read(BinaryStream bs)
        {
            ID = bs.ReadString(0x80).Replace("\0", "");
            offset = bs.ReadInt32();
            size = bs.ReadInt32();
            index = bs.ReadInt32();
            flags = bs.ReadInt32();
            fileOffset = bs.ReadInt32();
            part0_Offset = bs.ReadInt32();
            part0_ZSize = bs.ReadInt32();
            part0_Size = bs.ReadInt32();
            part0_Size2 = bs.ReadInt32();
            part1_Offset = bs.ReadInt32();
            part1_ZSize = bs.ReadInt32();
            part1_Size = bs.ReadInt32();
            bs.IdInt32(0);
            bs.IdInt32(0);
            bs.IdInt32(0);
            bs.IdInt32(0);
            bs.IdInt32(0);
            ptx_Number = bs.ReadInt32();
            ptx_BeforeNumber = bs.ReadInt32();
            return this;
        }
    }
}
