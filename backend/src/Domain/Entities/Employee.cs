using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.Entities;

/// <summary>
/// Employee entity representing a regular employee in the system.
/// Base class for employees and responsibles.
/// </summary>
public class Employee : User
{
    /// <summary>
    /// ID of the department this employee belongs to
    /// </summary>
    public Guid DepartmentId { get; private set; }

    /// <summary>
    /// Parameterless constructor for EF Core
    /// </summary>
    protected Employee() { }

    /// <summary>
    /// Protected constructor for factory method and derived classes
    /// </summary>
    protected Employee(string name, Email email, PasswordHash passwordHash, int roleId, Guid departmentId)
        : base(name, email, passwordHash, roleId)
    {
        ValidateDepartmentId(departmentId);
        DepartmentId = departmentId;
    }

    /// <summary>
    /// Factory method to create a new Employee instance
    /// </summary>
    /// <param name="name">Employee's name</param>
    /// <param name="email">Employee's email</param>
    /// <param name="passwordHash">Employee's password hash</param>
    /// <param name="departmentId">Department ID the employee belongs to</param>
    /// <returns>A new Employee instance</returns>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    public static Employee Create(string name, Email email, PasswordHash passwordHash, Guid departmentId)
        => new(name, email, passwordHash, Role.Employee.Id, departmentId);

    /// <summary>
    /// Updates the employee's department ID
    /// </summary>
    /// <param name="newDepartmentId">The new department ID</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    public void UpdateDepartmentId(Guid newDepartmentId)
    {
        ValidateDepartmentId(newDepartmentId);
        DepartmentId = newDepartmentId;
    }

    /// <summary>
    /// Updates employee's name, email and password atomically
    /// </summary>
    /// <param name="newName">The new name for the employee</param>
    /// <param name="newEmail">The new email for the employee</param>
    /// <param name="newPasswordHash">The new password hash for the employee</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    public void Update(string newName, Email newEmail, PasswordHash newPasswordHash)
    {
        UpdateCommon(newName, newEmail, newPasswordHash);
    }

    /// <summary>
    /// Updates employee's name, email, password and department atomically
    /// </summary>
    /// <param name="newName">The new name for the employee</param>
    /// <param name="newEmail">The new email for the employee</param>
    /// <param name="newPasswordHash">The new password hash for the employee</param>
    /// <param name="newDepartmentId">The new department ID</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    public void UpdateBasicInfo(string newName, Email newEmail, PasswordHash newPasswordHash, Guid newDepartmentId)
    {
        ValidateDepartmentId(newDepartmentId);
        UpdateCommon(newName, newEmail, newPasswordHash);
        DepartmentId = newDepartmentId;
    }

    /// <summary>
    /// Checks if the employee belongs to a specific department
    /// </summary>
    /// <param name="departmentId">The department ID to check</param>
    /// <returns>True if the employee belongs to the specified department; otherwise false</returns>
    public bool BelongsToDepartment(Guid departmentId) => DepartmentId == departmentId;

    #region Validation Methods

    /// <summary>
    /// Validates the department ID property
    /// </summary>
    /// <param name="departmentId">The department ID to validate</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    protected void ValidateDepartmentId(Guid departmentId)
    {
        if (departmentId == Guid.Empty)
            throw new InvalidEntityException(
                nameof(Employee),
                "Department ID cannot be empty");
    }

    #endregion
}