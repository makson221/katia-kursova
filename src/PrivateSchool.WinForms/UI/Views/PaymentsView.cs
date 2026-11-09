using PrivateSchool.Core.Data;
using PrivateSchool.Core.Models;
using PrivateSchool.Core.Reports;
using PrivateSchool.Core.Services;
using PrivateSchool.WinForms.UI.Controls;
using PrivateSchool.WinForms.UI.Dialogs;

namespace PrivateSchool.WinForms.UI.Views;

public sealed class PaymentsView : CrudView<Payment>
{
    private ComboBox _group = null!;
    private DateTimePicker _from = null!;
    private DateTimePicker _to = null!;

    public PaymentsView(SchoolSystem app) : base(app) { }

    public override string Title => "Оплати";
    public override string Subtitle => "Реєстр надходжень за договорами про навчання";

    protected override Repository<Payment> Repository => App.Db.Payments;
    protected override string EntityName => "оплати";
    protected override bool CanModify => App.Auth.Can(Permission.ManagePayments);

    protected override void ConfigureColumns(StyledGrid grid)
    {
        grid.AddColumn("Дата", 70, ColumnAlign.Center, "dd.MM.yyyy");
        grid.AddColumn("Квитанція", 95);
        grid.AddColumn("Студент", 190, minWidth: 160);
        grid.AddColumn("Група", 55, ColumnAlign.Center);
        grid.AddColumn("Сума, грн", 80, ColumnAlign.Right, "N2");
        grid.AddColumn("Спосіб оплати", 110);
        grid.AddColumn("Призначення", 220);
    }

    protected override void BuildFilters()
    {
        _group = AddFilterCombo("Група:", 100);
        Lookup.Fill(_group, Lookup.Groups(App.Db), null, "Усі групи");
        var today = DateTime.Today;
        var academicStart = new DateTime(today.Month >= 9 ? today.Year : today.Year - 1, 9, 1);
        _from = AddFilterDate("з", academicStart);
        _to = AddFilterDate("по", today);
    }

    public override void OnActivated()
    {
        Lookup.Fill(_group, Lookup.Groups(App.Db), Lookup.SelectedId(_group), "Усі групи");
        if (_to.Value.Date < DateTime.Today && _to.Value.Date == DateTime.Today.AddDays(-1)) _to.Value = DateTime.Today;
        base.OnActivated();
    }

    protected override IEnumerable<Payment> GetItems() =>
        Repository.GetAll().OrderByDescending(p => p.Date).ThenByDescending(p => p.Id);

    protected override bool PassesFilters(Payment p)
    {
        if (p.Date.Date < _from.Value.Date || p.Date.Date > _to.Value.Date) return false;
        if (Lookup.SelectedId(_group) is int g && App.Db.Students.GetById(p.StudentId)?.GroupId != g) return false;
        return true;
    }

    protected override object?[] ToRow(Payment p)
    {
        var student = App.Db.Students.GetById(p.StudentId);
        return new object?[]
        {
            p.Date,
            p.ReceiptNumber,
            student?.FullName ?? "—",
            student == null ? "—" : App.Db.Groups.GetById(student.GroupId)?.Name,
            p.Amount,
            p.Method.ToDisplay(),
            p.Purpose
        };
    }

    protected override string GetSearchText(Payment p) =>
        $"{App.Db.Students.GetById(p.StudentId)?.FullName} {p.ReceiptNumber} {p.Purpose}";

    protected override Payment CreateNew() => new() { Date = DateTime.Today };

    protected override Form CreateDialog(Payment item, bool isNew, Action<Payment> save) =>
        new PaymentDialog(App, item, isNew, save);

    protected override string GetFooterText(IReadOnlyList<Payment> shown) =>
        $"Оплат за період: {shown.Count}  ·  Сума надходжень: {shown.Sum(p => p.Amount):N2} грн";
}
