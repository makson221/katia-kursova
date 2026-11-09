using System.Globalization;
using PrivateSchool.Core.Models;
using PrivateSchool.Core.Reports;
using PrivateSchool.Core.Services;
using PrivateSchool.WinForms.UI.Controls;

namespace PrivateSchool.WinForms.UI.Views;

/// <summary>
/// Головна панель: ключові показники, діаграма, розклад на сьогодні, боржники/рейтинг.
/// </summary>
public sealed class DashboardView : ViewBase
{
    private static readonly CultureInfo Uk = new("uk-UA");

    private readonly FlowLayoutPanel _cardsPanel;
    private readonly List<StatCard> _cards = new();
    private readonly StatCard _students;
    private readonly StatCard _teachers;
    private readonly StatCard _average;
    private readonly StatCard? _income;
    private readonly StatCard? _debt;
    private readonly StatCard? _lessonsToday;
    private readonly BarChart _chart;
    private readonly StyledGrid _today;
    private readonly StyledGrid _bottomGrid;
    private readonly Label _bottomTitle;

    public DashboardView(SchoolSystem app) : base(app)
    {
        _cardsPanel = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 128, WrapContents = false, BackColor = Theme.Background };

        _students = AddCard("Навчається студентів", Theme.Glyphs.People, Theme.Primary);
        _teachers = AddCard("Викладачів", Theme.Glyphs.Teacher, Theme.Violet);
        _average = AddCard("Середній бал", Theme.Glyphs.Star, Theme.Warning);
        if (ShowFinance)
        {
            _income = AddCard("Оплати за місяць", Theme.Glyphs.Money, Theme.Success);
            _debt = AddCard("Заборгованість", Theme.Glyphs.Warning, Theme.Danger);
        }
        else
        {
            _lessonsToday = AddCard("Занять сьогодні", Theme.Glyphs.Calendar, Theme.Teal);
        }
        _cardsPanel.Resize += (_, _) => LayoutCards();

