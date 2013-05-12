using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace DesktopDungeons.TileClasses.HeroClasses
{
	/// <summary>
	/// Priest
	/// </summary>
	public class Priest : Hero
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="X">X on game</param>
		/// <param name="Y">Y on game</param>
		/// <param name="parTexture">Texture</param>
		/// <param name="parRace">Race</param>
		public Priest(int X, int Y, Texture2D parTexture, Race parRace)
			: base(X, Y, parTexture, parRace, Class.Priest)
		{
			HealthPerLevel += 2;
		}

		/// <summary>
		/// Apply potion
		/// </summary>
		/// <param name="parPotionType">Type of potion</param>
		public override void ApplyPotion(TileType parPotionType)
		{
			switch (parPotionType)
			{
				case TileType.HealthPotion:
					if (HealthPotion <= 0)
						return;
					--HealthPotion;
					HealthCurrent = HealthMax;
					break;
				default:
					base.ApplyPotion(parPotionType);
					break;
			}
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
			if (parMonster != null && parMonster.Undead)
				ret *= 2;
			return ret;
		}
	}
}
