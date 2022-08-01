using System.Text;

namespace PopStudio.Image.Cdat
{
    internal static class Cdat
    {
        public static void Encode(string inFile, string outFile)
        {
            byte[] code = Encoding.UTF8.GetBytes(Setting.CdatKey);
            using (BinaryStream bs2 = BinaryStream.Create(outFile))
            {
                using (BinaryStream bs = BinaryStream.Open(inFile))
                {
                    CdatHead head = new CdatHead
                    {
                        size = bs.Length
                    };
                    head.Write(bs2);
                    if (head.size >= 0x100)
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

        public static void Decode(string inFile, string outFile)
        {
            byte[] code = Encoding.UTF8.GetBytes(Setting.CdatKey);
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
