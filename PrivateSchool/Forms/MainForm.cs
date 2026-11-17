using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PrivateSchool.Controls;
using PrivateSchool.Data;
using PrivateSchool.Models;
using PrivateSchool.Reports;
using PrivateSchool.Services;
using PrivateSchool.Utils;

namespace PrivateSchool.Forms
{
    /// <summary>
    /// Головна форма програми: бічна панель навігації, заголовок сторінки
    /// та сторінки для роботи з усіма сутностями, головна панель показників і звіти.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>Опис сторінки, на яку веде пункт бічного меню.</summary>
        private sealed class PageInfo
        {
            public PageInfo(Panel page, string title, string subtitle)
            {
                Page = page;
                Title = title;
                Subtitle = subtitle;
            }

            public Panel Page { get; private set; }
            public string Title { get; private set; }
            public string Subtitle { get; private set; }
        }

        private readonly SchoolService _service;
        private readonly User _user;
        private readonly Dictionary<SidebarButton, PageInfo> _pages = new Dictionary<SidebarButton, PageInfo>();
        private SidebarButton _currentNav;
        private ReportResult _lastReport;

        public MainForm(SchoolService service, User user)
        {
            InitializeComponent();
            _service = service;
            _user = user;

            Text = service.SchoolName + " — автоматизована система управління";
            lblUserName.Text = user.FullName;
            lblUserRole.Text = EnumHelper.ToText(user.Role) + " · " + user.Login;

            ApplyTheme();
            RegisterPages();

            // Керування користувачами доступне лише адміністратору
            navUsers.Visible = user.IsAdministrator;

            WireLists();
            cmbReport.Items.AddRange(ReportCatalog.GetAll().Cast<object>().ToArray());

            _service.DataChanged += OnDataChanged;
            RefreshAll();
            ShowPage(navDashboard);
        }

        /// <summary>Користувач обрав «Змінити користувача».</summary>
        public bool LogoutRequested { get; private set; }

        #region Оформлення та навігація

        private void ApplyTheme()
        {
            AppTheme.StyleGrid(gridToday);
            AppTheme.StyleGrid(gridReport);
            gridToday.DataBindingComplete += (s, e) => UiHelper.FormatColumns(gridToday);
            gridReport.DataBindingComplete += (s, e) => UiHelper.FormatColumns(gridReport);

            AppTheme.StyleButton(btnHeaderExport, ButtonKind.Secondary);
            AppTheme.StyleButton(btnAbout, ButtonKind.Ghost);
            AppTheme.StyleButton(btnBuild, ButtonKind.Primary);
            AppTheme.StyleButton(btnExport, ButtonKind.Secondary);

            SetupCard(card1, "Учні", AppTheme.IconStudents);
            SetupCard(card2, "Викладачі", AppTheme.IconTeachers);
            SetupCard(card3, "Навчальні групи", AppTheme.IconGroups);
            SetupCard(card4, "Курси", AppTheme.IconCourses);
            SetupCard(card5, "Надходження", AppTheme.IconMoney);
            SetupCard(card6, "Заборгованість", AppTheme.IconWarning);
            SetupCard(card7, "Середній бал", AppTheme.IconGrades);
            SetupCard(card8, "Занять на тиждень", AppTheme.IconSchedule);
        }

        private static void SetupCard(StatCard card, string title, string icon)
        {
            card.Title = title;
            card.IconGlyph = icon;
        }

