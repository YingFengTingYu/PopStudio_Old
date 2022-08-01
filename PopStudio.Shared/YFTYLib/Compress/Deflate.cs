namespace PopStudio.Compress
{
    internal static class Deflate
    {
        public static void Decompress(string inFile, string outFile)
        {
            using (FileStream infileStream = new FileStream(inFile, FileMode.Open))
            {
                using (DeflateStream deflateStream = new DeflateStream(infileStream, CompressionMode.Decompress))
                {
                    using (FileStream outfileStream = new FileStream(outFile, FileMode.Create))
                    {
                        deflateStream.CopyTo(outfileStream);
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
                    using (DeflateStream deflateStream = new DeflateStream(compressedFileStream, CompressionMode.Compress))
                    {
                        originalFileStream.CopyTo(deflateStream);
                    }
                }
            }
        }
    }
}
