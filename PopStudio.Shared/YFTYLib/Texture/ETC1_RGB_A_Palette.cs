using PopStudio.Platform;

namespace PopStudio.Texture
{
    internal static unsafe class ETC1_RGB_A_Palette
    {
        public static YFBitmap Read(BinaryStream bs, int width, int height)
        {
            YFBitmap image = YFBitmap.Create(width, height);
            YFColor* pixels = (YFColor*)image.GetPixels().ToPointer();
            ulong temp;
            bool diffbit, flipbit;
            int r1, r2, g1, g2, b1, b2;
            Endian etcendian = bs.Endian == Endian.Small ? Endian.Big : Endian.Small;
            int Table1, Table2, val, add;
            bool neg;
            for (int y = 0; y < height; y += 4)
            {
                for (int x = 0; x < width; x += 4)
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
                            if ((x + j) < width && (y + i) < height)
                            {
                                val = (int)((temp >> ((j << 2) | i)) & 0x1);
                                neg = ((temp >> (((j << 2) | i) + 16)) & 0x1) == 1;
                                if ((flipbit && i < 2) || (!flipbit && j < 2))
                                {
                                    add = ETCEncode.ETC1Modifiers[Table1, val] * (neg ? -1 : 1);
                                    pixels[(i + y) * width + x + j] = new YFColor(ETCEncode.ColorClamp(r1 + add), ETCEncode.ColorClamp(g1 + add), ETCEncode.ColorClamp(b1 + add));
                                }
                                else
                                {
                                    add = ETCEncode.ETC1Modifiers[Table2, val] * (neg ? -1 : 1);
                                    pixels[(i + y) * width + x + j] = new YFColor(ETCEncode.ColorClamp(r2 + add), ETCEncode.ColorClamp(g2 + add), ETCEncode.ColorClamp(b2 + add));
                                }
                            }
                        }
                    }
                }
            }
            int S = image.Square;
            byte indexNumber = bs.ReadByte();
            int bitsLength;
            byte[] indexTable;
            byte bufferbyte;
            if (indexNumber == 0)
            {
                bitsLength = 1;
                indexTable = new byte[2] { 0, 255 };
            }
            else
            {
                bitsLength = indexNumber == 1 ? 1 : Math.ILogB(indexNumber - 1) + 1;
                indexTable = new byte[indexNumber];
                for (int i = 0; i < indexNumber; i++)
                {
                    bufferbyte = bs.ReadByte();
                    indexTable[i] = (byte)((bufferbyte << 4) | bufferbyte);
                }
            }
            using (BitStream bitstream = new BitStream(bs))
            {
                bitstream.LeaveOpen = true;
                for (int i = 0; i < S; i++)
                {
                    pixels++->Alpha = indexTable[bitstream.ReadBits(bitsLength)];
                }
            }
            return image;
        }

        public static int Write(BinaryStream bs, YFBitmap image, out int AlphaSize)
        {
            YFColor* pixels = (YFColor*)image.GetPixels().ToPointer();
            int width = image.Width;
            int height = image.Height;
            YFColor* color = stackalloc YFColor[16];
            Endian etcendian = bs.Endian == Endian.Small ? Endian.Big : Endian.Small;
            for (int i = 0; i < height; i += 4)
            {
                for (int w = 0; w < width; w += 4)
                {
                    //Copy color
                    for (int j = 0; j < 4; j++)
                    {
                        for (int k = 0; k < 4; k++)
                        {
                            if ((i + j) < height && (w + k) < width)
                            {
                                color[(j << 2) | k] = pixels[(i + j) * width + w + k];
                            }
                            else
                            {
                                color[(j << 2) | k] = YFColor.Empty;
                            }
                        }
                    }
                    //Write
                    bs.WriteUInt64(ETCEncode.GenETC1(color), etcendian);
                }
            }
            int S = image.Square;
            bs.WriteByte(0x10);
            for (byte i = 0; i < 16; i++)
            {
                bs.WriteByte(i);
            }
            bool odd = (S & 0b1) == 1;
            S >>= 1;
            AlphaSize = S + 17;
            for (int i = 0; i < S; i++)
            {
                bs.WriteByte((byte)((pixels[i << 1].Alpha & 0b11110000) | (pixels[(i << 1) | 1].Alpha >> 4)));
            }
            if (odd)
            {
                AlphaSize++;
                bs.WriteByte((byte)((pixels[S << 1].Alpha & 0b11110000)));
            }
            return width << 2;
        }
    }
}
