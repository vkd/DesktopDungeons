using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace DesktopDungeons.TileClasses.HeroClasses
{
	/// <summary>
	/// Fighter
	/// </summary>
	public class Fighter : Hero
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="X">X on game</param>
		/// <param name="Y">Y on game</param>
		/// <param name="parTexture">Texture</param>
		/// <param name="parRace">Race</param>
		public Fighter(int X, int Y, Texture2D parTexture, Race parRace)
			: base(X, Y, parTexture, parRace, Class.Fighter)
		{
			KillProtect = true;
		}

		/// <summary>
		/// Add experience and if need get next level
		/// </summary>
		/// <param name="parLevelMonster">Level of killed monster</param>
		/// <param name="monsterKill">Is killed monster</param>
		/// <param name="bonusExp">Bonus point of experience</param>
		public override void AddExperience(int parLevelMonster, bool monsterKill, int bonusExp)
		{
			if (monsterKill)
				base.AddExperience(parLevelMonster, monsterKill, bonusExp + 1);
			else
				base.AddExperience(parLevelMonster, monsterKill, bonusExp);
		}
	}
}
