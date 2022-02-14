using SkiaSharp;

namespace PopStudio.Texture
{
    internal static class ABGR8888
    {
        public static SKBitmap Read(BinaryStream bs, int width, int height)
        {
            int S = width * height;
            SKColor[] pixels = new SKColor[S];
            int temp;
            for (int i = 0; i < S; i++)
            {
                temp = bs.ReadInt32();
                pixels[i] = new SKColor((byte)(temp & 0xFF), (byte)((temp & 0xFF00) >> 8), (byte)((temp & 0xFF0000) >> 16), (byte)(temp >> 24));
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
                bs.WriteInt32((pixels[i].Alpha << 24) | (pixels[i].Blue << 16) | (pixels[i].Green << 8) | pixels[i].Red);
            }
            return image.Width << 2;
        }
    }
}