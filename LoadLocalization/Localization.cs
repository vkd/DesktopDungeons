using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace LoadLocalization
{
	/// <summary>
	/// All text strings of game in file
	/// </summary>
	public class Localization
	{
		/// <summary>
		/// Char is split
		/// </summary>
		const char SPLIT_CHAR = '='; //for example: ExitButton=Exit

		/// <summary>
		/// If string not found
		/// </summary>
		const string NOT_FOUNT = "**not_found**";

		/// <summary>
		/// Path to file
		/// </summary>
		const string PATH = "Languages/";

		/// <summary>
		/// Filename
		/// </summary>
		const string END_OF_FILENAME = ".strings.txt"; //for example: en.strings.txt

		/// <summary>
		/// All text strings of game
		/// </summary>
		Dictionary<string, string> _Strings;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="parLanguage">Current language</param>
		public Localization(string parLanguage)
		{
			System.IO.StreamReader streamReader;
			_Strings = new Dictionary<string,string>();

			try
			{
				streamReader = new System.IO.StreamReader(PATH + parLanguage + END_OF_FILENAME);
			}
			catch (System.IO.FileNotFoundException)
			{
				return;
			}

			string line;
			string[] keyValue;

			
			while((line = streamReader.ReadLine()) != null)
			{
				try
				{
					keyValue = line.Split(SPLIT_CHAR);
					keyValue[1] = keyValue[1].Replace("\\n", "\n");
					_Strings.Add(keyValue[0], keyValue[1]);
				}
				catch (ArgumentException)
				{

				}
			}		

			streamReader.Close();
		}

		/// <summary>
		/// Get string with index
		/// </summary>
		/// <param name="parIndex">Index of string</param>
		/// <returns>Text</returns>
		public string Get(string parIndex)
		{
			try
			{
				return _Strings[parIndex];
			}
			catch (KeyNotFoundException)
			{

			}
			return NOT_FOUNT;
		}
	}
}
