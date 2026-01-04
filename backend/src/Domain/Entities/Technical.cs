using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.Entities;

/// <summary>
/// Technical entity representing a technical employee in the system.
/// A technical can perform maintenance on equipment and receive assessments from directors.
/// </summary>
public class Technical : User
{
    private const int MinExperience = 0;
    private const int MaxExperience = 100;
    private const int MaxSpecialtyLength = 100;
    private const int MinSpecialtyLength = 2;

    private readonly List<Assessment> _assessments = new();

    /// <summary>
    /// Collection of assessments received by this technical
    /// </summary>
    public IReadOnlyCollection<Assessment> Assessments => _assessments.AsReadOnly();

    /// <summary>
    /// Years of experience the technical has
    /// </summary>
    public int Experience { get; private set; }

    /// <summary>
    /// Technical specialty or area of expertise
    /// </summary>
    public string Specialty { get; private set; } = string.Empty;

    /// <summary>
    /// Parameterless constructor for EF Core
    /// </summary>
    private Technical() { }

    /// <summary>
    /// Private constructor for factory method
    /// </summary>
    private Technical(string name, Email email, PasswordHash passwordHash, int experience, string specialty)
        : base(name, email, passwordHash, Role.Technical.Id)
    {
        ValidateExperience(experience);
        ValidateSpecialty(specialty);
        
        Experience = experience;
        Specialty = specialty.Trim();
    }

    /// <summary>
    /// Factory method to create a new Technical instance
    /// </summary>
    /// <param name="name">Technical's name</param>
    /// <param name="email">Technical's email</param>
    /// <param name="passwordHash">Technical's password hash</param>
    /// <param name="experience">Years of experience</param>
    /// <param name="specialty">Technical specialty</param>
    /// <returns>A new Technical instance</returns>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    public static Technical Create(string name, Email email, PasswordHash passwordHash, int experience, string specialty)
        => new(name, email, passwordHash, experience, specialty);

    /// <summary>
    /// Updates the technical's experience
    /// </summary>
    /// <param name="newExperience">The new experience value</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    public void UpdateExperience(int newExperience)
    {
        ValidateExperience(newExperience);
        Experience = newExperience;
    }

    /// <summary>
    /// Updates the technical's specialty
    /// </summary>
    /// <param name="newSpecialty">The new specialty</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    public void UpdateSpecialty(string newSpecialty)
    {
        ValidateSpecialty(newSpecialty);
        Specialty = newSpecialty.Trim();
    }

    /// <summary>
    /// Updates the technical's name, email and password atomically
    /// </summary>
    /// <param name="newName">The new name for the technical</param>
    /// <param name="newEmail">The new email for the technical</param>
    /// <param name="newPasswordHash">The new password hash for the technical</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    public void Update(string newName, Email newEmail, PasswordHash newPasswordHash)
    {
        UpdateCommon(newName, newEmail, newPasswordHash);
    }

    /// <summary>
    /// Updates technical's name, email, password, experience and specialty atomically
    /// </summary>
    /// <param name="newName">The new name for the technical</param>
    /// <param name="newEmail">The new email for the technical</param>
    /// <param name="newPasswordHash">The new password hash for the technical</param>
    /// <param name="newExperience">The new experience value</param>
    /// <param name="newSpecialty">The new specialty</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    public void UpdateBasicInfo(string newName, Email newEmail, PasswordHash newPasswordHash, int newExperience, string newSpecialty)
    {
        ValidateExperience(newExperience);
        ValidateSpecialty(newSpecialty);
        UpdateCommon(newName, newEmail, newPasswordHash);
        Experience = newExperience;
        Specialty = newSpecialty.Trim();
    }

    /// <summary>
    /// Adds an assessment from a director to this technical
    /// </summary>
    /// <param name="directorId">The director giving the assessment</param>
    /// <param name="scoreValue">The performance score</param>
    /// <param name="comment">Assessment comment</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    public void AddAssessment(Guid directorId, decimal scoreValue, string comment)
    {
        ValidateGuidProperty(directorId, nameof(directorId));
        
        var assessment = Assessment.Create(Id, directorId, scoreValue, comment);
        _assessments.Add(assessment);
    }

    /// <summary>
    /// Gets all assessments given by a specific director
    /// </summary>
    /// <param name="directorId">The director ID to filter by</param>
    /// <returns>Collection of assessments from the specified director</returns>
    public IEnumerable<Assessment> GetAssessmentsByDirector(Guid directorId)
        => _assessments.Where(a => a.WasGivenBy(directorId)).ToList();

    /// <summary>
    /// Checks if the technical has any assessments
    /// </summary>
    /// <returns>True if the technical has assessments; otherwise false</returns>
    public bool HasAssessments() => _assessments.Count > 0;

    /// <summary>
    /// Checks if technical has assessments from a specific director
    /// </summary>
    /// <param name="directorId">The director ID to check</param>
    /// <returns>True if the technical has assessments from the director; otherwise false</returns>
    public bool HasAssessmentsFrom(Guid directorId)
        => _assessments.Any(a => a.WasGivenBy(directorId));

    #region Validation Methods

    /// <summary>
    /// Validates a generic Guid property
    /// </summary>
    /// <param name="id">The ID to validate</param>
    /// <param name="propertyName">The name of the property being validated</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    private void ValidateGuidProperty(Guid id, string propertyName)
    {
        if (id == Guid.Empty)
            throw new InvalidEntityException(
                nameof(Technical),
                $"{propertyName} cannot be empty");
    }

    /// <summary>
    /// Validates the experience property
    /// </summary>
    /// <param name="experience">The experience value to validate</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    private void ValidateExperience(int experience)
    {
        if (experience < MinExperience)
            throw new InvalidEntityException(
                nameof(Technical),
                $"Experience cannot be less than {MinExperience}");

        if (experience > MaxExperience)
            throw new InvalidEntityException(
                nameof(Technical),
                $"Experience cannot exceed {MaxExperience} years");
    }

    /// <summary>
    /// Validates the specialty property
    /// </summary>
    /// <param name="specialty">The specialty to validate</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    private void ValidateSpecialty(string specialty)
    {
        if (string.IsNullOrWhiteSpace(specialty))
            throw new InvalidEntityException(
                nameof(Technical),
                "Specialty cannot be empty");

        var trimmedSpecialty = specialty.Trim();

        if (trimmedSpecialty.Length < MinSpecialtyLength)
            throw new InvalidEntityException(
                nameof(Technical),
                $"Specialty must be at least {MinSpecialtyLength} characters");

        if (trimmedSpecialty.Length > MaxSpecialtyLength)
            throw new InvalidEntityException(
                nameof(Technical),
                $"Specialty cannot exceed {MaxSpecialtyLength} characters");
    }

    #endregion
}