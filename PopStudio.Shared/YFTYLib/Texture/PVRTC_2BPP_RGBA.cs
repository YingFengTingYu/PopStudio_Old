using PopStudio.Platform;

namespace PopStudio.Texture
{
    internal static unsafe class PVRTC_2BPP_RGBA
    {
        static int GetNextPOT(int v)
        {
            int k = 1;
            while (k < v)
            {
                k <<= 1;
            }
            return k;
        }

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
                newwidth = GetNextPOT(newwidth);
                t = true;
            }
            if ((newheight & (newheight - 1)) != 0)
            {
                newheight = GetNextPOT(newheight);
                t = true;
            }
            byte[] packets = new byte[(newwidth * newheight) >> 2];
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

        public static int Write(BinaryStream bs, YFBitmap image)
        {
            int ans = image.Width;
            bool t = false;
            int newwidth = image.Width;
            int newheight = image.Height;
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
                newwidth = GetNextPOT(newwidth);
                t = true;
            }
            if ((newheight & (newheight - 1)) != 0)
            {
                newheight = GetNextPOT(newheight);
                t = true;
            }
            if (t)
            {
                YFBitmap image2 = YFBitmap.Create(newwidth, newheight);
                image.MoveTo(image2, 0, 0);
                image = image2;
            }
            YFColor* pixels = (YFColor*)image.GetPixels().ToPointer();
            byte[] outData = new byte[(newwidth * newheight) >> 2];
            fixed (byte* outDataPtr = outData)
            {
                PVRTCEncode.CompressPVRTCI_2BPP(pixels, (uint)newwidth, (uint)newheight, outDataPtr, true);
            }
            bs.WriteBytes(outData);
            if (t)
            {
                image.Dispose();
            }
            return newwidth >> 2;
        }
    }
}
