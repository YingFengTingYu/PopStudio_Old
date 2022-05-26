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
            for (int i = 0; i < S; i++)
            {
                temp = bs.ReadUInt16();
                *pixels++ = new YFColor((byte)((temp & 0xF800) >> 8), (byte)((temp & 0x7E0) >> 3), (byte)((temp & 0x1F) << 3));
            }
            return image;
        }

        public static int Write(BinaryStream bs, YFBitmap image)
        {
            YFColor* pixels = (YFColor*)image.GetPixels().ToPointer();
            int S = image.Square;
            for (int i = 0; i < S; i++)
            {
                bs.WriteUInt16((ushort)(((pixels->Blue & 0xF8) >> 3) | ((pixels->Green & 0xFC) << 3) | ((pixels->Red & 0xF8) << 8)));
                pixels++;
            }
            return image.Width << 1;
        }
    }
}
