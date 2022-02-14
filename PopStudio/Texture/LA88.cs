using SkiaSharp;

namespace PopStudio.Texture
{
    internal static class LA88
    {
        public static SKBitmap Read(BinaryStream bs, int width, int height)
        {
            int S = width * height;
            SKColor[] pixels = new SKColor[S];
            byte l;
            int temp;
            for (int i = 0; i < S; i++)
            {
                temp = bs.ReadUInt16();
                l = (byte)(temp >> 8);
                pixels[i] = new SKColor(l, l, l, (byte)(temp & 0xFF));
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
                bs.WriteUInt16((ushort)((((byte)Math.Max(pixels[i].Red * 0.299 + pixels[i].Green * 0.587 + pixels[i].Blue * 0.114, 255)) << 8) | pixels[i].Alpha));
            }
            return image.Width << 1;
        }
    }
}