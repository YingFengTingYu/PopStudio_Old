using SkiaSharp;

namespace PopStudio.Texture
{
    internal static class ETC1_RGB_A8
    {
        public static SKBitmap Read(BinaryStream bs, int width, int height)
        {
            bool t = false;
            int newwidth = width;
            int newheight = height;
            if (newwidth % 4 != 0)
            {
                newwidth += 4 - newwidth % 4;
                t = true;
            }
            if (newheight % 4 != 0)
            {
                newheight += 4 - newheight % 4;
                t = true;
            }
            int S = newwidth * newheight;
            SKColor[] pixels = new SKColor[S];
            ulong temp;
            bool diffbit, flipbit;
            int r1, r2, g1, g2, b1, b2;
            Endian etcendian = bs.Endian == Endian.Small ? Endian.Big : Endian.Small;
            int Table1, Table2, val, add;
            bool neg;
            for (int y = 0; y < newheight; y += 4)
            {
                for (int x = 0; x < newwidth; x += 4)
                {
                    temp = bs.ReadUInt64(etcendian);
                    diffbit = ((temp >> 33) & 1) == 1;
                    flipbit = ((temp >> 32) & 1) == 1;
                    if (diffbit)
                    {
                        int r = (int)((temp >> 59) & 0x1F);
                        int g = (int)((temp >> 51) & 0x1F);
                        int b = (int)((temp >> 43) & 0x1F);
                        r1 = (r << 3) | ((r & 0x1C) >> 2);
                        g1 = (g << 3) | ((g & 0x1C) >> 2);
                        b1 = (b << 3) | ((b & 0x1C) >> 2);
                        r += (int)((temp >> 56) & 0x7) << 29 >> 29;
                        g += (int)((temp >> 48) & 0x7) << 29 >> 29;
                        b += (int)((temp >> 40) & 0x7) << 29 >> 29;
                        r2 = (r << 3) | ((r & 0x1C) >> 2);
                        g2 = (g << 3) | ((g & 0x1C) >> 2);
                        b2 = (b << 3) | ((b & 0x1C) >> 2);
                    }
                    else
                    {
                        r1 = (int)((temp >> 60) & 0xF) * 0x11;
                        g1 = (int)((temp >> 52) & 0xF) * 0x11;
                        b1 = (int)((temp >> 44) & 0xF) * 0x11;
                        r2 = (int)((temp >> 56) & 0xF) * 0x11;
                        g2 = (int)((temp >> 48) & 0xF) * 0x11;
                        b2 = (int)((temp >> 40) & 0xF) * 0x11;
                    }
                    Table1 = (int)((temp >> 37) & 0x7);
                    Table2 = (int)((temp >> 34) & 0x7);
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            val = (int)((temp >> ((j << 2) | i)) & 0x1);
                            neg = ((temp >> (((j << 2) | i) + 16)) & 0x1) == 1;
                            if ((flipbit && i < 2) || (!flipbit && j < 2))
                            {
                                add = ETCEncode.ETC1Modifiers[Table1, val] * (neg ? -1 : 1);
                                pixels[(i + y) * newwidth + x + j] = new SKColor(ETCEncode.ColorClamp(r1 + add), ETCEncode.ColorClamp(g1 + add), ETCEncode.ColorClamp(b1 + add));
                            }
                            else
                            {
                                add = ETCEncode.ETC1Modifiers[Table2, val] * (neg ? -1 : 1);
                                pixels[(i + y) * newwidth + x + j] = new SKColor(ETCEncode.ColorClamp(r2 + add), ETCEncode.ColorClamp(g2 + add), ETCEncode.ColorClamp(b2 + add));
                            }
                        }
                    }
                }
            }
            if (t)
            {
                SKBitmap image1 = new SKBitmap(newwidth, newheight);
                image1.Pixels = pixels;
                SKBitmap image2 = new SKBitmap(width, height);
                using (SKCanvas canvas = new SKCanvas(image2))
                {
                    canvas.DrawBitmap(image1, new SKRect(0, 0, newwidth, newheight));
                }
                pixels = image2.Pixels;
                image1.Dispose();
                image2.Dispose();
            }
            S = pixels.Length;
            for (int i = 0; i < S; i++)
            {
                pixels[i] = pixels[i].WithAlpha(bs.ReadByte());
            }
            SKBitmap image = new SKBitmap(width, height);
            image.Pixels = pixels;
            return image;
        }

        public static int Write(BinaryStream bs, SKBitmap image)
        {
            SKBitmap imagein = image;
            bool t = false;
            int newwidth = image.Width;
            int newheight = image.Height;
            if (newwidth % 4 != 0)
            {
                newwidth += 4 - newwidth % 4;
                t = true;
            }
            if (newheight % 4 != 0)
            {
                newheight += 4 - newheight % 4;
                t = true;
            }
            if (t)
            {
                SKBitmap image2 = new SKBitmap(newwidth, newheight);
                using (SKCanvas canvas = new SKCanvas(image2))
                {
                    canvas.DrawBitmap(image, new SKRect(0, 0, image.Width, image.Height));
                }
                image = image2;
            }
            SKColor[] pixels = image.Pixels;
            SKColor[] color = new SKColor[16];
            Endian etcendian = bs.Endian == Endian.Small ? Endian.Big : Endian.Small;
            for (int i = 0; i < newheight; i += 4)
            {
                for (int w = 0; w < newwidth; w += 4)
                {
                    //Copy color
                    for (int j = 0; j < 4; j++)
                    {
                        for (int k = 0; k < 4; k++)
                        {
                            color[(j << 2) | k] = pixels[(i + j) * newwidth + w + k];
                        }
                    }
                    //Write
                    bs.WriteUInt64(ETCEncode.GenETC1(color), etcendian);
                }
            }
            if (t) pixels = imagein.Pixels;
            int S = pixels.Length;
            for (int i = 0; i < S; i++)
            {
                bs.WriteByte(pixels[i].Alpha);
            }
            if (t)
            {
                image.Dispose();
            }
            return imagein.Width << 2;
        }
    }
}