using System;

namespace Application.Exceptions
{

    public class NotFoundException : Exception
    {
        public string EntityName { get; }
        public object EntityId { get; }

        public NotFoundException(string entityName, object entityId) 
            : base($"{entityName} with ID '{entityId}' was not found")
        {
            EntityName = entityName;
            EntityId = entityId;
        }

        public NotFoundException(string entityName, string searchCriteria)
            : base($"{entityName} with criteria '{searchCriteria}' was not found")
        {
            EntityName = entityName;
        }

        public NotFoundException(string message) 
            : base(message) 
        { 
        }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}