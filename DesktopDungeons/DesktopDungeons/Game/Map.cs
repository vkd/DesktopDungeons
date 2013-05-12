using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoadLocalization;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using DesktopDungeons.TileClasses;
using DesktopDungeons.TileClasses.HeroClasses;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DesktopDungeons
{
	/// <summary>
	/// Map of game
	/// </summary>
	public class Map
	{
		#region Variables
		/// <summary>
		/// State of game
		/// </summary>
		public GameState GameState { get; set; }

		/// <summary>
		/// Data for save on disk
		/// </summary>
		public SaveData saveData;

		/// <summary>
		/// Game map
		/// </summary>
		public short[,] map;

		/// <summary>
		/// Visible block on map
		/// </summary>
		public short[,] visible;

		/// <summary>
		/// Hero
		/// </summary>
		public Hero hero;

		/// <summary>
		/// All monsters on map
		/// </summary>
		public List<Monster> monsterList;

		/// <summary>
		/// All blood on map
		/// </summary>
		public List<Tile> bloodList;

		/// <summary>
		/// All magic on map
		/// </summary>
		public List<MagicTile> magicTileList;

		/// <summary>
		/// All bonus on map
		/// </summary>
		public List<BonusTile> bonusList;
		#endregion

		/// <summary>
		/// Constructor
		/// </summary>
		public Map()
		{
			saveData = SaveData.Instance();

			GameState = GameState.Menu;

			map = new short[Constants.MAP_SIZE, Constants.MAP_SIZE];
			visible = new short[Constants.MAP_SIZE, Constants.MAP_SIZE];

			monsterList = new List<Monster>();
			bloodList = new List<Tile>();
			magicTileList = new List<MagicTile>();
			bonusList = new List<BonusTile>();
		}

		/// <summary>
		/// Create new map
		/// </summary>
		/// <param name="parHeroesRace">Race of hero</param>
		/// <param name="parHeroesClass">Class of hero</param>
		public void CreateMap(Race parHeroesRace, Class parHeroesClass)
		{
			//clear all lists
			monsterList.Clear();
			bloodList.Clear();
			magicTileList.Clear();
			bonusList.Clear();

			Generator generator = new Generator();
			Point pointHero = generator.Generate(out map);

			CreateHero(pointHero.X * Constants.TEXTURE_SIZE, pointHero.Y * Constants.TEXTURE_SIZE,
				parHeroesRace, parHeroesClass, out hero);

			map[pointHero.X, pointHero.Y] = (int)MapItem.Hero;
			visible = new short[Constants.MAP_SIZE, Constants.MAP_SIZE];
			SearchAround(pointHero.X, pointHero.Y);

			#region Create monsters
			List<MonsterType> listMonsterTypes = new List<MonsterType>();
			if (saveData._ListWinClass.Any(p => p == Class.Rogue))
				listMonsterTypes.Add(MonsterType.Bandit);
			listMonsterTypes.Add(MonsterType.Goblin);
			if (saveData._ListWinClass.Any(p => p == Class.Sorcerer))
				listMonsterTypes.Add(MonsterType.Golem);
			if (saveData._ListWinClass.Any(p => p == Class.Thief))
				listMonsterTypes.Add(MonsterType.Gorgon);
			if (saveData._ListWinClass.Any(p => p == Class.Monk))
				listMonsterTypes.Add(MonsterType.Dragon);
			if (saveData._ListWinClass.Any(p => p == Class.Priest))
				listMonsterTypes.Add(MonsterType.Serpent);
			listMonsterTypes.Add(MonsterType.Zombie);
			if (saveData._ListWinClass.Any(p => p == Class.Wizard))
				listMonsterTypes.Add(MonsterType.Goat);
			listMonsterTypes.Add(MonsterType.MeatMan);
			if (saveData._ListWinClass.Any(p => p == Class.Fighter))
				listMonsterTypes.Add(MonsterType.Wraith);
			if (saveData._ListWinClass.Any(p => p == Class.Berserker))
				listMonsterTypes.Add(MonsterType.Goo);
			listMonsterTypes.Add(MonsterType.Warlock);

			//monsters
			CreateMonsterOnMap(1, 10, listMonsterTypes);
			CreateMonsterOnMap(2, 5, listMonsterTypes);
			CreateMonsterOnMap(3, 4, listMonsterTypes);
			CreateMonsterOnMap(4, 4, listMonsterTypes);
			CreateMonsterOnMap(5, 4, listMonsterTypes);
			CreateMonsterOnMap(6, 3, listMonsterTypes);
			CreateMonsterOnMap(7, 3, listMonsterTypes);
			CreateMonsterOnMap(8, 3, listMonsterTypes);
			CreateMonsterOnMap(9, 2, listMonsterTypes);

			//boss
			Random rand = new Random();
			int X, Y;
			MonsterType typeMonster;
			do
			{
				X = rand.Next(Constants.MAP_SIZE);
				Y = rand.Next(Constants.MAP_SIZE);
			} while (map[X, Y] != (int)MapItem.None);
			typeMonster = listMonsterTypes[rand.Next(listMonsterTypes.Count)];
			monsterList.Add(new MonsterBoss(X * Constants.TEXTURE_SIZE, Y * Constants.TEXTURE_SIZE,
				ContentPack.MonsterTexture[(int)typeMonster], typeMonster));
			map[X, Y] = (int)MapItem.Monster;
			//end boss
			#endregion

			//magic
			CreateMagicOnMap();

			int countItem = 3;
			int countAttack = countItem;
			int countHealth = countItem;
			int countMana = countItem;
			int countPotionHP = countItem;
			int countPotionMP = countItem;

			//int countGold = 10;
			
			int bonusCountAdd = 0;
			if (hero.ClassHero == Class.Thief)
			{
				bonusCountAdd = 1;
				//countGold += 3;
			}

			//Attack
			CreateBonusOnMap(countAttack + bonusCountAdd, TileType.Attackboost);
			//HPBoost
			CreateBonusOnMap(countHealth + bonusCountAdd, TileType.HPBoost);
			//MPBoost
			CreateBonusOnMap(countMana + bonusCountAdd, TileType.MPBoost);
			//PotionHP
			CreateBonusOnMap(countPotionHP + bonusCountAdd, TileType.HealthPotion);
			//PotionMP
			CreateBonusOnMap(countPotionMP + bonusCountAdd, TileType.ManaPotion);
			////Gold
			//CreateBonusOnMap(countGold, TileType.Gold);


			for (int i = 0; i < Constants.MAP_SIZE; ++i)
				for (int j = 0; j < Constants.MAP_SIZE; ++j)
					if (map[i, j] == (int)MapItem.Other)
						map[i, j] = (int)MapItem.None;
		}

		/// <summary>
		/// Click on map for move of hero
		/// </summary>
		/// <param name="X">Mouse click X on map</param>
		/// <param name="Y">Mouse click Y on map</param>
		public void ClickHeroMove(int X, int Y)
		{
			int[,] heroMove;
			Dijkstra(out heroMove);
			if (heroMove[X, Y] >= 0)
			{

				HeroReplace(X, Y);
			}
		}

		/// <summary>
		/// Move hero to free block around monster
		/// </summary>
		/// <param name="X">Monster on map X</param>
		/// <param name="Y">Monster on map Y</param>
		/// <returns></returns>
		public bool ClickHeroMoveToMonster(int X, int Y)
		{
			int[,] heroMove;
			Dijkstra(out heroMove);

			int minW = int.MaxValue;
			int minX = 0;
			int minY = 0;

			// 0 * *
			// * * *
			// * * *
			if (X != 0 && Y != 0)
			{
				if (heroMove[X - 1, Y - 1] != -1 && heroMove[X - 1, Y - 1] <= minW)
				{
					minW = heroMove[X - 1, Y - 1];
					minX = X - 1;
					minY = Y - 1;
				}
			}
			// * * *
			// * * *
			// 0 * *
			if (X != 0 && Y != Constants.MAP_SIZE - 1)
			{
				if (heroMove[X - 1, Y + 1] != -1 && heroMove[X - 1, Y + 1] <= minW)
				{
					minW = heroMove[X - 1, Y + 1];
					minX = X - 1;
					minY = Y + 1;
				}
			}
			// * * 0
			// * * *
			// * * *
			if (X != Constants.MAP_SIZE - 1 && Y != 0)
			{
				if (heroMove[X + 1, Y - 1] != -1 && heroMove[X + 1, Y - 1] <= minW)
				{
					minW = heroMove[X + 1, Y - 1];
					minX = X + 1;
					minY = Y - 1;
				}
			}
			// * * *
			// * * *
			// * * 0
			if (X != Constants.MAP_SIZE - 1 && Y != Constants.MAP_SIZE - 1)
			{
				if (heroMove[X + 1, Y + 1] != -1 && heroMove[X + 1, Y + 1] <= minW)
				{
					minW = heroMove[X + 1, Y + 1];
					minX = X + 1;
					minY = Y + 1;
				}
			}
			// * * *
			// 0 * *
			// * * *
			if (X != 0)
			{
				if (heroMove[X - 1, Y] != -1 && heroMove[X - 1, Y] <= minW)
				{
					minW = heroMove[X - 1, Y];
					minX = X - 1;
					minY = Y;
				}
			}
			// * 0 *
			// * * *
			// * * *
			if (Y != 0)
			{
				if (heroMove[X, Y - 1] != -1 && heroMove[X, Y - 1] <= minW)
				{
					minW = heroMove[X, Y - 1];
					minX = X;
					minY = Y - 1;
				}
			}
			// * * *
			// * * 0
			// * * *
			if (X != Constants.MAP_SIZE - 1)
			{
				if (heroMove[X + 1, Y] != -1 && heroMove[X + 1, Y] <= minW)
				{
					minW = heroMove[X + 1, Y];
					minX = X + 1;
					minY = Y;
				}
			}
			// * * *
			// * * *
			// * 0 *
			if (Y != Constants.MAP_SIZE - 1)
			{
				if (heroMove[X, Y + 1] != -1 && heroMove[X, Y + 1] <= minW)
				{
					minW = heroMove[X, Y + 1];
					minX = X;
					minY = Y + 1;
				}
			}

			if (minW == int.MaxValue)
				return false;

			HeroReplace(minX, minY);
			return true;
		}

		/// <summary>
		/// Draw map
		/// </summary>
		/// <param name="spriteBatch">SpriteBatch</param>
		public void Draw(SpriteBatch spriteBatch)
		{
			Rectangle rect = new Rectangle(0, 0, Constants.TEXTURE_SIZE, Constants.TEXTURE_SIZE);
			for (int x = 0; x < Constants.MAP_SIZE; ++x)
			{
				for (int y = 0; y < Constants.MAP_SIZE; ++y)
				{
					rect.X = x * Constants.TEXTURE_SIZE;
					rect.Y = y * Constants.TEXTURE_SIZE;
					if (map[x, y] == 1)
						spriteBatch.Draw(ContentPack.WallTexture, rect, Color.White);
					else
						spriteBatch.Draw(ContentPack.DirtTexture, rect, Color.White);
				}
			}

			foreach (Tile blood in bloodList)
			{
				blood.Draw(spriteBatch);
			}

			foreach (BonusTile tile in bonusList)
			{
				tile.Draw(spriteBatch);
			}

			foreach (MagicTile tile in magicTileList)
			{
				tile.Draw(spriteBatch);
			}

			foreach (Monster monster in monsterList)
			{
				monster.Draw(spriteBatch);
			}

			//draw black block when invisible
			for (int i = 0; i < Constants.MAP_SIZE; ++i)
				for (int j = 0; j < Constants.MAP_SIZE; ++j)
					if (visible[i, j] == 0)
						spriteBatch.Draw(ContentPack.PointTexture,
							new Rectangle(i * Constants.TEXTURE_SIZE, j * Constants.TEXTURE_SIZE, 
								Constants.TEXTURE_SIZE, Constants.TEXTURE_SIZE),
							Color.Black);

			hero.Draw(spriteBatch);

			if (hero.ClassHero == Class.Fighter)
			{
				foreach (Monster monster in monsterList)
				{
					if (monster.Level <= hero.Level 
						&& visible[monster.Rectangle.X / Constants.TEXTURE_SIZE, 
						monster.Rectangle.Y / Constants.TEXTURE_SIZE] == 0)
					{
						spriteBatch.Draw(ContentPack.EnemyGeneric, monster.Rectangle, Color.Gray);
					}
				}
			}
			else if (hero.ClassHero == Class.Wizard)
			{
				foreach (MagicTile magic in magicTileList)
				{
					if (visible[magic.Rectangle.X / Constants.TEXTURE_SIZE,
						magic.Rectangle.Y / Constants.TEXTURE_SIZE] == 0)
						spriteBatch.Draw(ContentPack.MagicGenericTexture, magic.Rectangle, Color.Gray);
				}
			}
		}

		/// <summary>
		/// Death monster
		/// </summary>
		/// <param name="parMonster">Monster</param>
		public void DeathMonster(Monster parMonster)
		{
			map[parMonster.Rectangle.X / Constants.TEXTURE_SIZE,
				parMonster.Rectangle.Y / Constants.TEXTURE_SIZE] = (int)MapItem.None;
			bloodList.Add(new Tile(parMonster.Rectangle.X, 
				parMonster.Rectangle.Y, ContentPack.TileTexture[(int)TileType.Blood]));
			monsterList.Remove(parMonster);
		}

		/// <summary>
		/// Load data
		/// </summary>
		public void LoadSaveData()
		{
			if (File.Exists(Constants.SAVEDATA_FILENAME))
			{
				Stream FileStream = File.OpenRead(Constants.SAVEDATA_FILENAME);
				BinaryFormatter deserializer = new BinaryFormatter();
				saveData = (SaveData)deserializer.Deserialize(FileStream);
				FileStream.Close();
			}
		}

		/// <summary>
		/// Save data
		/// </summary>
		public void UpdateSaveData()
		{
			if (!monsterList.Any(p => p.Level == 10))
			{
				if (!saveData._ListWinRace.Any(p => p == hero.RaceHero))
					saveData._ListWinRace.Add(hero.RaceHero);
				if (!saveData._ListWinClass.Any(p => p == hero.ClassHero))
					saveData._ListWinClass.Add(hero.ClassHero);
			}
			Stream FileStream = File.Create(Constants.SAVEDATA_FILENAME);
			BinaryFormatter serializer = new BinaryFormatter();
			serializer.Serialize(FileStream, saveData);
			FileStream.Close();
		}

		/// <summary>
		/// Search random invisible block on map and health
		/// </summary>
		/// <param name="count">Count searched blocks</param>
		public void SearchRandom(int count)
		{
			Random rand = new Random();
			int X, Y;

			int countInvisibleTile = 0;
			for (int i = 0; i < Constants.MAP_SIZE; ++i)
				for (int j = 0; j < Constants.MAP_SIZE; ++j)
					if (visible[i, j] == 0)
						++countInvisibleTile;

			int countIteration = count;
			if (countInvisibleTile < count)
				countIteration = countInvisibleTile;

			for (int c = 0; c < countIteration; ++c)
			{
				do
				{
					X = rand.Next(Constants.MAP_SIZE);
					Y = rand.Next(Constants.MAP_SIZE);
				} while (visible[X, Y] != 0);

				hero.ResearchMap(1);
				visible[X, Y] = 1;
				foreach (Monster monster in monsterList)
				{
					monster.ResearchMap(1);
				}
			}
		}

		/// <summary>
		/// Search blocks around (X, Y) and health
		/// </summary>
		/// <param name="X">X on map</param>
		/// <param name="Y">Y on map</param>
		public void SearchAround(int X, int Y)
		{
			int count = 0;

			if (X > 0)
			{
				if (Y > 0)
					if (visible[X - 1, Y - 1] == 0)
					{
						visible[X - 1, Y - 1] = 1;
						++count;
					}
				if (visible[X - 1, Y] == 0)
				{
					visible[X - 1, Y] = 1;
					++count;
				}
				if (Y < Constants.MAP_SIZE - 1)
					if (visible[X - 1, Y + 1] == 0)
					{
						visible[X - 1, Y + 1] = 1;
						++count;
					}
			}

			if (Y > 0)
				if (visible[X, Y - 1] == 0)
				{
					visible[X, Y - 1] = 1;
					++count;
				}
			if (visible[X, Y] == 0)
			{
				visible[X, Y] = 1;
				++count;
			}
			if (Y < Constants.MAP_SIZE - 1)
				if (visible[X, Y + 1] == 0)
				{
					visible[X, Y + 1] = 1;
					++count;
				}

			if (X < Constants.MAP_SIZE - 1)
			{
				if (Y > 0)
					if (visible[X + 1, Y - 1] == 0)
					{
						visible[X + 1, Y - 1] = 1;
						++count;
					}
				if (visible[X + 1, Y] == 0)
				{
					visible[X + 1, Y] = 1;
					++count;
				}
				if (Y < Constants.MAP_SIZE - 1)
					if (visible[X + 1, Y + 1] == 0)
					{
						visible[X + 1, Y + 1] = 1;
						++count;
					}
			}

			hero.ResearchMap(count);
			foreach (Monster monster in monsterList)
			{
				monster.ResearchMap(count);
			}
		}

		/// <summary>
		/// Create hero
		/// </summary>
		/// <param name="X">X on map</param>
		/// <param name="Y">Y on map</param>
		/// <param name="parRace">Race</param>
		/// <param name="parClass">Class</param>
		/// <param name="hero">Instance of hero class</param>
		private void CreateHero(int X, int Y, Race parRace, Class parClass, out Hero hero)
		{
			hero = new Hero(X, Y, ContentPack.HeroTexture[(int)parClass], parRace, parClass);
			switch (parClass)
			{
				case Class.Fighter:
					hero = new Fighter(X, Y, ContentPack.HeroTexture[(int)parClass], parRace);
					break;
				case Class.Thief:
					hero = new Thief(X, Y, ContentPack.HeroTexture[(int)parClass], parRace);
					break;
				case Class.Priest:
					hero = new Priest(X, Y, ContentPack.HeroTexture[(int)parClass], parRace);
					break;
				case Class.Wizard:
					hero = new Wizard(X, Y, ContentPack.HeroTexture[(int)parClass], parRace);
					break;
				case Class.Berserker:
					hero = new Berserker(X, Y, ContentPack.HeroTexture[(int)parClass], parRace);
					break;
				case Class.Rogue:
					hero = new Rogue(X, Y, ContentPack.HeroTexture[(int)parClass], parRace);
					break;
				case Class.Monk:
					hero = new Monk(X, Y, ContentPack.HeroTexture[(int)parClass], parRace);
					break;
				case Class.Sorcerer:
					hero = new Sorcerer(X, Y, ContentPack.HeroTexture[(int)parClass], parRace);
					break;
				case Class.Warlord:
					hero = new Warlord(X, Y, ContentPack.HeroTexture[(int)parClass], parRace);
					break;
				case Class.Assassin:
					hero = new Assassin(X, Y, ContentPack.HeroTexture[(int)parClass], parRace);
					break;
				case Class.Paladin:
					hero = new Paladin(X, Y, ContentPack.HeroTexture[(int)parClass], parRace);
					break;
				case Class.Bloodmage:
					hero = new Bloodmage(X, Y, ContentPack.HeroTexture[(int)parClass], parRace);
					break;
				case Class.Transmuter:
					hero = new Transmuter(X, Y, ContentPack.HeroTexture[(int)parClass], parRace);
					break;
				case Class.Crusader:
					hero = new Crusader(X, Y, ContentPack.HeroTexture[(int)parClass], parRace);
					break;
				case Class.Tinker:
					hero = new Tinker(X, Y, ContentPack.HeroTexture[(int)parClass], parRace);
					break;
			}
		}

		/// <summary>
		/// Create bonus tile on map
		/// </summary>
		/// <param name="count">Count bonus tiles</param>
		/// <param name="parType">Type of bonus</param>
		private void CreateBonusOnMap(int count, TileType parType)
		{
			Random rand = new Random();
			int X, Y;

			for (int i = 0; i < count; ++i)
			{
				do
				{
					X = rand.Next(Constants.MAP_SIZE);
					Y = rand.Next(Constants.MAP_SIZE);
				} while (map[X, Y] != (int)MapItem.None);

				bonusList.Add(new BonusTile(X * Constants.TEXTURE_SIZE, Y * Constants.TEXTURE_SIZE,
					ContentPack.TileTexture[(int)parType], parType)); 
				map[X, Y] = (int)MapItem.Other;
			}
		}

		/// <summary>
		/// Create magic tile on map
		/// </summary>
		private void CreateMagicOnMap()
		{
			List<MagicType> listMagicType = new List<MagicType>();

			if (hero.ClassHero != Class.Transmuter)
				listMagicType.Add(MagicType.Endwall);
			listMagicType.Add(MagicType.FirstStrike);
			listMagicType.Add(MagicType.Might);
			listMagicType.Add(MagicType.Petrify);
			listMagicType.Add(MagicType.Reveal);
			listMagicType.Add(MagicType.Summon);
			listMagicType.Add(MagicType.TeleMonster);
			listMagicType.Add(MagicType.TeleSelf);

			Random rand = new Random();
			int X, Y;
			int count = 5;


			//fireball
			do
			{
				X = rand.Next(Constants.MAP_SIZE);
				Y = rand.Next(Constants.MAP_SIZE);
			} while (map[X, Y] != (int)MapItem.None);
			magicTileList.Add(new MagicTile(X * Constants.TEXTURE_SIZE, Y * Constants.TEXTURE_SIZE, 
				ContentPack.MagicTexture[(int)MagicType.Fireball], MagicType.Fireball));
			map[X, Y] = (int)MapItem.Other;
			//end fireball

			//heal
			if (saveData._ListWinClass.Any(p => p == Class.Paladin) && hero.ClassHero != Class.Paladin)
			{
				do
				{
					X = rand.Next(Constants.MAP_SIZE);
					Y = rand.Next(Constants.MAP_SIZE);
				} while (map[X, Y] != (int)MapItem.None);
				magicTileList.Add(new MagicTile(X * Constants.TEXTURE_SIZE, Y * Constants.TEXTURE_SIZE,
					ContentPack.MagicTexture[(int)MagicType.Heal], MagicType.Heal));
				map[X, Y] = (int)MapItem.Other;

				--count;
			}
			//end heal

			//other magic
			MagicType typeMagic;

			if (hero.ClassHero == Class.Thief || hero.ClassHero == Class.Wizard)
				++count;

			if (hero.ClassHero == Class.Warlord)
				magicTileList.Add(new MagicTile(hero.Rectangle.X, hero.Rectangle.Y, 
					ContentPack.MagicTexture[(int)MagicType.KillProtect], MagicType.KillProtect));
			if (hero.ClassHero == Class.Assassin)
				magicTileList.Add(new MagicTile(hero.Rectangle.X, hero.Rectangle.Y,
					ContentPack.MagicTexture[(int)MagicType.Poison], MagicType.Poison));
			if (hero.ClassHero == Class.Paladin)
				magicTileList.Add(new MagicTile(hero.Rectangle.X, hero.Rectangle.Y,
					ContentPack.MagicTexture[(int)MagicType.Heal], MagicType.Heal));
			if (hero.ClassHero == Class.Bloodmage)
				magicTileList.Add(new MagicTile(hero.Rectangle.X, hero.Rectangle.Y,
					ContentPack.MagicTexture[(int)MagicType.Blood], MagicType.Blood));
			if (hero.ClassHero == Class.Transmuter)
				magicTileList.Add(new MagicTile(hero.Rectangle.X, hero.Rectangle.Y,
					ContentPack.MagicTexture[(int)MagicType.Endwall], MagicType.Endwall));

			for (int i = 0; i < count; ++i)
			{
				do
				{
					X = rand.Next(Constants.MAP_SIZE);
					Y = rand.Next(Constants.MAP_SIZE);
				} while (map[X, Y] != (int)MapItem.None);
				typeMagic = listMagicType[rand.Next(listMagicType.Count)];
				magicTileList.Add(new MagicTile(X * Constants.TEXTURE_SIZE, Y * Constants.TEXTURE_SIZE, 
					ContentPack.MagicTexture[(int)typeMagic], typeMagic));
				map[X, Y] = (int)MapItem.Other;
				listMagicType.Remove(typeMagic);
			}
		}

		/// <summary>
		/// Create monsters on map
		/// </summary>
		/// <param name="parLevel">Level of new monsters</param>
		/// <param name="parCount">Count of new monsters</param>
		/// <param name="parListMonsterTypes">Avaliable types of monster</param>
		private void CreateMonsterOnMap(int parLevel, int parCount, List<MonsterType> parListMonsterTypes)
		{
			Random rand = new Random();
			int X, Y;
			MonsterType typeMonster;
			for (int i = 0; i < parCount; ++i)
			{
				do
				{
					X = rand.Next(Constants.MAP_SIZE);
					Y = rand.Next(Constants.MAP_SIZE);
				} while (map[X, Y] != (int)MapItem.None);
				typeMonster = parListMonsterTypes[rand.Next(parListMonsterTypes.Count)];
				monsterList.Add(new Monster(X * Constants.TEXTURE_SIZE, Y * Constants.TEXTURE_SIZE, 
					ContentPack.MonsterTexture[(int)typeMonster], parLevel, typeMonster));
				map[X, Y] = (int)MapItem.Monster;
			}
		}

		/// <summary>
		/// Replace hero
		/// </summary>
		/// <param name="X">X on map</param>
		/// <param name="Y">Y on map</param>
		public void HeroReplace(int X, int Y)
		{
			map[hero.Rectangle.X / Constants.TEXTURE_SIZE, hero.Rectangle.Y / Constants.TEXTURE_SIZE] 
				= (int)MapItem.None;
			SearchAround(X, Y);

			BonusTile currentBonus = null;
			for (int i = 0; i < bonusList.Count; ++i)
			{
				if (bonusList[i].Rectangle.Contains(X * Constants.TEXTURE_SIZE, Y * Constants.TEXTURE_SIZE))
				{
					currentBonus = bonusList[i];
				}
			}

			if (currentBonus != null)
			{
				//if (currentBonus.Param != TileType.Gold)
					hero.SetBonus(currentBonus.Param);
				//else
				//{
				//   Random rand = new Random();
				//   saveData.Gold += rand.Next(1, 4);
				//}
				bonusList.Remove(currentBonus);
			}

			hero.Move(X * 20, Y * 20);
			map[X, Y] = (int)MapItem.Hero;
		}

		/// <summary>
		/// Class for leaf of dijkstra graph
		/// </summary>
		private class DijkstraItem
		{
			/// <summary>
			/// Weight
			/// </summary>
			public int W { get; set; }

			/// <summary>
			/// X on map
			/// </summary>
			public int X { get; set; }

			/// <summary>
			/// Y on map
			/// </summary>
			public int Y { get; set; }

			/// <summary>
			/// Construct
			/// </summary>
			/// <param name="w">Weight</param>
			/// <param name="x">X on map</param>
			/// <param name="y">Y on map</param>
			public DijkstraItem(int w, int x, int y)
			{
				this.W = w;
				this.X = x;
				this.Y = y;
			}
		}

		/// <summary>
		/// Build dijkstra graph
		/// </summary>
		/// <param name="mas">Graph</param>
		private void Dijkstra(out int[,] mas)
		{
			mas = new int[Constants.MAP_SIZE, Constants.MAP_SIZE];
			for (int i = 0; i < Constants.MAP_SIZE; ++i)
				for (int j = 0; j < Constants.MAP_SIZE; ++j)
					mas[i, j] = -1;

			bool[,] finishItem = new bool[Constants.MAP_SIZE, Constants.MAP_SIZE];

			mas[hero.Rectangle.X / Constants.MAP_SIZE, hero.Rectangle.Y / Constants.MAP_SIZE] = 0;

			List<DijkstraItem> nextStep = new List<DijkstraItem>();
			nextStep.Add(new DijkstraItem(0, hero.Rectangle.X / Constants.MAP_SIZE,
				hero.Rectangle.Y / Constants.MAP_SIZE));

			DijkstraItem min;

			while (nextStep.Count != 0)
			{
				//search min element
				min = nextStep[0];
				foreach (DijkstraItem item in nextStep)
				{
					if (item.W < min.W)
						min = item;
				}

				//Calc min leaf
				if (min.X != 0 && min.Y != 0)
				{
					DijkstraCheckLeaf(min.X - 1, min.Y - 1, mas, finishItem, min, nextStep);
				}
				if (min.X != 0 && min.Y != Constants.MAP_SIZE - 1)
				{
					DijkstraCheckLeaf(min.X - 1, min.Y + 1, mas, finishItem, min, nextStep);
				}
				if (min.X != Constants.MAP_SIZE - 1 && min.Y != 0)
				{
					DijkstraCheckLeaf(min.X + 1, min.Y - 1, mas, finishItem, min, nextStep);
				}
				if (min.X != Constants.MAP_SIZE - 1 && min.Y != Constants.MAP_SIZE - 1)
				{
					DijkstraCheckLeaf(min.X + 1, min.Y + 1, mas, finishItem, min, nextStep);
				}
				if (min.X != 0)
				{
					DijkstraCheckLeaf(min.X - 1, min.Y, mas, finishItem, min, nextStep);
				}
				if (min.Y != 0)
				{
					DijkstraCheckLeaf(min.X, min.Y - 1, mas, finishItem, min, nextStep);
				}
				if (min.X != Constants.MAP_SIZE - 1)
				{
					DijkstraCheckLeaf(min.X + 1, min.Y, mas, finishItem, min, nextStep);
				}
				if (min.Y != Constants.MAP_SIZE - 1)
				{
					DijkstraCheckLeaf(min.X, min.Y + 1, mas, finishItem, min, nextStep);
				}

				//remove and finish min leaf
				nextStep.Remove(min);
				finishItem[min.X, min.Y] = true;
		
			}
		}

		/// <summary>
		/// Find min leaf in current min leaf and next leaf
		/// </summary>
		/// <param name="X">X on map</param>
		/// <param name="Y">Y on map</param>
		/// <param name="mas">Graph</param>
		/// <param name="finishItem">Items of finish calc</param>
		/// <param name="min">Min leaf</param>
		/// <param name="nextStep">Next leaf</param>
		private void DijkstraCheckLeaf(int X, int Y, int[,] mas, bool[,] finishItem, 
			DijkstraItem min, List<DijkstraItem> nextStep)
		{
			if (map[X, Y] == (int)MapItem.None && visible[X, Y] == 1)
			{
				if (!finishItem[X, Y])
				{
					int masCur = mas[min.X, min.Y];
					if (mas[X, Y] == -1 || mas[X, Y] > masCur + 1)
					{
						mas[X, Y] = masCur + 1;
						nextStep.Add(new DijkstraItem(masCur + 1, X, Y));
					}
				}
			}
		}
	}
}
