using System.Text;
using PrivateSchool.Core.Interfaces;

namespace PrivateSchool.Core.Reports;

/// <summary>
/// Експорт звіту у CSV (роздільник ";" та UTF-8 з BOM — коректно відкривається в Excel).
/// </summary>
public class CsvReportExporter : IReportExporter
{
    public string FormatName => "CSV (Excel)";
    public string FileFilter => "CSV-файл (*.csv)|*.csv";

    public void Export(ReportTable table, string filePath)
    {
        var sb = new StringBuilder();
        sb.AppendLine(Escape(table.Title));
        if (table.Subtitle.Length > 0) sb.AppendLine(Escape(table.Subtitle));
        sb.AppendLine();
        sb.AppendLine(string.Join(';', table.Columns.Select(c => Escape(c.Header))));
        foreach (var row in table.Rows)
            sb.AppendLine(string.Join(';', row.Select(Escape)));
        sb.AppendLine();
        foreach (var line in table.Summary)
            sb.AppendLine(Escape(line));

        File.WriteAllText(filePath, sb.ToString(), new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
    }

    internal static string Escape(string value)
    {
        if (value.IndexOfAny(new[] { ';', '"', '\n', '\r' }) < 0) return value;
        return '"' + value.Replace("\"", "\"\"") + '"';
    }
}
