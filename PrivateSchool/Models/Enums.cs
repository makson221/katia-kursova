using System.ComponentModel;

namespace PrivateSchool.Models
{
    public enum StudentStatus
    {
        [Description("Навчається")] Active,
        [Description("Академвідпустка")] Suspended,
        [Description("Завершив навчання")] Graduated,
        [Description("Відрахований")] Expelled
    }

    public enum TeacherCategory
    {
        [Description("Спеціаліст")] Specialist,
        [Description("Друга категорія")] Second,
        [Description("Перша категорія")] First,
        [Description("Вища категорія")] Highest
    }

    public enum CourseLevel
    {
        [Description("Початковий")] Beginner,
        [Description("Середній")] Intermediate,
        [Description("Високий")] Advanced
    }

    public enum PaymentMethod
    {
        [Description("Готівка")] Cash,
        [Description("Банківська картка")] Card,
        [Description("Безготівковий переказ")] BankTransfer
    }

    public enum GradeKind
    {
        [Description("Поточна")] Current,
        [Description("Контрольна робота")] Test,
        [Description("Тематична")] Thematic,
        [Description("Підсумкова")] Final
    }

    public enum UserRole
    {
        [Description("Адміністратор")] Administrator,
        [Description("Менеджер")] Manager
    }
}
