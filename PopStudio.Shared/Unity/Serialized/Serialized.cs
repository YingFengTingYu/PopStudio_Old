namespace PopStudio.Unity.Serialized
{
    /// <summary>
    /// To Do
    /// </summary>
    internal class Serialized
    {
        public static void Extract(string inFile, string outFile)
        {
            using (BinaryStream bs = new BinaryStream(inFile, FileMode.Open))
            {
                bs.Endian = Endian.Big;
                SerializedHead head = new SerializedHead().Read(bs);
                if (head.Version >= 9)
                {
                    bs.Endian = bs.ReadByte() == 0 ? Endian.Small : Endian.Big;
                    bs.Position += 3;
                }
                else
                {
                    bs.Position = head.FileSize - head.MetadataSize;
                    bs.Endian = bs.ReadByte() == 0 ? Endian.Small : Endian.Big;
                }
                if (head.Version >= 22)
                {
                    head.MetadataSize = bs.ReadInt32();
                    head.FileSize = bs.ReadInt64();
                    head.DataOffset = bs.ReadInt64();
                    bs.Position += 8;
                }
                if (head.Version >= 7)
                {
                    head.UnityVersion = bs.ReadStringByEmpty();
                }
                if (head.Version >= 8)
                {
                    head.UnityVersion = bs.ReadStringByEmpty();
                    head.Platform = bs.ReadInt32();
                }
                if (head.Version >= 13)
                {
                    head.EnableTypeTree = bs.ReadBoolean();
                }
            }
        }
    }
}
