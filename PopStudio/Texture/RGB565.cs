using SkiaSharp;

namespace PopStudio.Texture
{
    internal static class RGB565
    {
        public static SKBitmap Read(BinaryStream bs, int width, int height)
        {
            int S = width * height;
            SKColor[] pixels = new SKColor[S];
            ushort temp;
            for (int i = 0; i < S; i++)
            {
                temp = bs.ReadUInt16();
                pixels[i] = new SKColor((byte)((temp & 0xF800) >> 8), (byte)((temp & 0x7E0) >> 3), (byte)((temp & 0x1F) << 3), 255);
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
                bs.WriteUInt16((ushort)(((pixels[i].Blue & 0xF8) >> 3) | ((pixels[i].Green & 0xFC) << 3) | ((pixels[i].Red & 0xF8) << 8)));
            }
            return image.Width << 1;
        }
    }
}
