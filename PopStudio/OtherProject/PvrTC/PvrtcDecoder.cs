using System.Runtime.CompilerServices;

namespace Pvrtc
{
	/// <summary>
	/// PvrTC的C#实现，依赖于SkiaSharp
	/// </summary>
	public class PvrtcDecoder
	{
		private struct AmtcBlock
		{
			public AmtcBlock(uint v0, uint v1)
			{
				PackedData0 = v0;
				PackedData1 = v1;
			}

			// Uses 64 bits pre block
			public readonly uint PackedData0;
			public readonly uint PackedData1;
		}

		/// <summary>
		/// Low precision colours extracted from the blocks
		/// </summary>
		private struct Colours5554
		{
			public int[,] Reps;
		}

		public PvrtcDecoder()
		{
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					m_colors[i, j].Reps = new int[2, 4];
				}
			}
		}

		public SkiaSharp.SKColor[] DecompressPVRTC(byte[] input, int xDim, int yDim, bool do2bitMode)
		{
			AmtcBlock[] blocks = GenerateBlocks(input);
			SkiaSharp.SKColor[] output = new SkiaSharp.SKColor[xDim * yDim];
			PVRDecompress(blocks, do2bitMode, xDim, yDim, output);
			return output;
		}

		public void DecompressPVRTC(byte[] input, int xDim, int yDim, SkiaSharp.SKColor[] output, bool do2bitMode)
		{
			AmtcBlock[] blocks = GenerateBlocks(input);
			PVRDecompress(blocks, do2bitMode, xDim, yDim, output);
		}

		private static AmtcBlock[] GenerateBlocks(byte[] input)
		{
			AmtcBlock[] blocks = new AmtcBlock[input.Length / 8];
			using (MemoryStream ms = new MemoryStream(input))
			{
				using (BinaryReader reader = new BinaryReader(ms))
				{
					int i = 0;
					while (reader.BaseStream.Position != reader.BaseStream.Length)
					{
						uint v0 = reader.ReadUInt32();
						uint v1 = reader.ReadUInt32();
						AmtcBlock block = new AmtcBlock(v0, v1);
						blocks[i++] = block;
					}
				}
			}
			return blocks;
		}

		/// <summary>
		/// Decompresses PVRTC to RGBA 8888
		/// </summary>
		/// <param name="compressedData">The PVRTC texture data to decompress</param>
		/// <param name="do2bitMode">Signifies whether the data is PVRTC2 or PVRTC4</param>
		/// <param name="xDim">X dimension (width) of the texture</param>
		/// <param name="yDim">Y dimension (height) of the texture</param>
		/// <param name="output">The decompressed texture data</param>
		private void PVRDecompress(AmtcBlock[] compressedData, bool do2bitMode, int xDim, int yDim, SkiaSharp.SKColor[] output)
		{
			int xBlockSize = do2bitMode ? BlockX2bpp : BlockX4bpp;
			// for MBX don't allow the sizes to get too small
			int blockXDim = Math.Max(2, xDim / xBlockSize);
			int blockYDim = Math.Max(2, yDim / BlockYSize);

			uint pblock00 = uint.MaxValue;
			uint pblock01 = uint.MaxValue;
			uint pblock10 = uint.MaxValue;
			uint pblock11 = uint.MaxValue;

			// step through the pixels of the image decompressing each one in turn.
			// Note that this is a hideously inefficient way to do this!
			for (int y = 0; y < yDim; y++)
			{
				for (int x = 0; x < xDim; x++)
				{
					// map this pixel to the top left neighbourhood of blocks
					int blockX = (x - xBlockSize / 2);
					int blockY = (y - BlockYSize / 2);

					blockX = LimitCoord(blockX, xDim) / xBlockSize;
					blockY = LimitCoord(blockY, yDim) / BlockYSize;

					// compute the positions of the other 3 blocks
					int blockXp1 = LimitCoord(blockX + 1, blockXDim);
					int blockYp1 = LimitCoord(blockY + 1, blockYDim);

					// map to block memory locations
					uint block00 = TwiddleUV((uint)blockYDim, (uint)blockXDim, (uint)blockY, (uint)blockX);
					uint block01 = TwiddleUV((uint)blockYDim, (uint)blockXDim, (uint)blockY, (uint)blockXp1);
					uint block10 = TwiddleUV((uint)blockYDim, (uint)blockXDim, (uint)blockYp1, (uint)blockX);
					uint block11 = TwiddleUV((uint)blockYDim, (uint)blockXDim, (uint)blockYp1, (uint)blockXp1);

					// extract the colours and the modulation information IF the previous values have changed.
					bool changed = block00 != pblock00 || block01 != pblock01 || block10 != pblock10 || block11 != pblock11;
					if (changed)
					{
						Unpack5554Colour(compressedData[block00], m_colors[0, 0].Reps);
						UnpackModulations(compressedData[block00], do2bitMode, m_modulationVals, m_modulationModes, 0, 0);
						Unpack5554Colour(compressedData[block01], m_colors[0, 1].Reps);
						UnpackModulations(compressedData[block01], do2bitMode, m_modulationVals, m_modulationModes, xBlockSize, 0);
						Unpack5554Colour(compressedData[block10], m_colors[1, 0].Reps);
						UnpackModulations(compressedData[block10], do2bitMode, m_modulationVals, m_modulationModes, 0, BlockYSize);
						Unpack5554Colour(compressedData[block11], m_colors[1, 1].Reps);
						UnpackModulations(compressedData[block11], do2bitMode, m_modulationVals, m_modulationModes, xBlockSize, BlockYSize);

						pblock00 = block00;
						pblock01 = block01;
						pblock10 = block10;
						pblock11 = block11;
					}

					// decompress the pixel.  First compute the interpolated A and B signals
					InterpolateColours(m_colors[0, 0].Reps, m_colors[0, 1].Reps, m_colors[1, 0].Reps, m_colors[1, 1].Reps, 0, do2bitMode, x, y, m_aSig);
					InterpolateColours(m_colors[0, 0].Reps, m_colors[0, 1].Reps, m_colors[1, 0].Reps, m_colors[1, 1].Reps, 1, do2bitMode, x, y, m_bSig);
					GetModulationValue(x, y, do2bitMode, m_modulationVals, m_modulationModes, out int mod, out bool doPT);

					// compute the modulated color. Swap red and blue channel
					output[x + y * xDim] = new SkiaSharp.SKColor((byte)((m_aSig[0] * 8 + mod * (m_bSig[0] - m_aSig[0])) >> 3), (byte)((m_aSig[1] * 8 + mod * (m_bSig[1] - m_aSig[1])) >> 3), (byte)((m_aSig[2] * 8 + mod * (m_bSig[2] - m_aSig[2])) >> 3), doPT ? (byte)0 : (byte)((m_aSig[3] * 8 + mod * (m_bSig[3] - m_aSig[3])) >> 3));
				}
			}
		}

		/// <summary>
		/// Given the Block (or pixel) coordinates and the dimension of the texture in blocks (or pixels) this returns
		/// the twiddled offset of the block (or pixel) from the start of the map.
		/// <para>NOTE the dimensions of the texture must be a power of 2</para>
		/// </summary>
		/// <param name="ySize">Y dimension of the texture in pixels</param>
		/// <param name="xSize">X dimension of the texture in pixels</param>
		/// <param name="yPos">Pixel Y position</param>
		/// <param name="xPos">Pixel X position</param>
		/// <returns>The twiddled offset of the pixel</returns>
		private static uint TwiddleUV(uint ySize, uint xSize, uint yPos, uint xPos)
		{
			uint minDimension;
			uint maxValue;
			if (ySize < xSize)
			{
				minDimension = ySize;
				maxValue = xPos;
			}
			else
			{
				minDimension = xSize;
				maxValue = yPos;
			}
			// step through all the bits in the "minimum" dimension
			uint srcBitPos = 1;
			uint dstBitPos = 1;
			uint twiddled = 0;
			int shiftCount = 0;

			while (srcBitPos < minDimension)
			{
				if ((yPos & srcBitPos) != 0)
				{
					twiddled |= dstBitPos;
				}

				if ((xPos & srcBitPos) != 0)
				{
					twiddled |= (dstBitPos << 1);
				}

				srcBitPos <<= 1;
				dstBitPos <<= 2;
				shiftCount += 1;
			}

			// prepend any unused bits
			maxValue >>= shiftCount;
			twiddled |= (maxValue << (2 * shiftCount));
			return twiddled;
		}

		/// <summary>
		/// Get the modulation value as a numerator of a fraction of 8ths
		/// </summary>
		private void GetModulationValue(int x, int y, bool do2bitMode, int[,] modulationVals, int[,] modulationModes, out int mod, out bool doPT)
		{
			// Map X and Y into the local 2x2 block
			y = (y & 0x3) | ((~y & 0x2) << 1);

			if (do2bitMode)
				x = (x & 0x7) | ((~x & 0x4) << 1);
			else
				x = (x & 0x3) | ((~x & 0x2) << 1);

			// assume no PT for now
			doPT = false;

			// extract the modulation value. If a simple encoding
			int modVal;
			if (modulationModes[y, x] == 0)
			{
				modVal = m_repVals0[modulationVals[y, x]];
			}
			else if (do2bitMode)
			{
				// if this is a stored value
				if (((x ^ y) & 1) == 0)
				{
					modVal = m_repVals0[modulationVals[y, x]];
				}
				// else average from the neighbours if H&V interpolation..
				else if (modulationModes[y, x] == 1)
				{
					modVal = (m_repVals0[modulationVals[y - 1, x]] + m_repVals0[modulationVals[y + 1, x]] +
						m_repVals0[modulationVals[y, x - 1]] + m_repVals0[modulationVals[y, x + 1]] + 2) / 4;
				}
				// else if H-Only
				else if (modulationModes[y, x] == 2)
				{
					modVal = (m_repVals0[modulationVals[y, x - 1]] + m_repVals0[modulationVals[y, x + 1]] + 1) / 2;
				}
				// else it's V-Only
				else
				{
					modVal = (m_repVals0[modulationVals[y - 1, x]] + m_repVals0[modulationVals[y + 1, x]] + 1) / 2;
				}
			}
			// else it's 4BPP and PT encoding
			else
			{
				modVal = m_repVals1[modulationVals[y, x]];
				doPT = modulationVals[y, x] == PTIndex;
			}

			mod = modVal;
		}

		/// <summary>
		/// This performs a HW bit accurate interpolation of either the A or B colours for a particular pixel
		/// <para>NOTE: It is assumed that the source colours are in ARGB 5554 format - This means that some "preparation" of the values will be necessary</para>
		/// </summary>
		private static void InterpolateColours(int[,] colorP, int[,] colorQ, int[,] colorR, int[,] colorS, int ci, bool do2bitMode, int x, int y, int[] result)
		{
			// put the x and y values into the right range and get the u and v scale amounts
			int v = ((y & 0x3) | ((~y & 0x2) << 1)) - BlockYSize / 2;
			int u = do2bitMode ? (((x & 0x7) | ((~x & 0x4) << 1)) - BlockX2bpp / 2) : (((x & 0x3) | ((~x & 0x2) << 1)) - BlockX4bpp / 2);
			int uscale = do2bitMode ? 8 : 4;

			for (int i = 0; i < 4; i++)
			{
				int tmp1 = colorP[ci, i] * uscale + u * (colorQ[ci, i] - colorP[ci, i]);
				int tmp2 = colorR[ci, i] * uscale + u * (colorS[ci, i] - colorR[ci, i]);
				result[i] = tmp1 * 4 + v * (tmp2 - tmp1);
			}

			// lop off the appropriate number of bits to get us to 8 bit precision
			if (do2bitMode)
			{
				// do RGB
				for (int i = 0; i < 3; i++)
				{
					result[i] >>= 2;
				}
				result[3] >>= 1;
			}
			else
			{
				// do RGB  (A is ok)
				for (int i = 0; i < 3; i++)
				{
					result[i] >>= 1;
				}
			}

			// convert from 5554 to 8888, do RGB 5.3 => 8
			for (int i = 0; i < 3; i++)
			{
				result[i] += result[i] >> 5;
			}
			result[3] += result[3] >> 4;
		}

		/// <summary>
		/// Given the block and the texture type and it's relative position in the 2x2 group of blocks, extract the bit patterns for the fully defined pixels.
		/// </summary>
		private static void UnpackModulations(AmtcBlock block, bool do2bitMode, int[,] modulationVals, int[,] modulationModes, int startX, int startY)
		{
			int blockModMode = (int)(block.PackedData1 & 1);
			uint modulationBits = block.PackedData0;

			// if it's in an interpolated mode
			if (do2bitMode && (blockModMode != 0))
			{
				// run through all the pixels in the block. Note we can now treat all the "stored" values as if they have 2bits (even when they didn't!)
				for (int y = 0; y < BlockYSize; y++)
				{
					for (int x = 0; x < BlockX2bpp; x++)
					{
						modulationModes[y + startY, x + startX] = blockModMode;

						// if this is a stored value...
						if (((x ^ y) & 1) == 0)
						{
							modulationVals[y + startY, x + startX] = (int)(modulationBits & 3);
							modulationBits >>= 2;
						}
					}
				}
			}
			// else if direct encoded 2bit mode - i.e. 1 mode bit per pixel
			else if (do2bitMode)
			{
				for (int y = 0; y < BlockYSize; y++)
				{
					for (int x = 0; x < BlockX2bpp; x++)
					{
						modulationModes[y + startY, x + startX] = blockModMode;

						// double the bits so 0=> 00, and 1=>11
						if ((modulationBits & 1) != 0)
						{
							modulationVals[y + startY, x + startX] = 0x3;
						}
						else
						{
							modulationVals[y + startY, x + startX] = 0x0;
						}
						modulationBits >>= 1;
					}
				}
			}
			// else its the 4bpp mode so each value has 2 bits
			else
			{
				for (int y = 0; y < BlockYSize; y++)
				{
					for (int x = 0; x < BlockX4bpp; x++)
					{
						modulationModes[y + startY, x + startX] = blockModMode;

						modulationVals[y + startY, x + startX] = (int)(modulationBits & 3);
						modulationBits >>= 2;
					}
				}
			}

			// make sure nothing is left over
			if (modulationBits != 0)
			{
				throw new Exception("Something is left over");
			}
		}

		/// <summary>
		/// Given a block, extract the colour information and convert to 5554 formats
		/// </summary>
		private void Unpack5554Colour(AmtcBlock block, int[,] abColors)
		{
			// extract A and B
			// 15 bits (shifted up by one)
			m_rawBits[0] = block.PackedData1 & (0xFFFE);
			// 16 bits
			m_rawBits[1] = block.PackedData1 >> 16;

			// step through both colours
			for (int i = 0; i < 2; i++)
			{
				// if completely opaque
				if ((m_rawBits[i] & (1 << 15)) != 0)
				{
					// extract R and G (both 5 bit)
					abColors[i, 0] = (int)((m_rawBits[i] >> 10) & 0x1F);
					abColors[i, 1] = (int)((m_rawBits[i] >> 5) & 0x1F);

					// The precision of Blue depends on  A or B. If A then we need to replicate the top bit to get 5 bits in total
					abColors[i, 2] = (int)(m_rawBits[i] & 0x1F);
					if (i == 0)
					{
						abColors[0, 2] |= abColors[0, 2] >> 4;
					}

					// set 4bit alpha fully on...
					abColors[i, 3] = 0xF;
				}
				// else if colour has variable translucency
				else
				{
					// extract R and G (both 4 bit). Leave a space on the end for the replication of bits
					abColors[i, 0] = (int)((m_rawBits[i] >> (8 - 1)) & 0x1E);
					abColors[i, 1] = (int)((m_rawBits[i] >> (4 - 1)) & 0x1E);

					// replicate bits to truly expand to 5 bits
					abColors[i, 0] |= abColors[i, 0] >> 4;
					abColors[i, 1] |= abColors[i, 1] >> 4;

					// grab the 3(+padding) or 4 bits of blue and add an extra padding bit
					abColors[i, 2] = (int)((m_rawBits[i] & 0xF) << 1);

					// expand from 3 to 5 bits if this is from colour A, or 4 to 5 bits if from colour B
					if (i == 0)
					{
						abColors[0, 2] |= abColors[0, 2] >> 3;
					}
					else
					{
						abColors[0, 2] |= abColors[0, 2] >> 4;
					}

					// set the alpha bits to be 3 + a zero on the end
					abColors[i, 3] = (int)((m_rawBits[i] >> 11) & 0xE);
				}
			}
		}

		/// <summary>
		/// Check that a number is an integer power of two, i.e.  1, 2, 4, 8, ... etc. Returns false for zero
		/// </summary>
		/// <param name="input">A number</param>
		/// <returns>True if the number is an integer power of two, else false</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsPowerOf2(uint input)
		{
			if (input == 0)
			{
				return false;
			}

			uint minus1 = input - 1;
			return ((input | minus1) == (input ^ minus1));
		}

		/// <summary>
		/// Define an expression to either wrap or clamp large or small vals to the legal coordinate range
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int LimitCoord(int value, int size)
		{
			// wrap coord
			return value & (size - 1);
		}

		/// <summary>
		/// The Punch-through index
		/// </summary>
		private const int PTIndex = 2;
		/// <summary>
		/// Always 4 for all 2D block types
		/// </summary>
		private const int BlockYSize = 4;
		/// <summary>
		/// Max X dimension for blocks
		/// </summary>
		private const int BlockXMax = 8;
		// Dimensions for the two formats
		private const int BlockX2bpp = 8;
		private const int BlockX4bpp = 4;

		private readonly IReadOnlyList<int> m_repVals0 = new int[] { 0, 3, 5, 8 };
		private readonly IReadOnlyList<int> m_repVals1 = new int[] { 0, 4, 4, 8 };

		private readonly Colours5554[,] m_colors = new Colours5554[2, 2];
		private readonly int[,] m_modulationVals = new int[8, 16];
		private readonly int[,] m_modulationModes = new int[8, 16];
		/// <summary>
		/// Interpolated A colors for the pixel
		/// </summary>
		private readonly int[] m_aSig = new int[4];
		/// <summary>
		/// Interpolated B colors for the pixel
		/// </summary>
		private readonly int[] m_bSig = new int[4];
		private readonly uint[] m_rawBits = new uint[2];
	}
}
