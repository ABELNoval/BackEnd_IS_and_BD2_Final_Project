using System;
using Xunit;
using Domain.Entities;
using Domain.Exceptions;

namespace Domain.Tests.Entities
{
    public class EquipmentTypeTests
    {
        [Fact]
        public void Create_ValidEquipmentType_ReturnsEquipmentType()
        {
            // Arrange
            var name = "Proyector";

            // Act
            var equipmentType = EquipmentType.Create(name);

            // Assert
            Assert.Equal(name, equipmentType.Name);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Create_EmptyName_ThrowsInvalidEntityException(string name)
        {
            // Act & Assert
            Assert.Throws<InvalidEntityException>(() => EquipmentType.Create(name));
        }

        [Fact]
        public void Create_NameTooLong_ThrowsInvalidEntityException()
        {
            // Arrange
            var name = new string('a', 101); // 101 chars

            // Act & Assert
            Assert.Throws<InvalidEntityException>(() => EquipmentType.Create(name));
        }
    }
}
