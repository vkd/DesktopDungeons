using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace DesktopDungeons.TileClasses.HeroClasses
{
	/// <summary>
	/// Rogue
	/// </summary>
	public class Rogue : Hero
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="X">X on game</param>
		/// <param name="Y">Y on game</param>
		/// <param name="parTexture">Texture</param>
		/// <param name="parRace">Race</param>
		public Rogue(int X, int Y, Texture2D parTexture, Race parRace)
			: base(X, Y, parTexture, parRace, Class.Rogue)
		{
			FirstStrike = true;
			DodgeRate += 20;
			DamageBonus += 50;
			HealthMax += 5;
			HealthPerLevel += -5;
		}

		/// <summary>
		/// Check that hero attack is first
		/// </summary>
		/// <param name="parMonster">Monster</param>
		/// <returns>First attack</returns>
		public override bool CheckFirstStrike(Monster parMonster)
		{
			FirstStrike = true;
			bool first = base.CheckFirstStrike(parMonster);
			FirstStrike = true;
			return first;
		}

	}
}
