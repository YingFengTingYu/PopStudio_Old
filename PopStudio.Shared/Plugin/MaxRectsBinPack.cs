using System;
using System.Collections;
using System.Collections.Generic;

namespace PopStudio.Plugin
{
	/// <summary>
	/// From http://wiki.unity3d.com/index.php/MaxRectsBinPack
	/// By Web Archive https://web.archive.org/web/20141003045928/http://wiki.unity3d.com/index.php/MaxRectsBinPack
	/// </summary>
	internal class MaxRectsBinPack
	{
		public int binWidth = 0;
		public int binHeight = 0;
		public bool allowRotations;

		public List<SubImageInfo> usedRectangles = new List<SubImageInfo>();
		public List<SubImageInfo> freeRectangles = new List<SubImageInfo>();

		public enum FreeRectChoiceHeuristic
		{
			RectBestShortSideFit, //BSSF: Positions the rectangle against the short side of a free rectangle into which it fits the best.
			RectBestLongSideFit, //BLSF: Positions the rectangle against the long side of a free rectangle into which it fits the best.
			RectBestAreaFit, //BAF: Positions the rectangle into the smallest free rect into which it fits.
			RectBottomLeftRule, //BL: Does the Tetris placement.
			RectContactPointRule //CP: Choosest the placement where the rectangle touches other rects as much as possible.
		};

		public MaxRectsBinPack(int width, int height, bool rotations = true)
		{
			Init(width, height, rotations);
		}

		public void Init(int width, int height, bool rotations = true)
		{
			binWidth = width;
			binHeight = height;
			allowRotations = rotations;

			SubImageInfo n = new SubImageInfo();
			n.X = 0;
			n.Y = 0;
			n.Width = width;
			n.Height = height;

			usedRectangles.Clear();

			freeRectangles.Clear();
			freeRectangles.Add(n);
		}

		public SubImageInfo Insert(SubImageInfo info, FreeRectChoiceHeuristic method)
		{
			int width = info.Width + 1;
			int height = info.Height + 1;
			SubImageInfo newNode = new SubImageInfo();
			int score1 = 0; // Unused in this function. We don't need to know the score after finding the position.
			int score2 = 0;
			switch (method)
			{
				case FreeRectChoiceHeuristic.RectBestShortSideFit: newNode = FindPositionForNewNodeBestShortSideFit(width, height, ref score1, ref score2); break;
				case FreeRectChoiceHeuristic.RectBottomLeftRule: newNode = FindPositionForNewNodeBottomLeft(width, height, ref score1, ref score2); break;
				case FreeRectChoiceHeuristic.RectContactPointRule: newNode = FindPositionForNewNodeContactPoint(width, height, ref score1); break;
				case FreeRectChoiceHeuristic.RectBestLongSideFit: newNode = FindPositionForNewNodeBestLongSideFit(width, height, ref score2, ref score1); break;
				case FreeRectChoiceHeuristic.RectBestAreaFit: newNode = FindPositionForNewNodeBestAreaFit(width, height, ref score1, ref score2); break;
			}

			if (newNode.Height == 0)
				return newNode;

			int numRectanglesToProcess = freeRectangles.Count;
			for (int i = 0; i < numRectanglesToProcess; ++i)
			{
				if (SplitFreeNode(freeRectangles[i], ref newNode))
				{
					freeRectangles.RemoveAt(i);
					--i;
					--numRectanglesToProcess;
				}
			}

			PruneFreeList();

			usedRectangles.Add(newNode);
			newNode.ID = info.ID;
			newNode.Image = info.Image;
			return newNode;
		}

