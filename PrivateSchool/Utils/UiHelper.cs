using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace PrivateSchool.Utils
{
    /// <summary>
    /// Допоміжні методи для роботи з елементами керування Windows Forms.
    /// </summary>
    public static class UiHelper
    {
        /// <summary>Заповнює ComboBox елементами та вибирає елемент із заданим значенням.</summary>
        public static void Fill(ComboBox combo, IEnumerable<ComboItem> items, object selectedValue)
        {
            combo.BeginUpdate();
            combo.Items.Clear();
            foreach (ComboItem item in items)
                combo.Items.Add(item);
            combo.EndUpdate();
            Select(combo, selectedValue);
        }

        public static void Select(ComboBox combo, object value)
        {
            ComboItem match = combo.Items.Cast<ComboItem>().FirstOrDefault(i => Equals(i.Value, value));
            if (match != null)
                combo.SelectedItem = match;
            else if (combo.Items.Count > 0)
                combo.SelectedIndex = 0;
        }

        public static T GetValue<T>(ComboBox combo)
        {
            ComboItem item = combo.SelectedItem as ComboItem;
            return item == null ? default(T) : (T)item.Value;
        }

        public static bool Confirm(IWin32Window owner, string message)
        {
            return MessageBox.Show(owner, message, "Підтвердження", MessageBoxButtons.YesNo,
                       MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
        }

        public static void ShowWarning(IWin32Window owner, string message)
        {
            MessageBox.Show(owner, message, "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void ShowError(IWin32Window owner, string message)
        {
            MessageBox.Show(owner, message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowInfo(IWin32Window owner, string message)
        {
            MessageBox.Show(owner, message, "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    
        /// <summary>
        /// Перетворює колекцію об'єктів (у тому числі анонімних типів) на DataTable
        /// за допомогою рефлексії. Символ «_» у назвах властивостей замінюється пробілом.
        /// </summary>
        public static DataTable ToDataTable<T>(IEnumerable<T> rows)
        {
            DataTable table = new DataTable();
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo property in properties)
            {
                Type type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                if (type.IsEnum)
                    type = typeof(string);
                table.Columns.Add(property.Name.Replace('_', ' '), type);
            }

            foreach (T row in rows)
            {
                object[] values = properties.Select(p =>
                {
                    object value = p.GetValue(row, null);
                    if (value == null) return DBNull.Value;
                    if (value is Enum) return EnumHelper.ToText((Enum)value);
                    return value;
                }).ToArray();
                table.Rows.Add(values);
            }
            return table;
        }

        /// <summary>
        /// Формат відображення колонок залежно від типу даних. Відносна ширина колонки
        /// (FillWeight) пропорційна найдовшому значенню в ній, тому короткі числа
        /// займають мало місця, а ПІБ і назви — більше.
        /// </summary>
        public static void FormatColumns(DataGridView grid)
        {
            int rowsToCheck = Math.Min(grid.Rows.Count, 200);
            foreach (DataGridViewColumn column in grid.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Automatic;
                column.MinimumWidth = 48;
                Type type = column.ValueType;

                int length = column.HeaderText.Split(' ').Max(w => w.Length);
                for (int i = 0; i < rowsToCheck; i++)
                    length = Math.Max(length, DisplayLength(grid.Rows[i].Cells[column.Index].Value));
                column.FillWeight = (Math.Min(length, 22) + 4) * 10;

                if (type == typeof(decimal))
                {
                    column.DefaultCellStyle.Format = "N2";
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                else if (type == typeof(double))
                {
                    column.DefaultCellStyle.Format = "0.00";
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                else if (type == typeof(int))
                {
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else if (type == typeof(DateTime))
                {
                    column.DefaultCellStyle.Format = "dd.MM.yyyy";
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
        }

        private static int DisplayLength(object value)
        {
            if (value == null || value is DBNull)
                return 0;
            if (value is decimal)
                return ((decimal)value).ToString("N2").Length;
            if (value is double)
                return ((double)value).ToString("0.00").Length;
            if (value is DateTime)
                return 10;
            return value.ToString().Length;
        }
    }
}
