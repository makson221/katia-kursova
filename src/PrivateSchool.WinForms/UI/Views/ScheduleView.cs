using PrivateSchool.Core.Models;
using PrivateSchool.Core.Services;
using PrivateSchool.WinForms.UI.Controls;
using PrivateSchool.WinForms.UI.Dialogs;

namespace PrivateSchool.WinForms.UI.Views;

/// <summary>
/// Тижневий розклад у вигляді сітки "пари × дні" для групи, викладача або аудиторії.
/// </summary>
public sealed class ScheduleView : ViewBase
{
    private enum Mode { Group, Teacher }

    private readonly ComboBox _mode = Lookup.DropDownList();
    private readonly ComboBox _target = Lookup.DropDownList();
    private readonly DataGridView _grid;
    private readonly FlatButton _add;
    private readonly FlatButton _edit;
    private readonly FlatButton _delete;
    private readonly Label _footer;
    private readonly Dictionary<(DayOfWeek Day, int Number), Lesson> _cells = new();
    private bool _loading;

    public ScheduleView(SchoolSystem app) : base(app)
    {
        var toolbar = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 52, WrapContents = false, Padding = new Padding(0, 0, 0, 12) };
        _add = new FlatButton("Додати заняття", ButtonVariant.Primary, Theme.Glyphs.Add);
        _edit = new FlatButton("Редагувати", ButtonVariant.Secondary, Theme.Glyphs.Edit);
        _delete = new FlatButton("Видалити", ButtonVariant.Danger, Theme.Glyphs.Delete);
        var print = new FlatButton("Експорт", ButtonVariant.Secondary, Theme.Glyphs.Export);
        _add.Click += (_, _) => Try(() => AddLesson(null));
        _edit.Click += (_, _) => Try(EditSelected);
        _delete.Click += (_, _) => Try(DeleteSelected);
        print.Click += (_, _) => Try(Export);

        _mode.Width = 130;
        _mode.Margin = new Padding(16, 6, 4, 0);
        _mode.Items.AddRange(new object[] { "Розклад групи", "Розклад викладача" });
        _target.Width = 260;
        _target.Margin = new Padding(4, 6, 4, 0);
        _mode.SelectedIndexChanged += (_, _) => { FillTargets(); RefreshData(); };
        _target.SelectedIndexChanged += (_, _) => { if (!_loading) RefreshData(); };

        toolbar.Controls.AddRange(new Control[] { _add, _edit, _delete, print, _mode, _target });

        _grid = new StyledGrid
        {
            Dock = DockStyle.Fill,
            SelectionMode = DataGridViewSelectionMode.CellSelect,
            RowHeadersVisible = true,
            RowHeadersWidth = 120,
            AllowUserToOrderColumns = false,
            CellBorderStyle = DataGridViewCellBorderStyle.Single,
            RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single,
            ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single
        };
        _grid.RowHeadersDefaultCellStyle.BackColor = Theme.SurfaceAlt;
        _grid.RowHeadersDefaultCellStyle.ForeColor = Theme.TextMuted;
        _grid.RowHeadersDefaultCellStyle.Font = Theme.LabelFont;
        _grid.RowHeadersDefaultCellStyle.SelectionBackColor = Theme.SurfaceAlt;
        _grid.RowHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
        _grid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        _grid.DefaultCellStyle.Font = Theme.SmallFont;
        _grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        _grid.AlternatingRowsDefaultCellStyle.BackColor = Theme.Surface;
        _grid.RowTemplate.Height = 78;
        _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

        foreach (var day in LessonTimes.StudyDays)
        {
            var col = new DataGridViewTextBoxColumn
            {
                HeaderText = LessonTimes.DayName(day),
                SortMode = DataGridViewColumnSortMode.NotSortable,
                Tag = day,
                DefaultCellStyle = { WrapMode = DataGridViewTriState.True, Alignment = DataGridViewContentAlignment.MiddleCenter }
            };
            _grid.Columns.Add(col);
        }
        for (var n = 1; n <= LessonTimes.MaxLessonsPerDay; n++)
        {
            var index = _grid.Rows.Add();
            _grid.Rows[index].HeaderCell.Value = $"{n} пара{Environment.NewLine}{LessonTimes.GetTime(n)}";
            _grid.Rows[index].Tag = n;
        }

