using System;
using Domain.Entities;

namespace Domain.Tests.Fixtures
{
	public class EquipmentDecommissionBuilder
	{
		private Guid _equipmentId = Guid.NewGuid();
		private Guid _technicalId = Guid.NewGuid();
		private Guid _departmentId = Guid.NewGuid();
		private int _destinyTypeId = 1;
		private Guid _recipientId = Guid.NewGuid();
		private string _reason = "Fin de vida util";
		private DateTime _decommissionDate = DateTime.UtcNow.AddDays(-10);

		public EquipmentDecommissionBuilder WithEquipmentId(Guid id)
		{
			_equipmentId = id;
			return this;
		}
		public EquipmentDecommissionBuilder WithTechnicalId(Guid id)
		{
			_technicalId = id;
			return this;
		}
		public EquipmentDecommissionBuilder WithDepartmentId(Guid id)
		{
			_departmentId = id;
			return this;
		}
		public EquipmentDecommissionBuilder WithDestinyTypeId(int id)
		{
			_destinyTypeId = id;
			return this;
		}
		public EquipmentDecommissionBuilder WithRecipientId(Guid id)
		{
			_recipientId = id;
			return this;
		}
		public EquipmentDecommissionBuilder WithReason(string reason)
		{
			_reason = reason;
			return this;
		}
		public EquipmentDecommissionBuilder WithDecommissionDate(DateTime date)
		{
			_decommissionDate = date;
			return this;
		}
		public EquipmentDecommission Build()
		{
			return EquipmentDecommission.Create(
				_equipmentId,
				_technicalId,
				_departmentId,
				_destinyTypeId,
				_recipientId,
				_decommissionDate,
				_reason);
		}
	}
}
