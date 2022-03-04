using SkiaSharp;

namespace PopStudio.Texture
{
    internal static class RGB565_Block
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
            for (int i = 0; i < newheight; i += 32)
            {
                for (int w = 0; w < newwidth; w += 32)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        for (int k = 0; k < 32; k++)
                        {
                            temp = bs.ReadUInt16();
                            pixels[(i + j) * newwidth + w + k] = new SKColor((byte)((temp & 0xF800) >> 8), (byte)((temp & 0x7E0) >> 3), (byte)((temp & 0x1F) << 3), 255);
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
                            bs.WriteUInt16((ushort)(((pixels[temp].Blue & 0xF8) >> 3) | ((pixels[temp].Green & 0xFC) << 3) | ((pixels[temp].Red & 0xF8) << 8)));
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
