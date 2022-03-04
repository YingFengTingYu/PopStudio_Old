using SkiaSharp;

namespace PopStudio.Texture
{
    internal static class DXT5_RGBA_Morton
    {
        static readonly int[] Order = { 0, 2, 8, 10, 1, 3, 9, 11, 4, 6, 12, 14, 5, 7, 13, 15 };

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
            SKColor[] color = new SKColor[16];
            int[] tempa = new int[2];
            int[] tempalpha = new int[8];
            long AlphaUInt48;
            ushort[] tempc = new ushort[2];
            SKColor[] tempcolor = new SKColor[4];
            byte[] ColorByte = new byte[4];
            byte[] alpha = new byte[16];
            int temp;
            int r, g, b;
            int pixelOffset = 0;
            int minwh = newwidth > newheight ? newheight : newwidth;
            int mink = (int)Math.Log(minwh, 2);
            bool bigwidth = newwidth > newheight;
            for (int y = 0; y < newheight; y += 4)
            {
                for (int x = 0; x < newwidth; x += 4)
                {
                    temp = bs.ReadUInt16();
                    tempa[0] = temp & 0xFF;
                    tempa[1] = temp >> 8;
                    AlphaUInt48 = bs.ReadUInt16() | (((long)bs.ReadUInt16()) << 16) | (((long)bs.ReadUInt16()) << 32);
                    //计算alpha值
                    if (tempa[0] > tempa[1])
                    {
                        tempalpha[0] = tempa[0];
                        tempalpha[1] = tempa[1];
                        tempalpha[2] = (6 * tempa[0] + tempa[1]) / 7;
                        tempalpha[3] = (5 * tempa[0] + (tempa[1] << 1)) / 7;
                        tempalpha[4] = ((tempa[0] << 2) + 3 * tempa[1]) / 7;
                        tempalpha[5] = (3 * tempa[0] + (tempa[1] << 2)) / 7;
                        tempalpha[6] = ((tempa[0] << 1) + 5 * tempa[1]) / 7;
                        tempalpha[7] = (tempa[0] + 6 * tempa[1]) / 7;
                    }
                    else
                    {
                        tempalpha[0] = tempa[0];
                        tempalpha[1] = tempa[1];
                        tempalpha[2] = ((tempa[0] << 2) + tempa[1]) / 5;
                        tempalpha[3] = (3 * tempa[0] + (tempa[1] << 1)) / 5;
                        tempalpha[4] = ((tempa[0] << 1) + 3 * tempa[1]) / 5;
                        tempalpha[5] = (tempa[0] + (tempa[1] << 2)) / 5;
                        tempalpha[6] = 0;
                        tempalpha[7] = 255;
                    }
                    for (int i = 0; i < 16; i++)
                    {
                        alpha[i] = (byte)tempalpha[AlphaUInt48 & 0b111];
                        AlphaUInt48 >>= 3;
                    }
                    //计算color值
                    tempc[0] = bs.ReadUInt16();
                    tempc[1] = bs.ReadUInt16();
                    temp = bs.ReadUInt16();
                    ColorByte[0] = (byte)(temp & 0xFF);
                    ColorByte[1] = (byte)(temp >> 8);
                    temp = bs.ReadUInt16();
                    ColorByte[2] = (byte)(temp & 0xFF);
                    ColorByte[3] = (byte)(temp >> 8);
                    //计算color值
                    b = tempc[0] & 0x1F;
                    g = (tempc[0] & 0x7E0) >> 5;
                    r = (tempc[0] & 0xF800) >> 11;
                    tempcolor[0] = new SKColor((byte)(r << 3 | r >> 2), (byte)(g << 2 | g >> 3), (byte)(b << 3 | b >> 2));
                    b = tempc[1] & 0x1F;
                    g = (tempc[1] & 0x7E0) >> 5;
                    r = (tempc[1] & 0xF800) >> 11;
                    tempcolor[1] = new SKColor((byte)(r << 3 | r >> 2), (byte)(g << 2 | g >> 3), (byte)(b << 3 | b >> 2));
                    tempcolor[2] = new SKColor((byte)(((tempcolor[0].Red << 1) + tempcolor[1].Red + 1) / 3), (byte)(((tempcolor[0].Green << 1) + tempcolor[1].Green + 1) / 3), (byte)(((tempcolor[0].Blue << 1) + tempcolor[1].Blue + 1) / 3));
                    tempcolor[3] = new SKColor((byte)((tempcolor[0].Red + (tempcolor[1].Red << 1) + 1) / 3), (byte)((tempcolor[0].Green + (tempcolor[1].Green << 1) + 1) / 3), (byte)((tempcolor[0].Blue + (tempcolor[1].Blue << 1) + 1) / 3));
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            int k = (i << 2) | j;
                            int bb = ColorByte[i] & 0b11;
                            color[k] = new SKColor(tempcolor[bb].Red, tempcolor[bb].Green, tempcolor[bb].Blue, alpha[k]);
                            ColorByte[i] >>= 2;
                        }
                    }
                    //赋值
                    for (int i = 0; i < 16; i++)
                    {
                        pixels[GetIndex(pixelOffset + Order[i], minwh, mink, bigwidth, newwidth)] = color[i];
                    }
                    pixelOffset += 16;
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

