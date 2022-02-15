using SkiaSharp;

namespace PopStudio.Texture
{
    internal static class DXT3
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
            SKColor[] color = new SKColor[16];
            ushort[] tempa = new ushort[4];
            ushort[] tempc = new ushort[2];
            SKColor[] tempcolor = new SKColor[4];
            byte[] ColorByte = new byte[4];
            byte[] alpha = new byte[16];
            int temp;
            int r, g, b;
            for (int y = 0; y < newheight; y += 4)
            {
                for (int x = 0; x < newwidth; x += 4)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        tempa[i] = bs.ReadUInt16();
                    }
                    for (int j = 0; j < 4; j++)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            temp = tempa[j] & 0xF;
                            alpha[(j << 2) | i] = (byte)((temp << 4) | temp);
                            tempa[j] >>= 4;
                        }
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
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            pixels[(i + y) * newwidth + x + j] = color[(i << 2) | j];
                        }
                    }
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
