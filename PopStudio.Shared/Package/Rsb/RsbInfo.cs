namespace PopStudio.Package.Rsb
{
    internal class RsbInfo
    {
        public RsbHeadInfo head;
        public CompressStringList fileList = new CompressStringList(0);
        public CompressStringList rsgpList = new CompressStringList(0);
        public RsbCompositeInfo[] compositeInfo;
        public CompressStringList compositeList = new CompressStringList(0);
        public RsbRsgpInfo[] rsgpInfo;
        public RsbAutoPoolInfo[] autopoolInfo;
        public RsbPtxInfo[] ptxInfo;
        public RsgpInfo[] rsgp;

        public RsbInfo Read(BinaryStream bs)
        {
            head = new RsbHeadInfo().Read(bs);
            compositeInfo = new RsbCompositeInfo[head.composite_Number];
            rsgpInfo = new RsbRsgpInfo[head.rsgp_Number];
            //autopoolInfo = new RsbAutoPoolInfo[head.autopool_Number];
            ptxInfo = new RsbPtxInfo[head.ptx_Number];
            rsgp = new RsgpInfo[head.rsgp_Number];
            //bs.Position = head.fileList_BeginOffset;
            //byte[] b_file = new byte[head.fileList_Length];
            //using (BinaryStream b_file_stream = new BinaryStream(b_file))
            //{
            //    int dividefour = head.fileList_Length >> 2;
            //    for (int i = 0; i < dividefour; i++)
            //    {
            //        b_file_stream.WriteInt32(bs.ReadInt32());
            //    }
            //}
            //fileList.Read(b_file);
            //bs.Position = head.rsgpList_BeginOffset;
            //byte[] b_rsgp = new byte[head.rsgpList_Length];
            //using (BinaryStream b_rsgp_stream = new BinaryStream(b_rsgp))
            //{
            //    int dividefour = head.rsgpList_Length >> 2;
            //    for (int i = 0; i < dividefour; i++)
            //    {
            //        b_rsgp_stream.WriteInt32(bs.ReadInt32());
            //    }
            //}
            //rsgpList.Read(b_rsgp);
            bs.Position = head.compositeInfo_BeginOffset;
            for (int i = 0; i < compositeInfo.Length; i++)
            {
                compositeInfo[i] = new RsbCompositeInfo().Read(bs);
            }
            //bs.Position = head.compositeList_BeginOffset;
            //byte[] b_composite = new byte[head.compositeList_Length];
            //using (BinaryStream b_composite_stream = new BinaryStream(b_composite))
            //{
            //    int dividefour = head.compositeList_Length >> 2;
            //    for (int i = 0; i < dividefour; i++)
            //    {
            //        b_composite_stream.WriteInt32(bs.ReadInt32());
            //    }
            //}
            //compositeList.Read(b_composite);
            bs.Position = head.rsgpInfo_BeginOffset;
            for (int i = 0; i < rsgpInfo.Length; i++)
            {
                rsgpInfo[i] = new RsbRsgpInfo().Read(bs);
            }
            //bs.Position = head.autopoolInfo_BeginOffset;
            //for (int i = 0; i < autopoolInfo.Length; i++)
            //{
            //    autopoolInfo[i] = new RsbAutoPoolInfo().Read(bs);
            //}
            bs.Position = head.ptxInfo_BeginOffset;
            for (int i = 0; i < ptxInfo.Length; i++)
            {
                ptxInfo[i] = new RsbPtxInfo(head.ptxInfo_EachLength).Read(bs);
            }
            for (int i = 0; i < rsgpInfo.Length; i++)
            {
                bs.Position = rsgpInfo[i].offset;
                rsgp[i] = new RsgpInfo().Read(bs);
            }
            return this;
        }

        [Obsolete]
        public RsbInfo ReadJustForUnPack(BinaryStream bs)
        {
            head = new RsbHeadInfo().Read(bs);
            compositeInfo = new RsbCompositeInfo[head.composite_Number];
            rsgpInfo = new RsbRsgpInfo[head.rsgp_Number];
            ptxInfo = new RsbPtxInfo[head.ptx_Number];
            rsgp = new RsgpInfo[head.rsgp_Number];
            bs.Position = head.compositeInfo_BeginOffset;
            for (int i = 0; i < compositeInfo.Length; i++)
            {
                compositeInfo[i] = new RsbCompositeInfo().Read(bs);
            }
            bs.Position = head.rsgpInfo_BeginOffset;
            for (int i = 0; i < rsgpInfo.Length; i++)
            {
                rsgpInfo[i] = new RsbRsgpInfo().Read(bs);
            }
            bs.Position = head.ptxInfo_BeginOffset;
            for (int i = 0; i < ptxInfo.Length; i++)
            {
                ptxInfo[i] = new RsbPtxInfo(head.ptxInfo_EachLength).Read(bs);
            }
            for (int i = 0; i < rsgpInfo.Length; i++)
            {
                bs.Position = rsgpInfo[i].offset;
                rsgp[i] = new RsgpInfo().Read(bs);
            }
            return this;
        }
    }
}
