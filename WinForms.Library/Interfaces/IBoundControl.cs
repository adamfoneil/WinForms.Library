using System;

namespace WinForms.Library.Interfaces
{
	/// <summary>
	/// Implement this on UserControls that you want to work with DocumentManager
	/// </summary>
	public interface IBoundControl<TValue>
	{
		event EventHandler ValueChanged;

		TValue Value { get; set; }
	}
}