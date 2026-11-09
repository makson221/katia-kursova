using System;

namespace PrivateSchool.Models
{
    /// <summary>
    /// Абстрактний базовий клас для всіх сутностей системи.
    /// Реалізує спільні інтерфейси та допоміжні методи валідації.
    /// </summary>
    [Serializable]
    public abstract class EntityBase : IEntity, IValidatable, ISearchable
    {
        public int Id { get; set; }

        /// <summary>
        /// Кожна сутність зобов'язана визначити власні правила перевірки (поліморфізм).
        /// </summary>
        public abstract void Validate();

        /// <summary>
        /// Пошук за замовчуванням — за текстовим представленням об'єкта.
        /// Похідні класи можуть перевизначити логіку пошуку.
        /// </summary>
        public virtual bool Matches(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return true;
            return Contains(ToString(), query);
        }

        /// <summary>
        /// Створює поверхневу копію об'єкта (використовується у формах редагування,
        /// щоб зміни не потрапляли в дані до натискання кнопки «Зберегти»).
        /// </summary>
        public T Clone<T>() where T : EntityBase
        {
            return (T)MemberwiseClone();
        }

        protected static void Require(bool condition, string message)
        {
            if (!condition)
                throw new ValidationException(message);
        }

        protected static bool Contains(string source, string query)
        {
            return !string.IsNullOrEmpty(source)
                   && source.IndexOf(query.Trim(), StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        protected static string Clean(string value)
        {
            return (value ?? string.Empty).Trim();
        }
    }
}
