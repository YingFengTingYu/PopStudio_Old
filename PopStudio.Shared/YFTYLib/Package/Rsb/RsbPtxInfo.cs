namespace PopStudio.Package.Rsb
{
    internal class RsbPtxInfo
    {
        readonly uint Ptx_EachLength = 0x10;

        public uint width;
        public uint height;
        public uint check;
        public uint format;
        public uint alphaSize;
        public uint alphaFormat;

        public RsbPtxInfo()
        {

        }

        public RsbPtxInfo(uint Ptx_EachLength)
        {
            this.Ptx_EachLength = Ptx_EachLength;
            if (Ptx_EachLength != 0x10 && Ptx_EachLength != 0x14 && Ptx_EachLength != 0x18)
            {
                throw new Exception();
            }
        }

        public void Write(BinaryStream bs)
        {
            bs.WriteUInt32(width);
            bs.WriteUInt32(height);
            bs.WriteUInt32(check);
            bs.WriteUInt32(format);
            if (Ptx_EachLength != 0x10) bs.WriteUInt32(alphaSize);
            if (Ptx_EachLength == 0x18) bs.WriteUInt32(alphaFormat);
        }

        public RsbPtxInfo Read(BinaryStream bs)
        {
            width = bs.ReadUInt32();
            height = bs.ReadUInt32();
            check = bs.ReadUInt32();
            format = bs.ReadUInt32();
            if (Ptx_EachLength >= 0x14)
            {
                alphaSize = bs.ReadUInt32();
                alphaFormat = Ptx_EachLength == 0x18 ? bs.ReadUInt32() : (alphaSize == 0 ? 0x0u : 0x64u);
            }
            return this;
        }
    }
}