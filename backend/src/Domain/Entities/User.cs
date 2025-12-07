using Domain.Common;
using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.Entities;

/// <summary>
/// Base user entity for the Technical Equipment Management System.
/// </summary>
public abstract class User : Entity
{
    public string Name { get; private set; } = string.Empty;
    public Email Email { get; private set; } = default!;
    public PasswordHash PasswordHash { get; private set; } = default!;
    public int RoleId { get; private set; }

    // EF Core constructor
    protected User() { }

    protected User(string name, Email email, PasswordHash passwordHash, int roleId)
    {
        GenerateId();
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        RoleId = roleId;

        Validate();
    }

    protected void UpdateCommon(string name, Email email, PasswordHash passwordHash)
    {
        SetName(name);

        if (email is not null)
            Email = email;

        if (passwordHash is not null)
            PasswordHash = passwordHash;

        Validate();
    }

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidEntityException(nameof(User), "Name cannot be empty");

        if (name.Length > 100)
            throw new InvalidEntityException(nameof(User), "Name cannot exceed 100 characters");

        Name = name.Trim();
    }


    private void Validate()
    {
        const int MaxNameLength = 100;

        if (Id == Guid.Empty)
            throw new InvalidEntityException(nameof(User), "User ID cannot be empty");

        if (string.IsNullOrWhiteSpace(Name))
            throw new InvalidEntityException(nameof(User), "Name cannot be empty");

        if (Name.Length > MaxNameLength)
            throw new InvalidEntityException(nameof(User), $"Name cannot exceed {MaxNameLength} characters");

        // Email and PasswordHash are validated by their own ValueObjects when created

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