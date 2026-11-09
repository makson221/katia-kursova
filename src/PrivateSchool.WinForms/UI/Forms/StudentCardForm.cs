using System.ComponentModel;
using PrivateSchool.Core.Models;
using PrivateSchool.Core.Reports;
using PrivateSchool.Core.Services;
using PrivateSchool.WinForms.UI.Controls;

namespace PrivateSchool.WinForms.UI.Forms;

/// <summary>
/// Особова картка студента: дані, успішність, розрахунки.
/// </summary>
[DesignerCategory("Code")]
public sealed class StudentCardForm : Form
{
    public StudentCardForm(SchoolSystem app, Student student)
    {
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoScaleDimensions = new SizeF(96F, 96F);
        Text = $"Картка студента — {student.FullName}";
        Font = Theme.BaseFont;
        BackColor = Theme.Background;
        ClientSize = new Size(900, 620);
        MinimumSize = new Size(760, 520);
        StartPosition = FormStartPosition.CenterParent;
        ShowInTaskbar = false;
        MinimizeBox = false;

        var group = app.Db.Groups.GetById(student.GroupId);
        var showFinance = app.Auth.Can(Permission.ViewPayments);

        // Шапка
        var header = new Panel { Dock = DockStyle.Top, Height = 110, BackColor = Theme.Surface, Padding = new Padding(24, 16, 24, 12) };
        var name = new Label { Text = student.FullName, Font = Theme.TitleFont, AutoSize = true, Location = new Point(24, 14) };
        var info = new Label
        {
            Text = $"Група {group?.Name} · {group?.Specialty} · {group?.CourseNumber} курс\n" +
                   $"Статус: {student.Status.ToDisplay()} · Договір № {student.ContractNumber} від {student.EnrollmentDate:dd.MM.yyyy}",
            ForeColor = Theme.TextMuted,
            AutoSize = true,
            Location = new Point(26, 52)
        };
        header.Controls.Add(name);
        header.Controls.Add(info);

        var tabs = new TabControl { Dock = DockStyle.Fill, Padding = new Point(16, 6), Font = Theme.BaseFont };

        // Успішність
        var gradesTab = new TabPage("Успішність") { BackColor = Theme.Surface, Padding = new Padding(8) };
        var grades = new StyledGrid { Dock = DockStyle.Fill };
        grades.AddColumn("Дата", 70, ColumnAlign.Center, "dd.MM.yyyy");
        grades.AddColumn("Дисципліна", 200);
        grades.AddColumn("Вид контролю", 110);
        grades.AddColumn("Бали", 50, ColumnAlign.Center);
        grades.AddColumn("ЄКТС", 50, ColumnAlign.Center);
        grades.AddColumn("Оцінка", 90, ColumnAlign.Center);
        var studentGrades = app.Db.Grades.Find(g => g.StudentId == student.Id).OrderByDescending(g => g.Date).ToList();
        foreach (var g in studentGrades)
        {
            var row = grades.Rows[grades.Rows.Add(g.Date, app.Db.Subjects.GetById(g.SubjectId)?.Name, g.Type.ToDisplay(),
                g.Score, g.Ects, GradeScale.ToNationalText(g.Score))];
            if (!g.IsPassed) row.DefaultCellStyle.ForeColor = Theme.Danger;
        }
        var avg = app.Academic.GetAverage(student.Id);
        var bySubject = studentGrades.GroupBy(g => g.SubjectId)
            .Select(x => $"{app.Db.Subjects.GetById(x.Key)?.Name}: {x.Average(g => g.Score):0.0}");
        gradesTab.Controls.Add(grades);
        gradesTab.Controls.Add(SummaryLabel(
            $"Середній бал: {avg?.ToString("0.0") ?? "—"}" +
            (avg.HasValue ? $" ({GradeScale.ToEcts((int)Math.Round(avg.Value))})" : "") +
            $"   ·   Незадовільних: {studentGrades.Count(g => !g.IsPassed)}\n" + string.Join("   ·   ", bySubject)));
        tabs.TabPages.Add(gradesTab);

        // Оплати
        if (showFinance)
        {
            var payTab = new TabPage("Розрахунки") { BackColor = Theme.Surface, Padding = new Padding(8) };
            var pays = new StyledGrid { Dock = DockStyle.Fill };
            pays.AddColumn("Дата", 70, ColumnAlign.Center, "dd.MM.yyyy");
            pays.AddColumn("Квитанція", 100);
            pays.AddColumn("Сума, грн", 80, ColumnAlign.Right, "N2");
            pays.AddColumn("Спосіб", 110);
            pays.AddColumn("Призначення", 220);
            foreach (var p in app.Db.Payments.Find(p => p.StudentId == student.Id).OrderByDescending(p => p.Date))
                pays.Rows.Add(p.Date, p.ReceiptNumber, p.Amount, p.Method.ToDisplay(), p.Purpose);

            var b = app.Finance.GetBalance(student);
            var summary = SummaryLabel(
                $"Вартість: {b.MonthlyFee:N2} грн/міс. (знижка {student.DiscountPercent:0.#} %)   ·   Оплачуваних місяців: {b.BilledMonths}\n" +
                $"Нараховано: {b.Accrued:N2} грн   ·   Сплачено: {b.Paid:N2} грн   ·   " +
                (b.HasDebt ? $"ЗАБОРГОВАНІСТЬ: {b.Debt:N2} грн" : $"Сальдо: +{b.Balance:N2} грн"));
            summary.ForeColor = b.HasDebt ? Theme.Danger : Theme.Success;
            payTab.Controls.Add(pays);
            payTab.Controls.Add(summary);
            tabs.TabPages.Add(payTab);
        }

        // Особисті дані
        var personal = new TabPage("Особисті дані") { BackColor = Theme.Surface, Padding = new Padding(20) };
        var table = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, AutoScroll = true };
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        void Row(string label, string value)
        {
            table.Controls.Add(new Label { Text = label, ForeColor = Theme.TextMuted, AutoSize = true, Margin = new Padding(0, 6, 0, 6) });
            table.Controls.Add(new Label { Text = value.Length == 0 ? "—" : value, AutoSize = true, Font = Theme.BoldFont, Margin = new Padding(0, 6, 0, 6) });
        }
        Row("Дата народження", $"{student.BirthDate:dd.MM.yyyy} ({student.Age} р.)");
        Row("Телефон", student.Phone);
        Row("Електронна пошта", student.Email);
        Row("Дата зарахування", student.EnrollmentDate.ToString("dd.MM.yyyy"));
        Row("Дата завершення", student.EndDate?.ToString("dd.MM.yyyy") ?? "");
        Row("Знижка", $"{student.DiscountPercent:0.#} %");
        Row("Батьки / представник", student.ParentName);
        Row("Телефон батьків", student.ParentPhone);
        if (group?.CuratorId is int curatorId) Row("Куратор групи", app.Db.Teachers.GetById(curatorId)?.FullName ?? "");
        personal.Controls.Add(table);
        tabs.TabPages.Add(personal);

        var footer = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 56, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(16, 10, 16, 10), BackColor = Theme.Background };
        var close = new FlatButton("Закрити", ButtonVariant.Primary) { DialogResult = DialogResult.OK };
        footer.Controls.Add(close);
        CancelButton = close;

        var body = new Panel { Dock = DockStyle.Fill, Padding = new Padding(16, 12, 16, 0) };
        body.Controls.Add(tabs);

        Controls.Add(body);
        Controls.Add(footer);
        Controls.Add(header);
    }

    private static Label SummaryLabel(string text) => new()
    {
        Text = text,
        Dock = DockStyle.Bottom,
        Height = 60,
        Padding = new Padding(10, 6, 10, 6),
        BackColor = Theme.SurfaceAlt,
        Font = Theme.BoldFont
    };
}
