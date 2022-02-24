namespace PopStudio.Image.PtxPSV
{
    internal class PtxHead
    {
        public static readonly string magic = "GXT\0";
        public static readonly int version = 0x10000003;

        public int size;
        public ushort width;
        public ushort height;

        public PtxHead Read(BinaryStream bs)
        {
            bs.IdString(magic);
            bs.IdInt32(version);
            bs.IdInt32(1);
            bs.IdInt32(0x40);
            size = bs.ReadInt32();
            bs.IdInt32(0);
            bs.IdInt32(0);
            bs.IdInt32(0);
            bs.IdInt32(0x40);
            bs.IdInt32(size);
            bs.IdInt32(-1);
            bs.IdInt32(0);
            bs.IdInt32(0);
            bs.IdInt32(-2030043136);
            width = bs.ReadUInt16();
            height = bs.ReadUInt16();
            bs.IdInt32(1);
            return this;
        }

        public void Write(BinaryStream bs)
        {
            if (size == 0) size = (int)(bs.Length - 0x40);
            bs.WriteString(magic);
            bs.WriteInt32(version);
            bs.WriteInt32(1);
            bs.WriteInt32(0x40);
            bs.WriteInt32(size);
            bs.WriteInt32(0);
            bs.WriteInt32(0);
            bs.WriteInt32(0);
            bs.WriteInt32(0x40);
            bs.WriteInt32(size);
            bs.WriteInt32(-1);
            bs.WriteInt32(0);
            bs.WriteInt32(0);
            bs.WriteInt32(-2030043136);
            bs.WriteUInt16(width);
            bs.WriteUInt16(height);
            bs.WriteInt32(1);
        }
    }
}
