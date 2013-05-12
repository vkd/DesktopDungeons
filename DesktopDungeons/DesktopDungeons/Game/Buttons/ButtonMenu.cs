using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DesktopDungeons
{
	/// <summary>
	/// Button of choice race and class
	/// </summary>
	/// <typeparam name="T">Type of button</typeparam>
	public class ButtonMenu<T> : Button<T>
	{
		#region Constants
		/// <summary>
		/// Color button when invisible
		/// </summary>
		private static Color COLOR_INVISIBLE = Color.Navy;

		/// <summary>
		/// Color button when marked
		/// </summary>
		private static Color COLOR_MARKED = Color.SkyBlue;

		/// <summary>
		/// Color normal button
		/// </summary>
		private static Color COLOR_NORMAL = Color.Blue;

		/// <summary>
		/// Color button when selected and not win
		/// </summary>
		private static Color COLOR_SELECTED_NOT_WIN = Color.Yellow;

		/// <summary>
		/// Color button when selected and win
		/// </summary>
		private static Color COLOR_SELECTED_WIN = new Color(255, 255, 140);

		/// <summary>
		/// Text on button when invisible
		/// </summary>
		private const string TEXT_INVISIBLE = "???";
		#endregion

		/// <summary>
		/// Win with this parameter
		/// </summary>
		public bool MarkedIsWin { get; set; }

		/// <summary>
		/// String in button
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Is invisible button
		/// </summary>
		public bool Invisible { get; set; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="parRect">Size of button</param>
		/// <param name="parText">Text in button</param>
		/// <param name="parParam">Parameter of button</param>
		public ButtonMenu(Rectangle parRect, string parText, T parParam) //pam param papam opilki =)
			: base(parRect, parParam)
		{
			MarkedIsWin = false;
			Text = parText;
		}

		/// <summary>
		/// Draw button
		/// </summary>
		/// <param name="spriteBatch">SpriteBatch</param>
		public void Draw(SpriteBatch spriteBatch)
		{
			Color color;
			if (Invisible)
				color = COLOR_INVISIBLE;
			else if (Selected)
			{
				if (MarkedIsWin)
					color = COLOR_SELECTED_WIN;
				else
					color = COLOR_SELECTED_NOT_WIN;
			}
			else if (MarkedIsWin)
				color = COLOR_MARKED;
			else
				color = COLOR_NORMAL;

			spriteBatch.Draw(ContentPack.PointTexture, Rectangle, color);

			string str = Text;
			if (Invisible)
				str = TEXT_INVISIBLE;
			Vector2 vector = ContentPack.Font.MeasureString(str);
			vector.X = Rectangle.X + (int)(Rectangle.Width - vector.X) / 2;
			vector.Y = Rectangle.Y + (int)(Rectangle.Height - vector.Y) / 2;
			spriteBatch.DrawString(ContentPack.Font, str, vector, Color.Black);
		}
	}
}
