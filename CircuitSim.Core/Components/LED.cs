using CircuitSim.Core.Common;

namespace CircuitSim.Console
{
    public class LED : Wire
    {
        public double VoltageConsumption { get; set; }
        public bool IsOn => Voltage > VoltageConsumption;

        public override void Flow()
        {
            if (IsOn)
            {
                Voltage -= VoltageConsumption;
                base.Flow();
            }
        }
    }
}
