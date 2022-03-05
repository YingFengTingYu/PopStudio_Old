namespace PopStudio.Package.Rsb
{
    internal class RsbPtxInfo
    {
        readonly int Ptx_EachLength = 0x10;

        public int width;
        public int height;
        public int check;
        public int format;
        public int alphaSize;
        public int alphaFormat;

        public RsbPtxInfo()
        {

        }

        public RsbPtxInfo(int Ptx_EachLength)
        {
            this.Ptx_EachLength = Ptx_EachLength;
            if (Ptx_EachLength != 0x10 && Ptx_EachLength != 0x14 && Ptx_EachLength != 0x18)
            {
                throw new Exception();
            }
        }

        public void Write(BinaryStream bs)
        {
            bs.WriteInt32(width);
            bs.WriteInt32(height);
            bs.WriteInt32(check);
            bs.WriteInt32(format);
            if (Ptx_EachLength != 0x10) bs.WriteInt32(alphaSize);
            if (Ptx_EachLength == 0x18) bs.WriteInt32(alphaFormat);
        }

        public RsbPtxInfo Read(BinaryStream bs)
        {
            width = bs.ReadInt32();
            height = bs.ReadInt32();
            check = bs.ReadInt32();
            format = bs.ReadInt32();
            if (Ptx_EachLength >= 0x14)
            {
                alphaSize = bs.ReadInt32();
                alphaFormat = Ptx_EachLength == 0x18 ? bs.ReadInt32() : (alphaSize == 0 ? 0x0 : 0x64);
            }
            return this;
        }
    }
}