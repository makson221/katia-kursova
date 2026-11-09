using System.ComponentModel;
using PrivateSchool.Core.Data;
using PrivateSchool.Core.Models;
using PrivateSchool.Core.Services;
using PrivateSchool.WinForms.UI.Controls;

namespace PrivateSchool.WinForms.UI.Views;

/// <summary>
/// Узагальнений розділ "список + додавання/редагування/видалення" для будь-якої сутності.
/// Нащадки описують колонки, рядки, фільтри та діалог редагування.
/// </summary>
[DesignerCategory("Code")]
public abstract class CrudView<T> : ViewBase where T : Entity
{
    private readonly TextBox _search;
    private readonly Label _footer;
    private readonly FlatButton _add;
    private readonly FlatButton _edit;
    private readonly FlatButton _delete;
    private Dictionary<int, T> _shown = new();
    private bool _ready;

    protected CrudView(SchoolSystem app) : base(app)
    {
        var card = new Panel { Dock = DockStyle.Fill, BackColor = Theme.Surface, Padding = new Padding(1) };
        card.Paint += (_, e) =>
        {
            using var pen = new Pen(Theme.Border);
            e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
        };

        Grid = new StyledGrid { Dock = DockStyle.Fill };
        Grid.CellDoubleClick += (_, e) => { if (e.RowIndex >= 0) OnDoubleClickRow(); };
        Grid.SelectionChanged += (_, _) => UpdateButtons();
        Grid.CellFormatting += OnCellFormatting;

        _footer = new Label
        {
            Dock = DockStyle.Bottom,
            Height = 36,
            Padding = new Padding(12, 0, 0, 0),
            TextAlign = ContentAlignment.MiddleLeft,
            ForeColor = Theme.TextMuted,
            BackColor = Theme.SurfaceAlt
        };

        card.Controls.Add(Grid);
        card.Controls.Add(_footer);

        // Панель інструментів
        Toolbar = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 52,
            WrapContents = false,
            BackColor = Theme.Background,
            Padding = new Padding(0, 0, 0, 12)
        };

        _add = new FlatButton("Додати", ButtonVariant.Primary, Theme.Glyphs.Add);
        _edit = new FlatButton("Редагувати", ButtonVariant.Secondary, Theme.Glyphs.Edit);
        _delete = new FlatButton("Видалити", ButtonVariant.Danger, Theme.Glyphs.Delete);
        var export = new FlatButton("Експорт", ButtonVariant.Secondary, Theme.Glyphs.Export);

        _add.Click += (_, _) => Try(AddItem);
        _edit.Click += (_, _) => Try(EditSelected);
        _delete.Click += (_, _) => Try(DeleteSelected);
        export.Click += (_, _) => Try(ExportGrid);

        _search = new TextBox
        {
            PlaceholderText = "Пошук…",
            Width = 220,
            BorderStyle = BorderStyle.FixedSingle,
            Margin = new Padding(12, 6, 8, 0)
        };
        _search.TextChanged += (_, _) => ApplyFilter();

        Toolbar.Controls.AddRange(new Control[] { _add, _edit, _delete, export, _search });

