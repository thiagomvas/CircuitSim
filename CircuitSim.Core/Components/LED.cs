using CircuitSim.Core.Annotations;
using CircuitSim.Core.Common;
using System.Drawing;

namespace CircuitSim.Core;

/// <summary>
/// Represents a LED component in a circuit.
/// </summary>
public class LED : Wire
{
    /// <summary>
    /// Gets or sets the voltage consumption of the LED.
    /// </summary>
    public double VoltageConsumption { get; set; } = 5;

    private byte r = 0xff;
    private byte g = 0xff;
    private byte b = 0xff;

    public Color LitColor { get; set; } = Color.White;

    /// <summary>
    /// Gets a value indicating whether the LED is on.
    /// </summary>
    public bool IsOn => Voltage > VoltageConsumption;

    [PropertyEditable]
    public byte R { get => r; set { r = value; LitColor = Color.FromArgb(r, g, b); } }
    [PropertyEditable]
    public byte G { get => g; set { g = value; LitColor = Color.FromArgb(r, g, b); } }
    [PropertyEditable]
    public byte B { get => b; set { b = value; LitColor = Color.FromArgb(r, g, b); } }

    /// <inheritdoc/>
    public override void Flow()
    {
        DefaultFlow(-VoltageConsumption);
    }
}
