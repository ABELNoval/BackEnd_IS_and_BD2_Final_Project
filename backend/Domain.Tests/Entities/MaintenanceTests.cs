using System;
using Xunit;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Tests.Fixtures;

namespace Domain.Tests.Entities
{
    public class MaintenanceTests
    {
        [Fact]
        public void Create_WithValidParameters_CreatesMaintenance()
        {
            // Arrange
            var builder = new MaintenanceBuilder()
                .WithEquipmentId(Guid.NewGuid())
                .WithTechnicalId(Guid.NewGuid())
                .WithMaintenanceDate(DateTime.UtcNow.AddDays(-2))
                .WithMaintenanceTypeId(1)
                .WithCost(150m);

            // Act
            var maintenance = builder.Build();

            // Assert
            Assert.NotEqual(Guid.Empty, maintenance.EquipmentId);
            Assert.NotEqual(Guid.Empty, maintenance.TechnicalId);
            Assert.True(maintenance.MaintenanceDate <= DateTime.UtcNow);
            Assert.Equal(1, maintenance.MaintenanceTypeId);
            Assert.Equal(150m, maintenance.Cost);
        }

        [Fact]
        public void Create_WithEmptyEquipmentId_ThrowsInvalidEntityException()
        {
            var builder = new MaintenanceBuilder().WithEquipmentId(Guid.Empty);
            Assert.Throws<InvalidEntityException>(() => builder.Build());
        }

        [Fact]
        public void Create_WithEmptyTechnicalId_ThrowsInvalidEntityException()
        {
            var builder = new MaintenanceBuilder().WithTechnicalId(Guid.Empty);
            Assert.Throws<InvalidEntityException>(() => builder.Build());
        }

        [Fact]
        public void Create_WithFutureMaintenanceDate_ThrowsInvalidEntityException()
        {
            var builder = new MaintenanceBuilder().WithMaintenanceDate(DateTime.UtcNow.AddDays(1));
            Assert.Throws<InvalidEntityException>(() => builder.Build());
        }

        [Fact]
        public void Create_WithInvalidMaintenanceTypeId_ThrowsInvalidEntityException()
        {
            var builder = new MaintenanceBuilder().WithMaintenanceTypeId(-1);
            Assert.Throws<InvalidEntityException>(() => builder.Build());
        }

        [Fact]
        public void Create_WithNegativeCost_ThrowsInvalidEntityException()
        {
            var builder = new MaintenanceBuilder().WithCost(-10m);
            Assert.Throws<InvalidEntityException>(() => builder.Build());
        }
    }
}