        private void RegisterPages()
        {
            _pages.Add(navDashboard, new PageInfo(pageDashboard, "Головна", null));
            _pages.Add(navStudents, new PageInfo(pageStudents, "Учні", "Облік учнів, договорів та заборгованості"));
            _pages.Add(navTeachers, new PageInfo(pageTeachers, "Викладачі", "Персонал, кваліфікація та тижневе навантаження"));
            _pages.Add(navCourses, new PageInfo(pageCourses, "Курси", "Освітні програми та вартість навчання"));
            _pages.Add(navGroups, new PageInfo(pageGroups, "Групи", "Навчальні групи, викладачі та наповненість"));
            _pages.Add(navSchedule, new PageInfo(pageSchedule, "Розклад", "Щотижневий розклад занять з автоматичним контролем накладок"));
            _pages.Add(navPayments, new PageInfo(pagePayments, "Оплати", "Надходження коштів за навчання"));
            _pages.Add(navGrades, new PageInfo(pageGrades, "Успішність", "Журнал оцінок за 12-бальною шкалою"));
            _pages.Add(navReports, new PageInfo(pageReports, "Звіти", "Аналітика закладу з експортом у CSV"));
            _pages.Add(navUsers, new PageInfo(pageUsers, "Користувачі", "Облікові записи та права доступу"));
        }

        private void nav_Click(object sender, EventArgs e)
        {
            ShowPage((SidebarButton)sender);
        }

        /// <summary>Показує сторінку, на яку веде пункт меню, і приховує решту.</summary>
        private void ShowPage(SidebarButton nav)
        {
            PageInfo info = _pages[nav];
            _currentNav = nav;

            info.Page.Visible = true;
            info.Page.BringToFront();
            foreach (KeyValuePair<SidebarButton, PageInfo> pair in _pages)
            {
                pair.Key.Selected = pair.Key == nav;
                if (pair.Value.Page != info.Page)
                    pair.Value.Page.Visible = false;
            }

            lblPageTitle.Text = info.Title;
            lblPageSubtitle.Text = nav == navDashboard ? GetGreeting() : info.Subtitle;

            if (info.Page == pageReports && cmbReport.SelectedIndex < 0 && cmbReport.Items.Count > 0)
                cmbReport.SelectedIndex = 0; // автоматично будує перший звіт
        }

        private string GetGreeting()
        {
            DateTime today = DateTime.Today;
            return string.Format("Вітаємо, {0}! Сьогодні {1}, {2:d MMMM yyyy} р.",
                _user.FullName, DayNames.ToText(today.DayOfWeek).ToLower(), today);
        }

        /// <summary>Список на поточній сторінці (або null для головної та звітів).</summary>
        private EntityListControl CurrentList
        {
            get
            {
                return _currentNav == null ? null
                    : _pages[_currentNav].Page.Controls.OfType<EntityListControl>().FirstOrDefault();
            }
        }

        /// <summary>Гарячі клавіші головного вікна.</summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Control | Keys.E:
                    btnHeaderExport_Click(this, EventArgs.Empty);
                    return true;
                case Keys.Control | Keys.S:
                    SaveData();
                    return true;
                case Keys.Control | Keys.F:
                    if (CurrentList != null)
                        CurrentList.FocusSearch();
                    return true;
                case Keys.F1:
                    btnAbout_Click(this, EventArgs.Empty);
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion


        #region Підключення подій списків

