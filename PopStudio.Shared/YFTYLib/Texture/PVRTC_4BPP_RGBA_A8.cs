using PopStudio.Platform;

namespace PopStudio.Texture
{
    internal static unsafe class PVRTC_4BPP_RGBA_A8
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
                newwidth = GetNextPOT(newwidth);
                t = true;
            }
            if ((newheight & (newheight - 1)) != 0)
            {
                newheight = GetNextPOT(newheight);
                t = true;
            }
            byte[] packets = new byte[(newwidth * newheight) >> 1];
            bs.Read(packets, 0, packets.Length);
            YFBitmap image = YFBitmap.Create(newwidth, newheight);
            YFColor* pixels = (YFColor*)image.GetPixels().ToPointer();
            fixed (byte* bbb = packets)
            {
                PVRTCDecode.PvrtcDecompress(bbb, pixels, (uint)newwidth, (uint)newheight, 4);
            }
            if (t)
            {
                YFBitmap image2 = image.Cut(0, 0, width, height);
                image.Dispose();
                image = image2;
            }
            pixels = (YFColor*)image.GetPixels().ToPointer();
            int S = image.Square;
            for (int i = 0; i < S; i++)
            {
                pixels++->Alpha = bs.ReadByte();
            }
            return image;
        }

        public static int Write(BinaryStream bs, YFBitmap image)
        {
            YFColor* pixels_raw = (YFColor*)image.GetPixels().ToPointer();
            int S = image.Square;
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
            byte[] outData = new byte[(newwidth * newheight) >> 1];
            fixed (byte* outDataPtr = outData)
            {
                PVRTCEncode.CompressPVRTCI_4BPP(pixels, (uint)newwidth, (uint)newheight, outDataPtr, false);
            }
            bs.WriteBytes(outData);
            for (int i = 0; i < S; i++)
            {
                bs.WriteByte(pixels_raw++->Alpha);
            }
            if (t)
            {
                image.Dispose();
            }
            return newwidth << 2;
        }
    }
}