        static int GetIndex(int i, int min, int k, bool bw, int width)
        {
            int x, y;
            int mx = 0, my = 0;
            for (int j = 0; j < 16; j++)
            {
                mx |= (i & (1 << (j << 1))) >> j;
                my |= (i & ((1 << (j << 1)) << 1)) >> j;
            }
            my >>= 1;
            if (bw)
            {
                int j = i >> (2 * k) << (2 * k) | (my & (min - 1)) << k | (mx & (min - 1)) << 0;
                x = j / min;
                y = j % min;
            }
            else
            {
                int j = i >> (2 * k) << (2 * k) | (mx & (min - 1)) << k | (my & (min - 1)) << 0;
                x = j % min;
                y = j / min;
            }
            return (y * width) + x;
        }

        static byte C(int v)
        {
            if (v >= 255) return 255;
            if (v <= 0) return 0;
            return (byte)v;
        }

        public static int Write(BinaryStream bs, SKBitmap image)
        {
            int ans = image.Width;
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
            ushort[] temp = new ushort[4];
            SKColor[] color = new SKColor[16];
            byte maxalpha, minalpha;
            SKColor min, max;
            int result;
            int tempvalue;
            int pixelOffset = 0;
            int minwh = newwidth > newheight ? newheight : newwidth;
            int mink = (int)Math.Log(minwh, 2);
            bool bigwidth = newwidth > newheight;
            for (int i = 0; i < newheight; i += 4)
            {
                for (int w = 0; w < newwidth; w += 4)
                {
                    maxalpha = 0;
                    minalpha = 255;
                    //Copy color
                    for (int n = 0; n < 16; n++)
                    {
                        color[n] = pixels[GetIndex(pixelOffset + Order[n], minwh, mink, bigwidth, newwidth)];
                        byte a = color[n].Alpha;
                        if (a > maxalpha) maxalpha = a;
                        if (a < minalpha) minalpha = a;
                    }
                    pixelOffset += 16;
                    //Alpha code, only use a1 > a2 mode
                    if (minalpha == maxalpha)
                    {
                        temp[0] = (ushort)((minalpha << 8) | maxalpha);
                        temp[1] = 0;
                        temp[2] = 0;
                        temp[3] = 0;
                    }
                    else
                    {
                        tempvalue = (maxalpha - minalpha) >> 4;
                        maxalpha = C(maxalpha - tempvalue);
                        minalpha = C(minalpha + tempvalue);
                        temp[0] = (ushort)((minalpha << 8) | maxalpha);
                        byte[] alphabytes = DXTEncode.EmitAlphaIndices(color, minalpha, maxalpha);
                        long flag = 0;
                        int pos = 0;
                        for (int ii = 0; ii < 16; ii++)
                        {
                            flag |= ((long)alphabytes[ii]) << pos;
                            pos += 3;
                        }
                        temp[1] = (ushort)(flag & 0xFFFF);
                        temp[2] = (ushort)((flag >> 16) & 0xFFFF);
                        temp[3] = (ushort)(flag >> 32);
                    }
                    for (int ii = 0; ii < 4; ii++)
                    {
                        bs.WriteUInt16(temp[ii]);
                    }
                    //Color code
                    DXTEncode.GetMinMaxColorsByEuclideanDistance(color, out min, out max);
                    result = DXTEncode.EmitColorIndices(color, min, max);
                    //Write
                    bs.WriteUInt16(DXTEncode.ColorTo565(max));
                    bs.WriteUInt16(DXTEncode.ColorTo565(min));
                    bs.WriteUInt16((ushort)(result & 0xFFFF));
                    bs.WriteUInt16((ushort)(result >> 16));
                }
            }
            if (t)
            {
                image.Dispose();
            }
            return ans;
        }
    }
}
