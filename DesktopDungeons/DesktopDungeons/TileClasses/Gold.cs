using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DesktopDungeons.TileClasses
{
	/// <summary>
	/// Gold tile
	/// </summary>
	public class Gold : Tile
	{
		/// <summary>
		/// Value
		/// </summary>
		public int Value { get; private set; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="X">X on game</param>
		/// <param name="Y">Y on game</param>
		/// <param name="parValue">Value</param>
		public Gold(int X, int Y, int parValue)
			: base(X, Y, ContentPack.TileTexture[(int)TileType.Gold])
		{
			Value = parValue;
		}
	}
}
