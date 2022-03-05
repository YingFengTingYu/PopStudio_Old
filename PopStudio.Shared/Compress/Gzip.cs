namespace PopStudio.Compress
{
    internal static class Gzip
    {
        public static void Decompress(string inFile, string outFile)
        {
            using (FileStream infileStream = new FileStream(inFile, FileMode.Open))
            {
                using (GZipStream gZipStream = new GZipStream(infileStream, CompressionMode.Decompress))
                {
                    using (FileStream outfileStream = new FileStream(outFile, FileMode.Create))
                    {
                        gZipStream.CopyTo(outfileStream);
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
                    using (GZipStream gZipStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                    {
                        originalFileStream.CopyTo(gZipStream);
                    }
                }
            }
        }
    }
}