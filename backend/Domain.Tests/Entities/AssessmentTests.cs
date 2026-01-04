using System;
using Domain.Entities;
using Domain.Exceptions;
using Xunit;

namespace Domain.Tests.Entities
{
    public class AssessmentTests
    {
        [Fact]
        public void Create_WithValidParameters_CreatesAssessment()
        {
            // Arrange
            var technicalId = Guid.NewGuid();
            var directorId = Guid.NewGuid();
            var score = Domain.ValueObjects.PerformanceScore.Create(85m);
            var comment = "Evaluación satisfactoria";
            var builder = new Domain.Tests.Fixtures.AssessmentBuilder()
                .WithTechnicalId(technicalId)
                .WithDirectorId(directorId)
                .WithScore(score)
                .WithComment(comment);

            // Act
            var assessment = builder.Build();

            // Assert
            Assert.Equal(technicalId, assessment.TechnicalId);
            Assert.Equal(directorId, assessment.DirectorId);
            Assert.Equal(score.Value, assessment.Score.Value);
            Assert.Equal(comment, assessment.Comment);
            Assert.True((DateTime.UtcNow - assessment.AssessmentDate).TotalSeconds < 5);
        }

        [Fact]
        public void Create_WithEmptyTechnicalId_ThrowsInvalidEntityException()
        {
            // Arrange
            var builder = new Domain.Tests.Fixtures.AssessmentBuilder()
                .WithTechnicalId(Guid.Empty);

            // Act & Assert
            Assert.Throws<InvalidEntityException>(() => builder.Build());
        }

        [Fact]
        public void Create_WithEmptyDirectorId_ThrowsInvalidEntityException()
        {
            // Arrange
            var builder = new Domain.Tests.Fixtures.AssessmentBuilder()
                .WithDirectorId(Guid.Empty);

            // Act & Assert
            Assert.Throws<InvalidEntityException>(() => builder.Build());
        }

        [Fact]
        public void Create_WithEmptyComment_ThrowsInvalidEntityException()
        {
            // Arrange
            var builder = new Domain.Tests.Fixtures.AssessmentBuilder()
                .WithComment("   ");

            // Act & Assert
            Assert.Throws<InvalidEntityException>(() => builder.Build());
        }

        [Fact]
        public void Create_WithCommentExceedingMaxLength_ThrowsInvalidEntityException()
        {
            // Arrange
            var longComment = new string('x', 501); 
            var builder = new Domain.Tests.Fixtures.AssessmentBuilder()
                .WithComment(longComment);

            // Act & Assert
            Assert.Throws<InvalidEntityException>(() => builder.Build());
        }
    }
}
