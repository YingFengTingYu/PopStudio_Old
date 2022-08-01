namespace PopStudio.Compress
{
    internal static class Lzma
    {
        public static void Decompress(string inFile, string outFile)
        {
            SevenZip.Compression.LZMA.Decoder coder = new SevenZip.Compression.LZMA.Decoder();
            using (FileStream input = new FileStream(inFile, FileMode.Open))
            {
                using (FileStream output = new FileStream(outFile, FileMode.Create))
                {
                    byte[] properties = new byte[5];
                    input.Read(properties, 0, 5);
                    byte[] fileLengthBytes = new byte[8];
                    input.Read(fileLengthBytes, 0, 8);
                    long fileLength = BitConverter.ToInt64(fileLengthBytes, 0);
                    coder.SetDecoderProperties(properties);
                    coder.Code(input, output, input.Length, fileLength, null);
                    output.Flush();
                }
            }
        }

        public static void Compress(string inFile, string outFile)
        {
            SevenZip.Compression.LZMA.Encoder coder = new SevenZip.Compression.LZMA.Encoder();
            using (FileStream input = new FileStream(inFile, FileMode.Open))
            {
                using (FileStream output = new FileStream(outFile, FileMode.Create))
                {
                    coder.WriteCoderProperties(output);
                    output.Write(BitConverter.GetBytes(input.Length), 0, 8);
                    coder.Code(input, output, input.Length, -1, null);
                    output.Flush();
                }
            }
        }
    }
}