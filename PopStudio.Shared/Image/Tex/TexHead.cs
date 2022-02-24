namespace PopStudio.Image.Tex
{
    internal class TexHead
    {
        public static readonly ushort magic = 2677;
        public ushort width;
        public ushort height;
        public TexFormat format;

        public TexHead Read(BinaryStream bs)
        {
            bs.IdUInt16(magic);
            width = bs.ReadUInt16();
            height = bs.ReadUInt16();
            format = (TexFormat)bs.ReadUInt16();
            return this;
        }

        public void Write(BinaryStream bs)
        {
            bs.WriteUInt16(magic);
            bs.WriteUInt16(width);
            bs.WriteUInt16(height);
            bs.WriteUInt16((ushort)format);
        }
    }
}