        private void WireLists()
        {
            // Учні
            listStudents.AddClicked += (s, e) => ShowEditor(new StudentForm(_service, null));
            listStudents.EditClicked += (s, e) => EditSelected(listStudents, _service.Students, x => new StudentForm(_service, x));
            listStudents.DeleteClicked += (s, e) => DeleteSelected(listStudents, _service.Students,
                x => string.Format("Видалити учня «{0}»?\nРазом з ним буде видалено всі його оцінки та оплати.", x.FullName),
                _service.DeleteStudent);
            listStudents.FilterChanged += (s, e) => RefreshStudents();
            listStudents.RowColorSelector = row =>
            {
                if ((string)row["Статус"] != EnumHelper.ToText(StudentStatus.Active)) return AppTheme.TextMuted;
                if ((decimal)row["Борг"] > 0) return AppTheme.Danger;
                return null;
            };

            // Викладачі
            listTeachers.AddClicked += (s, e) => ShowEditor(new TeacherForm(_service, null));
            listTeachers.EditClicked += (s, e) => EditSelected(listTeachers, _service.Teachers, x => new TeacherForm(_service, x));
            listTeachers.DeleteClicked += (s, e) => DeleteSelected(listTeachers, _service.Teachers,
                x => string.Format("Видалити викладача «{0}»?", x.FullName), _service.DeleteTeacher);
            listTeachers.FilterChanged += (s, e) => RefreshTeachers();

            // Курси
            listCourses.AddClicked += (s, e) => ShowEditor(new CourseForm(_service, null));
            listCourses.EditClicked += (s, e) => EditSelected(listCourses, _service.Courses, x => new CourseForm(_service, x));
            listCourses.DeleteClicked += (s, e) => DeleteSelected(listCourses, _service.Courses,
                x => string.Format("Видалити курс «{0}»?", x.Name), _service.DeleteCourse);
            listCourses.FilterChanged += (s, e) => RefreshCourses();

            // Групи
            listGroups.AddClicked += (s, e) => ShowEditor(new GroupForm(_service, null));
            listGroups.EditClicked += (s, e) => EditSelected(listGroups, _service.Groups, x => new GroupForm(_service, x));
            listGroups.DeleteClicked += (s, e) => DeleteSelected(listGroups, _service.Groups,
                x => string.Format("Видалити групу «{0}»?\nРазом з нею буде видалено її розклад та оцінки.", x.Name),
                _service.DeleteGroup);
            listGroups.FilterChanged += (s, e) => RefreshGroups();

            // Розклад
            listSchedule.AddClicked += (s, e) => ShowEditor(new LessonForm(_service, null, FilterId(listSchedule)));
            listSchedule.EditClicked += (s, e) => EditSelected(listSchedule, _service.Lessons, x => new LessonForm(_service, x, 0));
            listSchedule.DeleteClicked += (s, e) => DeleteSelected(listSchedule, _service.Lessons,
                x => string.Format("Видалити заняття групи «{0}» ({1}, {2})?",
                    _service.GetGroupName(x.GroupId), DayNames.ToText(x.Day), x.TimeRange), _service.DeleteLesson);
            listSchedule.FilterChanged += (s, e) => RefreshSchedule();

            // Оплати
            listPayments.AddClicked += (s, e) => ShowEditor(new PaymentForm(_service, null, SelectedStudentId()));
            listPayments.EditClicked += (s, e) => EditSelected(listPayments, _service.Payments, x => new PaymentForm(_service, x, 0));
            listPayments.DeleteClicked += (s, e) => DeleteSelected(listPayments, _service.Payments,
                x => string.Format("Видалити оплату {0:N2} грн від {1:dd.MM.yyyy} ({2})?",
                    x.Amount, x.Date, _service.GetStudentName(x.StudentId)), _service.DeletePayment);
            listPayments.FilterChanged += (s, e) => RefreshPayments();

            // Оцінки
            listGrades.AddClicked += (s, e) => ShowEditor(new GradeForm(_service, null, FilterId(listGrades)));
            listGrades.EditClicked += (s, e) => EditSelected(listGrades, _service.Grades, x => new GradeForm(_service, x, 0));
            listGrades.DeleteClicked += (s, e) => DeleteSelected(listGrades, _service.Grades,
                x => string.Format("Видалити оцінку {0} учня «{1}»?", x.Value, _service.GetStudentName(x.StudentId)),
                _service.DeleteGrade);
            listGrades.FilterChanged += (s, e) => RefreshGrades();
            listGrades.RowColorSelector = row =>
            {
                int value = (int)row["Оцінка"];
                if (value < 4) return AppTheme.Danger;
                if (value >= 10) return AppTheme.Success;
                return null;
            };

            // Користувачі
            listUsers.AddClicked += (s, e) => ShowEditor(new UserForm(_service, null));
            listUsers.EditClicked += (s, e) => EditSelected(listUsers, _service.Users, x => new UserForm(_service, x));
            listUsers.DeleteClicked += (s, e) => DeleteSelected(listUsers, _service.Users,
                x => string.Format("Видалити користувача «{0}»?", x), id => _service.DeleteUser(id, _user));
            listUsers.FilterChanged += (s, e) => RefreshUsers();
        }

