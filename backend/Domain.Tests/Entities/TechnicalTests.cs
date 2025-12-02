using System;
using Xunit;
using Domain.Entities;
using Domain.ValueObjects;
using Domain.Exceptions;

namespace Domain.Tests.Entities
{
    public class TechnicalTests
    {
        [Fact]
        public void Create_ValidTechnical_ReturnsTechnical()
        {
            // Arrange
            var builder = new Domain.Tests.Fixtures.TechnicalBuilder()
                .WithName("Luis")
                .WithEmail(Domain.ValueObjects.Email.Create("luis@empresa.com"))
                .WithPasswordHash(Domain.ValueObjects.PasswordHash.Create("hashed_password"))
                .WithExperience(5)
                .WithSpecialty("Redes");

            // Act
            var technical = builder.Build();

            // Assert
            Assert.Equal("Luis", technical.Name);
            Assert.Equal(Domain.ValueObjects.Email.Create("luis@empresa.com"), technical.Email);
            Assert.Equal(Domain.ValueObjects.PasswordHash.Create("hashed_password"), technical.PasswordHash);
            Assert.Equal(5, technical.Experience);
            Assert.Equal("Redes", technical.Specialty);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Create_EmptySpecialty_ThrowsInvalidEntityException(string specialty)
        {
            // Arrange
            var builder = new Domain.Tests.Fixtures.TechnicalBuilder()
                .WithSpecialty(specialty);

            // Act & Assert
            Assert.Throws<InvalidEntityException>(() => builder.Build());
        }

        [Fact]
        public void Create_NegativeExperience_ThrowsInvalidEntityException()
        {
            // Arrange
            var builder = new Domain.Tests.Fixtures.TechnicalBuilder()
                .WithExperience(-1);

            // Act & Assert
            Assert.Throws<InvalidEntityException>(() => builder.Build());
        }
        [Fact]
        public void AddAssessment_AddsAssessmentToCollection()
        {
            // Arrange
            var technical = new Domain.Tests.Fixtures.TechnicalBuilder()
                .WithName("Ana")
                .WithEmail(Email.Create("ana@empresa.com"))
                .WithPasswordHash(PasswordHash.Create("hash"))
                .WithExperience(3)
                .WithSpecialty("Sistemas")
                .Build();

            var directorId = Guid.NewGuid();
            var score = 85m;
            var comment = "Buen desempeño";

            // Act
            technical.AddAssessment(directorId, score, comment);

            // Assert
            Assert.Single(technical.Assessments);
            var assessment = technical.Assessments.First();
            Assert.Equal(technical.Id, assessment.TechnicalId);
            Assert.Equal(directorId, assessment.DirectorId);
            Assert.Equal(score, assessment.Score.Value);
            Assert.Equal(comment, assessment.Comment);
        }

        [Fact]
        public void AddAssessment_WithEmptyDirectorId_ThrowsInvalidEntityException()
        {
            // Arrange
            var technical = new Domain.Tests.Fixtures.TechnicalBuilder()
                .WithSpecialty("Sistemas")
                .Build();
            var score = 90m;
            var comment = "Excelente";

            // Act & Assert
            Assert.Throws<InvalidEntityException>(() =>
                technical.AddAssessment(Guid.Empty, score, comment));
        }

        [Fact]
        public void AddAssessment_WithInvalidScore_ThrowsDomainException()
        {
            // Arrange
            var technical = new Domain.Tests.Fixtures.TechnicalBuilder()
                .WithSpecialty("Sistemas")
                .Build();
            var directorId = Guid.NewGuid();
            var invalidScore = -10m; // Out of range
            var comment = "Mal puntaje";

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                technical.AddAssessment(directorId, invalidScore, comment));
        }

        [Fact]
        public void AddAssessment_WithEmptyComment_ThrowsInvalidEntityException()
        {
            // Arrange
            var technical = new Domain.Tests.Fixtures.TechnicalBuilder()
                .WithSpecialty("Sistemas")
                .Build();
            var directorId = Guid.NewGuid();
            var score = 75m;
            var emptyComment = "   ";

            // Act & Assert
            Assert.Throws<InvalidEntityException>(() =>
                technical.AddAssessment(directorId, score, emptyComment));
        }
    }
}
