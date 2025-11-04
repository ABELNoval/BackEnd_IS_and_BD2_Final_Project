namespace Domain.Entities;

/// <summary>
/// Employee user type.
/// Base class for regular employees and responsibles.
/// </summary>
public class Employee : User
{
    // EF Core constructor
    protected Employee() { }

    protected Employee(string name, string email, string passwordHash, int roleId)
        : base(name, email, passwordHash, roleId) { }

    /// <summary>
    /// Creates a new Employee instance.
    /// </summary>
    public static Employee Create(string name, string email, string passwordHash)
    {
        return new Employee(name, email, passwordHash, Role.Employee.Id);
    }
}