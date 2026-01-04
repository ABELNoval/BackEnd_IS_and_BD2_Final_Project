using Domain.Common;
using Domain.Exceptions;

namespace Domain.Entities;

/// <summary>
/// Represents a type/category of equipment.
/// Equipment types are managed entities that define categories for equipment in the system.
/// </summary>
public class EquipmentType : Entity
{
    private const int MaxNameLength = 100;
    private const int MinNameLength = 2;

    /// <summary>
    /// Name of the equipment type
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Parameterless constructor for EF Core
    /// </summary>
    private EquipmentType() { }

    /// <summary>
    /// Private constructor with validation (Always-Valid pattern)
    /// </summary>
    private EquipmentType(string name)
    {
        GenerateId();
        ValidateName(name);
        Name = name.Trim();
    }

    /// <summary>
    /// Creates a new EquipmentType instance
    /// </summary>
    /// <param name="name">The name of the equipment type</param>
    /// <returns>A new valid EquipmentType instance</returns>
    /// <exception cref="InvalidEntityException">If name validation fails</exception>
    public static EquipmentType Create(string name)
        => new(name);

    /// <summary>
    /// Updates the equipment type name
    /// </summary>
    /// <param name="newName">The new name for the equipment type</param>
    /// <exception cref="InvalidEntityException">If name validation fails</exception>
    public void UpdateName(string newName)
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
                nameof(EquipmentType),
                "Name cannot be empty");

        var trimmedName = name.Trim();

        if (trimmedName.Length < MinNameLength)
            throw new InvalidEntityException(
                nameof(EquipmentType),
                $"Name must be at least {MinNameLength} characters");

        if (trimmedName.Length > MaxNameLength)
            throw new InvalidEntityException(
                nameof(EquipmentType),
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
                nameof(EquipmentType),
                "EquipmentType ID cannot be empty");

        ValidateName(Name);
    }

    #endregion
}