using System.Net;
using System.Text;
using PrivateSchool.Core.Interfaces;

namespace PrivateSchool.Core.Reports;

/// <summary>
/// Експорт звіту у HTML-сторінку, готову до друку з браузера.
/// </summary>
public class HtmlReportExporter : IReportExporter
{
    private readonly Func<string> _institution;

    /// <param name="institution">Функція, що повертає актуальну назву закладу для шапки звіту.</param>
    public HtmlReportExporter(Func<string> institution) => _institution = institution;

    public string FormatName => "HTML (для друку)";
    public string FileFilter => "HTML-сторінка (*.html)|*.html";

    public void Export(ReportTable table, string filePath)
    {
        static string E(string s) => WebUtility.HtmlEncode(s);
        static string Align(ColumnAlign a) => a switch
        {
            ColumnAlign.Right => "right",
            ColumnAlign.Center => "center",
            _ => "left"
        };

        var sb = new StringBuilder();
        sb.Append("""
            <!DOCTYPE html>
            <html lang="uk"><head><meta charset="utf-8">
            <style>
              body { font-family: 'Segoe UI', Arial, sans-serif; margin: 32px; color: #1e293b; }
              .inst { color: #64748b; font-size: 13px; text-transform: uppercase; letter-spacing: .05em; }
              h1 { margin: 4px 0; font-size: 22px; }
              .sub { color: #475569; margin-bottom: 18px; }
              table { border-collapse: collapse; width: 100%; font-size: 13px; }
              th { background: #2563eb; color: #fff; padding: 8px; border: 1px solid #1d4ed8; }
              td { padding: 6px 8px; border: 1px solid #cbd5e1; }
              tr:nth-child(even) td { background: #f1f5f9; }
              .summary { margin-top: 16px; font-weight: 600; }
              .foot { margin-top: 32px; color: #94a3b8; font-size: 12px; }
              @media print { body { margin: 10mm; } }
            </style>
            """);
        sb.Append($"<title>{E(table.Title)}</title></head><body>");
        sb.Append($"<div class=\"inst\">{E(_institution())}</div><h1>{E(table.Title)}</h1>");
        if (table.Subtitle.Length > 0) sb.Append($"<div class=\"sub\">{E(table.Subtitle)}</div>");

        sb.Append("<table><thead><tr>");
        foreach (var c in table.Columns) sb.Append($"<th>{E(c.Header)}</th>");
        sb.Append("</tr></thead><tbody>");
        foreach (var row in table.Rows)
        {
            sb.Append("<tr>");
            for (var i = 0; i < row.Length; i++)
                sb.Append($"<td style=\"text-align:{Align(table.Columns[i].Align)}\">{E(row[i])}</td>");
            sb.Append("</tr>");
        }
        sb.Append("</tbody></table><div class=\"summary\">");
        foreach (var line in table.Summary) sb.Append($"<div>{E(line)}</div>");
        sb.Append($"</div><div class=\"foot\">Сформовано: {table.CreatedAt:dd.MM.yyyy HH:mm}</div></body></html>");

        File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
    }
}
