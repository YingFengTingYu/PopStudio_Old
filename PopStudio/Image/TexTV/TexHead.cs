namespace PopStudio.Image.TexTV
{
    internal class TexHead
    {
        public static readonly string magic = "SEXYTEX\0";
        public static readonly int version = 0;
        public int width;
        public int height;
        public TexFormat format;
        public int zsize;

        public TexHead Read(BinaryStream bs)
        {
            bs.IdString(magic);
            bs.IdInt32(version);
            width = bs.ReadInt32();
            height = bs.ReadInt32();
            format = (TexFormat)bs.ReadInt32();
            bs.IdInt32(1);
            bs.IdInt32(1);
            zsize = bs.ReadInt32();
            bs.IdInt32(0);
            bs.IdInt32(0);
            bs.IdInt32(0);
            return this;
        }

        public void Write(BinaryStream bs)
        {
            bs.WriteString(magic);
            bs.WriteInt32(version);
            bs.WriteInt32(width);
            bs.WriteInt32(height);
            bs.WriteInt32((int)format);
            bs.WriteInt32(1);
            bs.WriteInt32(1);
            bs.WriteInt32(zsize);
            bs.WriteInt32(0);
            bs.WriteInt32(0);
            bs.WriteInt32(0);
        }
    }
}
