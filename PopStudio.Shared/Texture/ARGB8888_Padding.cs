using SkiaSharp;

namespace PopStudio.Texture
{
    internal static class ARGB8888_Padding
    {
        public static SKBitmap Read(BinaryStream bs, int width, int height, int blockSize)
        {
            SKColor[] pixels = new SKColor[width * height];
            int temp;
            long off = bs.Position;
            int times = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    temp = bs.ReadInt32();
                    pixels[i * width + j] = new SKColor((byte)((temp & 0xFF0000) >> 16), (byte)((temp & 0xFF00) >> 8), (byte)(temp & 0xFF), (byte)(temp >> 24));
                }
                bs.Position = off + (++times) * blockSize;
            }
            SKBitmap image = new SKBitmap(width, height);
            image.Pixels = pixels;
            return image;
        }

        public static int Write(BinaryStream bs, SKBitmap image, int blockSize)
        {
            SKColor[] pixels = image.Pixels;
            int height = image.Height;
            int width = image.Width;
            int CDSize = blockSize - (width << 2);
            int k;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    k = i * width + j;
                    bs.WriteInt32((pixels[k].Alpha << 24) | (pixels[k].Red << 16) | (pixels[k].Green << 8) | pixels[k].Blue);
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