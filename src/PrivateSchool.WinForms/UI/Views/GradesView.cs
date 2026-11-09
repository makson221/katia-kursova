using PrivateSchool.Core.Data;
using PrivateSchool.Core.Models;
using PrivateSchool.Core.Reports;
using PrivateSchool.Core.Services;
using PrivateSchool.WinForms.UI.Controls;
using PrivateSchool.WinForms.UI.Dialogs;

namespace PrivateSchool.WinForms.UI.Views;

public sealed class GradesView : CrudView<Grade>
{
    private ComboBox _group = null!;
    private ComboBox _subject = null!;
    private ComboBox _type = null!;
    private DataGridViewColumn _scoreColumn = null!;
    private DataGridViewColumn _ectsColumn = null!;

    public GradesView(SchoolSystem app) : base(app) { }

    public override string Title => "Успішність";
    public override string Subtitle => App.IsTeacher()
        ? "Журнал оцінок з ваших дисциплін (100-бальна шкала, ЄКТС)"
        : "Журнал оцінок студентів (100-бальна шкала, ЄКТС)";

    protected override Repository<Grade> Repository => App.Db.Grades;
    protected override string EntityName => "оцінки";
    protected override bool CanModify => App.Auth.Can(Permission.ManageGrades);
    protected override bool CanModifyItem(Grade item) => App.CanEditGrade(item);

    protected override void ConfigureColumns(StyledGrid grid)
    {
        grid.AddColumn("Дата", 70, ColumnAlign.Center, "dd.MM.yyyy");
        grid.AddColumn("Студент", 190, minWidth: 160);
        grid.AddColumn("Група", 55, ColumnAlign.Center);
        grid.AddColumn("Дисципліна", 180);
        grid.AddColumn("Вид контролю", 110);
        _scoreColumn = grid.AddColumn("Бали", 50, ColumnAlign.Center);
        _ectsColumn = grid.AddColumn("ЄКТС", 50, ColumnAlign.Center);
        grid.AddColumn("Нац. шкала", 90, ColumnAlign.Center);
        grid.AddColumn("Коментар", 110);
    }

    protected override void BuildFilters()
    {
        _group = AddFilterCombo("Група:", 100);
        _subject = AddFilterCombo("Дисципліна:", 200);
        _type = AddFilterCombo("Вид:", 130);
        _type.Items.Add("Усі");
        foreach (var item in EnumDisplay.GetItems<GradeType>()) _type.Items.Add(item);
        _type.SelectedIndex = 0;
        FillLookups();
    }

    private void FillLookups()
    {
        Lookup.Fill(_group, Lookup.Groups(App.Db), Lookup.SelectedId(_group), "Усі групи");
        Lookup.Fill(_subject, Lookup.Subjects(App.GradableSubjects()), Lookup.SelectedId(_subject), "Усі дисципліни");
    }

    public override void OnActivated()
    {
        FillLookups();
        base.OnActivated();
    }

    protected override IEnumerable<Grade> GetItems()
    {
        var allowed = App.GradableSubjects().Select(s => s.Id).ToHashSet();
        return Repository.GetAll()
            .Where(g => allowed.Contains(g.SubjectId))
            .OrderByDescending(g => g.Date);
    }

    protected override bool PassesFilters(Grade g)
    {
        if (Lookup.SelectedId(_subject) is int subj && g.SubjectId != subj) return false;
        if (_type.SelectedItem is EnumItem<GradeType> t && g.Type != t.Value) return false;
        if (Lookup.SelectedId(_group) is int groupId && App.Db.Students.GetById(g.StudentId)?.GroupId != groupId) return false;
        return true;
    }

    protected override object?[] ToRow(Grade g)
    {
        var student = App.Db.Students.GetById(g.StudentId);
        return new object?[]
        {
            g.Date,
            student?.FullName ?? "—",
            student == null ? "—" : App.Db.Groups.GetById(student.GroupId)?.Name,
            App.Db.Subjects.GetById(g.SubjectId)?.Name ?? "—",
            g.Type.ToDisplay(),
            g.Score,
            g.Ects,
            $"{g.National} ({GradeScale.ToNationalText(g.Score)})",
            g.Comment
        };
    }

    protected override void FormatCell(Grade g, DataGridViewColumn column, DataGridViewCellFormattingEventArgs e)
    {
        if (column != _scoreColumn && column != _ectsColumn) return;
        e.CellStyle!.Font = Theme.BoldFont;
        e.CellStyle.ForeColor = e.CellStyle.SelectionForeColor = g.Score >= 90 ? Theme.Success : g.IsPassed ? Theme.Text : Theme.Danger;
    }

    protected override string GetSearchText(Grade g)
    {
        var student = App.Db.Students.GetById(g.StudentId);
        return $"{student?.FullName} {App.Db.Subjects.GetById(g.SubjectId)?.Name} {g.Comment}";
    }

    protected override Grade CreateNew() => new()
    {
        Date = DateTime.Today,
        Score = 75,
        SubjectId = Lookup.SelectedId(_subject) ?? 0
    };

    protected override Form CreateDialog(Grade item, bool isNew, Action<Grade> save) =>
        new GradeDialog(App, item, isNew, save);

    protected override string GetFooterText(IReadOnlyList<Grade> shown)
    {
        if (shown.Count == 0) return "Оцінок не знайдено";
        return $"Оцінок: {shown.Count}  ·  Середній бал: {shown.Average(g => g.Score):0.0}  ·  " +
               $"Незадовільних: {shown.Count(g => !g.IsPassed)}  ·  Відмінних (A): {shown.Count(g => g.Score >= 90)}";
    }
}
