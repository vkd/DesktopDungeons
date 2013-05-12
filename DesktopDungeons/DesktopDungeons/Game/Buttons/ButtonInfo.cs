using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DesktopDungeons
{
	/// <summary>
	/// Button with string in center
	/// </summary>
	public class ButtonInfo : Button<MagicType>
	{
		/// <summary>
		/// String in button
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="parRect">Size of button</param>
		public ButtonInfo(Rectangle parRect)
			: base(parRect, MagicType.Empty)
		{

		}

		/// <summary>
		/// Draw button
		/// </summary>
		/// <param name="spriteBatch">SpriteBatch</param>
		public void Draw(SpriteBatch spriteBatch)
		{
			//Draw white frame around black fon
			int sizeFrame = 1;
			spriteBatch.Draw(ContentPack.PointTexture, Rectangle, Color.White);
			spriteBatch.Draw(ContentPack.PointTexture,
				new Rectangle(Rectangle.X + sizeFrame, Rectangle.Y + sizeFrame,
					Rectangle.Width - 1 - sizeFrame, Rectangle.Height - 1 - sizeFrame),
				Color.Black);

			//Draw string in center button
			Vector2 vector = ContentPack.Font.MeasureString(Text);
			vector.X = Rectangle.X + (int)(Rectangle.Width - vector.X) / 2;
			vector.Y = Rectangle.Y + (int)(Rectangle.Height - vector.Y) / 2;
			spriteBatch.DrawString(ContentPack.Font, Text, vector, Color.White);
		}
	}
}
