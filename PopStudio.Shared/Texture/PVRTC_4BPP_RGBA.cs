using PopStudio.Platform;

namespace PopStudio.Texture
{
    internal static unsafe class PVRTC_4BPP_RGBA
    {
        public static YFBitmap Read(BinaryStream bs, int width, int height)
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
            YFBitmap image = YFBitmap.Create(newwidth, newheight);
            YFColor* pixels = (YFColor*)image.GetPixels().ToPointer();
            PVRTCEncode.Decode4Bpp(packets, newwidth, pixels);
            if (t)
            {
                YFBitmap image2 = image.Cut(0, 0, width, height);
                image.Dispose();
                return image2;
            }
            return image;
        }

        public static int Write(BinaryStream bs, YFBitmap image)
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
                YFBitmap image2 = YFBitmap.Create(newwidth, newheight);
                image.MoveTo(image2, 0, 0);
                image = image2;
            }
            YFColor* pixels = (YFColor*)image.GetPixels().ToPointer();
            PVRTCEncode.PvrTcPacket[] words = PVRTCEncode.EncodeRGBA4Bpp(pixels, newwidth);
            int index = words.Length;
            for (int i = 0; i < index; i++)
            {
                bs.WriteUInt64(words[i].PvrTcWord);
            }
            if (t)
            {
                image.Dispose();
            }
            return newwidth >> 1;
        }
    }
}
