using PrivateSchool.Core.Data;
using PrivateSchool.Core.Models;
using PrivateSchool.Core.Reports;
using PrivateSchool.Core.Services;
using PrivateSchool.WinForms.UI.Controls;
using PrivateSchool.WinForms.UI.Dialogs;
using PrivateSchool.WinForms.UI.Forms;

namespace PrivateSchool.WinForms.UI.Views;

public sealed class StudentsView : CrudView<Student>
{
    private ComboBox _group = null!;
    private ComboBox _status = null!;
    private CheckBox? _debtorsOnly;
    private DataGridViewColumn? _balanceColumn;

    public StudentsView(SchoolSystem app) : base(app)
    {
        var card = new FlatButton("Картка", ButtonVariant.Secondary, Theme.Glyphs.Card);
        card.Click += (_, _) => Try(OpenCard);
        Toolbar.Controls.Add(card);
        Toolbar.Controls.SetChildIndex(card, 4);
    }

    public override string Title => "Студенти";
    public override string Subtitle => "Контингент здобувачів освіти, договори та стан розрахунків";

    protected override Repository<Student> Repository => App.Db.Students;
    protected override string EntityName => "студента";
    protected override bool CanModify => App.Auth.Can(Permission.ManageStudents);
    private bool ShowFinance => App.Auth.Can(Permission.ViewPayments);

    protected override void ConfigureColumns(StyledGrid grid)
    {
        grid.AddColumn("ПІБ", 220, minWidth: 180);
        grid.AddColumn("Група", 70, ColumnAlign.Center);
        grid.AddColumn("Дата народження", 90, ColumnAlign.Center, "dd.MM.yyyy");
        grid.AddColumn("Телефон", 100);
        grid.AddColumn("Статус", 95);
        grid.AddColumn("Договір", 85);
        grid.AddColumn("Знижка", 60, ColumnAlign.Center);
        grid.AddColumn("Сер. бал", 65, ColumnAlign.Center, "0.0");
        _balanceColumn = grid.AddColumn("Сальдо, грн", 95, ColumnAlign.Right, "N2");
        _balanceColumn.Visible = ShowFinance;
    }

    protected override void BuildFilters()
    {
        _group = AddFilterCombo("Група:", 110);
        _status = AddFilterCombo("Статус:", 140);
        Lookup.Fill(_group, Lookup.Groups(App.Db), null, "Усі групи");
        _status.Items.Add("Усі статуси");
        foreach (var item in EnumDisplay.GetItems<StudentStatus>()) _status.Items.Add(item);
        _status.SelectedIndex = 0;

        if (ShowFinance)
        {
            _debtorsOnly = new CheckBox { Text = "Лише боржники", AutoSize = true, Margin = new Padding(16, 3, 0, 0) };
            _debtorsOnly.CheckedChanged += (_, _) => ApplyFilter();
            FilterPanel.Controls.Add(_debtorsOnly);
        }
    }

    public override void OnActivated()
    {
        var selected = Lookup.SelectedId(_group);
        Lookup.Fill(_group, Lookup.Groups(App.Db), selected, "Усі групи");
        base.OnActivated();
    }

    protected override bool PassesFilters(Student s)
    {
        if (Lookup.SelectedId(_group) is int g && s.GroupId != g) return false;
        if (_status.SelectedItem is EnumItem<StudentStatus> st && s.Status != st.Value) return false;
        if (_debtorsOnly?.Checked == true && !App.Finance.GetBalance(s).HasDebt) return false;
        return true;
    }

    protected override object?[] ToRow(Student s) => new object?[]
    {
        s.FullName,
        App.Db.Groups.GetById(s.GroupId)?.Name ?? "—",
        s.BirthDate,
        s.Phone,
        s.Status.ToDisplay(),
        s.ContractNumber,
        s.DiscountPercent > 0 ? $"{s.DiscountPercent:0.#} %" : "—",
        App.Academic.GetAverage(s.Id),
        ShowFinance ? App.Finance.GetBalance(s).Balance : null
    };

    protected override Color? GetRowColor(Student s) => s.IsStudying ? null : Theme.TextMuted;

    protected override void FormatCell(Student s, DataGridViewColumn column, DataGridViewCellFormattingEventArgs e)
    {
        if (column == _balanceColumn && e.Value is decimal balance)
        {
            e.CellStyle!.ForeColor = e.CellStyle.SelectionForeColor = balance < 0 ? Theme.Danger : Theme.Success;
            e.CellStyle.Font = Theme.BoldFont;
        }
    }

    protected override string GetSearchText(Student s) => $"{s.FullName} {s.Phone} {s.Email} {s.ContractNumber} {s.ParentName}";

    protected override Student CreateNew() => new()
    {
        GroupId = Lookup.SelectedId(_group) ?? 0,
        BirthDate = DateTime.Today.AddYears(-16),
        EnrollmentDate = DateTime.Today
    };

    protected override Form CreateDialog(Student item, bool isNew, Action<Student> save) =>
        new StudentDialog(App, item, isNew, save);

    protected override string GetFooterText(IReadOnlyList<Student> shown)
    {
        var text = base.GetFooterText(shown) + $"  ·  Навчаються: {shown.Count(s => s.Status == StudentStatus.Active)}";
        if (ShowFinance)
        {
            var debt = shown.Select(s => App.Finance.GetBalance(s)).Where(b => b.HasDebt).ToList();
            text += $"  ·  Боржників: {debt.Count} на суму {debt.Sum(b => b.Debt):N2} грн";
        }
        return text;
    }

    protected override void OnDoubleClickRow()
    {
        if (CanModify) base.OnDoubleClickRow();
        else Try(OpenCard);
    }

    private void OpenCard()
    {
        if (SelectedItem is not { } student)
        {
            Messages.Info("Оберіть студента у списку.", FindForm());
            return;
        }
        using var form = new StudentCardForm(App, student);
        form.ShowDialog(FindForm());
    }
}
