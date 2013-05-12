using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesktopDungeons
{
	/// <summary>
	/// State of game
	/// </summary>
	public enum GameState
	{
		Menu, 
		Game,
		Score,
		About
	}

	/// <summary>
	/// Races of hero
	/// </summary>
	public enum Race : int
	{
		Human = 0, 
		Elf = 1,
		Dwarf = 2, 
		Halfling = 3, 
		Gnome = 4, 
		Goblin = 5,
		Orc = 6
	}

	/// <summary>
	/// Classes of hero
	/// </summary>
	public enum Class : int
	{
		Fighter = 0,
		Berserker = 1,
		Warlord = 2,
		Thief = 3,
		Rogue = 4,
		Assassin = 5,
		Priest = 6,
		Monk = 7,
		Paladin = 8,
		Wizard = 9,
		Sorcerer = 10,
		Bloodmage = 11,
		Transmuter = 12,
		Crusader = 13,
		Tinker = 14
	}

	/// <summary>
	/// Types of monsters
	/// </summary>
	public enum MonsterType : int
	{
		Bandit = 0,
		Dragon = 1,
		Goat = 2,
		Goblin = 3,
		Golem = 4,
		Goo = 5,
		Gorgon = 6,
		MeatMan = 7,
		Serpent = 8,
		Warlock = 9,
		Wraith = 10,
		Zombie = 11
	}

	/// <summary>
	/// Types of tiles
	/// </summary>
	public enum TileType : int
	{
		Attackboost = 0,
		Blood = 1,
		EnemyGeneric = 2,
		Gold = 3,
		HealthPotion = 4,
		HPBoost = 5,
		ManaPotion = 6,
		MPBoost = 7,
		PowerupGeneric = 8,
		Shop = 9
	}

	/// <summary>
	/// Types of magics
	/// </summary>
	public enum MagicType : int
	{
		Empty = 0,
		Blood = 1,
		Converter = 2,
		Endwall = 3,
		Fireball = 4,
		FirstStrike = 5,
		Heal = 6,
		KillProtect = 7,
		Might = 8,
		Petrify = 9,
		Poison = 10,
		Reveal = 11,
		Summon = 12,
		TeleMonster = 13,
		TeleSelf = 14
	}

	/// <summary>
	/// Types of artefacts
	/// </summary>
	public enum ArtefactType : int
	{
		PendantOfHealth = 1,
		PendantOfMana = 2,
		FineSword = 3,
		HealthPotion = 4,
		ManaPotion = 5,
		BloodySigil = 6,
		ViperWard = 7,
		SoulOrb = 8,
		TrollHeart = 9,
		TowerShield = 10,
		MageHelm = 11,
		ScoutingOrb = 12,
		BlueBead = 13,
		StoneOfSeekers = 14,
		Spoon = 15,
		StoneSigil = 16,
		BadgeOfCourage = 17,
		TalismanOfRebirth = 18,
		SignOfTheSpirits = 19,
		Bonebreaker = 20,
		StoneHeart = 21,
		FireHeart = 22,
		Platemail = 23,
		MagePlate = 24,
		VenomBlade = 25,
		FlamingSword = 26,
		DancingSword = 27,
		ZombieDog = 28,
		DwarvenGauntlets = 29,
		ElvenBoots = 30,
		KegOHealth = 31,
		KegOMagic = 32,
		TPRune = 33,
		HealthRune = 34,
		BloodRune = 35,
		CrystalBall = 36,
		AgnosticsCollar = 37,
		VampiricSword = 38,
		SpikedFlail = 39,
		AlchemistsScroll = 40,
		RingOfTheBattlemage = 41,
		WickedGuitar = 42,
		BerserkersBlade = 43,
		MagiciansMoonstrike = 44,
		TerrorSlice = 45,
		AmuletOfYendor = 46,
		OrbOfZot = 47
	}

	/// <summary>
	/// Items on map
	/// </summary>
	public enum MapItem : int
	{
		None = 0,
		Wall = 1,
		Hero = 2,
		Monster = 3,
		Other = 4
	}
}
