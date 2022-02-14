using SkiaSharp;

namespace PopStudio.Texture
{
    internal static class RGB888
    {
        public static SKBitmap Read(BinaryStream bs, int width, int height)
        {
            int S = width * height;
            SKColor[] pixels = new SKColor[S];
            uint temp;
            for (int i = 0; i < S; i++)
            {
                temp = bs.ReadUInt24();
                pixels[i] = new SKColor((byte)(temp >> 16), (byte)((temp & 0xFF00) >> 8), (byte)(temp & 0xFF));
            }
            SKBitmap image = new SKBitmap(width, height);
            image.Pixels = pixels;
            return image;
        }

        public static int Write(BinaryStream bs, SKBitmap image)
        {
            SKColor[] pixels = image.Pixels;
            int S = pixels.Length;
            for (int i = 0; i < S; i++)
            {
                bs.WriteUInt24((uint)((pixels[i].Red << 16) | (pixels[i].Green << 8) | pixels[i].Blue));
            }
            return image.Width * 3;
        }
    }
}