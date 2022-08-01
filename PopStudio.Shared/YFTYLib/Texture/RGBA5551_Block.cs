using PopStudio.Platform;

namespace PopStudio.Texture
{
    internal static unsafe class RGBA5551_Block
    {
        public static YFBitmap Read(BinaryStream bs, int width, int height)
        {
            YFBitmap image = YFBitmap.Create(width, height);
            int S = width * height;
            YFColor* pixels = (YFColor*)image.GetPixels().ToPointer();
            ushort temp;
            int r, g, b;
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
                                r = temp >> 11;
                                g = (temp & 0x7C0) >> 6;
                                b = (temp & 0x3E) >> 1;
                                pixels[(i + j) * width + w + k] = new YFColor((byte)((r << 3) | (r >> 2)), (byte)((g << 3) | (g >> 2)), (byte)((b << 3) | (b >> 2)), (byte)-(temp & 0x1));
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
                                bs.WriteUInt16((ushort)(((pixels[temp].Alpha & 0x80) >> 7) | ((pixels[temp].Blue & 0xF8) >> 2) | ((pixels[temp].Green & 0xF8) << 3) | ((pixels[temp].Red & 0xF8) << 8)));
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
