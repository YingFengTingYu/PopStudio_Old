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
