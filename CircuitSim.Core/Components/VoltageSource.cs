using CircuitSim.Core.Common;

namespace CircuitSim.Core.Components
{
    public class VoltageSource : Wire
    {
        public override void Flow()
        {
            if (Inputs.Count != 0)
                Inputs = new();

            Current = Voltage / GetCircuitResistance();

            base.Flow();
        }
    }
}
