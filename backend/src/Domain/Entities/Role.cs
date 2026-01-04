using Domain.Common;

namespace Domain.Entities;

/// <summary>
/// User roles in the Technical Equipment Management System.
/// This is a type-safe enumeration with predefined values.
/// </summary>
public class Role : Enumeration
{
    public static readonly Role Administrator = new(1, "Administrator");
    public static readonly Role Director = new(2, "Director");
    public static readonly Role Technical = new(3, "Technical");
    public static readonly Role Employee = new(4, "Employee");
    public static readonly Role Responsible = new(5, "Responsible");
    public static readonly Role Receptor = new(6, "Receptor");

    private Role(int id, string name) : base(id, name) { }

    /// <summary>
    /// Gets all available roles
    /// </summary>
    public static IEnumerable<Role> GetAll()
    {
        yield return Administrator;
        yield return Director;
        yield return Technical;
        yield return Employee;
        yield return Responsible;
        yield return Receptor;
    }

    /// <summary>
    /// Gets a role by ID
    /// </summary>
    public static Role? FromId(int id)
    {
        return GetAll().FirstOrDefault(r => r.Id == id);
    }

    /// <summary>
    /// Gets a role by name
    /// </summary>
    public static Role? FromName(string name)
    {
        return GetAll().FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}