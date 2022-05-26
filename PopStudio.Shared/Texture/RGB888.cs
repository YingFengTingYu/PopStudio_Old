using PopStudio.Platform;

namespace PopStudio.Texture
{
    internal static unsafe class RGB888
    {
        public static YFBitmap Read(BinaryStream bs, int width, int height)
        {
            YFBitmap image = YFBitmap.Create(width, height);
            int S = width * height;
            YFColor* pixels = (YFColor*)image.GetPixels().ToPointer();
            uint temp;
            for (int i = 0; i < S; i++)
            {
                temp = bs.ReadUInt24();
                *pixels++ = new YFColor((byte)(temp >> 16), (byte)((temp & 0xFF00) >> 8), (byte)(temp & 0xFF));
            }
            return image;
        }

        public static int Write(BinaryStream bs, YFBitmap image)
        {
            YFColor* pixels = (YFColor*)image.GetPixels().ToPointer();
            int S = image.Square;
            for (int i = 0; i < S; i++)
            {
                bs.WriteUInt24((uint)((pixels[i].Red << 16) | (pixels[i].Green << 8) | pixels[i].Blue));
            }
            return image.Width * 3;
        }
    }
}