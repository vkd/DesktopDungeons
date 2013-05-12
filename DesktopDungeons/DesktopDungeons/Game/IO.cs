using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using LoadLocalization;
using DesktopDungeons.TileClasses;

namespace DesktopDungeons
{
	/// <summary>
	/// Input and output
	/// </summary>
	public class IO
	{
		#region Variables
		#region Input state
		/// <summary>
		/// Last state of keyboard
		/// </summary>
		private KeyboardState keyStateLast;

		/// <summary>
		/// Now state of keyboard
		/// </summary>
		private KeyboardState keyStateNow;

		/// <summary>
		/// Last state of mouse
		/// </summary>
		private MouseState mouseStateLast;

		/// <summary>
		/// Now state of mouse
		/// </summary>
		private MouseState mouseStateNow;
		#endregion

		/// <summary>
		/// Instance of the game class
		/// </summary>
		private Game game;

		/// <summary>
		/// Map of game
		/// </summary>
		private Map map;

		/// <summary>
		/// Referee of game
		/// </summary>
		private Referee referee;

		/// <summary>
		/// All text string of game
		/// </summary>
		private Localization localization;

		/// <summary>
		/// Choice race of hero in game menu
		/// </summary>
		private Race HeroesRace { get; set; }

		/// <summary>
		/// Choice class of hero in game menu
		/// </summary>
		private Class HeroesClass { get; set; }

		/// <summary>
		/// Information, that draw in menu
		/// </summary>
		private string InformationInMenu { get; set; }

		/// <summary>
		/// Monster, that under game cursor
		/// </summary>
		private Monster monsterCurrent;

		#region Buttons
		/// <summary>
		/// List of race buttons
		/// </summary>
		private List<ButtonMenu<Race>> buttonRaces;

		/// <summary>
		/// List of class buttons
		/// </summary>
		private List<ButtonMenu<Class>> buttonClass;

		/// <summary>
		/// Start button
		/// </summary>
		private ButtonMenu<GameState> buttonStart;

		/// <summary>
		/// Exit button
		/// </summary>
		private ButtonMenu<GameState> buttonExit;

		/// <summary>
		/// About button
		/// </summary>
		private ButtonMenu<GameState> buttonAbout;

		/// <summary>
		/// Magic buttons on heroes slots
		/// </summary>
		private ButtonMagic[] buttonMagic;

		/// <summary>
		/// Button in info area, using when hero stand on magic tile
		/// </summary>
		private ButtonInfo buttonInfo;

		/// <summary>
		/// Button of health potion, using for hero drink potion
		/// </summary>
		private Button<TileType> buttonPotionHealth;

		/// <summary>
		/// Button of mana potion, using for hero drink potion
		/// </summary>
		private Button<TileType> buttonPotionMana;

		/// <summary>
		/// Heroes attack boost, using for draw attack damage on info area
		/// </summary>
		private Button<TileType> buttonAttack;

		/// <summary>
		/// Bonus button, that under game cursor
		/// </summary>
		private Button<TileType> buttonBonusCurrent;

		/// <summary>
		/// Magic button, that under game cursor
		/// </summary>
		private ButtonMagic buttonMagicCurrent;
		#endregion

		/// <summary>
		/// Current index of chosen magic button
		/// </summary>
		private int CurrentMagicIndex { get; set; }
		#endregion

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="parGame">Instance of Game class</param>
		/// <param name="parMap">Instance of Map class</param>
		/// <param name="parReferee">Instance of Referee class</param>
		/// <param name="parLocalization">Instance of Localization class</param>
		public IO(Game parGame, Map parMap, Referee parReferee, Localization parLocalization)
		{
			game = parGame;
			map = parMap;
			referee = parReferee;
			localization = parLocalization;

			InformationInMenu = "";
			CurrentMagicIndex = -1;
		}

		/// <summary>
		/// Initialization of menu interface
		/// </summary>
		public void InitializeMenu()
		{
			//Buttons of race
			buttonRaces = new List<ButtonMenu<Race>>();
			for (int i = 0; i < 7; ++i)
			{
				buttonRaces.Add(new ButtonMenu<Race>(new Rectangle(5 + 79 * i, 25, 75, 30),
					localization.Get("RACE_" + (i + 1)), (Race)i));
			}

			//current race
			if (map.hero == null)
			{
				//start heroes race
				buttonRaces[(int)Race.Human].Selected = true;
				HeroesRace = Race.Human;
			}
			else
			{
				foreach (ButtonMenu<Race> button in buttonRaces)
				{
					if (button.Parameter == map.hero.RaceHero)
						button.Selected = true;
				}
				HeroesRace = map.hero.RaceHero;
			}

			//marked button
			foreach (ButtonMenu<Race> button in buttonRaces)
			{
				if (map.saveData._ListWinRace.Any(p => p == button.Parameter))
					button.MarkedIsWin = true;
			}
			if (map.saveData._ListWinRace.Count < 1)
				buttonRaces[4].Invisible = true;
			if (map.saveData._ListWinRace.Count < 2)
				buttonRaces[5].Invisible = true;
			if (map.saveData._ListWinRace.Count < 3)
				buttonRaces[6].Invisible = true;

			//button of class
			buttonClass = new List<ButtonMenu<Class>>();
			for (int y = 0; y < 4; ++y)
			{
				for (int x = 0; x < 3; ++x)
				{
					buttonClass.Add(new ButtonMenu<Class>(new Rectangle(30 + 90 * y, 100 + 35 * x, 85, 30),
						localization.Get("CLASS_" + (y * 3 + x + 1)), (Class)(y * 3 + x)));
				}
			}

			// 3 if with Tinker
			int countBonusClasses = 3;
			--countBonusClasses;

			//Bonus classes
			for (int i = 0; i < countBonusClasses; ++i)
			{
				buttonClass.Add(new ButtonMenu<Class>(new Rectangle(430, 100 + 35 * i, 85, 30),
					localization.Get("CLASS_" + (i + 13)), (Class)(12 + i)));
			}

			//current class
			if (map.hero == null)
			{
				//start heroes class
				buttonClass[(int)Class.Fighter].Selected = true;
				HeroesClass = Class.Fighter;
			}
			else
			{
				foreach (ButtonMenu<Class> button in buttonClass)
				{
					if (button.Parameter == map.hero.ClassHero)
						button.Selected = true;
				}
				HeroesClass = map.hero.ClassHero;
			}

			//marked class buttons
			foreach (ButtonMenu<Class> button in buttonClass)
			{
				if (map.saveData._ListWinClass.Any(p => p == button.Parameter))
					button.MarkedIsWin = true;
			}

			//dependes of classes
			DependOnClass(Class.Berserker, Class.Fighter);
			DependOnClass(Class.Rogue, Class.Thief);
			DependOnClass(Class.Monk, Class.Priest);
			DependOnClass(Class.Sorcerer, Class.Wizard);

			DependOnClass(Class.Warlord, Class.Berserker);
			DependOnClass(Class.Assassin, Class.Rogue);
			DependOnClass(Class.Paladin, Class.Monk);
			DependOnClass(Class.Bloodmage, Class.Sorcerer);

			DependOnClass(Class.Transmuter, Class.Bloodmage);
			DependOnClass(Class.Crusader, Class.Warlord);
			//DependOnClass(Class.Tinker, Class.Assassin);

			//Other buttons
			buttonStart = new ButtonMenu<GameState>(new Rectangle(350, 250, 150, 60),
				localization.Get("BUTTON_START"), GameState.Menu);
			buttonStart.Selected = true;
			buttonExit = new ButtonMenu<GameState>(new Rectangle(450, 383, 80, 15),
				localization.Get("BUTTON_EXIT"), GameState.Menu);
			buttonAbout = new ButtonMenu<GameState>(new Rectangle(20, 383, 100, 15),
				 localization.Get("BUTTON_ABOUT"), GameState.About);
		}

