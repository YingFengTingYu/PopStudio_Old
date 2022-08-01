using PopStudio.Platform;

namespace PopStudio.Texture
{
    internal static unsafe class RGBA4444_Block
    {
        public static YFBitmap Read(BinaryStream bs, int width, int height)
        {
            YFBitmap image = YFBitmap.Create(width, height);
            int S = width * height;
            YFColor* pixels = (YFColor*)image.GetPixels().ToPointer();
            ushort temp;
            int r, g, b, a;
            for (int i = 0; i < height; i += 32)
            {
                for (int w = 0; w < width; w += 32)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        for (int k = 0; k < 32; k++)
                        {
                            temp = bs.ReadUInt16();
                            if ((i + j) < height && (w + k) < width)
                            {
                                r = temp >> 12;
                                g = (temp & 0xF00) >> 8;
                                b = (temp & 0xF0) >> 4;
                                a = temp & 0xF;
                                pixels[(i + j) * width + w + k] = new YFColor((byte)((r << 4) | r), (byte)((g << 4) | g), (byte)((b << 4) | b), (byte)((a << 4) | a));
                            }
                        }
                    }
                }
            }
            return image;
        }

        public static int Write(BinaryStream bs, YFBitmap image)
        {
            YFColor* pixels = (YFColor*)image.GetPixels().ToPointer();
            int width = image.Width;
            int height = image.Height;
            int newwidth = width;
            if ((newwidth & 31) != 0)
            {
                newwidth |= 31;
                newwidth++;
            }
            int temp;
            for (int i = 0; i < height; i += 32)
            {
                for (int w = 0; w < width; w += 32)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        for (int k = 0; k < 32; k++)
                        {
                            if ((i + j) < height && (w + k) < width)
                            {
                                temp = (i + j) * width + w + k;
                                bs.WriteUInt16((ushort)((pixels[temp].Alpha >> 4) | (pixels[temp].Blue & 0xF0) | ((pixels[temp].Green & 0xF0) << 4) | ((pixels[temp].Red & 0xF0) << 8)));
                            }
                            else
                            {
                                bs.WriteUInt16(0);
                            }
                        }
                    }
                }
            }
            return newwidth << 1;
        }
    }
}
