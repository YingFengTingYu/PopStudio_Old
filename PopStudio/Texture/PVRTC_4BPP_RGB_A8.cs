using SkiaSharp;

namespace PopStudio.Texture
{
    internal static class PVRTC_4BPP_RGB_A8
    {
        public static SKBitmap Read(BinaryStream bs, int width, int height)
        {
            bool t = false;
            int newwidth = width;
            int newheight = height;
            if (newwidth < 8)
            {
                newwidth = 8;
                t = true;
            }
            if (newheight < 8)
            {
                newheight = 8;
                t = true;
            }
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
            if (newwidth != newheight)
            {
                newwidth = newheight = Math.Max(newwidth, newheight);
                t = true;
            }
            int S = newwidth * newheight;
            Pvrtc.PvrtcDecoder decoder = new();
            SKColor[] pixels = decoder.DecompressPVRTC(bs.ReadBytes(S >> 1), newwidth, newheight, false);
            for (int i = 0; i < S; i++)
            {
                pixels[i] = pixels[i].WithAlpha(bs.ReadByte());
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
