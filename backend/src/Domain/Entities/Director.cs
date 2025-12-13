using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.Entities;

/// <summary>
/// Director entity representing a director user in the system.
/// A director can assess technical employees and manage equipment.
/// </summary>
public class Director : User
{
    /// <summary>
    /// Parameterless constructor for EF Core
    /// </summary>
    private Director() { }

    /// <summary>
    /// Private constructor for factory method
    /// </summary>
    private Director(string name, Email email, PasswordHash passwordHash)
        : base(name, email, passwordHash, Role.Director.Id)
    {
    }

    /// <summary>
    /// Factory method to create a new Director instance
    /// </summary>
    /// <param name="name">Director's name</param>
    /// <param name="email">Director's email</param>
    /// <param name="passwordHash">Director's password hash</param>
    /// <returns>A new Director instance</returns>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    public static Director Create(string name, Email email, PasswordHash passwordHash)
        => new(name, email, passwordHash);

    /// <summary>
    /// Updates director's name, email and password atomically
    /// </summary>
    /// <param name="newName">The new name for the director</param>
    /// <param name="newEmail">The new email for the director</param>
    /// <param name="newPasswordHash">The new password hash for the director</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    public void Update(string newName, Email newEmail, PasswordHash newPasswordHash)
    {
        UpdateCommon(newName, newEmail, newPasswordHash);
    }
}