using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DesktopDungeons
{
	/// <summary>
	/// Button with parameter, without Draw method
	/// </summary>
	/// <typeparam name="T">Type of parameter</typeparam>
	public class Button<T>
	{
		/// <summary>
		/// Size of button
		/// </summary>
		public Rectangle Rectangle { get; set; }

		/// <summary>
		/// Button is selected
		/// </summary>
		public bool Selected { get; set; }

		/// <summary>
		/// Parameter
		/// </summary>
		public T Parameter { get; set; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="parRect">Size of button</param>
		/// <param name="parParam">Parameter</param>
		public Button(Rectangle parRect, T parParam)
		{
			Rectangle = parRect;
			Parameter = parParam;
		}
	}
}
