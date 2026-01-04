/// <summary>
/// Enumeration representing the state of equipment
/// </summary>
using Domain.Common;
namespace Domain.Enumerations
{
    public class EquipmentState : Enumeration
    {
        public static readonly EquipmentState Operative = new(1, "Operative");
        public static readonly EquipmentState UnderMaintenance = new(2, "UnderMaintenance");
        public static readonly EquipmentState Decommissioned = new(3, "Decommissioned");
        public static readonly EquipmentState Disposed = new(4, "Disposed");

        private EquipmentState(int id, string name) : base(id, name) { }

        public static EquipmentState FromId(int id)
        {
            return FromValue<EquipmentState>(id);
        }

        public static IEnumerable<EquipmentState> GetAll()
        {
            yield return Operative;
            yield return UnderMaintenance;
            yield return Decommissioned;
            yield return Disposed;
        }
    }    
}
