using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Exceptions;

namespace Domain.ValueObjects
{
    /// <summary>
    /// Represents a technical's performance score based on assessments.
    /// Value must be between 0 and 100.
    /// Immutable value object that encapsulates performance evaluation logic.
    /// </summary>
    public sealed record PerformanceScore
    {
        /// <summary>
        /// Gets the performance score value (0-100)
        /// </summary>
        public decimal Value { get; }

        /// <summary>
        /// Private constructor with validation (always-valid pattern).
        /// Ensures that no invalid PerformanceScore instance can exist.
        /// </summary>
        /// <param name="value">The score value to set</param>
        /// <exception cref="DomainException">Thrown when value is outside the valid range (0-100)</exception>
        private PerformanceScore(decimal value)
        {
            // Validation happens in constructor to guarantee "always-valid" pattern
            // This protects against EF Core, reflection, and serialization
            if (value < 0 || value > 100)
                throw new DomainException("Performance score must be between 0 and 100.");

            Value = Math.Round(value, 2);
        }

        /// <summary>
        /// Creates a performance score with a specific value.
        /// </summary>
        /// <param name="value">The score value (must be between 0 and 100)</param>
        /// <returns>A new PerformanceScore instance</returns>
        /// <exception cref="DomainException">Thrown when value is outside the valid range</exception>
        public static PerformanceScore Create(decimal value)
        {
            return new PerformanceScore(value);
        }

        /// <summary>
        /// Calculates the average performance score from a collection of assessments.
        /// Returns a score of 0 if the collection is empty.
        /// </summary>
        /// <param name="assessments">Collection of assessments to calculate from</param>
        /// <returns>A new PerformanceScore with the calculated average</returns>
        /// <exception cref="ArgumentNullException">Thrown when assessments is null</exception>
        public static PerformanceScore FromAssessments(IEnumerable<Assessment> assessments)
        {
            ArgumentNullException.ThrowIfNull(assessments);

            var list = assessments.ToList();
            if (!list.Any())
                return new PerformanceScore(0);

            var average = list.Average(a => a.Score);
            return new PerformanceScore(average);
        }

        /// <summary>
        /// Returns a string representation of the performance score.
        /// </summary>
        /// <returns>The score value formatted as a percentage (e.g., "85.50%")</returns>
        public override string ToString() => $"{Value}%";
    }
}
