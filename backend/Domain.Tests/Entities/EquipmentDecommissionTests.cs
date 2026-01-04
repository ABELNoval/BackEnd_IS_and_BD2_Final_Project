using System;
using Xunit;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Tests.Fixtures;

namespace Domain.Tests.Entities
{
    public class EquipmentDecommissionTests
    {
        [Fact]
        public void Create_WithValidParameters_CreatesDecommission()
        {
            // Arrange
            var builder = new EquipmentDecommissionBuilder()
                .WithEquipmentId(Guid.NewGuid())
                .WithTechnicalId(Guid.NewGuid())
                .WithDepartmentId(Guid.NewGuid())
                .WithDestinyTypeId(1)
                .WithRecipientId(Guid.NewGuid())
                .WithReason("Fin de vida útil")
                .WithDecommissionDate(DateTime.UtcNow.AddDays(-1));

            // Act
            var decommission = builder.Build();

            // Assert
            Assert.NotEqual(Guid.Empty, decommission.EquipmentId);
            Assert.NotEqual(Guid.Empty, decommission.TechnicalId);
            Assert.NotEqual(Guid.Empty, decommission.DepartmentId);
            Assert.NotEqual(Guid.Empty, decommission.RecipientId);
            Assert.Equal("Fin de vida útil", decommission.Reason);
            Assert.True(decommission.DecommissionDate <= DateTime.UtcNow);
        }

        [Fact]
        public void Create_WithEmptyEquipmentId_ThrowsInvalidEntityException()
        {
            var builder = new EquipmentDecommissionBuilder().WithEquipmentId(Guid.Empty);
            Assert.Throws<InvalidEntityException>(() => builder.Build());
        }

        [Fact]
        public void Create_WithEmptyTechnicalId_ThrowsInvalidEntityException()
        {
            var builder = new EquipmentDecommissionBuilder().WithTechnicalId(Guid.Empty);
            Assert.Throws<InvalidEntityException>(() => builder.Build());
        }

        [Theory]
        [InlineData(1, true)] // Department, must have non-empty DepartmentId
        [InlineData(2, false)] // Disposal, must have empty DepartmentId
        [InlineData(3, false)] // Warehouse, must have empty DepartmentId
        public void Create_ValidatesDepartmentIdAccordingToDestinyType(int destinyTypeId, bool shouldHaveDepartment)
        {
            var builder = new EquipmentDecommissionBuilder()
                .WithDestinyTypeId(destinyTypeId)
                .WithDepartmentId(shouldHaveDepartment ? Guid.NewGuid() : Guid.Empty);

            if (shouldHaveDepartment)
            {
                var decommission = builder.Build();
                Assert.NotEqual(Guid.Empty, decommission.DepartmentId);
            }
            else
            {
                var decommission = builder.Build();
                Assert.Equal(Guid.Empty, decommission.DepartmentId);
            }
        }

        [Theory]
        [InlineData(1)] // Department
        [InlineData(2)] // Disposal
        [InlineData(3)] // Warehouse
        public void Create_WithInvalidDepartmentIdAccordingToDestinyType_Throws(int destinyTypeId)
        {
            var builder = new EquipmentDecommissionBuilder()
                .WithDestinyTypeId(destinyTypeId)
                .WithDepartmentId(destinyTypeId == 1 ? Guid.Empty : Guid.NewGuid());

            Assert.Throws<InvalidEntityException>(() => builder.Build());
        }

        [Fact]
        public void Create_WithEmptyRecipientId_ThrowsInvalidEntityException()
        {
            var builder = new EquipmentDecommissionBuilder().WithRecipientId(Guid.Empty);
            Assert.Throws<InvalidEntityException>(() => builder.Build());
        }

        [Fact]
        public void Create_WithFutureDecommissionDate_ThrowsInvalidEntityException()
        {
            var builder = new EquipmentDecommissionBuilder().WithDecommissionDate(DateTime.UtcNow.AddDays(1));
            Assert.Throws<InvalidEntityException>(() => builder.Build());
        }

        [Fact]
        public void Create_WithEmptyReason_ThrowsInvalidEntityException()
        {
            var builder = new EquipmentDecommissionBuilder().WithReason("");
            Assert.Throws<InvalidEntityException>(() => builder.Build());
        }

        [Fact]
        public void Create_WithLongReason_ThrowsInvalidEntityException()
        {
            var longReason = new string('x', 501);
            var builder = new EquipmentDecommissionBuilder().WithReason(longReason);
            Assert.Throws<InvalidEntityException>(() => builder.Build());
        }
    }
}
