using PopStudio.Platform;

namespace PopStudio.Texture
{
    internal static unsafe class L8
    {
        public static YFBitmap Read(BinaryStream bs, int width, int height)
        {
            YFBitmap image = YFBitmap.Create(width, height);
            int S = width * height;
            byte* pixels = (byte*)image.GetPixels().ToPointer();
            byte l;
            for (int i = 0; i < S; i++)
            {
                l = bs.ReadByte();
                *pixels++ = l;
                *pixels++ = l;
                *pixels++ = l;
                *pixels++ = 255;
            }
            return image;
        }

        public static int Write(BinaryStream bs, YFBitmap image)
        {
            YFColor* pixels = (YFColor*)image.GetPixels().ToPointer();
            int S = image.Square;
            for (int i = 0; i < S; i++)
            {
                bs.WriteByte(Max(pixels->Red * 0.299 + pixels->Green * 0.587 + pixels->Blue * 0.114));
                pixels++;
            }
            return image.Width;
        }

        static byte Max(double a)
        {
            int k = (int)a;
            if (k >= 255) return 255;
            return (byte)k;
        }
    }
}