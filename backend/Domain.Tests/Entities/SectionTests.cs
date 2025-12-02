using System;
using Xunit;
using Domain.Entities;
using Domain.Exceptions;

namespace Domain.Tests.Entities
{
    public class SectionTests
    {
        [Fact]
        public void Create_ValidSection_ReturnsSection()
        {
            // Arrange
            var name = "Recursos Humanos";

            // Act
            var section = Section.Create(name);

            // Assert
            Assert.Equal(name, section.Name);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Create_EmptyName_ThrowsInvalidEntityException(string name)
        {
            // Act & Assert
            Assert.Throws<InvalidEntityException>(() => Section.Create(name));
        }

        [Fact]
        public void Create_NameTooLong_ThrowsInvalidEntityException()
        {
            // Arrange
            var name = new string('a', 101); // 101 chars

            // Act & Assert
            Assert.Throws<InvalidEntityException>(() => Section.Create(name));
        }
    }
}
