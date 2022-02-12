namespace PopStudio.Package.Rsb
{
    internal class RsgpHeadInfo
    {
        public static readonly int magic = 1920165744;
        public static readonly int maxVersion = 4;

        public int magic_t = magic; //for other rsb obscure tools
        public int version = 0x3;
        public int flags = 0b1;
        public int fileOffset;
        public int part0_Offset;
        public int part0_ZSize;
        public int part0_Size;
        public int part1_Offset;
        public int part1_ZSize;
        public int part1_Size;
        public int fileList_Length;
        public int fileList_BeginOffset = 0x5C;

        public void Write(BinaryStream bs)
        {
            bs.WriteInt32(magic_t);
            bs.WriteInt32(version);
            bs.WriteInt32(0);
            bs.WriteInt32(0);
            bs.WriteInt32(flags);
            bs.WriteInt32(fileOffset);
            bs.WriteInt32(part0_Offset);
            bs.WriteInt32(part0_ZSize);
            bs.WriteInt32(part0_Size);
            bs.WriteInt32(0);
            bs.WriteInt32(part1_Offset);
            bs.WriteInt32(part1_ZSize);
            bs.WriteInt32(part1_Size);
            bs.WriteInt32(0);
            bs.WriteInt32(0);
            bs.WriteInt32(0);
            bs.WriteInt32(0);
            bs.WriteInt32(0);
            bs.WriteInt32(fileList_Length);
            bs.WriteInt32(fileList_BeginOffset);
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
            flags = bs.ReadInt32();
            fileOffset = bs.ReadInt32();
            part0_Offset = bs.ReadInt32();
            part0_ZSize = bs.ReadInt32();
            part0_Size = bs.ReadInt32();
            _ = bs.ReadInt32();
            part1_Offset = bs.ReadInt32();
            part1_ZSize = bs.ReadInt32();
            part1_Size = bs.ReadInt32();
            _ = bs.ReadInt32();
            _ = bs.ReadInt32();
            _ = bs.ReadInt32();
            _ = bs.ReadInt32();
            _ = bs.ReadInt32();
            fileList_Length = bs.ReadInt32();
            fileList_BeginOffset = bs.ReadInt32();
            _ = bs.ReadInt32();
            _ = bs.ReadInt32();
            _ = bs.ReadInt32();
            return this;
        }
    }
}
