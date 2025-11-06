using Domain.Common;
using Domain.Enumerations;
using Domain.Exceptions;
using Domain.Strategies;

namespace Domain.Entities;

/// <summary>
/// Represents a piece of equipment in the system.
/// This is an aggregate root that manages equipment lifecycle, location, and associated operations.
/// </summary>
public class Equipment : Entity
{
    private readonly List<EquipmentDecommission> _decommissions = new();
    private readonly List<Transfer> _transfers = new();
    private readonly List<Maintenance> _maintenances = new();

    /// <summary>
    /// Equipment name/identifier
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Current state of the equipment
    /// </summary>
    public EquipmentState State { get; private set; }

    /// <summary>
    /// Current location type of the equipment
    /// </summary>
    public LocationType LocationType { get; private set; }

    /// <summary>
    /// Date when the equipment was acquired
    /// </summary>
    public DateTime AcquisitionDate { get; private set; }

    /// <summary>
    /// ID of the equipment type
    /// </summary>
    public Guid EquipmentTypeId { get; private set; }

    /// <summary>
    /// ID of the current department (null if in warehouse or disposed)
    /// </summary>
    public Guid? DepartmentId { get; private set; }

    /// <summary>
    /// Collection of decommission records
    /// </summary>
    public IReadOnlyCollection<EquipmentDecommission> Decommissions => _decommissions.AsReadOnly();

    /// <summary>
    /// Collection of transfer records
    /// </summary>
    public IReadOnlyCollection<Transfer> Transfers => _transfers.AsReadOnly();

    /// <summary>
    /// Collection of maintenance records
    /// </summary>
    public IReadOnlyCollection<Maintenance> Maintenances => _maintenances.AsReadOnly();

    // EF Core constructor
    private Equipment() 
    {
        Name = string.Empty;
        State = EquipmentState.Operative;
        LocationType = LocationType.Warehouse;
    }

    private Equipment(
        string name,
        DateTime acquisitionDate,
        Guid equipmentTypeId,
        Guid? departmentId)
    {
        GenerateId();
        Name = name;
        AcquisitionDate = acquisitionDate;
        EquipmentTypeId = equipmentTypeId;
        DepartmentId = departmentId;
        State = EquipmentState.Operative;
        LocationType = departmentId.HasValue ? LocationType.Department : LocationType.Warehouse;

        ValidateEquipment();
    }

    /// <summary>
    /// Creates a new equipment instance
    /// </summary>
    public static Equipment Create(
        string name,
        DateTime acquisitionDate,
        Guid equipmentTypeId,
        Guid? departmentId = null)
    {
        return new Equipment(name, acquisitionDate, equipmentTypeId, departmentId);
    }

    /// <summary>
    /// Adds a decommission record to the equipment.
    /// Uses Strategy Pattern to delegate destination-specific logic.
    /// Equipment doesn't need to know about destination types - it delegates to the strategy.
    /// </summary>
    public void AddDecommission(
        IDestinationStrategy destinationStrategy,
        Guid responsibleId,
        Guid technicalId,
        DateTime decommissionDate,
        string reason)
    {
        ValidateCanBeDecommissioned();

        // Create the decommission record BEFORE applying the strategy
        var decommission = EquipmentDecommission.Create(
            equipmentId: Id,
            technicalId: technicalId,
            departmentId: DepartmentId ?? Guid.Empty,
            destinyTypeId: destinationStrategy.DestinyTypeId,
            recipientId: responsibleId,
            decommissionDate: decommissionDate,
            reason: reason);

        // Apply the destination strategy (Tell, Don't Ask)
        // The strategy knows what to do with the equipment
        destinationStrategy.ApplyTo(this);

        // Add to collection after successful application
        _decommissions.Add(decommission);
        State = EquipmentState.Decommissioned;
    }

    /// <summary>
    /// Adds a transfer record and updates the equipment's department.
    /// Only references by ID - no aggregate crossing.
    /// </summary>
    public void AddTransfer(
        Guid targetDepartmentId,
        Guid responsibleId,
        DateTime transferDate)
    {
        ValidateCanBeTransferred(targetDepartmentId);

        var sourceDepartmentId = DepartmentId!.Value;

        var transfer = Transfer.Create(
            equipmentId: Id,
            sourceDepartmentId: sourceDepartmentId,
            targetDepartmentId: targetDepartmentId,
            responsibleId: responsibleId,
            transferDate: transferDate);

        _transfers.Add(transfer);
        DepartmentId = targetDepartmentId;
        LocationType = LocationType.Department;
    }

    /// <summary>
    /// Adds a maintenance record to the equipment.
    /// Equipment can only be maintained if not disposed.
    /// </summary>
    public void AddMaintenance(
        Guid technicalId,
        DateTime maintenanceDate,
        int maintenanceTypeId,
        decimal cost)
    {
        ValidateCanBeMaintained();

        var maintenance = Maintenance.Create(
            equipmentId: Id,
            technicalId: technicalId,
            maintenanceDate: maintenanceDate,
            maintenanceTypeId: maintenanceTypeId,
            cost: cost);

        _maintenances.Add(maintenance);
        State = EquipmentState.UnderMaintenance;
    }

