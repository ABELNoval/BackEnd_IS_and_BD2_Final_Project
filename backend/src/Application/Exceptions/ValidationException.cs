using FluentValidation.Results;

namespace Application.Exceptions;

/// <summary>
/// Exception thrown when validation of a DTO fails.
/// </summary>
public class ValidationException : ApplicationException
{
    public IReadOnlyList<ValidationFailure> Errors { get; }

    public ValidationException(IEnumerable<ValidationFailure> errors)
        : base("One or more validation failures occurred")
    {
        Errors = errors.ToList().AsReadOnly();
    }
}