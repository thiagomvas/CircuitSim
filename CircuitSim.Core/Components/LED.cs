using CircuitSim.Core.Common;
using System.Drawing;

namespace CircuitSim.Core
{
    /// <summary>
    /// Represents a LED component in a circuit.
    /// </summary>
    public class LED : Wire
    {
        /// <summary>
        /// Gets or sets the voltage consumption of the LED.
        /// </summary>
        public double VoltageConsumption { get; set; } = 5;

        public Color LitColor { get; set; } = Color.White;

        /// <summary>
        /// Gets a value indicating whether the LED is on.
        /// </summary>
        public bool IsOn => Voltage > VoltageConsumption;

        /// <inheritdoc/>
        public override void Flow()
        {
            DefaultFlow(-VoltageConsumption);
        }
    }
}
