using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DesktopDungeons.TileClasses
{
	/// <summary>
	/// Unit on map
	/// </summary>
	public abstract class Unit : Tile
	{
		#region Variables
		/// <summary>
		/// Max health points
		/// </summary>
		public int HealthMax { get; set; }

		/// <summary>
		/// Max mana points
		/// </summary>
		public int ManaMax { get; set; }

		/// <summary>
		/// Unit is poisoned
		/// </summary>
		public bool Poisoned { get; set; }

		/// <summary>
		/// Unit is burned of mana
		/// </summary>
		public bool ManaBurned { get; set; }

		/// <summary>
		/// Base damage
		/// </summary>
		public int DamageBase { get; set; }

		/// <summary>
		/// Unit is immune to poison
		/// </summary>
		public bool ImmunePoison { get; set; }

		/// <summary>
		/// First strike
		/// </summary>
		public bool FirstStrike { get; set; }

		/// <summary>
		/// Unit have poison strike
		/// </summary>
		public bool PoisonStrike { get; set; }

		/// <summary>
		/// Unit have mana burn strike
		/// </summary>
		public bool ManaBurn { get; set; }

		/// <summary>
		/// Attack of unit is magic
		/// </summary>
		public bool MagicAttack { get; protected set; }

		/// <summary>
		/// Percent of DeathGaze
		/// </summary>
		public int DeathGaze { get; protected set; }

		/// <summary>
		/// Unit is undead
		/// </summary>
		public bool Undead { get; protected set; }

		/// <summary>
		/// Percent of Dodge from attack
		/// </summary>
		public int DodgeRate { get; set; }

		/// <summary>
		/// Resist of physical damage
		/// </summary>
		public int ResistPhysical { get; set; }

		/// <summary>
		/// Resist of magic damage
		/// </summary>
		public int ResistMagic { get; set; }

		/// <summary>
		/// Current level of unit
		/// </summary>
		public int Level { get; protected set; }

		/// <summary>
		/// Current health points
		/// </summary>
		private int _HealthCurrent;

		/// <summary>
		/// Current mana points
		/// </summary>
		private int _ManaCurrent;

		#region Properties
		/// <summary>
		/// Current health points
		/// </summary>
		public int HealthCurrent
		{
			get
			{
				return _HealthCurrent;
			}
			set
			{
				if (value > HealthMax)
					_HealthCurrent = HealthMax;
				else if (value < 0)
					_HealthCurrent = 0;
				else
					_HealthCurrent = value;
			}
		}

		/// <summary>
		/// Current mana points
		/// </summary>
		public int ManaCurrent
		{
			get
			{
				return _ManaCurrent;
			}
			set
			{
				if (value > ManaMax)
					_ManaCurrent = ManaMax;
				else if (value < 0)
					_ManaCurrent = 0;
				else
					_ManaCurrent = value;
			}
		}

		#endregion
		#endregion

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="X">X on game</param>
		/// <param name="Y">Y on game</param>
		/// <param name="parTexture">Texture</param>
		public Unit(int X, int Y, Texture2D parTexture)
			: base(X, Y, parTexture)
		{
			Level = 1;
		}

		/// <summary>
		/// Draw unit with level indicator
		/// </summary>
		/// <param name="spriteBatch">SpriteBatch</param>
		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
			spriteBatch.Draw(ContentPack.NumberTexture,
				new Rectangle(Rectangle.Left, Rectangle.Bottom - ContentPack.NumberTexture.Height,
					ContentPack.NumberTexture.Width / 10, ContentPack.NumberTexture.Height),
				new Rectangle(9 * (Level - 1), 0,
					ContentPack.NumberTexture.Width / 10, ContentPack.NumberTexture.Height),
				Color.White);
		}

		/// <summary>
		/// Calc damage to current monster
		/// </summary>
		/// <param name="info">Is for information</param>
		/// <param name="parMonster">Monster</param>
		/// <returns>Points of damage</returns>
		public virtual int GetDamageNext(bool info, Unit parMonster)
		{
			if (parMonster != null)
				return DamageBase * (100 - 
					(MagicAttack ? parMonster.ResistMagic : parMonster.ResistPhysical)) / 100;;
			return DamageBase;
		}

		/// <summary>
		/// if not dodge then to damage this object
		/// </summary>
		/// <param name="parDamage">Points of damage</param>
		/// <param name="isMagicDamage">Is magic damage</param>
		public virtual void SetDamage(int parDamage, bool isMagicDamage)
		{
			Random rand = new Random();
			if (rand.Next(100) < DodgeRate)
				return;

			HealthCurrent -= parDamage;
		}
	}
}
