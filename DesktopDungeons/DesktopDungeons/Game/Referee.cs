using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DesktopDungeons.TileClasses;

namespace DesktopDungeons
{
	/// <summary>
	/// Referee on game
	/// </summary>
	public class Referee
	{
		/// <summary>
		/// Map of game
		/// </summary>
		private Map map;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="parMap">Map of game</param>
		public Referee(Map parMap)
		{
			map = parMap;
		}

		/// <summary>
		/// Change state of game
		/// </summary>
		/// <param name="parGameState"></param>
		public void ChangeGameState(GameState parGameState)
		{
			switch (parGameState)
			{
				case GameState.Game:

					break;
				case GameState.Menu:
					
					break;
				case GameState.Score:
					break;
			}
			map.GameState = parGameState;
		}

		/// <summary>
		/// Fight among hero and monster
		/// </summary>
		/// <param name="parHero">Hero</param>
		/// <param name="parMonster">Monster</param>
		public void Fight(Hero parHero, Monster parMonster)
		{
			if (parHero.ClassHero == Class.Assassin)
			{
				if (parMonster.Level < parHero.Level)
				{
					parHero.AddExperience(parMonster.Level, true, 0);
					map.DeathMonster(parMonster);
					return;
				}
				
				if (VisibleAroundMonster(parMonster))
					parHero.FirstStrike = true;
			}

			//first strike
			if (parHero.CheckFirstStrike(parMonster))
			{
				if (FirstStrikeSecond(parHero, parMonster))
				{
					if (parHero.ClassHero == Class.Paladin && parMonster.Undead)
						parHero.HealthCurrent += parHero.HealthMax / 2;
					parHero.AddExperience(parMonster.Level, true, 0);
					map.DeathMonster(parMonster);
					return;
				}
				if (FirstStrikeSecond(parMonster, parHero))
				{
					if (parHero.Death())
					{
						if (parHero.ClassHero == Class.Crusader)
						{
							parMonster.HealthCurrent -= parHero.DamageBase * 3;
							if (parMonster.HealthCurrent <= 0)
							{
								parHero.AddExperience(parMonster.Level, true, 0);
								map.DeathMonster(parMonster);
							}
						}
						ChangeGameState(GameState.Score);
					}
				}
				if (parHero.ClassHero == Class.Sorcerer)
				{
					parMonster.SetDamage(parHero.AttackResponse(true), true);
					if (parMonster.HealthCurrent <= 0)
					{
						parHero.AddExperience(parMonster.Level, true, 0);
						map.DeathMonster(parMonster);
						return;
					}
				}
			}
			else
			{
				if (FirstStrikeSecond(parMonster, parHero))
				{					
					if (parHero.Death())
					{
						if (parHero.ClassHero == Class.Crusader)
						{
							parMonster.HealthCurrent -= parHero.DamageBase * 3;
							if (parMonster.HealthCurrent <= 0)
							{
								parHero.AddExperience(parMonster.Level, true, 0);
								map.DeathMonster(parMonster);
							}
						}
						ChangeGameState(GameState.Score);
					}
				}
				if (parHero.ClassHero == Class.Sorcerer)
				{
					parMonster.SetDamage(parHero.AttackResponse(true), true);
					if (parMonster.HealthCurrent <= 0)
					{
						if (parHero.ClassHero == Class.Paladin && parMonster.Undead)
							parHero.HealthCurrent += parHero.HealthMax / 2;
						parHero.AddExperience(parMonster.Level, true, 0);
						map.DeathMonster(parMonster);
						return;
					}
				}
				if (FirstStrikeSecond(parHero, parMonster))
				{
					parHero.AddExperience(parMonster.Level, true, 0);   
					map.DeathMonster(parMonster);
				}
			}
		}

