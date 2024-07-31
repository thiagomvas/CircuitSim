using CircuitSim.Core.Common;

namespace CircuitSim.Core.Components
{
    public class Ohmeter : Wire
    {
        public double CircuitResistance { get; set; } = 0;

        public override void Flow()
        {
            CircuitResistance = GetCircuitResistance();
            DefaultFlow();
        }
    }
}