		/// <summary>
		/// Update IO class
		/// </summary>
		/// <param name="gameTime">GameTime</param>
		public void Update(GameTime gameTime)
		{
			keyStateNow = Keyboard.GetState();
			mouseStateNow = Mouse.GetState();

			//position of mouse with scale
			int stateX = (int)((float)mouseStateNow.X / Game.ScaleWindow);
			int stateY = (int)((float)mouseStateNow.Y / Game.ScaleWindow);

			// ===== Keyboard Update =====

			if (keyStateNow.IsKeyDown(Keys.Escape))
				game.Exit();
			else if (keyStateNow.IsKeyDown(Keys.F4) && keyStateLast.IsKeyUp(Keys.F4))
				game.ChangeFullScreen();
			else if (keyStateNow.IsKeyDown(Keys.H) && keyStateLast.IsKeyUp(Keys.H))
				map.hero.ApplyPotion(TileType.HealthPotion);
			else if (keyStateNow.IsKeyDown(Keys.M) && keyStateLast.IsKeyUp(Keys.M))
				map.hero.ApplyPotion(TileType.ManaPotion);
			else if (keyStateNow.IsKeyDown(Keys.F2) && keyStateLast.IsKeyUp(Keys.F2))
				game.DoubleScreen();
			else if (keyStateNow.IsKeyDown(Keys.Space) && keyStateLast.IsKeyUp(Keys.Space))
				referee.AddMagicToHeroSlot();
			#region NumberKeyboard
			else if (keyStateNow.IsKeyDown(Keys.D1) && keyStateLast.IsKeyUp(Keys.D1))
			{
				int index = 0;
				if (map.hero.Magics[index] != MagicType.Empty
					&& map.hero.ManaCurrent >= map.hero.CostMagic(map.hero.Magics[index]))
				{
					CurrentMagicIndex = index;
					if (referee.ApplyMagic(map.hero.Magics[CurrentMagicIndex], -1, -1))
						CurrentMagicIndex = -1;
				}
			}
			else if (keyStateNow.IsKeyDown(Keys.D2) && keyStateLast.IsKeyUp(Keys.D2))
			{
				int index = 1;
				if (map.hero.Magics[index] != MagicType.Empty
					&& map.hero.ManaCurrent >= map.hero.CostMagic(map.hero.Magics[index]))
				{
					CurrentMagicIndex = index;
					if (referee.ApplyMagic(map.hero.Magics[CurrentMagicIndex], -1, -1))
						CurrentMagicIndex = -1;
				}
			}
			else if (keyStateNow.IsKeyDown(Keys.D3) && keyStateLast.IsKeyUp(Keys.D3))
			{
				int index = 2;
				if (map.hero.Magics[index] != MagicType.Empty
					&& map.hero.ManaCurrent >= map.hero.CostMagic(map.hero.Magics[index]))
				{
					CurrentMagicIndex = index;
					if (referee.ApplyMagic(map.hero.Magics[CurrentMagicIndex], -1, -1))
						CurrentMagicIndex = -1;
				}
			}
			else if (keyStateNow.IsKeyDown(Keys.D4) && keyStateLast.IsKeyUp(Keys.D4))
			{
				int index = 3;
				if (map.hero.Magics[index] != MagicType.Empty
					&& map.hero.ManaCurrent >= map.hero.CostMagic(map.hero.Magics[index]))
				{
					CurrentMagicIndex = index;
					if (referee.ApplyMagic(map.hero.Magics[CurrentMagicIndex], -1, -1))
						CurrentMagicIndex = -1;
				}
			}
			else if (keyStateNow.IsKeyDown(Keys.D5) && keyStateLast.IsKeyUp(Keys.D5))
			{
				int index = 4;
				if (map.hero.Magics[index] != MagicType.Empty
					&& map.hero.ManaCurrent >= map.hero.CostMagic(map.hero.Magics[index]))
				{
					CurrentMagicIndex = index;
					if (referee.ApplyMagic(map.hero.Magics[CurrentMagicIndex], -1, -1))
						CurrentMagicIndex = -1;
				}
			}
			#endregion

			//===== Mouse Update =====

			//mouse click
			if (mouseStateNow.LeftButton == ButtonState.Pressed && mouseStateLast.LeftButton == ButtonState.Released)
			{
				//choose one magic
				if (CurrentMagicIndex != -1)
				{
					//chosen CONVERTER
					if (buttonMagic[CurrentMagicIndex].Parameter == MagicType.Converter)
					{
						for (int i = 0; i < buttonMagic.Length; ++i)
						{
							if (buttonMagic[i] != null && buttonMagic[i].Rectangle.Contains(stateX, stateY))
							{
								referee.ApplyMagic(map.hero.Magics[CurrentMagicIndex], i, stateY);
								break;
							}
						}
					}
					//not CONVERTER, apply magic, if posible
					else if (stateX >= 0 && stateY >= 0
						&& stateX < Constants.MAP_SIZE * Constants.TEXTURE_SIZE 
						&& stateY < Constants.MAP_SIZE * Constants.TEXTURE_SIZE
						&& map.visible[stateX / Constants.TEXTURE_SIZE, stateY / Constants.TEXTURE_SIZE] != 0)
							referee.ApplyMagic(map.hero.Magics[CurrentMagicIndex], stateX, stateY);

					CurrentMagicIndex = -1;
				}
				else
					MouseClick(stateX, stateY);
			}

			//not mouse click
			switch (map.GameState)
			{
				#region case GameState.Menu
				case GameState.Menu:
					//===== information =====
					bool cleanString = true;

					//if button of race under cursor
					for (int i = 0; i < buttonRaces.Count; ++i)
					{
						if (buttonRaces[i].Rectangle.Contains(stateX, stateY))
						{
							if (buttonRaces[i].Invisible)
							{
								switch (buttonRaces[i].Parameter)
								{
									case Race.Gnome:
										InformationInMenu = localization.Get("RACE_INVISIBLE_GNOME");
										break;
									case Race.Goblin:
										InformationInMenu = localization.Get("RACE_INVISIBLE_GOBLIN");
										break;
									case Race.Orc:
										InformationInMenu = localization.Get("RACE_INVISIBLE_ORC");
										break;
								}
							}
							else
							{
								InformationInMenu = localization.Get("RACE_" + (i + 1));
								InformationInMenu += " " + localization.Get("CONVERT_SKILL_STRING") + "\n";
								InformationInMenu += localization.Get("CONVERT_BONUS_" + (i + 1));
							}
							cleanString = false;
						}
					}

					//if button of class under cursor
					for (int i = 0; i < buttonClass.Count; ++i)
					{
						if (buttonClass[i].Rectangle.Contains(stateX, stateY))
						{
							if (buttonClass[i].Invisible)
							{
								switch (buttonClass[i].Parameter)
								{
									case Class.Berserker:
										InformationInMenu = localization.Get("CLASS_INVISIBLE_BERSERKER");
										break;
									case Class.Rogue:
										InformationInMenu = localization.Get("CLASS_INVISIBLE_ROGUE");
										break;
									case Class.Monk:
										InformationInMenu = localization.Get("CLASS_INVISIBLE_MONK");
										break;
									case Class.Sorcerer:
										InformationInMenu = localization.Get("CLASS_INVISIBLE_SORCERER");
										break;
									case Class.Warlord:
										InformationInMenu = localization.Get("CLASS_INVISIBLE_WARLORD");
										break;
									case Class.Assassin:
										InformationInMenu = localization.Get("CLASS_INVISIBLE_ASSASSIN");
										break;
									case Class.Paladin:
										InformationInMenu = localization.Get("CLASS_INVISIBLE_PALADIN");
										break;
									case Class.Bloodmage:
										InformationInMenu = localization.Get("CLASS_INVISIBLE_BLOODMAGE");
										break;
									case Class.Transmuter:
										InformationInMenu = localization.Get("CLASS_INVISIBLE_TRANSMUTER");
										break;
									case Class.Crusader:
										InformationInMenu = localization.Get("CLASS_INVISIBLE_CRUSADER");
										break;
									case Class.Tinker:
										InformationInMenu = localization.Get("CLASS_INVISIBLE_TINKER");
										break;
								}
							}
							else
							{
								InformationInMenu = localization.Get("CLASS_" + (i + 1));
								InformationInMenu += " " + localization.Get("CLASS_TRAITS_STRING") + ":\n";
								InformationInMenu += localization.Get("CLASS_STRING_BONUS_" + (i + 1));
							}
							cleanString = false;
						}
					}

					if (cleanString)
						InformationInMenu = "";
					break;
				#endregion

				#region GameState.Game
				case GameState.Game:
					monsterCurrent = null;

					//if cursor over visible block of map
					if (stateX >= 0 && stateY >= 0
						&& stateX < Constants.MAP_SIZE * Constants.TEXTURE_SIZE 
						&& stateY < Constants.MAP_SIZE * Constants.TEXTURE_SIZE
						&& map.visible[stateX / Constants.TEXTURE_SIZE, stateY / Constants.TEXTURE_SIZE] != 0)
					{
						foreach (Monster monster in map.monsterList)
						{
							if (monster.Rectangle.Contains(stateX, stateY))
							{
								monsterCurrent = monster;
								break;
							}
						}
					}

					buttonBonusCurrent = null;

					if (buttonAttack.Rectangle.Contains(stateX, stateY))
						buttonBonusCurrent = buttonAttack;

					buttonMagicCurrent = null;

					for (int i = 0; i < buttonMagic.Length; ++i)
					{
						if (buttonMagic[i] == null)
							continue;
						if (buttonMagic[i].Rectangle.Contains(stateX, stateY))
						{
							buttonMagicCurrent = buttonMagic[i];
							buttonMagicCurrent.Parameter = map.hero.Magics[i];
						}
					}

					break;
				#endregion
			}

			keyStateLast = Keyboard.GetState();
			mouseStateLast = Mouse.GetState();
		}

