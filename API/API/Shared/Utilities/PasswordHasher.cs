using System.Security.Cryptography;
using System.Text;

namespace API.Shared.Utilities;

public static class PasswordHasher
{
    public static string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes);
    }

    public static bool VerifyPassword(string password, string hash)
    {
        var inputHash = HashPassword(password);
        return string.Equals(inputHash, hash, StringComparison.OrdinalIgnoreCase);
    }
}