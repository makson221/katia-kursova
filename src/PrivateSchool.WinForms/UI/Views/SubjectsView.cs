using PrivateSchool.Core.Data;
using PrivateSchool.Core.Models;
using PrivateSchool.Core.Reports;
using PrivateSchool.Core.Services;
using PrivateSchool.WinForms.UI.Controls;
using PrivateSchool.WinForms.UI.Dialogs;

namespace PrivateSchool.WinForms.UI.Views;

public sealed class SubjectsView : CrudView<Subject>
{
    public SubjectsView(SchoolSystem app) : base(app) { }

    public override string Title => "Дисципліни";
    public override string Subtitle => "Навчальний план: обсяг годин, форма контролю, викладачі";

    protected override Repository<Subject> Repository => App.Db.Subjects;
    protected override string EntityName => "дисципліни";
    protected override bool CanModify => App.Auth.Can(Permission.ManageSubjects);

    protected override void ConfigureColumns(StyledGrid grid)
    {
        grid.AddColumn("Назва", 220, minWidth: 180);
        grid.AddColumn("Годин", 55, ColumnAlign.Center);
        grid.AddColumn("Кредитів", 60, ColumnAlign.Center, "0.0");
        grid.AddColumn("Контроль", 70, ColumnAlign.Center);
        grid.AddColumn("Викладач", 170);
        grid.AddColumn("Групи", 120);
        grid.AddColumn("Опис", 220);
    }

    protected override object?[] ToRow(Subject s)
    {
        var groups = App.Db.Lessons.Find(l => l.SubjectId == s.Id).Select(l => l.GroupId).Distinct()
            .Select(id => App.Db.Groups.GetById(id)?.Name).Where(n => n != null).OrderBy(n => n);
        return new object?[]
        {
            s.Name,
            s.Hours,
            s.Credits,
            s.ControlForm.ToDisplay(),
            s.TeacherId is int t ? App.Db.Teachers.GetById(t)?.FullName : "—",
            string.Join(", ", groups),
            s.Description
        };
    }

    protected override string GetSearchText(Subject s) =>
        $"{s.Name} {s.Description} {(s.TeacherId is int t ? App.Db.Teachers.GetById(t)?.FullName : "")}";

    protected override Subject CreateNew() => new();

    protected override Form CreateDialog(Subject item, bool isNew, Action<Subject> save) =>
        new SubjectDialog(App, item, isNew, save);
}
