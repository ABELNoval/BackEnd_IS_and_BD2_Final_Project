using Domain.Exceptions;

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

    public bool Verify(string plainTextPassword)
    {
        return Value ==  plainTextPassword;
    }

    public static PasswordHash Create(string value) => new PasswordHash(value);

    /// <summary>
    /// Updates the password hash value.
    /// </summary>
    /// <param name="newValue">The new password hash</param>
    /// <returns>A new PasswordHash instance with the updated value</returns>
    /// <exception cref="InvalidValueObjectException">If validation fails</exception>
    public PasswordHash Update(string newValue) => new PasswordHash(newValue);

    public override string ToString() => Value;
}
