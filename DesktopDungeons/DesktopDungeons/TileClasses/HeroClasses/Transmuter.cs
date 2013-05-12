using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace DesktopDungeons.TileClasses.HeroClasses
{
	/// <summary>
	/// Transmuter
	/// </summary>
	public class Transmuter : Hero
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="X">X on game</param>
		/// <param name="Y">Y on game</param>
		/// <param name="parTexture">Texture</param>
		/// <param name="parRace">Race</param>
		public Transmuter(int X, int Y, Texture2D parTexture, Race parRace)
			: base(X, Y, parTexture, parRace, Class.Transmuter)
		{

		}

		/// <summary>
		/// Calc cost magic
		/// </summary>
		/// <param name="parMagicType">Type of magic</param>
		/// <returns>Cost mana points</returns>
		public override int CostMagic(MagicType parMagicType)
		{
			if (parMagicType == MagicType.Endwall)
				return 1;
			return base.CostMagic(parMagicType);
		}

		/// <summary>
		/// Get health and mana points when hero is research invisible blocks of map
		/// </summary>
		/// <param name="countPoints">Count research blocks</param>
		public override void ResearchMap(int countPoints)
		{
			
		}

		/// <summary>
		/// Apply magic
		/// </summary>
		/// <param name="parMagicType">Type of magic</param>
		public override void ApplyMagic(MagicType parMagicType)
		{
			base.ApplyMagic(parMagicType);
			HealthCurrent += 2;
		}
	}
}
