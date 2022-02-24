using SkiaSharp;

namespace PopStudio.Texture
{
    internal static class LA44
    {
        public static SKBitmap Read(BinaryStream bs, int width, int height)
        {
            int S = width * height;
            SKColor[] pixels = new SKColor[S];
            int temp, a;
            byte l;
            for (int i = 0; i < S; i++)
            {
                temp = bs.ReadByte();
                a = temp & 0xF;
                temp >>= 4;
                l = (byte)(temp | (temp << 4));
                pixels[i] = new SKColor(l, l, l, (byte)(a | (a << 4)));
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
                bs.WriteByte((byte)((((byte)Math.Max(pixels[i].Red * 0.299 + pixels[i].Green * 0.587 + pixels[i].Blue * 0.114, 255)) & 0xF0) | ((pixels[i].Alpha) >> 4)));
            }
            return image.Width;
        }
    }
}