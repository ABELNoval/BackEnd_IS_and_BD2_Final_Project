using System;
using Xunit;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Tests.Fixtures;
using Moq;

namespace Domain.Tests.Entities
{
    public class EquipmentTests
    {      
        [Fact]
        public void Create_WithValidParameters_CreatesEquipment()
        {
            // Arrange
            var builder = new EquipmentBuilder()
                .WithName("Equipo A")
                .WithAcquisitionDate(DateTime.UtcNow.AddYears(-2))
                .WithEquipmentTypeId(Guid.NewGuid())
                .WithDepartmentId(Guid.NewGuid())
                .WithStateId(Domain.Enumerations.EquipmentState.Operative.Id)
                .WithLocationTypeId(Domain.Enumerations.LocationType.Department.Id);

            // Act
            var equipment = builder.Build();

            // Assert
            Assert.Equal("Equipo A", equipment.Name);
            Assert.Equal(Domain.Enumerations.EquipmentState.Operative.Id, equipment.StateId);
            Assert.Equal(Domain.Enumerations.LocationType.Department.Id, equipment.LocationTypeId);
            Assert.NotEqual(Guid.Empty, equipment.EquipmentTypeId);
            Assert.NotNull(equipment.DepartmentId);
        }

        [Fact]
        public void Create_WithEmptyName_ThrowsInvalidEntityException()
        {
            // Arrange
            var builder = new EquipmentBuilder().WithName("");
            // Act & Assert
            Assert.Throws<InvalidEntityException>(() => builder.Build());
        }

        [Fact]
        public void Create_WithEmptyEquipmentTypeId_ThrowsInvalidEntityException()
        {
            // Arrange
            var builder = new EquipmentBuilder().WithEquipmentTypeId(Guid.Empty);
            // Act & Assert
            Assert.Throws<InvalidEntityException>(() => builder.Build());
        }

        [Fact]
        public void Create_WithFutureAcquisitionDate_ThrowsInvalidEntityException()
        {
            // Arrange
            var builder = new EquipmentBuilder().WithAcquisitionDate(DateTime.UtcNow.AddDays(1));
            // Act & Assert
            Assert.Throws<InvalidEntityException>(() => builder.Build());
        }

        [Fact]
        public void Create_WithNameExceedingMaxLength_ThrowsInvalidEntityException()
        {
            // Arrange
            var longName = new string('x', 201); // MaxNameLength = 200
            var builder = new EquipmentBuilder().WithName(longName);
            // Act & Assert
            Assert.Throws<InvalidEntityException>(() => builder.Build());
        }

        [Fact]
        public void Create_WithInvalidStateId_ThrowsInvalidEntityException()
        {
            // Arrange
            var builder = new EquipmentBuilder().WithStateId(-999);
            // Act & Assert
            Assert.Throws<InvalidEntityException>(() => builder.Build());
        }

        [Fact]
        public void Create_WithInvalidLocationTypeId_ThrowsInvalidEntityException()
        {
            // Arrange
            var builder = new EquipmentBuilder().WithLocationTypeId(-999);
            // Act & Assert
            Assert.Throws<InvalidEntityException>(() => builder.Build());
        }

        [Fact]
        public void Create_LocationTypeDepartmentWithoutDepartmentId_ThrowsInvalidEntityException()
        {
            // Arrange
            var builder = new EquipmentBuilder()
                .WithLocationTypeId(Domain.Enumerations.LocationType.Department.Id)
                .WithDepartmentId(null);
            // Act & Assert
            Assert.Throws<InvalidEntityException>(() => builder.Build());
        }

        [Fact]
        public void Create_LocationTypeDepartmentWithEmptyDepartmentId_ThrowsInvalidEntityException()
        {
            // Arrange
            var builder = new EquipmentBuilder()
                .WithLocationTypeId(Domain.Enumerations.LocationType.Department.Id)
                .WithDepartmentId(Guid.Empty);
            // Act & Assert
            Assert.Throws<InvalidEntityException>(() => builder.Build());
        }

        [Fact]
        public void Create_LocationTypeWarehouseWithDepartmentIdSet_ThrowsInvalidEntityException()
        {
            // Arrange
            var builder = new EquipmentBuilder()
                .WithLocationTypeId(Domain.Enumerations.LocationType.Warehouse.Id)
                .WithDepartmentId(Guid.NewGuid());
            // Act & Assert
            Assert.Throws<InvalidEntityException>(() => builder.Build());
        }

