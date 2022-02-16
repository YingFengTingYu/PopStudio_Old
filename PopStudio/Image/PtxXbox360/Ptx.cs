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
                    Texture.DXT5Padding.Write(bs, sKBitmap, head.blockSize);
                    head.Write(bs);
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
                using (SKBitmap sKBitmap = Texture.DXT5Padding.Read(bs, head.width, head.height, head.blockSize))
                {
                    sKBitmap.Save(outFile);
                }
            }
        }
    }
}