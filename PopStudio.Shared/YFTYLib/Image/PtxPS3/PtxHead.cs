namespace PopStudio.Image.PtxPS3
{
    internal class PtxHead
    {
        public static readonly string magic = "DDS ";
        public static readonly string nvt = "NVTT";
        public static readonly string tex = "DXT5";

        public int height;
        public int width;
        public int texturesize;

        public PtxHead Read(BinaryStream bs)
        {
            bs.IdString(magic);
            bs.IdInt32(0x7C);
            bs.IdInt32(528391);
            height = bs.ReadInt32();
            width = bs.ReadInt32();
            texturesize = bs.ReadInt32();
            for (int i = 0; i < 11; i++)
            {
                bs.IdInt32(0);
            }
            bs.IdString(nvt);
            bs.IdInt32(131080);
            bs.IdInt32(32);
            bs.IdInt32(4);
            bs.IdString(tex);
            for (int i = 0; i < 5; i++)
            {
                bs.IdInt32(0);
            }
            bs.IdInt32(4096);
            for (int i = 0; i < 4; i++)
            {
                bs.IdInt32(0);
            }
            return this;
        }

        public void Write(BinaryStream bs)
        {
            if (texturesize == 0) texturesize = width * height;
            bs.WriteString(magic);
            bs.WriteInt32(0x7C);
            bs.WriteInt32(528391);
            bs.WriteInt32(height);
            bs.WriteInt32(width);
            bs.WriteInt32(texturesize);
            for (int i = 0; i < 11; i++)
            {
                bs.WriteInt32(0);
            }
            bs.WriteString(nvt);
            bs.WriteInt32(131080);
            bs.WriteInt32(32);
            bs.WriteInt32(4);
            bs.WriteString(tex);
            for (int i = 0; i < 5; i++)
            {
                bs.WriteInt32(0);
            }
            bs.WriteInt32(4096);
            for (int i = 0; i < 4; i++)
            {
                bs.WriteInt32(0);
            }
        }
    }
}
