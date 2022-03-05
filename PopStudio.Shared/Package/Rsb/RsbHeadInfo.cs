namespace PopStudio.Package.Rsb
{
    internal class RsbHeadInfo
    {
        public static readonly int magic = 1920164401;
        public static readonly int maxVersion = 4;
        public static readonly int rsgpInfo_EachLength = 0xCC;
        public static readonly int compositeInfo_EachLength = 0x484;
        public static readonly int autopoolInfo_EachLength = 0x98;

        public int version = 3;
        public int headLength;
        public int fileList_Length;
        public int fileList_BeginOffset;
        public int rsgpList_Length;
        public int rsgpList_BeginOffset;
        public int rsgp_Number;
        public int rsgpInfo_BeginOffset;
        public int composite_Number;
        public int compositeInfo_BeginOffset;
        public int compositeList_Length;
        public int compositeList_BeginOffset;
        public int autopool_Number;
        public int autopoolInfo_BeginOffset;
        public int ptx_Number;
        public int ptxInfo_BeginOffset;
        public int ptxInfo_EachLength = 0x10; //中文版二代rsb是0x18
        public int xmlPart1_BeginOffset;
        public int xmlPart2_BeginOffset;
        public int xmlPart3_BeginOffset;
        public int rsbInfo_Length;

        public void Write(BinaryStream bs)
        {
            bs.WriteInt32(magic);
            bs.WriteInt32(version);
            bs.WriteInt32(0);
            bs.WriteInt32(headLength);
            bs.WriteInt32(fileList_Length);
            bs.WriteInt32(fileList_BeginOffset);
            bs.WriteInt32(0);
            bs.WriteInt32(0);
            bs.WriteInt32(rsgpList_Length);
            bs.WriteInt32(rsgpList_BeginOffset);
            bs.WriteInt32(rsgp_Number);
            bs.WriteInt32(rsgpInfo_BeginOffset);
            bs.WriteInt32(rsgpInfo_EachLength);
            bs.WriteInt32(composite_Number);
            bs.WriteInt32(compositeInfo_BeginOffset);
            bs.WriteInt32(compositeInfo_EachLength);
            bs.WriteInt32(compositeList_Length);
            bs.WriteInt32(compositeList_BeginOffset);
            bs.WriteInt32(autopool_Number);
            bs.WriteInt32(autopoolInfo_BeginOffset);
            bs.WriteInt32(autopoolInfo_EachLength);
            bs.WriteInt32(ptx_Number);
            bs.WriteInt32(ptxInfo_BeginOffset);
            bs.WriteInt32(ptxInfo_EachLength);
            bs.WriteInt32(xmlPart1_BeginOffset);
            bs.WriteInt32(xmlPart2_BeginOffset);
            bs.WriteInt32(xmlPart3_BeginOffset);
            if (version == 4) bs.WriteInt32(rsbInfo_Length == 0 ? (xmlPart1_BeginOffset == 0 ? headLength : xmlPart1_BeginOffset) : rsbInfo_Length);
        }

        public RsbHeadInfo Read(BinaryStream bs)
        {
            bs.IdInt32(magic);
            version = bs.ReadInt32();
            bs.IdInt32(0);
            headLength = bs.ReadInt32();
            fileList_Length = bs.ReadInt32();
            fileList_BeginOffset = bs.ReadInt32();
            bs.IdInt32(0);
            bs.IdInt32(0);
            rsgpList_Length = bs.ReadInt32();
            rsgpList_BeginOffset = bs.ReadInt32();
            rsgp_Number = bs.ReadInt32();
            rsgpInfo_BeginOffset = bs.ReadInt32();
            bs.IdInt32(rsgpInfo_EachLength);
            composite_Number = bs.ReadInt32();
            compositeInfo_BeginOffset = bs.ReadInt32();
            bs.IdInt32(compositeInfo_EachLength);
            compositeList_Length = bs.ReadInt32();
            compositeList_BeginOffset = bs.ReadInt32();
            autopool_Number = bs.ReadInt32();
            autopoolInfo_BeginOffset = bs.ReadInt32();
            bs.IdInt32(autopoolInfo_EachLength);
            ptx_Number = bs.ReadInt32();
            ptxInfo_BeginOffset = bs.ReadInt32();
            ptxInfo_EachLength = bs.ReadInt32();
            xmlPart1_BeginOffset = bs.ReadInt32();
            xmlPart2_BeginOffset = bs.ReadInt32();
            xmlPart3_BeginOffset = bs.ReadInt32();
            if (version == 4) rsbInfo_Length = bs.ReadInt32();
            return this;
        }
    }
}
