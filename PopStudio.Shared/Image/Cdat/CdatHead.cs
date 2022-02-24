namespace PopStudio.Image.Cdat
{
    internal class CdatHead
    {
        public string magic = "CRYPT_RES\x0A\x00";
        public long size;

        public CdatHead Read(BinaryStream bs)
        {
            bs.IdString(magic);
            size = bs.ReadInt64();
            return this;
        }

        public void Write(BinaryStream bs)
        {
            bs.WriteString(magic);
            bs.WriteInt64(size);
        }
    }
}