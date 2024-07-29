using CircuitSim.Core.Common;

namespace CircuitSim.Core.Components
{
    public class Resistor : Wire
    {
        public override void Flow()
        {
            ResetState();
            DefaultFlow(Voltage - Current * Resistance, Current);
        }
    }
}
