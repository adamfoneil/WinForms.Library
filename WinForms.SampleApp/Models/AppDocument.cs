using System;

namespace WinForms.SampleApp.Models
{
	public enum BeltOptions
	{
		White,
		Gold,
		Green,
		Purple,
		Blue,
		Red,
		Brown,
		Black
	}

	/// <summary>
	/// This is just a bogus model class demonstrating various types for use with DocumentManager
	/// </summary>
	public class AppDocument
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public bool IsActive { get; set; }
		public BeltOptions Belt { get; set; } // combobox from enum
		public DateTime EffectiveDate { get; set; }
		public int LookupValue { get; set; } // combo box of int values
		public int Level { get; set; } // numeric updown
		public string[] Items { get; set; } // not sure about this yet
		public string BuilderText { get; set; } // plain text used with builder textbox
	}
}