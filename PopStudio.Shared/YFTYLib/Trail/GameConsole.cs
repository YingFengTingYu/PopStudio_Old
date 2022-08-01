namespace PopStudio.Trail
{
    internal class GameConsole
    {
        public static void Encode(Trail trail, string outFile)
        {
            using (BinaryStream bs = new BinaryStream(outFile, FileMode.Create))
            {
                bs.Endian = Endian.Big;
                bs.WriteInt32(0);
                bs.WriteInt32(0);
                bs.WriteInt32(trail.MaxPoints ?? 2);
                bs.WriteFloat32(trail.MinPointDistance ?? 1F);
                bs.WriteInt32(trail.TrailFlags);
                for (int i = 0; i < 5; i++)
                {
                    bs.WriteInt32(0x0);
                    bs.WriteInt32(0x0);
                }
                bs.WriteStringByInt32Head((string)trail.Image);
                if (trail.WidthOverLength == null)
                {
                    bs.WriteInt32(0x0);
                }
                else
                {
                    int count = trail.WidthOverLength.Length;
                    bs.WriteInt32(count);
                    for (int i = 0; i < count; i++)
                    {
                        TrailTrackNode node = trail.WidthOverLength[i];
                        bs.WriteFloat32(node.Time);
                        bs.WriteFloat32(node.LowValue ?? 0F);
                        bs.WriteFloat32(node.HighValue ?? 0F);
                        bs.WriteInt32(node.CurveType ?? 1);
                        bs.WriteInt32(node.Distribution ?? 1);
                    }
                }
                if (trail.WidthOverTime == null)
                {
                    bs.WriteInt32(0x0);
                }
                else
                {
                    int count = trail.WidthOverTime.Length;
                    bs.WriteInt32(count);
                    for (int i = 0; i < count; i++)
                    {
                        TrailTrackNode node = trail.WidthOverTime[i];
                        bs.WriteFloat32(node.Time);
                        bs.WriteFloat32(node.LowValue ?? 0F);
                        bs.WriteFloat32(node.HighValue ?? 0F);
                        bs.WriteInt32(node.CurveType ?? 1);
                        bs.WriteInt32(node.Distribution ?? 1);
                    }
                }
                if (trail.AlphaOverLength == null)
                {
                    bs.WriteInt32(0x0);
                }
                else
                {
                    int count = trail.AlphaOverLength.Length;
                    bs.WriteInt32(count);
                    for (int i = 0; i < count; i++)
                    {
                        TrailTrackNode node = trail.AlphaOverLength[i];
                        bs.WriteFloat32(node.Time);
                        bs.WriteFloat32(node.LowValue ?? 0F);
                        bs.WriteFloat32(node.HighValue ?? 0F);
                        bs.WriteInt32(node.CurveType ?? 1);
                        bs.WriteInt32(node.Distribution ?? 1);
                    }
                }
                if (trail.AlphaOverTime == null)
                {
                    bs.WriteInt32(0x0);
                }
                else
                {
                    int count = trail.AlphaOverTime.Length;
                    bs.WriteInt32(count);
                    for (int i = 0; i < count; i++)
                    {
                        TrailTrackNode node = trail.AlphaOverTime[i];
                        bs.WriteFloat32(node.Time);
                        bs.WriteFloat32(node.LowValue ?? 0F);
                        bs.WriteFloat32(node.HighValue ?? 0F);
                        bs.WriteInt32(node.CurveType ?? 1);
                        bs.WriteInt32(node.Distribution ?? 1);
                    }
                }
                if (trail.TrailDuration == null)
                {
                    bs.WriteInt32(0x0);
                }
                else
                {
                    int count = trail.TrailDuration.Length;
                    bs.WriteInt32(count);
                    for (int i = 0; i < count; i++)
                    {
                        TrailTrackNode node = trail.TrailDuration[i];
                        bs.WriteFloat32(node.Time);
                        bs.WriteFloat32(node.LowValue ?? 0F);
                        bs.WriteFloat32(node.HighValue ?? 0F);
                        bs.WriteInt32(node.CurveType ?? 1);
                        bs.WriteInt32(node.Distribution ?? 1);
                    }
                }
            }
        }

        public static Trail Decode(string inFile)
        {
            using (BinaryStream bs = new BinaryStream())
            {
                using (BinaryStream bs_source = new BinaryStream(inFile, FileMode.Open))
                {
                    bs_source.Endian = Endian.Big;
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
                bs.Endian = Endian.Big;
                Trail trail = new Trail();
                bs.Position = 8;
                int ti = bs.ReadInt32();
                if (ti != 0) trail.MaxPoints = ti;
                float tf = bs.ReadFloat32();
                if (tf != 0F) trail.MinPointDistance = tf;
                trail.TrailFlags = bs.ReadInt32();
                bs.Position += 40;
                string ts = bs.ReadStringByInt32Head();
                if (!string.IsNullOrEmpty(ts)) trail.Image = ts;
                int count = bs.ReadInt32();
                trail.WidthOverLength = new TrailTrackNode[count];
                for (int i = 0; i < count; i++)
                {
                    TrailTrackNode node = new TrailTrackNode();
                    node.Time = bs.ReadFloat32();
                    float temppfloat = bs.ReadFloat32();
                    if (temppfloat != 0) node.LowValue = temppfloat;
                    temppfloat = bs.ReadFloat32();
                    if (temppfloat != 0) node.HighValue = temppfloat;
                    int tempint = bs.ReadInt32();
                    if (tempint != 1) node.CurveType = tempint;
                    tempint = bs.ReadInt32();
                    if (tempint != 1) node.Distribution = tempint;
                    trail.WidthOverLength[i] = node;
                }
                count = bs.ReadInt32();
                trail.WidthOverTime = new TrailTrackNode[count];
                for (int i = 0; i < count; i++)
                {
                    TrailTrackNode node = new TrailTrackNode();
                    node.Time = bs.ReadFloat32();
                    float temppfloat = bs.ReadFloat32();
                    if (temppfloat != 0) node.LowValue = temppfloat;
                    temppfloat = bs.ReadFloat32();
                    if (temppfloat != 0) node.HighValue = temppfloat;
                    int tempint = bs.ReadInt32();
                    if (tempint != 1) node.CurveType = tempint;
                    tempint = bs.ReadInt32();
                    if (tempint != 1) node.Distribution = tempint;
                    trail.WidthOverTime[i] = node;
                }
                count = bs.ReadInt32();
                trail.AlphaOverLength = new TrailTrackNode[count];
                for (int i = 0; i < count; i++)
                {
                    TrailTrackNode node = new TrailTrackNode();
                    node.Time = bs.ReadFloat32();
                    float temppfloat = bs.ReadFloat32();
                    if (temppfloat != 0) node.LowValue = temppfloat;
                    temppfloat = bs.ReadFloat32();
                    if (temppfloat != 0) node.HighValue = temppfloat;
                    int tempint = bs.ReadInt32();
                    if (tempint != 1) node.CurveType = tempint;
                    tempint = bs.ReadInt32();
                    if (tempint != 1) node.Distribution = tempint;
                    trail.AlphaOverLength[i] = node;
                }
                count = bs.ReadInt32();
                trail.AlphaOverTime = new TrailTrackNode[count];
                for (int i = 0; i < count; i++)
                {
                    TrailTrackNode node = new TrailTrackNode();
                    node.Time = bs.ReadFloat32();
                    float temppfloat = bs.ReadFloat32();
                    if (temppfloat != 0) node.LowValue = temppfloat;
                    temppfloat = bs.ReadFloat32();
                    if (temppfloat != 0) node.HighValue = temppfloat;
                    int tempint = bs.ReadInt32();
                    if (tempint != 1) node.CurveType = tempint;
                    tempint = bs.ReadInt32();
                    if (tempint != 1) node.Distribution = tempint;
                    trail.AlphaOverTime[i] = node;
                }
                count = bs.ReadInt32();
                trail.TrailDuration = new TrailTrackNode[count];
                for (int i = 0; i < count; i++)
                {
                    TrailTrackNode node = new TrailTrackNode();
                    node.Time = bs.ReadFloat32();
                    float temppfloat = bs.ReadFloat32();
                    if (temppfloat != 0) node.LowValue = temppfloat;
                    temppfloat = bs.ReadFloat32();
                    if (temppfloat != 0) node.HighValue = temppfloat;
                    int tempint = bs.ReadInt32();
                    if (tempint != 1) node.CurveType = tempint;
                    tempint = bs.ReadInt32();
                    if (tempint != 1) node.Distribution = tempint;
                    trail.TrailDuration[i] = node;
                }
                return trail;
            }
        }
    }
}