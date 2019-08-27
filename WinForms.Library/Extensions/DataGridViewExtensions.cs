﻿using System;
using System.Collections.Generic;
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

            comboBox.Items.Clear();

            int index = 0;
            values.ToList().ForEach((e) =>
            {
                comboBox.Items.Add(new ListItem<TEnum>(e, names[index]));
                index++;
            });
        }
    }
}