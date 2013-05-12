using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace DesktopDungeons.TileClasses.HeroClasses
{
	/// <summary>
	/// Berserker
	/// </summary>
	public class Berserker : Hero
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="X">X on game</param>
		/// <param name="Y">Y on game</param>
		/// <param name="parTexture">Texture</param>
		/// <param name="parRace">Race</param>
		public Berserker(int X, int Y, Texture2D parTexture, Race parRace)
			: base(X, Y, parTexture, parRace, Class.Berserker)
		{
			ResistMagic += 50;
			DamageBonus = 30;
		}

		/// <summary>
		/// Calc damage to current monster
		/// </summary>
		/// <param name="info">Is for information</param>
		/// <param name="parMonster">Monster</param>
		/// <returns>Points of damage</returns>
		public override int GetDamageNext(bool info, Unit parMonster)
		{
			int ret = base.GetDamageNext(info, parMonster);
			if (parMonster != null && parMonster.Level > Level)
				ret += DamageBase * 30 / 100;
			return ret;
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
				ret += 2;
			return ret;
		}
	}
}
