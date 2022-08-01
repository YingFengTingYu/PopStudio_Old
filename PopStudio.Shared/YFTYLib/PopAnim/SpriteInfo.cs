using System;
using System.Collections.Generic;
using System.Text;

namespace PopStudio.PopAnim
{
    internal class SpriteInfo
    {
        public string name { get; set; }
        public string description { get; set; } //?
        public double frame_rate { get; set; }
        public int[] work_area { get; set; }
        public FrameInfo[] frame { get; set; }

        public void Write(BinaryStream bs, int version)
        {
            if (version >= 4)
            {
                bs.WriteStringByInt16Head(name);
                if (version >= 6)
                {
                    bs.WriteStringByInt16Head(description);
                }
                bs.WriteInt32((int)(frame_rate * 65536d));
            }
            if (version >= 5)
            {
                if (work_area == null || work_area.Length < 2)
                {
                    bs.WriteInt16(1);
                    bs.WriteInt16(0);
                    bs.WriteInt16(0);
                }
                else
                {
                    bs.WriteInt16((short)work_area[1]);
                    bs.WriteInt16((short)work_area[0]);
                    bs.WriteInt16((short)(work_area[0] + work_area[1] - 1));
                }
            }
            else
            {
                if (work_area == null || work_area.Length < 2)
                {
                    bs.WriteInt16(1);
                }
                else
                {
                    bs.WriteInt16((short)work_area[1]);
                }
            }
            int framesCount = frame.Length;
            for (int i = 0; i < framesCount; i++)
            {
                frame[i].Write(bs, version);
            }
        }

        public SpriteInfo Read(BinaryStream bs, int version)
        {
            if (version >= 4)
            {
                name = bs.ReadStringByInt16Head();
                if (version >= 6)
                {
                    description = bs.ReadStringByInt16Head();
                }
                frame_rate = bs.ReadInt32() / 65536d;
            }
            else
            {
                name = null;
                frame_rate = -1;
            }
            int framesCount = bs.ReadInt16();
            work_area = new int[2];
            if (version >= 5)
            {
                work_area[0] = bs.ReadInt16();
                work_area[1] = bs.ReadInt16();
            }
            else
            {
                work_area[0] = 0;
                work_area[1] = framesCount - 1;
            }
            work_area[1] = framesCount;
            frame = new FrameInfo[framesCount];
            for (int i = 0; i < framesCount; i++)
            {
                frame[i] = new FrameInfo().Read(bs, version);
            }
            return this;
        }
    }
}
