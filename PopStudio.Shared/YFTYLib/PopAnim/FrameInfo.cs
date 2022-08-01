namespace PopStudio.PopAnim
{
    internal class FrameInfo
    {
        public string label { get; set; }
        public bool stop { get; set; }
        public CommandsInfo[] command { get; set; }
        public RemovesInfo[] remove { get; set; }
        public AddsInfo[] append { get; set; }
        public MovesInfo[] change { get; set; }

        public void Write(BinaryStream bs, int version)
        {
            FrameFlags flags = 0;
            if (remove != null && remove.Length > 0) flags |= FrameFlags.Removes;
            if (append != null && append.Length > 0) flags |= FrameFlags.Adds;
            if (change != null && change.Length > 0) flags |= FrameFlags.Moves;
            if (label != null) flags |= FrameFlags.FrameName;
            if (stop) flags |= FrameFlags.Stop;
            if (command != null && command.Length > 0) flags |= FrameFlags.Commands;
            bs.WriteByte((byte)flags);
            if ((flags & FrameFlags.Removes) != 0)
            {
                int count = remove.Length;
                if (count < 255 && count >= 0)
                {
                    bs.WriteByte((byte)count);
                }
                else
                {
                    bs.WriteByte(255);
                    bs.WriteInt16((short)count);
                }
                for (int i = 0; i < count; i++)
                {
                    remove[i].Write(bs, version);
                }
            }
            if ((flags & FrameFlags.Adds) != 0)
            {
                int count = append.Length;
                if (count < 255 && count >= 0)
                {
                    bs.WriteByte((byte)count);
                }
                else
                {
                    bs.WriteByte(255);
                    bs.WriteInt16((short)count);
                }
                for (int i = 0; i < count; i++)
                {
                    append[i].Write(bs, version);
                }
            }
            if ((flags & FrameFlags.Moves) != 0)
            {
                int count = change.Length;
                if (count < 255 && count >= 0)
                {
                    bs.WriteByte((byte)count);
                }
                else
                {
                    bs.WriteByte(255);
                    bs.WriteInt16((short)count);
                }
                for (int i = 0; i < count; i++)
                {
                    change[i].Write(bs, version);
                }
            }
            if ((flags & FrameFlags.FrameName) != 0)
            {
                bs.WriteStringByInt16Head(label);
            }
            if ((flags & FrameFlags.Stop) != 0)
            {
                //nothing to do
            }
            if ((flags & FrameFlags.Commands) != 0)
            {
                //Can't bigger than 255
                int count = command.Length;
                if (count > 255) count = 255;
                bs.WriteByte((byte)count);
                for (int i = 0; i < count; i++)
                {
                    command[i].Write(bs, version);
                }
            }
        }

        public FrameInfo Read(BinaryStream bs, int version)
        {
            FrameFlags flags = (FrameFlags)bs.ReadByte();
            if ((flags & FrameFlags.Removes) != 0)
            {
                int count = bs.ReadByte();
                if (count == 255)
                {
                    count = bs.ReadInt16();
                }
                remove = new RemovesInfo[count];
                for (int i = 0; i < count; i++)
                {
                    remove[i] = new RemovesInfo().Read(bs, version);
                }
            }
            else
            {
                remove = Array.Empty<RemovesInfo>();
            }
            if ((flags & FrameFlags.Adds) != 0)
            {
                int count = bs.ReadByte();
                if (count == 255)
                {
                    count = bs.ReadInt16();
                }
                append = new AddsInfo[count];
                for (int i = 0; i < count; i++)
                {
                    append[i] = new AddsInfo().Read(bs, version);
                }
            }
            else
            {
                append = Array.Empty<AddsInfo>();
            }
            if ((flags & FrameFlags.Moves) != 0)
            {
                int count = bs.ReadByte();
                if (count == 255)
                {
                    count = bs.ReadInt16();
                }
                change = new MovesInfo[count];
                for (int i = 0; i < count; i++)
                {
                    change[i] = new MovesInfo().Read(bs, version);
                }
            }
            else
            {
                change = Array.Empty<MovesInfo>();
            }
            if ((flags & FrameFlags.FrameName) != 0)
            {
                label = bs.ReadStringByInt16Head();
            }
            if ((flags & FrameFlags.Stop) != 0)
            {
                stop = true;
            }
            if ((flags & FrameFlags.Commands) != 0)
            {
                int num12 = bs.ReadByte();
                command = new CommandsInfo[num12];
                for (int m = 0; m < num12; m++)
                {
                    command[m] = new CommandsInfo().Read(bs, version);
                }
            }
            else
            {
                command = Array.Empty<CommandsInfo>();
            }
            return this;
        }

        public class CommandsInfo
        {
            public string command { get; set; }

            public string parameter { get; set; }

            public void Write(BinaryStream bs, int version)
            {
                bs.WriteStringByInt16Head(command);
                bs.WriteStringByInt16Head(parameter);
            }

            public CommandsInfo Read(BinaryStream bs, int version)
            {
                command = bs.ReadStringByInt16Head();
                parameter = bs.ReadStringByInt16Head();
                return this;
            }
        }

        public class RemovesInfo
        {
            public int index { get; set; }

            public void Write(BinaryStream bs, int version)
            {
                if (index >= 2047)
                {
                    bs.WriteInt16(2047);
                    bs.WriteInt32(index);
                }
                else
                {
                    bs.WriteInt16((short)index);
                }
            }

            public RemovesInfo Read(BinaryStream bs, int version)
            {
                index = bs.ReadInt16();
                if (index >= 2047)
                {
                    index = bs.ReadInt32();
                }
                return this;
            }
        }

        public class AddsInfo
        {
            public int index { get; set; }
            public string name { get; set; }
            public int resource { get; set; }
            public bool sprite { get; set; }
            public bool additive { get; set; }
            public int preload_frames { get; set; }
            public float timescale { get; set; } = 1;

            public void Write(BinaryStream bs, int version)
            {
                long beginPos = bs.Position;
                bs.Position += 2;
                int flags = 0;
                if (index >= 2047 || index < 0)
                {
                    flags |= 2047;
                    bs.WriteInt32(index);
                }
                else
                {
                    flags |= index;
                }
                flags |= sprite ? 32768 : 0;
                flags |= additive ? 16384 : 0;
                if (version >= 6)
                {
                    if (resource >= 255 || resource < 0)
                    {
                        bs.WriteByte(0xFF);
                        bs.WriteInt16((short)resource);
                    }
                    else
                    {
                        bs.WriteByte((byte)resource);
                    }
                }
                else
                {
                    bs.WriteByte((byte)resource);
                }
                if (preload_frames != 0)
                {
                    flags |= 8192;
                    bs.WriteInt16((short)preload_frames);
                }
                if (name != null)
                {
                    flags |= 4096;
                    bs.WriteStringByInt16Head(name);
                }
                if (timescale != 1)
                {
                    flags |= 2048;
                    bs.WriteInt32((int)(timescale * 65536));
                }
                long endPos = bs.Position;
                bs.Position = beginPos;
                bs.WriteUInt16((ushort)flags);
                bs.Position = endPos;
            }

            public AddsInfo Read(BinaryStream bs, int version)
            {
                ushort num5 = bs.ReadUInt16();
                index = num5 & 2047;
                if (index == 2047)
                {
                    index = bs.ReadInt32();
                }
                sprite = (num5 & 32768) != 0;
                additive = (num5 & 16384) != 0;
                resource = bs.ReadByte();
                if (version >= 6 && resource == 255)
                {
                    resource = bs.ReadInt16();
                }
                if ((num5 & 8192) != 0)
                {
                    preload_frames = bs.ReadInt16();
                }
                else
                {
                    preload_frames = 0;
                }
                if ((num5 & 4096) != 0)
                {
                    name = bs.ReadStringByInt16Head();
                }
                if ((num5 & 2048) != 0)
                {
                    timescale = bs.ReadInt32() / 65536f;
                }
                else
                {
                    timescale = 1;
                }
                return this;
            }
        }

        public class MovesInfo
        {
            public static readonly int LongCoordsMinVersion = 5;
            public static readonly int MatrixMinVersion = 2;

            public enum MoveFlags
            {
                SrcRect = 32768,
                Rotate = 16384,
                Color = 8192,
                Matrix = 4096,
                LongCoords = 2048,
                AnimFrameNum = 1024
            }

            public int index { get; set; }
            public double[] transform { get; set; }
            public double[] color { get; set; }
            public int[] src_rect { get; set; }
            public int anim_frame_num { get; set; }

            public void Write(BinaryStream bs, int version)
            {
                long beginPos = bs.Position;
                bs.Position += 2;
                int flags = 0;
                if (index >= 1023 || index < 0)
                {
                    flags |= 1023;
                    bs.WriteInt32(index);
                }
                else
                {
                    flags |= index;
                }
                MoveFlags f7 = 0;
                if (transform == null || transform.Length < 2)
                {
                    if (version >= LongCoordsMinVersion)
                    {
                        bs.WriteInt32(0);
                        bs.WriteInt32(0);
                        f7 |= MoveFlags.LongCoords;
                    }
                    else
                    {   
                        bs.WriteInt16(0);
                        bs.WriteInt16(0);
                    }
                }
                else if (transform.Length < 4)
                {
                    int v0 = (int)(transform[0] * 20);
                    int v1 = (int)(transform[1] * 20);
                    if (version >= LongCoordsMinVersion)
                    {
                        bs.WriteInt32(v0);
                        bs.WriteInt32(v1);
                        f7 |= MoveFlags.LongCoords;
                    }
                    else
                    {
                        bs.WriteInt16((short)v0);
                        bs.WriteInt16((short)v1);
                    }
                }
                else if (transform.Length < 6)
                {
                    if (version >= MatrixMinVersion)
                    {
                        f7 |= MoveFlags.Matrix;
                        bs.WriteInt32((int)(transform[0] * 65536));
                        bs.WriteInt32((int)(transform[2] * 65536));
                        bs.WriteInt32((int)(transform[1] * 65536));
                        bs.WriteInt32((int)(transform[3] * 65536));
                    }
                    else
                    {
                        f7 |= MoveFlags.Rotate;
                        double Rcos = Math.Acos(transform[0]);
                        if (transform[1] * (version == 2 ? -1 : 1) < 0) //sin < 0 => cos + pi
                        {
                            Rcos = -Rcos;
                        }
                        bs.WriteInt16((short)(Rcos * 1000));
                    }
                    if (version >= LongCoordsMinVersion)
                    {
                        bs.WriteInt32(0);
                        bs.WriteInt32(0);
                        f7 |= MoveFlags.LongCoords;
                    }
                    else
                    {
                        bs.WriteInt16(0);
                        bs.WriteInt16(0);
                    }
                }
                else
                {
                    if (version >= MatrixMinVersion)
                    {
                        f7 |= MoveFlags.Matrix;
                        bs.WriteInt32((int)(transform[0] * 65536));
                        bs.WriteInt32((int)(transform[2] * 65536));
                        bs.WriteInt32((int)(transform[1] * 65536));
                        bs.WriteInt32((int)(transform[3] * 65536));
                    }
                    else
                    {
                        f7 |= MoveFlags.Rotate;
                        double Rcos = Math.Acos(transform[0]);
                        if (transform[1] * (version == 2 ? -1 : 1) < 0) //sin < 0 => cos + pi
                        {
                            Rcos = -Rcos;
                        }
                        bs.WriteInt16((short)(Rcos * 1000));
                    }
                    int v0 = (int)(transform[4] * 20);
                    int v1 = (int)(transform[5] * 20);
                    if (version >= LongCoordsMinVersion)
                    {
                        bs.WriteInt32(v0);
                        bs.WriteInt32(v1);
                        f7 |= MoveFlags.LongCoords;
                    }
                    else
                    {
                        bs.WriteInt16((short)v0);
                        bs.WriteInt16((short)v1);
                    }
                }
                if (src_rect != null && src_rect.Length >= 4)
                {
                    f7 |= MoveFlags.SrcRect;
                    bs.WriteInt16((short)(src_rect[0] * 20));
                    bs.WriteInt16((short)(src_rect[1] * 20));
                    bs.WriteInt16((short)(src_rect[2] * 20));
                    bs.WriteInt16((short)(src_rect[3] * 20));
                }
                if (color != null && color.Length >= 4)
                {
                    f7 |= MoveFlags.Color;
                    bs.WriteByte((byte)(color[0] * 255));
                    bs.WriteByte((byte)(color[1] * 255));
                    bs.WriteByte((byte)(color[2] * 255));
                    bs.WriteByte((byte)(color[3] * 255));
                }
                if (anim_frame_num != 0)
                {
                    f7 |= MoveFlags.AnimFrameNum;
                    bs.WriteInt16((short)anim_frame_num);
                }
                flags |= (int)f7;
                long endPos = bs.Position;
                bs.Position = beginPos;
                bs.WriteUInt16((ushort)flags);
                bs.Position = endPos;
            }

            public MovesInfo Read(BinaryStream bs, int version)
            {
                ushort num7 = bs.ReadUInt16();
                int num8 = num7 & 1023;
                if (num8 == 1023)
                {
                    num8 = bs.ReadInt32();
                }
                index = num8;
                MoveFlags f7 = (MoveFlags)num7;
                if ((f7 & MoveFlags.Matrix) != 0)
                {
                    transform = new double[6];
                    transform[0] = bs.ReadInt32() / 65536d;
                    transform[2] = bs.ReadInt32() / 65536d;
                    transform[1] = bs.ReadInt32() / 65536d;
                    transform[3] = bs.ReadInt32() / 65536d;
                }
                else if ((f7 & MoveFlags.Rotate) != 0)
                {
                    transform = new double[6];
                    double num9 = bs.ReadInt16() / 1000d;
                    double num10 = Math.Sin((double)num9);
                    double num11 = Math.Cos((double)num9);
                    if (version == 2)
                    {
                        num10 = -num10;
                    }
                    transform[0] = num11;
                    transform[2] = -num10;
                    transform[1] = num10;
                    transform[3] = num11;
                }
                else
                {
                    transform = new double[2];
                }
                if ((f7 & MoveFlags.LongCoords) != 0)
                {
                    transform[^2] = bs.ReadInt32() / 20d;
                    transform[^1] = bs.ReadInt32() / 20d;
                }
                else
                {
                    transform[^2] = bs.ReadInt16() / 20d;
                    transform[^1] = bs.ReadInt16() / 20d;
                }
                if ((f7 & MoveFlags.SrcRect) != 0)
                {
                    src_rect = new int[4];
                    src_rect[0] = bs.ReadInt16() / 20;
                    src_rect[1] = bs.ReadInt16() / 20;
                    src_rect[2] = bs.ReadInt16() / 20;
                    src_rect[3] = bs.ReadInt16() / 20;
                }
                if ((f7 & MoveFlags.Color) != 0)
                {
                    color = new double[4];
                    color[0] = bs.ReadByte() / 255d;
                    color[1] = bs.ReadByte() / 255d;
                    color[2] = bs.ReadByte() / 255d;
                    color[3] = bs.ReadByte() / 255d;
                    //color = (bs.ReadByte() << 16) | (bs.ReadByte() << 8) | bs.ReadByte() | (bs.ReadByte() << 24);
                }
                if ((f7 & MoveFlags.AnimFrameNum) != 0)
                {
                    anim_frame_num = bs.ReadInt16();
                }
                else
                {
                    anim_frame_num = 0;
                }
                return this;
            }
        }
    }
}
