using PopStudio.Platform;

namespace PopStudio.Texture
{
    internal static unsafe class ARGB1555
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
                r = (temp & 0x7C00) >> 10;
                g = (temp & 0x3E0) >> 5;
                b = temp & 0x1F;
                *pixels++ = new YFColor((byte)((r << 3) | (r >> 2)), (byte)((g << 3) | (g >> 2)), (byte)((b << 3) | (b >> 2)), (byte)-(temp >> 15));
            }
            return image;
        }

        public static int Write(BinaryStream bs, YFBitmap image)
        {
            YFColor* pixels = (YFColor*)image.GetPixels().ToPointer();
            int S = image.Square;
            for (int i = 0; i < S; i++)
            {
                bs.WriteUInt16((ushort)(((pixels->Alpha & 0x80) << 8) | (pixels->Blue >> 3) | ((pixels->Green & 0xF8) << 2) | ((pixels->Red & 0xF8) << 7)));
                pixels++;
            }
            return image.Width << 1;
        }
    }
}
