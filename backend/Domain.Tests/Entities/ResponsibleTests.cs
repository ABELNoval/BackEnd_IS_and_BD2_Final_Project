using System;
using Domain.Entities;
using Domain.ValueObjects;
using Domain.Exceptions;
using Xunit;

namespace Domain.Tests.Entities
{
    public class ResponsibleTests
    {
        [Fact]
        public void Create_WithValidParameters_CreatesResponsible()
        {
            // Arrange
            var name = "Juan Pérez";
            var email = Email.Create("juan.perez@example.com");
            var passwordHash = PasswordHash.Create("hashedpassword123");

            // Act
            var responsible = Responsible.Create(name, email, passwordHash);

            // Assert
            Assert.Equal(name, responsible.Name);
            Assert.Equal(email, responsible.Email);
            Assert.Equal(passwordHash, responsible.PasswordHash);
            Assert.Equal(Role.Responsible.Id, responsible.RoleId);
        }

        [Fact]
        public void Create_WithEmptyName_ThrowsInvalidEntityException()
        {
            // Arrange
            var email = Email.Create("juan.perez@example.com");
            var passwordHash = PasswordHash.Create("hashedpassword123");

            // Act & Assert
            Assert.Throws<InvalidEntityException>(() =>
                Responsible.Create("   ", email, passwordHash));
        }

        [Fact]
        public void Create_WithTooLongName_ThrowsInvalidEntityException()
        {
            // Arrange
            var longName = new string('a', 101); // 101 caracteres
            var email = Email.Create("juan.perez@example.com");
            var passwordHash = PasswordHash.Create("hashedpassword123");

            // Act & Assert
            Assert.Throws<InvalidEntityException>(() =>
                Responsible.Create(longName, email, passwordHash));
        }
    }
}
