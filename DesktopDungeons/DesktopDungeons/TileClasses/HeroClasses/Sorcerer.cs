using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace DesktopDungeons.TileClasses.HeroClasses
{
	/// <summary>
	/// Sorcerer
	/// </summary>
	public class Sorcerer : Hero
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="X">X on game</param>
		/// <param name="Y">Y on game</param>
		/// <param name="parTexture">Texture</param>
		/// <param name="parRace">Race</param>
		public Sorcerer(int X, int Y, Texture2D parTexture, Race parRace)
			: base(X, Y, parTexture, parRace, Class.Sorcerer)
		{
			ManaMax += 5;
			ManaCurrent = ManaMax;
		}

		/// <summary>
		/// Apply magic
		/// </summary>
		/// <param name="parMagicType">Type of magic</param>
		public override void ApplyMagic(MagicType parMagicType)
		{
			base.ApplyMagic(parMagicType);
			HealthCurrent += CostMagic(parMagicType) * 2;
		}

		/// <summary>
		/// Response attack to monster
		/// </summary>
		/// <param name="magic">Is magic attack</param>
		/// <returns></returns>
		public override int AttackResponse(bool magic)
		{
			if (magic)
				return Level;
			return base.AttackResponse(magic);
		}
	}
}
