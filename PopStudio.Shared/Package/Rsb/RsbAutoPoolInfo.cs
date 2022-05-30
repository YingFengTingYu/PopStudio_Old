namespace PopStudio.Package.Rsb
{
    internal class RsbAutoPoolInfo
    {
        public string ID;
        public uint part1_MaxOffset_InDecompress;
        public uint part1_MaxSize;
        public int type = 0x1;

        public void Write(BinaryStream bs)
        {
            bs.WriteString(ID, 0x80);
            bs.WriteUInt32(part1_MaxOffset_InDecompress);
            bs.WriteUInt32(part1_MaxSize);
            bs.WriteInt32(type);
            bs.WriteInt32(0);
            bs.WriteInt32(0);
            bs.WriteInt32(0);
        }

        public RsbAutoPoolInfo Read(BinaryStream bs)
        {
            ID = bs.ReadString(0x80).Replace("\0", "");
            part1_MaxOffset_InDecompress = bs.ReadUInt32();
            part1_MaxSize = bs.ReadUInt32();
            type = bs.ReadInt32();
            bs.IdInt32(0);
            bs.IdInt32(0);
            bs.IdInt32(0);
            return this;
        }
    }
}