		public void Insert(List<SubImageInfo> rects, List<SubImageInfo> dst, FreeRectChoiceHeuristic method)
		{
			dst.Clear();

			while (rects.Count > 0)
			{
				int bestScore1 = int.MaxValue;
				int bestScore2 = int.MaxValue;
				int bestRectIndex = -1;
				SubImageInfo bestNode = new SubImageInfo();

				for (int i = 0; i < rects.Count; ++i)
				{
					int score1 = 0;
					int score2 = 0;
					SubImageInfo newNode = ScoreRect(rects[i].Width, rects[i].Height, method, ref score1, ref score2);

					if (score1 < bestScore1 || (score1 == bestScore1 && score2 < bestScore2))
					{
						bestScore1 = score1;
						bestScore2 = score2;
						bestNode = newNode;
						bestRectIndex = i;
					}
				}

				if (bestRectIndex == -1)
					return;

				PlaceRect(bestNode);
				rects.RemoveAt(bestRectIndex);
			}
		}

		void PlaceRect(SubImageInfo node)
		{
			int numRectanglesToProcess = freeRectangles.Count;
			for (int i = 0; i < numRectanglesToProcess; ++i)
			{
				if (SplitFreeNode(freeRectangles[i], ref node))
				{
					freeRectangles.RemoveAt(i);
					--i;
					--numRectanglesToProcess;
				}
			}

			PruneFreeList();

			usedRectangles.Add(node);
		}

		SubImageInfo ScoreRect(int width, int height, FreeRectChoiceHeuristic method, ref int score1, ref int score2)
		{
			SubImageInfo newNode = new SubImageInfo();
			score1 = int.MaxValue;
			score2 = int.MaxValue;
			switch (method)
			{
				case FreeRectChoiceHeuristic.RectBestShortSideFit: newNode = FindPositionForNewNodeBestShortSideFit(width, height, ref score1, ref score2); break;
				case FreeRectChoiceHeuristic.RectBottomLeftRule: newNode = FindPositionForNewNodeBottomLeft(width, height, ref score1, ref score2); break;
				case FreeRectChoiceHeuristic.RectContactPointRule:
					newNode = FindPositionForNewNodeContactPoint(width, height, ref score1);
					score1 = -score1; // Reverse since we are minimizing, but for contact point score bigger is better.
					break;
				case FreeRectChoiceHeuristic.RectBestLongSideFit: newNode = FindPositionForNewNodeBestLongSideFit(width, height, ref score2, ref score1); break;
				case FreeRectChoiceHeuristic.RectBestAreaFit: newNode = FindPositionForNewNodeBestAreaFit(width, height, ref score1, ref score2); break;
			}

			// Cannot fit the current rectangle.
			if (newNode.Height == 0)
			{
				score1 = int.MaxValue;
				score2 = int.MaxValue;
			}

			return newNode;
		}

		/// Computes the ratio of used surface area.
		public float Occupancy()
		{
			ulong usedSurfaceArea = 0;
			for (int i = 0; i < usedRectangles.Count; ++i)
				usedSurfaceArea += (uint)usedRectangles[i].Width * (uint)usedRectangles[i].Height;

			return (float)usedSurfaceArea / (binWidth * binHeight);
		}

		SubImageInfo FindPositionForNewNodeBottomLeft(int width, int height, ref int bestY, ref int bestX)
		{
			SubImageInfo bestNode = new SubImageInfo();
			//memset(bestNode, 0, sizeof(Rect));

			bestY = int.MaxValue;

			for (int i = 0; i < freeRectangles.Count; ++i)
			{
				// Try to place the rectangle in upright (non-flipped) orientation.
				if (freeRectangles[i].Width >= width && freeRectangles[i].Height >= height)
				{
					int topSideY = (int)freeRectangles[i].Y + height;
					if (topSideY < bestY || (topSideY == bestY && freeRectangles[i].X < bestX))
					{
						bestNode.X = freeRectangles[i].X;
						bestNode.Y = freeRectangles[i].Y;
						bestNode.Width = width;
						bestNode.Height = height;
						bestY = topSideY;
						bestX = freeRectangles[i].X;
					}
				}
				if (allowRotations && freeRectangles[i].Width >= height && freeRectangles[i].Height >= width)
				{
					int topSideY = freeRectangles[i].Y + width;
					if (topSideY < bestY || (topSideY == bestY && freeRectangles[i].X < bestX))
					{
						bestNode.X = freeRectangles[i].X;
						bestNode.Y = freeRectangles[i].Y;
						bestNode.Width = height;
						bestNode.Height = width;
						bestY = topSideY;
						bestX = freeRectangles[i].X;
					}
				}
			}
			return bestNode;
		}