		/// <summary>
		/// Draw interface of game
		/// </summary>
		/// <param name="spriteBatch">SpriteBatch</param>
		public void Draw(SpriteBatch spriteBatch)
		{
			switch (map.GameState)
			{
				case GameState.Menu:
					DrawMenu(spriteBatch);
					break;
				case GameState.Game:
					map.Draw(spriteBatch);
					DrawGame(spriteBatch);
					break;
				case GameState.Score:
					DrawScore(spriteBatch);
					break;
				case GameState.About:
					DrawAbout(spriteBatch);
					break;
			}

			//frame abound game window
			DrawSquare(spriteBatch, new Rectangle(0, 0, Constants.WINDOW_NORMAL_SIZE_WIDTH,
				Constants.WINDOW_NORMAL_SIZE_HEIGHT), 1, Color.White);
		}

		/// <summary>
		/// Initialization of game interface
		/// </summary>
		public void InitializeGame()
		{
			//magics
			buttonMagic = new ButtonMagic[5];

			//if Wizard then 4 slot of magic
			if (map.hero.ClassHero == Class.Wizard)
			{
				for (int i = 0; i < 4; ++i)
				{
					buttonMagic[i] = new ButtonMagic(new Rectangle(405 + 30 * i, 175, 20, 20), MagicType.Empty);
				}
			}
			else
			{
				for (int i = 0; i < 3; ++i)
				{
					buttonMagic[i] = new ButtonMagic(new Rectangle(410 + 40 * i, 175, 20, 20), MagicType.Empty);
				}
			}
			buttonMagic[4] = new ButtonMagic(new Rectangle(535, 175, 20, 20), MagicType.Converter);

			//other button
			buttonAttack = new Button<TileType>(new Rectangle(405, 101, 20, 20), TileType.Attackboost);
			buttonPotionHealth = new Button<TileType>(new Rectangle(485, 50, 20, 20), TileType.HealthPotion);
			buttonPotionMana = new Button<TileType>(new Rectangle(485, 77, 20, 20), TileType.ManaPotion);
		}

