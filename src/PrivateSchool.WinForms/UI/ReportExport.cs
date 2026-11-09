using System.Diagnostics;
using PrivateSchool.Core.Reports;
using PrivateSchool.Core.Services;

namespace PrivateSchool.WinForms.UI;

/// <summary>
/// Збереження звіту у файл через стандартний діалог з вибором формату.
/// </summary>
public static class ReportExport
{
    public static void SaveWithDialog(SchoolSystem app, ReportTable table, IWin32Window? owner)
    {
        using var dialog = new SaveFileDialog
        {
            Title = "Збереження звіту",
            Filter = string.Join("|", app.Exporters.Select(e => e.FileFilter)),
            FileName = MakeFileName(table.Title),
            AddExtension = true,
            OverwritePrompt = true
        };
        if (dialog.ShowDialog(owner) != DialogResult.OK) return;

        var exporter = app.Exporters[Math.Clamp(dialog.FilterIndex - 1, 0, app.Exporters.Count - 1)];
        exporter.Export(table, dialog.FileName);

        if (Messages.Confirm($"Файл збережено:\n{dialog.FileName}\n\nВідкрити його зараз?", owner))
            Process.Start(new ProcessStartInfo(dialog.FileName) { UseShellExecute = true });
    }

    private static string MakeFileName(string title)
    {
        var invalid = Path.GetInvalidFileNameChars();
        var clean = new string(title.Select(c => invalid.Contains(c) ? '_' : c).ToArray());
        return $"{clean} {DateTime.Now:yyyy-MM-dd}";
    }
}
