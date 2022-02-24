using SkiaSharp;

namespace PopStudio.Texture
{
    internal static class XRGB8888_A8
    {
        public static SKBitmap Read(BinaryStream bs, int width, int height)
        {
            int S = width * height;
            SKColor[] pixels = new SKColor[S];
            int temp;
            for (int i = 0; i < S; i++)
            {
                temp = bs.ReadInt32();
                pixels[i] = new SKColor((byte)((temp & 0xFF0000) >> 16), (byte)((temp & 0xFF00) >> 8), (byte)(temp & 0xFF));
            }
            for (int i = 0; i < S; i++)
            {
                pixels[i] = pixels[i].WithAlpha(bs.ReadByte());
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
                bs.WriteInt32((-16777216) | (pixels[i].Red << 16) | (pixels[i].Green << 8) | pixels[i].Blue);
            }
            for (int i = 0; i < S; i++)
            {
                bs.WriteByte(pixels[i].Alpha);
            }
            return image.Width << 2;
        }
    }
}