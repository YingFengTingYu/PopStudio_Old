namespace PopStudio.Package.Rsb
{
    internal class RsbHeadInfo
    {
        public static readonly int magic = 1920164401;
        public static readonly int maxVersion = 4;
        public static readonly uint rsgpInfo_EachLength = 0xCC;
        public static readonly uint compositeInfo_EachLength = 0x484;
        public static readonly uint autopoolInfo_EachLength = 0x98;

        public int version = 3;
        public uint headLength;
        public uint fileList_Length;
        public uint fileList_BeginOffset;
        public uint rsgpList_Length;
        public uint rsgpList_BeginOffset;
        public uint rsgp_Number;
        public uint rsgpInfo_BeginOffset;
        public uint composite_Number;
        public uint compositeInfo_BeginOffset;
        public uint compositeList_Length;
        public uint compositeList_BeginOffset;
        public uint autopool_Number;
        public uint autopoolInfo_BeginOffset;
        public uint ptx_Number;
        public uint ptxInfo_BeginOffset;
        public uint ptxInfo_EachLength = 0x10; //中文版二代rsb是0x18
        public uint xmlPart1_BeginOffset;
        public uint xmlPart2_BeginOffset;
        public uint xmlPart3_BeginOffset;
        public uint rsbInfo_Length;

        public void Write(BinaryStream bs)
        {
            bs.WriteInt32(magic);
            bs.WriteInt32(version);
            bs.WriteInt32(0);
            bs.WriteUInt32(headLength);
            bs.WriteUInt32(fileList_Length);
            bs.WriteUInt32(fileList_BeginOffset);
            bs.WriteInt32(0);
            bs.WriteInt32(0);
            bs.WriteUInt32(rsgpList_Length);
            bs.WriteUInt32(rsgpList_BeginOffset);
            bs.WriteUInt32(rsgp_Number);
            bs.WriteUInt32(rsgpInfo_BeginOffset);
            bs.WriteUInt32(rsgpInfo_EachLength);
            bs.WriteUInt32(composite_Number);
            bs.WriteUInt32(compositeInfo_BeginOffset);
            bs.WriteUInt32(compositeInfo_EachLength);
            bs.WriteUInt32(compositeList_Length);
            bs.WriteUInt32(compositeList_BeginOffset);
            bs.WriteUInt32(autopool_Number);
            bs.WriteUInt32(autopoolInfo_BeginOffset);
            bs.WriteUInt32(autopoolInfo_EachLength);
            bs.WriteUInt32(ptx_Number);
            bs.WriteUInt32(ptxInfo_BeginOffset);
            bs.WriteUInt32(ptxInfo_EachLength);
            bs.WriteUInt32(xmlPart1_BeginOffset);
            bs.WriteUInt32(xmlPart2_BeginOffset);
            bs.WriteUInt32(xmlPart3_BeginOffset);
            if (version == 4) bs.WriteUInt32(rsbInfo_Length == 0 ? (xmlPart1_BeginOffset == 0 ? headLength : xmlPart1_BeginOffset) : rsbInfo_Length);
        }

        public RsbHeadInfo Read(BinaryStream bs)
        {
            bs.IdInt32(magic);
            version = bs.ReadInt32();
            bs.IdInt32(0);
            headLength = bs.ReadUInt32();
            fileList_Length = bs.ReadUInt32();
            fileList_BeginOffset = bs.ReadUInt32();
            bs.IdInt32(0);
            bs.IdInt32(0);
            rsgpList_Length = bs.ReadUInt32();
            rsgpList_BeginOffset = bs.ReadUInt32();
            rsgp_Number = bs.ReadUInt32();
            rsgpInfo_BeginOffset = bs.ReadUInt32();
            bs.IdUInt32(rsgpInfo_EachLength);
            composite_Number = bs.ReadUInt32();
            compositeInfo_BeginOffset = bs.ReadUInt32();
            bs.IdUInt32(compositeInfo_EachLength);
            compositeList_Length = bs.ReadUInt32();
            compositeList_BeginOffset = bs.ReadUInt32();
            autopool_Number = bs.ReadUInt32();
            autopoolInfo_BeginOffset = bs.ReadUInt32();
            bs.IdUInt32(autopoolInfo_EachLength);
            ptx_Number = bs.ReadUInt32();
            ptxInfo_BeginOffset = bs.ReadUInt32();
            ptxInfo_EachLength = bs.ReadUInt32();
            xmlPart1_BeginOffset = bs.ReadUInt32();
            xmlPart2_BeginOffset = bs.ReadUInt32();
            xmlPart3_BeginOffset = bs.ReadUInt32();
            if (version == 4) rsbInfo_Length = bs.ReadUInt32();
            return this;
        }
    }
}
