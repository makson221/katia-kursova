using PrivateSchool.Core.Interfaces;

namespace PrivateSchool.Core.Data;

/// <summary>
/// Сховище в пам'яті — використовується у модульних тестах.
/// </summary>
public class InMemoryDataStorage : IDataStorage
{
    public SchoolData? Data { get; private set; }
    public int SaveCount { get; private set; }

    public bool Exists => Data != null;

    public SchoolData Load() => Data ?? new SchoolData();

    public void Save(SchoolData data)
    {
        Data = data;
        SaveCount++;
    }
}