        #endregion

        #region Узагальнені операції редагування

        private void ShowEditor(Form form)
        {
            using (form)
            {
                form.ShowDialog(this);
            }
            // Оновлення таблиць відбувається автоматично через подію SchoolService.DataChanged
        }

        private T GetSelected<T>(EntityListControl list, IRepository<T> repository) where T : EntityBase
        {
            int? id = list.SelectedId;
            T item = id.HasValue ? repository.GetById(id.Value) : null;
            if (item == null)
                UiHelper.ShowWarning(this, "Спочатку оберіть запис у таблиці.");
            return item;
        }

        private void EditSelected<T>(EntityListControl list, IRepository<T> repository, Func<T, Form> createForm)
            where T : EntityBase
        {
            T item = GetSelected(list, repository);
            if (item != null)
                ShowEditor(createForm(item));
        }

        private void DeleteSelected<T>(EntityListControl list, IRepository<T> repository,
            Func<T, string> question, Action<int> delete) where T : EntityBase
        {
            T item = GetSelected(list, repository);
            if (item == null || !UiHelper.Confirm(this, question(item)))
                return;
            try
            {
                delete(item.Id);
            }
            catch (ValidationException ex)
            {
                UiHelper.ShowWarning(this, ex.Message);
            }
        }

        private static int FilterId(EntityListControl list)
        {
            return list.FilterValue is int ? (int)list.FilterValue : 0;
        }

        /// <summary>Якщо на вкладці «Учні» вибрано учня — підставляємо його у нову оплату.</summary>
        private int SelectedStudentId()
        {
            return listStudents.SelectedId ?? 0;
        }