        [Fact]
        public void AddDecommission_WithStrategyMock_CallsApplyToAndMutatesAggregate()
        {
            // Arrange
            var equipment = new EquipmentBuilder()
                .WithName("Equipo Test")
                .WithAcquisitionDate(DateTime.UtcNow.AddYears(-1))
                .WithEquipmentTypeId(Guid.NewGuid())
                .WithDepartmentId(null)
                .WithStateId(Domain.Enumerations.EquipmentState.Operative.Id)
                .WithLocationTypeId(Domain.Enumerations.LocationType.Warehouse.Id)
                .Build();

            var responsibleId = Guid.NewGuid();
            var technicalId = Guid.NewGuid();
            var decommissionDate = DateTime.UtcNow;
            var reason = "Obsolescencia por antigüedad";
            var strategyMock = new Moq.Mock<Domain.Strategies.IDestinationStrategy>();
            strategyMock.Setup(s => s.DestinyType).Returns(Domain.Enumerations.DestinyType.Warehouse);
            strategyMock.Setup(s => s.Validate(It.IsAny<Domain.ValueObjects.DecommissionContext>()));
            strategyMock.Setup(s => s.ApplyTo(It.IsAny<Equipment>(), It.IsAny<Domain.ValueObjects.DecommissionContext>()));

            var context = Domain.ValueObjects.DecommissionContext.ForWarehouse(responsibleId, decommissionDate);

            // Act
            equipment.AddDecommission(strategyMock.Object, context, technicalId, reason);

            // Assert
            strategyMock.Verify(s => s.ApplyTo(equipment, context), Moq.Times.Once);
            Assert.Equal(Domain.Enumerations.EquipmentState.Decommissioned.Id, equipment.StateId);
            Assert.Single(equipment.Decommissions);
            var decommission = equipment.Decommissions.First();
            Assert.Equal(reason, decommission.Reason);
            Assert.Equal(technicalId, decommission.TechnicalId);
            Assert.Equal(responsibleId, decommission.RecipientId);
            Assert.Equal(decommissionDate, decommission.DecommissionDate);
            Assert.Equal(strategyMock.Object.DestinyType.Id, decommission.DestinyTypeId);
            equipment.ValidateEquipment();
        }
[Fact]
            public void AddMaintenance_ChangesStateToUnderMaintenanceAndAddsRecord()
            {
                // Arrange
                var equipment = new EquipmentBuilder()
                    .WithStateId(Domain.Enumerations.EquipmentState.Operative.Id)
                    .Build();
                var technicalId = Guid.NewGuid();
                var maintenanceDate = DateTime.UtcNow;
                var maintenanceTypeId = 1;
                var cost = 100m;

                // Act
                equipment.AddMaintenance(technicalId, maintenanceDate, maintenanceTypeId, cost);

                // Assert
                Assert.Equal(Domain.Enumerations.EquipmentState.UnderMaintenance.Id, equipment.StateId);
                Assert.Single(equipment.Maintenances);
                var maintenance = equipment.Maintenances.First();
                Assert.Equal(technicalId, maintenance.TechnicalId);
                Assert.Equal(maintenanceDate, maintenance.MaintenanceDate);
                Assert.Equal(maintenanceTypeId, maintenance.MaintenanceTypeId);
                Assert.Equal(cost, maintenance.Cost);
            }

            [Fact]
            public void AddMaintenance_WhenDisposed_ThrowsEquipmentDisposedException()
            {
                // Arrange
                var equipment = new EquipmentBuilder()
                    .WithStateId(Domain.Enumerations.EquipmentState.Disposed.Id)
                    .Build();
                var technicalId = Guid.NewGuid();
                var maintenanceDate = DateTime.UtcNow;
                var maintenanceTypeId = 1;
                var cost = 100m;

                // Act & Assert
                Assert.Throws<EquipmentDisposedException>(() =>
                    equipment.AddMaintenance(technicalId, maintenanceDate, maintenanceTypeId, cost));
            }

            [Fact]
            public void AddMaintenance_WhenDecommissioned_ThrowsBusinessRuleViolationException()
            {
                // Arrange
                var equipment = new EquipmentBuilder()
                    .WithStateId(Domain.Enumerations.EquipmentState.Decommissioned.Id)
                    .Build();
                var technicalId = Guid.NewGuid();
                var maintenanceDate = DateTime.UtcNow;
                var maintenanceTypeId = 1;
                var cost = 100m;

                // Act & Assert
                Assert.Throws<BusinessRuleViolationException>(() =>
                    equipment.AddMaintenance(technicalId, maintenanceDate, maintenanceTypeId, cost));
            }

            [Fact]
            public void CompleteMaintenance_ChangesStateToOperative()
            {
                // Arrange
                var equipment = new EquipmentBuilder()
                    .WithStateId(Domain.Enumerations.EquipmentState.UnderMaintenance.Id)
                    .Build();

                // Act
                equipment.CompleteMaintenance();

                // Assert
                Assert.Equal(Domain.Enumerations.EquipmentState.Operative.Id, equipment.StateId);
            }

