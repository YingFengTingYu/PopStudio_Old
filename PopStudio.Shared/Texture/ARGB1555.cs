using SkiaSharp;

namespace PopStudio.Texture
{
    internal static class ARGB1555
    {
        public static SKBitmap Read(BinaryStream bs, int width, int height)
        {
            int S = width * height;
            SKColor[] pixels = new SKColor[S];
            ushort temp;
            int r, g, b;
            for (int i = 0; i < S; i++)
            {
                temp = bs.ReadUInt16();
                r = (temp & 0x7C00) >> 10;
                g = (temp & 0x3E0) >> 5;
                b = temp & 0x1F;
                pixels[i] = new SKColor((byte)((r << 3) | (r >> 2)), (byte)((g << 3) | (g >> 2)), (byte)((b << 3) | (b >> 2)), (byte)((temp & 0x8000) == 0 ? 0 : 255));
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
                bs.WriteUInt16((ushort)(((pixels[i].Alpha & 0b10000000) << 8) | ((pixels[i].Blue & 0xF8) >> 3) | ((pixels[i].Green & 0xF8) << 2) | ((pixels[i].Red & 0xF8) << 7)));
            }
            return image.Width << 1;
        }
    }
}
