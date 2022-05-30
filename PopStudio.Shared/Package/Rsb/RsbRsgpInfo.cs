namespace PopStudio.Package.Rsb
{
    internal class RsbRsgpInfo
    {
        public string ID;
        public uint offset;
        public uint size;
        public uint pool_Index; //AutoPool index
        public uint flags = 0b1;
        public uint fileOffset;
        public uint part0_Offset;
        public uint part0_ZSize;
        public uint part0_Size;
        public uint part0_Size2;
        public uint part1_Offset;
        public uint part1_ZSize;
        public uint part1_Size;
        public uint ptx_Number;
        public uint ptx_BeforeNumber;

        public void Write(BinaryStream bs)
        {
            bs.WriteString(ID, 0x80);
            bs.WriteUInt32(offset);
            bs.WriteUInt32(size);
            bs.WriteUInt32(pool_Index);
            bs.WriteUInt32(flags);
            bs.WriteUInt32(fileOffset);
            bs.WriteUInt32(part0_Offset);
            bs.WriteUInt32(part0_ZSize);
            bs.WriteUInt32(part0_Size);
            bs.WriteUInt32(part0_Size2 == 0 ? part0_Size : part0_Size2);
            bs.WriteUInt32(part1_Offset);
            bs.WriteUInt32(part1_ZSize);
            bs.WriteUInt32(part1_Size);
            bs.WriteInt32(0);
            bs.WriteInt32(0);
            bs.WriteInt32(0);
            bs.WriteInt32(0);
            bs.WriteInt32(0);
            bs.WriteUInt32(ptx_Number);
            bs.WriteUInt32(ptx_BeforeNumber);
        }

        public RsbRsgpInfo Read(BinaryStream bs)
        {
            ID = bs.ReadString(0x80).Replace("\0", "");
            offset = bs.ReadUInt32();
            size = bs.ReadUInt32();
            pool_Index = bs.ReadUInt32();
            flags = bs.ReadUInt32();
            fileOffset = bs.ReadUInt32();
            part0_Offset = bs.ReadUInt32();
            part0_ZSize = bs.ReadUInt32();
            part0_Size = bs.ReadUInt32();
            part0_Size2 = bs.ReadUInt32();
            part1_Offset = bs.ReadUInt32();
            part1_ZSize = bs.ReadUInt32();
            part1_Size = bs.ReadUInt32();
            bs.IdInt32(0);
            bs.IdInt32(0);
            bs.IdInt32(0);
            bs.IdInt32(0);
            bs.IdInt32(0);
            ptx_Number = bs.ReadUInt32();
            ptx_BeforeNumber = bs.ReadUInt32();
            return this;
        }
    }
}
