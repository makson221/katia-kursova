using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PrivateSchool.Utils;

namespace PrivateSchool.Controls
{
    /// <summary>
    /// Повторно використовуваний елемент керування для відображення списку сутностей:
    /// панель інструментів (додати / редагувати / видалити), пошук, фільтр і таблиця.
    /// Сам елемент не знає про конкретні сутності — він лише генерує події,
    /// які обробляє головна форма.
    /// </summary>
    public partial class EntityListControl : UserControl
    {
        /// <summary>Назва прихованої колонки з ідентифікатором запису.</summary>
        public const string IdColumn = "Id";

        private bool _suppressFilterEvent;

        public event EventHandler AddClicked;
        public event EventHandler EditClicked;
        public event EventHandler DeleteClicked;

        /// <summary>Змінився текст пошуку або значення фільтра — дані треба оновити.</summary>
        public event EventHandler FilterChanged;

        public EntityListControl()
        {
            InitializeComponent();
            UiHelper.StyleGrid(grid);
        }

        /// <summary>Функція, що визначає колір рядка (наприклад, червоний для боржників).</summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Func<DataRow, Color?> RowColorSelector { get; set; }

        [Category("Поведінка")]
        [Description("Чи доступні кнопки додавання, редагування та видалення.")]
        [DefaultValue(true)]
        public bool AllowModify
        {
            get { return btnAdd.Visible; }
            set
            {
                btnAdd.Visible = value;
                btnEdit.Visible = value;
                btnDelete.Visible = value;
                sepEdit.Visible = value;
            }
        }

        [Category("Поведінка")]
        [Description("Підпис фільтра на панелі інструментів.")]
        [DefaultValue("Група:")]
        public string FilterCaption
        {
            get { return lblFilter.Text; }
            set { lblFilter.Text = value; }
        }

        [Category("Поведінка")]
        [DefaultValue(false)]
        public bool FilterVisible
        {
            get { return cmbFilter.Visible; }
            set
            {
                lblFilter.Visible = value;
                cmbFilter.Visible = value;
                sepFilter.Visible = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SearchText
        {
            get { return txtSearch.Text.Trim(); }
        }

        /// <summary>Значення обраного елемента фільтра (null — «усі»).</summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object FilterValue
        {
            get
            {
                ComboItem item = cmbFilter.SelectedItem as ComboItem;
                return item == null ? null : item.Value;
            }
        }

        /// <summary>Ідентифікатор вибраного рядка або null.</summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int? SelectedId
        {
            get
            {
                if (grid.CurrentRow == null || !grid.Columns.Contains(IdColumn))
                    return null;
                object value = grid.CurrentRow.Cells[IdColumn].Value;
                return value is int ? (int?)(int)value : null;
            }
        }

        [Browsable(false)]
        public DataGridView Grid
        {
            get { return grid; }
        }

        /// <summary>Заповнює список фільтра; першим додається пункт «усі».</summary>
        public void SetFilterItems(IEnumerable<ComboItem> items, string allText)
        {
            object current = FilterValue;
            _suppressFilterEvent = true;
            try
            {
                UiHelper.Fill(cmbFilter.ComboBox, new[] { new ComboItem(null, allText) }.Concat(items), current);
            }
            finally
            {
                _suppressFilterEvent = false;
            }
        }

        /// <summary>
        /// Відображає дані у таблиці. Рядки будь-якого типу (зокрема анонімного)
        /// перетворюються на DataTable, що дає змогу сортувати колонки кліком по заголовку.
        /// Виділення та сортування зберігаються після оновлення.
        /// </summary>
        public void Bind<T>(IEnumerable<T> rows, string summary = null)
        {
            int? selectedId = SelectedId;
            DataGridViewColumn sortColumn = grid.SortedColumn;
            string sortName = sortColumn == null ? null : sortColumn.DataPropertyName;
            ListSortDirection direction = grid.SortOrder == SortOrder.Descending
                ? ListSortDirection.Descending : ListSortDirection.Ascending;

            DataTable table = UiHelper.ToDataTable(rows);
            grid.DataSource = table;

            if (sortName != null && grid.Columns.Contains(sortName))
                grid.Sort(grid.Columns[sortName], direction);

            RestoreSelection(selectedId);
            lblCount.Text = "Записів: " + table.Rows.Count
                + (string.IsNullOrEmpty(summary) ? string.Empty : "    |    " + summary);
        }

        /// <summary>
        /// Колонки таблиці створюються під час прив'язки даних (для прихованих вкладок — пізніше,
        /// при першому показі), тому форматування виконується саме в цій події.
        /// </summary>
        private void grid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            UiHelper.FormatColumns(grid);
            if (grid.Columns.Contains(IdColumn))
                grid.Columns[IdColumn].Visible = false;
        }

        private void RestoreSelection(int? id)
        {
            if (id == null || !grid.Columns.Contains(IdColumn))
                return;
            foreach (DataGridViewRow row in grid.Rows)
            {
                if (Equals(row.Cells[IdColumn].Value, id.Value))
                {
                    grid.CurrentCell = row.Cells.Cast<DataGridViewCell>().First(c => c.Visible);
                    return;
                }
            }
        }

        private void Raise(EventHandler handler)
        {
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Raise(AddClicked);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            Raise(EditClicked);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Raise(DeleteClicked);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            Raise(FilterChanged);
        }

        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_suppressFilterEvent)
                Raise(FilterChanged);
        }

        private void grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && AllowModify)
                Raise(EditClicked);
        }

        private void grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (!AllowModify)
                return;
            if (e.KeyCode == Keys.Enter)
            {
                Raise(EditClicked);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Delete)
            {
                Raise(DeleteClicked);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Insert)
            {
                Raise(AddClicked);
                e.Handled = true;
            }
        }

        private void grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (RowColorSelector == null || e.RowIndex < 0)
                return;
            DataRowView view = grid.Rows[e.RowIndex].DataBoundItem as DataRowView;
            if (view == null)
                return;
            Color? color = RowColorSelector(view.Row);
            if (color.HasValue)
                e.CellStyle.ForeColor = color.Value;
        }
    }
}
