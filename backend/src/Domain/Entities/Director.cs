namespace Domain.Entities;
using Domain.ValueObjects;
/// <summary>
/// Director user type.
/// Can assess technicals and approve equipment operations.
/// </summary>
public class Director : User
{
    private Director() { }

    private Director(string name, Email email, PasswordHash passwordHash)
        : base(name, email, passwordHash, Role.Director.Id) { }

    public static Director Create(string name, Email email, PasswordHash passwordHash)
    {
        return new Director(name, email, passwordHash);
    }
}