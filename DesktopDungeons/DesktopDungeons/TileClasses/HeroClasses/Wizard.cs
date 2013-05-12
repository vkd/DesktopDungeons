using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace DesktopDungeons.TileClasses.HeroClasses
{
	/// <summary>
	/// Wizard
	/// </summary>
	public class Wizard : Hero
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="X">X on game</param>
		/// <param name="Y">Y on game</param>
		/// <param name="parTexture">Texture</param>
		/// <param name="parRace">Race</param>
		public Wizard(int X, int Y, Texture2D parTexture, Race parRace)
			: base(X, Y, parTexture, parRace, Class.Wizard)
		{
			DamageBonus += -25;
		}

		/// <summary>
		/// Calc cost magic
		/// </summary>
		/// <param name="parMagicType">Type of magic</param>
		/// <returns>Cost mana points</returns>
		public override int CostMagic(MagicType parMagicType)
		{
			int ret = base.CostMagic(parMagicType);
			if (ret > 0)
				--ret;
			return ret;
		}
	}
}
