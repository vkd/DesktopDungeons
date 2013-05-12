using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace DesktopDungeons.TileClasses.HeroClasses
{
	/// <summary>
	/// Crusader
	/// </summary>
	public class Crusader : Hero
	{
		/// <summary>
		/// Bonus damage to next strike
		/// </summary>
		private int _DamageBonusKillMonster;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="X">X on game</param>
		/// <param name="Y">Y on game</param>
		/// <param name="parTexture">Texture</param>
		/// <param name="parRace">Race</param>
		public Crusader(int X, int Y, Texture2D parTexture, Race parRace)
			: base(X, Y, parTexture, parRace, Class.Crusader)
		{

		}

		/// <summary>
		/// Add experience and if need get next level
		/// </summary>
		/// <param name="parLevelMonster">Level of killed monster</param>
		/// <param name="monsterKill">Is killed monster</param>
		/// <param name="bonusExp">Bonus point of experience</param>
		public override void AddExperience(int parLevelMonster, bool monsterKill, int bonusExp)
		{
			base.AddExperience(parLevelMonster, monsterKill, bonusExp);
		}

		/// <summary>
		/// Calc damage to current monster
		/// </summary>
		/// <param name="info">Is for information</param>
		/// <param name="parMonster">Monster</param>
		/// <returns>Points of damage</returns>
		public override int GetDamageNext(bool info, Unit parMonster)
		{
			int damage = base.GetDamageNext(info, parMonster) + DamageBase * _DamageBonusKillMonster / 100;
			if (!MagicAttack)
				damage = damage * (100 - parMonster.ResistPhysical) / 100;
			else
				damage = damage * (100 - parMonster.ResistMagic) / 100;

			if (parMonster.HealthCurrent <= damage)
				_DamageBonusKillMonster += 10;
			else
				_DamageBonusKillMonster = 0;
			return damage;
		}

		/// <summary>
		/// if not dodge then to damage this object
		/// </summary>
		/// <param name="parDamage">Points of damage</param>
		/// <param name="isMagicDamage">Is magic damage</param>
		public override void SetDamage(int parDamage, bool isMagicDamage)
		{
			base.SetDamage(parDamage, isMagicDamage);
			Poisoned = false;
			ManaBurn = false;
		}

		/// <summary>
		/// Get health and mana points when hero is research invisible blocks of map
		/// </summary>
		/// <param name="countPoints">Count research blocks</param>
		public override void ResearchMap(int countPoints)
		{
			Poisoned = false;
			ManaBurn = false;
			base.ResearchMap(countPoints);
		}
	}
}
