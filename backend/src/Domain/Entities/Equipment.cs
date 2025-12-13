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
    /// Current state ID of the equipment
    /// </summary>
    public int StateId { get; private set; }

    /// <summary>
    /// Current location type ID of the equipment
    /// </summary>
    public int LocationTypeId { get; private set; }

    /// <summary>
    /// Calculated property to get the EquipmentState object
    /// </summary>
    public EquipmentState State => EquipmentState.FromId(StateId);

    /// <summary>
    /// Calculated property to get the LocationType object
    /// </summary>
    public LocationType LocationType => LocationType.FromId(LocationTypeId);

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
        StateId = EquipmentState.Operative.Id;
        LocationTypeId = LocationType.Warehouse.Id;
    }

    private Equipment(
        string name,
        DateTime acquisitionDate,
        Guid equipmentTypeId,
        Guid? departmentId,
        int stateId,
        int locationTypeId)
    {
        GenerateId();
        Name = name;
        AcquisitionDate = acquisitionDate;
        EquipmentTypeId = equipmentTypeId;
        DepartmentId = departmentId;
        StateId = stateId;
        LocationTypeId = locationTypeId;

        ValidateEquipment();
    }

    /// <summary>
    /// Creates a new equipment instance
    /// </summary>
    public static Equipment Create(
        string name,
        DateTime acquisitionDate,
        Guid equipmentTypeId,
        Guid? departmentId,
        int stateId,
        int locationTypeId)
    {
        return new Equipment(name, acquisitionDate, equipmentTypeId, departmentId, stateId, locationTypeId);
    }

    /// <summary>
    /// Adds a decommission record to the equipment.
    /// Uses Strategy Pattern to delegate destination-specific logic.
    /// Equipment doesn't need to know about destination types - it delegates to the strategy.
    /// </summary>
    public void AddDecommission(
        IDestinationStrategy destinationStrategy,
        Domain.ValueObjects.DecommissionContext context,
        Guid technicalId,
        string reason)
    {
        ValidateCanBeDecommissioned();
        if (destinationStrategy == null) throw new ArgumentNullException(nameof(destinationStrategy));
        if (context == null) throw new ArgumentNullException(nameof(context));

        // Validate strategy-specific requirements (fail-fast)
        destinationStrategy.Validate(context);

        var departmentForDecommission = context.TargetDepartmentId ?? Guid.Empty;

        var decommission = EquipmentDecommission.Create(
            equipmentId: Id,
            technicalId: technicalId,
            departmentId: departmentForDecommission,
            destinyTypeId: destinationStrategy.DestinyType.Id,
            recipientId: context.ResponsibleId,
            decommissionDate: context.TransferDate,
            reason: reason);

        // Apply the destination strategy (Command)
        destinationStrategy.ApplyTo(this, context);

        _decommissions.Add(decommission);
        if (StateId != EquipmentState.Disposed.Id)
            StateId = EquipmentState.Decommissioned.Id;
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
        LocationTypeId = LocationType.Department.Id;
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
        StateId = EquipmentState.UnderMaintenance.Id;
    }

    /// <summary>
    /// Marks maintenance as completed and returns equipment to operative state
    /// </summary>
    public void CompleteMaintenance()
    {
        if (StateId != EquipmentState.UnderMaintenance.Id)
            throw new BusinessRuleViolationException(
                "CompleteMaintenanceOnNonMaintainedEquipment",
                "Equipment must be under maintenance to complete maintenance");
        StateId = EquipmentState.Operative.Id;
    }

    #region Internal Methods (Called by Strategies)

    /// <summary>
    /// Moves equipment to disposal.
    /// Called by DisposalDestinationStrategy.
    /// </summary>
    internal void MoveToDisposal()
    {
        DepartmentId = null;
        LocationTypeId = LocationType.Disposal.Id;
        StateId = EquipmentState.Disposed.Id;
    }

    /// <summary>
    /// Moves equipment to warehouse.
    /// Called by WarehouseDestinationStrategy.
    /// </summary>
    internal void MoveToWarehouse()
    {
        DepartmentId = null;
        LocationTypeId = LocationType.Warehouse.Id;
    }

    /// <summary>
    /// Moves equipment to a department.
    /// Called by DepartmentDestinationStrategy.
    /// </summary>
    public void MoveToDepartment(Guid departmentId)
    {
        if (departmentId == Guid.Empty)
            throw new ArgumentException("Department ID cannot be empty", nameof(departmentId));

        DepartmentId = departmentId;
        LocationTypeId = LocationType.Department.Id;
    }

    #endregion

    #region Validation Methods

    private const int MaxNameLength = 200;

    public void ValidateEquipment()
    {
        if (Id == Guid.Empty)
            throw new InvalidEntityException(nameof(Equipment), "Equipment ID cannot be empty");

        if (string.IsNullOrWhiteSpace(Name))
            throw new InvalidEntityException(nameof(Equipment), "Name cannot be empty");

        if (Name.Length > MaxNameLength)
            throw new InvalidEntityException(nameof(Equipment), $"Name cannot exceed {MaxNameLength} characters");

        if (AcquisitionDate > DateTime.UtcNow)
            throw new InvalidEntityException(nameof(Equipment), "Acquisition date cannot be in the future");

        if (EquipmentTypeId == Guid.Empty)
            throw new InvalidEntityException(nameof(Equipment), "Equipment type ID cannot be empty");

        try
        {
            EquipmentState.FromId(StateId);
        }
        catch (InvalidOperationException)
        {
            throw new InvalidEntityException(nameof(Equipment), $"Invalid state ID: {StateId}");
        }
        try
        {
            LocationType.FromId(LocationTypeId);
        }
        catch (InvalidOperationException)
        {
            throw new InvalidEntityException(nameof(Equipment), $"Invalid location type ID: {LocationTypeId}");
        }

        if (LocationType == LocationType.Department)
        {
            if (!DepartmentId.HasValue || DepartmentId.Value == Guid.Empty)
                throw new InvalidEntityException(nameof(Equipment), "If LocationType is Department, DepartmentId must be set and not empty.");
        }
        else
        {
            if (DepartmentId.HasValue)
                throw new InvalidEntityException(nameof(Equipment), "If LocationType is Warehouse or Disposal, DepartmentId must be null.");
        }
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

    public void Update(string name, DateTime acquisitionDate, Guid equipmentTypeId, Guid? departmentId, int stateId, int locationTypeId)
    {
        Name = name;
        AcquisitionDate = acquisitionDate;
        EquipmentTypeId = equipmentTypeId;
        DepartmentId = departmentId;
        StateId = stateId;
        LocationTypeId = locationTypeId;

        ValidateEquipment();
    }

}



