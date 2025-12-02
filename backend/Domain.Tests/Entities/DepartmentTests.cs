using System;
using Xunit;
using Domain.Entities;
using Domain.Exceptions;

namespace Domain.Tests.Entities
{
    public class DepartmentTests
    {
        [Fact]
        public void Create_ValidDepartment_ReturnsDepartment()
        {
            // Arrange
            var name = "Informática";
            var sectionId = Guid.NewGuid();
            var responsibleId = Guid.NewGuid();

            // Act
            var dept = Department.Create(name, sectionId, responsibleId);

            // Assert
            Assert.Equal(name, dept.Name);
            Assert.Equal(sectionId, dept.SectionId);
            Assert.Equal(responsibleId, dept.ResponsibleId);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Create_EmptyName_ThrowsInvalidEntityException(string name)
        {
            // Arrange
            var sectionId = Guid.NewGuid();
            var responsibleId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<InvalidEntityException>(() => Department.Create(name, sectionId, responsibleId));
        }

        [Fact]
        public void Create_NameTooLong_ThrowsInvalidEntityException()
        {
            // Arrange
            var name = new string('a', 101); // 101 chars
            var sectionId = Guid.NewGuid();
            var responsibleId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<InvalidEntityException>(() => Department.Create(name, sectionId, responsibleId));
        }

        [Fact]
        public void Create_EmptySectionId_ThrowsInvalidEntityException()
        {
            // Arrange
            var name = "Informática";
            var sectionId = Guid.Empty;
            var responsibleId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<InvalidEntityException>(() => Department.Create(name, sectionId, responsibleId));
        }

        [Fact]
        public void Create_EmptyResponsibleId_ThrowsInvalidEntityException()
        {
            // Arrange
            var name = "Informática";
            var sectionId = Guid.NewGuid();
            var responsibleId = Guid.Empty;

            // Act & Assert
            Assert.Throws<InvalidEntityException>(() => Department.Create(name, sectionId, responsibleId));
        }
    }
}
