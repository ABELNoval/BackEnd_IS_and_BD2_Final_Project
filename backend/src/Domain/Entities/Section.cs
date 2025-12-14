using Domain.Common;
using Domain.Exceptions;

namespace Domain.Entities;

/// <summary>
/// Represents a section in the organization.
/// Sections group multiple departments and are managed by a responsible person.
/// </summary>
public class Section : Entity
{
    private const int MaxNameLength = 100;
    private const int MinNameLength = 2;

    /// <summary>
    /// Name of the section
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Parameterless constructor for EF Core
    /// </summary>
    private Section() { }

    /// <summary>
    /// Private constructor with validation (Always-Valid pattern)
    /// </summary>
    private Section(string name)
    {
        GenerateId();
        ValidateName(name);
        Name = name.Trim();
    }

    /// <summary>
    /// Creates a new Section instance
    /// </summary>
    /// <param name="name">The name of the section</param>
    /// <returns>A new valid Section instance</returns>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    public static Section Create(string name)
        => new(name);

    /// <summary>
    /// Updates the section name
    /// </summary>
    /// <param name="newName">The new name for the section</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    public void UpdateName(string newName)
    {
        ValidateName(newName);
        Name = newName.Trim();
    }

    /// <summary>
    /// Updates both name and responsible in a single atomic operation
    /// </summary>
    /// <param name="newName">The new name for the section</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    public void UpdateBasicInfo(string newName)
    {
        ValidateName(newName);
        Name = newName.Trim();
    }

    #region Validation Methods

    /// <summary>
    /// Validates the name property
    /// </summary>
    /// <param name="name">The name to validate</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    private void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidEntityException(
                nameof(Section),
                "Name cannot be empty");

        var trimmedName = name.Trim();

        if (trimmedName.Length < MinNameLength)
            throw new InvalidEntityException(
                nameof(Section),
                $"Name must be at least {MinNameLength} characters");

        if (trimmedName.Length > MaxNameLength)
            throw new InvalidEntityException(
                nameof(Section),
                $"Name cannot exceed {MaxNameLength} characters");
    }

    /// <summary>
    /// Validates the entire entity
    /// </summary>
    /// <exception cref="InvalidEntityException">If entity validation fails</exception>
    private void Validate()
    {
        if (Id == Guid.Empty)
            throw new InvalidEntityException(
                nameof(Section),
                "Section ID cannot be empty");

        ValidateName(Name);
    }

    #endregion
}