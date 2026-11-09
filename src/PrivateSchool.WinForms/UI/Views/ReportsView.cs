using PrivateSchool.Core.Reports;
using PrivateSchool.Core.Services;
using PrivateSchool.WinForms.UI.Controls;

namespace PrivateSchool.WinForms.UI.Views;

/// <summary>
/// Формування, перегляд, друк та експорт звітів.
/// </summary>
public sealed class ReportsView : ViewBase
{
    private readonly ListBox _reports;
    private readonly Label _description;
    private readonly Label _fromLabel;
    private readonly Label _toLabel;
    private readonly Label _groupLabel;
    private readonly DateTimePicker _from;
    private readonly DateTimePicker _to;
    private readonly ComboBox _group = Lookup.DropDownList();
    private readonly StyledGrid _grid;
    private readonly Label _summary;
    private readonly Label _resultTitle;
    private readonly FlatButton _print;
    private readonly FlatButton _export;
    private ReportTable? _current;

    public ReportsView(SchoolSystem app) : base(app)
    {
        // Ліва панель — перелік звітів і параметри
        var left = new Panel { Dock = DockStyle.Left, Width = 300, BackColor = Theme.Surface, Padding = new Padding(16) };
        var listTitle = new Label { Text = "Оберіть звіт", Dock = DockStyle.Top, Height = 28, Font = Theme.SectionFont };

        _reports = new ListBox
        {
            Dock = DockStyle.Top,
            Height = 200,
            BorderStyle = BorderStyle.None,
            DrawMode = DrawMode.OwnerDrawFixed,
            ItemHeight = 32,
            IntegralHeight = false
        };
        foreach (var report in App.Reports.Where(r => IsAllowed(r))) _reports.Items.Add(report);
        _reports.DrawItem += DrawReportItem;
        _reports.SelectedIndexChanged += (_, _) => OnReportChanged();

        _description = new Label { Dock = DockStyle.Top, Height = 64, ForeColor = Theme.TextMuted, Padding = new Padding(0, 8, 0, 0) };

        var parameters = new TableLayoutPanel { Dock = DockStyle.Top, Height = 190, ColumnCount = 1 };
        _fromLabel = ParamLabel("Період з");
        _from = new DateTimePicker { Format = DateTimePickerFormat.Custom, CustomFormat = "dd.MM.yyyy", Width = 250 };
        _toLabel = ParamLabel("по");
        _to = new DateTimePicker { Format = DateTimePickerFormat.Custom, CustomFormat = "dd.MM.yyyy", Width = 250 };
        _groupLabel = ParamLabel("Група");
        _group.Width = 250;
        var today = DateTime.Today;
        _from.Value = new DateTime(today.Year, today.Month, 1);
        _to.Value = today;
        parameters.Controls.AddRange(new Control[] { _fromLabel, _from, _toLabel, _to, _groupLabel, _group });

        var build = new FlatButton("Сформувати звіт", ButtonVariant.Primary, Theme.Glyphs.Play) { AutoSize = false, Dock = DockStyle.Top, Height = 40 };
        build.Click += (_, _) => Try(BuildReport);

        left.Controls.Add(build);
        left.Controls.Add(parameters);
        left.Controls.Add(_description);
        left.Controls.Add(_reports);
        left.Controls.Add(listTitle);

        // Права частина — результат
        var right = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20, 0, 0, 0), BackColor = Theme.Background };
        var resultCard = new Panel { Dock = DockStyle.Fill, BackColor = Theme.Surface };
        var resultHeader = new Panel { Dock = DockStyle.Top, Height = 56, BackColor = Theme.Surface, Padding = new Padding(16, 10, 12, 10) };
        _resultTitle = new Label { Dock = DockStyle.Fill, Font = Theme.SectionFont, TextAlign = ContentAlignment.MiddleLeft, Text = "Результат" };
        var actions = new FlowLayoutPanel { Dock = DockStyle.Right, AutoSize = true, WrapContents = false };
        _print = new FlatButton("Друк", ButtonVariant.Secondary, Theme.Glyphs.Print) { Enabled = false };
        _export = new FlatButton("Зберегти у файл", ButtonVariant.Secondary, Theme.Glyphs.Export) { Enabled = false };
        _print.Click += (_, _) => Try(Print);
        _export.Click += (_, _) => Try(() => { if (_current != null) ReportExport.SaveWithDialog(App, _current, FindForm()); });
        actions.Controls.Add(_print);
        actions.Controls.Add(_export);
        resultHeader.Controls.Add(_resultTitle);
        resultHeader.Controls.Add(actions);

        _grid = new StyledGrid { Dock = DockStyle.Fill };
        _summary = new Label
        {
            Dock = DockStyle.Bottom,
            Height = 80,
            Padding = new Padding(16, 8, 16, 8),
            BackColor = Theme.SurfaceAlt,
            Font = Theme.BoldFont
        };
        resultCard.Controls.Add(_grid);
        resultCard.Controls.Add(_summary);
        resultCard.Controls.Add(resultHeader);
        right.Controls.Add(resultCard);

        Content.Controls.Add(right);
        Content.Controls.Add(left);

        if (_reports.Items.Count > 0) _reports.SelectedIndex = 0;
    }

    public override string Title => "Звіти";
    public override string Subtitle => "Аналітичні та фінансові звіти з можливістю друку й експорту";

    private bool IsAllowed(Report report) =>
        report is not (DebtorsReport or PaymentsReport or TeacherLoadReport) || App.Auth.Can(Permission.ViewPayments);

    private static Label ParamLabel(string text) => new()
    {
        Text = text,
        AutoSize = true,
        Font = Theme.LabelFont,
        ForeColor = Theme.TextMuted,
        Margin = new Padding(0, 8, 0, 4)
    };

    public override void RefreshData()
    {
        var selected = Lookup.SelectedId(_group);
        Lookup.Fill(_group, Lookup.Groups(App.Db), selected, "Усі групи");
        OnReportChanged();
    }

    private Report? SelectedReport => _reports.SelectedItem as Report;

    private void OnReportChanged()
    {
        if (SelectedReport is not { } report) return;
        _description.Text = report.Description;
        _fromLabel.Visible = _from.Visible = _toLabel.Visible = _to.Visible = report.UsesPeriod;
        _groupLabel.Visible = _group.Visible = report.UsesGroup;

        // Звіт, що вимагає групу, не може мати варіант "Усі групи"
        var selected = Lookup.SelectedId(_group);
        if (report.RequiresGroup)
        {
            Lookup.Fill(_group, Lookup.Groups(App.Db), selected);
            if (_group.SelectedIndex < 0 && _group.Items.Count > 0) _group.SelectedIndex = 0;
        }
        else
        {
            Lookup.Fill(_group, Lookup.Groups(App.Db), selected, "Усі групи");
        }
    }

    private void BuildReport()
    {
        if (SelectedReport is not { } report) return;

        var table = report.Build(new ReportParameters
        {
            From = _from.Value.Date,
            To = _to.Value.Date,
            GroupId = report.UsesGroup ? Lookup.SelectedId(_group) : null
        });

        _current = table;
        _resultTitle.Text = table.Subtitle.Length > 0 ? $"{table.Title}  —  {table.Subtitle}" : table.Title;

        _grid.Columns.Clear();
        foreach (var column in table.Columns) _grid.AddColumn(column.Header, column.Width, column.Align);
        foreach (var row in table.Rows) _grid.Rows.Add(row.Cast<object>().ToArray());

        _summary.Text = string.Join("     ", table.Summary);
        _print.Enabled = _export.Enabled = true;

        if (table.Rows.Count == 0) Messages.Info("За заданими параметрами даних не знайдено.", FindForm());
    }

    private void Print()
    {
        if (_current == null) return;
        using var printer = new ReportPrinter(_current, App.Db.InstitutionName);
        printer.ShowPreview(FindForm());
    }

    private void DrawReportItem(object? sender, DrawItemEventArgs e)
    {
        if (e.Index < 0) return;
        var selected = (e.State & DrawItemState.Selected) != 0;
        using var back = new SolidBrush(selected ? Theme.PrimaryLight : Theme.Surface);
        e.Graphics.FillRectangle(back, e.Bounds);
        if (selected)
        {
            using var accent = new SolidBrush(Theme.Primary);
            e.Graphics.FillRectangle(accent, e.Bounds.Left, e.Bounds.Top + 4, 3, e.Bounds.Height - 8);
        }
        var text = _reports.Items[e.Index].ToString();
        TextRenderer.DrawText(e.Graphics, text, selected ? Theme.BoldFont : Theme.BaseFont,
            new Rectangle(e.Bounds.Left + 12, e.Bounds.Top, e.Bounds.Width - 12, e.Bounds.Height),
            selected ? Theme.PrimaryDark : Theme.Text, TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
    }
}
