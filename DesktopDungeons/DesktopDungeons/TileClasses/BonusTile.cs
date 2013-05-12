using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace DesktopDungeons.TileClasses
{
	/// <summary>
	/// Bonus tile
	/// </summary>
	public class BonusTile : Tile
	{
		/// <summary>
		/// Type of bonus
		/// </summary>
		public TileType Param { get; private set; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="X">X on game</param>
		/// <param name="Y">Y on game</param>
		/// <param name="parTexture">Texture</param>
		/// <param name="parType">Type of bonus</param>
		public BonusTile(int X, int Y, Texture2D parTexture, TileType parType)
			: base(X, Y, parTexture)
		{
			Param = parType;
		}
	}
}
