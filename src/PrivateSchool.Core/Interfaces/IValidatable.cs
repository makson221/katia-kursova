namespace PrivateSchool.Core.Interfaces;

/// <summary>
/// Об'єкт, що вміє перевіряти коректність власного стану.
/// </summary>
public interface IValidatable
{
    /// <summary>Повертає перелік помилок валідації (порожній, якщо об'єкт коректний).</summary>
    IEnumerable<string> Validate();
}
