namespace Domain.Entities;

/// <summary>
/// Director user type.
/// Can assess technicals and approve equipment operations.
/// </summary>
public class Director : User
{
    private Director() { }

    private Director(string name, string email, string passwordHash)
        : base(name, email, passwordHash, Role.Director.Id) { }

    public static Director Create(string name, string email, string passwordHash)
    {
        return new Director(name, email, passwordHash);
    }
}