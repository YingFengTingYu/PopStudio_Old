using SkiaSharp;

namespace PopStudio.Plugin
{
    internal static class SKPlugin
    {
        public static void Save(this SKBitmap pic, string outFile)
        {
            using (SKData p = pic.Encode(SKEncodedImageFormat.Png, 100))
            {
                using (FileStream fs = new FileStream(outFile, FileMode.Create))
                {
                    byte[] t = p.ToArray();
                    fs.Write(t, 0, t.Length);
                    t = null;
                }
            }
        }

        public static SKBitmap Rotate270(this SKBitmap img)
        {
            int resH = img.Height;
            int resW = img.Width;
            SKBitmap N = new(resH, resW);
            SKColor[] res = img.Pixels;
            SKColor[] dec = new SKColor[res.Length];
            for (int x = 0; x < resW; x++)
            {
                for (int y = 0; y < resH; y++)
                {
                    dec[(resW - x - 1) * resH + y] = res[y * resW + x];
                }
            }
            N.Pixels = dec;
            return N;
        }

        public static SKBitmap Cut(this SKBitmap img, int X, int Y, int Width, int Height)
        {
            SKBitmap ans = new SKBitmap(Width, Height);
            using (SKCanvas g = new SKCanvas(ans))
            {
                g.DrawBitmap(img, new SKRect(X, Y, X + Width, Y + Height), new SKRect(0, 0, Width, Height));
            }
            return ans;
        }

        public static void Put(this SKBitmap img, SKBitmap smallimage, int X, int Y, int Width, int Height)
        {
            using (SKCanvas g = new SKCanvas(img))
            {
                g.DrawBitmap(smallimage, new SKRect(0, 0, Width, Height), new SKRect(X, Y, X + Width, Y + Height));
            }
        }
    }
}
