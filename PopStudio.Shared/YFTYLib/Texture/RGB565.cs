using PopStudio.Platform;

namespace PopStudio.Texture
{
    internal static unsafe class RGB565
    {
        public static YFBitmap Read(BinaryStream bs, int width, int height)
        {
            YFBitmap image = YFBitmap.Create(width, height);
            int S = width * height;
            YFColor* pixels = (YFColor*)image.GetPixels().ToPointer();
            ushort temp;
            int r, g, b;
            for (int i = 0; i < S; i++)
            {
                temp = bs.ReadUInt16();
                r = temp >> 11;
                g = (temp & 0x7E0) >> 5;
                b = temp & 0x1F;
                *pixels++ = new YFColor((byte)((r << 3) | (r >> 2)), (byte)((g << 2) | (g >> 4)), (byte)((b << 3) | (b >> 2)));
            }
            return image;
        }

        public static int Write(BinaryStream bs, YFBitmap image)
        {
            YFColor* pixels = (YFColor*)image.GetPixels().ToPointer();
            int S = image.Square;
            for (int i = 0; i < S; i++)
            {
                bs.WriteUInt16((ushort)((pixels->Blue >> 3) | ((pixels->Green & 0xFC) << 3) | ((pixels->Red & 0xF8) << 8)));
                pixels++;
            }
            return image.Width << 1;
        }
    }
}
