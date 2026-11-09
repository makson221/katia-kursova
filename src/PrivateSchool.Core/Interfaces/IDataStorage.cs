using PrivateSchool.Core.Data;

namespace PrivateSchool.Core.Interfaces;

/// <summary>
/// Абстракція сховища даних. Дозволяє замінити JSON-файл на БД без зміни решти коду.
/// </summary>
public interface IDataStorage
{
    bool Exists { get; }
    SchoolData Load();
    void Save(SchoolData data);
}
