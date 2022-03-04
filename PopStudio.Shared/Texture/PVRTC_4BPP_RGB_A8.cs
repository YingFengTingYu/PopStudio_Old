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
            PVRTCEncode.PvrTcPacket[] packets = new PVRTCEncode.PvrTcPacket[S >> 4];
            int index = packets.Length;
            for (int i = 0; i < index; i++)
            {
                packets[i] = new PVRTCEncode.PvrTcPacket(bs.ReadUInt64());
            }
            SKColor[] pixels = PVRTCEncode.Decode4Bpp(packets, newwidth);
            if (t)
            {
                SKBitmap image1 = new SKBitmap(newwidth, newheight);
                image1.Pixels = pixels;
                SKBitmap image2 = new SKBitmap(width, height);
                using (SKCanvas canvas = new SKCanvas(image2))
                {
                    canvas.DrawBitmap(image1, new SKRect(0, 0, newwidth, newheight));
                }
                pixels = image2.Pixels;
                image1.Dispose();
                image2.Dispose();
            }
            S = pixels.Length;
            for (int i = 0; i < S; i++)
            {
                pixels[i] = pixels[i].WithAlpha(bs.ReadByte());
            }
            SKBitmap image = new SKBitmap(width, height);
            image.Pixels = pixels;
            return image;
        }

        public static int Write(BinaryStream bs, SKBitmap image)
        {
            SKBitmap imagein = image;
            bool t = false;
            int newwidth = image.Width;
            int newheight = image.Height;
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
            PVRTCEncode.PvrTcPacket[] words = PVRTCEncode.EncodeRGB4Bpp(pixels, newwidth);
            int index = words.Length;
            for (int i = 0; i < index; i++)
            {
                bs.WriteUInt64(words[i].PvrTcWord);
            }
            if (t) pixels = imagein.Pixels;
            index = pixels.Length;
            for (int i = 0; i < index; i++)
            {
                bs.WriteByte(pixels[i].Alpha);
            }
            if (t)
            {
                image.Dispose();
            }
            return imagein.Width << 2;
        }
    }
}
