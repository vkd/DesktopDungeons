using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace DesktopDungeons.TileClasses
{
	/// <summary>
	/// Monster
	/// </summary>
	public class Monster : Unit
	{
		/// <summary>
		/// Type of monster
		/// </summary>
		public MonsterType MonsterType { get; private set; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="X">X on game</param>
		/// <param name="Y">Y on game</param>
		/// <param name="parTexture">Texture</param>
		/// <param name="parLevel">Level of monster</param>
		/// <param name="parMonsterType">Type of monster</param>
		public Monster(int X, int Y, Texture2D parTexture, int parLevel, MonsterType parMonsterType)
			: base(X, Y, parTexture)
		{
			Level = parLevel;
			HealthMax = (Level + 3) * (Level + 3) - 10;
			DamageBase = (Level * Level / 2) + (5 * Level / 2);
			MonsterType = parMonsterType;

			switch (parMonsterType)
			{
				case MonsterType.Bandit:
					DamageBase = DamageBase * 70 / 100;
					HealthMax = HealthMax * 80 / 100;
					PoisonStrike = true;
					ManaBurn = true;
					break;
				case MonsterType.Goblin:
					DamageBase = DamageBase * 120 / 100;
					FirstStrike = true;
					break;
				case MonsterType.Golem:
					ResistMagic += 50;
					break;
				case MonsterType.Gorgon:
					HealthMax = HealthMax * 70 / 100;
					FirstStrike = true;
					DeathGaze += 50;
					break;
				case MonsterType.Dragon:
					HealthMax = HealthMax * 125 / 100;
					MagicAttack = true;
					break;
				case MonsterType.Serpent:
					PoisonStrike = true;
					break;
				case MonsterType.Zombie:
					HealthMax = HealthMax * 150 / 100;
					Undead = true;
					break;
				case MonsterType.Goat:
					HealthMax = HealthMax * 75 / 100;
					ResistMagic += 25;
					break;
				case MonsterType.MeatMan:
					DamageBase = DamageBase * 65 / 100;
					HealthMax = HealthMax * 200 / 100;
					break;
				case MonsterType.Wraith:
					HealthMax = HealthMax * 75 / 100;
					Undead = true;
					MagicAttack = true;
					ManaBurn = true;
					ResistPhysical += 30;
					break;
				case MonsterType.Goo:
					ResistPhysical = 50;
					break;
				case MonsterType.Warlock:
					DamageBase = DamageBase * 135 / 100;
					MagicAttack = true;
					break;

			}

			HealthCurrent = HealthMax;
		}

		/// <summary>
		/// Get health and mana points when hero is research invisible blocks of map
		/// </summary>
		/// <param name="countPoints">Count research blocks</param>
		public void ResearchMap(int countPoints)
		{
			if (Undead || !Poisoned)
				HealthCurrent += Level * countPoints;
		}
	}
}
