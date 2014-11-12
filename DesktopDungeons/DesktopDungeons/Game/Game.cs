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
using LoadLocalization;

namespace DesktopDungeons
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game : Microsoft.Xna.Framework.Game
	{
		/// <summary>
		/// Scale of window
		/// </summary>
		public static float ScaleWindow = 1.0f;

		/// <summary>
		/// Graphics device manager
		/// </summary>
		private GraphicsDeviceManager graphics;

		/// <summary>
		/// SpriteBatch
		/// </summary>
		private SpriteBatch spriteBatch;
		
		/// <summary>
		/// All text string in game
		/// </summary>
		private Localization localization;

		/// <summary>
		/// Input and output
		/// </summary>
		private IO io;

		/// <summary>
		/// Referee of game
		/// </summary>
		private Referee referee;

		/// <summary>
		/// Map of game
		/// </summary>
		private Map map;

		/// <summary>
		/// Constructor
		/// </summary>
		public Game()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			graphics.PreferredBackBufferHeight = Constants.WINDOW_NORMAL_SIZE_HEIGHT;
			graphics.PreferredBackBufferWidth = Constants.WINDOW_NORMAL_SIZE_WIDTH;

			localization = new Localization(Constants.LANGUAGE);

			map = new Map();
			referee = new Referee(map);
			io = new IO(this, map, referee, localization);
		}

		/// <summary>
		/// Set double size of window
		/// </summary>
		public void DoubleScreen()
		{
			//if normal size then set double size
			if (graphics.PreferredBackBufferWidth == Constants.WINDOW_NORMAL_SIZE_WIDTH
				&& graphics.PreferredBackBufferHeight == Constants.WINDOW_NORMAL_SIZE_HEIGHT)
			{
				graphics.PreferredBackBufferWidth = Constants.WINDOW_NORMAL_SIZE_WIDTH * 2;
				graphics.PreferredBackBufferHeight = Constants.WINDOW_NORMAL_SIZE_HEIGHT * 2;
				graphics.ApplyChanges();
			}
			//else set normal size
			else
			{
				graphics.PreferredBackBufferWidth = Constants.WINDOW_NORMAL_SIZE_WIDTH;
				graphics.PreferredBackBufferHeight = Constants.WINDOW_NORMAL_SIZE_HEIGHT;
				graphics.IsFullScreen = false;
				graphics.ApplyChanges();
			}
		}

		/// <summary>
		/// Set full screen
		/// </summary>
		public void ChangeFullScreen()
		{
			if (graphics.IsFullScreen)
			{
				graphics.PreferredBackBufferWidth = Constants.WINDOW_NORMAL_SIZE_WIDTH;
				graphics.PreferredBackBufferHeight = Constants.WINDOW_NORMAL_SIZE_HEIGHT;
				graphics.IsFullScreen = false;
				graphics.ApplyChanges();
			}
			else
			{
				graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
				graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
				graphics.IsFullScreen = true;
				graphics.ApplyChanges();
			}
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			this.IsMouseVisible = true;

			this.Window.AllowUserResizing = true;
			this.Window.Title = localization.Get("NAME_GAME");

			map.LoadSaveData();

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			ContentPack.Font = Content.Load<SpriteFont>("Fonts/ComicSans");

			ContentPack.WallTexture = Content.Load<Texture2D>("Tiles/Wall");
			ContentPack.DirtTexture = Content.Load<Texture2D>("Tiles/Dirt_crypt");
			ContentPack.PointTexture = Content.Load<Texture2D>("WhitePoint");
			ContentPack.NumberTexture = Content.Load<Texture2D>("NumberTexture");
			ContentPack.FonTexture = Content.Load<Texture2D>("Tiles/Wall2");

			ContentPack.HeroTexture = new Texture2D[Enum.GetValues(typeof(Class)).Length];
			for (int i = 0; i < Enum.GetValues(typeof(Class)).Length; ++i)
				ContentPack.HeroTexture[i] = Content.Load<Texture2D>("Tiles/Hero" + Enum.GetName(typeof(Class), (Class) i));

			ContentPack.MonsterTexture = new Texture2D[Enum.GetValues(typeof(MonsterType)).Length];
			for (int i = 0; i < Enum.GetValues(typeof(MonsterType)).Length; ++i)
				ContentPack.MonsterTexture[i] = Content.Load<Texture2D>("Tiles/" 
					+ Enum.GetName(typeof(MonsterType), (MonsterType)i));

			ContentPack.TileTexture = new Texture2D[Enum.GetValues(typeof(TileType)).Length];
			for (int i = 0; i < Enum.GetValues(typeof(TileType)).Length; ++i)
				ContentPack.TileTexture[i] = Content.Load<Texture2D>("Tiles/" + Enum.GetName(typeof(TileType), (TileType)i));

			ContentPack.MagicTexture = new Texture2D[Enum.GetValues(typeof(MagicType)).Length];
			for (int i = 0; i < Enum.GetValues(typeof(MagicType)).Length; ++i)
				ContentPack.MagicTexture[i] = Content.Load<Texture2D>("Tiles/G_" + Enum.GetName(typeof(MagicType), (MagicType)i));

			ContentPack.MagicGenericTexture = Content.Load<Texture2D>("Tiles/G_Generic");
			ContentPack.MagicSelectTexture = Content.Load<Texture2D>("Tiles/GlyphSelector");
			ContentPack.EnemyGeneric = Content.Load<Texture2D>("Tiles/EnemyGeneric");

			io.InitializeMenu();
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{

		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			io.Update(gameTime);

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			float scaleHeight = 1.0f;
			float scaleWidth = 1.0f;
			Matrix spriteScale;

			scaleHeight = (float)Window.ClientBounds.Height /
				Constants.WINDOW_NORMAL_SIZE_HEIGHT;
			scaleWidth = (float)Window.ClientBounds.Width /
				Constants.WINDOW_NORMAL_SIZE_WIDTH;
			ScaleWindow = scaleHeight < scaleWidth ? scaleHeight : scaleWidth;

			spriteScale = Matrix.CreateScale(ScaleWindow, ScaleWindow, 1.0f);

			GraphicsDevice.Clear(Color.Black);


			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None,
				RasterizerState.CullCounterClockwise, null, spriteScale);

			io.Draw(spriteBatch);

			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
