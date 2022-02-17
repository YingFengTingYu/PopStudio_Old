using SkiaSharp;

namespace PopStudio.Plugin
{
    internal class Palette
    {
		public static List<byte> GeneratePalette_A8(SKColor[] Colors, int NrOutputColors, int NrOutputBits = 8)
		{
			List<byte> UniqueColors = new List<byte>();
			if (NrOutputBits > 8) NrOutputBits = 8;
			if (NrOutputBits < 1) NrOutputBits = 1;
			uint mask = (uint)(~((1 << (8 - NrOutputBits)) - 1)) & 0xFF;
			foreach (SKColor color in Colors)
			{
				UniqueColors.Add((byte)(color.Alpha & mask));
			}
			UniqueColors = UniqueColors.Distinct().ToList();
			int t = NrOutputColors - 2;
			while (UniqueColors.Count > t)
			{
				int mindiff = int.MaxValue;
				int amin = -1;
				int bmin = -1;
				for (int a = 0; a < UniqueColors.Count; a++)
				{
					for (int c = a + 1; c < UniqueColors.Count; c++)
					{
						int diff = abs(UniqueColors[a] - UniqueColors[c]);
						if (diff < mindiff)
						{
							mindiff = diff;
							amin = a;
							bmin = c;
							if (diff < 14) goto go;
						}
					}
				}
			go:
				UniqueColors.RemoveAt(amin);
				if (bmin > amin) bmin--;
				UniqueColors.RemoveAt(bmin);
				UniqueColors.Add((byte)((UniqueColors[amin] + UniqueColors[bmin]) >> 1));
			}
			UniqueColors.Add(0);
			UniqueColors.Add(255);
			UniqueColors.AddRange(new byte[NrOutputColors - UniqueColors.Count]);
			return UniqueColors;
		}

		static int abs(int v)
		{
			if (v < 0) return -v;
			return v;
		}

		public static List<SKColor> GeneratePalette_RGBA8888(SKColor[] Colors, int NrOutputColors, int NrOutputBits = 8, bool FirstTransparent = false)
		{
			List<SKColor> UniqueColors = new List<SKColor>();
			if (NrOutputBits > 8) NrOutputBits = 8;
			if (NrOutputBits < 1) NrOutputBits = 1;
			uint mask = (uint)(~((1 << (8 - NrOutputBits)) - 1)) & 0xFF;
			foreach (SKColor color in Colors)
			{
				uint a = color.Alpha & mask;
				if (a < 127) continue;
				uint r = color.Red & mask;
				uint g = color.Green & mask;
				uint b = color.Blue & mask;
				UniqueColors.Add(new SKColor((byte)r, (byte)g, (byte)b));
			}
			UniqueColors = UniqueColors.Distinct().ToList();
			while (UniqueColors.Count > NrOutputColors - (FirstTransparent ? 1 : 0))
			{
				float mindiff = float.MaxValue;
				int amin = -1;
				int bmin = -1;
				for (int a = 0; a < UniqueColors.Count; a++)
				{
					for (int c = a + 1; c < UniqueColors.Count; c++)
					{
						float diff = ColorDifference(UniqueColors[a], UniqueColors[c]);
						if (diff < mindiff)
						{
							mindiff = diff;
							amin = a;
							bmin = c;
							if (diff < 700) goto go;
						}
					}
				}
			go:
				SKColor e = ColorMean(UniqueColors[amin], UniqueColors[bmin]);
				UniqueColors.RemoveAt(amin);
				if (bmin > amin) bmin--;
				UniqueColors.RemoveAt(bmin);
				UniqueColors.Add(e);
			}
			if (FirstTransparent)// && Colors[0].A != 0)
			{
				UniqueColors.Insert(0, SKColor.Empty);
			}
			UniqueColors.AddRange(new SKColor[NrOutputColors - UniqueColors.Count]);
			return UniqueColors;
		}

	    static int ColorDifference(SKColor c1, SKColor c2)
		{
			return Pow2(c1.Red - c2.Red) + Pow2(c1.Green - c2.Green) + Pow2(c1.Blue - c2.Blue);
		}

		static int Pow2(int input)
		{
			return input * input;
		}

		static SKColor ColorMean(SKColor a, SKColor b)
		{
			return new SKColor((byte)((a.Red + b.Red) >> 1), (byte)((a.Green + b.Green) >> 1), (byte)((a.Blue + b.Blue) >> 1));
		}
	}
}
