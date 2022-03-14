namespace PopStudio.Unity.Serialized
{
    internal class SerializedHead
    {
        public int MetadataSize;
        public long FileSize;
        public int Version;
        public long DataOffset;
        public string UnityVersion;
        public int Platform;
        public bool EnableTypeTree;

        public SerializedHead Read(BinaryStream bs)
        {
            MetadataSize = bs.ReadInt32();
            FileSize = bs.ReadInt32();
            Version = bs.ReadInt32();
            DataOffset = bs.ReadInt32();
            return this;
        }
    }
}
