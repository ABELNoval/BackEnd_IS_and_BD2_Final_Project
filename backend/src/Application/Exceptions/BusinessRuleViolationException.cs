using System;

namespace Application.Exceptions
{
    public class BusinessRuleViolationException : Exception
    {
        public string RuleName { get; }
        public string EntityName { get; }
        public object EntityId { get; }

        public BusinessRuleViolationException(string ruleName, string message) 
            : base(message)
        {
            RuleName = ruleName;
        }

        public BusinessRuleViolationException(string message) 
            : base(message)
        {
            RuleName = "BusinessRuleViolation";
        }

        public BusinessRuleViolationException(string ruleName, string entityName, object entityId, string message)
            : base($"{entityName} ({entityId}): {message}")
        {
            RuleName = ruleName;
            EntityName = entityName;
            EntityId = entityId;
        }

        public BusinessRuleViolationException(string ruleName, string entityName, string message)
            : base($"{entityName}: {message}")
        {
            RuleName = ruleName;
            EntityName = entityName;
        }
    }
}