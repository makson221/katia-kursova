using PrivateSchool.Core.Reports;

namespace PrivateSchool.Core.Interfaces;

/// <summary>
/// Експортер звіту у певний файловий формат (шаблон Strategy).
/// </summary>
public interface IReportExporter
{
    /// <summary>Назва формату для користувача.</summary>
    string FormatName { get; }

    /// <summary>Фільтр для діалогу збереження файлу, напр. "CSV (*.csv)|*.csv".</summary>
    string FileFilter { get; }

    void Export(ReportTable table, string filePath);
}
