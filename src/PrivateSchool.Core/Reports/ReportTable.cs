namespace PrivateSchool.Core.Reports;

/// <summary>Вирівнювання колонки звіту.</summary>
public enum ColumnAlign { Left, Center, Right }

/// <summary>Опис колонки звіту.</summary>
public sealed record ReportColumn(string Header, ColumnAlign Align = ColumnAlign.Left, int Width = 120);

/// <summary>
/// Результат побудови звіту: заголовок, колонки, рядки та підсумок.
/// Не залежить від інтерфейсу користувача — може бути відображений у таблиці або експортований.
/// </summary>
public class ReportTable
{
    public string Title { get; }
    public string Subtitle { get; init; } = string.Empty;
    public IReadOnlyList<ReportColumn> Columns { get; }
    public List<string[]> Rows { get; } = new();
    public List<string> Summary { get; } = new();
    public DateTime CreatedAt { get; } = DateTime.Now;

    public ReportTable(string title, params ReportColumn[] columns)
    {
        Title = title;
        Columns = columns;
    }

    public void AddRow(params object?[] values)
    {
        if (values.Length != Columns.Count)
            throw new ArgumentException($"Очікується {Columns.Count} значень, отримано {values.Length}.");
        Rows.Add(values.Select(Format).ToArray());
    }

    private static string Format(object? value) => value switch
    {
        null => "",
        decimal d => d.ToString("N2"),
        double d => d.ToString("0.0"),
        DateTime dt => dt.ToString("dd.MM.yyyy"),
        _ => value.ToString() ?? ""
    };
}
