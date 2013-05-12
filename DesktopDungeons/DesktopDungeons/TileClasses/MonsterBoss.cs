using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace DesktopDungeons.TileClasses
{
	/// <summary>
	/// Boss
	/// </summary>
	public class MonsterBoss : Monster
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="X">X on game</param>
		/// <param name="Y">Y on game</param>
		/// <param name="parTexture">Texture</param>
		/// <param name="parMonsterType">Type of monster</param>
		public MonsterBoss(int X, int Y, Texture2D parTexture, MonsterType parMonsterType)
			: base(X, Y, parTexture, 10, parMonsterType)
		{
			switch (parMonsterType)
			{
				case MonsterType.Bandit:
					FirstStrike = true;
					PoisonStrike = true;
					ManaBurn = true;
					DamageBase = 52;
					HealthMax = 238;
					break;
				case MonsterType.Goblin:
					FirstStrike = true;
					ResistPhysical = 20;
					ResistMagic = 20;
					DamageBase = 90;
					HealthMax = 318;
					break;
				case MonsterType.Golem:
					ResistMagic = 75;
					DamageBase = 75;
					HealthMax = 318;
					break;
				case MonsterType.Gorgon:
					FirstStrike = true;
					DeathGaze = 100;
					DamageBase = 75;
					HealthMax = 190;
					break;
				case MonsterType.Dragon:
					MagicAttack = true;
					DamageBase = 75;
					HealthMax = 477;
					break;
				case MonsterType.Serpent:
					PoisonStrike = true;
					DamageBase = 75;
					HealthMax = 318;
					break;
				case MonsterType.Zombie:
					Undead = true;
					DamageBase = 75;
					HealthMax = 636;
					break;
				case MonsterType.Goat:
					ResistMagic = 60;
					DamageBase = 225;
					HealthMax = 159;
					break;
				case MonsterType.MeatMan:
					DamageBase = 48;
					HealthMax = 954;
					break;
				case MonsterType.Wraith:
					Undead = true;
					MagicAttack = true;
					ManaBurn = true;
					ResistPhysical = 60;
					DamageBase = 75;
					HealthMax = 238;
					break;
				case MonsterType.Goo:
					ResistPhysical = 75;
					DamageBase = 75;
					HealthMax = 318;
					break;
				case MonsterType.Warlock:
					MagicAttack = true;
					DamageBase = 112;
					HealthMax = 318;
					break;
			}

			HealthCurrent = HealthMax;
		}
	}
}
