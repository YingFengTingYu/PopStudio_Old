using SkiaSharp;

namespace PopStudio.Texture
{
    internal static class ETC1AIndex
    {
        private static readonly int[,] ETC1Modifiers =
        {
            { 2, 8 },
            { 5, 17 },
            { 9, 29 },
            { 13, 42 },
            { 18, 60 },
            { 24, 80 },
            { 33, 106 },
            { 47, 183 }
        };

        private static byte ColorClamp(int Color) //加颜色可能加出来超过结果的
        {
            if (Color > 255) return 255;
            if (Color < 0) return 0;
            return (byte)Color;
        }

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
                                add = ETC1Modifiers[Table1, val] * (neg ? -1 : 1);
                                pixels[(i + y) * newwidth + x + j] = new SKColor(ColorClamp(r1 + add), ColorClamp(g1 + add), ColorClamp(b1 + add));
                            }
                            else
                            {
                                add = ETC1Modifiers[Table2, val] * (neg ? -1 : 1);
                                pixels[(i + y) * newwidth + x + j] = new SKColor(ColorClamp(r2 + add), ColorClamp(g2 + add), ColorClamp(b2 + add));
                            }
                        }
                    }
                }
            }
            byte indexNumber = bs.ReadByte();
            if (indexNumber == 0)
            {
                byte temp2 = 0;
                int ind = 0;
                for (int i = 0; i < S; i++)
                {
                    if (ind == 0) temp2 = bs.ReadByte();
                    pixels[i] = (byte)(((temp2 >> ind) & 0b1) == 0 ? 0 : 255);
                    ind = (ind + 1) % 8;
                }
            }
            else
            {
                byte[] table = bs.ReadBytes(indexNumber);
                byte temp2 = 0;
                byte temp3;
                int ind = 4;
                for (int i = 0; i < S; i++)
                {
                    if (ind == 4) temp2 = bs.ReadByte();
                    temp3 = table[(temp2 >> ind) & 0b1111];
                    pixels[i] = pixels[i].WithAlpha((byte)((temp3 << 4) | temp3));
                    ind = (ind + 4) % 8;
                }
            }
            SKBitmap image = new SKBitmap(newwidth, newheight);
            image.Pixels = pixels;
            if (t)
            {
                SKBitmap image2 = new SKBitmap(width, height);
                using (SKCanvas canvas = new SKCanvas(image2))
                {
                    canvas.DrawBitmap(image, new SKRect(0, 0, newwidth, newheight));
                }
                image.Dispose();
                return image2;
            }
            return image;
        }
    }
}
