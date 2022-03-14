namespace PopStudio.Unity.UnityFS
{
    internal class StorageBlock
    {
        public int uncompressedSize;
        public int compressedSize;
        public ushort flags;

        public StorageBlock Read(BinaryStream bs)
        {
            uncompressedSize = bs.ReadInt32();
            compressedSize = bs.ReadInt32();
            flags = bs.ReadUInt16();
            return this;
        }
    }
}