        private static bool TextMatch(string query, params string[] values)
        {
            if (string.IsNullOrWhiteSpace(query))
                return true;
            return values.Any(v => v != null && v.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) >= 0);
        }

        #endregion

        #region Оновлення даних

        private void OnDataChanged(object sender, EventArgs e)
        {
            RefreshAll();
        }

        private void RefreshAll()
        {
            RefreshFilters();
            RefreshDashboard();
            RefreshStudents();
            RefreshTeachers();
            RefreshCourses();
            RefreshGroups();
            RefreshSchedule();
            RefreshPayments();
            RefreshGrades();
            if (_user.IsAdministrator)
                RefreshUsers();
            if (_lastReport != null)
                BuildReport();
        }

        private void RefreshFilters()
        {
            List<ComboItem> groups = _service.Groups.GetAll().OrderBy(g => g.Name)
                .Select(g => new ComboItem(g.Id, g.Name)).ToList();

            listStudents.SetFilterItems(groups.Concat(new[] { new ComboItem(0, "— без групи —") }), "Усі групи");
            listSchedule.SetFilterItems(groups, "Усі групи");
            listGrades.SetFilterItems(groups, "Усі групи");

            CultureInfo culture = CultureInfo.CurrentCulture;
            IEnumerable<ComboItem> months = _service.Payments.GetAll()
                .Select(p => new DateTime(p.Date.Year, p.Date.Month, 1))
                .Distinct()
                .OrderByDescending(d => d)
                .Select(d => new ComboItem(d, culture.TextInfo.ToTitleCase(d.ToString("MMMM yyyy", culture))));
            listPayments.SetFilterItems(months, "Усі місяці");
        }

        private void RefreshStudents()
        {
            object filter = listStudents.FilterValue;
            var rows = _service.Students.Search(listStudents.SearchText)
                .Where(s => filter == null || s.GroupId == (int)filter)
                .OrderBy(s => s.FullName)
                .Select(s => new
                {
                    s.Id,
                    ПІБ = s.FullName,
                    Вік = s.Age,
                    Група = _service.GetGroupName(s.GroupId),
                    Договір = s.ContractNumber,
                    Статус = s.Status,
                    Знижка = s.Discount == 0 ? "—" : s.Discount.ToString("0.#") + " %",
                    Оплата_за_місяць = _service.GetMonthlyFee(s),
                    Борг = _service.GetDebt(s),
                    Телефон = s.Phone,
                    Контакт_батьків = (s.ParentName + " " + s.ParentPhone).Trim()
                }).ToList();

            decimal totalDebt = rows.Where(r => r.Борг > 0).Sum(r => r.Борг);
            listStudents.Bind(rows, string.Format("Заборгованість у вибірці: {0:N2} грн", totalDebt));
        }

        private void RefreshTeachers()
        {
            var rows = _service.Teachers.Search(listTeachers.SearchText)
                .OrderBy(t => t.FullName)
                .Select(t => new
                {
                    t.Id,
                    ПІБ = t.FullName,
                    Вік = t.Age,
                    Спеціалізація = t.Specialization,
                    Категорія = t.Category,
                    Стаж_років = t.ExperienceYears,
                    Ставка_грн_год = t.HourlyRate,
                    Групи = string.Join(", ", _service.GetGroupsOfTeacher(t.Id).Select(g => g.Name)),
                    Годин_на_тиждень = _service.GetTeacherWeeklyHours(t.Id),
                    Телефон = t.Phone,
                    Email = t.Email
                });
            listTeachers.Bind(rows);
        }

        private void RefreshCourses()
        {
            var rows = _service.Courses.Search(listCourses.SearchText)
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    c.Id,
                    Назва = c.Name,
                    Рівень = c.Level,
                    Ціна_за_місяць = c.PricePerMonth,
                    Тривалість_міс = c.DurationMonths,
                    Занять_на_тиждень = c.LessonsPerWeek,
                    Повна_вартість = c.TotalPrice,
                    Груп = _service.Groups.Find(g => g.CourseId == c.Id).Count(),
                    Опис = c.Description
                });
            listCourses.Bind(rows);
        }

        private void RefreshGroups()
        {
            string query = listGroups.SearchText;
            var rows = _service.Groups.GetAll()
                .Where(g => g.Matches(query) || TextMatch(query, _service.GetCourseName(g.CourseId), _service.GetTeacherName(g.TeacherId)))
                .OrderBy(g => g.Name)
                .Select(g =>
                {
                    double? avg = _service.GetGroupAverage(g.Id);
                    return new
                    {
                        g.Id,
                        Назва = g.Name,
                        Курс = _service.GetCourseName(g.CourseId),
                        Викладач = _service.GetTeacherName(g.TeacherId),
                        Дата_старту = g.StartDate,
                        Учнів = _service.GetActiveStudentCount(g.Id),
                        Місць = g.MaxStudents,
                        Аудиторія = g.Room,
                        Середній_бал = avg.HasValue ? avg.Value.ToString("0.00") : "—"
                    };
                });
            listGroups.Bind(rows);
        }

        private void RefreshSchedule()
        {
            int groupId = FilterId(listSchedule);
            string query = listSchedule.SearchText;
            var rows = _service.GetSchedule(l => groupId == 0 || l.GroupId == groupId)
                .Select(l =>
                {
                    Teacher teacher = _service.GetTeacherOfGroup(l.GroupId);
                    Course course = _service.GetCourseOfGroup(l.GroupId);
                    return new
                    {
                        l.Id,
                        День_тижня = DayNames.ToText(l.Day),
                        Час = l.TimeRange,
                        Група = _service.GetGroupName(l.GroupId),
                        Курс = course == null ? "—" : course.Name,
                        Викладач = teacher == null ? "—" : teacher.ShortName,
                        Аудиторія = l.Room,
                        Тривалість_хв = l.DurationMinutes
                    };
                })
                .Where(r => TextMatch(query, r.День_тижня, r.Група, r.Курс, r.Викладач, r.Аудиторія));
            listSchedule.Bind(rows);
        }

        private void RefreshPayments()
        {
            object month = listPayments.FilterValue;
            string query = listPayments.SearchText;
            var rows = _service.Payments.GetAll()
                .Where(p => month == null || (p.Date.Year == ((DateTime)month).Year && p.Date.Month == ((DateTime)month).Month))
                .Where(p => p.Matches(query) || TextMatch(query, _service.GetStudentName(p.StudentId)))
                .OrderByDescending(p => p.Date).ThenByDescending(p => p.Id)
                .Select(p =>
                {
                    Student student = _service.Students.GetById(p.StudentId);
                    return new
                    {
                        p.Id,
                        Дата = p.Date,
                        Учень = _service.GetStudentName(p.StudentId),
                        Група = student == null ? "—" : _service.GetGroupName(student.GroupId),
                        Сума_грн = p.Amount,
                        Спосіб_оплати = p.Method,
                        Коментар = p.Comment
                    };
                }).ToList();

            listPayments.Bind(rows, string.Format("Сума у вибірці: {0:N2} грн", rows.Sum(r => r.Сума_грн)));
        }

        private void RefreshGrades()
        {
            int groupId = FilterId(listGrades);
            string query = listGrades.SearchText;
            var rows = _service.Grades.GetAll()
                .Where(g => groupId == 0 || g.GroupId == groupId)
                .Where(g => g.Matches(query) || TextMatch(query, _service.GetStudentName(g.StudentId)))
                .OrderByDescending(g => g.Date).ThenBy(g => _service.GetStudentName(g.StudentId))
                .Select(g => new
                {
                    g.Id,
                    Дата = g.Date,
                    Учень = _service.GetStudentName(g.StudentId),
                    Група = _service.GetGroupName(g.GroupId),
                    Оцінка = g.Value,
                    Рівень = g.LevelName,
                    Вид = g.Kind,
                    Коментар = g.Comment
                }).ToList();

            string summary = rows.Count == 0 ? null
                : string.Format("Середній бал у вибірці: {0:0.00}", rows.Average(r => r.Оцінка));
            listGrades.Bind(rows, summary);
        }

        private void RefreshUsers()
        {
            var rows = _service.Users.Search(listUsers.SearchText)
                .OrderBy(u => u.Login)
                .Select(u => new
                {
                    u.Id,
                    Логін = u.Login,
                    ПІБ = u.FullName,
                    Роль = u.Role
                });
            listUsers.Bind(rows);
        }

        #endregion

        #region Головна панель

        private void RefreshDashboard()
        {
            DateTime today = DateTime.Today;
            DateTime monthStart = new DateTime(today.Year, today.Month, 1);
            CultureInfo culture = CultureInfo.CurrentCulture;

            List<Student> students = _service.Students.GetAll().ToList();
            List<decimal> debts = students.Select(s => _service.GetDebt(s)).Where(d => d > 0).ToList();
            List<Group> groups = _service.Groups.GetAll().ToList();
            int capacity = groups.Sum(g => g.MaxStudents);
            int filled = groups.Sum(g => _service.GetActiveStudentCount(g.Id));
            double weeklyHours = _service.Teachers.GetAll().Sum(t => _service.GetTeacherWeeklyHours(t.Id));
            int paymentsThisMonth = _service.Payments.Find(p => p.Date >= monthStart).Count();
            double? avg = _service.GetAverageGrade(g => true);
            int lessonsToday = _service.Lessons.Find(l => l.Day == today.DayOfWeek).Count();

            card1.SetData(students.Count(s => s.IsActive).ToString(),
                "навчаються · усього записів: " + students.Count, AppTheme.Primary);
            card2.SetData(_service.Teachers.Count.ToString(),
                string.Format("навантаження {0:0.#} год/тиждень", weeklyHours), AppTheme.Violet);
            card3.SetData(groups.Count.ToString(),
                string.Format("заповненість {0:0} %", capacity == 0 ? 0 : filled * 100.0 / capacity), AppTheme.Teal);
            card4.SetData(_service.Courses.Count.ToString(),
                _service.Courses.Count == 0 ? "курсів ще немає"
                    : string.Format("від {0:N0} грн на місяць", _service.Courses.GetAll().Min(c => c.PricePerMonth)),
                AppTheme.Warning);

            card5.Title = "Оплати за " + today.ToString("MMMM", culture);
            card5.SetData(_service.GetIncome(monthStart, today).ToString("N0") + " грн",
                "оплат цього місяця: " + paymentsThisMonth, AppTheme.Success);
            card6.SetData(debts.Sum().ToString("N0") + " грн",
                debts.Count == 0 ? "боржників немає" : "боржників: " + debts.Count,
                debts.Count == 0 ? AppTheme.Success : AppTheme.Danger);
            card7.SetData(avg.HasValue ? avg.Value.ToString("0.00") : "—",
                avg.HasValue ? "рівень: " + Grade.GetLevelName(avg.Value).ToLower() : "оцінок ще немає", AppTheme.Violet);
            card8.SetData(_service.Lessons.Count.ToString(),
                lessonsToday == 0 ? "сьогодні занять немає" : "сьогодні: " + lessonsToday, AppTheme.Primary);

            // Діаграма надходжень за останні 6 місяців
            List<ChartItem> chart = Enumerable.Range(0, 6)
                .Select(i => monthStart.AddMonths(i - 5))
                .Select(m => new ChartItem(
                    culture.TextInfo.ToTitleCase(culture.DateTimeFormat.GetAbbreviatedMonthName(m.Month).TrimEnd('.')),
                    _service.GetIncome(m, m.AddMonths(1).AddDays(-1))))
                .ToList();
            chartIncome.SetData(chart);
            lblChartSubtitle.Text = string.Format("Оплати за останні 6 місяців · разом {0:N0} грн", chart.Sum(c => c.Value));

            // Розклад на сьогодні
            var lessons = _service.GetSchedule(l => l.Day == today.DayOfWeek)
                .Select(l =>
                {
                    Teacher teacher = _service.GetTeacherOfGroup(l.GroupId);
                    return new
                    {
                        Час = l.TimeRange,
                        Група = _service.GetGroupName(l.GroupId),
                        Викладач = teacher == null ? "—" : teacher.ShortName,
                        Аудиторія = l.Room
                    };
                }).ToList();

            lblTodaySubtitle.Text = string.Format("{0}, {1:d MMMM} · {2}", DayNames.ToText(today.DayOfWeek), today,
                lessons.Count == 0 ? "занять немає" : "занять: " + lessons.Count);
            gridToday.DataSource = UiHelper.ToDataTable(lessons);

            if (_currentNav == navDashboard)
                lblPageSubtitle.Text = GetGreeting();
        }

        #endregion

        #region Звіти

        private void btnBuild_Click(object sender, EventArgs e)
        {
            BuildReport();
        }

        private void BuildReport()
        {
            ReportBase report = cmbReport.SelectedItem as ReportBase;
            if (report == null)
                return;
            _lastReport = report.Build(_service);
            gridReport.DataSource = _lastReport.Table;
            lblSummary.Text = _lastReport.Summary;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (_lastReport == null)
                BuildReport();
            if (_lastReport != null)
                ExportTable(_lastReport.Table.DefaultView.ToTable(), _lastReport.Title);
        }

        #endregion

        #region Дії заголовка та події форми

        private void ExportTable(DataTable table, string name)
        {
            if (table == null || table.Rows.Count == 0)
            {
                UiHelper.ShowWarning(this, "Немає даних для експорту.");
                return;
            }

            // Приховану службову колонку «Id» у файл не виводимо
            DataTable export = table.Copy();
            if (export.Columns.Contains(EntityListControl.IdColumn))
                export.Columns.Remove(EntityListControl.IdColumn);

            string fileName = new string((name + " " + DateTime.Today.ToString("yyyy-MM-dd"))
                .Where(c => !Path.GetInvalidFileNameChars().Contains(c)).ToArray());
            saveFileDialog.FileName = fileName + ".csv";
            if (saveFileDialog.ShowDialog(this) != DialogResult.OK)
                return;

            try
            {
                CsvExporter.Export(export, saveFileDialog.FileName);
                UiHelper.ShowInfo(this, "Дані експортовано у файл:\n" + saveFileDialog.FileName);
            }
            catch (IOException ex)
            {
                UiHelper.ShowError(this, "Не вдалося зберегти файл: " + ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                UiHelper.ShowError(this, "Немає доступу до файлу: " + ex.Message);
            }
        }

        private void btnHeaderExport_Click(object sender, EventArgs e)
        {
            PageInfo info = _pages[_currentNav];
            if (info.Page == pageReports)
            {
                btnExport_Click(sender, e);
                return;
            }

            DataGridView grid = info.Page == pageDashboard
                ? gridToday
                : CurrentList == null ? null : CurrentList.Grid;
            DataTable table = grid == null ? null : grid.DataSource as DataTable;
            ExportTable(table == null ? null : table.DefaultView.ToTable(),
                info.Page == pageDashboard ? "Розклад на сьогодні" : info.Title);
        }

        private void SaveData()
        {
            try
            {
                _service.Save();
                UiHelper.ShowInfo(this, "Дані збережено у файл:\n" + _service.StorageLocation);
            }
            catch (Exception ex)
            {
                UiHelper.ShowError(this, "Не вдалося зберегти дані: " + ex.Message);
            }
        }

        private void navLogout_Click(object sender, EventArgs e)
        {
            LogoutRequested = true;
            Close();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this,
                "Автоматизована система приватного навчального закладу\n" +
                "Версія 1.1\n\n" +
                "Курсова робота з дисципліни «Об'єктно-орієнтоване програмування».\n\n" +
                "Можливості: облік учнів, викладачів, курсів і груп; розклад занять із контролем накладок; " +
                "облік оплат і заборгованості; журнал успішності; звіти з експортом у CSV; " +
                "розмежування прав користувачів.\n\n" +
                "Гарячі клавіші: Ctrl+F — пошук, Ctrl+E — експорт у CSV, Ctrl+S — зберегти, " +
                "Insert / Enter / Delete — додати, редагувати, видалити запис.\n\n" +
                "Технології: C#, .NET, Windows Forms, GDI+, XML-серіалізація.\n" +
                "Файл даних: " + _service.StorageLocation,
                "Про програму", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>Логотип у бічній панелі: синій квадрат із заокругленими кутами та символом закладу.</summary>
        private void panelBrand_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            float k = AppTheme.ScaleOf(g);
            Rectangle logo = new Rectangle((int)(20 * k), (int)(20 * k), (int)(40 * k), (int)(40 * k));
            using (System.Drawing.Drawing2D.GraphicsPath path = AppTheme.RoundedRect(logo, 10 * k))
            using (System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                       logo, Color.FromArgb(59, 130, 246), AppTheme.PrimaryDark, 45F))
                g.FillPath(brush, path);
            AppTheme.DrawLogoMark(g, logo, 14F);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!LogoutRequested && e.CloseReason == CloseReason.UserClosing
                && !UiHelper.Confirm(this, "Завершити роботу з програмою?"))
            {
                e.Cancel = true;
                return;
            }

            try
            {
                _service.Save();
            }
            catch (Exception ex)
            {
                UiHelper.ShowError(this, "Не вдалося зберегти дані: " + ex.Message);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // Відписуємося, щоб закрита форма не отримувала сповіщень після зміни користувача
            _service.DataChanged -= OnDataChanged;
            base.OnFormClosed(e);
        }

        #endregion
    }
}