            [Fact]
            public void CompleteMaintenance_WhenNotUnderMaintenance_ThrowsBusinessRuleViolationException()
            {
                // Arrange
                var equipment = new EquipmentBuilder()
                    .WithStateId(Domain.Enumerations.EquipmentState.Operative.Id)
                    .Build();

                // Act & Assert
                Assert.Throws<BusinessRuleViolationException>(() =>
                    equipment.CompleteMaintenance());
            }
            [Fact]
            public void AddTransfer_WhenDisposed_ThrowsEquipmentDisposedException()
            {
                // Arrange
                var initialDepartmentId = Guid.NewGuid();
                var targetDepartmentId = Guid.NewGuid();
                var equipment = new EquipmentBuilder()
                    .WithDepartmentId(initialDepartmentId)
                    .WithLocationTypeId(Domain.Enumerations.LocationType.Department.Id)
                    .WithStateId(Domain.Enumerations.EquipmentState.Disposed.Id)
                    .Build();

                var responsibleId = Guid.NewGuid();
                var transferDate = DateTime.UtcNow;

                // Act & Assert
                Assert.Throws<EquipmentDisposedException>(() =>
                    equipment.AddTransfer(targetDepartmentId, responsibleId, transferDate));
            }

            [Fact]
            public void AddTransfer_WhenUnderMaintenance_ThrowsBusinessRuleViolationException()
            {
                // Arrange
                var initialDepartmentId = Guid.NewGuid();
                var targetDepartmentId = Guid.NewGuid();
                var equipment = new EquipmentBuilder()
                    .WithDepartmentId(initialDepartmentId)
                    .WithLocationTypeId(Domain.Enumerations.LocationType.Department.Id)
                    .WithStateId(Domain.Enumerations.EquipmentState.UnderMaintenance.Id)
                    .Build();

                var responsibleId = Guid.NewGuid();
                var transferDate = DateTime.UtcNow;

                // Act & Assert
                Assert.Throws<BusinessRuleViolationException>(() =>
                    equipment.AddTransfer(targetDepartmentId, responsibleId, transferDate));
            }
            [Fact]
            public void AddTransfer_UpdatesDepartmentAndLocationType()
            {
                // Arrange
                var initialDepartmentId = Guid.NewGuid();
                var targetDepartmentId = Guid.NewGuid();
                var equipment = new EquipmentBuilder()
                    .WithDepartmentId(initialDepartmentId)
                    .WithLocationTypeId(Domain.Enumerations.LocationType.Department.Id)
                    .Build();

                var responsibleId = Guid.NewGuid();
                var transferDate = DateTime.UtcNow;

                // Act
                equipment.AddTransfer(targetDepartmentId, responsibleId, transferDate);

                // Assert
                Assert.Equal(targetDepartmentId, equipment.DepartmentId);
                Assert.Equal(Domain.Enumerations.LocationType.Department.Id, equipment.LocationTypeId);
                Assert.Single(equipment.Transfers);
                var transfer = equipment.Transfers.First();
                Assert.Equal(initialDepartmentId, transfer.SourceDepartmentId);
                Assert.Equal(targetDepartmentId, transfer.TargetDepartmentId);
                Assert.Equal(responsibleId, transfer.ResponsibleId);
                Assert.Equal(transferDate, transfer.TransferDate);
            }

            [Fact]
            public void AddTransfer_ToSameDepartment_ThrowsBusinessRuleViolationException()
            {
                // Arrange
                var departmentId = Guid.NewGuid();
                var equipment = new EquipmentBuilder()
                    .WithDepartmentId(departmentId)
                    .WithLocationTypeId(Domain.Enumerations.LocationType.Department.Id)
                    .Build();

                var responsibleId = Guid.NewGuid();
                var transferDate = DateTime.UtcNow;

                // Act & Assert
                Assert.Throws<BusinessRuleViolationException>(() =>
                    equipment.AddTransfer(departmentId, responsibleId, transferDate));
            }

