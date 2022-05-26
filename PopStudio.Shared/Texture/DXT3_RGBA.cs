using PopStudio.Platform;

namespace PopStudio.Texture
{
    internal static unsafe class DXT3_RGBA
    {
        public static YFBitmap Read(BinaryStream bs, int width, int height)
        {
            YFBitmap image = YFBitmap.Create(width, height);
            YFColor* pixels = (YFColor*)image.GetPixels().ToPointer();
            YFColor* color = stackalloc YFColor[16];
            ushort* tempa = stackalloc ushort[4];
            ushort* tempc = stackalloc ushort[2];
            YFColor* tempcolor = stackalloc YFColor[4];
            byte* ColorByte = stackalloc byte[4];
            byte* alpha = stackalloc byte[16];
            int temp;
            int r, g, b;
            for (int y = 0; y < height; y += 4)
            {
                for (int x = 0; x < width; x += 4)
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
                    tempcolor[0] = new YFColor((byte)(r << 3 | r >> 2), (byte)(g << 2 | g >> 3), (byte)(b << 3 | b >> 2));
                    b = tempc[1] & 0x1F;
                    g = (tempc[1] & 0x7E0) >> 5;
                    r = (tempc[1] & 0xF800) >> 11;
                    tempcolor[1] = new YFColor((byte)(r << 3 | r >> 2), (byte)(g << 2 | g >> 3), (byte)(b << 3 | b >> 2));
                    tempcolor[2] = new YFColor((byte)(((tempcolor[0].Red << 1) + tempcolor[1].Red + 1) / 3), (byte)(((tempcolor[0].Green << 1) + tempcolor[1].Green + 1) / 3), (byte)(((tempcolor[0].Blue << 1) + tempcolor[1].Blue + 1) / 3));
                    tempcolor[3] = new YFColor((byte)((tempcolor[0].Red + (tempcolor[1].Red << 1) + 1) / 3), (byte)((tempcolor[0].Green + (tempcolor[1].Green << 1) + 1) / 3), (byte)((tempcolor[0].Blue + (tempcolor[1].Blue << 1) + 1) / 3));
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            int k = (i << 2) | j;
                            int bb = ColorByte[i] & 0b11;
                            color[k] = new YFColor(tempcolor[bb].Red, tempcolor[bb].Green, tempcolor[bb].Blue, alpha[k]);
                            ColorByte[i] >>= 2;
                        }
                    }
                    //赋值
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if ((x + j) < width && (y + i) < height)
                            {
                                pixels[(i + y) * width + x + j] = color[(i << 2) | j];
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
            ushort temp;
            YFColor* color = stackalloc YFColor[16];
            YFColor min, max;
            int result;
            for (int i = 0; i < height; i += 4)
            {
                for (int w = 0; w < width; w += 4)
                {
                    //Copy color
                    for (int j = 0; j < 4; j++)
                    {
                        temp = 0;
                        for (int k = 0; k < 4; k++)
                        {
                            if ((i + j) < height && (w + k) < width)
                            {
                                int n = (j << 2) | k;
                                color[n] = pixels[(i + j) * width + w + k];
                                temp |= (ushort)((color[n].Alpha >> 4) << (k << 2));
                            }
                            else
                            {
                                color[(j << 2) | k] = YFColor.Empty;
                            }
                        }
                        bs.WriteUInt16(temp);
                    }
                    //Color code
                    DXTEncode.GetMinMaxColorsByEuclideanDistance(color, &min, &max);
                    result = DXTEncode.EmitColorIndices(color, min, max);
                    //Write
                    bs.WriteUInt16(DXTEncode.ColorTo565(max));
                    bs.WriteUInt16(DXTEncode.ColorTo565(min));
                    bs.WriteUInt16((ushort)(result & 0xFFFF));
                    bs.WriteUInt16((ushort)(result >> 16));
                }
            }
            return width;
        }
    }
}
