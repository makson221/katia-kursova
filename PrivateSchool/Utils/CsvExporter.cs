using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace PrivateSchool.Utils
{
    /// <summary>
    /// Експорт таблиці у CSV-файл (роздільник «;», кодування UTF-8 з BOM — коректно відкривається в Excel).
    /// </summary>
    public static class CsvExporter
    {
        public static void Export(DataTable table, string filePath)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Join(";", table.Columns.Cast<DataColumn>().Select(c => Escape(c.ColumnName))));
            foreach (DataRow row in table.Rows)
                sb.AppendLine(string.Join(";", row.ItemArray.Select(v => Escape(v == null ? "" : v.ToString()))));
            File.WriteAllText(filePath, sb.ToString(), new UTF8Encoding(true));
        }

        private static string Escape(string value)
        {
            if (value.IndexOfAny(new[] { ';', '"', '\n', '\r' }) >= 0)
                return "\"" + value.Replace("\"", "\"\"") + "\"";
            return value;
        }
    }
}
