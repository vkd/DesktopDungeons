using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DesktopDungeons
{
	/// <summary>
	/// Button of Magic slots
	/// </summary>
	public class ButtonMagic : Button<MagicType>
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="parRect">Size of button</param>
		/// <param name="parParam">Type of magic</param>
		public ButtonMagic(Rectangle parRect, MagicType parParam)
			: base(parRect, parParam)
		{

		}

		/// <summary>
		/// Draw button
		/// </summary>
		/// <param name="spriteBatch">SpriteBatch</param>
		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(ContentPack.MagicTexture[(int)Parameter], Rectangle, Color.White);
		}
	}
}
