namespace PopStudio.Compress
{
    internal static class Zlib
    {
        public static void Decompress(string inFile, string outFile)
        {
            using (FileStream infileStream = new FileStream(inFile, FileMode.Open))
            {
                using (ZLibStream zLibStream = new ZLibStream(infileStream, CompressionMode.Decompress))
                {
                    using (FileStream outfileStream = new FileStream(outFile, FileMode.Create))
                    {
                        zLibStream.CopyTo(outfileStream);
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
                    using (ZLibStream zLibStream = new ZLibStream(compressedFileStream, CompressionMode.Compress))
                    {
                        originalFileStream.CopyTo(zLibStream);
                    }
                }
            }
        }
    }
}