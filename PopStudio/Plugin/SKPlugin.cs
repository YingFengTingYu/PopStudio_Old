using SkiaSharp;

namespace PopStudio.Plugin
{
    internal static class SKPlugin
    {
        public static void Save(this SKBitmap pic, string outFile)
        {
            using (SKData p = pic.Encode(SKEncodedImageFormat.Png, 100))
            {
                using (FileStream fs = new FileStream(outFile, FileMode.Create))
                {
                    byte[] t = p.ToArray();
                    fs.Write(t, 0, t.Length);
                    t = null;
                }
            }
        }
    }
}
