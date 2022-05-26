using PopStudio.Platform;

namespace PopStudio.Texture
{
    internal static unsafe class ARGB8888
    {
        public static YFBitmap Read(BinaryStream bs, int width, int height)
        {
            YFBitmap image = YFBitmap.Create(width, height);
            int S = width * height;
            YFColor* pixels = (YFColor*)image.GetPixels().ToPointer();
            uint temp;
            for (int i = 0; i < S; i++)
            {
                temp = bs.ReadUInt32();
                *pixels++ = new YFColor((byte)((temp & 0xFF0000) >> 16), (byte)((temp & 0xFF00) >> 8), (byte)(temp & 0xFF), (byte)(temp >> 24));
            }
            return image;
        }

        public static int Write(BinaryStream bs, YFBitmap image)
        {
            YFColor* pixels = (YFColor*)image.GetPixels().ToPointer();
            int S = image.Square;
            for (int i = 0; i < S; i++)
            {
                bs.WriteUInt32(((uint)pixels->Alpha << 24) | ((uint)pixels->Red << 16) | ((uint)pixels->Green << 8) | pixels->Blue);
                pixels++;
            }
            return image.Width << 2;
        }
    }
}