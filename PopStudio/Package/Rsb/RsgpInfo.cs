namespace PopStudio.Package.Rsb
{
    internal class RsgpInfo
    {
        public RsgpHeadInfo head;
        public CompressStringList fileList = new CompressStringList(1);

        public RsgpInfo Read(BinaryStream bs)
        {
            long back = bs.Position;
            head = new RsgpHeadInfo().Read(bs);
            bs.Position = back + head.fileList_BeginOffset;
            byte[] b_file = new byte[head.fileList_Length];
            using (BinaryStream b_file_stream = new BinaryStream(b_file))
            {
                int dividefour = head.fileList_Length >> 2;
                for (int i = 0; i < dividefour; i++)
                {
                    b_file_stream.WriteInt32(bs.ReadInt32());
                }
            }
            fileList.Read(b_file);
            return this;
        }
    }
}