        var body = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 2, BackColor = Theme.Background };
        body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        body.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        body.RowStyles.Add(new RowStyle(SizeType.Percent, 50));

        _chart = new BarChart { Dock = DockStyle.Fill, Margin = new Padding(0, 0, 8, 0) };
        if (ShowFinance)
        {
            _chart.Title = "Надходження коштів за останні 6 місяців, грн";
            _chart.BarColor = Theme.Success;
            _chart.ValueFormatter = v => v >= 1000 ? $"{v / 1000:0.#} тис." : v.ToString("N0");
        }
        else
        {
            _chart.Title = "Середній бал за групами";
            _chart.ValueFormatter = v => v.ToString("0.0");
            _chart.Threshold = GradeScale.PassScore;
            _chart.HighlightLast = false;
        }
        var chartFrame = Framed(_chart, new Padding(0, 0, 8, 0));
        body.Controls.Add(chartFrame, 0, 0);
        body.SetRowSpan(chartFrame, 2);

        _today = CompactGrid();
        _today.AddColumn("№", 30, ColumnAlign.Center, minWidth: 30);
        _today.AddColumn("Час", 95, ColumnAlign.Center, minWidth: 95);
        _today.AddColumn("Група", 55, ColumnAlign.Center);
        _today.AddColumn("Дисципліна", 170);
        _today.AddColumn(App.IsTeacher() ? "Тип" : "Викладач", 110);
        _today.AddColumn("Ауд.", 45, ColumnAlign.Center, minWidth: 45);
        var todayTitle = $"Розклад на сьогодні — {DateTime.Today.ToString("dddd, d MMMM", Uk)}";
        body.Controls.Add(Framed(_today, new Padding(8, 0, 0, 8), TitleLabel(todayTitle)), 1, 0);

        _bottomGrid = CompactGrid();
        if (ShowFinance)
        {
            _bottomGrid.AddColumn("Студент", 190);
            _bottomGrid.AddColumn("Група", 60, ColumnAlign.Center);
            _bottomGrid.AddColumn("Борг, грн", 90, ColumnAlign.Right, "N2");
            _bottomGrid.AddColumn("Остання оплата", 100, ColumnAlign.Center);
        }
        else
        {
            _bottomGrid.AddColumn("№", 35, ColumnAlign.Center);
            _bottomGrid.AddColumn("Студент", 190);
            _bottomGrid.AddColumn("Група", 60, ColumnAlign.Center);
            _bottomGrid.AddColumn("Сер. бал", 70, ColumnAlign.Center, "0.0");
        }
        _bottomTitle = TitleLabel(ShowFinance ? "Найбільші боржники" : "Найкращі студенти");
        body.Controls.Add(Framed(_bottomGrid, new Padding(8, 8, 0, 0), _bottomTitle), 1, 1);

        var spacer = new Panel { Dock = DockStyle.Top, Height = 4 };
        Content.Controls.Add(body);
        Content.Controls.Add(spacer);
        Content.Controls.Add(_cardsPanel);
    }

    public override string Title => "Головна";
    public override string Subtitle =>
        $"Вітаємо, {App.Auth.CurrentUser?.DisplayName}!  Сьогодні {DateTime.Today.ToString("d MMMM yyyy", Uk)} р.";

    private bool ShowFinance => App.Auth.Can(Permission.ViewPayments);

    private StatCard AddCard(string title, string glyph, Color accent)
    {
        var card = new StatCard(title, glyph, accent);
        _cards.Add(card);
        _cardsPanel.Controls.Add(card);
        return card;
    }

    private void LayoutCards()
    {
        if (_cards.Count == 0) return;
        const int gap = 16;
        var width = (_cardsPanel.ClientSize.Width - gap * (_cards.Count - 1)) / _cards.Count;
        for (var i = 0; i < _cards.Count; i++)
        {
            _cards[i].Width = Math.Max(160, width);
            _cards[i].Margin = new Padding(0, 0, i == _cards.Count - 1 ? 0 : gap, 16);
        }
    }

    /// <summary>Обгортає елемент у білу картку із заголовком.</summary>
    private static Panel Framed(Control inner, Padding margin, Label? title = null)
    {
        var outer = new Panel { Dock = DockStyle.Fill, Margin = margin, BackColor = Theme.Background, Padding = Padding.Empty };
        var card = new Panel { Dock = DockStyle.Fill, BackColor = Theme.Surface, Padding = new Padding(1) };
        card.Paint += (_, e) =>
        {
            using var pen = new Pen(Theme.Border);
            e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
        };
        card.Controls.Add(inner);
        outer.Controls.Add(card);
        if (title != null) card.Controls.Add(title);
        return outer;
    }

    private static StyledGrid CompactGrid()
    {
        var grid = new StyledGrid { Dock = DockStyle.Fill };
        grid.DefaultCellStyle.Font = Theme.SmallFont;
        grid.DefaultCellStyle.Padding = new Padding(4, 0, 4, 0);
        grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(4, 0, 4, 0);
        grid.RowTemplate.Height = 32;
        grid.ColumnHeadersHeight = 34;
        return grid;
    }

    private static Label TitleLabel(string text) => new()
    {
        Text = text,
        Dock = DockStyle.Top,
        Height = 44,
        Font = Theme.SectionFont,
        Padding = new Padding(14, 0, 0, 0),
        TextAlign = ContentAlignment.MiddleLeft,
        BackColor = Theme.Surface
    };

    public override void RefreshData()
    {
        SetSubtitle(Subtitle);
        var db = App.Db;
        var studying = db.Students.Find(s => s.Status == StudentStatus.Active).ToList();

        _students.Value = studying.Count.ToString();
        _students.Caption = $"{db.Groups.Count} груп · академвідп.: {db.Students.Find(s => s.Status == StudentStatus.AcademicLeave).Count()}";
        _teachers.Value = db.Teachers.Count.ToString();
        _teachers.Caption = $"Пар на тиждень: {db.Lessons.Count}";

        var avg = App.Academic.GetOverallAverage();
        _average.Value = avg?.ToString("0.0") ?? "—";
        var grades = db.Grades.GetAll();
        _average.Caption = grades.Count == 0 ? "оцінок немає" : $"Успішність: {100.0 * grades.Count(g => g.IsPassed) / grades.Count:0.#} %";

        var today = DateTime.Today;
        if (_income != null && _debt != null)
        {
            var monthStart = new DateTime(today.Year, today.Month, 1);
            _income.Value = $"{App.Finance.GetIncome(monthStart, today):N0} ₴";
            var prev = App.Finance.GetIncome(monthStart.AddMonths(-1), monthStart.AddDays(-1));
            _income.Caption = $"Мин. місяць: {prev:N0} ₴";

            var debtors = App.Finance.GetDebtors();
            _debt.Value = $"{debtors.Sum(d => d.Debt):N0} ₴";
            _debt.Caption = $"Боржників: {debtors.Count}";

            _chart.Items = App.Finance.GetMonthlyIncome(6)
                .Select(m => (m.Month.ToString("MMM yy", Uk), (double)m.Amount)).ToList();

            _bottomGrid.Rows.Clear();
            foreach (var d in debtors.Take(8))
                _bottomGrid.Rows.Add(d.Student.FullName, d.GroupName, d.Debt, d.LastPaymentDate?.ToString("dd.MM.yyyy") ?? "немає");
            _bottomTitle.Text = $"Найбільші боржники (усього {debtors.Count})";
        }
        else
        {
            _chart.Items = App.Academic.GetGroupPerformance()
                .Where(p => p.Average.HasValue)
                .OrderBy(p => p.Group.Name)
                .Select(p => (p.Group.Name, p.Average!.Value)).ToList();

            var best = db.Groups.GetAll()
                .SelectMany(g => App.Academic.GetGroupRating(g.Id))
                .Where(r => r.GradesCount > 0)
                .OrderByDescending(r => r.Average)
                .Take(8)
                .ToList();
            _bottomGrid.Rows.Clear();
            var i = 1;
            foreach (var r in best)
                _bottomGrid.Rows.Add(i++, r.Student.FullName, db.Groups.GetById(r.Student.GroupId)?.Name, r.Average);
        }

        var lessons = App.Schedule.GetForDay(today.DayOfWeek);
        if (App.IsTeacher())
            lessons = lessons.Where(l => l.TeacherId == App.Auth.CurrentUser!.TeacherId).ToList();
        if (_lessonsToday != null)
        {
            _lessonsToday.Value = lessons.Count.ToString();
            _lessonsToday.Caption = today.DayOfWeek == DayOfWeek.Sunday ? "Неділя — вихідний" : LessonTimes.DayName(today.DayOfWeek);
        }

        _today.Rows.Clear();
        foreach (var l in lessons)
        {
            _today.Rows.Add(l.Number, l.Time, db.Groups.GetById(l.GroupId)?.Name,
                db.Subjects.GetById(l.SubjectId)?.Name,
                App.IsTeacher() ? l.Type.ToDisplay() : db.Teachers.GetById(l.TeacherId)?.ShortName,
                l.Room);
        }
        if (lessons.Count == 0) _today.Rows.Add("", "", "", "Занять немає", "", "");
    }
}