		/// <summary>
		/// Pick magin on map and set it in slot of hero
		/// </summary>
		public void AddMagicToHeroSlot()
		{
			MagicTile currentMagicTile = null;
			foreach (MagicTile magicTile in map.magicTileList)
			{
				if (magicTile.Rectangle == map.hero.Rectangle)
					currentMagicTile = magicTile;
			}
			if (currentMagicTile != null)
			{
				int i = 0;
				int count = map.hero.ClassHero == Class.Wizard 
					? Constants.COUNT_HERO_SLOTS_FOR_WIZARD : Constants.COUNT_HERO_SLOTS_NORMAL;
				for (; i <= count; ++i)
				{
					if (i == count)
						return;
					if (map.hero.Magics[i] == MagicType.Empty)
						break;
				}
				map.hero.Magics[i] = currentMagicTile.Magic;
				map.magicTileList.Remove(currentMagicTile);
			}
		}

		/// <summary>
		/// Apply magic
		/// </summary>
		/// <param name="parMagicType">Type magic</param>
		/// <param name="X">X on game</param>
		/// <param name="Y">Y on game</param>
		/// <returns></returns>
		public bool ApplyMagic(MagicType parMagicType, int X, int Y)
		{
			switch (parMagicType)
			{
				#region case MagicType.Converter:
				case MagicType.Converter:
					if (X == -1 && Y == -1)
						return false;
					if (X == Constants.COUNT_HERO_SLOTS_FOR_WIZARD)
						return false;
					if (map.hero.Magics[X] != MagicType.Empty)
						map.hero.ConvertMagic(map.hero.Magics[X]);
					break;
				#endregion
				#region case MagicType.TeleMonster:
				case MagicType.TeleMonster:
					if (X == -1 && Y == -1)
						return false;
					foreach (Monster monster in map.monsterList)
					{
						if (monster.Rectangle.Contains(X, Y))
						{
							if (!map.ClickHeroMoveToMonster(X / Constants.TEXTURE_SIZE, 
								Y / Constants.TEXTURE_SIZE))
								return false;

							map.hero.ApplyMagic(parMagicType);
							map.map[monster.Rectangle.X / Constants.TEXTURE_SIZE, 
								monster.Rectangle.Y / Constants.TEXTURE_SIZE]
								= (int) MapItem.None;
							Random rand = new Random();
							int Xnew, Ynew;
							do
							{
								Xnew = rand.Next(Constants.MAP_SIZE);
								Ynew = rand.Next(Constants.MAP_SIZE);
							} while (map.map[Xnew, Ynew] != (int)MapItem.None);
							map.map[Xnew, Ynew] = (int)MapItem.Monster;
							monster.Move(Xnew * Constants.TEXTURE_SIZE, Ynew * Constants.TEXTURE_SIZE);
							return true;
						}
					}
					break;
				#endregion	
				#region case MagicType.Heal:
				case MagicType.Heal:
					map.hero.ApplyMagic(parMagicType);
					map.hero.Poisoned = false;
					map.hero.HealthCurrent += Constants.COUNT_HEAL_HEALTH_POINT * map.hero.Level;
					return true;
				#endregion
				#region case MagicType.Endwall:
				case MagicType.Endwall:
					if (X == -1 && Y == -1)
						return false;
					if (map.map[X / Constants.TEXTURE_SIZE, Y / Constants.TEXTURE_SIZE] == (int)MapItem.Wall)
					{
						if (!map.ClickHeroMoveToMonster(X / Constants.TEXTURE_SIZE, Y / Constants.TEXTURE_SIZE))
							return false;
						map.hero.ApplyMagic(parMagicType);
						map.map[X / Constants.TEXTURE_SIZE, Y / Constants.TEXTURE_SIZE] = (int)MapItem.None;
					}
					return true;
				#endregion
				#region case MagicType.Might:
				case MagicType.Might:
					map.hero.ApplyMagic(parMagicType);
					map.hero.Might = true;
					break;
				#endregion
				#region case MagicType.Reveal:
				case MagicType.Reveal:
					map.hero.ApplyMagic(parMagicType);
					map.SearchRandom(Constants.COUNT_REVEAL_MAGIC_BLOCKS);
					break;
				#endregion
				#region case MagicType.Fireball:
				case MagicType.Fireball:
					if (X == -1 && Y == -1)
						return false;
					if (map.map[X / Constants.TEXTURE_SIZE, Y / Constants.TEXTURE_SIZE] != (int)MapItem.Monster)
						return false;
					if (!map.ClickHeroMoveToMonster(X / Constants.TEXTURE_SIZE, Y / Constants.TEXTURE_SIZE))
						return false;

					for (int i = 0; i < map.monsterList.Count; ++i)
					{
						if (map.monsterList[i].Rectangle.Contains(X, Y))
						{
							map.hero.ApplyMagic(parMagicType);
							map.monsterList[i].SetDamage(Constants.DAMAGE_FIREBALL_PER_LEVEL * map.hero.Level, true);
							if (map.monsterList[i].HealthCurrent <= 0)
							{
								if (map.hero.ClassHero == Class.Paladin && map.monsterList[i].Undead)
									map.hero.HealthCurrent += map.hero.HealthMax / 2;
								map.hero.AddExperience(map.monsterList[i].Level, true, 0);
								map.DeathMonster(map.monsterList[i]);
								--i;
							}
							return true;
						}
					}
					return false;
				#endregion
				#region case MagicType.Petrify:
				case MagicType.Petrify:
					if (X == -1 && Y == -1)
						return false;
					if (map.map[X / Constants.TEXTURE_SIZE, Y / Constants.TEXTURE_SIZE] != (int)MapItem.Monster)
						return false;
					Monster monsterPetrify = null;
					foreach (Monster monster in map.monsterList)
					{
						if (monster.Rectangle.Contains(X, Y))
						{
							if (monster.Level == Constants.MONSTER_BOSS_LEVEL)
								return false;
							monsterPetrify = monster;
							break;
						}
					}
					if (!map.ClickHeroMoveToMonster(X / Constants.TEXTURE_SIZE, Y / Constants.TEXTURE_SIZE))
						return false;

					map.hero.ApplyMagic(parMagicType);
					map.monsterList.Remove(monsterPetrify);
					map.map[X / Constants.TEXTURE_SIZE, Y / Constants.TEXTURE_SIZE] = (int)MapItem.Wall;
					return true;
				#endregion
				#region case MagicType.Poison:
				case MagicType.Poison:
					if (X == -1 && Y == -1)
						return false;
					if (map.map[X / Constants.TEXTURE_SIZE, Y / Constants.TEXTURE_SIZE] != (int)MapItem.Monster)
						return false;
					if (!map.ClickHeroMoveToMonster(X / Constants.TEXTURE_SIZE, Y / Constants.TEXTURE_SIZE))
						return false;

					foreach (Monster monster in map.monsterList)
					{
						if (monster.Rectangle.Contains(X, Y))
						{
							map.hero.ApplyMagic(parMagicType);
							if (!monster.Undead)
								monster.Poisoned = true;
							return true;
						}
					}
					break;
				#endregion
				#region case MagicType.FirstStrike:
				case MagicType.FirstStrike:
					map.hero.ApplyMagic(parMagicType);
					map.hero.FirstStrike = true;
					break;
				#endregion
				#region case MagicType.KillProtect:
				case MagicType.KillProtect:
					map.hero.ApplyMagic(parMagicType);
					map.hero.KillProtect = true;
					break;
				#endregion
				#region case MagicType.Summon:
				case MagicType.Summon:
					map.hero.ApplyMagic(parMagicType);

					int minDist = int.MaxValue;
					Monster summonMonster = null;

					foreach (Monster monster in map.monsterList)
					{
						if (monster.Level == map.hero.Level)
						{
							int dX = (map.hero.Rectangle.X - monster.Rectangle.X) / Constants.TEXTURE_SIZE;
							dX *= dX < 0 ? -1 : 1;
							int dY = (map.hero.Rectangle.Y - monster.Rectangle.Y) / Constants.TEXTURE_SIZE;
							dY *= dY < 0 ? -1 : 1;

							int tempDist = (dX * dX + dY * dY);
							if (minDist > tempDist)
							{
								minDist = tempDist;
								summonMonster = monster;
							}
						}
					}

					if (summonMonster == null)
						return true;

					// int = (X * 100) + Y
					List<int> position = new List<int>();
					int X0 = map.hero.Rectangle.X / Constants.TEXTURE_SIZE;
					int Y0 = map.hero.Rectangle.Y / Constants.TEXTURE_SIZE;

					if (X0 > 0)
					{
						if (Y0 > 0)
							if (map.map[X0 - 1, Y0 - 1] == (int)MapItem.None)
								position.Add((X0 - 1) * 100 + Y0 - 1);
						if (map.map[X0 - 1, Y0] == (int)MapItem.None)
							position.Add((X0 - 1) * 100 + Y0);
						if (Y0 < Constants.MAP_SIZE - 1)
							if (map.map[X0 - 1, Y0 + 1] == (int)MapItem.None)
								position.Add((X0 - 1) * 100 + Y0 + 1);
					}

					if (Y0 > 0)
						if (map.map[X0, Y0 - 1] == (int)MapItem.None)
							position.Add(X0 * 100 + Y0 - 1);
					if (map.map[X0, Y0] == (int)MapItem.None)
						position.Add(X0 * 100 + Y0);
					if (Y0 < Constants.MAP_SIZE - 1)
						if (map.map[X0, Y0 + 1] == (int)MapItem.None)
							position.Add(X0 * 100 + Y0 + 1);

					if (X0 < Constants.MAP_SIZE - 1)
					{
						if (Y0 > 0)
							if (map.map[X0 + 1, Y0 - 1] == (int)MapItem.None)
								position.Add((X0 + 1) * 100 + Y0 - 1);
						if (map.map[X0 + 1, Y0] == (int)MapItem.None)
							position.Add((X0 + 1) * 100 + Y0);
						if (Y0 < Constants.MAP_SIZE - 1)
							if (map.map[X0 + 1, Y0 + 1] == (int)MapItem.None)
								position.Add((X0 + 1) * 100 + Y0 + 1);
					}

					if (position.Count == 0)
						return true;

					Random randSummon = new Random();
					int indexRand = randSummon.Next(position.Count);

					int aroundX = position[indexRand] / 100;
					int aroundY = position[indexRand] % 100;

					map.map[summonMonster.Rectangle.X / Constants.TEXTURE_SIZE,
						summonMonster.Rectangle.Y / Constants.TEXTURE_SIZE] = (int)MapItem.None;
					summonMonster.Move(aroundX * Constants.TEXTURE_SIZE, aroundY * Constants.TEXTURE_SIZE);
					map.map[aroundX, aroundY] = (int)MapItem.Monster;

					return true;
				#endregion
				#region case MagicType.Blood:
				case MagicType.Blood:
					map.hero.ApplyMagic(parMagicType);
					map.hero.Blood = true;
					break;
				#endregion
				#region case MagicType.TeleSelf:
				case MagicType.TeleSelf:
					map.hero.ApplyMagic(parMagicType);

					Random randTele = new Random();
					int XTele, YTele;
					do
					{
						XTele = randTele.Next(Constants.MAP_SIZE);
						YTele = randTele.Next(Constants.MAP_SIZE);
					} while (map.map[XTele, YTele] != (int)MapItem.None);

					map.HeroReplace(XTele, YTele);
					return true;
				#endregion
			}
			return true;
		}

