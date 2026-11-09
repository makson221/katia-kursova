using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using PrivateSchool.Models;
using PrivateSchool.Services;
using PrivateSchool.Utils;

namespace PrivateSchool.Reports
{
    /// <summary>Список учнів, які мають заборгованість з оплати.</summary>
    public class DebtorsReport : ReportBase
    {
        public override string Title
        {
            get { return "Заборгованість з оплати"; }
        }

        protected override void DefineColumns(DataTable table)
        {
            AddColumn<string>(table, "Учень");
            AddColumn<string>(table, "Група");
            AddColumn<string>(table, "Договір");
            AddColumn<decimal>(table, "Нараховано, грн");
            AddColumn<decimal>(table, "Сплачено, грн");
            AddColumn<decimal>(table, "Борг, грн");
            AddColumn<string>(table, "Контакт батьків");
        }

        protected override void FillRows(DataTable table, SchoolService service)
        {
            var debtors = service.Students.GetAll()
                .Select(s => new { Student = s, Debt = service.GetDebt(s) })
                .Where(x => x.Debt > 0)
                .OrderByDescending(x => x.Debt);

            foreach (var item in debtors)
            {
                Student s = item.Student;
                table.Rows.Add(s.FullName, service.GetGroupName(s.GroupId), s.ContractNumber,
                    service.GetCharged(s), service.GetPaid(s), item.Debt,
                    (s.ParentName + " " + s.ParentPhone).Trim());
            }
        }

        protected override string BuildSummary(DataTable table, SchoolService service)
        {
            decimal total = table.Rows.Cast<DataRow>().Sum(r => (decimal)r["Борг, грн"]);
            return string.Format("Боржників: {0}. Загальна сума заборгованості: {1:N2} грн.", table.Rows.Count, total);
        }
    }

    /// <summary>Надходження коштів за останні 12 місяців.</summary>
    public class IncomeReport : ReportBase
    {
        public override string Title
        {
            get { return "Надходження коштів за місяцями"; }
        }

        protected override void DefineColumns(DataTable table)
        {
            AddColumn<string>(table, "Місяць");
            AddColumn<int>(table, "Кількість оплат");
            AddColumn<decimal>(table, "Готівка, грн");
            AddColumn<decimal>(table, "Картка, грн");
            AddColumn<decimal>(table, "Переказ, грн");
            AddColumn<decimal>(table, "Разом, грн");
        }

        protected override void FillRows(DataTable table, SchoolService service)
        {
            CultureInfo culture = new CultureInfo("uk-UA");
            DateTime first = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-11);
            for (DateTime month = first; month <= DateTime.Today; month = month.AddMonths(1))
            {
                DateTime monthStart = month;
                List<Payment> payments = service.Payments
                    .Find(p => p.Date.Year == monthStart.Year && p.Date.Month == monthStart.Month).ToList();
                if (payments.Count == 0)
                    continue;

                table.Rows.Add(
                    culture.TextInfo.ToTitleCase(month.ToString("MMMM yyyy", culture)),
                    payments.Count,
                    payments.Where(p => p.Method == PaymentMethod.Cash).Sum(p => p.Amount),
                    payments.Where(p => p.Method == PaymentMethod.Card).Sum(p => p.Amount),
                    payments.Where(p => p.Method == PaymentMethod.BankTransfer).Sum(p => p.Amount),
                    payments.Sum(p => p.Amount));
            }
        }

