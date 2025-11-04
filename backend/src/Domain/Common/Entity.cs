namespace Domain.Common;

/// <summary>
/// Base class for all domain entities.
/// Provides common identity and equality behavior.
/// </summary>
public abstract class Entity
{
    /// <summary>
    /// Unique identifier for the entity
    /// </summary>
    public Guid Id { get; protected set; }

    /// <summary>
    /// Parameterless constructor for EF Core
    /// </summary>
    protected Entity() { }

    /// <summary>
    /// Generates a new unique identifier for the entity
    /// </summary>
    protected void GenerateId()
    {
        if (Id == Guid.Empty)
        {
            Id = Guid.NewGuid();
        }
    }

    /// <summary>
    /// Determines whether two entities are equal based on their IDs
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (obj is not Entity other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        if (Id == Guid.Empty || other.Id == Guid.Empty)
            return false;

        return Id == other.Id;
    }

    /// <summary>
    /// Returns the hash code based on the entity's ID
    /// </summary>
    public override int GetHashCode()
    {
        return (GetType().ToString() + Id).GetHashCode();
    }

    /// <summary>
    /// Equality operator
    /// </summary>
    public static bool operator ==(Entity? a, Entity? b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    /// <summary>
    /// Inequality operator
    /// </summary>
    public static bool operator !=(Entity? a, Entity? b)
    {
        return !(a == b);
    }
}
