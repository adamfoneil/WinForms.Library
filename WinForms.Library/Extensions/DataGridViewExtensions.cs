using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using WinForms.Library.Models;

namespace WinForms.Library.Extensions
{
    public static class DataGridViewExtensions
    {
        public static void Fill<T>(this DataGridViewComboBoxColumn toolStripComboBox, Dictionary<T, string> values)
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

        public static void Fill<TEnum>(this DataGridViewComboBoxColumn comboBox)
        {
            var names = Enum.GetNames(typeof(TEnum));
            var values = Enum.GetValues(typeof(TEnum)).OfType<TEnum>();

            var bindingList = new BindingList<ListItem<TEnum>>();
            int index = 0;
            values.ToList().ForEach((value) =>
            {
                bindingList.Add(new ListItem<TEnum>(value, names[index]));
                index++;
            });

            comboBox.DataSource = bindingList;
            comboBox.ValueMember = "Value";
            comboBox.DisplayMember = "Text";
        }

        public static void Fill<T>(this DataGridView dataGridView, IEnumerable<T> items)
        {
            var list = new BindingList<T>((items ?? Enumerable.Empty<T>()).ToList());
            BindingSource bs = new BindingSource();
            bs.DataSource = list;
            dataGridView.DataSource = bs;
        }

        public static IEnumerable<T> GetItems<T>(this DataGridView dataGridView)
        {
            var list = (dataGridView.DataSource as BindingSource).DataSource as BindingList<T>;
            return list;
        }
    }
}
