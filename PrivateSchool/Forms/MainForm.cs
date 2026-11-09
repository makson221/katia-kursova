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
    /// Головна форма програми. Містить вкладки для роботи з усіма сутностями,
    /// панель показників і звіти.
    /// </summary>
    public partial class MainForm : Form
    {
        private static readonly Color DebtColor = Color.FromArgb(190, 30, 30);
        private static readonly Color InactiveColor = Color.Gray;
        private static readonly Color GoodColor = Color.FromArgb(0, 120, 60);
        private static readonly Color AccentColor = Color.FromArgb(0, 84, 153);

        private readonly SchoolService _service;
        private readonly User _user;
        private ReportResult _lastReport;

        public MainForm(SchoolService service, User user)
        {
            InitializeComponent();
            _service = service;
            _user = user;

            Text = service.SchoolName + " — автоматизована система управління";
            lblStatusUser.Text = string.Format("Користувач: {0} [{1}]", user.FullName, EnumHelper.ToText(user.Role));
            lblStatusFile.Text = "Файл даних: " + service.StorageLocation;
            lblStatusDate.Text = DateTime.Today.ToString("D");

            UiHelper.StyleGrid(gridToday);
            UiHelper.StyleGrid(gridReport);
            gridToday.DataBindingComplete += (s, e) => UiHelper.FormatColumns(gridToday);
            gridReport.DataBindingComplete += (s, e) => UiHelper.FormatColumns(gridReport);

            // Керування користувачами доступне лише адміністратору
            if (!user.IsAdministrator)
                tabControl.TabPages.Remove(tabUsers);

            WireLists();
            cmbReport.Items.AddRange(ReportCatalog.GetAll().Cast<object>().ToArray());

            _service.DataChanged += OnDataChanged;
            RefreshAll();
        }

        /// <summary>Користувач обрав «Змінити користувача».</summary>
        public bool LogoutRequested { get; private set; }

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
                if ((string)row["Статус"] != EnumHelper.ToText(StudentStatus.Active)) return InactiveColor;
                if ((decimal)row["Борг"] > 0) return DebtColor;
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
                if (value < 4) return DebtColor;
                if (value >= 10) return GoodColor;
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
                    Дата_народження = s.BirthDate,
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
            lblWelcome.Text = string.Format("Вітаємо, {0}!", _user.FullName);

            List<Student> students = _service.Students.GetAll().ToList();
            List<decimal> debts = students.Select(s => _service.GetDebt(s)).Where(d => d > 0).ToList();
            DateTime monthStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            double? avg = _service.GetAverageGrade(g => true);

            flowCards.SuspendLayout();
            foreach (Control control in flowCards.Controls.Cast<Control>().ToList())
                control.Dispose();
            flowCards.Controls.Clear();

            AddCard("Учнів навчається", students.Count(s => s.IsActive) + " з " + students.Count, AccentColor);
            AddCard("Викладачів", _service.Teachers.Count.ToString(), AccentColor);
            AddCard("Навчальних груп", _service.Groups.Count.ToString(), AccentColor);
            AddCard("Курсів", _service.Courses.Count.ToString(), AccentColor);
            AddCard("Надходження за " + DateTime.Today.ToString("MMMM"),
                _service.GetIncome(monthStart, DateTime.Today).ToString("N0") + " грн", GoodColor);
            AddCard("Загальна заборгованість", debts.Sum().ToString("N0") + " грн", DebtColor);
            AddCard("Боржників", debts.Count.ToString(), debts.Count > 0 ? DebtColor : GoodColor);
            AddCard("Середній бал", avg.HasValue ? avg.Value.ToString("0.00") : "—", AccentColor);
            flowCards.ResumeLayout();

            DayOfWeek today = DateTime.Today.DayOfWeek;
            var lessons = _service.GetSchedule(l => l.Day == today)
                .Select(l =>
                {
                    Teacher teacher = _service.GetTeacherOfGroup(l.GroupId);
                    Course course = _service.GetCourseOfGroup(l.GroupId);
                    return new
                    {
                        Час = l.TimeRange,
                        Група = _service.GetGroupName(l.GroupId),
                        Курс = course == null ? "—" : course.Name,
                        Викладач = teacher == null ? "—" : teacher.ShortName,
                        Аудиторія = l.Room,
                        Учнів = _service.GetActiveStudentCount(l.GroupId)
                    };
                }).ToList();

            lblToday.Text = string.Format("Розклад на сьогодні ({0}, {1:dd.MM.yyyy}){2}",
                DayNames.ToText(today).ToLower(), DateTime.Today, lessons.Count == 0 ? " — занять немає" : string.Empty);
            gridToday.DataSource = UiHelper.ToDataTable(lessons);
        }

        private void AddCard(string title, string value, Color valueColor)
        {
            Panel card = new Panel
            {
                Size = new Size(272, 88),
                Margin = new Padding(0, 0, 12, 12),
                BackColor = Color.FromArgb(242, 247, 252),
                BorderStyle = BorderStyle.FixedSingle
            };
            Label lblTitle = new Label
            {
                Text = title,
                AutoSize = false,
                Location = new Point(12, 10),
                Size = new Size(250, 20),
                ForeColor = Color.DimGray
            };
            Label lblValue = new Label
            {
                Text = value,
                AutoSize = false,
                Location = new Point(10, 34),
                Size = new Size(252, 40),
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = valueColor
            };
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblValue);
            flowCards.Controls.Add(card);
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

        #region Меню та події форми

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

        private void mnuExport_Click(object sender, EventArgs e)
        {
            TabPage tab = tabControl.SelectedTab;
            if (tab == tabReports)
            {
                btnExport_Click(sender, e);
                return;
            }

            DataGridView grid = tab == tabDashboard
                ? gridToday
                : tab.Controls.OfType<EntityListControl>().Select(l => l.Grid).FirstOrDefault();
            DataTable table = grid == null ? null : grid.DataSource as DataTable;
            ExportTable(table == null ? null : table.DefaultView.ToTable(), tab.Text);
        }

        private void mnuSave_Click(object sender, EventArgs e)
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

        private void mnuLogout_Click(object sender, EventArgs e)
        {
            LogoutRequested = true;
            Close();
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void mnuAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this,
                "Автоматизована система приватного навчального закладу\n" +
                "Версія 1.0\n\n" +
                "Курсова робота з дисципліни «Об'єктно-орієнтоване програмування».\n\n" +
                "Можливості: облік учнів, викладачів, курсів і груп; розклад занять із контролем накладок; " +
                "облік оплат і заборгованості; журнал успішності; звіти з експортом у CSV; " +
                "розмежування прав користувачів.\n\n" +
                "Технології: C#, .NET, Windows Forms, XML-серіалізація.",
                "Про програму", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabReports && cmbReport.SelectedIndex < 0 && cmbReport.Items.Count > 0)
                cmbReport.SelectedIndex = 0; // автоматично будує перший звіт
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
