using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace DesktopDungeons.TileClasses.HeroClasses
{
	/// <summary>
	/// Assassin
	/// </summary>
	public class Assassin : Hero
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="X">X on game</param>
		/// <param name="Y">Y on game</param>
		/// <param name="parTexture">Texture</param>
		/// <param name="parRace">Race</param>
		public Assassin(int X, int Y, Texture2D parTexture, Race parRace)
			: base(X, Y, parTexture, parRace, Class.Assassin)
		{

		}
	}
}
