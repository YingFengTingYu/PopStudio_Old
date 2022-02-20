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
                    Texture.DXT5_RGBA_Padding.Write(bs, sKBitmap, head.blockSize);
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
                //Console.WriteLine("name:{4} width:{0} height:{1} storewidth:{2} wrong:{3}", head.width, head.height, head.blockSize >> 2, (head.width % 4 == 0 && head.height % 4 == 0), inFile);
                using (SKBitmap sKBitmap = Texture.DXT5_RGBA_Padding.Read(bs, head.width, head.height, head.blockSize))
                {
                    sKBitmap.Save(outFile);
                }
            }
        }
    }
}