using System;

namespace PrivateSchool.Models
{
    /// <summary>
    /// Виняток, що виникає при порушенні правил валідації даних
    /// або бізнес-правил предметної області.
    /// </summary>
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message)
        {
        }
    }
}
