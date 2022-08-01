namespace PopStudio.Image.Ptx
{
    internal class PtxHead
    {
        public static readonly int magic = 1886681137;
        public static readonly int version = 1;

        public int width;
        public int height;
        public int check;
        public PtxFormat format;
        public int alphaSize;
        public int alphaFormat;

        public PtxHead Read(BinaryStream bs)
        {
            int thismagic = bs.ReadInt32();
            if (thismagic == 829977712)
            {
                bs.Endian = bs.Endian == Endian.Small ? Endian.Big : Endian.Small;
            }
            else if (thismagic != magic)
            {
                throw new Exception(Str.Obj.DataMisMatch);
            }
            bs.IdInt32(version);
            width = bs.ReadInt32();
            height = bs.ReadInt32();
            check = bs.ReadInt32();
            format = (PtxFormat)bs.ReadInt32();
            alphaSize = bs.ReadInt32();
            alphaFormat = bs.ReadInt32();
            return this;
        }

        public void Write(BinaryStream bs)
        {
            bs.WriteInt32(magic);
            bs.WriteInt32(version);
            bs.WriteInt32(width);
            bs.WriteInt32(height);
            bs.WriteInt32(check);
            bs.WriteInt32((int)format);
            bs.WriteInt32(alphaSize);
            bs.WriteInt32(alphaFormat);
        }
    }
}