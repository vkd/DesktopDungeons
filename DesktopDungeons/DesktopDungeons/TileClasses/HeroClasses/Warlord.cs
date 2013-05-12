using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace DesktopDungeons.TileClasses.HeroClasses
{
	/// <summary>
	/// Warlord
	/// </summary>
	public class Warlord : Hero
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="X">X on game</param>
		/// <param name="Y">Y on game</param>
		/// <param name="parTexture">Texture</param>
		/// <param name="parRace">Race</param>
		public Warlord(int X, int Y, Texture2D parTexture, Race parRace)
			: base(X, Y, parTexture, parRace, Class.Warlord)
		{
			KillProtect = true;
		}

		/// <summary>
		/// Apply potion
		/// </summary>
		/// <param name="parPotionType">Type of potion</param>
		public override void ApplyPotion(TileType parPotionType)
		{
			base.ApplyPotion(parPotionType);
			if (parPotionType == TileType.ManaPotion)
				DamageNextBonus += 30;
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
			if (HealthCurrent / HealthMax < 0.5f)
				ret += DamageBase * 30 / 100;
			return ret;
		}
	}
}
