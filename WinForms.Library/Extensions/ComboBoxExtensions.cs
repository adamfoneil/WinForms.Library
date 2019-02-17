using System;
using System.Linq;
using System.Windows.Forms;
using WinForms.Library.Models;

namespace WinForms.Library.Extensions
{
	public static class ComboBoxExtensions
	{
		public static void Fill<TEnum>(this ComboBox comboBox)
		{
			var names = Enum.GetNames(typeof(TEnum));
			var values = Enum.GetValues(typeof(TEnum)).OfType<TEnum>();

			comboBox.Items.Clear();

			int index = 0;
			values.ToList().ForEach((e) =>
			{
				comboBox.Items.Add(new ComboBoxItem<TEnum>(e, names[index]));
				index++;
			});
		}

		public static void SetValue<TEnum>(this ComboBox comboBox, TEnum value)
		{
			int index = 0;
			foreach (var item in comboBox.Items)
			{
				if ((item as ComboBoxItem<TEnum>)?.Value.Equals(value) ?? false)
				{
					comboBox.SelectedIndex = index;
					return;
				}
				index++;
			}
		}

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