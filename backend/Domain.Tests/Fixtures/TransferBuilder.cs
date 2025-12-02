using System;
using Domain.Entities;

namespace Domain.Tests.Fixtures
{
    public class TransferBuilder
    {
        private Guid _equipmentId = Guid.NewGuid();
        private Guid _sourceDepartmentId = Guid.NewGuid();
        private Guid _targetDepartmentId = Guid.NewGuid();
        private Guid _responsibleId = Guid.NewGuid();
        private DateTime _transferDate = DateTime.UtcNow.AddDays(-5);

        public TransferBuilder WithEquipmentId(Guid id)
        {
            _equipmentId = id;
            return this;
        }
        public TransferBuilder WithSourceDepartmentId(Guid id)
        {
            _sourceDepartmentId = id;
            return this;
        }
        public TransferBuilder WithTargetDepartmentId(Guid id)
        {
            _targetDepartmentId = id;
            return this;
        }
        public TransferBuilder WithResponsibleId(Guid id)
        {
            _responsibleId = id;
            return this;
        }
        public TransferBuilder WithTransferDate(DateTime date)
        {
            _transferDate = date;
            return this;
        }
        public Transfer Build()
        {
            return Transfer.Create(
                _equipmentId,
                _sourceDepartmentId,
                _targetDepartmentId,
                _responsibleId,
                _transferDate
            );
        }
    }
}
