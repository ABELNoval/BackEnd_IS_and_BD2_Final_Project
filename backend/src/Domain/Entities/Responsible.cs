namespace Domain.Entities;
using Domain.ValueObjects;
/// <summary>
/// Responsible user type (inherits from Employee).
/// Employee with additional responsibilities: manages department operations and authorizes transfers.
/// Each department has one responsible.
/// </summary>
public class Responsible : Employee
{
    // EF Core constructor
    private Responsible() { }


    private Responsible(string name, Email email, PasswordHash passwordHash, Guid DepartmentId)
        : base(name, email, passwordHash, Role.Responsible.Id, DepartmentId) { }

    /// <summary>
    /// Creates a new Responsible instance.
    /// </summary>
    public new static Responsible Create(string name, Email email, PasswordHash passwordHash, Guid departmentId)
    {
        return new Responsible(name, email, passwordHash, departmentId);
    }
}