		SubImageInfo FindPositionForNewNodeBestShortSideFit(int width, int height, ref int bestShortSideFit, ref int bestLongSideFit)
		{
			SubImageInfo bestNode = new SubImageInfo();
			//memset(&bestNode, 0, sizeof(Rect));

			bestShortSideFit = int.MaxValue;

			for (int i = 0; i < freeRectangles.Count; ++i)
			{
				// Try to place the rectangle in upright (non-flipped) orientation.
				if (freeRectangles[i].Width >= width && freeRectangles[i].Height >= height)
				{
					int leftoverHoriz = Math.Abs(freeRectangles[i].Width - width);
					int leftoverVert = Math.Abs(freeRectangles[i].Height - height);
					int shortSideFit = Math.Min(leftoverHoriz, leftoverVert);
					int longSideFit = Math.Max(leftoverHoriz, leftoverVert);

					if (shortSideFit < bestShortSideFit || (shortSideFit == bestShortSideFit && longSideFit < bestLongSideFit))
					{
						bestNode.X = freeRectangles[i].X;
						bestNode.Y = freeRectangles[i].Y;
						bestNode.Width = width;
						bestNode.Height = height;
						bestShortSideFit = shortSideFit;
						bestLongSideFit = longSideFit;
					}
				}

				if (allowRotations && freeRectangles[i].Width >= height && freeRectangles[i].Height >= width)
				{
					int flippedLeftoverHoriz = Math.Abs((int)freeRectangles[i].Width - height);
					int flippedLeftoverVert = Math.Abs((int)freeRectangles[i].Height - width);
					int flippedShortSideFit = Math.Min(flippedLeftoverHoriz, flippedLeftoverVert);
					int flippedLongSideFit = Math.Max(flippedLeftoverHoriz, flippedLeftoverVert);

					if (flippedShortSideFit < bestShortSideFit || (flippedShortSideFit == bestShortSideFit && flippedLongSideFit < bestLongSideFit))
					{
						bestNode.X = freeRectangles[i].X;
						bestNode.Y = freeRectangles[i].Y;
						bestNode.Width = height;
						bestNode.Height = width;
						bestShortSideFit = flippedShortSideFit;
						bestLongSideFit = flippedLongSideFit;
					}
				}
			}
			return bestNode;
		}

		SubImageInfo FindPositionForNewNodeBestLongSideFit(int width, int height, ref int bestShortSideFit, ref int bestLongSideFit)
		{
			SubImageInfo bestNode = new SubImageInfo();
			//memset(&bestNode, 0, sizeof(Rect));

			bestLongSideFit = int.MaxValue;

			for (int i = 0; i < freeRectangles.Count; ++i)
			{
				// Try to place the rectangle in upright (non-flipped) orientation.
				if (freeRectangles[i].Width >= width && freeRectangles[i].Height >= height)
				{
					int leftoverHoriz = Math.Abs(freeRectangles[i].Width - width);
					int leftoverVert = Math.Abs(freeRectangles[i].Height - height);
					int shortSideFit = Math.Min(leftoverHoriz, leftoverVert);
					int longSideFit = Math.Max(leftoverHoriz, leftoverVert);

					if (longSideFit < bestLongSideFit || (longSideFit == bestLongSideFit && shortSideFit < bestShortSideFit))
					{
						bestNode.X = freeRectangles[i].X;
						bestNode.Y = freeRectangles[i].Y;
						bestNode.Width = width;
						bestNode.Height = height;
						bestShortSideFit = shortSideFit;
						bestLongSideFit = longSideFit;
					}
				}

				if (allowRotations && freeRectangles[i].Width >= height && freeRectangles[i].Height >= width)
				{
					int leftoverHoriz = Math.Abs(freeRectangles[i].Width - height);
					int leftoverVert = Math.Abs(freeRectangles[i].Height - width);
					int shortSideFit = Math.Min(leftoverHoriz, leftoverVert);
					int longSideFit = Math.Max(leftoverHoriz, leftoverVert);

					if (longSideFit < bestLongSideFit || (longSideFit == bestLongSideFit && shortSideFit < bestShortSideFit))
					{
						bestNode.X = freeRectangles[i].X;
						bestNode.Y = freeRectangles[i].Y;
						bestNode.Width = height;
						bestNode.Height = width;
						bestShortSideFit = shortSideFit;
						bestLongSideFit = longSideFit;
					}
				}
			}
			return bestNode;
		}