		/// <summary>
		/// Depend Class from another and set button as invisible
		/// </summary>
		/// <param name="parDependent">Dependent class</param>
		/// <param name="parDependsOnThis">Depends of this class</param>
		private void DependOnClass(Class parDependent, Class parDependsOnThis)
		{
			if (!map.saveData._ListWinClass.Any(p => p == parDependsOnThis))
			{
				foreach (ButtonMenu<Class> button in buttonClass)
				{
					if (button.Parameter == parDependent)
					{
						button.Invisible = true;
						return;
					}
				}
			}
		}

		/// <summary>
		/// Draw text string in center of rectangle area
		/// </summary>
		/// <param name="spriteBatch">SpriteBatch</param>
		/// <param name="parString">Text</param>
		/// <param name="parRect">Size of rectangle</param>
		/// <param name="parColor">Color of text</param>
		private void DrawStringInRectangle(SpriteBatch spriteBatch, String parString,
			Rectangle parRect, Color parColor)
		{
			Vector2 vector = ContentPack.Font.MeasureString(parString);
			vector.X = parRect.X + (int)(parRect.Width - vector.X) / 2;
			vector.Y = parRect.Y + (int)(parRect.Height - vector.Y) / 2;
			spriteBatch.DrawString(ContentPack.Font, parString, vector, parColor);
		}

		/// <summary>
		/// Draw and pave rectangle area by a fon texture 
		/// </summary>
		/// <param name="spriteBatch">SpriteBatch</param>
		/// <param name="parRect">Size of area</param>
		private void DrawFon(SpriteBatch spriteBatch, Rectangle parRect)
		{
			Texture2D fonTexture = ContentPack.FonTexture;

			Rectangle rect = new Rectangle(0, 0, Constants.TEXTURE_SIZE, Constants.TEXTURE_SIZE);
			for (int x = parRect.X; x < parRect.Right; x += Constants.TEXTURE_SIZE)
			{
				for (int y = parRect.Y; y < parRect.Bottom; y += Constants.TEXTURE_SIZE)
				{
					rect.X = x;
					rect.Y = y;
					spriteBatch.Draw(fonTexture, rect, Color.White);
				}
			}
		}

		/// <summary>
		/// Draw menu user interface
		/// </summary>
		/// <param name="spriteBatch">SpriteBatch</param>
		private void DrawMenu(SpriteBatch spriteBatch)
		{
			DrawFon(spriteBatch, new Rectangle(0, 0, Constants.WINDOW_NORMAL_SIZE_WIDTH - 1,
				Constants.WINDOW_NORMAL_SIZE_HEIGHT - 1));

			//Races
			DrawStringInRectangle(spriteBatch, localization.Get("MENU_STRING1") + ":",
				new Rectangle(0, 0, Constants.WINDOW_NORMAL_SIZE_WIDTH, 25), Color.Red);
			foreach (ButtonMenu<Race> button in buttonRaces)
			{
				button.Draw(spriteBatch);
			}

			//Classes
			DrawStringInRectangle(spriteBatch, localization.Get("MENU_STRING2") + ":",
				new Rectangle(0, 75, Constants.WINDOW_NORMAL_SIZE_WIDTH, 25), Color.Red);
			foreach (ButtonMenu<Class> button in buttonClass)
			{
				button.Draw(spriteBatch);
			}

			//Others
			buttonExit.Draw(spriteBatch);
			buttonStart.Draw(spriteBatch);
			buttonAbout.Draw(spriteBatch);

			spriteBatch.DrawString(ContentPack.Font, InformationInMenu, 
				new Vector2(30, 250), Color.White);
		}

		/// <summary>
		/// Draw result string on info area
		/// </summary>
		/// <param name="spriteBatch">SpriteBatch</param>
		private void DrawResultStrikeStringOnInfoArea(SpriteBatch spriteBatch)
		{
			string infoStrike = localization.Get("RESULT_FIGHT_SAFE");
			Color color = Color.Yellow;

			int damageToMonster = map.hero.GetDamageNext(true, monsterCurrent);
			bool liveMonster = monsterCurrent.HealthCurrent > damageToMonster;
			bool firstStrike = monsterCurrent.FirstStrike == false
				&& (map.hero.Level > monsterCurrent.Level || map.hero.FirstStrike == true);

			if (map.hero.ClassHero == Class.Assassin)
				firstStrike = firstStrike || (monsterCurrent.FirstStrike == false
					&& referee.VisibleAroundMonster(monsterCurrent));

			if (map.hero.ClassHero == Class.Assassin && map.hero.Level > monsterCurrent.Level)
			{
				infoStrike = localization.Get("RESULT_FIGHT_VICTORY");
				color = Color.Lime;
			}
			else if ((monsterCurrent.DeathGaze / 100) > (map.hero.HealthCurrent / map.hero.HealthMax))
			{
				infoStrike = localization.Get("RESULT_FIGHT_PETRIFICATION");
				color = Color.Magenta;
			}
			else if (monsterCurrent.GetDamageNext(true, map.hero) >= map.hero.HealthCurrent
				&& !(firstStrike && !liveMonster))
			{
				infoStrike = localization.Get("RESULT_FIGHT_DEATH");
				color = Color.Red;
			}
			else if (monsterCurrent.PoisonStrike && !(firstStrike && !liveMonster))
			{
				infoStrike = localization.Get("RESULT_FIGHT_POISON");
				color = Color.Magenta;
			}
			else if (monsterCurrent.ManaBurn && !(firstStrike && !liveMonster))
			{
				infoStrike = localization.Get("RESULT_FIGHT_MANA_BURN");
				color = Color.Magenta;
			}
			else if (!liveMonster)
			{
				infoStrike = localization.Get("RESULT_FIGHT_VICTORY");
				color = Color.Lime;
			}

			spriteBatch.DrawString(ContentPack.Font, infoStrike,
				new Vector2(450, 220), color);
		}

