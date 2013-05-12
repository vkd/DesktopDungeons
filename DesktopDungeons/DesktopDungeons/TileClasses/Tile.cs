using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DesktopDungeons.TileClasses
{
	/// <summary>
	/// Tile on map
	/// </summary>
	public class Tile
	{
		/// <summary>
		/// Rectangle
		/// </summary>
		public Rectangle Rectangle { get; protected set; }

		/// <summary>
		/// Texture
		/// </summary>
		Texture2D texture;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="X">X on game</param>
		/// <param name="Y">Y on game</param>
		/// <param name="parTexture">Texture</param>
		public Tile(int X, int Y, Texture2D parTexture)
		{
			texture = parTexture;
			Rectangle = new Rectangle(X, Y, texture.Width, texture.Height);
		}

		/// <summary>
		/// Move tile
		/// </summary>
		/// <param name="X">New X on game</param>
		/// <param name="Y">New Y on game</param>
		public void Move(int X, int Y)
		{
			Rectangle = new Rectangle(X, Y, Rectangle.Width, Rectangle.Height);
		}

		/// <summary>
		/// Draw tile
		/// </summary>
		/// <param name="spriteBatch"></param>
		public virtual void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(texture, Rectangle, Color.White);
		}
	}
}
