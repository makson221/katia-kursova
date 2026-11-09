using System;
using System.Data;
using PrivateSchool.Services;

namespace PrivateSchool.Reports
{
    /// <summary>
    /// Абстрактний звіт. Кожен конкретний звіт визначає назву, набір колонок
    /// та спосіб заповнення рядків (шаблонний метод Build).
    /// </summary>
    public abstract class ReportBase
    {
        public abstract string Title { get; }

        public virtual string Description
        {
            get { return string.Empty; }
        }

        /// <summary>Шаблонний метод: створює таблицю, заповнює її та формує підсумок.</summary>
        public ReportResult Build(SchoolService service)
        {
            if (service == null)
                throw new ArgumentNullException("service");

            DataTable table = new DataTable(Title);
            DefineColumns(table);
            FillRows(table, service);
            return new ReportResult(Title, table, BuildSummary(table, service));
        }

        protected abstract void DefineColumns(DataTable table);

        protected abstract void FillRows(DataTable table, SchoolService service);

        protected virtual string BuildSummary(DataTable table, SchoolService service)
        {
            return "Кількість рядків: " + table.Rows.Count;
        }

        protected static void AddColumn<T>(DataTable table, string name)
        {
            table.Columns.Add(name, typeof(T));
        }

        public override string ToString()
        {
            return Title;
        }
    }

    /// <summary>
    /// Результат формування звіту.
    /// </summary>
    public class ReportResult
    {
        public ReportResult(string title, DataTable table, string summary)
        {
            Title = title;
            Table = table;
            Summary = summary;
            CreatedAt = DateTime.Now;
        }

        public string Title { get; private set; }
        public DataTable Table { get; private set; }
        public string Summary { get; private set; }
        public DateTime CreatedAt { get; private set; }
    }
}
