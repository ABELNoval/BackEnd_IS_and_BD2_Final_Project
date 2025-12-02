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
    /// <summary>
    /// ID of the Technical being assessed (Aggregate Root)
    /// </summary>
    public Guid TechnicalId { get; private set; }

    /// <summary>
    /// ID of the Director who performed the assessment (Different Aggregate - Only ID)
    /// </summary>
    public Guid DirectorId { get; private set; }

    /// <summary>
    /// Performance score (0-100) as a Value Object
    /// </summary>
    public PerformanceScore Score { get; private set; }

    /// <summary>
    /// Comment about the assessment (required)
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
        Score = PerformanceScore.Create(0); // Default value for EF Core
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
        TechnicalId = technicalId;
        DirectorId = directorId;
        Score = score;
        Comment = comment.Trim();
        AssessmentDate = DateTime.UtcNow;

        ValidateAssessment();
    }

    /// <summary>
    /// Creates a new Assessment instance
    /// </summary>
    /// <param name="technicalId">ID of the technical being assessed</param>
    /// <param name="directorId">ID of the director performing the assessment</param>
    /// <param name="scoreValue">Performance score value (0-100)</param>
    /// <param name="comment">Comment about the assessment (required)</param>
    /// <returns>A new valid Assessment instance</returns>
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
    public static Assessment Create(
        Guid technicalId,
        Guid directorId,
        PerformanceScore score,
        string comment)
    {
        return new Assessment(technicalId, directorId, score, comment);
    }

    /// <summary>
    /// Updates the assessment score
    /// </summary>
    public void UpdateScore(decimal newScoreValue)
    {
        Score = PerformanceScore.Create(newScoreValue);
    }

    /// <summary>
    /// Updates the assessment comment
    /// </summary>
    public void UpdateComment(string newComment)
    {
        ValidateComment(newComment);
        Comment = newComment.Trim();
    }

    #region Validation Methods

    private void ValidateAssessment()
    {
        if (Id == Guid.Empty)
            throw new InvalidEntityException(
                nameof(Assessment),
                "Assessment ID cannot be empty");

        if (TechnicalId == Guid.Empty)
            throw new InvalidEntityException(
                nameof(Assessment),
                "Technical ID cannot be empty");

        if (DirectorId == Guid.Empty)
            throw new InvalidEntityException(
                nameof(Assessment),
                "Director ID cannot be empty");

        ValidateComment(Comment);
    }

    private void ValidateComment(string comment)
    {
        const int MaxCommentLength = 500;

        if (string.IsNullOrWhiteSpace(comment))
            throw new InvalidEntityException(
                nameof(Assessment),
                "Comment cannot be empty");

        if (comment.Length > MaxCommentLength)
            throw new InvalidEntityException(
                nameof(Assessment),
                $"Comment cannot exceed {MaxCommentLength} characters. Current length: {comment.Length}");
    }

    #endregion

    public override string ToString()
    {
        return $"Assessment [{Score}] by Director {DirectorId} on {AssessmentDate:yyyy-MM-dd}";
    }
}