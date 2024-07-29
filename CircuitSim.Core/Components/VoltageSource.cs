using CircuitSim.Core.Common;

namespace CircuitSim.Core.Components
{
    public class VoltageSource : Wire
    {
        public override void Flow()
        {
            if (Inputs.Count != 0)
                Inputs = new();
            if(Current == 0)
            {
                Current = Voltage / GetCircuitResistance();
            }

            DefaultFlow(Voltage, Current);
        }
    }
}
