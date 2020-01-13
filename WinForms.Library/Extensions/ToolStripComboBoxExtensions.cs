using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WinForms.Library.Models;

namespace WinForms.Library.Extensions.ToolStrip
{
    public static class ToolStripComboBoxExtensions
    {
        public static void Fill<T>(this ToolStripComboBox toolStripComboBox, Dictionary<T, string> values)
        {
            toolStripComboBox.Items.Clear();
            foreach (var kp in values) toolStripComboBox.Items.Add(new ListItem<T>(kp.Key, kp.Value));
        }

        public static void Fill<T>(this ToolStripComboBox toolStripComboBox, IEnumerable<T> items)
        {
            if (toolStripComboBox is null)
            {
                throw new ArgumentNullException(nameof(toolStripComboBox));
            }

            toolStripComboBox.Items.Clear();
            items.ToList().ForEach((item) =>
            {
                toolStripComboBox.Items.Add(item);
            });
        }

        public static void Fill<TEnum>(this ToolStripComboBox toolStripComboBox)
        {
            var names = Enum.GetNames(typeof(TEnum));
            var values = Enum.GetValues(typeof(TEnum)).OfType<TEnum>();

            toolStripComboBox.Items.Clear();

            int index = 0;
            values.ToList().ForEach((e) =>
            {
                toolStripComboBox.Items.Add(new ListItem<TEnum>(e, names[index]));
                index++;
            });
        }

        public static void SetValue<T>(this ToolStripComboBox toolStripComboBox, T value)
        {
            int index = 0;
            foreach (var item in toolStripComboBox.Items)
            {
                if ((item as ListItem<T>)?.Value.Equals(value) ?? false)
                {
                    toolStripComboBox.SelectedIndex = index;
                    return;
                }
                index++;
            }

            toolStripComboBox.SelectedIndex = -1;
        }

        public static void SetItem<TItem>(this ToolStripComboBox toolStripComboBox, TItem value) where TItem : class
        {
            int index = 0;
            foreach (var item in toolStripComboBox.Items)
            {
                if ((item as TItem)?.Equals(value) ?? false)
                {
                    toolStripComboBox.SelectedIndex = index;
                    return;
                }
                index++;
            }
        }

        public static T GetValue<T>(this ToolStripComboBox toolStripComboBox)
        {
            var value = toolStripComboBox.SelectedItem as ListItem<T>;
            return (value != null) ? value.Value : default(T);
        }

        public static TItem GetItem<TItem>(this ToolStripComboBox toolStripComboBox) where TItem : class
        {
            var value = toolStripComboBox.SelectedItem as TItem;
            return (value != null) ? value : default(TItem);
        }
    }
}