		/// <summary>
		/// Draw game user interface
		/// </summary>
		/// <param name="spriteBatch">SpriteBatch</param>
		private void DrawGame(SpriteBatch spriteBatch)
		{
			map.Draw(spriteBatch);

			DrawFon(spriteBatch, new Rectangle(400, 0, Constants.WINDOW_NORMAL_SIZE_WIDTH - 1 - 400,
				Constants.WINDOW_NORMAL_SIZE_HEIGHT - 1));

			//class and race
			DrawStringInRectangle(spriteBatch,
				localization.Get("CLASS_" + ((int)HeroesClass + 1)) + " " 
				+ localization.Get("RACE_" + ((int)HeroesRace + 1)),
				new Rectangle(400, 0, 160, 18), Color.Red);

			//avatar
			Rectangle rectOfAvater = new Rectangle(400, 18, 80, 80);
			spriteBatch.Draw(ContentPack.PointTexture, rectOfAvater, Color.Black);
			spriteBatch.Draw(ContentPack.HeroTexture[(int)HeroesClass], rectOfAvater, Color.White);

			////gold
			//Rectangle rectOfGold = new Rectangle(485, 23, 20, 20);
			//spriteBatch.Draw(ContentPack.pointTexture, rectOfGold, Color.Black);
			//DrawSquare(spriteBatch, rectOfGold, 1, Color.DimGray);
			//spriteBatch.Draw(ContentPack.tileTexture[(int)TileType.Gold], rectOfGold, Color.White);
			//spriteBatch.DrawString(ContentPack.font, map.saveData.Gold.ToString(), new Vector2(508, 23), Color.White);

			//hpPoison
			spriteBatch.Draw(ContentPack.PointTexture, buttonPotionHealth.Rectangle, Color.Black);
			DrawSquare(spriteBatch, buttonPotionHealth.Rectangle, 1, Color.White);
			spriteBatch.Draw(ContentPack.TileTexture[(int)TileType.HealthPotion], 
				buttonPotionHealth.Rectangle, Color.White);
			spriteBatch.DrawString(ContentPack.Font, map.hero.HealthPotion.ToString(),
				new Vector2(508, 50), Color.White);

			//mpPoison
			spriteBatch.Draw(ContentPack.PointTexture, buttonPotionMana.Rectangle, Color.Black);
			DrawSquare(spriteBatch, buttonPotionMana.Rectangle, 1, Color.White);
			spriteBatch.Draw(ContentPack.TileTexture[(int)TileType.ManaPotion], 
				buttonPotionMana.Rectangle, Color.White);
			spriteBatch.DrawString(ContentPack.Font, map.hero.ManaPotion.ToString(),
				new Vector2(508, 77), Color.White);

			//damage
			spriteBatch.Draw(ContentPack.TileTexture[(int)buttonAttack.Parameter], 
				buttonAttack.Rectangle, Color.White);
			Color colorDamage = Color.White;
			int damage = map.hero.GetDamageNext(true, null);
			if (map.hero.DamageBase < damage)
				colorDamage = Color.Lime;
			else if (map.hero.DamageBase > damage)
				colorDamage = Color.Red;
			spriteBatch.DrawString(ContentPack.Font, damage + "", new Vector2(428, 101), colorDamage);

			//experience
			Rectangle rectOfExperience = new Rectangle(455, 103, 101, 17);
			spriteBatch.Draw(ContentPack.PointTexture, rectOfExperience, Color.Black);
			rectOfExperience.Width = 101 * map.hero.Experience / map.hero.ExperienceNextLevel;
			spriteBatch.Draw(ContentPack.PointTexture, rectOfExperience, Color.DarkGreen);
			spriteBatch.DrawString(ContentPack.Font,
				localization.Get("HERO_LEVEL") + " " + map.hero.Level + " (" 
				+ map.hero.Experience + "/" + map.hero.ExperienceNextLevel + ")",
				new Vector2(458, 101), Color.White);

			//health
			spriteBatch.Draw(ContentPack.TileTexture[(int)TileType.HPBoost],
				new Vector2(405, 124), Color.White);

			Rectangle rectOfHealth = new Rectangle(425, 126, 131, 17);
			spriteBatch.Draw(ContentPack.PointTexture, rectOfHealth, Color.Black);
			rectOfHealth.Width = 131 * map.hero.HealthCurrent / map.hero.HealthMax;
			spriteBatch.Draw(ContentPack.PointTexture, rectOfHealth,
				map.hero.Poisoned ? Color.Purple : Color.Red);

			//damage for hero
			if (monsterCurrent != null)
			{
				int damageToMonster = map.hero.GetDamageNext(true, monsterCurrent);
				bool liveMonster = monsterCurrent.HealthCurrent > damageToMonster;
				bool firstStrike = monsterCurrent.FirstStrike == false
					&& (map.hero.Level > monsterCurrent.Level || map.hero.FirstStrike == true);

				if (map.hero.ClassHero == Class.Assassin)
					firstStrike = firstStrike || (monsterCurrent.FirstStrike == false
						&& referee.VisibleAroundMonster(monsterCurrent));
				
				if (!firstStrike || liveMonster 
					|| (map.hero.ClassHero == Class.Assassin && map.hero.Level > monsterCurrent.Level))
				{
					int healthHeroAfterStrikeX = 131 * 
						(map.hero.HealthCurrent - monsterCurrent.GetDamageNext(true, map.hero)) / map.hero.HealthMax;
					if (map.hero.HealthCurrent - monsterCurrent.GetDamageNext(true, map.hero) < 0)
						healthHeroAfterStrikeX = 0;
					spriteBatch.Draw(ContentPack.PointTexture,
						new Rectangle(rectOfHealth.X + healthHeroAfterStrikeX, rectOfHealth.Y, 
							rectOfHealth.Width - healthHeroAfterStrikeX, rectOfHealth.Height), Color.DarkRed);
				}
			}
			spriteBatch.DrawString(ContentPack.Font,
				map.hero.HealthCurrent + "/" + map.hero.HealthMax, new Vector2(428, 124), Color.White);

			//mana
			spriteBatch.Draw(ContentPack.TileTexture[(int)TileType.MPBoost], 
				new Vector2(405, 147), Color.White);
			Rectangle rectOfMana = new Rectangle(425, 149, 131, 17);
			spriteBatch.Draw(ContentPack.PointTexture, rectOfMana, Color.Black);
			rectOfMana.Width = 131 * map.hero.ManaCurrent / map.hero.ManaMax;
			spriteBatch.Draw(ContentPack.PointTexture, rectOfMana, Color.Blue);
			spriteBatch.DrawString(ContentPack.Font,
				map.hero.ManaCurrent + "/" + map.hero.ManaMax, new Vector2(428, 147), Color.White);

			//magic
			// 4 slots
			if (map.hero.ClassHero == Class.Wizard)
			{
				for (int i = 0; i < 4; ++i)
				{
					spriteBatch.Draw(ContentPack.MagicTexture[(int)map.hero.Magics[i]],
						buttonMagic[i].Rectangle, Color.White);
					if (map.hero.Magics[i] != MagicType.Empty
						&& map.hero.CostMagic(map.hero.Magics[i]) <= map.hero.ManaCurrent)
						spriteBatch.Draw(ContentPack.MagicSelectTexture, buttonMagic[i].Rectangle, Color.White);
				}

			}
			else
			{
				for (int i = 0; i < 3; ++i)
				{
					spriteBatch.Draw(ContentPack.MagicTexture[(int)map.hero.Magics[i]],
						buttonMagic[i].Rectangle, Color.White);
					if (map.hero.Magics[i] != MagicType.Empty
						&& map.hero.CostMagic(map.hero.Magics[i]) <= map.hero.ManaCurrent)
						spriteBatch.Draw(ContentPack.MagicSelectTexture, buttonMagic[i].Rectangle, Color.White);
				}
			}
			spriteBatch.Draw(ContentPack.MagicTexture[(int)MagicType.Converter], buttonMagic[4].Rectangle, Color.White);

			if (CurrentMagicIndex >= 0)
				spriteBatch.Draw(ContentPack.MagicSelectTexture, buttonMagic[CurrentMagicIndex].Rectangle, Color.Lime);

			//window help
			Rectangle rectOfInfoArea = new Rectangle(401, 201, 159, 180);
			spriteBatch.Draw(ContentPack.PointTexture, rectOfInfoArea, Color.Black);
			DrawSquare(spriteBatch, rectOfInfoArea, 1, Color.White);

			MagicTile currentMagicTile = null;
			buttonInfo = null;

			if (monsterCurrent != null)
			{
				//avatar monster
				spriteBatch.Draw(ContentPack.MonsterTexture[(int)monsterCurrent.MonsterType],
					new Rectangle(405, 205, 40, 40), Color.White);

				//monster type
				spriteBatch.DrawString(ContentPack.Font,
					localization.Get("MONSTER_" + ((int)monsterCurrent.MonsterType + 1)),
					new Vector2(450, 205), Color.White);

				//result of strike
				DrawResultStrikeStringOnInfoArea(spriteBatch);

				//damage monster
				spriteBatch.Draw(ContentPack.TileTexture[(int)TileType.Attackboost],
					new Rectangle(405, 253, 20, 20), Color.White);
				spriteBatch.DrawString(ContentPack.Font, monsterCurrent.GetDamageNext(true, map.hero) + "",
					new Vector2(428, 253), Color.White);

				//health monster
				spriteBatch.Draw(ContentPack.TileTexture[(int)TileType.HPBoost],
					new Rectangle(405, 276, 20, 20), Color.White);

				Rectangle rectOfMonsterHealth = new Rectangle(425, 278, 131, 17);
				rectOfMonsterHealth.Width = 131 * monsterCurrent.HealthCurrent / monsterCurrent.HealthMax;
				spriteBatch.Draw(ContentPack.PointTexture, rectOfMonsterHealth, 
					monsterCurrent.Poisoned ? Color.Purple : Color.Red);
				int monsterHealthAfterStrike = 131 * (monsterCurrent.HealthCurrent
					- map.hero.GetDamageNext(true, monsterCurrent))	/ monsterCurrent.HealthMax;

				if (map.hero.ClassHero == Class.Assassin && map.hero.Level > monsterCurrent.Level)
					monsterHealthAfterStrike = 0;

				if (monsterCurrent.HealthCurrent - map.hero.GetDamageNext(true, monsterCurrent) < 0)
					monsterHealthAfterStrike = 0;

				spriteBatch.Draw(ContentPack.PointTexture,
					new Rectangle(rectOfMonsterHealth.X + monsterHealthAfterStrike,
						rectOfMonsterHealth.Y, 
						rectOfMonsterHealth.Width - monsterHealthAfterStrike,
						rectOfMonsterHealth.Height),
					Color.DarkRed);

				spriteBatch.DrawString(ContentPack.Font,
					monsterCurrent.HealthCurrent + "/" + monsterCurrent.HealthMax, new Vector2(428, 276), Color.White);

				string infoOnMonster = localization.Get("MONSTER_INFO_" + ((int)monsterCurrent.MonsterType + 1));
				if (monsterCurrent.MonsterType == MonsterType.Gorgon)
					infoOnMonster += monsterCurrent.DeathGaze + "%)";
				spriteBatch.DrawString(ContentPack.Font, infoOnMonster,
					new Vector2(405, 300), Color.White);

			}
			//magic under hero
			else if (map.magicTileList.Any(p => p.Rectangle == map.hero.Rectangle))
			{
				foreach (MagicTile magicTile in map.magicTileList)
				{
					if (magicTile.Rectangle == map.hero.Rectangle)
						currentMagicTile = magicTile;
				}
				spriteBatch.Draw(ContentPack.MagicTexture[(int)currentMagicTile.Magic],
					new Rectangle(405, 205, 40, 40), Color.White);
				spriteBatch.DrawString(ContentPack.Font,
					localization.Get("MAGIC_" + (int)currentMagicTile.Magic),
					new Vector2(450, 205), Color.Lime);
				spriteBatch.DrawString(ContentPack.Font,
					localization.Get("MANA_COST_STRING") + ": " + map.hero.CostMagic(currentMagicTile.Magic),
					new Vector2(450, 220), Color.White);

				//pick up button
				buttonInfo = new ButtonInfo(new Rectangle(445, 253, 71, 26));
				buttonInfo.Text = localization.Get("BUTTON_INFO_MAGIC");
				buttonInfo.Draw(spriteBatch);

				//info
				spriteBatch.DrawString(ContentPack.Font,
					localization.Get("MAGIC_INFO_" + (int)currentMagicTile.Magic),
					new Vector2(405, 280), Color.White);
			}
			else if (buttonBonusCurrent != null)
			{
				spriteBatch.DrawString(ContentPack.Font,
					localization.Get("DAMAGE_BASE_STRING") + ": " + map.hero.DamageBase,
					new Vector2(405, 205), Color.White);

				Color color = Color.White;
				if (map.hero.DamageBonus < 0)
					color = Color.Red;
				else if (map.hero.DamageBonus > 0)
					color = Color.Lime;
				int damageBonus = map.hero.DamageBonus + map.hero.DamageNextBonus;
				spriteBatch.DrawString(ContentPack.Font,
					localization.Get("DAMAGE_BONUS_STRING") + ": " + damageBonus + "%",
					new Vector2(405, 225), color);

				spriteBatch.DrawString(ContentPack.Font,
					localization.Get("DAMAGE_TOTAL_STRING") + ": " + map.hero.GetDamageNext(true, null),
					new Vector2(405, 245), Color.White);
			}
			else if (buttonMagicCurrent != null && buttonMagicCurrent.Parameter != MagicType.Empty)
			{
				spriteBatch.Draw(ContentPack.MagicTexture[(int)buttonMagicCurrent.Parameter],
					new Rectangle(405, 205, 40, 40), Color.White);
				spriteBatch.DrawString(ContentPack.Font,
					localization.Get("MAGIC_" + (int)buttonMagicCurrent.Parameter),
					new Vector2(450, 205), Color.Lime);
				spriteBatch.DrawString(ContentPack.Font,
					localization.Get("MANA_COST_STRING") + ": " + map.hero.CostMagic(buttonMagicCurrent.Parameter),
					new Vector2(450, 220), Color.White);

				if (buttonMagicCurrent.Parameter == MagicType.Fireball)
					spriteBatch.DrawString(ContentPack.Font, 
						localization.Get("DAMAGE_FIREBALL_STRING") + ": " 
							+ Constants.DAMAGE_FIREBALL_PER_LEVEL * map.hero.Level,
						new Vector2(405, 250), Color.White);

				//info
				spriteBatch.DrawString(ContentPack.Font,
					localization.Get("MAGIC_INFO_" + (int)buttonMagicCurrent.Parameter),
					new Vector2(405, 280), Color.White);
			}
			else
			{
				string info = "";
				if (map.hero.FirstStrike)
					info += localization.Get("MAGIC_" + (int)MagicType.FirstStrike) + '\n';
				if (map.hero.KillProtect)
					info += localization.Get("MAGIC_" + (int)MagicType.KillProtect) + '\n';
				if (map.hero.Might)
					info += localization.Get("MAGIC_" + (int)MagicType.Might) + '\n';
				spriteBatch.DrawString(ContentPack.Font, info,
					new Vector2(405, 205), Color.White);
			}

			buttonExit.Draw(spriteBatch);
		}

