using PopStudio.Platform;

namespace PopStudio.Texture
{
    internal static unsafe class PVRTC_2BPP_RGBA
    {
        public static YFBitmap Read(BinaryStream bs, int width, int height)
        {
            bool t = false;
            int newwidth = width;
            int newheight = height;
            if (newwidth < 16)
            {
                newwidth = 16;
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
            byte[] packets = new byte[(newwidth * newwidth) >> 1];
            bs.Read(packets, 0, packets.Length);
            YFBitmap image = YFBitmap.Create(newwidth, newheight);
            YFColor* pixels = (YFColor*)image.GetPixels().ToPointer();
            fixed (byte* bbb = packets)
            {
                PVRTCDecode.PvrtcDecompress(bbb, pixels, (uint)newwidth, (uint)newheight, 2);
            }
            if (t)
            {
                YFBitmap image2 = image.Cut(0, 0, width, height);
                image.Dispose();
                return image2;
            }
            return image;
        }

        public static int Write(BinaryStream bs, YFBitmap image) => throw new NotImplementedException();
    }
}
