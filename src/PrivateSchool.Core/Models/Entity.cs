using PrivateSchool.Core.Interfaces;

namespace PrivateSchool.Core.Models;

/// <summary>
/// Базовий абстрактний клас для всіх сутностей предметної області.
/// Містить ідентифікатор, механізм валідації та клонування (шаблон Prototype).
/// </summary>
public abstract class Entity : IValidatable
{
    public int Id { get; set; }

    /// <summary>
    /// Перевірка коректності даних. Нащадки розширюють перелік правил,
    /// викликаючи базову реалізацію (поліморфізм).
    /// </summary>
    public virtual IEnumerable<string> Validate()
    {
        yield break;
    }

    public bool IsValid => !Validate().Any();

    /// <summary>Створює поверхневу копію сутності (для безпечного редагування у формах).</summary>
    public virtual Entity Clone() => (Entity)MemberwiseClone();

    public override bool Equals(object? obj) =>
        obj is Entity other && other.GetType() == GetType() && other.Id == Id && Id != 0;

    public override int GetHashCode() => HashCode.Combine(GetType(), Id);
}
