namespace Application.Exceptions;

/// <summary>
/// Exception thrown when an entity is not found in the repository.
/// </summary>
public class EntityNotFoundException : ApplicationException
{
    public string EntityName { get; }
    public object EntityId { get; }

    public EntityNotFoundException(string entityName, object entityId)
        : base($"{entityName} with ID '{entityId}' was not found")
    {
        EntityName = entityName;
        EntityId = entityId;
    }
}