		SubImageInfo FindPositionForNewNodeBestAreaFit(int width, int height, ref int bestAreaFit, ref int bestShortSideFit)
		{
			SubImageInfo bestNode = new SubImageInfo();
			bestAreaFit = int.MaxValue;

			for (int i = 0; i < freeRectangles.Count; ++i)
			{
				int areaFit = (int)freeRectangles[i].Width * (int)freeRectangles[i].Height - width * height;

				// Try to place the rectangle in upright (non-flipped) orientation.
				if (freeRectangles[i].Width >= width && freeRectangles[i].Height >= height)
				{
					int leftoverHoriz = Math.Abs(freeRectangles[i].Width - width);
					int leftoverVert = Math.Abs(freeRectangles[i].Height - height);
					int shortSideFit = Math.Min(leftoverHoriz, leftoverVert);

					if (areaFit < bestAreaFit || (areaFit == bestAreaFit && shortSideFit < bestShortSideFit))
					{
						bestNode.X = freeRectangles[i].X;
						bestNode.Y = freeRectangles[i].Y;
						bestNode.Width = width;
						bestNode.Height = height;
						bestShortSideFit = shortSideFit;
						bestAreaFit = areaFit;
					}
				}

				if (allowRotations && freeRectangles[i].Width >= height && freeRectangles[i].Height >= width)
				{
					int leftoverHoriz = Math.Abs(freeRectangles[i].Width - height);
					int leftoverVert = Math.Abs(freeRectangles[i].Height - width);
					int shortSideFit = Math.Min(leftoverHoriz, leftoverVert);

					if (areaFit < bestAreaFit || (areaFit == bestAreaFit && shortSideFit < bestShortSideFit))
					{
						bestNode.X = freeRectangles[i].X;
						bestNode.Y = freeRectangles[i].Y;
						bestNode.Width = height;
						bestNode.Height = width;
						bestShortSideFit = shortSideFit;
						bestAreaFit = areaFit;
					}
				}
			}
			return bestNode;
		}

		/// Returns 0 if the two intervals i1 and i2 are disjoint, or the length of their overlap otherwise.
		int CommonIntervalLength(int i1start, int i1end, int i2start, int i2end)
		{
			if (i1end < i2start || i2end < i1start)
				return 0;
			return Math.Min(i1end, i2end) - Math.Max(i1start, i2start);
		}

		int ContactPointScoreNode(int x, int y, int width, int height)
		{
			int score = 0;

			if (x == 0 || x + width == binWidth)
				score += height;
			if (y == 0 || y + height == binHeight)
				score += width;

			for (int i = 0; i < usedRectangles.Count; ++i)
			{
				if (usedRectangles[i].X == x + width || usedRectangles[i].X + usedRectangles[i].Width == x)
					score += CommonIntervalLength(usedRectangles[i].Y, usedRectangles[i].Y + usedRectangles[i].Height, y, y + height);
				if (usedRectangles[i].Y == y + height || usedRectangles[i].Y + usedRectangles[i].Height == y)
					score += CommonIntervalLength(usedRectangles[i].X, usedRectangles[i].X + usedRectangles[i].Width, x, x + width);
			}
			return score;
		}

