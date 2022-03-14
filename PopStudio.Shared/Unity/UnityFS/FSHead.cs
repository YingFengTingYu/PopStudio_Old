namespace PopStudio.Unity.UnityFS
{
    internal class FSHead
    {
        public string signature;
        public int version;
        public string unityVersion;
        public string unityRevision;
        public long size;
        public int compressedBlocksInfoSize;
        public int uncompressedBlocksInfoSize;
        public uint flags;

        public FSHead Read(BinaryStream bs)
        {
            signature = bs.ReadStringByEmpty();
            version = bs.ReadInt32();
            unityVersion = bs.ReadStringByEmpty();
            unityRevision = bs.ReadStringByEmpty();
            size = bs.ReadInt64();
            compressedBlocksInfoSize = bs.ReadInt32();
            uncompressedBlocksInfoSize = bs.ReadInt32();
            flags = bs.ReadUInt32();
            return this;
        }
    }
}
