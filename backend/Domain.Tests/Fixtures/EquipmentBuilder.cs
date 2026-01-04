using System;
using Domain.Entities;

namespace Domain.Tests.Fixtures
{
	public class EquipmentBuilder
	{
		private string _name = "Equipo de prueba";
		private DateTime _acquisitionDate = DateTime.UtcNow.AddYears(-1);
		private Guid _equipmentTypeId = Guid.NewGuid();
		private Guid? _departmentId = Guid.NewGuid();
		private int _stateId = Domain.Enumerations.EquipmentState.Operative.Id;
		private int _locationTypeId = Domain.Enumerations.LocationType.Department.Id;

		public EquipmentBuilder WithName(string name)
		{
			_name = name;
			return this;
		}
		public EquipmentBuilder WithAcquisitionDate(DateTime date)
		{
			_acquisitionDate = date;
			return this;
		}
		public EquipmentBuilder WithEquipmentTypeId(Guid id)
		{
			_equipmentTypeId = id;
			return this;
		}
		public EquipmentBuilder WithDepartmentId(Guid? id)
		{
			_departmentId = id;
			return this;
		}
		public EquipmentBuilder WithStateId(int stateId)
		{
			_stateId = stateId;
			return this;
		}
		public EquipmentBuilder WithLocationTypeId(int locationTypeId)
		{
			_locationTypeId = locationTypeId;
			return this;
		}
		public Equipment Build()
		{
			return Equipment.Create(_name, _acquisitionDate, _equipmentTypeId, _departmentId, _stateId, _locationTypeId);
		}
	}
}
