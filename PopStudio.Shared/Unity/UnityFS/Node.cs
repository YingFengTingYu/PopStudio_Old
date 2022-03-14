namespace PopStudio.Unity.UnityFS
{
    internal class Node
    {
        public long offset;
        public long size;
        public uint flags;
        public string path;

        public Node Read(BinaryStream bs)
        {
            offset = bs.ReadInt64();
            size = bs.ReadInt64();
            flags = bs.ReadUInt32();
            path = bs.ReadStringByEmpty();
            return this;
        }
    }
}