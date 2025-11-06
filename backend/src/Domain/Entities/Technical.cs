using Domain.Exceptions;

namespace Domain.Entities;

/// <summary>
/// Technical user type.
/// Can perform maintenance on equipment and receive assessments from directors.
/// </summary>
public class Technical : User
{
    private readonly List<Assessment> _assessments = new();

    public IReadOnlyCollection<Assessment> Assessments => _assessments.AsReadOnly();
    public int Experience { get; private set; }
    public string Specialty { get; private set; } = string.Empty;

    private Technical() { }

    private Technical(string name, string email, string passwordHash, int experience, string specialty)
        : base(name, email, passwordHash, Role.Technical.Id)
    {
        Experience = experience;
        Specialty = specialty.Trim();
        ValidateTechnical();
    }

    public static Technical Create(string name, string email, string passwordHash, int experience, string specialty)
    {
        return new Technical(name, email, passwordHash, experience, specialty);
    }

    private void ValidateTechnical()
    {
        if (Experience < 0)
            throw new InvalidEntityException(nameof(Technical), "Experience cannot be negative");

        if (string.IsNullOrWhiteSpace(Specialty))
            throw new InvalidEntityException(nameof(Technical), "Specialty cannot be empty");
    }

    /// <summary>
    /// Adds an assessment from a director.
    /// References director by ID only.
    /// </summary>
    public void AddAssessment(Guid directorId, decimal scoreValue, string comment)
    {
        if (directorId == Guid.Empty)
            throw new InvalidEntityException(
                nameof(Assessment),
                "Director ID cannot be empty");

        var assessment = Assessment.Create(Id, directorId, scoreValue, comment);
        _assessments.Add(assessment);
    }
}