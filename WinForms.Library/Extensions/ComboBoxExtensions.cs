using System;
using System.Windows.Forms;

namespace WinForms.Library.Extensions
{
	public static class ComboBoxExtensions
	{
		public static void SetValue<T>(this ComboBox comboBox, Func<T, bool> predicate) where T : class
		{
			object[] values = new object[comboBox.Items.Count];
			comboBox.Items.CopyTo(values, 0);

			for (int i = 0; i < values.Length; i++)
			{
				if (predicate.Invoke(values[i] as T))
				{
					comboBox.SelectedIndex = i;
					return;
				}
			}

			comboBox.SelectedIndex = -1;
		}
	}
}