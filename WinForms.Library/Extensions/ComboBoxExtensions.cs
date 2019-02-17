using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WinForms.Library.Models;

namespace WinForms.Library.Extensions
{
	public static class ComboBoxExtensions
	{
		public static void Fill<TItem>(this ComboBox comboBox, IEnumerable<TItem> items)
		{
			comboBox.Items.Clear();
			items.ToList().ForEach((item) =>
			{
				comboBox.Items.Add(item);
			});
		}

		public static void Fill<TEnum>(this ComboBox comboBox)
		{
			var names = Enum.GetNames(typeof(TEnum));
			var values = Enum.GetValues(typeof(TEnum)).OfType<TEnum>();

			comboBox.Items.Clear();

			int index = 0;
			values.ToList().ForEach((e) =>
			{
				comboBox.Items.Add(new EnumValue<TEnum>(e, names[index]));
				index++;
			});
		}

		public static void SetValue<TEnum>(this ComboBox comboBox, TEnum value)
		{
			int index = 0;
			foreach (var item in comboBox.Items)
			{
				if ((item as EnumValue<TEnum>)?.Value.Equals(value) ?? false)
				{
					comboBox.SelectedIndex = index;
					return;
				}
				index++;
			}

			comboBox.SelectedIndex = -1;
		}

		public static void SetItem<TItem>(this ComboBox comboBox, TItem value) where TItem : class
		{
			int index = 0;
			foreach (var item in comboBox.Items)
			{
				if ((item as TItem)?.Equals(value) ?? false)
				{
					comboBox.SelectedIndex = index;
					return;
				}
				index++;
			}
		}
	}
}