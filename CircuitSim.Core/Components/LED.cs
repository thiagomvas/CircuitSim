using CircuitSim.Core.Common;

namespace CircuitSim.Console
{
    /// <summary>
    /// Represents a LED component in a circuit.
    /// </summary>
    public class LED : Wire
    {
        /// <summary>
        /// Gets or sets the voltage consumption of the LED.
        /// </summary>
        public double VoltageConsumption { get; set; }

        /// <summary>
        /// Gets a value indicating whether the LED is on.
        /// </summary>
        public bool IsOn => Voltage > VoltageConsumption;

        /// <inheritdoc/>
        public override void Flow()
        {
            if (IsOn)
            {
                AddVoltage(-VoltageConsumption);
                base.Flow();
            }
        }
    }
}