		/// <summary>
		/// Draw about user interface
		/// </summary>
		/// <param name="spriteBatch">SpriteBatch</param>
		private void DrawAbout(SpriteBatch spriteBatch)
		{
			spriteBatch.DrawString(ContentPack.Font, localization.Get("ABOUT_INFORMATION"), 
				new Vector2(20, 20), Color.White);
			buttonExit.Draw(spriteBatch);
		}

		/// <summary>
		/// Draw score user interface
		/// </summary>
		/// <param name="spriteBatch">SpriteBatch</param>
		private void DrawScore(SpriteBatch spriteBatch)
		{
			int YNextString = 20;
			bool win = !map.monsterList.Any(p => p.Level == 10);
			string winString = "";
			Color winColor;
			if (win)
			{
				winColor = Color.Lime;
				winString = localization.Get("SCORE_VICTORY_STRING");
			}
			else
			{
				winColor = Color.Red;
				winString = localization.Get("SCORE_DEFEAT_STRING");
			}
			spriteBatch.DrawString(ContentPack.Font, winString, new Vector2(20, YNextString), winColor);
			YNextString += 20;

			//lvl
			spriteBatch.DrawString(ContentPack.Font, localization.Get("SCOPE_LEVEL_STRING") + ": " + map.hero.Level,
				new Vector2(20, YNextString), Color.White);
			YNextString += 20;

			//exp
			spriteBatch.DrawString(ContentPack.Font, 
				localization.Get("SCOPE_EXP_STRING") + ": " + (map.hero.Experience + (map.hero.Level - 1) * 5),
				new Vector2(20, YNextString), Color.White);
			YNextString += 20;

			//count kill monsters
			spriteBatch.DrawString(ContentPack.Font,
				localization.Get("SCOPE_COUNT_DEAD_MONSTERS") + ": " + (39 - map.monsterList.Count) + "/39",
				new Vector2(20, YNextString), Color.White);
			YNextString += 20;

			//unlook
			YNextString += 20;

			bool winNewRace = win && !map.saveData._ListWinRace.Any(p => p == map.hero.RaceHero);
			
			if (winNewRace)
			{
				switch (map.saveData._ListWinRace.Count)
				{
					case 0:
						spriteBatch.DrawString(ContentPack.Font,
							localization.Get("SCOPE_OPEN_NEW_RACE") + ": " + localization.Get("RACE_5"),
							new Vector2(20, YNextString), Color.White);
						YNextString += 20;
						break;
					case 1:
						spriteBatch.DrawString(ContentPack.Font,
							localization.Get("SCOPE_OPEN_NEW_RACE") + ": " + localization.Get("RACE_6"),
							new Vector2(20, YNextString), Color.White);
						YNextString += 20;
						break;
					case 2:
						spriteBatch.DrawString(ContentPack.Font,
							localization.Get("SCOPE_OPEN_NEW_RACE") + ": " + localization.Get("RACE_7"),
							new Vector2(20, YNextString), Color.White);
						YNextString += 20;
						break;
				}
			}

			bool winNewClass = win && !map.saveData._ListWinClass.Any(p => p == map.hero.ClassHero);
			int indexNewClass = -1;

			if (winNewClass)
			{
				switch (map.hero.ClassHero)
				{
					case Class.Fighter:
						indexNewClass = (int)Class.Berserker;
						break;
					case Class.Berserker:
						indexNewClass = (int)Class.Warlord;
						break;
					case Class.Warlord:
						indexNewClass = (int)Class.Crusader;
						break;
					case Class.Thief:
						indexNewClass = (int)Class.Rogue;
						break;
					case Class.Rogue:
						indexNewClass = (int)Class.Assassin;
						break;
					case Class.Priest:
						indexNewClass = (int)Class.Monk;
						break;
					case Class.Monk:
						indexNewClass = (int)Class.Paladin;
						break;
					case Class.Wizard:
						indexNewClass = (int)Class.Sorcerer;
						break;
					case Class.Sorcerer:
						indexNewClass = (int)Class.Bloodmage;
						break;
					case Class.Bloodmage:
						indexNewClass = (int)Class.Transmuter;
						break;
				}
				if (indexNewClass != -1)
				{
					spriteBatch.DrawString(ContentPack.Font,
						localization.Get("SCOPE_OPEN_NEW_CLASS") + ": " + localization.Get("CLASS_" + (indexNewClass + 1)),
						new Vector2(20, YNextString), Color.White);
					YNextString += 20;
				}

				//unlook monster
				int indexNewMonster = -1;
				if (winNewClass)
				{
					switch (map.hero.ClassHero)
					{
						case Class.Rogue:
							indexNewMonster = (int)MonsterType.Bandit;
							break;
						case Class.Sorcerer:
							indexNewMonster = (int)MonsterType.Golem;
							break;
						case Class.Thief:
							indexNewMonster = (int)MonsterType.Gorgon;
							break;
						case Class.Monk:
							indexNewMonster = (int)MonsterType.Dragon;
							break;
						case Class.Priest:
							indexNewMonster = (int)MonsterType.Serpent;
							break;
						case Class.Wizard:
							indexNewMonster = (int)MonsterType.Goat;
							break;
						case Class.Fighter:
							indexNewMonster = (int)MonsterType.Wraith;
							break;
						case Class.Berserker:
							indexNewMonster = (int)MonsterType.Goo;
							break;
					}

					if (indexNewMonster != -1)
					{
						spriteBatch.DrawString(ContentPack.Font,
							localization.Get("SCOPE_OPEN_NEW_MONSTER") + ": " + localization.Get("MONSTER_" + (indexNewMonster + 1)),
							new Vector2(20, YNextString), Color.White);
						YNextString += 20;
					}
				}

			}

			buttonExit.Draw(spriteBatch);
		}

