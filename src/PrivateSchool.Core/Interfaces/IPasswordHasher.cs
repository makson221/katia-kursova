namespace PrivateSchool.Core.Interfaces;

/// <summary>
/// Стратегія хешування паролів.
/// </summary>
public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hash);
}
