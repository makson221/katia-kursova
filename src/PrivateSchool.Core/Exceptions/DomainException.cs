namespace PrivateSchool.Core.Exceptions;

/// <summary>
/// Базовий виняток предметної області (порушення бізнес-правила).
/// </summary>
public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}

/// <summary>
/// Виняток, що виникає при спробі зберегти некоректну сутність.
/// </summary>
public class ValidationException : DomainException
{
    public IReadOnlyList<string> Errors { get; }

    public ValidationException(IEnumerable<string> errors)
        : this(errors.ToList()) { }

    private ValidationException(List<string> errors)
        : base(string.Join(Environment.NewLine, errors))
    {
        Errors = errors;
    }
}
