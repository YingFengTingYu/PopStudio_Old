namespace PopStudio.Particles
{
    internal class WP
    {
        static byte[] xnbmagic = new byte[0x6] { 0x58, 0x4E, 0x42, 0x6D, 0x05, 0x00 };
        static byte[] xnbinfo = new byte[0x2C] { 0x01, 0x24, 0x53, 0x65, 0x78, 0x79, 0x2E, 0x54, 0x6F, 0x64, 0x4C, 0x69, 0x62, 0x2E, 0x53, 0x65, 0x78, 0x79, 0x50, 0x61, 0x72, 0x74, 0x69, 0x63, 0x6C, 0x65, 0x52, 0x65, 0x61, 0x64, 0x65, 0x72, 0x2C, 0x20, 0x4C, 0x41, 0x57, 0x4E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };

        public static void Encode(Particles particles, string outFile)
        {
            using (BinaryStream bs = new BinaryStream(outFile, FileMode.Create))
            {
                bs.WriteBytes(xnbmagic);
                long off_size = bs.Position;
                bs.Position += 4;
                bs.WriteBytes(xnbinfo);
                int count = particles.Emitters?.Length ?? 0;
                bs.WriteInt32(count);
                for (int i = 0; i < count; i++)
                {
                    ParticlesEmitter emitter = particles.Emitters[i];
                    bs.WriteStringByVarInt32Head((string)emitter.Image);
                    bs.WriteInt32(emitter.ImageCol ?? 0);
                    bs.WriteInt32(emitter.ImageRow ?? 0);
                    bs.WriteInt32(emitter.ImageFrames ?? 1);
                    bs.WriteInt32(emitter.Animated ?? 0);
                    bs.WriteInt32(emitter.ParticleFlags);
                    bs.WriteInt32(emitter.EmitterType ?? 1);
                    bs.WriteStringByVarInt32Head(emitter.Name);
                    bs.WriteStringByVarInt32Head(emitter.OnDuration);
                    WriteTrackNodes(bs, emitter.SystemDuration);
                    WriteTrackNodes(bs, emitter.CrossFadeDuration);
                    WriteTrackNodes(bs, emitter.SpawnRate);
                    WriteTrackNodes(bs, emitter.SpawnMinActive);
                    WriteTrackNodes(bs, emitter.SpawnMaxActive);
                    WriteTrackNodes(bs, emitter.SpawnMaxLaunched);
                    WriteTrackNodes(bs, emitter.EmitterRadius);
                    WriteTrackNodes(bs, emitter.EmitterOffsetX);
                    WriteTrackNodes(bs, emitter.EmitterOffsetY);
                    WriteTrackNodes(bs, emitter.EmitterBoxX);
                    WriteTrackNodes(bs, emitter.EmitterBoxY);
                    WriteTrackNodes(bs, emitter.EmitterSkewX);
                    WriteTrackNodes(bs, emitter.EmitterSkewY);
                    WriteTrackNodes(bs, emitter.EmitterPath);
                    WriteTrackNodes(bs, emitter.ParticleDuration);
                    WriteTrackNodes(bs, emitter.LaunchSpeed);
                    WriteTrackNodes(bs, emitter.LaunchAngle);
                    WriteTrackNodes(bs, emitter.SystemRed);
                    WriteTrackNodes(bs, emitter.SystemGreen);
                    WriteTrackNodes(bs, emitter.SystemBlue);
                    WriteTrackNodes(bs, emitter.SystemAlpha);
                    WriteTrackNodes(bs, emitter.SystemBrightness);
                    WriteFields(bs, emitter.Field);
                    WriteFields(bs, emitter.SystemField);
                    WriteTrackNodes(bs, emitter.ParticleRed);
                    WriteTrackNodes(bs, emitter.ParticleGreen);
                    WriteTrackNodes(bs, emitter.ParticleBlue);
                    WriteTrackNodes(bs, emitter.ParticleAlpha);
                    WriteTrackNodes(bs, emitter.ParticleBrightness);
                    WriteTrackNodes(bs, emitter.ParticleSpinAngle);
                    WriteTrackNodes(bs, emitter.ParticleSpinSpeed);
                    WriteTrackNodes(bs, emitter.ParticleScale);
                    WriteTrackNodes(bs, emitter.ParticleStretch);
                    WriteTrackNodes(bs, emitter.CollisionReflect);
                    WriteTrackNodes(bs, emitter.CollisionSpin);
                    WriteTrackNodes(bs, emitter.ClipTop);
                    WriteTrackNodes(bs, emitter.ClipBottom);
                    WriteTrackNodes(bs, emitter.ClipLeft);
                    WriteTrackNodes(bs, emitter.ClipRight);
                    WriteTrackNodes(bs, emitter.AnimationRate);
                }
                bs.Position = off_size;
                bs.WriteInt32((int)bs.Length);
            }
        }

        static void WriteFields(BinaryStream bs, ParticlesField[] fields)
        {
            if (fields == null)
            {
                bs.WriteInt32(0);
                return;
            }
            int count = fields.Length;
            bs.WriteInt32(count);
            for (int i = 0; i < count; i++)
            {
                WriteTrackNodes(bs, fields[i].X);
                WriteTrackNodes(bs, fields[i].Y);
                bs.WriteInt32(fields[i].FieldType ?? 0);
            }
        }

        static void WriteTrackNodes(BinaryStream bs, ParticlesTrackNode[] nodes)
        {
            if (nodes == null)
            {
                bs.WriteInt32(0);
                return;
            }
            int count = nodes.Length;
            bs.WriteInt32(count);
            for (int i = 0; i < count; i++)
            {
                ParticlesTrackNode node = nodes[i];
                bs.WriteFloat32(node.Time);
                bs.WriteFloat32(node.LowValue ?? 0F);
                bs.WriteFloat32(node.HighValue ?? 0F);
                bs.WriteInt32(node.CurveType ?? 1);
                bs.WriteInt32(node.Distribution ?? 1);
            }
        }

        public static Particles Decode(string inFile)
        {
            using (BinaryStream bs = new BinaryStream(inFile, FileMode.Open))
            {
                Particles particles = new Particles();
                bs.IdBytes(xnbmagic);
                int size = bs.ReadInt32();
                bs.IdBytes(xnbinfo);
                int count = bs.ReadInt32();
                particles.Emitters = new ParticlesEmitter[count];
                for (int i = 0; i < count; i++)
                {
                    ParticlesEmitter emitter = new ParticlesEmitter();
                    string tempstr = bs.ReadStringByVarInt32Head();
                    if (!string.IsNullOrEmpty(tempstr)) emitter.Image = tempstr;
                    int tempint = bs.ReadInt32();
                    if (tempint != 0) emitter.ImageCol = tempint;
                    tempint = bs.ReadInt32();
                    if (tempint != 0) emitter.ImageRow = tempint;
                    tempint = bs.ReadInt32();
                    if (tempint != 1) emitter.ImageFrames = tempint;
                    tempint = bs.ReadInt32();
                    if (tempint != 0) emitter.Animated = tempint;
                    emitter.ParticleFlags = bs.ReadInt32();
                    tempint = bs.ReadInt32();
                    if (tempint != 1) emitter.EmitterType = tempint;
                    tempstr = bs.ReadStringByVarInt32Head();
                    if (!string.IsNullOrEmpty(tempstr)) emitter.Name = tempstr;
                    tempstr = bs.ReadStringByVarInt32Head();
                    if (!string.IsNullOrEmpty(tempstr)) emitter.OnDuration = tempstr;
                    emitter.SystemDuration = ReadTrackNodes(bs);
                    emitter.CrossFadeDuration = ReadTrackNodes(bs);
                    emitter.SpawnRate = ReadTrackNodes(bs);
                    emitter.SpawnMinActive = ReadTrackNodes(bs);
                    emitter.SpawnMaxActive = ReadTrackNodes(bs);
                    emitter.SpawnMaxLaunched = ReadTrackNodes(bs);
                    emitter.EmitterRadius = ReadTrackNodes(bs);
                    emitter.EmitterOffsetX = ReadTrackNodes(bs);
                    emitter.EmitterOffsetY = ReadTrackNodes(bs);
                    emitter.EmitterBoxX = ReadTrackNodes(bs);
                    emitter.EmitterBoxY = ReadTrackNodes(bs);
                    emitter.EmitterSkewX = ReadTrackNodes(bs);
                    emitter.EmitterSkewY = ReadTrackNodes(bs);
                    emitter.EmitterPath = ReadTrackNodes(bs);
                    emitter.ParticleDuration = ReadTrackNodes(bs);
                    emitter.LaunchSpeed = ReadTrackNodes(bs);
                    emitter.LaunchAngle = ReadTrackNodes(bs);
                    emitter.SystemRed = ReadTrackNodes(bs);
                    emitter.SystemGreen = ReadTrackNodes(bs);
                    emitter.SystemBlue = ReadTrackNodes(bs);
                    emitter.SystemAlpha = ReadTrackNodes(bs);
                    emitter.SystemBrightness = ReadTrackNodes(bs);
                    int fcount = bs.ReadInt32();
                    if (fcount != 0)
                    {
                        ParticlesField[] fields = new ParticlesField[fcount];
                        for (int j = 0; j < fcount; j++)
                        {
                            ParticlesField field = new ParticlesField();
                            field.X = ReadTrackNodes(bs);
                            field.Y = ReadTrackNodes(bs);
                            int type = bs.ReadInt32();
                            if (type != 0) field.FieldType = type;
                            fields[j] = field;
                        }
                        emitter.Field = fields;
                    }
                    fcount = bs.ReadInt32();
                    if (fcount != 0)
                    {
                        ParticlesField[] fields = new ParticlesField[fcount];
                        for (int j = 0; j < fcount; j++)
                        {
                            ParticlesField field = new ParticlesField();
                            field.X = ReadTrackNodes(bs);
                            field.Y = ReadTrackNodes(bs);
                            int type = bs.ReadInt32();
                            if (type != 0) field.FieldType = type;
                            fields[j] = field;
                        }
                        emitter.SystemField = fields;
                    }
                    emitter.ParticleRed = ReadTrackNodes(bs);
                    emitter.ParticleGreen = ReadTrackNodes(bs);
                    emitter.ParticleBlue = ReadTrackNodes(bs);
                    emitter.ParticleAlpha = ReadTrackNodes(bs);
                    emitter.ParticleBrightness = ReadTrackNodes(bs);
                    emitter.ParticleSpinAngle = ReadTrackNodes(bs);
                    emitter.ParticleSpinSpeed = ReadTrackNodes(bs);
                    emitter.ParticleScale = ReadTrackNodes(bs);
                    emitter.ParticleStretch = ReadTrackNodes(bs);
                    emitter.CollisionReflect = ReadTrackNodes(bs);
                    emitter.CollisionSpin = ReadTrackNodes(bs);
                    emitter.ClipTop = ReadTrackNodes(bs);
                    emitter.ClipBottom = ReadTrackNodes(bs);
                    emitter.ClipLeft = ReadTrackNodes(bs);
                    emitter.ClipRight = ReadTrackNodes(bs);
                    emitter.AnimationRate = ReadTrackNodes(bs);
                    particles.Emitters[i] = emitter;
                }
                return particles;
            }
        }

        static ParticlesTrackNode[] ReadTrackNodes(BinaryStream bs)
        {
            int count = bs.ReadInt32();
            if (count == 0) return null;
            ParticlesTrackNode[] ans = new ParticlesTrackNode[count];
            for (int i = 0; i < count; i++)
            {
                ParticlesTrackNode node = new ParticlesTrackNode();
                node.Time = bs.ReadFloat32();
                float tempfloat = bs.ReadFloat32();
                if (tempfloat != 0) node.LowValue = tempfloat;
                tempfloat = bs.ReadFloat32();
                if (tempfloat != 0) node.HighValue = tempfloat;
                int tempint = bs.ReadInt32();
                if (tempint != 1) node.CurveType = tempint;
                tempint = bs.ReadInt32();
                if (tempint != 1) node.Distribution = tempint;
                ans[i] = node;
            }
            return ans;
        }
    }
}