    /// <summary>
    /// Marks maintenance as completed and returns equipment to operative state
    /// </summary>
    public void CompleteMaintenance()
    {
        if (State != EquipmentState.UnderMaintenance)
            throw new BusinessRuleViolationException(
                "CompleteMaintenanceOnNonMaintainedEquipment",
                "Equipment must be under maintenance to complete maintenance");

        // Equipment returns to operative state after maintenance
        // Note: Disposed/Decommissioned equipment cannot reach this point
        // because AddMaintenance() validates and prevents maintenance on such equipment
        State = EquipmentState.Operative;
    }

    #region Internal Methods (Called by Strategies)

    /// <summary>
    /// Moves equipment to disposal.
    /// Called by DisposalDestinationStrategy.
    /// </summary>
    internal void MoveToDisposal()
    {
        DepartmentId = null;
        LocationType = LocationType.Disposal;
        State = EquipmentState.Disposed;
    }

    /// <summary>
    /// Moves equipment to warehouse.
    /// Called by WarehouseDestinationStrategy.
    /// </summary>
    internal void MoveToWarehouse()
    {
        DepartmentId = null;
        LocationType = LocationType.Warehouse;
    }

    /// <summary>
    /// Moves equipment to a department.
    /// Called by DepartmentDestinationStrategy.
    /// </summary>
    internal void MoveToDepartment(Guid departmentId)
    {
        if (departmentId == Guid.Empty)
            throw new ArgumentException("Department ID cannot be empty", nameof(departmentId));

        DepartmentId = departmentId;
        LocationType = LocationType.Department;
    }

    #endregion

    #region Validation Methods

    private const int MaxNameLength = 200;

    private void ValidateEquipment()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new InvalidEntityException(
                nameof(Equipment),
                "Name cannot be empty");

        if (Name.Length > MaxNameLength)
            throw new InvalidEntityException(
                nameof(Equipment),
                $"Name cannot exceed {MaxNameLength} characters");

        if (AcquisitionDate > DateTime.UtcNow)
            throw new InvalidEntityException(
                nameof(Equipment),
                "Acquisition date cannot be in the future");

        if (EquipmentTypeId == Guid.Empty)
            throw new InvalidEntityException(
                nameof(Equipment),
                "Equipment type ID cannot be empty");
    }

    private void ValidateCanBeDecommissioned()
    {
        if (State == EquipmentState.Disposed)
            throw new EquipmentAlreadyDisposedException(Id);

        if (State == EquipmentState.Decommissioned)
            throw new EquipmentAlreadyDecommissionedException(Id);

        if (State == EquipmentState.UnderMaintenance)
            throw new BusinessRuleViolationException(
                "DecommissionEquipmentUnderMaintenance",
                "Cannot decommission equipment that is under maintenance");
    }

    private void ValidateCanBeTransferred(Guid targetDepartmentId)
    {
        if (State == EquipmentState.Disposed)
            throw new EquipmentDisposedException(Id, "Cannot transfer disposed equipment");

        if (State == EquipmentState.UnderMaintenance)
            throw new BusinessRuleViolationException(
                "TransferEquipmentUnderMaintenance",
                "Cannot transfer equipment that is under maintenance");

        if (!DepartmentId.HasValue)
            throw new BusinessRuleViolationException(
                "TransferEquipmentWithoutDepartment",
                "Equipment must be assigned to a department to be transferred");

        if (DepartmentId.Value == targetDepartmentId)
            throw new BusinessRuleViolationException(
                "TransferToSameDepartment",
                "Cannot transfer equipment to the same department");

        if (targetDepartmentId == Guid.Empty)
            throw new InvalidEntityException(
                nameof(Equipment),
                "Target department ID cannot be empty");
    }

    private void ValidateCanBeMaintained()
    {
        if (State == EquipmentState.Disposed)
            throw new EquipmentDisposedException(Id, "Cannot maintain disposed equipment");

        if (State == EquipmentState.Decommissioned)
            throw new BusinessRuleViolationException(
                "MaintainDecommissionedEquipment",
                "Cannot maintain decommissioned equipment");
    }

    #endregion
}

/// <summary>
/// Enumeration representing the state of equipment
/// </summary>
public class EquipmentState : Enumeration
{
    public static readonly EquipmentState Operative = new(1, "Operative");
    public static readonly EquipmentState UnderMaintenance = new(2, "UnderMaintenance");
    public static readonly EquipmentState Decommissioned = new(3, "Decommissioned");
    public static readonly EquipmentState Disposed = new(4, "Disposed");

    private EquipmentState(int id, string name) : base(id, name) { }

    public static IEnumerable<EquipmentState> GetAll()
    {
        yield return Operative;
        yield return UnderMaintenance;
        yield return Decommissioned;
        yield return Disposed;
    }
}

/// <summary>
/// Enumeration representing the location type of equipment
/// </summary>
public class LocationType : Enumeration
{
    public static readonly LocationType Department = new(1, "Department");
    public static readonly LocationType Warehouse = new(2, "Warehouse");
    public static readonly LocationType Disposal = new(3, "Disposal");

    private LocationType(int id, string name) : base(id, name) { }

    public static IEnumerable<LocationType> GetAll()
    {
        yield return Department;
        yield return Warehouse;
        yield return Disposal;
    }
}