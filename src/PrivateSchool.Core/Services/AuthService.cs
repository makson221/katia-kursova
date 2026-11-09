using PrivateSchool.Core.Data;
using PrivateSchool.Core.Exceptions;
using PrivateSchool.Core.Interfaces;
using PrivateSchool.Core.Models;

namespace PrivateSchool.Core.Services;

/// <summary>
/// Автентифікація користувачів та рольова модель доступу.
/// </summary>
public class AuthService
{
    public const int MinPasswordLength = 6;

    private static readonly IReadOnlyDictionary<UserRole, HashSet<Permission>> RolePermissions =
        new Dictionary<UserRole, HashSet<Permission>>
        {
            [UserRole.Administrator] = Enum.GetValues<Permission>().ToHashSet(),
            [UserRole.Manager] = new()
            {
                Permission.ViewStudents, Permission.ManageStudents, Permission.ManageGroups,
                Permission.ViewPayments, Permission.ManagePayments, Permission.ManageSchedule,
                Permission.ViewReports
            },
            [UserRole.Teacher] = new()
            {
                Permission.ViewStudents, Permission.ManageGrades, Permission.ViewReports
            }
        };

    private readonly SchoolDatabase _db;
    private readonly IPasswordHasher _hasher;

    public AuthService(SchoolDatabase db, IPasswordHasher hasher)
    {
        _db = db;
        _hasher = hasher;
    }

    public User? CurrentUser { get; private set; }

    public bool IsAuthenticated => CurrentUser != null;

    public event EventHandler? CurrentUserChanged;

    public bool Login(string login, string password)
    {
        var user = _db.Users.Find(u => string.Equals(u.Login, login.Trim(), StringComparison.OrdinalIgnoreCase))
            .FirstOrDefault();

        if (user == null || !user.IsActive || !_hasher.Verify(password, user.PasswordHash))
            return false;

        var updated = (User)user.Clone();
        updated.LastLogin = DateTime.Now;
        _db.Users.Update(updated);

        CurrentUser = updated;
        CurrentUserChanged?.Invoke(this, EventArgs.Empty);
        return true;
    }

    public void Logout()
    {
        CurrentUser = null;
        CurrentUserChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool Can(Permission permission) =>
        CurrentUser != null && RolePermissions[CurrentUser.Role].Contains(permission);

    public static bool RoleHas(UserRole role, Permission permission) => RolePermissions[role].Contains(permission);

    public void Demand(Permission permission)
    {
        if (!Can(permission)) throw new DomainException("Недостатньо прав для виконання операції.");
    }

    public static IEnumerable<string> ValidatePassword(string password)
    {
        if (password.Length < MinPasswordLength) yield return $"Пароль має містити щонайменше {MinPasswordLength} символів.";
        if (!password.Any(char.IsLetter) || !password.Any(char.IsDigit)) yield return "Пароль має містити літери та цифри.";
    }

    public User CreateUser(User user, string password)
    {
        EnsurePasswordValid(password);
        EnsureLoginUnique(user);
        user.PasswordHash = _hasher.Hash(password);
        return _db.Users.Add(user);
    }

    public void UpdateUser(User user)
    {
        EnsureLoginUnique(user);
        if (CurrentUser?.Id == user.Id)
        {
            if (!user.IsActive) throw new DomainException("Не можна деактивувати власний обліковий запис.");
            if (user.Role != CurrentUser.Role) throw new DomainException("Не можна змінити власну роль.");
        }
        if (!IsAdminAfterChange(user) && CountActiveAdmins(exceptId: user.Id) == 0)
            throw new DomainException("У системі має залишитися хоча б один активний адміністратор.");

        _db.Users.Update(user);
        if (CurrentUser?.Id == user.Id) CurrentUser = user;
    }

    public void SetPassword(int userId, string newPassword)
    {
        EnsurePasswordValid(newPassword);
        var user = _db.Users.GetById(userId) ?? throw new DomainException("Користувача не знайдено.");
        var updated = (User)user.Clone();
        updated.PasswordHash = _hasher.Hash(newPassword);
        _db.Users.Update(updated);
        if (CurrentUser?.Id == userId) CurrentUser = updated;
    }

    public void ChangeOwnPassword(string oldPassword, string newPassword)
    {
        if (CurrentUser == null) throw new DomainException("Користувач не авторизований.");
        if (!_hasher.Verify(oldPassword, CurrentUser.PasswordHash))
            throw new DomainException("Поточний пароль введено неправильно.");
        if (oldPassword == newPassword)
            throw new DomainException("Новий пароль має відрізнятися від поточного.");
        SetPassword(CurrentUser.Id, newPassword);
    }

    internal int CountActiveAdmins(int exceptId = 0) =>
        _db.Users.Find(u => u.Id != exceptId && u.IsActive && u.Role == UserRole.Administrator).Count();

    private static bool IsAdminAfterChange(User user) => user.IsActive && user.Role == UserRole.Administrator;

    private static void EnsurePasswordValid(string password)
    {
        var errors = ValidatePassword(password).ToList();
        if (errors.Count > 0) throw new ValidationException(errors);
    }

    private void EnsureLoginUnique(User user)
    {
        if (_db.Users.Exists(u => u.Id != user.Id &&
                                  string.Equals(u.Login, user.Login, StringComparison.OrdinalIgnoreCase)))
            throw new DomainException($"Логін \"{user.Login}\" вже зайнятий.");
    }
}
