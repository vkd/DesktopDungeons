using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesktopDungeons
{
	/// <summary>
	/// Data to save on disk
	/// Is singleton
	/// </summary>
	[Serializable()]
	public class SaveData
	{
		//public int Gold { get; set; }

		/// <summary>
		/// List of race is win on last games
		/// </summary>
		public List<Race> _ListWinRace;

		/// <summary>
		/// List of class is win on last games
		/// </summary>
		public List<Class> _ListWinClass;

		/// <summary>
		/// Instance of this class
		/// </summary>
		private static SaveData _Instance;

		/// <summary>
		/// Constructor
		/// </summary>
		private SaveData()
		{
			_ListWinRace = new List<Race>();
			_ListWinClass = new List<Class>();
		}

		/// <summary>
		/// Get instance this class
		/// </summary>
		/// <returns>This class</returns>
		public static SaveData Instance()
		{
			if (_Instance == null)
				_Instance = new SaveData();
			return _Instance;
		}
	}
}
