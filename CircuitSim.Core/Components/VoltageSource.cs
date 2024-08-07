using CircuitSim.Core.Annotations;
using CircuitSim.Core.Common;

namespace CircuitSim.Core.Components;

/// <summary>
/// Represents a voltage source component in a circuit.
/// </summary>
public class VoltageSource : Wire
{
    /// <summary>
    /// Gets or sets the supply voltage of the voltage source.
    /// </summary>
    [PropertyEditable]
    public double SupplyVoltage { get; set; }

    /// <inheritdoc/>
    public override void Flow()
    {
        if (Inputs.Count != 0)
            Inputs = new();

        SetVoltage(SupplyVoltage);
        SetCurrent(SupplyVoltage / GetCircuitResistance());

        DefaultFlow();
    }
}