        _grid.CellDoubleClick += (_, e) =>
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || !CanModify) return;
            Try(() =>
            {
                if (SelectedLesson != null) EditSelected();
                else AddLesson((Day(e.ColumnIndex), e.RowIndex + 1));
            });
        };
        _grid.SelectionChanged += (_, _) => UpdateButtons();
        _grid.Resize += (_, _) => FitRows();
        _grid.KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.Delete && CanModify) { Try(DeleteSelected); e.Handled = true; }
        };

        var card = new Panel { Dock = DockStyle.Fill, BackColor = Theme.Surface };
        _footer = new Label
        {
            Dock = DockStyle.Bottom,
            Height = 36,
            Padding = new Padding(12, 0, 0, 0),
            TextAlign = ContentAlignment.MiddleLeft,
            ForeColor = Theme.TextMuted,
            BackColor = Theme.SurfaceAlt,
            Text = "Подвійний клік по порожній клітинці — додати заняття, по заповненій — редагувати."
        };
        card.Controls.Add(_grid);
        card.Controls.Add(_footer);

        Content.Controls.Add(card);
        Content.Controls.Add(toolbar);

        _mode.SelectedIndex = App.IsTeacher() ? 1 : 0;
    }

    public override string Title => "Розклад занять";
    public override string Subtitle => "Тижневий розклад з автоматичною перевіркою накладок";

    private bool CanModify => App.Auth.Can(Permission.ManageSchedule);
    private Mode CurrentMode => _mode.SelectedIndex == 1 ? Mode.Teacher : Mode.Group;

    private static DayOfWeek Day(int columnIndex) => LessonTimes.StudyDays[columnIndex];

    private Lesson? SelectedLesson
    {
        get
        {
            var cell = _grid.CurrentCell;
            if (cell == null) return null;
            return _cells.GetValueOrDefault((Day(cell.ColumnIndex), cell.RowIndex + 1));
        }
    }

    private void FillTargets()
    {
        _loading = true;
        var selected = Lookup.SelectedId(_target);
        if (CurrentMode == Mode.Group)
        {
            Lookup.Fill(_target, Lookup.Groups(App.Db), selected);
        }
        else
        {
            var teacherId = App.Auth.CurrentUser?.TeacherId;
            Lookup.Fill(_target, Lookup.Teachers(App.Db), selected ?? teacherId);
        }
        if (_target.SelectedIndex < 0 && _target.Items.Count > 0) _target.SelectedIndex = 0;
        _loading = false;
    }

    public override void OnActivated()
    {
        FillTargets();
        base.OnActivated();
    }

    public override void RefreshData()
    {
        _cells.Clear();
        foreach (DataGridViewRow row in _grid.Rows)
            foreach (DataGridViewCell cell in row.Cells)
            {
                cell.Value = null;
                cell.Style.BackColor = Theme.Surface;
                cell.Style.SelectionBackColor = Theme.PrimaryLight;
            }

        if (Lookup.SelectedId(_target) is not int targetId)
        {
            UpdateButtons();
            return;
        }

        var lessons = CurrentMode == Mode.Group
            ? App.Schedule.GetForGroup(targetId)
            : App.Schedule.GetForTeacher(targetId);

        foreach (var lesson in lessons)
        {
            var column = Array.IndexOf(LessonTimes.StudyDays, lesson.Day);
            if (column < 0 || lesson.Number < 1 || lesson.Number > LessonTimes.MaxLessonsPerDay) continue;

            _cells[(lesson.Day, lesson.Number)] = lesson;
            var cell = _grid.Rows[lesson.Number - 1].Cells[column];
            var subject = App.Db.Subjects.GetById(lesson.SubjectId)?.Name ?? "?";
            var second = CurrentMode == Mode.Group
                ? App.Db.Teachers.GetById(lesson.TeacherId)?.ShortName
                : "гр. " + App.Db.Groups.GetById(lesson.GroupId)?.Name;
            cell.Value = string.Join(Environment.NewLine, subject, second, $"ауд. {lesson.Room} ({ShortType(lesson.Type)})");

            var color = lesson.Type switch
            {
                LessonType.Lecture => Theme.PrimaryLight,
                LessonType.Lab => Theme.SuccessLight,
                _ => Theme.WarningLight
            };
            cell.Style.BackColor = color;
            cell.Style.SelectionBackColor = Theme.Blend(color, Theme.Primary, 0.25);
        }

        var hours = lessons.Count * ScheduleService.HoursPerLesson;
        _footer.Text = $"Занять на тиждень: {lessons.Count} ({hours} год)  ·  " +
                       "Синій — лекція, жовтий — практичне, зелений — лабораторне.  " +
                       (CanModify ? "Подвійний клік — додати/редагувати." : "");
        UpdateButtons();
    }

    /// <summary>Розтягує рядки пар на всю доступну висоту таблиці.</summary>
    private void FitRows()
    {
        if (_grid.Rows.Count == 0) return;
        var height = Math.Max(78, (_grid.ClientSize.Height - _grid.ColumnHeadersHeight - 2) / _grid.Rows.Count);
        foreach (DataGridViewRow row in _grid.Rows) row.Height = height;
    }

    private static string ShortType(LessonType type) => type switch
    {
        LessonType.Lecture => "лекція",
        LessonType.Lab => "лаб.",
        _ => "практ."
    };

    private void UpdateButtons()
    {
        _add.Visible = _edit.Visible = _delete.Visible = CanModify;
        _edit.Enabled = _delete.Enabled = SelectedLesson != null;
    }

    private void AddLesson((DayOfWeek Day, int Number)? slot)
    {
        if (!CanModify) return;
        var cell = _grid.CurrentCell;
        var lesson = new Lesson
        {
            Day = slot?.Day ?? (cell != null ? Day(cell.ColumnIndex) : DayOfWeek.Monday),
            Number = slot?.Number ?? (cell != null ? cell.RowIndex + 1 : 1)
        };
        if (Lookup.SelectedId(_target) is int id)
        {
            if (CurrentMode == Mode.Group) lesson.GroupId = id;
            else lesson.TeacherId = id;
        }

        using var dialog = new LessonDialog(App, lesson, isNew: true);
        if (dialog.ShowDialog(FindForm()) == DialogResult.OK) RefreshData();
    }

    private void EditSelected()
    {
        if (SelectedLesson is not { } lesson) return;
        using var dialog = new LessonDialog(App, (Lesson)lesson.Clone(), isNew: false);
        if (dialog.ShowDialog(FindForm()) == DialogResult.OK) RefreshData();
    }

    private void DeleteSelected()
    {
        if (SelectedLesson is not { } lesson) return;
        var subject = App.Db.Subjects.GetById(lesson.SubjectId)?.Name;
        if (!Messages.Confirm($"Видалити заняття «{subject}» ({LessonTimes.DayName(lesson.Day)}, {lesson.Number} пара)?", FindForm()))
            return;
        App.Integrity.Delete(lesson);
        RefreshData();
    }

    private void Export()
    {
        var table = new Core.Reports.ReportTable($"Розклад занять: {_target.Text}",
            new[] { new Core.Reports.ReportColumn("Пара", Core.Reports.ColumnAlign.Center, 90) }
                .Concat(LessonTimes.StudyDays.Select(d => new Core.Reports.ReportColumn(LessonTimes.DayName(d), Width: 150)))
                .ToArray())
        {
            Subtitle = App.Db.InstitutionName
        };
        foreach (DataGridViewRow row in _grid.Rows)
        {
            var values = new List<object?> { row.HeaderCell.Value?.ToString()?.Replace(Environment.NewLine, " ") };
            values.AddRange(row.Cells.Cast<DataGridViewCell>().Select(c => (object?)c.Value?.ToString()?.Replace(Environment.NewLine, " / ")));
            table.AddRow(values.ToArray());
        }
        ReportExport.SaveWithDialog(App, table, FindForm());
    }
}
