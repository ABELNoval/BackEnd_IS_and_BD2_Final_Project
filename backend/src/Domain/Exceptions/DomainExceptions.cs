namespace Domain.Exceptions;

/// <summary>
/// Excepción base para todas las excepciones del dominio.
/// Se lanza cuando se viola una regla de negocio (invariante).
/// </summary>
public class DomainException : Exception
{
    public DomainException()
    {
    }

    public DomainException(string message) 
        : base(message)
    {
    }

    public DomainException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

/// <summary>
/// Excepción que se lanza cuando se intenta crear una entidad con datos inválidos
/// </summary>
public class InvalidEntityException : DomainException
{
    public InvalidEntityException(string entityName, string reason)
        : base($"Invalid {entityName}: {reason}")
    {
        EntityName = entityName;
        Reason = reason;
    }

    public string EntityName { get; }
    public string Reason { get; }
}

/// <summary>
/// Excepción que se lanza cuando se intenta crear un Value Object inválido
/// </summary>
public class InvalidValueObjectException : DomainException
{
    private string v;

    public InvalidValueObjectException(string message, string v) : base(message)
    {
        this.v = v;
    }

    public InvalidValueObjectException(string valueObjectName, string value, string reason)
        : base($"Invalid {valueObjectName} '{value}': {reason}")
    {
        ValueObjectName = valueObjectName;
        Value = value;
        Reason = reason;
    }

    public string ValueObjectName { get; }
    public string Value { get; }
    public string Reason { get; }
}

/// <summary>
/// Excepción que se lanza cuando se viola una regla de negocio
/// </summary>
public class BusinessRuleViolationException : DomainException
{
    public BusinessRuleViolationException(string ruleName, string details)
        : base($"Business rule violated: {ruleName}. {details}")
    {
        RuleName = ruleName;
        Details = details;
    }

    public string RuleName { get; }
    public string Details { get; }
}
