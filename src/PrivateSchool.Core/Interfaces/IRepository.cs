using PrivateSchool.Core.Models;

namespace PrivateSchool.Core.Interfaces;

/// <summary>
/// Узагальнений репозиторій для роботи з колекцією сутностей (шаблон Repository).
/// </summary>
/// <typeparam name="T">Тип сутності.</typeparam>
public interface IRepository<T> where T : Entity
{
    /// <summary>Подія, що виникає після будь-якої зміни колекції.</summary>
    event EventHandler? Changed;

    int Count { get; }

    IReadOnlyList<T> GetAll();
    T? GetById(int id);
    IEnumerable<T> Find(Func<T, bool> predicate);
    bool Exists(Func<T, bool> predicate);

    T Add(T item);
    void Update(T item);
    bool Remove(int id);
    int RemoveWhere(Func<T, bool> predicate);
}
