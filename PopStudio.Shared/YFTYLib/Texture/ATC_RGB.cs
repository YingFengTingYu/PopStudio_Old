using PopStudio.Platform;

namespace PopStudio.Texture
{
    internal static unsafe class ATC_RGB
    {
        public static YFBitmap Read(BinaryStream bs, int width, int height)
        {
            YFBitmap image = YFBitmap.Create(width, height);
            YFColor* pixels = (YFColor*)image.GetPixels().ToPointer();
            YFColor* color = stackalloc YFColor[16];
            YFColor* color_buffer = stackalloc YFColor[4];
            for (int y = 0; y < height; y += 4)
            {
                for (int x = 0; x < width; x += 4)
                {
                    ushort color0 = 0;
                    color0 |= bs.ReadByte();
                    color0 |= (ushort)(bs.ReadByte() << 8);
                    ushort color1 = 0;
                    color1 |= bs.ReadByte();
                    color1 |= (ushort)(bs.ReadByte() << 8);
                    bool mode = (color0 & 0x8000) != 0;
                    if (mode)
                    {
                        int r = (color0 >> 10) & 0x1F;
                        int g = (color0 >> 5) & 0x1F;
                        int b = color0 & 0x1F;
                        color_buffer[2].Red = (byte)((r << 3) | (r >> 2));
                        color_buffer[2].Green = (byte)((g << 3) | (g >> 2));
                        color_buffer[2].Blue = (byte)((b << 3) | (b >> 2));
                        color_buffer[2].Alpha = 0xFF;

                        r = color1 >> 11;
                        g = (color1 >> 5) & 0x3F;
                        b = color1 & 0x1F;
                        color_buffer[3].Red = (byte)((r << 3) | (r >> 2));
                        color_buffer[3].Green = (byte)((g << 2) | (g >> 4));
                        color_buffer[3].Blue = (byte)((b << 3) | (b >> 2));
                        color_buffer[3].Alpha = 0xFF;

                        color_buffer[0].Red = 0x0;
                        color_buffer[0].Green = 0x0;
                        color_buffer[0].Blue = 0x0;
                        color_buffer[0].Alpha = 0xFF;

                        color_buffer[1].Red = (byte)(Math.Abs((color_buffer[2].Red << 2) - color_buffer[3].Red) >> 2);
                        color_buffer[1].Green = (byte)(Math.Abs((color_buffer[2].Green << 2) - color_buffer[3].Green) >> 2);
                        color_buffer[1].Blue = (byte)(Math.Abs((color_buffer[2].Blue << 2) - color_buffer[3].Blue) >> 2);
                        color_buffer[1].Alpha = 0xFF;
                    }
                    else
                    {
                        int r = (color0 >> 10) & 0x1F;
                        int g = (color0 >> 5) & 0x1F;
                        int b = color0 & 0x1F;
                        color_buffer[0].Red = (byte)((r << 3) | (r >> 2));
                        color_buffer[0].Green = (byte)((g << 3) | (g >> 2));
                        color_buffer[0].Blue = (byte)((b << 3) | (b >> 2));
                        color_buffer[0].Alpha = 0xFF;

                        r = color1 >> 11;
                        g = (color1 >> 5) & 0x3F;
                        b = color1 & 0x1F;
                        color_buffer[3].Red = (byte)((r << 3) | (r >> 2));
                        color_buffer[3].Green = (byte)((g << 2) | (g >> 4));
                        color_buffer[3].Blue = (byte)((b << 3) | (b >> 2));
                        color_buffer[3].Alpha = 0xFF;

                        color_buffer[1].Red = (byte)((color_buffer[0].Red * 5 + color_buffer[3].Red * 3 + 4) >> 3);
                        color_buffer[1].Green = (byte)((color_buffer[0].Green * 5 + color_buffer[3].Green * 3 + 4) >> 3);
                        color_buffer[1].Blue = (byte)((color_buffer[0].Blue * 5 + color_buffer[3].Blue * 3 + 4) >> 3);
                        color_buffer[1].Alpha = 0xFF;

                        color_buffer[2].Red = (byte)((color_buffer[0].Red * 3 + color_buffer[3].Red * 5 + 4) >> 3);
                        color_buffer[2].Green = (byte)((color_buffer[0].Green * 3 + color_buffer[3].Green * 5 + 4) >> 3);
                        color_buffer[2].Blue = (byte)((color_buffer[0].Blue * 3 + color_buffer[3].Blue * 5 + 4) >> 3);
                        color_buffer[2].Alpha = 0xFF;
                    }

                    uint colorFlags = 0;
                    colorFlags |= bs.ReadByte();
                    colorFlags |= (uint)(bs.ReadByte() << 8);
                    colorFlags |= (uint)(bs.ReadByte() << 16);
                    colorFlags |= (uint)(bs.ReadByte() << 24);
                    for (int i = 0; i < 16; i++)
                    {
                        color[i] = color_buffer[colorFlags & 0b11];
                        colorFlags >>= 2;
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
                    //Color code
                    DXTEncode.GetMinMaxColorsByEuclideanDistance(color, &min, &max);
                    result = DXTEncode.EmitColorIndices_ATC(color, min, max);
                    //Write
                    uint buffer = DXTEncode.ColorTo555(max);
                    bs.WriteByte((byte)buffer);
                    bs.WriteByte((byte)(buffer >> 8));
                    buffer = DXTEncode.ColorTo565(min);
                    bs.WriteByte((byte)buffer);
                    bs.WriteByte((byte)(buffer >> 8));
                    bs.WriteByte((byte)result);
                    bs.WriteByte((byte)(result >> 8));
                    bs.WriteByte((byte)(result >> 16));
                    bs.WriteByte((byte)(result >> 24));
                }
            }
            return width >> 1;
        }
    }
}
