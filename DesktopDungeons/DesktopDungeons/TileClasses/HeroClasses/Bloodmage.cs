using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace DesktopDungeons.TileClasses.HeroClasses
{
	/// <summary>
	/// Bloodmage
	/// </summary>
	public class Bloodmage : Hero
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="X">X on game</param>
		/// <param name="Y">Y on game</param>
		/// <param name="parTexture">Texture</param>
		/// <param name="parRace">Race</param>
		public Bloodmage(int X, int Y, Texture2D parTexture, Race parRace)
			: base(X, Y, parTexture, parRace, Class.Bloodmage)
		{

		}

		/// <summary>
		/// Apply potion
		/// </summary>
		/// <param name="parPotionType">Type of potion</param>
		public override void ApplyPotion(TileType parPotionType)
		{
			switch (parPotionType)
			{
				case TileType.HealthPotion:
					if (HealthPotion <= 0)
						return;
					--HealthPotion;
					HealthCurrent += HealthMax * 40 / 100;
					break;
				case TileType.ManaPotion:
					if (ManaPotion <= 0)
						return;
					--ManaPotion;
					ManaCurrent = ManaMax;
					HealthCurrent -= Level * 6;
					break;
			}
		}
	}
}
