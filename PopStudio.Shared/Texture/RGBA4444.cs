using PopStudio.Platform;

namespace PopStudio.Texture
{
    internal static unsafe class RGBA4444
    {
        public static YFBitmap Read(BinaryStream bs, int width, int height)
        {
            YFBitmap image = YFBitmap.Create(width, height);
            int S = width * height;
            YFColor* pixels = (YFColor*)image.GetPixels().ToPointer();
            ushort temp;
            int r, g, b, a;
            for (int i = 0; i < S; i++)
            {
                temp = bs.ReadUInt16();
                r = temp >> 12;
                g = (temp & 0xF00) >> 8;
                b = (temp & 0xF0) >> 4;
                a = temp & 0xF;
                *pixels++ = new YFColor((byte)((r << 4) | r), (byte)((g << 4) | g), (byte)((b << 4) | b), (byte)((a << 4) | a));
            }
            return image;
        }

        public static int Write(BinaryStream bs, YFBitmap image)
        {
            YFColor* pixels = (YFColor*)image.GetPixels().ToPointer();
            int S = image.Square;
            for (int i = 0; i < S; i++)
            {
                bs.WriteUInt16((ushort)((pixels->Alpha >> 4) | (pixels->Blue & 0xF0) | ((pixels->Green & 0xF0) << 4) | ((pixels->Red & 0xF0) << 8)));
                pixels++;
            }
            return image.Width << 1;
        }
    }
}