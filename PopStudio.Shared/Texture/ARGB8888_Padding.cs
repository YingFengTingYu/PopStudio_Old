using PopStudio.Platform;

namespace PopStudio.Texture
{
    internal static unsafe class ARGB8888_Padding
    {
        public static YFBitmap Read(BinaryStream bs, int width, int height, int blockSize)
        {
            YFBitmap image = YFBitmap.Create(width, height);
            YFColor* pixels = (YFColor*)image.GetPixels().ToPointer();
            uint temp;
            long off = bs.Position;
            int times = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    temp = bs.ReadUInt32();
                    *pixels++ = new YFColor((byte)((temp & 0xFF0000) >> 16), (byte)((temp & 0xFF00) >> 8), (byte)(temp & 0xFF), (byte)(temp >> 24));
                }
                bs.Position = off + (++times) * blockSize;
            }
            return image;
        }

        public static int Write(BinaryStream bs, YFBitmap image, int blockSize)
        {
            YFColor* pixels = (YFColor*)image.GetPixels().ToPointer();
            int height = image.Height;
            int width = image.Width;
            int CDSize = blockSize - (width << 2);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    bs.WriteUInt32(((uint)pixels->Alpha << 24) | ((uint)pixels->Red << 16) | ((uint)pixels->Green << 8) | pixels->Blue);
                    pixels++;
                }
                for (int j = 0; j < CDSize; j++)
                {
                    bs.WriteByte(0x0);
                }
            }
            return blockSize;
        }
    }
}