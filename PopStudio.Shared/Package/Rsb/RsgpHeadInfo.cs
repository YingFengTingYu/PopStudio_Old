namespace PopStudio.Package.Rsb
{
    internal class RsgpHeadInfo
    {
        public static readonly int magic = 1920165744;
        public static readonly int maxVersion = 4;

        public int magic_t = magic; //for other rsb obscure tools
        public int version = 0x3;
        public uint flags = 0b1;
        public uint fileOffset;
        public uint part0_Offset;
        public uint part0_ZSize;
        public uint part0_Size;
        public uint part1_Offset;
        public uint part1_ZSize;
        public uint part1_Size;
        public uint fileList_Length;
        public uint fileList_BeginOffset = 0x5C;

        public void Write(BinaryStream bs)
        {
            bs.WriteInt32(magic_t);
            bs.WriteInt32(version);
            bs.WriteInt32(0);
            bs.WriteInt32(0);
            bs.WriteUInt32(flags);
            bs.WriteUInt32(fileOffset);
            bs.WriteUInt32(part0_Offset);
            bs.WriteUInt32(part0_ZSize);
            bs.WriteUInt32(part0_Size);
            bs.WriteInt32(0);
            bs.WriteUInt32(part1_Offset);
            bs.WriteUInt32(part1_ZSize);
            bs.WriteUInt32(part1_Size);
            bs.WriteInt32(0);
            bs.WriteInt32(0);
            bs.WriteInt32(0);
            bs.WriteInt32(0);
            bs.WriteInt32(0);
            bs.WriteUInt32(fileList_Length);
            bs.WriteUInt32(fileList_BeginOffset);
            bs.WriteInt32(0);
            bs.WriteInt32(0);
            bs.WriteInt32(0);
        }

        public RsgpHeadInfo Read(BinaryStream bs)
        {
            magic_t = bs.ReadInt32();
            version = bs.ReadInt32();
            _ = bs.ReadInt32();
            _ = bs.ReadInt32();
            flags = bs.ReadUInt32();
            fileOffset = bs.ReadUInt32();
            part0_Offset = bs.ReadUInt32();
            part0_ZSize = bs.ReadUInt32();
            part0_Size = bs.ReadUInt32();
            _ = bs.ReadInt32();
            part1_Offset = bs.ReadUInt32();
            part1_ZSize = bs.ReadUInt32();
            part1_Size = bs.ReadUInt32();
            _ = bs.ReadInt32();
            _ = bs.ReadInt32();
            _ = bs.ReadInt32();
            _ = bs.ReadInt32();
            _ = bs.ReadInt32();
            fileList_Length = bs.ReadUInt32();
            fileList_BeginOffset = bs.ReadUInt32();
            _ = bs.ReadInt32();
            _ = bs.ReadInt32();
            _ = bs.ReadInt32();
            return this;
        }
    }
}
