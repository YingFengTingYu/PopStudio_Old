namespace PopStudio.Plugin
{
    /// <summary>
    /// reference: https://www.researchgate.net/publication/259000525_Real-Time_DXT_Compression
    /// </summary>
    internal unsafe class DXTEncode
    {
        public static ushort ColorTo565(YFColor color)
        {
            return (ushort)(((color.Red >> 3) << 11) | ((color.Green >> 2) << 5) | (color.Blue >> 3));
        }

        public static ushort ColorTo555(YFColor color)
        {
            return (ushort)(((color.Red >> 3) << 10) | ((color.Green >> 3) << 5) | (color.Blue >> 3));
        }

        static void SwapColors(YFColor* c1, YFColor* c2)
        {
            YFColor temp = *c1;
            *c1 = *c2;
            *c2 = temp;
        }

        ////use luminance
        //static int ColorLuminance(SKColor color)
        //{
        //    return color.Red + (color.Green << 1) + color.Blue;
        //}

        //public static void GetMinMaxColorsByLuminance(SKColor[] colorBlock, out SKColor minColor, out SKColor maxColor)
        //{
        //    maxColor = minColor = SKColor.Empty;
        //    int maxLuminance = -1, minLuminance = int.MaxValue;
        //    for (int i = 0; i < 16; i++)
        //    {
        //        int luminance = ColorLuminance(colorBlock[i]);
        //        if (luminance > maxLuminance)
        //        {
        //            maxLuminance = luminance;
        //            maxColor = colorBlock[i];
        //        }
        //        if (luminance < minLuminance)
        //        {
        //            minLuminance = luminance;
        //            minColor = colorBlock[i];
        //        }
        //    }
        //    if (ColorTo565(maxColor) < ColorTo565(minColor))
        //    {
        //        SwapColors(ref minColor, ref maxColor);
        //    }
        //}

        //use euclidean distance
        static int ColorDistance(YFColor c1, YFColor c2)
        {
            return Pow2(c1.Red - c2.Red) + Pow2(c1.Green - c2.Green) + Pow2(c1.Blue - c2.Blue);
        }

        static int Pow2(int input)
        {
            return input * input;
        }

        public static void GetMinMaxColorsByEuclideanDistance(YFColor* colorBlock, YFColor* minColor, YFColor* maxColor)
        {
            *minColor = YFColor.Empty;
            *maxColor = YFColor.Empty;
            int maxDistance = -1;
            for (int i = 0; i < 15; i++)
            {
                for (int j = i + 1; j < 16; j++)
                {
                    int distance = ColorDistance(colorBlock[i], colorBlock[j]);
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        *minColor = colorBlock[i];
                        *maxColor = colorBlock[j];
                    }
                }
            }
            if (ColorTo565(*maxColor) < ColorTo565(*minColor))
            {
                SwapColors(minColor, maxColor);
            }
        }

        public static void GetMinMaxColorsByEuclideanDistanceForDXT1RGBA(YFColor* colorBlock, YFColor* minColor, YFColor* maxColor)
        {
            *minColor = YFColor.Empty;
            *maxColor = YFColor.Empty;
            int maxDistance = -1;
            for (int i = 0; i < 15; i++)
            {
                if (colorBlock[i].Alpha < 0x80) continue;
                for (int j = i + 1; j < 16; j++)
                {
                    if (colorBlock[j].Alpha < 0x80) continue;
                    int distance = ColorDistance(colorBlock[i], colorBlock[j]);
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        *minColor = colorBlock[i];
                        *maxColor = colorBlock[j];
                    }
                }
            }
            if (ColorTo565(*maxColor) < ColorTo565(*minColor))
            {
                SwapColors(minColor, maxColor);
            }
        }

        static int abs(int v)
        {
            if (v < 0) return -v;
            return v;
        }

        public static int EmitColorIndices(YFColor* colorBlock, YFColor minColor, YFColor maxColor)
        {
            int* colors = stackalloc int[16];
            int result = 0;
            colors[0] = (maxColor.Red & 0xF8) | (maxColor.Red >> 5);
            colors[1] = (maxColor.Green & 0xFC) | (maxColor.Green >> 6);
            colors[2] = (maxColor.Blue & 0xF8) | (maxColor.Blue >> 5);
            colors[4] = (minColor.Red & 0xF8) | (minColor.Red >> 5);
            colors[5] = (minColor.Green & 0xFC) | (minColor.Green >> 6);
            colors[6] = (minColor.Blue & 0xF8) | (minColor.Blue >> 5);
            colors[8] = ((colors[0] << 1) + colors[4]) / 3;
            colors[9] = ((colors[1] << 1) + colors[5]) / 3;
            colors[10] = ((colors[2] << 1) + colors[6]) / 3;
            colors[12] = (colors[0] + (colors[4] << 1)) / 3;
            colors[13] = (colors[1] + (colors[5] << 1)) / 3;
            colors[14] = (colors[2] + (colors[6] << 1)) / 3;
            for (int i = 15; i >= 0; i--)
            {
                int c0 = colorBlock[i].Red;
                int c1 = colorBlock[i].Green;
                int c2 = colorBlock[i].Blue;
                int d0 = abs(colors[0] - c0) + abs(colors[1] - c1) + abs(colors[2] - c2);
                int d1 = abs(colors[4] - c0) + abs(colors[5] - c1) + abs(colors[6] - c2);
                int d2 = abs(colors[8] - c0) + abs(colors[9] - c1) + abs(colors[10] - c2);
                int d3 = abs(colors[12] - c0) + abs(colors[13] - c1) + abs(colors[14] - c2);
                int b0 = d0 > d3 ? 1 : 0;
                int b1 = d1 > d2 ? 1 : 0;
                int b2 = d0 > d2 ? 1 : 0;
                int b3 = d1 > d3 ? 1 : 0;
                int b4 = d2 > d3 ? 1 : 0;
                int x0 = b1 & b2;
                int x1 = b0 & b3;
                int x2 = b0 & b4;
                result |= (x2 | ((x0 | x1) << 1)) << (i << 1);
            }
            return result;
        }

        public static int EmitColorIndices_ATC(YFColor* colorBlock, YFColor minColor, YFColor maxColor)
        {
            int* colors = stackalloc int[16];
            int result = 0;
            colors[0] = (maxColor.Red & 0xF8) | (maxColor.Red >> 5);
            colors[1] = (maxColor.Green & 0xF8) | (maxColor.Green >> 5);
            colors[2] = (maxColor.Blue & 0xF8) | (maxColor.Blue >> 5);
            colors[12] = (minColor.Red & 0xF8) | (minColor.Red >> 5);
            colors[13] = (minColor.Green & 0xFC) | (minColor.Green >> 6);
            colors[14] = (minColor.Blue & 0xF8) | (minColor.Blue >> 5);
            colors[4] = ((colors[0] << 1) + colors[12]) / 3;
            colors[5] = ((colors[1] << 1) + colors[13]) / 3;
            colors[6] = ((colors[2] << 1) + colors[14]) / 3;
            colors[8] = (colors[0] + (colors[12] << 1)) / 3;
            colors[9] = (colors[1] + (colors[13] << 1)) / 3;
            colors[10] = (colors[2] + (colors[14] << 1)) / 3;
            for (int i = 15; i >= 0; i--)
            {
                int c0 = colorBlock[i].Red;
                int c1 = colorBlock[i].Green;
                int c2 = colorBlock[i].Blue;
                int d0 = abs(colors[0] - c0) + abs(colors[1] - c1) + abs(colors[2] - c2);
                int d1 = abs(colors[4] - c0) + abs(colors[5] - c1) + abs(colors[6] - c2);
                int d2 = abs(colors[8] - c0) + abs(colors[9] - c1) + abs(colors[10] - c2);
                int d3 = abs(colors[12] - c0) + abs(colors[13] - c1) + abs(colors[14] - c2);
                int b0 = d0 > d3 ? 1 : 0;
                int b1 = d1 > d2 ? 1 : 0;
                int b2 = d0 > d2 ? 1 : 0;
                int b3 = d1 > d3 ? 1 : 0;
                int b4 = d2 > d3 ? 1 : 0;
                int x0 = b1 & b2;
                int x1 = b0 & b3;
                int x2 = b0 & b4;
                result |= (x2 | ((x0 | x1) << 1)) << (i << 1);
            }
            return result;
        }

        public static int EmitColorIndicesForDXT1RGBA(YFColor* colorBlock, YFColor minColor, YFColor maxColor)
        {
            int* colors = stackalloc int[16];
            int result = 0;
            colors[0] = (maxColor.Red & 0xF8) | (maxColor.Red >> 5);
            colors[1] = (maxColor.Green & 0xFC) | (maxColor.Green >> 6);
            colors[2] = (maxColor.Blue & 0xF8) | (maxColor.Blue >> 5);
            colors[4] = (minColor.Red & 0xF8) | (minColor.Red >> 5);
            colors[5] = (minColor.Green & 0xFC) | (minColor.Green >> 6);
            colors[6] = (minColor.Blue & 0xF8) | (minColor.Blue >> 5);
            colors[8] = (colors[0] + colors[4]) >> 1;
            colors[9] = (colors[1] + colors[5]) >> 1;
            colors[10] = (colors[2] + colors[6]) >> 1;
            colors[12] = 0;
            colors[13] = 0;
            colors[14] = 0;
            for (int i = 15; i >= 0; i--)
            {
                if (colorBlock[i].Alpha < 0x80)
                {
                    result |= (0b11) << (i << 1);
                }
                else
                {
                    int c0 = colorBlock[i].Red;
                    int c1 = colorBlock[i].Green;
                    int c2 = colorBlock[i].Blue;
                    int d0 = abs(colors[0] - c0) + abs(colors[1] - c1) + abs(colors[2] - c2);
                    int d1 = abs(colors[4] - c0) + abs(colors[5] - c1) + abs(colors[6] - c2);
                    int d2 = abs(colors[8] - c0) + abs(colors[9] - c1) + abs(colors[10] - c2);
                    if (d0 > d2 && d1 > d2)
                    {
                        result |= (0b10) << (i << 1);
                    }
                    else if (d1 > d0)
                    {
                        result |= (0b01) << (i << 1);
                    }
                }
            }
            colors = null;
            return result;
        }

        public static byte[] EmitAlphaIndices(YFColor* colorBlock, byte minAlpha, byte maxAlpha)
        {
            byte[] indices = new byte[16];
            byte[] alphas = new byte[8];
            alphas[0] = maxAlpha;
            alphas[1] = minAlpha;
            alphas[2] = (byte)((6 * maxAlpha + minAlpha) / 7);
            alphas[3] = (byte)((5 * maxAlpha + (minAlpha << 1)) / 7);
            alphas[4] = (byte)(((maxAlpha << 2) + 3 * minAlpha) / 7);
            alphas[5] = (byte)((3 * maxAlpha + (minAlpha << 2)) / 7);
            alphas[6] = (byte)(((maxAlpha << 1) + 5 * minAlpha) / 7);
            alphas[7] = (byte)((maxAlpha + 6 * minAlpha) / 7);
            for (int i = 0; i < 16; i++)
            {
                int minDistance = int.MaxValue;
                byte a = colorBlock[i].Alpha;
                for (byte j = 0; j < 8; j++)
                {
                    int dist = abs(a - alphas[j]);
                    if (dist < minDistance)
                    {
                        minDistance = dist;
                        indices[i] = j;
                    }
                }
            }
            alphas = null;
            return indices;
        }
    }
}
