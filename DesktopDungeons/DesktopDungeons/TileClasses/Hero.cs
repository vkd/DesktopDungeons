using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DesktopDungeons.TileClasses
{
	/// <summary>
	/// Hero
	/// </summary>
	public class Hero : Unit
	{
		#region Variables
		/// <summary>
		/// Race of hero
		/// </summary>
		public Race RaceHero { get; private set; }

		/// <summary>
		/// Class of hero
		/// </summary>
		public Class ClassHero { get; private set; }

		/// <summary>
		/// Damage bonus percent
		/// </summary>
		public int DamageBonus { get; set; }

		/// <summary>
		/// Damage bonus percent to next attack
		/// </summary>
		public int DamageNextBonus { get; set; }

		/// <summary>
		/// Kill protect
		/// </summary>
		public bool KillProtect { get; set; }

		/// <summary>
		/// Critical attack
		/// </summary>
		public bool Might { get; set; }

		/// <summary>
		/// Blood magic is active
		/// </summary>
		public bool Blood { get; set; }

		/// <summary>
		/// Current experience
		/// </summary>
		public int Experience { get; private set; }

		/// <summary>
		/// Need experience to get next level
		/// </summary>
		public int ExperienceNextLevel { get; private set; }

		/// <summary>
		/// Add health point when hero get next level
		/// </summary>
		public int HealthPerLevel { get; set; }

		/// <summary>
		/// Count health potion
		/// </summary>
		public int HealthPotion { get; set; }

		/// <summary>
		/// Count mana porion
		/// </summary>
		public int ManaPotion { get; set; }

		/// <summary>
		/// Magic on slots of hero
		/// </summary>
		public MagicType[] Magics;
		#endregion

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="X">X on game</param>
		/// <param name="Y">Y on game</param>
		/// <param name="parTexture">Texture</param>
		/// <param name="parRace">Race</param>
		/// <param name="parClass">Class</param>
		public Hero(int X, int Y, Texture2D parTexture, Race parRace, Class parClass)
			: base(X, Y, parTexture)
		{
			RaceHero = parRace;
			ClassHero = parClass;

			HealthMax = 10;
			HealthCurrent = HealthMax;
			ManaMax = 10;
			ManaCurrent = ManaMax;
			DamageBase = 5;
			DamageBonus = 0;

			Level = 1;
			Experience = 0;
			ExperienceNextLevel = 5;

			HealthPotion = 1;
			ManaPotion = 1;

			Magics = new MagicType[5];
			Magics[4] = MagicType.Converter;
		}

		/// <summary>
		/// Add experience and if need get next level
		/// </summary>
		/// <param name="parLevelMonster">Level of killed monster</param>
		/// <param name="monsterKill">Is killed monster</param>
		/// <param name="bonusExp">Bonus point of experience</param>
		public virtual void AddExperience(int parLevelMonster, bool monsterKill, int bonusExp)
		{
			int experience = parLevelMonster + bonusExp;
			if (parLevelMonster > Level)
				experience += (parLevelMonster - Level) * ((parLevelMonster - Level) + 1);
			Experience += experience;
			while (Experience >= ExperienceNextLevel)
			{
				++Level;
				Experience -= ExperienceNextLevel;
				ExperienceNextLevel = (Level) * 5;
				DamageBase += 5;
				HealthMax += 10;
				HealthMax += HealthPerLevel;
				HealthCurrent = HealthMax;
				ManaCurrent = ManaMax;
				Poisoned = false;
				ManaBurned = false;
			}
		}

		/// <summary>
		/// Check Kill protect
		/// </summary>
		/// <returns>Hero is death</returns>
		public bool Death()
		{
			if (KillProtect)
			{
				HealthCurrent = 1;
				KillProtect = false;
				return false;
			}
			return true;
		}

		/// <summary>
		/// Calc damage to current monster
		/// </summary>
		/// <param name="info">Is for information</param>
		/// <param name="parMonster">Monster</param>
		/// <returns>Points of damage</returns>
		public override int GetDamageNext(bool info, Unit parMonster)
		{
			int ret = DamageBase;
			ret += DamageBase * (DamageBonus + DamageNextBonus) / 100;
			if (Might)
				ret += DamageBase * 30 / 100;

			if (!info)
			{
				Might = false;
				DamageNextBonus = 0;
			}

			if (parMonster != null)
				ret = ret * (100 - (MagicAttack 
					? parMonster.ResistMagic : parMonster.ResistPhysical)) / 100;

			return ret;
		}

		/// <summary>
		/// Apply potion
		/// </summary>
		/// <param name="parPotionType">Type of potion</param>
		public virtual void ApplyPotion(TileType parPotionType)
		{
			switch (parPotionType)
			{
				case TileType.HealthPotion:
					if (HealthPotion <= 0)
						return;
					--HealthPotion;
					Poisoned = false;
					HealthCurrent += HealthMax * 40 / 100;
					break;
				case TileType.ManaPotion:
					if (ManaPotion <= 0)
						return;
					--ManaPotion;
					ManaBurned = false;
					ManaCurrent += ManaMax * 40 / 100;
					break;
			}
		}

		/// <summary>
		/// Apply bonus tile
		/// </summary>
		/// <param name="parTileType">Type of bonus</param>
		public void SetBonus(TileType parTileType)
		{
			switch (parTileType)
			{
				case TileType.Attackboost:
					DamageBonus += 10;
					break;
				case TileType.HPBoost:
					++HealthPerLevel;
					bool fullHealth = HealthMax == HealthCurrent;
					HealthMax += Level;
					if (fullHealth)
						HealthCurrent = HealthMax;
					break;
				case TileType.MPBoost:
					bool fullMana = ManaMax == ManaCurrent;
					++ManaMax;
					if (fullMana)
						ManaCurrent = ManaMax;
					break;
				case TileType.HealthPotion:
					++HealthPotion;
					break;
				case TileType.ManaPotion:
					++ManaPotion;
					break;
			}
		}

		/// <summary>
		/// Calc cost magic
		/// </summary>
		/// <param name="parMagicType">Type of magic</param>
		/// <returns>Cost mana points</returns>
		public virtual int CostMagic(MagicType parMagicType)
		{
			switch (parMagicType)
			{
				case MagicType.TeleMonster:
					return 10;
				case MagicType.Heal:
					return 3;
				case MagicType.Endwall:
					return 8;
				case MagicType.Might:
					return 2;
				case MagicType.Reveal:
					return 3;
				case MagicType.Fireball:
					return 6;
				case MagicType.Petrify:
					return 5;
				case MagicType.Poison:
					return 5;
				case MagicType.FirstStrike:
					return 3;
				case MagicType.KillProtect:
					return 10;
				case MagicType.Summon:
					return 6;
				case MagicType.Blood:
					return 0;
				case MagicType.TeleSelf:
					return 6;
			}
			return 0;
		}

		/// <summary>
		/// Check that hero attack is first
		/// </summary>
		/// <param name="parMonster">Monster</param>
		/// <returns>First attack</returns>
		public virtual bool CheckFirstStrike(Monster parMonster)
		{
			bool ret = false;

			if (parMonster.FirstStrike == false && (Level > parMonster.Level || FirstStrike == true))
				ret = true;

			FirstStrike = false;

			return ret;
		}

		/// <summary>
		/// Convert magic to race bonus
		/// </summary>
		/// <param name="parMagicType">Type of magic</param>
		public void ConvertMagic(MagicType parMagicType)
		{
			for (int i = 0; i < 5; ++i)
			{
				if (i == 5)
					return;
				if (Magics[i] == parMagicType)
				{
					Magics[i] = MagicType.Empty;
					break;
				}
			}
			
			switch (RaceHero)
			{
				case Race.Human:
					DamageBonus += 10;
					break;
				case Race.Elf:
					SetBonus(TileType.MPBoost);
					SetBonus(TileType.MPBoost);
					break;
				case Race.Dwarf:
					SetBonus(TileType.HPBoost);
					break;
				case Race.Halfling:
					++HealthPotion;
					break;
				case Race.Gnome:
					++ManaPotion;
					break;
				case Race.Goblin:
					AddExperience(0, false, 5);
					break;
				case Race.Orc:
					DamageBase += 2;
					break;
			}
		}

		/// <summary>
		/// Get health and mana points when hero is research invisible blocks of map
		/// </summary>
		/// <param name="countPoints">Count research blocks</param>
		public virtual void ResearchMap(int countPoints)
		{
			if (!Blood)
			{
				if (!Poisoned)
					HealthCurrent += Level * countPoints;
				if (!ManaBurned)
					ManaCurrent += countPoints;
			}
			else
			{
				if (!ManaBurned)
					ManaCurrent += countPoints * 2;
			}
		}

		/// <summary>
		/// Apply magic
		/// </summary>
		/// <param name="parMagicType">Type of magic</param>
		public virtual void ApplyMagic(MagicType parMagicType)
		{
			ManaCurrent -= CostMagic(parMagicType);
		}

		/// <summary>
		/// Response attack to monster
		/// </summary>
		/// <param name="magic">Is magic attack</param>
		/// <returns></returns>
		public virtual int AttackResponse(bool magic)
		{
			return 0;
		}
	}
}
