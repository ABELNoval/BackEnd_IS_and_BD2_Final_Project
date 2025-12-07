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
    public Guid DepartmentId { get; private set; }

    protected Employee(string name, Email email, PasswordHash passwordHash, int roleId, Guid departmentId)
        : base(name, email, passwordHash, roleId) => SetDepartmentId(departmentId);

    /// <summary>
    /// Creates a new Employee instance.
    /// </summary>
    public static Employee Create(string name, Email email, PasswordHash passwordHash, Guid departmentId)
    {
        return new Employee(name, email, passwordHash, Role.Employee.Id, departmentId);
    }

    public void SetDepartmentId(Guid departmentId) => DepartmentId = departmentId;
}