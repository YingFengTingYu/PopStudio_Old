using SkiaSharp;

namespace PopStudio.Image.Xnb
{
    /// <summary>
    /// It's dds used DXT5_RGBA Texture
    /// </summary>
    internal static class Xnb
    {
        public static void Encode(string inFile, string outFile)
        {
            using (BinaryStream bs = new BinaryStream(outFile, FileMode.Create))
            {
                using (SKBitmap sKBitmap = SKBitmap.Decode(inFile))
                {
                    XnbHead head = new XnbHead
                    {
                        width = (ushort)sKBitmap.Width,
                        height = (ushort)sKBitmap.Height
                    };
                    head.Write(bs);
                    Texture.ABGR8888.Write(bs, sKBitmap);
                    bs.Position = 0;
                    head.Write(bs);
                }
            }
        }

        public static void Decode(string inFile, string outFile)
        {
            using (BinaryStream bs = new BinaryStream(inFile, FileMode.Open))
            {
                XnbHead head = new XnbHead().Read(bs);
                using (SKBitmap sKBitmap = Texture.ABGR8888.Read(bs, head.width, head.height))
                {
                    sKBitmap.Save(outFile);
                }
            }
        }
    }
}