using PopStudio.Platform;

namespace PopStudio.Texture
{
    internal static unsafe class LA44
    {
        public static YFBitmap Read(BinaryStream bs, int width, int height)
        {
            YFBitmap image = YFBitmap.Create(width, height);
            int S = width * height;
            byte* pixels = (byte*)image.GetPixels().ToPointer();
            int temp, a;
            byte l;
            for (int i = 0; i < S; i++)
            {
                temp = bs.ReadByte();
                a = temp & 0xF;
                temp >>= 4;
                l = (byte)(temp | (temp << 4));
                *pixels++ = l;
                *pixels++ = l;
                *pixels++ = l;
                *pixels++ = (byte)(a | (a << 4));
            }
            return image;
        }

        public static int Write(BinaryStream bs, YFBitmap image)
        {
            YFColor* pixels = (YFColor*)image.GetPixels().ToPointer();
            int S = image.Square;
            for (int i = 0; i < S; i++)
            {
                bs.WriteByte((byte)(((Max(pixels->Red * 0.299 + pixels->Green * 0.587 + pixels->Blue * 0.114)) & 0xF0) | ((pixels->Alpha) >> 4)));
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