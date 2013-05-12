using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace DesktopDungeons
{
	/// <summary>
	/// Pack of all textures
	/// </summary>
	public static class ContentPack
	{
		/// <summary>
		/// Font on game
		/// </summary>
		public static SpriteFont Font;

		/// <summary>
		/// Texture of wall on map
		/// </summary>
		public static Texture2D WallTexture;

		/// <summary>
		/// Texture of dirt on map
		/// </summary>
		public static Texture2D DirtTexture;

		/// <summary>
		/// Texture consists of one white point
		/// </summary>
		public static Texture2D PointTexture;

		/// <summary>
		/// Texture of level indicator
		/// </summary>
		public static Texture2D NumberTexture;

		/// <summary>
		/// Fon texture
		/// </summary>
		public static Texture2D FonTexture;

		/// <summary>
		/// Array textures of hero
		/// </summary>
		public static Texture2D[] HeroTexture;

		/// <summary>
		/// Array textures of monsters
		/// </summary>
		public static Texture2D[] MonsterTexture;

		/// <summary>
		/// Array textures of tiles
		/// </summary>
		public static Texture2D[] TileTexture;

		/// <summary>
		/// Array textures of magic tiles
		/// </summary>
		public static Texture2D[] MagicTexture;

		/// <summary>
		/// Texture of indefinite magic
		/// </summary>
		public static Texture2D MagicGenericTexture;

		/// <summary>
		/// Texture is selecter of magic tile
		/// </summary>
		public static Texture2D MagicSelectTexture;

		/// <summary>
		/// Texture of indefinite monster
		/// </summary>
		public static Texture2D EnemyGeneric;
	}
}
