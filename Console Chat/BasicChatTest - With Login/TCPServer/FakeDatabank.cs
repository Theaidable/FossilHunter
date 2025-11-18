using System.Security.Cryptography;
using System.Text;

/// <summary>
/// En falsk database så længe serveren er åben.
/// Det bliver gemt lokalt, da der ikke er nogen database endnu
/// Koden bliver gemt med SALT+Hash gennem PBKDF2, da jeg kan forstå at princip er det samme
/// hash (password + salt) sammenligned ved login
/// </summary>
public class UserStore
{
    // Slår et brugernavn op til (salt, hash).
    private readonly Dictionary<string, (byte[] Salt, byte[] Hash)> _users = new Dictionary<string, (byte[] Salt, byte[] Hash)>(StringComparer.OrdinalIgnoreCase);

    // Forsøg at oprette ny bruger. false hvis brugernavnet findes.
    public bool TryAddUser(string username, string password)
    {
        if (_users.ContainsKey(username))
        {
            return false;
        }

        // Lav et tilfældigt salt og en tung hash (PBKDF2).
        byte[] salt = RandomNumberGenerator.GetBytes(16);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 100_000, HashAlgorithmName.SHA256, 32);

        _users[username] = (salt, hash);

        return true;
    }

    // Tjek om username/password passer: hash samme måde og sammenlign.
    public bool Validate(string username, string password)
    {
        if (!_users.TryGetValue(username, out var rec))
        {
            return false;
        }

        byte[] hash2 = Rfc2898DeriveBytes.Pbkdf2(password, rec.Salt, 100_000, HashAlgorithmName.SHA256, 32);

        // FixedTimeEquals undgår timing-angreb
        return CryptographicOperations.FixedTimeEquals(hash2, rec.Hash);
    }
}
