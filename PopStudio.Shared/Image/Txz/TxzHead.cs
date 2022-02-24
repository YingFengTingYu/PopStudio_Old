namespace PopStudio.Image.Txz
{
    internal class TxzHead
    {
        public static readonly ushort magic = 2677;
        public ushort width;
        public ushort height;
        public TxzFormat format;

        public TxzHead Read(BinaryStream bs)
        {
            bs.IdUInt16(magic);
            width = bs.ReadUInt16();
            height = bs.ReadUInt16();
            format = (TxzFormat)bs.ReadUInt16();
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