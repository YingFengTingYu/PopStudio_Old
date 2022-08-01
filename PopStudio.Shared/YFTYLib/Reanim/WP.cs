namespace PopStudio.Reanim
{
    internal static class WP
    {
        static byte[] xnbmagic = new byte[0x6] { 0x58, 0x4E, 0x42, 0x6D, 0x05, 0x00 };
        static byte[] xnbinfo = new byte[0x26] { 0x01, 0x1E, 0x53, 0x65, 0x78, 0x79, 0x2E, 0x54, 0x6F, 0x64, 0x4C, 0x69, 0x62, 0x2E, 0x52, 0x65, 0x61, 0x6E, 0x69, 0x6D, 0x52, 0x65, 0x61, 0x64, 0x65, 0x72, 0x2C, 0x20, 0x4C, 0x41, 0x57, 0x4E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };

        public static void Encode(Reanim reanim, string outFile)
        {
            using (BinaryStream bs = new BinaryStream(outFile, FileMode.Create))
            {
                bs.Encode = System.Text.Encoding.Unicode;
                bs.WriteBytes(xnbmagic);
                long off_size = bs.Position;
                bs.Position += 4;
                bs.WriteBytes(xnbinfo);
                bs.WriteSByte(reanim.doScale ?? 0);
                bs.WriteFloat32(reanim.fps);
                int trackNum = reanim.tracks.Length;
                bs.WriteInt32(trackNum);
                for (int i = 0; i < trackNum; i++)
                {
                    ReanimTrack track = reanim.tracks[i];
                    if (track.name == null)
                    {
                        bs.WriteInt32(0);
                    }
                    else
                    {
                        bs.WriteInt32(track.name.Length);
                        bs.WriteString(track.name);
                    }
                    int transformNum = track.transforms.Length;
                    bs.WriteInt32(transformNum);
                    for (int j = 0; j < transformNum; j++)
                    {
                        ReanimTransform transform = track.transforms[j];
                        byte type = 0;
                        if (j != 0 && transform.x == null && transform.y == null && transform.sx == null && transform.sy == null && transform.kx == null && transform.ky == null && transform.f == null && transform.a == null && transform.i == null && transform.font == null && transform.text == null) type = 0x1;
                        bs.WriteByte(type);
                        if (type == 0)
                        {
                            if (transform.font == null)
                            {
                                bs.WriteInt32(0);
                            }
                            else
                            {
                                bs.WriteInt32(transform.font.Length);
                                bs.WriteString(transform.font);
                            }
                            if (transform.i == null)
                            {
                                bs.WriteInt32(0);
                            }
                            else
                            {
                                string image = (string)transform.i;
                                bs.WriteInt32(image.Length);
                                bs.WriteString(image);
                            }
                            if (transform.text == null)
                            {
                                bs.WriteInt32(0);
                            }
                            else
                            {
                                bs.WriteInt32(transform.text.Length);
                                bs.WriteString(transform.text);
                            }
                            bs.WriteFloat32(transform.a ?? -99999F);
                            bs.WriteFloat32(transform.f ?? -99999F);
                            bs.WriteFloat32(transform.sx ?? -99999F);
                            bs.WriteFloat32(transform.sy ?? -99999F);
                            bs.WriteFloat32(transform.kx ?? -99999F);
                            bs.WriteFloat32(transform.ky ?? -99999F);
                            bs.WriteFloat32(transform.x ?? -99999F);
                            bs.WriteFloat32(transform.y ?? -99999F);
                        }
                    }
                }
                bs.Position = off_size;
                bs.WriteInt32((int)bs.Length);
            }
        }

        public static Reanim Decode(string inFile)
        {
            using (BinaryStream bs = new BinaryStream(inFile, FileMode.Open))
            {
                bs.Encode = System.Text.Encoding.Unicode;
                Reanim reanim = new Reanim();
                bs.IdBytes(xnbmagic);
                int size = bs.ReadInt32();
                bs.IdBytes(xnbinfo);
                reanim.doScale = bs.ReadSByte();
                reanim.fps = bs.ReadFloat32();
                int tracksNumber = bs.ReadInt32();
                reanim.tracks = new ReanimTrack[tracksNumber];
                for (int i = 0; i < tracksNumber; i++)
                {
                    ReanimTrack t = new ReanimTrack();
                    t.name = bs.ReadString(bs.ReadInt32() << 1);
                    int times = bs.ReadInt32();
                    t.transforms = new ReanimTransform[times];
                    for (int j = 0; j < times; j++)
                    {
                        ReanimTransform ts = new ReanimTransform();
                        byte type = bs.ReadByte();
                        if (type == 0)
                        {
                            int tempint = bs.ReadInt32();
                            if (tempint != 0)
                            {
                                ts.font = bs.ReadString(tempint << 1);
                            }
                            tempint = bs.ReadInt32();
                            if (tempint != 0)
                            {
                                ts.i = bs.ReadString(tempint << 1);
                            }
                            tempint = bs.ReadInt32();
                            if (tempint != 0)
                            {
                                ts.text = bs.ReadString(tempint << 1);
                            }
                            float tempfloat = bs.ReadFloat32();
                            if (tempfloat != -99999F)
                            {
                                ts.a = tempfloat;
                            }
                            tempfloat = bs.ReadFloat32();
                            if (tempfloat != -99999F)
                            {
                                ts.f = tempfloat;
                            }
                            tempfloat = bs.ReadFloat32();
                            if (tempfloat != -99999F)
                            {
                                ts.sx = tempfloat;
                            }
                            tempfloat = bs.ReadFloat32();
                            if (tempfloat != -99999F)
                            {
                                ts.sy = tempfloat;
                            }
                            tempfloat = bs.ReadFloat32();
                            if (tempfloat != -99999F)
                            {
                                ts.kx = tempfloat;
                            }
                            tempfloat = bs.ReadFloat32();
                            if (tempfloat != -99999F)
                            {
                                ts.ky = tempfloat;
                            }
                            tempfloat = bs.ReadFloat32();
                            if (tempfloat != -99999F)
                            {
                                ts.x = tempfloat;
                            }
                            tempfloat = bs.ReadFloat32();
                            if (tempfloat != -99999F)
                            {
                                ts.y = tempfloat;
                            }
                        }
                        t.transforms[j] = ts;
                    }
                    reanim.tracks[i] = t;
                }
                return reanim;
            }
        }
    }
}
