using Domain.Exceptions;
using System.Security.Cryptography;
using System.Text;

namespace Domain.ValueObjects;

/// <summary>
/// Value object that represents a password hash.
/// Encapsula validación mínima (no vacío, longitud razonable).
/// </summary>
public sealed record PasswordHash
{
    public string Value { get; }

    private PasswordHash(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidValueObjectException(nameof(PasswordHash), value ?? string.Empty, "Password hash cannot be empty");

        var trimmed = value.Trim();
        const int MaxPasswordHashLength = 255;
        if (trimmed.Length > MaxPasswordHashLength)
            throw new InvalidValueObjectException(nameof(PasswordHash), trimmed, $"Password hash cannot exceed {MaxPasswordHashLength} characters");

        Value = trimmed;
    }

    /// <summary>
    /// Creates a PasswordHash from a persisted hash string (e.g., loaded from DB).
    /// Does NOT re-hash; it only validates wrapper constraints.
    /// </summary>
    public static PasswordHash Create(string persistedHash) => new PasswordHash(persistedHash);

    /// <summary>
    /// Creates a PasswordHash from a plaintext password using PBKDF2.
    /// Format: PBKDF2$<iterations>$<saltBase64>$<hashBase64>$SHA256
    /// </summary>
    public static PasswordHash CreateFromPlainText(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new InvalidValueObjectException(nameof(PasswordHash), string.Empty, "Password cannot be empty");

        const int iterations = 100_000;
        const int saltSize = 16;   // 128-bit
        const int hashSize = 32;   // 256-bit

        Span<byte> salt = stackalloc byte[saltSize];
        RandomNumberGenerator.Fill(salt);

        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt.ToArray(), iterations, HashAlgorithmName.SHA256);
        var hash = pbkdf2.GetBytes(hashSize);

        var saltB64 = Convert.ToBase64String(salt);
        var hashB64 = Convert.ToBase64String(hash);

        var formatted = $"PBKDF2${iterations}${saltB64}${hashB64}$SHA256";
        return new PasswordHash(formatted);
    }

    /// <summary>
    /// Verifies that the provided plaintext password matches this stored hash.
    /// </summary>
    public bool Verify(string password)
    {
        if (string.IsNullOrEmpty(password)) return false;

        // Expected format: PBKDF2$<iterations>$<saltB64>$<hashB64>$SHA256
        var parts = Value.Split('$');
        if (parts.Length != 5 || !string.Equals(parts[0], "PBKDF2", StringComparison.Ordinal))
            return false;

        if (!int.TryParse(parts[1], out var iterations))
            return false;

        try
        {
            var salt = Convert.FromBase64String(parts[2]);
            var expectedHash = Convert.FromBase64String(parts[3]);
            var algo = parts[4];
            if (!string.Equals(algo, "SHA256", StringComparison.OrdinalIgnoreCase))
                return false;

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
            var actualHash = pbkdf2.GetBytes(expectedHash.Length);

            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }
        catch
        {
            return false;
        }
    }

    public override string ToString() => Value;
}
