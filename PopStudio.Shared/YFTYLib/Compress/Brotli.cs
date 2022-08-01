namespace PopStudio.Compress
{
    internal static class Brotli
    {
        public static void Decompress(string inFile, string outFile)
        {
            using (FileStream infileStream = new FileStream(inFile, FileMode.Open))
            {
                using (BrotliStream brotliStream = new BrotliStream(infileStream, CompressionMode.Decompress))
                {
                    using (FileStream outfileStream = new FileStream(outFile, FileMode.Create))
                    {
                        brotliStream.CopyTo(outfileStream);
                        outfileStream.Flush();
                    }
                }
            }
        }

        public static void Compress(string inFile, string outFile)
        {
            using (FileStream originalFileStream = new FileStream(inFile, FileMode.Open))
            {
                using (FileStream compressedFileStream = new FileStream(outFile, FileMode.Create))
                {
                    using (BrotliStream brotliStream = new BrotliStream(compressedFileStream, CompressionMode.Compress))
                    {
                        originalFileStream.CopyTo(brotliStream);
                    }
                }
            }
        }
    }
}