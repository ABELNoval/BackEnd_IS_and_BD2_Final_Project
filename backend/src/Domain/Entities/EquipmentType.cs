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

    protected EquipmentType() { }

    private EquipmentType(string name)
    {
        GenerateId();
        Name = name.Trim();
        Validate();
    }

    public static EquipmentType Create(string name)
    {
        return new EquipmentType(name);
    }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new InvalidEntityException(nameof(EquipmentType), "Name cannot be empty");
    }
}