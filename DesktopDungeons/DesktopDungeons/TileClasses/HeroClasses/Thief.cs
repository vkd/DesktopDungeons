using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace DesktopDungeons.TileClasses.HeroClasses
{
	/// <summary>
	/// Thief
	/// </summary>
	public class Thief : Hero
	{
		/// <summary>
		/// List of monsters that hero is stiked
		/// </summary>
		List<Unit> _StrikedMonsterList;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="X">X on game</param>
		/// <param name="Y">Y on game</param>
		/// <param name="parTexture">Texture</param>
		/// <param name="parRace">Race</param>
		public Thief(int X, int Y, Texture2D parTexture, Race parRace)
			: base(X, Y, parTexture, parRace, Class.Thief)
		{
			_StrikedMonsterList = new List<Unit>();
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

			if (!_StrikedMonsterList.Contains(parMonster))
			{
				if (!info)
					_StrikedMonsterList.Add(parMonster);
				ret += DamageBase * 30 / 100;
			}
			return ret;
		}

		/// <summary>
		/// Apply potion
		/// </summary>
		/// <param name="parPotionType">Type of potion</param>
		public override void ApplyPotion(TileType parPotionType)
		{
			base.ApplyPotion(parPotionType);
			switch (parPotionType)
			{
				case TileType.HealthPotion:
					Poisoned = false;
					ManaCurrent += ManaMax * 40 / 100;
					break;
				case TileType.ManaPotion:
					ManaBurned = false;
					HealthCurrent += HealthMax * 40 / 100;
					break;
			}
		}
	}
}