		/// <summary>
		/// Draw line around rectangle
		/// </summary>
		/// <param name="spriteBatch">SpriteBatch</param>
		/// <param name="parRect">Rectangle</param>
		/// <param name="parThickness">Thickness of line</param>
		/// <param name="parColor">Color line</param>
		private void DrawSquare(SpriteBatch spriteBatch, Rectangle parRect, int parThickness, Color parColor)
		{
			Rectangle rect = new Rectangle();
			rect.X = parRect.X;
			rect.Y = parRect.Y;
			rect.Width = parRect.Width;
			rect.Height = parThickness;
			spriteBatch.Draw(ContentPack.PointTexture, rect, parColor);

			rect.X = parRect.Right - parThickness;
			rect.Y = parRect.Y;
			rect.Width = parThickness;
			rect.Height = parRect.Height;
			spriteBatch.Draw(ContentPack.PointTexture, rect, parColor);

			rect.X = parRect.X;
			rect.Y = parRect.Bottom - parThickness;
			rect.Width = parRect.Width;
			rect.Height = parThickness;
			spriteBatch.Draw(ContentPack.PointTexture, rect, parColor);

			rect.X = parRect.X;
			rect.Y = parRect.Y;
			rect.Width = parThickness;
			rect.Height = parRect.Height;
			spriteBatch.Draw(ContentPack.PointTexture, rect, parColor);
		}

