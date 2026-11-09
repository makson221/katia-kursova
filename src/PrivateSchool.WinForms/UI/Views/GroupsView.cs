using PrivateSchool.Core.Data;
using PrivateSchool.Core.Models;
using PrivateSchool.Core.Reports;
using PrivateSchool.Core.Services;
using PrivateSchool.WinForms.UI.Controls;
using PrivateSchool.WinForms.UI.Dialogs;

namespace PrivateSchool.WinForms.UI.Views;

public sealed class GroupsView : CrudView<Group>
{
    public GroupsView(SchoolSystem app) : base(app) { }

    public override string Title => "Групи";
    public override string Subtitle => "Академічні групи, вартість навчання та куратори";

    protected override Repository<Group> Repository => App.Db.Groups;
    protected override string EntityName => "групи";
    protected override bool CanModify => App.Auth.Can(Permission.ManageGroups);

    protected override void ConfigureColumns(StyledGrid grid)
    {
        grid.AddColumn("Група", 70, ColumnAlign.Center);
        grid.AddColumn("Спеціальність", 200);
        grid.AddColumn("Курс", 50, ColumnAlign.Center);
        grid.AddColumn("Рік набору", 70, ColumnAlign.Center);
        grid.AddColumn("Студентів / місць", 90, ColumnAlign.Center);
        grid.AddColumn("Вартість, грн/міс.", 100, ColumnAlign.Right, "N2");
        grid.AddColumn("Куратор", 170);
        grid.AddColumn("Сер. бал", 70, ColumnAlign.Center);
    }

    protected override object?[] ToRow(Group g)
    {
        var studying = App.Db.Students.Find(s => s.GroupId == g.Id && s.IsStudying).Count();
        var perf = App.Academic.GetGroupPerformance().FirstOrDefault(p => p.Group.Id == g.Id);
        return new object?[]
        {
            g.Name,
            g.Specialty,
            g.CourseNumber,
            g.StartYear,
            $"{studying} / {g.Capacity}",
            g.MonthlyFee,
            g.CuratorId is int c ? App.Db.Teachers.GetById(c)?.FullName : "—",
            perf?.Average?.ToString("0.0") ?? "—"
        };
    }

    protected override string GetSearchText(Group g) => $"{g.Name} {g.Specialty}";

    protected override Group CreateNew() => new() { StartYear = DateTime.Today.Month >= 6 ? DateTime.Today.Year : DateTime.Today.Year - 1 };

    protected override Form CreateDialog(Group item, bool isNew, Action<Group> save) =>
        new GroupDialog(App, item, isNew, save);
}
