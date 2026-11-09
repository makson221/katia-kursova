using PrivateSchool.Core.Interfaces;
using PrivateSchool.Core.Models;

namespace PrivateSchool.Core.Data;

/// <summary>
/// Контекст даних закладу: об'єднує репозиторії всіх сутностей та
/// забезпечує завантаження/збереження через <see cref="IDataStorage"/>.
/// </summary>
public class SchoolDatabase
{
    private readonly IDataStorage _storage;
    private bool _suspendSave;

    public Repository<Student> Students { get; } = new();
    public Repository<Teacher> Teachers { get; } = new();
    public Repository<Group> Groups { get; } = new();
    public Repository<Subject> Subjects { get; } = new();
    public Repository<Grade> Grades { get; } = new();
    public Repository<Payment> Payments { get; } = new();
    public Repository<Lesson> Lessons { get; } = new();
    public Repository<User> Users { get; } = new();

    public string InstitutionName { get; set; } = "Приватний коледж «Освіта+»";

    /// <summary>Автоматично зберігати дані після кожної зміни.</summary>
    public bool AutoSave { get; set; } = true;

    /// <summary>Виникає після будь-якої зміни даних у будь-якому репозиторії.</summary>
    public event EventHandler? DataChanged;

    public SchoolDatabase(IDataStorage storage)
    {
        _storage = storage;

        Students.Changed += OnRepositoryChanged;
        Teachers.Changed += OnRepositoryChanged;
        Groups.Changed += OnRepositoryChanged;
        Subjects.Changed += OnRepositoryChanged;
        Grades.Changed += OnRepositoryChanged;
        Payments.Changed += OnRepositoryChanged;
        Lessons.Changed += OnRepositoryChanged;
        Users.Changed += OnRepositoryChanged;
    }

    /// <summary>
    /// Завантажує дані зі сховища. Якщо сховище порожнє — заповнює демонстраційними даними.
    /// </summary>
    /// <returns>true, якщо було створено демонстраційні дані.</returns>
    public bool Load(Action<SchoolDatabase>? seed = null)
    {
        if (!_storage.Exists && seed != null)
        {
            RunWithoutSaving(() => seed(this));
            Save();
            return true;
        }

        var data = _storage.Load();
        InstitutionName = data.InstitutionName;
        Students.Load(data.Students);
        Teachers.Load(data.Teachers);
        Groups.Load(data.Groups);
        Subjects.Load(data.Subjects);
        Grades.Load(data.Grades);
        Payments.Load(data.Payments);
        Lessons.Load(data.Lessons);
        Users.Load(data.Users);
        return false;
    }

    public void Save()
    {
        _storage.Save(new SchoolData
        {
            InstitutionName = InstitutionName,
            Students = Students.Snapshot(),
            Teachers = Teachers.Snapshot(),
            Groups = Groups.Snapshot(),
            Subjects = Subjects.Snapshot(),
            Grades = Grades.Snapshot(),
            Payments = Payments.Snapshot(),
            Lessons = Lessons.Snapshot(),
            Users = Users.Snapshot()
        });
    }

    /// <summary>
    /// Виконує групу змін як одну операцію: збереження відбувається один раз у кінці.
    /// </summary>
    public void RunInBatch(Action action)
    {
        RunWithoutSaving(action);
        if (AutoSave) Save();
        DataChanged?.Invoke(this, EventArgs.Empty);
    }

    private void RunWithoutSaving(Action action)
    {
        var previous = _suspendSave;
        _suspendSave = true;
        try { action(); }
        finally { _suspendSave = previous; }
    }

    private void OnRepositoryChanged(object? sender, EventArgs e)
    {
        if (_suspendSave) return;
        if (AutoSave) Save();
        DataChanged?.Invoke(this, EventArgs.Empty);
    }
}
