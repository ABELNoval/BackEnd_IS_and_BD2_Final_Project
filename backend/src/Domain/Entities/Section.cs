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

    protected Section() { }

    private Section(string name)
    {
        GenerateId();
        Name = name.Trim();
        Validate();
    }

    public static Section Create(string name)
    {
        return new Section(name);
    }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new InvalidEntityException(nameof(Section), "Name cannot be empty");
    }
}