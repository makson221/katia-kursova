using System.ComponentModel;
using PrivateSchool.Core.Exceptions;
using PrivateSchool.Core.Models;
using PrivateSchool.Core.Services;
using PrivateSchool.WinForms.UI.Controls;

namespace PrivateSchool.WinForms.UI.Dialogs;

/// <summary>
/// Базовий діалог редагування сутності (шаблон Template Method).
/// Нащадки створюють поля (<see cref="BuildFields"/>), переносять дані
/// в елементи керування (<see cref="BindToControls"/>) та назад (<see cref="ApplyToEntity"/>).
/// Збереження виконується через делегат: при помилці діалог не закривається.
/// </summary>
[DesignerCategory("Code")]
public abstract class EntityDialog<T> : Form where T : Entity
{
    private const int ColumnWidth = 270;

    private readonly Action<T>? _save;
    private readonly TableLayoutPanel _fields;
    private readonly Label _errors;
    private readonly int _columns;

    protected EntityDialog(SchoolSystem app, T entity, bool isNew, Action<T>? save, int columns = 2)
    {
        App = app;
        Entity = entity;
        IsNew = isNew;
        _save = save;
        _columns = columns;

        AutoScaleMode = AutoScaleMode.Dpi;
        AutoScaleDimensions = new SizeF(96F, 96F);
        Font = Theme.BaseFont;
        BackColor = Theme.Surface;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        KeyPreview = true;

        var root = new TableLayoutPanel
        {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 1,
            Padding = new Padding(24, 20, 24, 20),
            Location = Point.Empty
        };

        var header = new Label
        {
            AutoSize = true,
            Font = Theme.SectionFont,
            ForeColor = Theme.Text,
            Margin = new Padding(4, 0, 0, 14)
        };
        root.Controls.Add(header);

        _fields = new TableLayoutPanel
        {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = columns,
            Margin = Padding.Empty
        };
        for (var i = 0; i < columns; i++) _fields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, ColumnWidth));
        root.Controls.Add(_fields);

        _errors = new Label
        {
            AutoSize = true,
            ForeColor = Theme.Danger,
            BackColor = Theme.DangerLight,
            Padding = new Padding(10, 8, 10, 8),
            Margin = new Padding(4, 10, 4, 0),
            MaximumSize = new Size(ColumnWidth * columns - 8, 0),
            MinimumSize = new Size(ColumnWidth * columns - 8, 0),
            Visible = false
        };
        root.Controls.Add(_errors);

        var buttons = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.RightToLeft,
            AutoSize = true,
            Dock = DockStyle.Fill,
            Margin = new Padding(4, 18, 0, 0)
        };
        var ok = new FlatButton("Зберегти", ButtonVariant.Primary) { Margin = new Padding(8, 0, 0, 0) };
        var cancel = new FlatButton("Скасувати") { DialogResult = DialogResult.Cancel, Margin = new Padding(8, 0, 0, 0) };
        ok.Click += (_, _) => OnSave();
        buttons.Controls.Add(ok);
        buttons.Controls.Add(cancel);
        root.Controls.Add(buttons);

        AcceptButton = ok;
        CancelButton = cancel;
        Controls.Add(root);

        BuildFields();
        Theme.StyleInputs(_fields);
        BindToControls();

        Text = DialogTitle;
        header.Text = DialogTitle;
    }

    protected SchoolSystem App { get; }
    protected T Entity { get; }
    protected bool IsNew { get; }

    /// <summary>Назва сутності в родовому відмінку для заголовка, напр. "студента".</summary>
    protected abstract string EntityName { get; }

    protected virtual string DialogTitle => IsNew ? $"Додавання {EntityName}" : $"Редагування {EntityName}";

    protected abstract void BuildFields();
    protected abstract void BindToControls();
    protected abstract void ApplyToEntity();

    /// <summary>Збереження сутності. За замовчуванням викликає переданий делегат.</summary>
    protected virtual void Save(T entity)
    {
        if (_save == null) throw new InvalidOperationException("Не задано спосіб збереження.");
        _save(entity);
    }

    /// <summary>Додаткові перевірки на рівні форми (до збереження).</summary>
    protected virtual IEnumerable<string> ValidateForm() => Enumerable.Empty<string>();

    /// <summary>Додає поле з підписом у сітку форми.</summary>
    protected TControl AddField<TControl>(string label, TControl control, int span = 1) where TControl : Control
    {
        span = Math.Min(span, _columns);
        var cell = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoSize = true,
            Margin = new Padding(0, 0, 0, 10)
        };
        cell.Controls.Add(new Label
        {
            Text = label,
            AutoSize = true,
            Font = Theme.LabelFont,
            ForeColor = Theme.TextMuted,
            Margin = new Padding(4, 0, 0, 4)
        });

        control.Width = ColumnWidth * span - 20;
        control.Margin = new Padding(4, 0, 12, 0);
        cell.Controls.Add(control);

        _fields.Controls.Add(cell);
        if (span > 1) _fields.SetColumnSpan(cell, span);
        return control;
    }

    /// <summary>Додає підзаголовок розділу на всю ширину форми.</summary>
    protected void AddSection(string title)
    {
        var label = new Label
        {
            Text = title.ToUpperInvariant(),
            AutoSize = true,
            Font = new Font(Theme.FontName, 8.5f, FontStyle.Bold),
            ForeColor = Theme.Primary,
            Margin = new Padding(4, 6, 0, 8)
        };
        _fields.Controls.Add(label);
        _fields.SetColumnSpan(label, _columns);
    }

    /// <summary>Пропускає клітинку сітки (для вирівнювання).</summary>
    protected void AddSpacer() => _fields.Controls.Add(new Label { AutoSize = false, Size = new Size(1, 1) });

    protected static TextBox NewTextBox(int maxLength = 100) => new() { MaxLength = maxLength };

    protected static DateTimePicker NewDatePicker(bool optional = false) => new()
    {
        Format = DateTimePickerFormat.Custom,
        CustomFormat = "dd.MM.yyyy",
        ShowCheckBox = optional,
        MinDate = new DateTime(1920, 1, 1),
        MaxDate = DateTime.Today.AddYears(2)
    };

    protected static NumericUpDown NewNumeric(decimal min, decimal max, int decimals = 0, decimal increment = 1) => new()
    {
        Minimum = min,
        Maximum = max,
        DecimalPlaces = decimals,
        Increment = increment,
        ThousandsSeparator = true,
        TextAlign = HorizontalAlignment.Right
    };

    protected static void SetDate(DateTimePicker picker, DateTime value) =>
        picker.Value = value < picker.MinDate ? picker.MinDate : value > picker.MaxDate ? picker.MaxDate : value;

    protected static void SetNumber(NumericUpDown nud, decimal value) =>
        nud.Value = Math.Clamp(value, nud.Minimum, nud.Maximum);

    private void OnSave()
    {
        // Елементи з автодоповненням оновлюють вибір під час Validating.
        ValidateChildren();

        var errors = new List<string>();
        try
        {
            ApplyToEntity();
            errors.AddRange(ValidateForm());
        }
        catch (FormatException ex)
        {
            errors.Add(ex.Message);
        }

        if (errors.Count == 0)
        {
            try
            {
                Save(Entity);
                DialogResult = DialogResult.OK;
                Close();
                return;
            }
            catch (ValidationException ex)
            {
                errors.AddRange(ex.Errors);
            }
            catch (DomainException ex)
            {
                errors.Add(ex.Message);
            }
        }

        _errors.Text = string.Join(Environment.NewLine, errors.Distinct().Select(e => "• " + e));
        _errors.Visible = true;
    }
}
