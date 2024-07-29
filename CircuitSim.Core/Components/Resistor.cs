using CircuitSim.Core.Common;

namespace CircuitSim.Core.Components
{
    public class Resistor : Wire
    {
        public override void Flow()
        {
            DefaultFlow(-PreFlowCurrent * Resistance);
        }
    }
}
