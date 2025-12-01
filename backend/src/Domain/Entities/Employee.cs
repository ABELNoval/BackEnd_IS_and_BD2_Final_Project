namespace Domain.Entities;
using Domain.ValueObjects;  
/// <summary>
/// Employee user type.
/// Base class for regular employees and responsibles.
/// </summary>
public class Employee : User
{
    // EF Core constructor
    protected Employee() { }

    protected Employee(string name, Email email, PasswordHash passwordHash, int roleId)
        : base(name, email, passwordHash, roleId) { }

    /// <summary>
    /// Creates a new Employee instance.
    /// </summary>
    public static Employee Create(string name, Email email, PasswordHash passwordHash)
    {
        return new Employee(name, email, passwordHash, Role.Employee.Id);
    }
}