using SkiaSharp;

namespace PopStudio.Texture
{
    internal static class ETC1_RGB_A_Palette
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
            if ((newwidth & (newwidth - 1)) != 0)
            {
                newwidth = 0b10 << ((int)Math.Floor(Math.Log2(newwidth)));
                t = true;
            }
            if ((newheight & (newheight - 1)) != 0)
            {
                newheight = 0b10 << ((int)Math.Floor(Math.Log2(newheight)));
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
                    pixels[i] = pixels[i].WithAlpha((byte)(((temp2 >> ind) & 0b1) == 0 ? 0 : 255)); //2022.2.17 fix this bug
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

        public static int Write(BinaryStream bs, SKBitmap image, out int AlphaSize)
        {
            bool t = false;
            int newwidth = image.Width;
            int newheight = image.Height;
            if ((newwidth & (newwidth - 1)) != 0)
            {
                newwidth = 0b10 << ((int)Math.Floor(Math.Log2(newwidth)));
                t = true;
            }
            if ((newheight & (newheight - 1)) != 0)
            {
                newheight = 0b10 << ((int)Math.Floor(Math.Log2(newheight)));
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
            int S = pixels.Length;
            List<byte> ary = Palette.GeneratePalette_A8(pixels, 16);
            ary.Sort();
            if ((ary[13] <= 13 && ary[14] >= 245) || (ary[1] <= 10 && ary[2] >= 242))
            {
                AlphaSize = (S >> 3) + 1;
                bs.WriteByte(0);
                int flags;
                for (int i = 0; i < S; i += 8)
                {
                    flags = 0;
                    for (int j = 0; j < 8; j++)
                    {
                        flags |= ((pixels[i | j].Alpha & 0b10000000) >> 7) << j;
                    }
                    bs.WriteByte((byte)flags);
                }
            }
            else
            {
                AlphaSize = (S >> 1) + 17;
                bs.WriteByte(0x10);
                for (int i = 0; i < 16; i++)
                {
                    bs.WriteByte(ary[i]);
                }
                int flags;
                for (int i = 0; i < S; i += 2)
                {
                    flags = 0;
                    for (int j = 0; j < 2; j++)
                    {
                        int iorj = i | j;
                        int delta = pixels[iorj].Alpha;
                        if (delta == ary[0])
                        {
                            continue;
                        }
                        else if (delta == ary[15])
                        {
                            flags |= 240 >> (j << 2);
                            continue;
                        }
                        for (int k = 1; k < 16; k++)
                        {
                            int d = pixels[iorj].Alpha - ary[k];
                            if (d <= 0)
                            {
                                if (delta + d >= 0)
                                {
                                    flags |= (k << 4) >> (j << 2);
                                }
                                else
                                {
                                    flags |= ((k - 1) << 4) >> (j << 2);
                                }
                                break;
                            }
                            delta = d;
                        }
                    }
                    bs.WriteByte((byte)flags);
                }
            }
            //If you really need fast encoding, you can use A4 "compress"
            //AlphaSize = (S >> 1) + 17;
            //bs.WriteByte(0x10);
            //for (int i = 0; i < 16; i++)
            //{
            //    bs.WriteByte((byte)(i | (i << 4)));
            //}
            //for (int i = 0; i < S; i += 2)
            //{
            //    bs.WriteByte((byte)((pixels[i].Alpha << 4) | pixels[i | 1].Alpha));
            //}
            if (t)
            {
                image.Dispose();
            }
            return newwidth << 2;
        }
    }
}
