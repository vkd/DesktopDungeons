using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesktopDungeons
{
	/// <summary>
	/// Class containing all constants of game
	/// </summary>
	public static class Constants
	{
		/// <summary>
		/// Current language
		/// </summary>
		public const string LANGUAGE = "en";

		/// <summary>
		/// Normal size width of window
		/// </summary>
		public const int WINDOW_NORMAL_SIZE_WIDTH = 560;
		
		/// <summary>
		/// Normal size height of window
		/// </summary>
		public const int WINDOW_NORMAL_SIZE_HEIGHT = 400;

		/// <summary>
		/// Size of texture
		/// </summary>
		public const int TEXTURE_SIZE = 20;

		/// <summary>
		/// Size of game map
		/// </summary>
		public const int MAP_SIZE = 20;

		/// <summary>
		/// Filename of saves
		/// </summary>
		public const string SAVEDATA_FILENAME = "save";

		/// <summary>
		/// Damage of Fireball per level
		/// </summary>
		public const int DAMAGE_FIREBALL_PER_LEVEL = 4;

		/// <summary>
		/// Level of Boss monster
		/// </summary>
		public const int MONSTER_BOSS_LEVEL = 10;

		/// <summary>
		/// Count slots of magic if hero is wizard
		/// </summary>
		public const int COUNT_HERO_SLOTS_FOR_WIZARD = 4;

		/// <summary>
		/// Count slots of magic
		/// </summary>
		public const int COUNT_HERO_SLOTS_NORMAL = 3;

		/// <summary>
		/// Count block for reveal magic
		/// </summary>
		public const int COUNT_REVEAL_MAGIC_BLOCKS = 3;

		/// <summary>
		/// Count health for heal magic
		/// </summary>
		public const int COUNT_HEAL_HEALTH_POINT = 3;
	}
}
