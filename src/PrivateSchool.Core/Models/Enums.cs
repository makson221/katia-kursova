using System.ComponentModel;

namespace PrivateSchool.Core.Models;

public enum StudentStatus
{
    [Description("Навчається")] Active,
    [Description("Академвідпустка")] AcademicLeave,
    [Description("Відрахований")] Expelled,
    [Description("Випускник")] Graduated
}

public enum GradeType
{
    [Description("Іспит")] Exam,
    [Description("Залік")] Credit,
    [Description("Модульна робота")] Module,
    [Description("Лабораторна робота")] Lab,
    [Description("Практична робота")] Practice
}

public enum ControlForm
{
    [Description("Іспит")] Exam,
    [Description("Залік")] Credit
}

public enum PaymentMethod
{
    [Description("Готівка")] Cash,
    [Description("Банківська картка")] Card,
    [Description("Банківський переказ")] BankTransfer
}

public enum LessonType
{
    [Description("Лекція")] Lecture,
    [Description("Практичне")] Practice,
    [Description("Лабораторне")] Lab
}

public enum UserRole
{
    [Description("Адміністратор")] Administrator,
    [Description("Менеджер / бухгалтер")] Manager,
    [Description("Викладач")] Teacher
}
