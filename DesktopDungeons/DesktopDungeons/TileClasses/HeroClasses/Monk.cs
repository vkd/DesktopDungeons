using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace DesktopDungeons.TileClasses.HeroClasses
{
	/// <summary>
	/// Monk
	/// </summary>
	public class Monk : Hero
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="X">X on game</param>
		/// <param name="Y">Y on game</param>
		/// <param name="parTexture">Texture</param>
		/// <param name="parRace">Race</param>
		public Monk(int X, int Y, Texture2D parTexture, Race parRace)
			: base(X, Y, parTexture, parRace, Class.Monk)
		{
			ResistPhysical += 50;
			ResistMagic += 50;
			DamageBonus -= 50;
		}

		/// <summary>
		/// Get health and mana points when hero is research invisible blocks of map
		/// </summary>
		/// <param name="countPoints">Count research blocks</param>
		public override void ResearchMap(int countPoints)
		{
			if (!Blood)
			{
				if (!Poisoned)
					HealthCurrent += Level * countPoints * 2;
				if (!ManaBurned)
					ManaCurrent += countPoints;
			}
			else
			{
				if (!ManaBurned)
					ManaCurrent += countPoints * 2;
			}
		}
	}
}
