namespace PopStudio.Package.Rsb
{
    internal class RsbAutoPoolInfo
    {
        public string ID;
        public int part1_Offset_InDecompress;
        public int part1_Size;
        public int type = 0x1;
        //我很抱歉我无法解释rsb ps3版本号为3的rsb中出现的type不为1的情况
        //也无法解释这个rsb中rsgp和Autopool数量不相等的情况
        //I'm sorry that I cannot explain that sometimes the type is not 1 in rsb ps3 (version 3)
        //And I cannot explain why in this rsb the autopool's number is defferent form rsgp's number

        public void Write(BinaryStream bs)
        {
            bs.WriteString(ID, 0x80);
            bs.WriteInt32(part1_Offset_InDecompress);
            bs.WriteInt32(part1_Size);
            bs.WriteInt32(type);
            bs.WriteInt32(0);
            bs.WriteInt32(0);
            bs.WriteInt32(0);
        }

        public RsbAutoPoolInfo Read(BinaryStream bs)
        {
            ID = bs.ReadString(0x80).Replace("\0", "");
            part1_Offset_InDecompress = bs.ReadInt32();
            part1_Size = bs.ReadInt32();
            type = bs.ReadInt32();
            bs.IdInt32(0);
            bs.IdInt32(0);
            bs.IdInt32(0);
            return this;
        }
    }
}
