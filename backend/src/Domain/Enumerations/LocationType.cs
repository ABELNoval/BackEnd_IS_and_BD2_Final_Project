/// <summary>
/// Enumeration representing the location type of equipment
/// </summary>
using Domain.Common;
namespace Domain.Enumerations
{
    public class LocationType : Enumeration
    {
        public static readonly LocationType Department = new(1, "Department");
        public static readonly LocationType Warehouse = new(2, "Warehouse");
        public static readonly LocationType Disposal = new(3, "Disposal");

        private LocationType(int id, string name) : base(id, name) { }

        public static LocationType FromId(int id)
        {
            return FromValue<LocationType>(id);
        }

        public static IEnumerable<LocationType> GetAll()
        {
            yield return Department;
            yield return Warehouse;
            yield return Disposal;
        }
    }
}
