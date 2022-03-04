using SkiaSharp;

namespace PopStudio.Texture
{
    internal static class PVRTC_4BPP_RGB
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
            PVRTCEncode.PvrTcPacket[] packets = new PVRTCEncode.PvrTcPacket[(newwidth * newwidth) >> 4];
            int index = packets.Length;
            for (int i = 0; i < index; i++)
            {
                packets[i] = new PVRTCEncode.PvrTcPacket(bs.ReadUInt64());
            }
            SKColor[] pixels = PVRTCEncode.Decode4Bpp(packets, newwidth);
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
            if (t)
            {
                image.Dispose();
            }
            return ans >> 1;
        }
    }
}