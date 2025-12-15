using System;
using Xunit;
using Domain.Entities;
using Domain.ValueObjects;
using Domain.Exceptions;

namespace Domain.Tests.Entities
{
    public class EmployeeTests
    {
        [Fact]
        public void Create_ValidEmployee_ReturnsEmployee()
        {
            // Arrange
            var name = "Ana";
            var email = Email.Create("ana@empresa.com");
            var passwordHash = PasswordHash.Create("hashed_password");

            // Act
            var departmentId = Guid.NewGuid();
            var employee = Employee.Create(name, email, passwordHash, departmentId);

            // Assert
            Assert.Equal(name, employee.Name);
            Assert.Equal(email, employee.Email);
            Assert.Equal(passwordHash, employee.PasswordHash);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Create_EmptyName_ThrowsInvalidEntityException(string name)
        {
            // Arrange
            var email = Email.Create("ana@empresa.com");
            var passwordHash = PasswordHash.Create("hashed_password");

            // Act & Assert
            var departmentId = Guid.NewGuid();
            Assert.Throws<InvalidEntityException>(() => Employee.Create(name, email, passwordHash, departmentId));
        }
    }
}
