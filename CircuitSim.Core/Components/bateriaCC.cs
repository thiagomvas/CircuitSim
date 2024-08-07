using CircuitSim.Core.Common;

namespace CircuitSim.Core.Components;

/// <summary>
/// Represents a direct current (DC) battery component.
/// </summary>
public class BateriaCC : Wire
{
    /// <summary>
    /// Gets or sets the voltage output of the battery.
    /// </summary>
    public double VoltageOutput { get; set; }

    /// <summary>
    /// Gets or sets the capacity of the battery.
    /// </summary>
    public double Capacity { get; set; }

    /// <summary>
    /// Gets a value indicating whether the battery is active.
    /// </summary>
    public bool IsActive => Capacity > 0;

    /// <summary>
    /// Performs the flow of current through the battery.
    /// </summary>
    public override void Flow()
    {
        if (IsActive)
        {
            Capacity -= VoltageOutput * 0.1;
            if (Capacity < 0) Capacity = 0;

            SetVoltage(VoltageOutput);

            base.Flow();
        }
    }
}