		/// <summary>
		/// Mouse click
		/// </summary>
		/// <param name="X">Mouse X in window</param>
		/// <param name="Y">Mouse Y in window</param>
		private void MouseClick(int X, int Y)
		{
			switch (map.GameState)
			{
				case GameState.Menu:
					#region GameState.Menu
					//Races
					foreach (ButtonMenu<Race> button in buttonRaces)
					{
						if (button.Rectangle.Contains(X, Y) && button.Invisible == false)
						{
							foreach (ButtonMenu<Race> buttonUnmark in buttonRaces)
							{
								buttonUnmark.Selected = false;
							}
							button.Selected = true;
							HeroesRace = button.Parameter;
						}
					}

					//Classes
					foreach (ButtonMenu<Class> button in buttonClass)
					{
						if (button.Rectangle.Contains(X, Y) && button.Invisible == false)
						{
							foreach (ButtonMenu<Class> buttonUnmark in buttonClass)
							{
								buttonUnmark.Selected = false;
							}
							button.Selected = true;
							HeroesClass = button.Parameter;
						}
					}

					//Others
					if (buttonStart.Rectangle.Contains(X, Y))
					{
						buttonExit.Text = localization.Get("BUTTON_RETIRE");
						map.CreateMap(HeroesRace, HeroesClass);
						InitializeGame();
						referee.ChangeGameState(GameState.Game);
					}

					if (buttonAbout.Rectangle.Contains(X, Y))
						referee.ChangeGameState(GameState.About);

					if (buttonExit.Rectangle.Contains(X, Y))
						game.Exit();
					#endregion
					break;

				case GameState.Game:
					#region GameState.Game

					if (X >= 0 && Y >= 0 && X < 400 && Y < 400
						&& map.visible[X / Constants.TEXTURE_SIZE, Y / Constants.TEXTURE_SIZE] != 0)
					{
						if (map.map[X / Constants.TEXTURE_SIZE, Y / Constants.TEXTURE_SIZE] 
							== (int)MapItem.None)
						{
							if (map.hero.ClassHero == Class.Bloodmage)
							{
								for (int i = 0; i < map.bloodList.Count; ++i)
								{
									if (map.bloodList[i].Rectangle.Contains(X, Y))
									{
										map.hero.HealthCurrent += map.hero.HealthMax * 15 / 100;
										map.bloodList.RemoveAt(i);
										--i;
										continue;
									}
								}
							}
							map.ClickHeroMove(X / Constants.TEXTURE_SIZE, Y / Constants.TEXTURE_SIZE);
						}
						else if (map.map[X / Constants.TEXTURE_SIZE, Y / Constants.TEXTURE_SIZE]
							== (int)MapItem.Monster)
						{
							if (map.ClickHeroMoveToMonster(X / Constants.TEXTURE_SIZE, Y / Constants.TEXTURE_SIZE))
							{
								foreach (Monster monster in map.monsterList)
								{
									monster.Poisoned = false;
								}
								if (monsterCurrent != null)
									referee.Fight(map.hero, monsterCurrent);
							}
						}
					}


					if (buttonExit.Rectangle.Contains(X, Y))
						referee.ChangeGameState(GameState.Score);
					if (buttonInfo != null && buttonInfo.Rectangle.Contains(X, Y))
						referee.AddMagicToHeroSlot();
					if (buttonPotionHealth.Rectangle.Contains(X, Y))
						map.hero.ApplyPotion(TileType.HealthPotion);
					if (buttonPotionMana.Rectangle.Contains(X, Y))
						map.hero.ApplyPotion(TileType.ManaPotion);

					CurrentMagicIndex = -1;
					for (int i = 0; i < buttonMagic.Length; ++i)
					{
						if (map.hero.Magics[i] != MagicType.Empty && buttonMagic[i].Rectangle.Contains(X, Y))
						{
							if (map.hero.ManaCurrent >= map.hero.CostMagic(map.hero.Magics[i]))
								CurrentMagicIndex = i;
							else
								break;

							if (referee.ApplyMagic(map.hero.Magics[CurrentMagicIndex], -1, -1))
								CurrentMagicIndex = -1;
							break;
						}
					}

					#endregion
					break;

				case GameState.Score:
					#region GameState.Score

					if (buttonExit.Rectangle.Contains(X, Y))
					{
						buttonExit.Text = localization.Get("BUTTON_EXIT");
						map.UpdateSaveData();
						InitializeMenu();

						referee.ChangeGameState(GameState.Menu);
					}
					#endregion
					break;

				case GameState.About:
					#region GameState.About
					if (buttonExit.Rectangle.Contains(X, Y))
						referee.ChangeGameState(GameState.Menu);
					#endregion
					break;
			}
		}
	}
}
