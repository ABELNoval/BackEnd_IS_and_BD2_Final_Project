using System;
using Xunit;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Tests.Fixtures;

namespace Domain.Tests.Entities
{
    public class TransferTests
    {
        [Fact]
        public void Create_WithValidParameters_CreatesTransfer()
        {
            // Arrange
            var builder = new TransferBuilder()
                .WithEquipmentId(Guid.NewGuid())
                .WithSourceDepartmentId(Guid.NewGuid())
                .WithTargetDepartmentId(Guid.NewGuid())
                .WithResponsibleId(Guid.NewGuid())
                .WithTransferDate(DateTime.UtcNow.AddDays(-1));

            // Act
            var transfer = builder.Build();

            // Assert
            Assert.NotEqual(Guid.Empty, transfer.EquipmentId);
            Assert.NotEqual(Guid.Empty, transfer.SourceDepartmentId);
            Assert.NotEqual(Guid.Empty, transfer.TargetDepartmentId);
            Assert.NotEqual(Guid.Empty, transfer.ResponsibleId);
            Assert.True(transfer.TransferDate <= DateTime.UtcNow);
        }

        [Fact]
        public void Create_WithEmptyEquipmentId_ThrowsInvalidEntityException()
        {
            var builder = new TransferBuilder().WithEquipmentId(Guid.Empty);
            Assert.Throws<InvalidEntityException>(() => builder.Build());
        }

        [Fact]
        public void Create_WithEmptySourceDepartmentId_ThrowsInvalidEntityException()
        {
            var builder = new TransferBuilder().WithSourceDepartmentId(Guid.Empty);
            Assert.Throws<InvalidEntityException>(() => builder.Build());
        }

        [Fact]
        public void Create_WithEmptyTargetDepartmentId_ThrowsInvalidEntityException()
        {
            var builder = new TransferBuilder().WithTargetDepartmentId(Guid.Empty);
            Assert.Throws<InvalidEntityException>(() => builder.Build());
        }

        [Fact]
        public void Create_WithEmptyResponsibleId_ThrowsInvalidEntityException()
        {
            var builder = new TransferBuilder().WithResponsibleId(Guid.Empty);
            Assert.Throws<InvalidEntityException>(() => builder.Build());
        }

        [Fact]
        public void Create_WithFutureTransferDate_ThrowsBusinessRuleViolationException()
        {
            var builder = new TransferBuilder().WithTransferDate(DateTime.UtcNow.AddDays(2));
            Assert.Throws<BusinessRuleViolationException>(() => builder.Build());
        }
    }
}
