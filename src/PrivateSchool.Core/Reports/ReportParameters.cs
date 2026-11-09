namespace PrivateSchool.Core.Reports;

/// <summary>Параметри побудови звіту.</summary>
public class ReportParameters
{
    public DateTime From { get; set; } = new(DateTime.Today.Year, DateTime.Today.Month, 1);
    public DateTime To { get; set; } = DateTime.Today;
    public int? GroupId { get; set; }
}
