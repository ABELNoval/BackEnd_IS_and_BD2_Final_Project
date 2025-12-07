using Domain.Exceptions;

namespace Domain.ValueObjects;

/// <summary>
/// Value object that represents an email address.
/// Encapsula validación mínima (no vacío, longitud y formato con '@').
/// </summary>
public sealed record Email
{
    public string Value { get;}

    private Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidValueObjectException(nameof(Email), value ?? string.Empty, "Email cannot be empty");

        var trimmed = value.Trim();
        const int MaxEmailLength = 150;
        if (trimmed.Length > MaxEmailLength)
            throw new InvalidValueObjectException(nameof(Email), trimmed, $"Email cannot exceed {MaxEmailLength} characters");

        if (!trimmed.Contains("@"))
            throw new InvalidValueObjectException(nameof(Email), trimmed, "Email format is invalid");

        Value = trimmed;
    }

    public static Email Create(string value) => new Email(value);

    public override string ToString() => Value;
}
