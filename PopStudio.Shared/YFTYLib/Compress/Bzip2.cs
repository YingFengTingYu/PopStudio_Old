namespace PopStudio.Compress
{
    internal static class Bzip2
    {
        public static void Decompress(string inFile, string outFile)
        {
            using (FileStream infileStream = new FileStream(inFile, FileMode.Open))
            {
                using (Ionic.BZip2.BZip2InputStream bzip2Stream = new Ionic.BZip2.BZip2InputStream(infileStream))
                {
                    using (FileStream outfileStream = new FileStream(outFile, FileMode.Create))
                    {
                        bzip2Stream.CopyTo(outfileStream);
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
                    using (Ionic.BZip2.BZip2OutputStream bzip2Stream = new Ionic.BZip2.BZip2OutputStream(compressedFileStream))
                    {
                        originalFileStream.CopyTo(bzip2Stream);
                    }
                }
            }
        }
    }
}