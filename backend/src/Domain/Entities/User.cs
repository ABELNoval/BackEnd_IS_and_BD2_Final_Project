using Domain.Common;
using Domain.Exceptions;

namespace Domain.Entities;

/// <summary>
/// Base user entity for the Technical Equipment Management System.
/// </summary>
public abstract class User : Entity
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public int RoleId { get; private set; }

    // EF Core constructor
    protected User() { }

    protected User(string name, string email, string passwordHash, int roleId)
    {
        GenerateId();
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        RoleId = roleId;

        Validate();
    }

    private void Validate()
    {
        const int MaxNameLength = 100;
        const int MaxEmailLength = 150;
        const int MaxPasswordHashLength = 255;

        if (string.IsNullOrWhiteSpace(Name))
            throw new InvalidEntityException(nameof(User), "Name cannot be empty");

        if (Name.Length > MaxNameLength)
            throw new InvalidEntityException(nameof(User), $"Name cannot exceed {MaxNameLength} characters");

        if (string.IsNullOrWhiteSpace(Email))
            throw new InvalidEntityException(nameof(User), "Email cannot be empty");

        if (Email.Length > MaxEmailLength)
            throw new InvalidEntityException(nameof(User), $"Email cannot exceed {MaxEmailLength} characters");

        if (!Email.Contains("@"))
            throw new InvalidEntityException(nameof(User), "Email format is invalid");

        if (string.IsNullOrWhiteSpace(PasswordHash))
            throw new InvalidEntityException(nameof(User), "Password hash cannot be empty");

        if (PasswordHash.Length > MaxPasswordHashLength)
            throw new InvalidEntityException(nameof(User), $"Password hash cannot exceed {MaxPasswordHashLength} characters");

        var validRole = Role.FromId(RoleId);
        if (validRole == null)
            throw new InvalidEntityException(nameof(User), $"Invalid role ID: {RoleId}");
    }

    public Role GetRole() => Role.FromId(RoleId) 
        ?? throw new InvalidEntityException(nameof(User), $"Invalid role ID: {RoleId}");
    public bool HasRole(Role role) => RoleId == role.Id;
    public bool IsTechnical() => RoleId == Role.Technical.Id;
    public bool IsDirector() => RoleId == Role.Director.Id;
    public bool IsEmployee() => RoleId == Role.Employee.Id;
    public bool IsResponsible() => RoleId == Role.Responsible.Id;
}