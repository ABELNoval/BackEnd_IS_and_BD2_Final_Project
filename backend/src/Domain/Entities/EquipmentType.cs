using Domain.Common;
using Domain.Exceptions;

namespace Domain.Entities;

/// <summary>
/// Represents a type/category of equipment.
/// Equipment types are managed entities that can be created by users.
/// </summary>
public class EquipmentType : Entity
{
    public string Name { get; private set; }

    protected EquipmentType() 
    {
        Name = string.Empty;
    }

    private EquipmentType(string name)
    {
        GenerateId();
        Name = name?.Trim();
        Validate();
    }

    public static EquipmentType Create(string name)
    {
        return new EquipmentType(name);
    }

    private void Validate()
    {
        const int MaxNameLength = 100;

        if (Id == Guid.Empty)
            throw new InvalidEntityException(nameof(EquipmentType), "EquipmentType ID cannot be empty");

        if (string.IsNullOrWhiteSpace(Name))
            throw new InvalidEntityException(nameof(EquipmentType), "Name cannot be empty");

        if (Name.Length > MaxNameLength)
            throw new InvalidEntityException(nameof(EquipmentType), $"Name cannot exceed {MaxNameLength} characters");
    }
}