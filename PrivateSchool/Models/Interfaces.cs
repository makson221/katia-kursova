namespace PrivateSchool.Models
{
    /// <summary>
    /// Сутність, що має унікальний ідентифікатор.
    /// </summary>
    public interface IEntity
    {
        int Id { get; set; }
    }

    /// <summary>
    /// Об'єкт, який уміє перевіряти коректність власних даних.
    /// У разі помилки метод Validate() генерує ValidationException.
    /// </summary>
    public interface IValidatable
    {
        void Validate();
    }

    /// <summary>
    /// Об'єкт, який можна знайти за текстовим запитом.
    /// </summary>
    public interface ISearchable
    {
        bool Matches(string query);
    }
}
