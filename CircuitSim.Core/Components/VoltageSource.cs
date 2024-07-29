using CircuitSim.Core.Common;

namespace CircuitSim.Core.Components
{
    public class VoltageSource : Wire
    {
        public double SupplyVoltage { get; set; }
        public override void Flow()
        {
            if (Inputs.Count != 0)
                Inputs = new();

            SetVoltage(SupplyVoltage);
            SetCurrent(SupplyVoltage / GetCircuitResistance());


            DefaultFlow();
        }
    }
}
