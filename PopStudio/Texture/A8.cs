using SkiaSharp;

namespace PopStudio.Texture
{
    internal static class A8
    {
        public static SKBitmap Read(BinaryStream bs, int width, int height)
        {
            int S = width * height;
            SKColor[] pixels = new SKColor[S];
            for (int i = 0; i < S; i++)
            {
                pixels[i] = new SKColor(255, 255, 255, bs.ReadByte());
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
                bs.WriteByte(pixels[i].Alpha);
            }
            return image.Width;
        }
    }
}