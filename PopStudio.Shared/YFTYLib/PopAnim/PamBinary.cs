namespace PopStudio.PopAnim
{
    /// <summary>
    /// It's all from Disassembling PVZ2 and Zuma's Revenge!
    /// </summary>
    internal class PamBinary
    {
        public static void Encode(PopAnimInfo pam, string outFile)
        {
            using (BinaryStream bs = new BinaryStream(outFile, FileMode.Create))
            {
                pam.Write(bs);
            }
        }

        public static PopAnimInfo Decode(string inFile)
        {
            using (BinaryStream bs = new BinaryStream(inFile, FileMode.Open))
            {
                return new PopAnimInfo().Read(bs);
            }
        }
    }
}
