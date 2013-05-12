using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DesktopDungeons
{
	/// <summary>
	/// Generate map
	/// </summary>
	public class Generator
	{
		/// <summary>
		/// Proportion of count free block on map from wall
		/// </summary>
		private const int MAX_PROPORTION = 52;

		/// <summary>
		/// Instance of random class
		/// </summary>
		private Random rand;

		/// <summary>
		/// Map of game
		/// </summary>
		private short[,] map;

		/// <summary>
		/// Start point of hero
		/// </summary>
		private Point pointStartOfHero;

		/// <summary>
		/// Constructor
		/// </summary>
		public Generator()
		{
			rand = new Random();
			map = new short[Constants.MAP_SIZE, Constants.MAP_SIZE];
		}

		/// <summary>
		/// Generate of map
		/// </summary>
		/// <param name="outMap">Map array</param>
		/// <returns>Start point of hero</returns>
		public Point Generate(out short[,] outMap)
		{
			//generate pointStart
			pointStartOfHero = new Point(rand.Next(1, Constants.MAP_SIZE - 1), rand.Next(1, Constants.MAP_SIZE - 1));

			//initialization map
			for (int x = 0; x < Constants.MAP_SIZE; ++x)
			{
				for (int y = 0; y < Constants.MAP_SIZE; ++y)
				{
					map[x, y] = (int)MapItem.Wall;
				}
			}

			// N - lines
			DrawMainLine(true, 5);
			DrawMainLine(false, 5);
			CheckFigure(pointStartOfHero.X - 1, pointStartOfHero.Y - 1, 3, 3);

			// draw figures
			DrawFigures();

			//3x3
			for (int x = pointStartOfHero.X - 1; x <= pointStartOfHero.X + 1; ++x)
				for (int y = pointStartOfHero.Y - 1; y <= pointStartOfHero.Y + 1; ++y)
					map[x, y] = (int)MapItem.None;

			outMap = map;
			return pointStartOfHero;
		}

		/// <summary>
		/// Chech firure what it is intersects of not wall block on map
		/// if not interxects when draw line while is found not wall block
		/// </summary>
		/// <param name="parX">Figure X on map</param>
		/// <param name="parY">Fugure Y on map</param>
		/// <param name="parWidth">Width of figure</param>
		/// <param name="parHeight">Height of figure</param>
		private void CheckFigure(int parX, int parY, int parWidth, int parHeight)
		{
			for (int x = parX; x < parX + parWidth; ++x)
				for (int y = parY; y < parY + parHeight; ++y)
					if (map[x, y] == (int)MapItem.None)
						return;

			if (parX >= (Constants.MAP_SIZE / 2))
			{
				for (int x = parX - 1; x >= 0; --x)
				{
					if (map[x, parY + parHeight / 2] == (int)MapItem.None)
						break;
					else
						map[x, parY + parHeight / 2] = (int)MapItem.None;
				}
			}
			else
			{
				for (int x = parX + parWidth; x < Constants.MAP_SIZE; ++x)
				{
					if (map[x, parY + parHeight / 2] == (int)MapItem.None)
						break;
					else
						map[x, parY + parHeight / 2] = (int)MapItem.None;
				}
			}

			if (parY >= (Constants.MAP_SIZE / 2))
			{
				for (int y = parY; y >= 0; --y)
				{
					if (map[parX + parWidth / 2, y] == (int)MapItem.None)
						break;
					else
						map[parX + parWidth / 2, y] = (int)MapItem.None;
				}
			}
			else
			{
				for (int y = parY + parHeight; y < Constants.MAP_SIZE; ++y)
				{
					if (map[parX + parWidth / 2, y] == (int)MapItem.None)
						break;
					else
						map[parX + parWidth / 2, y] = (int)MapItem.None;
				}
			}
		}

		/// <summary>
		/// Draw figure on map array
		/// </summary>
		private void DrawFigures()
		{
			//.....
			//.0.0.
			//.0.0.
			//.0.0.
			//.....
			int x = rand.Next(0, Constants.MAP_SIZE - 4);
			int y = rand.Next(0, Constants.MAP_SIZE - 4);
			CheckFigure(x, y, 5, 5);

			for (int i = 0; i < 5; ++i)
			{
				map[x + i, y] = (int)MapItem.None;
				map[x + i, y + 4] = (int)MapItem.None;
			}
			for (int i = 1; i < 4; ++i)
			{
				map[x, y + i] = (int)MapItem.None;
				map[x + 1, y + i] = (int)MapItem.Wall;
				map[x + 2, y + i] = (int)MapItem.None;
				map[x + 3, y + i] = (int)MapItem.Wall;
				map[x + 4, y + i] = (int)MapItem.None;
			}

			//...
			//.0.
			//.0.
			//.0.
			//...
			x = rand.Next(0, Constants.MAP_SIZE - 2);
			y = rand.Next(0, Constants.MAP_SIZE - 4);
			CheckFigure(x, y, 3, 5);

			for (int i = 0; i < 3; ++i)
			{
				map[x + i, y] = (int)MapItem.None;
				map[x + i, y + 4] = (int)MapItem.None;
			}
			for (int i = 1; i < 4; ++i)
			{
				map[x, y + i] = (int)MapItem.None;
				map[x + 1, y + i] = (int)MapItem.Wall;
				map[x + 2, y + i] = (int)MapItem.None;
			}

			if (CalcWallProportion() < MAX_PROPORTION)
				return;

			//.0.
			//.0.
			//...
			x = rand.Next(0, Constants.MAP_SIZE - 2);
			y = rand.Next(0, Constants.MAP_SIZE - 2);
			CheckFigure(x, y, 3, 3);

			for (int i = 0; i < 3; ++i)
			{
				map[x + i, y + 2] = (int)MapItem.None;
			}
			for (int i = 0; i < 2; ++i)
			{
				map[x, y + i] = (int)MapItem.None; 
				map[x + 1, y + i] = (int)MapItem.Wall;
				map[x + 2, y + i] = (int)MapItem.None;
			}

			if (CalcWallProportion() < MAX_PROPORTION)
				return;

			//......
			//.0000.
			//......
			x = rand.Next(0, Constants.MAP_SIZE - 5);
			y = rand.Next(0, Constants.MAP_SIZE - 2);
			CheckFigure(x, y, 6, 3);

			for (int i = 0; i < 6; ++i)
			{
				map[x + i, y] = (int)MapItem.None;
				map[x + i, y + 2] = (int)MapItem.None;
			}
			for (int i = 1; i < 5; ++i)
			{
				map[x + i, y + 1] = (int)MapItem.Wall;
			}
			map[x, y + 1] = (int)MapItem.None;
			map[x + 5, y + 1] = (int)MapItem.None;

			if (CalcWallProportion() < MAX_PROPORTION)
				return;

			//...
			//00.
			//...
			x = rand.Next(0, Constants.MAP_SIZE - 2);
			y = rand.Next(0, Constants.MAP_SIZE - 2);
			CheckFigure(x, y, 3, 3);

			for (int i = 0; i < 3; ++i)
			{
				map[x + i, y] = (int)MapItem.None;
				map[x + i, y + 2] = (int)MapItem.None;
			}
			map[x, y + 1] = (int)MapItem.Wall;
			map[x + 1, y + 1] = (int)MapItem.Wall;
			map[x + 2, y + 1] = (int)MapItem.None;

			if (CalcWallProportion() < MAX_PROPORTION)
				return;

			//...
			//.0.
			//.0.
			x = rand.Next(0, Constants.MAP_SIZE - 2);
			y = rand.Next(0, Constants.MAP_SIZE - 2);
			CheckFigure(x, y, 3, 3);

			for (int i = 0; i < 3; ++i)
			{
				map[x + i, y] = (int)MapItem.None;
			}
			for (int i = 1; i < 3; ++i)
			{
				map[x, y + i] = (int)MapItem.None;
				map[x + 1, y + i] = (int)MapItem.Wall;
				map[x + 2, y + i] = (int)MapItem.None;
			}

			if (CalcWallProportion() < MAX_PROPORTION)
				return;

			//...
			//.0.
			//...
			x = rand.Next(0, Constants.MAP_SIZE - 2);
			y = rand.Next(0, Constants.MAP_SIZE - 2);
			CheckFigure(x, y, 3, 3);

			for (int i = 0; i < 3; ++i)
			{
				map[x + i, y] = (int)MapItem.None;
				map[x + i, y + 2] = (int)MapItem.None;
			}
			map[x, y + 1] = (int)MapItem.None;
			map[x + 1, y + 1] = (int)MapItem.Wall;
			map[x + 2, y + 1] = (int)MapItem.None;

			if (CalcWallProportion() < MAX_PROPORTION)
				return;

			//...
			//.00
			//...
			x = rand.Next(0, Constants.MAP_SIZE - 2);
			y = rand.Next(0, Constants.MAP_SIZE - 2);
			CheckFigure(x, y, 3, 3);

			for (int i = 0; i < 3; ++i)
			{
				map[x + i, y] = (int)MapItem.None;
				map[x + i, y + 2] = (int)MapItem.None;
			}
			map[x, y + 1] = (int)MapItem.None;
			map[x + 1, y + 1] = (int)MapItem.Wall;
			map[x + 2, y + 1] = (int)MapItem.Wall;

			if (CalcWallProportion() < MAX_PROPORTION)
				return;

			//.....
			//.....
			//.....
			//.....
			//.....
			for (int X = 0; X < 5; ++X)
				for (int Y = 0; Y < 5; ++Y)
					if (pointStartOfHero.X - 2 + X >= 0 
						&& pointStartOfHero.X - 2 + X < Constants.MAP_SIZE)
						if (pointStartOfHero.Y - 2 + Y >= 0 
							&& pointStartOfHero.Y - 2 + Y < Constants.MAP_SIZE)
							map[pointStartOfHero.X - 2 + X, pointStartOfHero.Y - 2 + Y] = (int)MapItem.None;


			if (CalcWallProportion() < MAX_PROPORTION)
				return;
		}

		/// <summary>
		/// Calc proportion count free block to all blocks on map
		/// </summary>
		/// <returns>Percent of count free block to all blocks on map</returns>
		private int CalcWallProportion()
		{
			int countWall = 0;
			for (int x = 0; x < Constants.MAP_SIZE; ++x)
				for (int y = 0; y < Constants.MAP_SIZE; ++y)
					if (map[x, y] == 1)
						++countWall;

			return countWall * 100 / (Constants.MAP_SIZE * Constants.MAP_SIZE);
		}

		/// <summary>
		/// Draw lines on map array
		/// </summary>
		/// <param name="orientationVertical">Is vertical line</param>
		/// <param name="parLineCount">Count lines</param>
		private void DrawMainLine(bool orientationVertical, int parLineCount)
		{
			for (int lineCount = 0; lineCount < parLineCount; ++lineCount)
			{
				int index;

				if (orientationVertical)
				{
					do
					{
						index = rand.Next(1, Constants.MAP_SIZE - 1);
					} while (map[0, index - 1] * map[0, index] * map[0, index + 1] == 0);
				}
				else
				{
					do
					{
						index = rand.Next(1, Constants.MAP_SIZE - 1);
					} while (map[index - 1, 0] * map[index, 0] * map[index + 1, 0] == 0);
				}
				for (int i = 0; i < Constants.MAP_SIZE; ++i)
				{
					if (orientationVertical)
						map[i, index] = 0;
					else
						map[index, i] = 0;
				}
			}
		}
	}
}
