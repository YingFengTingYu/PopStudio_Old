namespace PopStudio.Trail
{
    internal class WP
    {
        static byte[] xnbmagic = new byte[0x6] { 0x58, 0x4E, 0x42, 0x6D, 0x05, 0x00 };
        static byte[] xnbinfo = new byte[0x25] { 0x01, 0x1D, 0x53, 0x65, 0x78, 0x79, 0x2E, 0x54, 0x6F, 0x64, 0x4C, 0x69, 0x62, 0x2E, 0x54, 0x72, 0x61, 0x69, 0x6C, 0x52, 0x65, 0x61, 0x64, 0x65, 0x72, 0x2C, 0x20, 0x4C, 0x41, 0x57, 0x4E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };

        public static void Encode(Trail trail, string outFile)
        {
            using (BinaryStream bs = new BinaryStream(outFile, FileMode.Create))
            {
                bs.WriteBytes(xnbmagic);
                long off_size = bs.Position;
                bs.Position += 4;
                bs.WriteBytes(xnbinfo);
                bs.WriteStringByVarInt32Head((string)trail.Image);
                bs.WriteInt32(trail.MaxPoints ?? 2);
                bs.WriteFloat64(trail.MinPointDistance ?? 1F);
                bs.WriteInt32(trail.TrailFlags);
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
                        bs.WriteInt32(node.CurveType ?? 1);
                        bs.WriteInt32(node.Distribution ?? 1);
                        bs.WriteFloat64(node.HighValue ?? 0F);
                        bs.WriteFloat64(node.LowValue ?? 0F);
                        bs.WriteFloat64(node.Time);
                    }
                }
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
                        bs.WriteInt32(node.CurveType ?? 1);
                        bs.WriteInt32(node.Distribution ?? 1);
                        bs.WriteFloat64(node.HighValue ?? 0F);
                        bs.WriteFloat64(node.LowValue ?? 0F);
                        bs.WriteFloat64(node.Time);
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
                        bs.WriteInt32(node.CurveType ?? 1);
                        bs.WriteInt32(node.Distribution ?? 1);
                        bs.WriteFloat64(node.HighValue ?? 0F);
                        bs.WriteFloat64(node.LowValue ?? 0F);
                        bs.WriteFloat64(node.Time);
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
                        bs.WriteInt32(node.CurveType ?? 1);
                        bs.WriteInt32(node.Distribution ?? 1);
                        bs.WriteFloat64(node.HighValue ?? 0F);
                        bs.WriteFloat64(node.LowValue ?? 0F);
                        bs.WriteFloat64(node.Time);
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
                        bs.WriteInt32(node.CurveType ?? 1);
                        bs.WriteInt32(node.Distribution ?? 1);
                        bs.WriteFloat64(node.HighValue ?? 0F);
                        bs.WriteFloat64(node.LowValue ?? 0F);
                        bs.WriteFloat64(node.Time);
                    }
                }
                bs.Position = off_size;
                bs.WriteInt32((int)bs.Length);
            }
        }

        public static Trail Decode(string inFile)
        {
            using (BinaryStream bs = new BinaryStream(inFile, FileMode.Open))
            {
                Trail trail = new Trail();
                bs.IdBytes(xnbmagic);
                int size = bs.ReadInt32();
                bs.IdBytes(xnbinfo);
                string ts = bs.ReadStringByVarInt32Head();
                if (!string.IsNullOrEmpty(ts)) trail.Image = ts;
                int ti = bs.ReadInt32();
                if (ti != 0) trail.MaxPoints = ti;
                double tf = bs.ReadFloat64();
                if (tf != 0F) trail.MinPointDistance = (float)tf;
                trail.TrailFlags = bs.ReadInt32();
                int count = bs.ReadInt32();
                trail.TrailDuration = new TrailTrackNode[count];
                for (int i = 0; i < count; i++)
                {
                    TrailTrackNode node = new TrailTrackNode();
                    int tempint = bs.ReadInt32();
                    if (tempint != 1) node.CurveType = tempint;
                    tempint = bs.ReadInt32();
                    if (tempint != 1) node.Distribution = tempint;
                    double temppfloat = bs.ReadFloat64();
                    if (temppfloat != 0) node.HighValue = (float)temppfloat;
                    temppfloat = bs.ReadFloat64();
                    if (temppfloat != 0) node.LowValue = (float)temppfloat;
                    node.Time = (float)bs.ReadFloat64();
                    trail.TrailDuration[i] = node;
                }
                count = bs.ReadInt32();
                trail.WidthOverLength = new TrailTrackNode[count];
                for (int i = 0; i < count; i++)
                {
                    TrailTrackNode node = new TrailTrackNode();
                    int tempint = bs.ReadInt32();
                    if (tempint != 1) node.CurveType = tempint;
                    tempint = bs.ReadInt32();
                    if (tempint != 1) node.Distribution = tempint;
                    double temppfloat = bs.ReadFloat64();
                    if (temppfloat != 0) node.HighValue = (float)temppfloat;
                    temppfloat = bs.ReadFloat64();
                    if (temppfloat != 0) node.LowValue = (float)temppfloat;
                    node.Time = (float)bs.ReadFloat64();
                    trail.WidthOverLength[i] = node;
                }
                count = bs.ReadInt32();
                trail.WidthOverTime = new TrailTrackNode[count];
                for (int i = 0; i < count; i++)
                {
                    TrailTrackNode node = new TrailTrackNode();
                    int tempint = bs.ReadInt32();
                    if (tempint != 1) node.CurveType = tempint;
                    tempint = bs.ReadInt32();
                    if (tempint != 1) node.Distribution = tempint;
                    double temppfloat = bs.ReadFloat64();
                    if (temppfloat != 0) node.HighValue = (float)temppfloat;
                    temppfloat = bs.ReadFloat64();
                    if (temppfloat != 0) node.LowValue = (float)temppfloat;
                    node.Time = (float)bs.ReadFloat64();
                    trail.WidthOverTime[i] = node;
                }
                count = bs.ReadInt32();
                trail.AlphaOverLength = new TrailTrackNode[count];
                for (int i = 0; i < count; i++)
                {
                    TrailTrackNode node = new TrailTrackNode();
                    int tempint = bs.ReadInt32();
                    if (tempint != 1) node.CurveType = tempint;
                    tempint = bs.ReadInt32();
                    if (tempint != 1) node.Distribution = tempint;
                    double temppfloat = bs.ReadFloat64();
                    if (temppfloat != 0) node.HighValue = (float)temppfloat;
                    temppfloat = bs.ReadFloat64();
                    if (temppfloat != 0) node.LowValue = (float)temppfloat;
                    node.Time = (float)bs.ReadFloat64();
                    trail.AlphaOverLength[i] = node;
                }
                count = bs.ReadInt32();
                trail.AlphaOverTime = new TrailTrackNode[count];
                for (int i = 0; i < count; i++)
                {
                    TrailTrackNode node = new TrailTrackNode();
                    int tempint = bs.ReadInt32();
                    if (tempint != 1) node.CurveType = tempint;
                    tempint = bs.ReadInt32();
                    if (tempint != 1) node.Distribution = tempint;
                    double temppfloat = bs.ReadFloat64();
                    if (temppfloat != 0) node.HighValue = (float)temppfloat;
                    temppfloat = bs.ReadFloat64();
                    if (temppfloat != 0) node.LowValue = (float)temppfloat;
                    node.Time = (float)bs.ReadFloat64();
                    trail.AlphaOverTime[i] = node;
                }
                return trail;
            }
        }
    }
}