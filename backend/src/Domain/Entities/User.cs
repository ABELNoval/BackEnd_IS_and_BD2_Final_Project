using Domain.Common;
using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.Entities;

/// <summary>
/// Base user entity for the Technical Equipment Management System.
/// Abstract base class for all user types in the system.
/// </summary>
public abstract class User : Entity
{
    private const int MaxNameLength = 100;
    private const int MinNameLength = 2;

    /// <summary>
    /// Name of the user
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Email address of the user (Value Object)
    /// </summary>
    public Email Email { get; private set; } = default!;

    /// <summary>
    /// Password hash of the user (Value Object)
    /// </summary>
    public PasswordHash PasswordHash { get; private set; } = default!;

    /// <summary>
    /// ID of the role assigned to this user
    /// </summary>
    public int RoleId { get; private set; }

    /// <summary>
    /// Parameterless constructor for EF Core
    /// </summary>
    protected User() { }

    /// <summary>
    /// Protected constructor for derived classes
    /// </summary>
    protected User(string name, Email email, PasswordHash passwordHash, int roleId)
    {
        GenerateId();
        ValidateName(name);
        ValidateRoleId(roleId);
        
        Name = name.Trim();
        Email = email;
        PasswordHash = passwordHash;
        RoleId = roleId;
    }

    /// <summary>
    /// Updates the user's name
    /// </summary>
    /// <param name="newName">The new name for the user</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    public void UpdateName(string newName)
    {
        ValidateName(newName);
        Name = newName.Trim();
    }

    /// <summary>
    /// Updates the user's email
    /// </summary>
    /// <param name="newEmail">The new email for the user</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    public void UpdateEmail(Email newEmail)
    {
        if (newEmail == null)
            throw new InvalidEntityException(
                nameof(User),
                "Email cannot be null");
        Email = newEmail;
    }

    /// <summary>
    /// Updates the user's password hash
    /// </summary>
    /// <param name="newPasswordHash">The new password hash for the user</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    public void UpdatePasswordHash(PasswordHash newPasswordHash)
    {
        if (newPasswordHash == null)
            throw new InvalidEntityException(
                nameof(User),
                "Password hash cannot be null");
        PasswordHash = newPasswordHash;
    }

    /// <summary>
    /// Updates name, email and password hash atomically
    /// </summary>
    /// <param name="newName">The new name for the user</param>
    /// <param name="newEmail">The new email for the user</param>
    /// <param name="newPasswordHash">The new password hash for the user</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    protected void UpdateCommon(string newName, Email newEmail, PasswordHash newPasswordHash)
    {
        ValidateName(newName);
        
        if (newEmail != null)
            UpdateEmail(newEmail);

        if (newPasswordHash != null)
            UpdatePasswordHash(newPasswordHash);

        Name = newName.Trim();
    }

    /// <summary>
    /// Gets the role object for this user
    /// </summary>
    /// <returns>The role assigned to this user</returns>
    /// <exception cref="InvalidEntityException">If role ID is invalid</exception>
    public Role GetRole() => Role.FromId(RoleId) 
        ?? throw new InvalidEntityException(
            nameof(User), 
            $"Invalid role ID: {RoleId}");

    /// <summary>
    /// Checks if the user has a specific role
    /// </summary>
    /// <param name="role">The role to check</param>
    /// <returns>True if the user has the specified role; otherwise false</returns>
    public bool HasRole(Role role) => RoleId == role.Id;

    /// <summary>
    /// Checks if the user is a Technical
    /// </summary>
    /// <returns>True if the user is a Technical; otherwise false</returns>
    public bool IsTechnical() => RoleId == Role.Technical.Id;

    /// <summary>
    /// Checks if the user is a Director
    /// </summary>
    /// <returns>True if the user is a Director; otherwise false</returns>
    public bool IsDirector() => RoleId == Role.Director.Id;

    /// <summary>
    /// Checks if the user is an Employee
    /// </summary>
    /// <returns>True if the user is an Employee; otherwise false</returns>
    public bool IsEmployee() => RoleId == Role.Employee.Id;

    /// <summary>
    /// Checks if the user is a Responsible
    /// </summary>
    /// <returns>True if the user is a Responsible; otherwise false</returns>
    public bool IsResponsible() => RoleId == Role.Responsible.Id;

    #region Validation Methods

    /// <summary>
    /// Validates the name property
    /// </summary>
    /// <param name="name">The name to validate</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    protected void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidEntityException(
                nameof(User),
                "Name cannot be empty");

        var trimmedName = name.Trim();

        if (trimmedName.Length < MinNameLength)
            throw new InvalidEntityException(
                nameof(User),
                $"Name must be at least {MinNameLength} characters");

        if (trimmedName.Length > MaxNameLength)
            throw new InvalidEntityException(
                nameof(User),
                $"Name cannot exceed {MaxNameLength} characters");
    }

    /// <summary>
    /// Validates the role ID property
    /// </summary>
    /// <param name="roleId">The role ID to validate</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    protected void ValidateRoleId(int roleId)
    {
        var validRole = Role.FromId(roleId);
        if (validRole == null)
            throw new InvalidEntityException(
                nameof(User),
                $"Invalid role ID: {roleId}");
    }

    /// <summary>
    /// Validates the entire user entity
    /// </summary>
    /// <exception cref="InvalidEntityException">If entity validation fails</exception>
    protected void Validate()
    {
        if (Id == Guid.Empty)
            throw new InvalidEntityException(
                nameof(User),
                "User ID cannot be empty");

        ValidateName(Name);
        ValidateRoleId(RoleId);
        
        // Email and PasswordHash are validated by their own ValueObjects when created
    }

    #endregion
}