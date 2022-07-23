using PopStudio.Platform;

namespace PopStudio.Texture
{
    internal static unsafe class XBGR8888_A8
    {
        public static YFBitmap Read(BinaryStream bs, int width, int height)
        {
            YFBitmap image = YFBitmap.Create(width, height);
            int S = width * height;
            YFColor* pixels = (YFColor*)image.GetPixels().ToPointer();
            YFColor* pixels_bak = pixels;
            uint temp;
            for (int i = 0; i < S; i++)
            {
                temp = bs.ReadUInt32();
                *pixels++ = new YFColor((byte)(temp & 0xFF), (byte)((temp & 0xFF00) >> 8), (byte)((temp & 0xFF0000) >> 16));
            }
            for (int i = 0; i < S; i++)
            {
                pixels_bak++->Alpha = bs.ReadByte();
            }
            return image;
        }

        public static int Write(BinaryStream bs, YFBitmap image)
        {
            YFColor* pixels = (YFColor*)image.GetPixels().ToPointer();
            YFColor* pixels_bak = pixels;
            int S = image.Square;
            for (int i = 0; i < S; i++)
            {
                bs.WriteUInt32(0xFF000000 | ((uint)pixels->Blue << 16) | ((uint)pixels->Green << 8) | pixels->Red);
                pixels++;
            }
            for (int i = 0; i < S; i++)
            {
                bs.WriteByte(pixels_bak->Alpha);
                pixels_bak++;
            }
            return image.Width << 3;
        }
    }
}