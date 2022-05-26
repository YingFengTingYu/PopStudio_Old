using PopStudio.Platform;

namespace PopStudio.Texture
{
    internal static unsafe class A8
    {
        public static YFBitmap Read(BinaryStream bs, int width, int height)
        {
            YFBitmap image = YFBitmap.Create(width, height);
            int S = width * height;
            byte* pixels = (byte*)image.GetPixels().ToPointer();
            for (int i = 0; i < S; i++)
            {
                *pixels++ = 255; // b
                *pixels++ = 255; // g
                *pixels++ = 255; // r
                *pixels++ = bs.ReadByte(); // a
            }
            return image;
        }

        public static int Write(BinaryStream bs, YFBitmap image)
        {
            byte* pixels = (byte*)image.GetPixels().ToPointer();
            int S = image.Square;
            pixels += 3;
            for (int i = 0; i < S; i++)
            {
                bs.WriteByte(*pixels);
                pixels += 4;
            }
            return image.Width;
        }
    }
}