		SubImageInfo FindPositionForNewNodeContactPoint(int width, int height, ref int bestContactScore)
		{
			SubImageInfo bestNode = new SubImageInfo();

			bestContactScore = -1;

			for (int i = 0; i < freeRectangles.Count; ++i)
			{
				// Try to place the rectangle in upright (non-flipped) orientation.
				if (freeRectangles[i].Width >= width && freeRectangles[i].Height >= height)
				{
					int score = ContactPointScoreNode(freeRectangles[i].X, freeRectangles[i].Y, width, height);
					if (score > bestContactScore)
					{
						bestNode.X = freeRectangles[i].X;
						bestNode.Y = freeRectangles[i].Y;
						bestNode.Width = width;
						bestNode.Height = height;
						bestContactScore = score;
					}
				}
				if (allowRotations && freeRectangles[i].Width >= height && freeRectangles[i].Height >= width)
				{
					int score = ContactPointScoreNode(freeRectangles[i].X, freeRectangles[i].Y, height, width);
					if (score > bestContactScore)
					{
						bestNode.X = freeRectangles[i].X;
						bestNode.Y = freeRectangles[i].Y;
						bestNode.Width = height;
						bestNode.Height = width;
						bestContactScore = score;
					}
				}
			}
			return bestNode;
		}

		bool SplitFreeNode(SubImageInfo freeNode, ref SubImageInfo usedNode)
		{
			// Test with SAT if the rectangles even intersect.
			if (usedNode.X >= freeNode.X + freeNode.Width || usedNode.X + usedNode.Width <= freeNode.X ||
				usedNode.Y >= freeNode.Y + freeNode.Height || usedNode.Y + usedNode.Height <= freeNode.Y)
				return false;

			if (usedNode.X < freeNode.X + freeNode.Width && usedNode.X + usedNode.Width > freeNode.X)
			{
				// New node at the top side of the used node.
				if (usedNode.Y > freeNode.Y && usedNode.Y < freeNode.Y + freeNode.Height)
				{
					SubImageInfo newNode = freeNode;
					newNode.Height = usedNode.Y - newNode.Y;
					freeRectangles.Add(newNode);
				}

				// New node at the bottom side of the used node.
				if (usedNode.Y + usedNode.Height < freeNode.Y + freeNode.Height)
				{
					SubImageInfo newNode = freeNode;
					newNode.Y = usedNode.Y + usedNode.Height;
					newNode.Height = freeNode.Y + freeNode.Height - (usedNode.Y + usedNode.Height);
					freeRectangles.Add(newNode);
				}
			}

			if (usedNode.Y < freeNode.Y + freeNode.Height && usedNode.Y + usedNode.Height > freeNode.Y)
			{
				// New node at the left side of the used node.
				if (usedNode.X > freeNode.X && usedNode.X < freeNode.X + freeNode.Width)
				{
					SubImageInfo newNode = freeNode;
					newNode.Width = usedNode.X - newNode.X;
					freeRectangles.Add(newNode);
				}

				// New node at the right side of the used node.
				if (usedNode.X + usedNode.Width < freeNode.X + freeNode.Width)
				{
					SubImageInfo newNode = freeNode;
					newNode.X = usedNode.X + usedNode.Width;
					newNode.Width = freeNode.X + freeNode.Width - (usedNode.X + usedNode.Width);
					freeRectangles.Add(newNode);
				}
			}
			return true;
		}

		void PruneFreeList()
		{
			for (int i = 0; i < freeRectangles.Count; ++i)
				for (int j = i + 1; j < freeRectangles.Count; ++j)
				{
					if (IsContainedIn(freeRectangles[i], freeRectangles[j]))
					{
						freeRectangles.RemoveAt(i);
						--i;
						break;
					}
					if (IsContainedIn(freeRectangles[j], freeRectangles[i]))
					{
						freeRectangles.RemoveAt(j);
						--j;
					}
				}
		}

		bool IsContainedIn(SubImageInfo a, SubImageInfo b) => a.X >= b.X && a.Y >= b.Y && a.X + a.Width <= b.X + b.Width && a.Y + a.Height <= b.Y + b.Height;
	}
}
