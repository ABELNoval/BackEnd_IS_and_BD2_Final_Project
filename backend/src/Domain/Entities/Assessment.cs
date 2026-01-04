using Domain.Common;
using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.Entities;

/// <summary>
/// Represents a performance assessment given by a Director to a Technical employee.
/// This entity belongs to the Technical aggregate.
/// </summary>
public class Assessment : Entity
{
    private const int MaxCommentLength = 500;
    private const int MinCommentLength = 5;

    /// <summary>
    /// ID of the Technical being assessed (Aggregate Root)
    /// </summary>
    public Guid TechnicalId { get; private set; }

    /// <summary>
    /// ID of the Director who performed the assessment
    /// </summary>
    public Guid DirectorId { get; private set; }

    /// <summary>
    /// Performance score (0-100) as a Value Object
    /// </summary>
    public PerformanceScore Score { get; private set; }

    /// <summary>
    /// Comment about the assessment
    /// </summary>
    public string Comment { get; private set; } = string.Empty;

    /// <summary>
    /// Date and time when the assessment was created
    /// </summary>
    public DateTime AssessmentDate { get; private set; }

    /// <summary>
    /// Parameterless constructor for EF Core
    /// </summary>
    protected Assessment()
    {
        Score = PerformanceScore.Create(0);
    }

    /// <summary>
    /// Private constructor with validation (Always-Valid pattern)
    /// </summary>
    private Assessment(
        Guid technicalId,
        Guid directorId,
        PerformanceScore score,
        string comment)
    {
        GenerateId();
        ValidateGuidProperty(technicalId, "Technical ID");
        ValidateGuidProperty(directorId, "Director ID");
        ValidateComment(comment);
        
        TechnicalId = technicalId;
        DirectorId = directorId;
        Score = score ?? PerformanceScore.Create(0);
        Comment = comment.Trim();
        AssessmentDate = DateTime.UtcNow;
    }

    /// <summary>
    /// Creates a new Assessment instance with a raw score value
    /// </summary>
    /// <param name="technicalId">ID of the technical being assessed</param>
    /// <param name="directorId">ID of the director performing the assessment</param>
    /// <param name="scoreValue">Performance score value (0-100)</param>
    /// <param name="comment">Comment about the assessment</param>
    /// <returns>A new valid Assessment instance</returns>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    public static Assessment Create(
        Guid technicalId,
        Guid directorId,
        decimal scoreValue,
        string comment)
    {
        var score = PerformanceScore.Create(scoreValue);
        return new Assessment(technicalId, directorId, score, comment);
    }

    /// <summary>
    /// Creates a new Assessment instance with a PerformanceScore object
    /// </summary>
    /// <param name="technicalId">ID of the technical being assessed</param>
    /// <param name="directorId">ID of the director performing the assessment</param>
    /// <param name="score">Performance score object</param>
    /// <param name="comment">Comment about the assessment</param>
    /// <returns>A new valid Assessment instance</returns>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    public static Assessment Create(
        Guid technicalId,
        Guid directorId,
        PerformanceScore score,
        string comment)
        => new(technicalId, directorId, score, comment);

    /// <summary>
    /// Updates the assessment score
    /// </summary>
    /// <param name="newScoreValue">The new score value (0-100)</param>
    public void UpdateScore(decimal newScoreValue)
    {
        Score = Score.Update(newScoreValue);
    }

    /// <summary>
    /// Updates the assessment comment
    /// </summary>
    /// <param name="newComment">The new comment for the assessment</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    public void UpdateComment(string newComment)
    {
        ValidateComment(newComment);
        Comment = newComment.Trim();
    }

    /// <summary>
    /// Updates both score and comment atomically
    /// </summary>
    /// <param name="newScoreValue">The new score value (0-100)</param>
    /// <param name="newComment">The new comment for the assessment</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    public void UpdateAssessment(decimal newScoreValue, string newComment)
    {
        Score = Score.Update(newScoreValue);
        ValidateComment(newComment);
        Comment = newComment.Trim();
    }

    /// <summary>
    /// Checks if this assessment was given to a specific technical
    /// </summary>
    /// <param name="technicalId">The technical ID to check</param>
    /// <returns>True if this assessment was given to the specified technical; otherwise false</returns>
    public bool IsForTechnical(Guid technicalId) => TechnicalId == technicalId;

    /// <summary>
    /// Checks if this assessment was given by a specific director
    /// </summary>
    /// <param name="directorId">The director ID to check</param>
    /// <returns>True if this assessment was given by the specified director; otherwise false</returns>
    public bool WasGivenBy(Guid directorId) => DirectorId == directorId;

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
                nameof(Assessment),
                $"{propertyName} cannot be empty");
    }

    /// <summary>
    /// Validates the comment property
    /// </summary>
    /// <param name="comment">The comment to validate</param>
    /// <exception cref="InvalidEntityException">If validation fails</exception>
    private void ValidateComment(string comment)
    {
        if (string.IsNullOrWhiteSpace(comment))
            throw new InvalidEntityException(
                nameof(Assessment),
                "Comment cannot be empty");

        var trimmedComment = comment.Trim();

        if (trimmedComment.Length < MinCommentLength)
            throw new InvalidEntityException(
                nameof(Assessment),
                $"Comment must be at least {MinCommentLength} characters");

        if (trimmedComment.Length > MaxCommentLength)
            throw new InvalidEntityException(
                nameof(Assessment),
                $"Comment cannot exceed {MaxCommentLength} characters. Current length: {trimmedComment.Length}");
    }

    /// <summary>
    /// Validates the entire entity
    /// </summary>
    /// <exception cref="InvalidEntityException">If entity validation fails</exception>
    private void Validate()
    {
        if (Id == Guid.Empty)
            throw new InvalidEntityException(
                nameof(Assessment),
                "Assessment ID cannot be empty");

        ValidateGuidProperty(TechnicalId, "Technical ID");
        ValidateGuidProperty(DirectorId, "Director ID");
        ValidateComment(Comment);
    }

    #endregion
}