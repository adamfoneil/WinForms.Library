using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WinForms.Library.Models;

namespace WinForms.Library.Extensions
{
    public static class ComboBoxExtensions
    {
        public static void Fill<T>(this ComboBox comboBox, Dictionary<string, T> values)
        {                        
            comboBox.Items.Clear();
            foreach (var kp in values) comboBox.Items.Add(new ListItem<T>(kp.Value, kp.Key));
        }

        public static void Fill<T>(this ComboBox comboBox, IEnumerable<T> items)
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
                comboBox.Items.Add(new ListItem<TEnum>(e, names[index]));
                index++;
            });
        }

        public static void SetValue<T>(this ComboBox comboBox, T value)
        {
            int index = 0;
            foreach (var item in comboBox.Items)
            {
                if ((item as ListItem<T>)?.Value.Equals(value) ?? false)
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

        public static T GetValue<T>(this ComboBox comboBox)
        {
            var value = comboBox.SelectedItem as ListItem<T>;
            return (value != null) ? value.Value : default(T);
        }

        public static TItem GetItem<TItem>(this ComboBox comboBox) where TItem : class
        {
            var value = comboBox.SelectedItem as TItem;
            return (value != null) ? value : default(TItem);
        }
    }
}