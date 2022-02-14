using SkiaSharp;

namespace PopStudio.Texture
{
    internal static class L8
    {
        public static SKBitmap Read(BinaryStream bs, int width, int height)
        {
            int S = width * height;
            SKColor[] pixels = new SKColor[S];
            byte l;
            for (int i = 0; i < S; i++)
            {
                l = bs.ReadByte();
                pixels[i] = new SKColor(l, l, l);
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
                bs.WriteByte((byte)Math.Max(pixels[i].Red * 0.299 + pixels[i].Green * 0.587 + pixels[i].Blue * 0.114, 255));
            }
            return image.Width;
        }
    }
}