            [Fact]
            public void AddTransfer_WhenNoDepartmentAssigned_ThrowsBusinessRuleViolationException()
            {
                // Arrange
                var equipment = new EquipmentBuilder()
                    .WithDepartmentId(null)
                    .WithLocationTypeId(Domain.Enumerations.LocationType.Warehouse.Id)
                    .Build();

                var targetDepartmentId = Guid.NewGuid();
                var responsibleId = Guid.NewGuid();
                var transferDate = DateTime.UtcNow;

                // Act & Assert
                Assert.Throws<BusinessRuleViolationException>(() =>
                    equipment.AddTransfer(targetDepartmentId, responsibleId, transferDate));
            }
            [Fact]
            public void AddDecommission_ToDepartment_UpdatesDepartmentAndLocationType()
            {
                // Arrange
                var initialDepartmentId = Guid.NewGuid();
                var newDepartmentId = Guid.NewGuid();
                var equipment = new EquipmentBuilder()
                    .WithDepartmentId(initialDepartmentId)
                    .WithLocationTypeId(Domain.Enumerations.LocationType.Department.Id)
                    .Build();

                var strategy = new Domain.Strategies.DepartmentDestinationStrategy(newDepartmentId);
                var responsibleId = Guid.NewGuid();
                var technicalId = Guid.NewGuid();
                var decommissionDate = DateTime.UtcNow;
                var reason = "Traslado por baja";

                // Act
                equipment.AddDecommission(strategy, responsibleId, technicalId, decommissionDate, reason);

                // Assert
                Assert.Equal(newDepartmentId, equipment.DepartmentId);
                Assert.Equal(Domain.Enumerations.LocationType.Department.Id, equipment.LocationTypeId);
                Assert.Equal(Domain.Enumerations.EquipmentState.Decommissioned.Id, equipment.StateId);
                Assert.Single(equipment.Decommissions);
                var decommission = equipment.Decommissions.First();
                Assert.Equal(newDepartmentId, decommission.DepartmentId); 
            }

            [Fact]
            public void AddDecommission_ToWarehouse_UpdatesLocationTypeAndClearsDepartment()
            {
                // Arrange
                var initialDepartmentId = Guid.NewGuid();
                var equipment = new EquipmentBuilder()
                    .WithDepartmentId(initialDepartmentId)
                    .WithLocationTypeId(Domain.Enumerations.LocationType.Department.Id)
                    .Build();

                var strategy = new Domain.Strategies.WarehouseDestinationStrategy();
                var responsibleId = Guid.NewGuid();
                var technicalId = Guid.NewGuid();
                var decommissionDate = DateTime.UtcNow;
                var reason = "Baja por traslado a almacén";

                // Act
                equipment.AddDecommission(strategy, responsibleId, technicalId, decommissionDate, reason);

                // Assert
                Assert.Null(equipment.DepartmentId);
                Assert.Equal(Domain.Enumerations.LocationType.Warehouse.Id, equipment.LocationTypeId);
                Assert.Equal(Domain.Enumerations.EquipmentState.Decommissioned.Id, equipment.StateId);
                Assert.Single(equipment.Decommissions);
                var decommission = equipment.Decommissions.First();
                Assert.Equal(Guid.Empty, decommission.DepartmentId); 
            }

            [Fact]
            public void AddDecommission_ToDisposal_UpdatesLocationTypeAndClearsDepartmentAndSetsDisposedState()
            {
                // Arrange
                var initialDepartmentId = Guid.NewGuid();
                var equipment = new EquipmentBuilder()
                    .WithDepartmentId(initialDepartmentId)
                    .WithLocationTypeId(Domain.Enumerations.LocationType.Department.Id)
                    .Build();

                var strategy = new Domain.Strategies.DisposalDestinationStrategy();
                var responsibleId = Guid.NewGuid();
                var technicalId = Guid.NewGuid();
                var decommissionDate = DateTime.UtcNow;
                var reason = "Baja definitiva por desecho";

                // Act
                equipment.AddDecommission(strategy, responsibleId, technicalId, decommissionDate, reason);

                // Assert
                Assert.Null(equipment.DepartmentId);
                Assert.Equal(Domain.Enumerations.LocationType.Disposal.Id, equipment.LocationTypeId);
                Assert.Equal(Domain.Enumerations.EquipmentState.Disposed.Id, equipment.StateId); 
                Assert.Single(equipment.Decommissions);
                var decommission = equipment.Decommissions.First();
                Assert.Equal(Guid.Empty, decommission.DepartmentId); 
            }
                [Fact]
                public void AddDecommission_WhenAlreadyDisposed_ThrowsEquipmentAlreadyDisposedException()
                {
                    // Arrange
                    var initialDepartmentId = Guid.NewGuid();
                    var equipment = new EquipmentBuilder()
                        .WithDepartmentId(initialDepartmentId)
                        .WithLocationTypeId(Domain.Enumerations.LocationType.Department.Id)
                        .WithStateId(Domain.Enumerations.EquipmentState.Disposed.Id) // Already disposed
                        .Build();

                    var strategy = new Domain.Strategies.DisposalDestinationStrategy();
                    var responsibleId = Guid.NewGuid();
                    var technicalId = Guid.NewGuid();
                    var decommissionDate = DateTime.UtcNow;
                    var reason = "Intento de baja sobre equipo ya desechado";

                    // Act & Assert
                    Assert.Throws<EquipmentAlreadyDisposedException>(() =>
                        equipment.AddDecommission(strategy, responsibleId, technicalId, decommissionDate, reason));
                }
        
    }
}
