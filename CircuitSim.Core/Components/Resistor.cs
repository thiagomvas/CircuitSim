using CircuitSim.Core.Common;

namespace CircuitSim.Core.Components
{
    /// <summary>
    /// Represents a resistor component in a circuit.
    /// </summary>
    public class Resistor : Wire
    {
        /// <inheritdoc/>
        public override void Flow()
        {
            DefaultFlow(-PreFlowCurrent * Resistance);
        }
    }
}
