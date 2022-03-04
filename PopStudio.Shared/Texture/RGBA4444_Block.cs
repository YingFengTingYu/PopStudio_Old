using SkiaSharp;

namespace PopStudio.Texture
{
    internal static class RGBA4444_Block
    {
        public static SKBitmap Read(BinaryStream bs, int width, int height)
        {
            bool t = false;
            int newwidth = width;
            int newheight = height;
            if (newwidth % 32 != 0)
            {
                newwidth += 32 - newwidth % 32;
                t = true;
            }
            if (newheight % 32 != 0)
            {
                newheight += 32 - newheight % 32;
                t = true;
            }
            int S = newwidth * newheight;
            SKColor[] pixels = new SKColor[S];
            ushort temp;
            int r, g, b, a;
            for (int i = 0; i < newheight; i += 32)
            {
                for (int w = 0; w < newwidth; w += 32)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        for (int k = 0; k < 32; k++)
                        {
                            temp = bs.ReadUInt16();
                            r = (temp & 0xF000) >> 12;
                            g = (temp & 0xF00) >> 8;
                            b = (temp & 0xF0) >> 4;
                            a = temp & 0xF;
                            pixels[(i + j) * newwidth + w + k] = new SKColor((byte)((r << 4) | r), (byte)((g << 4) | g), (byte)((b << 4) | b), (byte)((a << 4) | a));
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

        public static int Write(BinaryStream bs, SKBitmap image)
        {
            int ans = image.Width;
            bool t = false;
            int newwidth = image.Width;
            int newheight = image.Height;
            if (newwidth % 32 != 0)
            {
                newwidth += 32 - newwidth % 32;
                t = true;
            }
            if (newheight % 32 != 0)
            {
                newheight += 32 - newheight % 32;
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
            int temp;
            for (int i = 0; i < newheight; i += 32)
            {
                for (int w = 0; w < newwidth; w += 32)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        for (int k = 0; k < 32; k++)
                        {
                            temp = (i + j) * newwidth + w + k;
                            bs.WriteUInt16((ushort)((pixels[temp].Alpha >> 4) | (pixels[temp].Blue & 0xF0) | ((pixels[temp].Green & 0xF0) << 4) | ((pixels[temp].Red & 0xF0) << 8)));
                        }
                    }
                }
            }
            if (t)
            {
                image.Dispose();
            }
            return ans << 1;
        }
    }
}
