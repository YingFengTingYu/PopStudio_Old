using SkiaSharp;

namespace PopStudio.Image.PtxXbox360
{
    internal static class Ptx
    {
        public static void Encode(string inFile, string outFile)
        {
            using (BinaryStream bs = new BinaryStream(outFile, FileMode.Create))
            {
                bs.Endian = Endian.Big;
                using (SKBitmap sKBitmap = SKBitmap.Decode(inFile))
                {
                    int newwidth = sKBitmap.Width;
                    if (newwidth % 128 != 0) newwidth = newwidth / 128 * 128 + 128;
                    PtxHead head = new PtxHead
                    {
                        width = sKBitmap.Width,
                        height = sKBitmap.Height,
                        blockSize = newwidth << 2
                    };
                    using (SKBitmap image2 = new SKBitmap(newwidth, head.height))
                    {
                        using (SKCanvas canvas = new SKCanvas(image2))
                        {
                            canvas.DrawBitmap(sKBitmap, new SKRect(0, 0, head.width, head.height));
                        }
                        Texture.DXT5.Write(bs, image2);
                        head.Write(bs);
                    }
                }
            }
        }

        public static void Decode(string inFile, string outFile)
        {
            using (BinaryStream bs = new BinaryStream(inFile, FileMode.Open))
            {
                bs.Endian = Endian.Big;
                if (bs.Length < 0x10) throw new Exception(Str.Obj.DataMisMatch);
                bs.Position = bs.Length - 0x10;
                PtxHead head = new PtxHead().Read(bs);
                bs.Position = 0;
                //Console.WriteLine("width:{0} blocksize:{1} storewidth:{2}", head.width, head.blockSize, head.blockSize >> 2);
                using (SKBitmap sKBitmap = Texture.DXT5.Read(bs, head.blockSize >> 2, head.height))
                {
                    using (SKBitmap image2 = new SKBitmap(head.width, head.height))
                    {
                        using (SKCanvas canvas = new SKCanvas(image2))
                        {
                            canvas.DrawBitmap(sKBitmap, new SKRect(0, 0, head.blockSize >> 2, head.height));
                        }
                        image2.Save(outFile);
                    }
                }
            }
        }
    }
}