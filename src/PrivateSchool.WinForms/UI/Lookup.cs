using PrivateSchool.Core.Models;

namespace PrivateSchool.WinForms.UI;

/// <summary>Елемент випадаючого списку: ідентифікатор сутності та текст.</summary>
public sealed record LookupItem(int? Id, string Text)
{
    public override string ToString() => Text;
}

/// <summary>
/// Допоміжні методи заповнення та читання випадаючих списків.
/// </summary>
public static class Lookup
{
    public static void Fill(ComboBox combo, IEnumerable<LookupItem> items, int? selectedId = null, string? emptyText = null)
    {
        combo.BeginUpdate();
        combo.Items.Clear();
        if (emptyText != null) combo.Items.Add(new LookupItem(null, emptyText));
        foreach (var item in items) combo.Items.Add(item);
        combo.EndUpdate();
        Select(combo, selectedId);
    }

    public static void Select(ComboBox combo, int? id)
    {
        for (var i = 0; i < combo.Items.Count; i++)
        {
            if (combo.Items[i] is LookupItem item && item.Id == id)
            {
                combo.SelectedIndex = i;
                return;
            }
        }
        combo.SelectedIndex = combo.Items.Count > 0 && id == null ? 0 : -1;
    }

    public static int? SelectedId(ComboBox combo) => (combo.SelectedItem as LookupItem)?.Id;

    public static void FillEnum<T>(ComboBox combo, T selected) where T : struct, Enum
    {
        combo.Items.Clear();
        foreach (var item in EnumDisplay.GetItems<T>()) combo.Items.Add(item);
        combo.SelectedItem = combo.Items.Cast<EnumItem<T>>().First(i => i.Value.Equals(selected));
    }

    public static T SelectedEnum<T>(ComboBox combo) where T : struct, Enum =>
        combo.SelectedItem is EnumItem<T> item ? item.Value : default;

    /// <summary>Створює випадаючий список з пошуком (автодоповненням).</summary>
    public static ComboBox SearchableCombo()
    {
        var combo = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDown,
            AutoCompleteMode = AutoCompleteMode.SuggestAppend,
            AutoCompleteSource = AutoCompleteSource.ListItems
        };
        // Якщо введений текст точно відповідає елементу — обираємо його.
        combo.Validating += (_, _) =>
        {
            if (combo.SelectedIndex >= 0) return;
            var index = combo.FindStringExact(combo.Text);
            if (index >= 0) combo.SelectedIndex = index;
        };
        return combo;
    }

    public static ComboBox DropDownList() => new() { DropDownStyle = ComboBoxStyle.DropDownList };

    public static IEnumerable<LookupItem> Groups(Core.Data.SchoolDatabase db) =>
        db.Groups.GetAll().OrderBy(g => g.Name).Select(g => new LookupItem(g.Id, g.Name));

    public static IEnumerable<LookupItem> Teachers(Core.Data.SchoolDatabase db) =>
        db.Teachers.GetAll().OrderBy(t => t.FullName).Select(t => new LookupItem(t.Id, t.FullName));

    public static IEnumerable<LookupItem> Subjects(IEnumerable<Subject> subjects) =>
        subjects.OrderBy(s => s.Name).Select(s => new LookupItem(s.Id, s.Name));

    public static IEnumerable<LookupItem> Students(Core.Data.SchoolDatabase db, Func<Student, bool>? filter = null) =>
        db.Students.Find(filter ?? (_ => true))
            .OrderBy(s => s.FullName)
            .Select(s => new LookupItem(s.Id, $"{s.FullName} ({db.Groups.GetById(s.GroupId)?.Name ?? "—"})"));
}
