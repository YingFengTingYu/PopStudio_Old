namespace PopStudio.Image.TexTV
{
    internal class TexHead
    {
        public static readonly string magic = "SEXYTEX\0";
        public static readonly int version = 0;
        public int width;
        public int height;
        public int format;
        public uint flags = 1u;
        public int zsize;

        public TexHead Read(BinaryStream bs)
        {
            bs.IdString(magic);
            bs.IdInt32(version);
            width = bs.ReadInt32();
            height = bs.ReadInt32();
            format = bs.ReadInt32();
            flags = bs.ReadUInt32();
            bs.Position += 4;
            zsize = bs.ReadInt32();
            bs.Position += 12;
            return this;
        }

        public void Write(BinaryStream bs)
        {
            bs.WriteString(magic);
            bs.WriteInt32(version);
            bs.WriteInt32(width);
            bs.WriteInt32(height);
            bs.WriteInt32(format);
            bs.WriteUInt32(flags);
            bs.WriteInt32(1);
            bs.WriteInt32(zsize);
            bs.WriteInt32(0);
            bs.WriteInt32(0);
            bs.WriteInt32(0);
        }
    }
}
