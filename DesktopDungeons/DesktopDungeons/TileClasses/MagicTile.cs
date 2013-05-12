using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace DesktopDungeons.TileClasses
{
	/// <summary>
	/// Tile of magic on map
	/// </summary>
	public class MagicTile : Tile
	{
		/// <summary>
		/// Type of magic
		/// </summary>
		public MagicType Magic { get; private set; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="X">X on map</param>
		/// <param name="Y">Y on map</param>
		/// <param name="parTexture">Texture</param>
		/// <param name="parMagicType">Type of magic</param>
		public MagicTile(int X, int Y, Texture2D parTexture, MagicType parMagicType)
			: base(X, Y, parTexture)
		{
			Magic = parMagicType;
		}
	}
}
