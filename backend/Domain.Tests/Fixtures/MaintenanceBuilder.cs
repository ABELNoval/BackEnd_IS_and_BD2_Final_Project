using System;
using Domain.Entities;

namespace Domain.Tests.Fixtures
{
    public class MaintenanceBuilder
    {
        private Guid _equipmentId = Guid.NewGuid();
        private Guid _technicalId = Guid.NewGuid();
        private DateTime _maintenanceDate = DateTime.UtcNow.AddDays(-3);
        private int _maintenanceTypeId = 1;
        private decimal _cost = 100m;

        public MaintenanceBuilder WithEquipmentId(Guid id)
        {
            _equipmentId = id;
            return this;
        }
        public MaintenanceBuilder WithTechnicalId(Guid id)
        {
            _technicalId = id;
            return this;
        }
        public MaintenanceBuilder WithMaintenanceDate(DateTime date)
        {
            _maintenanceDate = date;
            return this;
        }
        public MaintenanceBuilder WithMaintenanceTypeId(int id)
        {
            _maintenanceTypeId = id;
            return this;
        }
        public MaintenanceBuilder WithCost(decimal cost)
        {
            _cost = cost;
            return this;
        }
        public Maintenance Build()
        {
            return Maintenance.Create(
                _equipmentId,
                _technicalId,
                _maintenanceDate,
                _maintenanceTypeId,
                _cost
            );
        }
    }
}
