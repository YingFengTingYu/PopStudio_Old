namespace PopStudio.Image.PtxXbox360
{
    internal class PtxHead
    {
        public static readonly int magic = 1409294362;

        public int width;
        public int height;
        public int blockSize;

        public PtxHead Read(BinaryStream bs)
        {
            width = bs.ReadInt32();
            height = bs.ReadInt32();
            blockSize = bs.ReadInt32();
            bs.IdInt32(magic);
            return this;
        }

        public void Write(BinaryStream bs)
        {
            bs.WriteInt32(width);
            bs.WriteInt32(height);
            bs.WriteInt32(blockSize);
            bs.WriteInt32(magic);
        }
    }
}