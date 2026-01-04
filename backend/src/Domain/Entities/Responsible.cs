using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.Entities;

/// <summary>
/// Responsible entity representing a responsible user in the system.
/// Extends Employee with additional responsibilities for managing department operations and authorizing transfers.
/// Each department has one responsible.
/// </summary>
public class Responsible : Employee
{
    /// <summary>
    /// Parameterless constructor for EF Core
    /// </summary>
    private Responsible() { }

    /// <summary>
    /// Private constructor for factory method
    /// </summary>
    private Responsible(string name, Email email, PasswordHash passwordHash, Guid departmentId)
        : base(name, email, passwordHash, Role.Responsible.Id, departmentId)
    {
    }

    /// <summary>
    /// Factory method to create a new Responsible instance
    /// </summary>
    /// <param name="name">Responsible's name</param>
    /// <param name="email">Responsible's email</param>
    /// <param name="passwordHash">Responsible's password hash</param>
    /// <param name="departmentId">Department ID the responsible manages</param>
    /// <returns>A new Responsible instance</returns>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    public new static Responsible Create(string name, Email email, PasswordHash passwordHash, Guid departmentId)
        => new(name, email, passwordHash, departmentId);
}