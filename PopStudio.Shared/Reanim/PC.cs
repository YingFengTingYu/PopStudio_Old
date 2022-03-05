namespace PopStudio.Reanim
{
    internal static class PC
    {
        public static void Encode(Reanim reanim, string outFile)
        {
            using (BinaryStream bs = new BinaryStream())
            {
                bs.WriteInt32(-1282165568);
                bs.WriteInt32(0);
                int trackNum = reanim.tracks.Length;
                bs.WriteInt32(trackNum);
                bs.WriteFloat32(reanim.fps);
                bs.WriteInt32(0);
                bs.WriteInt32(0xC);
                for (int i = 0; i < trackNum; i++)
                {
                    bs.WriteInt32(0);
                    bs.WriteInt32(0);
                    bs.WriteInt32(reanim.tracks[i].transforms.Length);
                }
                for (int i = 0; i < trackNum; i++)
                {
                    ReanimTrack track = reanim.tracks[i];
                    bs.WriteStringByInt32Head(track.name);
                    bs.WriteInt32(0x2C);
                    int transformNum = track.transforms.Length;
                    for (int j = 0; j < transformNum; j++)
                    {
                        ReanimTransform transform = track.transforms[j];
                        bs.WriteFloat32(transform.x ?? -10000F);
                        bs.WriteFloat32(transform.y ?? -10000F);
                        bs.WriteFloat32(transform.kx ?? -10000F);
                        bs.WriteFloat32(transform.ky ?? -10000F);
                        bs.WriteFloat32(transform.sx ?? -10000F);
                        bs.WriteFloat32(transform.sy ?? -10000F);
                        bs.WriteFloat32(transform.f ?? -10000F);
                        bs.WriteFloat32(transform.a ?? -10000F);
                        bs.WriteInt32(0);
                        bs.WriteInt32(0);
                        bs.WriteInt32(0);
                    }
                    for (int j = 0; j < transformNum; j++)
                    {
                        ReanimTransform transform = track.transforms[j];
                        bs.WriteStringByInt32Head((string)transform.i);
                        bs.WriteStringByInt32Head(transform.font);
                        bs.WriteStringByInt32Head(transform.text);
                    }
                }
                bs.Position = 0;
                using (BinaryStream bs_source = new BinaryStream(outFile, FileMode.Create))
                {
                    bs_source.WriteInt32(-559022380);
                    bs_source.WriteInt32((int)bs.Length);
                    using (ZLibStream compressionStream = new ZLibStream(bs_source, CompressionMode.Compress))
                    {
                        bs.CopyTo(compressionStream);
                    }
                }
            }
        }

        public static Reanim Decode(string inFile)
        {
            using (BinaryStream bs = new BinaryStream())
            {
                using (BinaryStream bs_source = new BinaryStream(inFile, FileMode.Open))
                {
                    if (bs_source.PeekInt32() == -559022380)
                    {
                        bs_source.Position += 4;
                        int size = bs_source.ReadInt32();
                        //zlib
                        using (ZLibStream zLibStream = new ZLibStream(bs_source, CompressionMode.Decompress))
                        {
                            zLibStream.CopyTo(bs);
                        }
                    }
                    else
                    {
                        bs_source.CopyTo(bs);
                    }
                }
                Reanim reanim = new Reanim();
                bs.Position = 8;
                int tracksNumber = bs.ReadInt32();
                reanim.tracks = new ReanimTrack[tracksNumber];
                reanim.fps = bs.ReadFloat32();
                bs.Position += 4;
                bs.IdInt32(0xC);
                for (int i = 0; i < tracksNumber; i++)
                {
                    bs.Position += 8;
                    ReanimTrack t = new ReanimTrack
                    {
                        transforms = new ReanimTransform[bs.ReadInt32()]
                    };
                    reanim.tracks[i] = t;
                }
                for (int i = 0; i < tracksNumber; i++)
                {
                    ReanimTrack t = reanim.tracks[i];
                    t.name = bs.ReadStringByInt32Head();
                    bs.IdInt32(0x2C);
                    int times = t.transforms.Length;
                    for (int j = 0; j < times; j++)
                    {
                        ReanimTransform ts = new ReanimTransform();
                        float tempfloat = bs.ReadFloat32();
                        if (tempfloat != -10000F)
                        {
                            ts.x = tempfloat;
                        }
                        tempfloat = bs.ReadFloat32();
                        if (tempfloat != -10000F)
                        {
                            ts.y = tempfloat;
                        }
                        tempfloat = bs.ReadFloat32();
                        if (tempfloat != -10000F)
                        {
                            ts.kx = tempfloat;
                        }
                        tempfloat = bs.ReadFloat32();
                        if (tempfloat != -10000F)
                        {
                            ts.ky = tempfloat;
                        }
                        tempfloat = bs.ReadFloat32();
                        if (tempfloat != -10000F)
                        {
                            ts.sx = tempfloat;
                        }
                        tempfloat = bs.ReadFloat32();
                        if (tempfloat != -10000F)
                        {
                            ts.sy = tempfloat;
                        }
                        tempfloat = bs.ReadFloat32();
                        if (tempfloat != -10000F)
                        {
                            ts.f = tempfloat;
                        }
                        tempfloat = bs.ReadFloat32();
                        if (tempfloat != -10000F)
                        {
                            ts.a = tempfloat;
                        }
                        bs.Position += 12;
                        t.transforms[j] = ts;
                    }
                    for (int j = 0; j < times; j++)
                    {
                        ReanimTransform ts = t.transforms[j];
                        string tempstring = bs.ReadStringByInt32Head();
                        if (!string.IsNullOrEmpty(tempstring))
                        {
                            ts.i = tempstring;
                        }
                        tempstring = bs.ReadStringByInt32Head();
                        if (!string.IsNullOrEmpty(tempstring))
                        {
                            ts.font = tempstring;
                        }
                        tempstring = bs.ReadStringByInt32Head();
                        if (!string.IsNullOrEmpty(tempstring))
                        {
                            ts.text = tempstring;
                        }
                    }
                }
                return reanim;
            }
        }
    }
}
