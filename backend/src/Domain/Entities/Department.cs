using Domain.Common;
using Domain.Exceptions;

namespace Domain.Entities;

/// <summary>
/// Represents a department in the organization.
/// Departments are organizational units that own equipment and belong to a section.
/// </summary>
public class Department : Entity
{
    private const int MaxNameLength = 100;
    private const int MinNameLength = 2;

    /// <summary>
    /// Name of the department
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// ID of the section this department belongs to
    /// </summary>
    public Guid SectionId { get; private set; }

    /// <summary>
    /// Parameterless constructor for EF Core
    /// </summary>
    private Department() { }

    /// <summary>
    /// Private constructor with validation (Always-Valid pattern)
    /// </summary>
    private Department(string name, Guid sectionId)
    {
        GenerateId();
        ValidateName(name);
        ValidateSectionId(sectionId);
        Name = name.Trim();
        SectionId = sectionId;
    }

    /// <summary>
    /// Creates a new Department instance
    /// </summary>
    /// <param name="name">The name of the department</param>
    /// <param name="sectionId">The ID of the section this department belongs to</param>
    /// <returns>A new valid Department instance</returns>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    public static Department Create(string name, Guid sectionId)
        => new(name, sectionId);

    /// <summary>
    /// Updates the department name
    /// </summary>
    /// <param name="newName">The new name for the department</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    public void UpdateName(string newName)
    {
        ValidateName(newName);
        Name = newName.Trim();
    }

    /// <summary>
    /// Updates the section this department belongs to
    /// </summary>
    /// <param name="newSectionId">The new section ID</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    public void UpdateSection(Guid newSectionId)
    {
        ValidateSectionId(newSectionId);
        SectionId = newSectionId;
    }

    /// <summary>
    /// Updates both name and section in a single atomic operation
    /// </summary>
    /// <param name="newName">The new name for the department</param>
    /// <param name="newSectionId">The new section ID</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    public void UpdateBasicInfo(string newName, Guid newSectionId)
    {
        ValidateName(newName);
        ValidateSectionId(newSectionId);
        Name = newName.Trim();
        SectionId = newSectionId;
    }

    /// <summary>
    /// Checks if this department belongs to the given section
    /// </summary>
    /// <param name="sectionId">The section ID to check</param>
    /// <returns>True if the department belongs to the given section; otherwise false</returns>
    public bool BelongsToSection(Guid sectionId) => SectionId == sectionId;

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
                nameof(Department),
                "Name cannot be empty");

        var trimmedName = name.Trim();

        if (trimmedName.Length < MinNameLength)
            throw new InvalidEntityException(
                nameof(Department),
                $"Name must be at least {MinNameLength} characters");

        if (trimmedName.Length > MaxNameLength)
            throw new InvalidEntityException(
                nameof(Department),
                $"Name cannot exceed {MaxNameLength} characters");
    }

    /// <summary>
    /// Validates the section ID property
    /// </summary>
    /// <param name="sectionId">The section ID to validate</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    private void ValidateSectionId(Guid sectionId)
    {
        if (sectionId == Guid.Empty)
            throw new InvalidEntityException(
                nameof(Department),
                "Section ID cannot be empty");
    }

    /// <summary>
    /// Validates the entire entity
    /// </summary>
    /// <exception cref="InvalidEntityException">If entity validation fails</exception>
    private void Validate()
    {
        if (Id == Guid.Empty)
            throw new InvalidEntityException(
                nameof(Department),
                "Department ID cannot be empty");

        ValidateName(Name);
        ValidateSectionId(SectionId);
    }

    #endregion
}