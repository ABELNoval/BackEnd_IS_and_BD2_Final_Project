using Domain.Entities;
using Domain.Enumerations;
using Domain.ValueObjects;

namespace Domain.Strategies;

/// <summary>
/// IDestinationStrategy - CQS applied
/// Query : exposes the DestinyType
/// Command : Validate (fail-fast) and ApplyTo (mutates the aggregate)
/// </summary>
public interface IDestinationStrategy
{
    /// <summary>
    /// The destiny type this strategy implements
    /// </summary>
    DestinyType DestinyType { get; }

    /// <summary>
    /// Query : validates destination-specific requirements (fail-fast)
    /// </summary>
    void Validate(DecommissionContext context);

    /// <summary>
    /// Command : applies the business logic to the equipment
    /// </summary>
    void ApplyTo(Equipment equipment, DecommissionContext context);
}
