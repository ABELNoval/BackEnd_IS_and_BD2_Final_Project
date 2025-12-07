using Domain.Common;
using Domain.Exceptions;

namespace Domain.Entities;

/// <summary>
/// Represents a department in the organization.
/// Departments are organizational units that own equipment.
/// </summary>
public class Department : Entity
{
    public string Name { get; private set; } = string.Empty;
    public Guid SectionId { get; private set; }

    protected Department() { }

    private Department(string name, Guid sectionId)
    {
        GenerateId();
        Name = name?.Trim();
        SectionId = sectionId;
        Validate();
    }

    public static Department Create(string name, Guid sectionId)
    {
        return new Department(name, sectionId);
    }

    private void Validate()
    {
        const int MaxNameLength = 100;

        if (Id == Guid.Empty)
            throw new InvalidEntityException(nameof(Department), "Department ID cannot be empty");

        if (string.IsNullOrWhiteSpace(Name))
            throw new InvalidEntityException(nameof(Department), "Name cannot be empty");

        if (Name.Length > MaxNameLength)
            throw new InvalidEntityException(nameof(Department), $"Name cannot exceed {MaxNameLength} characters");

        if (SectionId == Guid.Empty)
            throw new InvalidEntityException(nameof(Department), "Section ID cannot be empty");
    }

    public bool BelongsToSection(Guid sectionId) => SectionId == sectionId;

    public void Update(string name, Guid sectionId)
    {
        if (!string.IsNullOrWhiteSpace(name))
            Name = name.Trim();

        if (sectionId != Guid.Empty)
            SectionId = sectionId;

        Validate();
    }

}