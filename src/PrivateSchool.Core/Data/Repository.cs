using PrivateSchool.Core.Exceptions;
using PrivateSchool.Core.Interfaces;
using PrivateSchool.Core.Models;

namespace PrivateSchool.Core.Data;

/// <summary>
/// Реалізація репозиторію в оперативній пам'яті з автоінкрементом ідентифікаторів
/// і валідацією перед збереженням.
/// </summary>
public class Repository<T> : IRepository<T> where T : Entity
{
    private readonly List<T> _items = new();
    private int _nextId = 1;

    public event EventHandler? Changed;

    public int Count => _items.Count;

    public IReadOnlyList<T> GetAll() => _items.AsReadOnly();

    public T? GetById(int id) => _items.FirstOrDefault(x => x.Id == id);

    public IEnumerable<T> Find(Func<T, bool> predicate) => _items.Where(predicate);

    public bool Exists(Func<T, bool> predicate) => _items.Any(predicate);

    public T Add(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        EnsureValid(item);

        item.Id = _nextId++;
        _items.Add(item);
        OnChanged();
        return item;
    }

    public void Update(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        var index = _items.FindIndex(x => x.Id == item.Id);
        if (index < 0) throw new DomainException($"Запис з ідентифікатором {item.Id} не знайдено.");

        EnsureValid(item);
        _items[index] = item;
        OnChanged();
    }

    public bool Remove(int id)
    {
        var removed = _items.RemoveAll(x => x.Id == id) > 0;
        if (removed) OnChanged();
        return removed;
    }

    public int RemoveWhere(Func<T, bool> predicate)
    {
        var count = _items.RemoveAll(x => predicate(x));
        if (count > 0) OnChanged();
        return count;
    }

    /// <summary>Завантаження збережених записів (без валідації та без генерації подій).</summary>
    internal void Load(IEnumerable<T>? items)
    {
        _items.Clear();
        if (items != null) _items.AddRange(items);
        _nextId = _items.Count == 0 ? 1 : _items.Max(x => x.Id) + 1;
    }

    internal List<T> Snapshot() => new(_items);

    private static void EnsureValid(T item)
    {
        var errors = item.Validate().ToList();
        if (errors.Count > 0) throw new ValidationException(errors);
    }

    protected virtual void OnChanged() => Changed?.Invoke(this, EventArgs.Empty);
}