		/// <summary>
		/// Check vilible blocks around monster
		/// </summary>
		/// <param name="parMonster">Monster</param>
		/// <returns>All blocks around monster is visible</returns>
		public bool VisibleAroundMonster(Unit parMonster)
		{
			bool visibleAroundMonster = true;
			if (parMonster.Rectangle.X > 0 && parMonster.Rectangle.Y > 0)
				visibleAroundMonster = visibleAroundMonster
					&& map.visible[(parMonster.Rectangle.X / Constants.TEXTURE_SIZE) - 1,
						(parMonster.Rectangle.Y / Constants.TEXTURE_SIZE) - 1] == 1;
			if (parMonster.Rectangle.X > 0)
				visibleAroundMonster = visibleAroundMonster
					&& map.visible[(parMonster.Rectangle.X / Constants.TEXTURE_SIZE) - 1,
						(parMonster.Rectangle.Y / Constants.TEXTURE_SIZE)] == 1;
			if (parMonster.Rectangle.X > 0 && parMonster.Rectangle.Y < Constants.MAP_SIZE - 1)
				visibleAroundMonster = visibleAroundMonster
					&& map.visible[(parMonster.Rectangle.X / Constants.TEXTURE_SIZE) - 1,
						(parMonster.Rectangle.Y / Constants.TEXTURE_SIZE) + 1] == 1;
			if (parMonster.Rectangle.Y > 0)
				visibleAroundMonster = visibleAroundMonster
					&& map.visible[(parMonster.Rectangle.X / Constants.TEXTURE_SIZE),
						(parMonster.Rectangle.Y / Constants.TEXTURE_SIZE) - 1] == 1;
			visibleAroundMonster = visibleAroundMonster
				&& map.visible[(parMonster.Rectangle.X / Constants.TEXTURE_SIZE),
					(parMonster.Rectangle.Y / Constants.TEXTURE_SIZE)] == 1;
			if (parMonster.Rectangle.Y < Constants.MAP_SIZE - 1)
				visibleAroundMonster = visibleAroundMonster
					&& map.visible[(parMonster.Rectangle.X / Constants.TEXTURE_SIZE),
						(parMonster.Rectangle.Y / Constants.TEXTURE_SIZE) + 1] == 1;
			if (parMonster.Rectangle.X < Constants.MAP_SIZE - 1 && parMonster.Rectangle.Y > 0)
				visibleAroundMonster = visibleAroundMonster
					&& map.visible[(parMonster.Rectangle.X / Constants.TEXTURE_SIZE) + 1,
						(parMonster.Rectangle.Y / Constants.TEXTURE_SIZE) - 1] == 1;
			if (parMonster.Rectangle.X < Constants.MAP_SIZE - 1)
				visibleAroundMonster = visibleAroundMonster
					&& map.visible[(parMonster.Rectangle.X / Constants.TEXTURE_SIZE) + 1,
						(parMonster.Rectangle.Y / Constants.TEXTURE_SIZE)] == 1;
			if (parMonster.Rectangle.X < Constants.MAP_SIZE - 1 && parMonster.Rectangle.Y < Constants.MAP_SIZE - 1)
				visibleAroundMonster = visibleAroundMonster
					&& map.visible[(parMonster.Rectangle.X / Constants.TEXTURE_SIZE) + 1,
						(parMonster.Rectangle.Y / Constants.TEXTURE_SIZE) + 1] == 1;
			return visibleAroundMonster;
		}

		/// <summary>
		/// First unit strike second unit
		/// </summary>
		/// <param name="parAttacker">Attacker Unit</param>
		/// <param name="parDefencer">Defencer Unit</param>
		/// <returns></returns>
		private bool FirstStrikeSecond(Unit parAttacker, Unit parDefencer)
		{
			parDefencer.SetDamage(parAttacker.GetDamageNext(false, parDefencer), parAttacker.MagicAttack);

			if (parAttacker.PoisonStrike == true && parDefencer.Undead == false)
				parDefencer.Poisoned = true;

			if (parAttacker.ManaBurn == true)
			{
				parDefencer.ManaCurrent = 0;
				parDefencer.ManaBurned = true;
			}

			if (parAttacker.DeathGaze > 0
				&& (parAttacker.DeathGaze / 100 > (parDefencer.HealthCurrent / parDefencer.HealthMax)))
				return true;

			if (parDefencer.HealthCurrent <= 0)
			{
				return true;
			}
			return false; 
		}
	}
}
