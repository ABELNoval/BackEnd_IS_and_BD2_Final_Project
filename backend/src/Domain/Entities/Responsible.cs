namespace Domain.Entities;

/// <summary>
/// Responsible user type (inherits from Employee).
/// Employee with additional responsibilities: manages department operations and authorizes transfers.
/// Each department has one responsible.
/// </summary>
public class Responsible : Employee
{
    // EF Core constructor
    private Responsible() { }

    private Responsible(string name, string email, string passwordHash)
        : base(name, email, passwordHash, Role.Responsible.Id) { }

    /// <summary>
    /// Creates a new Responsible instance.
    /// </summary>
    public static Responsible Create(string name, string email, string passwordHash)
    {
        return new Responsible(name, email, passwordHash);
    }
}