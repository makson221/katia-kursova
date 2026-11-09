using PrivateSchool.Core.Data;
using PrivateSchool.Core.Models;
using PrivateSchool.Core.Reports;
using PrivateSchool.Core.Services;
using PrivateSchool.WinForms.UI.Controls;
using PrivateSchool.WinForms.UI.Dialogs;

namespace PrivateSchool.WinForms.UI.Views;

public sealed class TeachersView : CrudView<Teacher>
{
    public TeachersView(SchoolSystem app) : base(app) { }

    public override string Title => "Викладачі";
    public override string Subtitle => "Кадровий склад, кваліфікація та тижневе навантаження";

    protected override Repository<Teacher> Repository => App.Db.Teachers;
    protected override string EntityName => "викладача";
    protected override bool CanModify => App.Auth.Can(Permission.ManageTeachers);

    protected override void ConfigureColumns(StyledGrid grid)
    {
        grid.AddColumn("ПІБ", 200, minWidth: 180);
        grid.AddColumn("Посада", 110);
        grid.AddColumn("Науковий ступінь", 150);
        grid.AddColumn("Спеціалізація", 120);
        grid.AddColumn("Стаж, р.", 60, ColumnAlign.Center);
        grid.AddColumn("Ставка, грн/год", 80, ColumnAlign.Right, "N2");
        grid.AddColumn("Год/тижд.", 70, ColumnAlign.Center);
        grid.AddColumn("Телефон", 100);
        grid.AddColumn("E-mail", 160);
    }

    protected override object?[] ToRow(Teacher t) => new object?[]
    {
        t.FullName,
        t.Position,
        t.AcademicDegree.Length > 0 ? t.AcademicDegree : "—",
        t.Specialization,
        t.ExperienceYears,
        t.HourlyRate,
        App.Schedule.GetWeeklyHours(t.Id),
        t.Phone,
        t.Email
    };

    protected override string GetSearchText(Teacher t) => $"{t.FullName} {t.Position} {t.Specialization} {t.Email} {t.Phone}";

    protected override Teacher CreateNew() => new() { BirthDate = DateTime.Today.AddYears(-35), HireDate = DateTime.Today, HourlyRate = 300 };

    protected override Form CreateDialog(Teacher item, bool isNew, Action<Teacher> save) =>
        new TeacherDialog(App, item, isNew, save);
}
