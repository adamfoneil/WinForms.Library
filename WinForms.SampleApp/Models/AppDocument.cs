using System;

namespace WinForms.SampleApp.Models
{
	/// <summary>
	/// This is just a bogus model class demonstrating various types for use with DocumentManager
	/// </summary>
	public class AppDocument
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public bool IsActive { get; set; }
		public DateTime EffectiveDate { get; set; }
		public int Level { get; set; }
	}
}