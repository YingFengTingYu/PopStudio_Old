using System.Runtime.InteropServices;

namespace PopStudio.Plugin
{
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    internal unsafe struct YFColor
    {
        public static readonly YFColor Empty;

        [FieldOffset(0)]
        public byte Blue;

        [FieldOffset(1)]
        public byte Green;

        [FieldOffset(2)]
        public byte Red;

        [FieldOffset(3)]
        public byte Alpha;

        public YFColor(byte red, byte green, byte blue, byte alpha)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }

        public YFColor(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = 255;
        }

        public byte GetChannel(int channel) => channel switch
        {
            0 => Red,
            1 => Green,
            2 => Blue,
            3 => Alpha,
            _ => 0
        };

        public void SetChannel(int i, byte value)
        {
            switch (i)
            {
                case 0:
                    Blue = value;
                    break;
                case 1:
                    Green = value;
                    break;
                case 2:
                    Red = value;
                    break;
                case 3:
                    Alpha = value;
                    break;
            }
        }

        public byte GetLuminance()
        {
            return (byte)((77 * Red + 150 * Green + 28 * Blue) / 255);
        }

        public void SetLuminance(byte l)
        {
            Red = l;
            Green = l;
            Blue = l;
        }

        public YFColor WithRed(byte red)
        {
            return new YFColor(red, Green, Blue, Alpha);
        }

        public YFColor WithGreen(byte green)
        {
            return new YFColor(Red, green, Blue, Alpha);
        }

        public YFColor WithBlue(byte blue)
        {
            return new YFColor(Red, Green, blue, Alpha);
        }

        public YFColor WithAlpha(byte alpha)
        {
            return new YFColor(Red, Green, Blue, alpha);
        }

        public override string ToString()
        {
            return $"#{Alpha:x2}{Red:x2}{Green:x2}{Blue:x2}";
        }

        public static implicit operator YFColor(uint color)
        {
            return *(YFColor*)&color;
        }

        public static explicit operator uint(YFColor color)
        {
            return *(uint*)&color;
        }
    }
}
