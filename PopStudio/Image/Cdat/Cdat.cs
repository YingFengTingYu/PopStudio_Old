namespace PopStudio.Image.Cdat
{
    internal static class Cdat
    {
        /// <summary>
        /// xor encrypt key
        /// </summary>
        static byte[] code = new byte[0x1F] { 0x41, 0x53, 0x32, 0x33, 0x44, 0x53, 0x52, 0x45, 0x50, 0x4C, 0x4B, 0x4C, 0x33, 0x33, 0x35, 0x4B, 0x4F, 0x34, 0x34, 0x33, 0x39, 0x30, 0x33, 0x32, 0x4E, 0x38, 0x33, 0x34, 0x35, 0x4E, 0x46 };

        public static void Encode(string inFile, string outFile)
        {
            using (BinaryStream bs2 = BinaryStream.Create(outFile))
            {
                using (BinaryStream bs = BinaryStream.Open(inFile))
                {
                    CdatHead head = new CdatHead
                    {
                        size = bs.Length
                    };
                    if (head.size >= 0x100)
                    {
                        head.Write(bs2);
                        int index = 0;
                        int arysize = code.Length;
                        for (int i = 0; i < 0x100; i++)
                        {
                            bs2.WriteByte((byte)(bs.ReadByte() ^ code[index++]));
                            index %= arysize;
                        }
                    }
                    bs.CopyTo(bs2);
                }
            }
        }

        public static void Decode(string inFile, string outFile)
        {
            using (BinaryStream bs = BinaryStream.Open(inFile))
            {
                CdatHead head = new CdatHead().Read(bs);
                using (BinaryStream bs2 = BinaryStream.Create(outFile))
                {
                    if (bs.Length >= 0x112)
                    {
                        int index = 0;
                        int arysize = code.Length;
                        for (int i = 0; i < 0x100; i++)
                        {
                            bs2.WriteByte((byte)(bs.ReadByte() ^ code[index++]));
                            index %= arysize;
                        }
                    }
                    bs.CopyTo(bs2);
                }
            }
        }
    }
}
