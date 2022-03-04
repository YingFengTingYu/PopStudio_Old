using SkiaSharp;

namespace PopStudio.Texture
{
    internal static class RGBA5551_Block
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
            int r, g, b;
            for (int i = 0; i < newheight; i += 32)
            {
                for (int w = 0; w < newwidth; w += 32)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        for (int k = 0; k < 32; k++)
                        {
                            temp = bs.ReadUInt16();
                            r = (temp & 0xF800) >> 11;
                            g = (temp & 0x7C0) >> 6;
                            b = (temp & 0x3E) >> 1;
                            pixels[(i + j) * newwidth + w + k] = new SKColor((byte)((r << 3) | (r >> 2)), (byte)((g << 3) | (g >> 2)), (byte)((b << 3) | (b >> 2)), (byte)((temp & 0x1) == 0 ? 0 : 255));
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
                            bs.WriteUInt16((ushort)(((pixels[temp].Alpha & 0b10000000) >> 7) | ((pixels[temp].Blue & 0xF8) >> 2) | ((pixels[temp].Green & 0xF8) << 3) | ((pixels[temp].Red & 0xF8) << 8)));
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
