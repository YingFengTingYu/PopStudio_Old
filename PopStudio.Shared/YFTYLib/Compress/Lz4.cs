namespace PopStudio.Compress
{
    internal static class Lz4
    {
        public static void Decompress(string inFile, string outFile)
        {
            using (FileStream infileStream = new FileStream(inFile, FileMode.Open))
            {
                using (K4os.Compression.LZ4.Streams.LZ4DecoderStream lz4Stream = K4os.Compression.LZ4.Streams.LZ4Stream.Decode(infileStream))
                {
                    using (FileStream outfileStream = new FileStream(outFile, FileMode.Create))
                    {
                        lz4Stream.CopyTo(outfileStream);
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
                    using (K4os.Compression.LZ4.Streams.LZ4EncoderStream lz4Stream = K4os.Compression.LZ4.Streams.LZ4Stream.Encode(compressedFileStream))
                    {
                        originalFileStream.CopyTo(lz4Stream);
                    }
                }
            }
        }
    }
}