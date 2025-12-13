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
    /// ID of the Responsible person managing this section
    /// </summary>
    public Guid ResponsibleId { get; private set; }

    /// <summary>
    /// Parameterless constructor for EF Core
    /// </summary>
    private Section() { }

    /// <summary>
    /// Private constructor with validation (Always-Valid pattern)
    /// </summary>
    private Section(string name, Guid responsibleId)
    {
        GenerateId();
        ValidateName(name);
        ValidateResponsibleId(responsibleId);
        Name = name.Trim();
        ResponsibleId = responsibleId;
    }

    /// <summary>
    /// Creates a new Section instance
    /// </summary>
    /// <param name="name">The name of the section</param>
    /// <param name="responsibleId">The ID of the responsible person managing this section</param>
    /// <returns>A new valid Section instance</returns>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    public static Section Create(string name, Guid responsibleId)
        => new(name, responsibleId);

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
    /// Updates the responsible person for this section
    /// </summary>
    /// <param name="newResponsibleId">The new responsible person ID</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    public void UpdateResponsible(Guid newResponsibleId)
    {
        ValidateResponsibleId(newResponsibleId);
        ResponsibleId = newResponsibleId;
    }

    /// <summary>
    /// Updates both name and responsible in a single atomic operation
    /// </summary>
    /// <param name="newName">The new name for the section</param>
    /// <param name="newResponsibleId">The new responsible person ID</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    public void UpdateBasicInfo(string newName, Guid newResponsibleId)
    {
        ValidateName(newName);
        ValidateResponsibleId(newResponsibleId);
        Name = newName.Trim();
        ResponsibleId = newResponsibleId;
    }

    /// <summary>
    /// Checks if this section is managed by the given responsible person
    /// </summary>
    /// <param name="responsibleId">The ID of the responsible to check</param>
    /// <returns>True if the section is managed by the given responsible; otherwise false</returns>
    public bool HasResponsible(Guid responsibleId) => ResponsibleId == responsibleId;

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
    /// Validates the responsible ID property
    /// </summary>
    /// <param name="responsibleId">The ID to validate</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    private void ValidateResponsibleId(Guid responsibleId)
    {
        if (responsibleId == Guid.Empty)
            throw new InvalidEntityException(
                nameof(Section),
                "Responsible ID cannot be empty");
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
        ValidateResponsibleId(ResponsibleId);
    }

    #endregion
}