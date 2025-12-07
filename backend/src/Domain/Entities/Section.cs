using Domain.Common;
using Domain.Exceptions;

namespace Domain.Entities;

/// <summary>
/// Represents a section in the organization.
/// Sections group multiple departments.
/// </summary>
public class Section : Entity
{
    public string Name { get; private set; } = string.Empty;
    public Guid ResponsibleId { get; private set; } // moved here

    protected Section() { }

    // Updated constructor to accept responsibleId
    private Section(string name, Guid responsibleId)
    {
        GenerateId();
        Name = name?.Trim();
        ResponsibleId = responsibleId;
        Validate();
    }

    // Updated Create signature
    public static Section Create(string name, Guid responsibleId)
    {
        return new Section(name, responsibleId);
    }

    private void Validate()
    {
        const int MaxNameLength = 100;

        if (Id == Guid.Empty)
            throw new InvalidEntityException(nameof(Section), "Section ID cannot be empty");

        if (string.IsNullOrWhiteSpace(Name))
            throw new InvalidEntityException(nameof(Section), "Name cannot be empty");

        if (Name.Length > MaxNameLength)
            throw new InvalidEntityException(nameof(Section), $"Name cannot exceed {MaxNameLength} characters");

        if (ResponsibleId == Guid.Empty)
            throw new InvalidEntityException(nameof(Section), "Responsible ID cannot be empty");
    }

    public bool HasResponsible(Guid responsibleId) => ResponsibleId == responsibleId;

    public void Update(string name, Guid responsibleId)
{
    Name = name?.Trim();
    ResponsibleId = responsibleId;
    Validate();
}

}