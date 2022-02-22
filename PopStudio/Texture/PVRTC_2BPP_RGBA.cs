using SkiaSharp;

namespace PopStudio.Texture
{
    internal static class PVRTC_2BPP_RGBA
    {
        public static SKBitmap Read(BinaryStream bs, int width, int height)
        {
            throw new NotImplementedException(); //I'll do it soon
            //bool t = false;
            //int newwidth = width;
            //int newheight = height;
            //if (newwidth < 8)
            //{
            //    newwidth = 8;
            //    t = true;
            //}
            //if (newheight < 8)
            //{
            //    newheight = 8;
            //    t = true;
            //}
            //if ((newwidth & (newwidth - 1)) != 0)
            //{
            //    newwidth = 0b10 << ((int)Math.Floor(Math.Log2(newwidth)));
            //    t = true;
            //}
            //if ((newheight & (newheight - 1)) != 0)
            //{
            //    newheight = 0b10 << ((int)Math.Floor(Math.Log2(newheight)));
            //    t = true;
            //}
            //if (newwidth != newheight)
            //{
            //    newwidth = newheight = Math.Max(newwidth, newheight);
            //    t = true;
            //}
            //Pvrtc.PvrtcDecoder decoder = new();
            //SKColor[] pixels = decoder.DecompressPVRTC(bs.ReadBytes((newwidth * newheight) >> 2), newwidth, newheight, true);
            //SKBitmap image = new SKBitmap(newwidth, newheight);
            //image.Pixels = pixels;
            //if (t)
            //{
            //    SKBitmap image2 = new SKBitmap(width, height);
            //    using (SKCanvas canvas = new SKCanvas(image2))
            //    {
            //        canvas.DrawBitmap(image, new SKRect(0, 0, newwidth, newheight));
            //    }
            //    image.Dispose();
            //    return image2;
            //}
            //return image;
        }
    }
}
