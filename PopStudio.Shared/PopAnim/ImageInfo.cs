namespace PopStudio.PopAnim
{
    internal class ImageInfo
    {
        public string name { get; set; }
        public int[] size { get; set; }
        public double[] transform { get; set; }

        public void Write(BinaryStream bs, int version)
        {
            bs.WriteStringByInt16Head(name);
            if (version >= 4)
            {
                if (size != null && size.Length >= 2)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        bs.WriteInt16((short)size[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < 2; i++)
                    {
                        bs.WriteInt16(-1);
                    }
                }
            }
            if (version == 1)
            {
                if (transform == null || transform.Length < 2)
                {
                    bs.WriteInt16(0);
                    bs.WriteInt16(0);
                    bs.WriteInt16(0);
                }
                else if (transform.Length >= 6)
                {
                    double Rcos = Math.Acos(transform[0]);
                    if (transform[1] * (version == 2 ? -1 : 1) < 0) //sin < 0 => cos + pi
                    {
                        Rcos = -Rcos;
                    }
                    bs.WriteInt16((short)Rcos);
                    bs.WriteInt16((short)(transform[4] * 20));
                    bs.WriteInt16((short)(transform[5] * 20));
                }
                else if (transform.Length >= 4)
                {
                    double Rcos = Math.Acos(transform[0]);
                    if (transform[1] * (version == 2 ? -1 : 1) < 0) //sin < 0 => cos + pi
                    {
                        Rcos = -Rcos;
                    }
                    bs.WriteInt16((short)Rcos);
                    bs.WriteInt16(0);
                    bs.WriteInt16(0);
                }
                else if (transform.Length >= 2)
                {
                    bs.WriteInt16(0);
                    bs.WriteInt16((short)(transform[0] * 20));
                    bs.WriteInt16((short)(transform[1] * 20));
                }
            }
            else
            {
                if (transform == null || transform.Length < 2)
                {
                    bs.WriteInt32(1310720);
                    bs.WriteInt32(0);
                    bs.WriteInt32(0);
                    bs.WriteInt32(1310720);
                    bs.WriteInt16(0);
                    bs.WriteInt16(0);
                }
                else if (transform.Length >= 6)
                {
                    bs.WriteInt32((int)(transform[0] * 1310720));
                    bs.WriteInt32((int)(transform[2] * 1310720));
                    bs.WriteInt32((int)(transform[1] * 1310720));
                    bs.WriteInt32((int)(transform[3] * 1310720));
                    bs.WriteInt16((short)(transform[4] * 20));
                    bs.WriteInt16((short)(transform[5] * 20));
                }
                else if (transform.Length >= 4)
                {
                    bs.WriteInt32((int)(transform[0] * 1310720));
                    bs.WriteInt32((int)(transform[2] * 1310720));
                    bs.WriteInt32((int)(transform[1] * 1310720));
                    bs.WriteInt32((int)(transform[3] * 1310720));
                    bs.WriteInt16(0);
                    bs.WriteInt16(0);
                }
                else if (transform.Length >= 2)
                {
                    bs.WriteInt32(1310720);
                    bs.WriteInt32(0);
                    bs.WriteInt32(0);
                    bs.WriteInt32(1310720);
                    bs.WriteInt16((short)(transform[0] * 20));
                    bs.WriteInt16((short)(transform[1] * 20));
                }
            }
        }

        public ImageInfo Read(BinaryStream bs, int version)
        {
            name = bs.ReadStringByInt16Head();
            size = new int[2];
            if (version >= 4)
            {
                for (int i = 0; i < 2; i++)
                {
                    size[i] = bs.ReadInt16();
                }
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    size[i] = -1;
                }
            }
            transform = new double[6];
            if (version == 1)
            {
                double num6 = bs.ReadInt16() / 1000d;
                double num7 = Math.Sin(num6);
                double num8 = Math.Cos(num6);
                transform[0] = num8;
                transform[2] = -num7;
                transform[1] = num7;
                transform[3] = num8;
                transform[4] = bs.ReadInt16() / 20d;
                transform[5] = bs.ReadInt16() / 20d;
            }
            else
            {
                transform[0] = bs.ReadInt32() / 1310720d;
                transform[2] = bs.ReadInt32() / 1310720d;
                transform[1] = bs.ReadInt32() / 1310720d;
                transform[3] = bs.ReadInt32() / 1310720d;
                transform[4] = bs.ReadInt16() / 20d;
                transform[5] = bs.ReadInt16() / 20d;
            }
            return this;
        }
    }
}
