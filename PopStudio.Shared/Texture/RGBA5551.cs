using PopStudio.Platform;

namespace PopStudio.Texture
{
    internal static unsafe class RGBA5551
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
                r = (temp & 0xF800) >> 11;
                g = (temp & 0x7C0) >> 6;
                b = (temp & 0x3E) >> 1;
                *pixels++ = new YFColor((byte)((r << 3) | (r >> 2)), (byte)((g << 3) | (g >> 2)), (byte)((b << 3) | (b >> 2)), (byte)-(temp & 0x1));
            }
            return image;
        }

        public static int Write(BinaryStream bs, YFBitmap image)
        {
            YFColor* pixels = (YFColor*)image.GetPixels().ToPointer();
            int S = image.Square;
            for (int i = 0; i < S; i++)
            {
                bs.WriteUInt16((ushort)(((pixels->Alpha & 0x80) >> 7) | ((pixels->Blue & 0xF8) >> 2) | ((pixels->Green & 0xF8) << 3) | ((pixels->Red & 0xF8) << 8)));
                pixels++;
            }
            return image.Width << 1;
        }
    }
}
