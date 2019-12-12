using System;
using System.Linq;
using System.Windows.Forms;
using WinForms.Library.Models;

namespace WinForms.Library.Extensions
{
    public static class DataGridViewComboBoxExtensions
    {
        public static void Fill<TEnum>(this DataGridViewComboBoxColumn column)
        {
            var names = Enum.GetNames(typeof(TEnum));
            var values = Enum.GetValues(typeof(TEnum)).OfType<TEnum>();

            column.Items.Clear();

            int index = 0;
            values.ToList().ForEach((e) =>
            {
                column.Items.Add(new ListItem<TEnum>(e, names[index]));
                index++;
            });
        }
    }
}
