using PrivateSchool.Core.Exceptions;

namespace PrivateSchool.WinForms.UI;

/// <summary>
/// Уніфіковані діалогові повідомлення.
/// </summary>
public static class Messages
{
    private const string AppTitle = "EduManager";

    public static void Info(string text, IWin32Window? owner = null) =>
        MessageBox.Show(owner, text, AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);

    public static void Warning(string text, IWin32Window? owner = null) =>
        MessageBox.Show(owner, text, AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);

    public static void Error(string text, IWin32Window? owner = null) =>
        MessageBox.Show(owner, text, AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

    public static bool Confirm(string text, IWin32Window? owner = null) =>
        MessageBox.Show(owner, text, AppTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2) == DialogResult.Yes;

    /// <summary>Показує повідомлення про виняток: бізнес-помилки — як попередження, решту — як помилку.</summary>
    public static void Exception(Exception ex, IWin32Window? owner = null)
    {
        if (ex is DomainException) Warning(ex.Message, owner);
        else Error($"Сталася непередбачена помилка:\n{ex.Message}", owner);
    }
}
