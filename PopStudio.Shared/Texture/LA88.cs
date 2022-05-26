using PopStudio.Platform;

namespace PopStudio.Texture
{
    internal static unsafe class LA88
    {
        public static YFBitmap Read(BinaryStream bs, int width, int height)
        {
            YFBitmap image = YFBitmap.Create(width, height);
            int S = width * height;
            byte* pixels = (byte*)image.GetPixels().ToPointer();
            byte l;
            int temp;
            for (int i = 0; i < S; i++)
            {
                temp = bs.ReadUInt16();
                l = (byte)(temp >> 8);
                *pixels++ = l;
                *pixels++ = l;
                *pixels++ = l;
                *pixels++ = (byte)(temp & 0xFF);
            }
            return image;
        }

        public static int Write(BinaryStream bs, YFBitmap image)
        {
            YFColor* pixels = (YFColor*)image.GetPixels().ToPointer();
            int S = image.Square;
            for (int i = 0; i < S; i++)
            {
                bs.WriteUInt16((ushort)(((Max(pixels[i].Red * 0.299 + pixels[i].Green * 0.587 + pixels[i].Blue * 0.114)) << 8) | pixels[i].Alpha));
            }
            return image.Width << 1;
        }

        static byte Max(double a)
        {
            int k = (int)a;
            if (k >= 255) return 255;
            return (byte)k;
        }
    }
}