        protected override string BuildSummary(DataTable table, SchoolService service)
        {
            decimal total = table.Rows.Cast<DataRow>().Sum(r => (decimal)r["Разом, грн"]);
            return string.Format("Загальні надходження за період: {0:N2} грн.", total);
        }
    }

    /// <summary>Наповненість і успішність навчальних груп.</summary>
    public class GroupPerformanceReport : ReportBase
    {
        public override string Title
        {
            get { return "Успішність і наповненість груп"; }
        }

        protected override void DefineColumns(DataTable table)
        {
            AddColumn<string>(table, "Група");
            AddColumn<string>(table, "Курс");
            AddColumn<string>(table, "Викладач");
            AddColumn<string>(table, "Учнів / місць");
            AddColumn<int>(table, "Оцінок");
            AddColumn<string>(table, "Середній бал");
            AddColumn<string>(table, "Рівень");
        }

        protected override void FillRows(DataTable table, SchoolService service)
        {
            foreach (Group g in service.Groups.GetAll().OrderBy(g => g.Name))
            {
                double? avg = service.GetGroupAverage(g.Id);
                table.Rows.Add(g.Name, service.GetCourseName(g.CourseId), service.GetTeacherName(g.TeacherId),
                    service.GetActiveStudentCount(g.Id) + " / " + g.MaxStudents,
                    service.Grades.Find(x => x.GroupId == g.Id).Count(),
                    avg.HasValue ? avg.Value.ToString("0.00") : "—",
                    avg.HasValue ? Grade.GetLevelName(avg.Value) : "—");
            }
        }

        protected override string BuildSummary(DataTable table, SchoolService service)
        {
            double? avg = service.GetAverageGrade(g => true);
            return string.Format("Груп: {0}. Середній бал по закладу: {1}.",
                table.Rows.Count, avg.HasValue ? avg.Value.ToString("0.00") : "—");
        }
    }

    /// <summary>Рейтинг учнів за середнім балом.</summary>
    public class StudentRatingReport : ReportBase
    {
        public override string Title
        {
            get { return "Рейтинг учнів за середнім балом"; }
        }

        protected override void DefineColumns(DataTable table)
        {
            AddColumn<int>(table, "Місце");
            AddColumn<string>(table, "Учень");
            AddColumn<string>(table, "Група");
            AddColumn<int>(table, "Оцінок");
            AddColumn<double>(table, "Середній бал");
            AddColumn<string>(table, "Рівень");
        }

        protected override void FillRows(DataTable table, SchoolService service)
        {
            var rating = service.Students.Find(s => s.IsActive)
                .Select(s => new { Student = s, Avg = service.GetStudentAverage(s.Id) })
                .Where(x => x.Avg.HasValue)
                .OrderByDescending(x => x.Avg.Value)
                .ThenBy(x => x.Student.FullName)
                .ToList();

            int place = 1;
            foreach (var item in rating)
            {
                table.Rows.Add(place++, item.Student.FullName, service.GetGroupName(item.Student.GroupId),
                    service.Grades.Find(g => g.StudentId == item.Student.Id).Count(),
                    item.Avg.Value, Grade.GetLevelName(item.Avg.Value));
            }
        }
    }

    /// <summary>Тижневе навантаження та орієнтовна зарплата викладачів.</summary>
    public class TeacherWorkloadReport : ReportBase
    {
        public override string Title
        {
            get { return "Навантаження та заробітна плата викладачів"; }
        }

        protected override void DefineColumns(DataTable table)
        {
            AddColumn<string>(table, "Викладач");
            AddColumn<string>(table, "Спеціалізація");
            AddColumn<string>(table, "Категорія");
            AddColumn<string>(table, "Групи");
            AddColumn<int>(table, "Учнів");
            AddColumn<double>(table, "Годин на тиждень");
            AddColumn<decimal>(table, "Ставка, грн/год");
            AddColumn<decimal>(table, "Зарплата за місяць, грн");
        }

        protected override void FillRows(DataTable table, SchoolService service)
        {
            foreach (Teacher t in service.Teachers.GetAll().OrderBy(t => t.FullName))
            {
                List<Group> groups = service.GetGroupsOfTeacher(t.Id).ToList();
                double hours = service.GetTeacherWeeklyHours(t.Id);
                table.Rows.Add(t.ShortName, t.Specialization, EnumHelper.ToText(t.Category),
                    groups.Count == 0 ? "—" : string.Join(", ", groups.Select(g => g.Name)),
                    groups.Sum(g => service.GetActiveStudentCount(g.Id)),
                    Math.Round(hours, 1), t.HourlyRate, t.CalculateMonthlySalary(hours));
            }
        }

        protected override string BuildSummary(DataTable table, SchoolService service)
        {
            decimal total = table.Rows.Cast<DataRow>().Sum(r => (decimal)r["Зарплата за місяць, грн"]);
            return string.Format("Фонд оплати праці викладачів (орієнтовно): {0:N2} грн/міс.", total);
        }
    }

    /// <summary>Перелік усіх доступних звітів (для вибору у формі).</summary>
    public static class ReportCatalog
    {
        public static IEnumerable<ReportBase> GetAll()
        {
            return new ReportBase[]
            {
                new DebtorsReport(),
                new IncomeReport(),
                new GroupPerformanceReport(),
                new StudentRatingReport(),
                new TeacherWorkloadReport()
            };
        }
    }
}
