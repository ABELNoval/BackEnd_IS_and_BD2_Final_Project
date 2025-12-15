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


            // Act
            var dept = Department.Create(name, sectionId);

            // Assert
            Assert.Equal(name, dept.Name);
            Assert.Equal(sectionId, dept.SectionId);
            
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Create_EmptyName_ThrowsInvalidEntityException(string name)
        {
            // Arrange

            var sectionId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<InvalidEntityException>(() => Department.Create(name, sectionId));
        }

        [Fact]
        public void Create_NameTooLong_ThrowsInvalidEntityException()
        {
            // Arrange

            var name = new string('a', 101); // 101 chars
            var sectionId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<InvalidEntityException>(() => Department.Create(name, sectionId));
        }

        [Fact]
        public void Create_EmptySectionId_ThrowsInvalidEntityException()
        {
            // Arrange

            var name = "Informática";
            var sectionId = Guid.Empty;

            // Act & Assert
            Assert.Throws<InvalidEntityException>(() => Department.Create(name, sectionId));
        }

       
        }
}