        // Другий рядок — фільтри (показується, лише якщо нащадок їх додав)
        FilterPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 42,
            WrapContents = false,
            BackColor = Theme.Background,
            Padding = new Padding(0, 0, 0, 8)
        };

        Content.Controls.Add(card);
        Content.Controls.Add(FilterPanel);
        Content.Controls.Add(Toolbar);

        ConfigureColumns(Grid);
        BuildFilters();
        FilterPanel.Visible = FilterPanel.Controls.Count > 0;
    }

    protected StyledGrid Grid { get; }
    protected FlowLayoutPanel Toolbar { get; }

    /// <summary>Контейнер для додаткових фільтрів нащадків.</summary>
    protected FlowLayoutPanel FilterPanel { get; }

    protected abstract Repository<T> Repository { get; }

    /// <summary>Назва сутності в однині (для повідомлень), напр. "студента".</summary>
    protected abstract string EntityName { get; }

    /// <summary>Чи має поточний користувач право змінювати записи.</summary>
    protected abstract bool CanModify { get; }

    protected abstract void ConfigureColumns(StyledGrid grid);
    protected abstract object?[] ToRow(T item);
    protected abstract T CreateNew();

    /// <summary>Створює діалог редагування; <paramref name="save"/> зберігає сутність у сховищі.</summary>
    protected abstract Form CreateDialog(T item, bool isNew, Action<T> save);

    /// <summary>Текст, по якому виконується пошук.</summary>
    protected abstract string GetSearchText(T item);

    /// <summary>Додаткові фільтри нащадка.</summary>
    protected virtual bool PassesFilters(T item) => true;

    protected virtual void BuildFilters() { }

    protected virtual IEnumerable<T> GetItems() => Repository.GetAll();

    /// <summary>Чи можна змінювати конкретний запис (для обмежень на рівні рядка).</summary>
    protected virtual bool CanModifyItem(T item) => CanModify;

    /// <summary>Колір тексту рядка (виділення боржників, незадовільних оцінок тощо).</summary>
    protected virtual Color? GetRowColor(T item) => null;

    protected virtual string GetFooterText(IReadOnlyList<T> shown) =>
        $"Показано записів: {shown.Count} із {Repository.Count}";

    protected virtual void Insert(T item) => Repository.Add(item);
    protected virtual void Update(T item) => Repository.Update(item);

    protected virtual void OnDoubleClickRow()
    {
        if (SelectedItem is { } item && CanModifyItem(item)) Try(EditSelected);
    }

    protected T? SelectedItem => Grid.SelectedId is int id ? Repository.GetById(id) : null;

    public override void RefreshData()
    {
        _ready = true;
        ApplyFilter();
    }

    protected void ApplyFilter()
    {
        if (!_ready) return;
        var selectedId = Grid.SelectedId;
        var query = _search.Text.Trim();

        var items = GetItems()
            .Where(PassesFilters)
            .Where(x => query.Length == 0 || GetSearchText(x).Contains(query, StringComparison.CurrentCultureIgnoreCase))
            .ToList();
        _shown = items.ToDictionary(x => x.Id);

        Grid.SuspendLayout();
        Grid.Rows.Clear();
        foreach (var item in items)
        {
            var index = Grid.Rows.Add(ToRow(item));
            var row = Grid.Rows[index];
            row.Tag = item.Id;
            if (GetRowColor(item) is Color color) row.DefaultCellStyle.ForeColor = row.DefaultCellStyle.SelectionForeColor = color;
        }
        Grid.ResumeLayout();

        if (selectedId is int id) Grid.SelectById(id);
        _footer.Text = GetFooterText(items);
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        _add.Visible = CanModify;
        _edit.Visible = CanModify;
        _delete.Visible = CanModify;
        var item = SelectedItem;
        _edit.Enabled = item != null && CanModifyItem(item);
        _delete.Enabled = item != null && CanModifyItem(item);
    }

    protected void AddItem()
    {
        if (!CanModify) return;
        var item = CreateNew();
        using var dialog = CreateDialog(item, true, Insert);
        if (dialog.ShowDialog(FindForm()) == DialogResult.OK)
        {
            ApplyFilter();
            Grid.SelectById(item.Id);
        }
    }

    protected void EditSelected()
    {
        if (SelectedItem is not { } selected || !CanModifyItem(selected)) return;
        var copy = (T)selected.Clone();
        using var dialog = CreateDialog(copy, false, Update);
        if (dialog.ShowDialog(FindForm()) == DialogResult.OK)
        {
            ApplyFilter();
            Grid.SelectById(copy.Id);
        }
    }

    protected void DeleteSelected()
    {
        if (SelectedItem is not { } item || !CanModifyItem(item)) return;

        var blocker = App.Integrity.GetDeleteBlocker(item);
        if (blocker != null)
        {
            Messages.Warning(blocker, FindForm());
            return;
        }

        var question = $"Видалити запис {EntityName}?";
        if (App.Integrity.DescribeCascade(item) is { } cascade) question += "\n\n" + cascade;
        if (!Messages.Confirm(question, FindForm())) return;

        App.Integrity.Delete(item);
        ApplyFilter();
    }

    private void ExportGrid()
    {
        var table = Grid.ToReportTable(Title, $"Сформовано {DateTime.Now:dd.MM.yyyy HH:mm}");
        ReportExport.SaveWithDialog(App, table, FindForm());
    }

    private void OnCellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        // Нащадки можуть розфарбовувати окремі клітинки
        if (e.RowIndex < 0 || Grid.Rows[e.RowIndex].Tag is not int id) return;
        if (_shown.TryGetValue(id, out var item)) FormatCell(item, Grid.Columns[e.ColumnIndex], e);
    }

    protected virtual void FormatCell(T item, DataGridViewColumn column, DataGridViewCellFormattingEventArgs e) { }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        switch (keyData)
        {
            case Keys.Control | Keys.N:
                Try(AddItem);
                return true;
            case Keys.F2:
                Try(EditSelected);
                return true;
            case Keys.Delete when Grid.Focused:
                Try(DeleteSelected);
                return true;
            case Keys.Control | Keys.F:
                _search.Focus();
                _search.SelectAll();
                return true;
            case Keys.F5:
                RefreshData();
                return true;
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }

    /// <summary>Створює підписаний випадаючий список-фільтр на панелі інструментів.</summary>
    protected ComboBox AddFilterCombo(string label, int width = 150)
    {
        FilterPanel.Controls.Add(new Label
        {
            Text = label,
            AutoSize = true,
            ForeColor = Theme.TextMuted,
            Margin = new Padding(FilterPanel.Controls.Count == 0 ? 0 : 12, 4, 4, 0)
        });
        var combo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = width, Margin = new Padding(0, 0, 4, 0) };
        combo.SelectedIndexChanged += (_, _) => ApplyFilter();
        FilterPanel.Controls.Add(combo);
        return combo;
    }

    protected DateTimePicker AddFilterDate(string label, DateTime value)
    {
        FilterPanel.Controls.Add(new Label
        {
            Text = label,
            AutoSize = true,
            ForeColor = Theme.TextMuted,
            Margin = new Padding(FilterPanel.Controls.Count == 0 ? 0 : 12, 4, 4, 0)
        });
        var picker = new DateTimePicker
        {
            Format = DateTimePickerFormat.Custom,
            CustomFormat = "dd.MM.yyyy",
            Width = 120,
            Value = value,
            Margin = new Padding(0, 0, 4, 0)
        };
        picker.ValueChanged += (_, _) => ApplyFilter();
        FilterPanel.Controls.Add(picker);
        return picker;
    }
}
