using System.Numerics;

namespace CircuitSim.Core.Common
{
    public class Wire
    {
        private readonly double preFirstSimVoltage = 0;
        private readonly double preFirstSimCurrent = 0;
        public double Voltage { get; set; } = 0;
        public double Current { get; set; } = 0;
        public double Resistance { get; set; } = 0;

        public Vector2 Start { get; set; }
        public Vector2 End { get; set; }
        public List<Wire> Inputs { get; set; }
        public List<Wire> Outputs { get; set; }
        public Wire()
        {
            preFirstSimCurrent = 0;
            preFirstSimVoltage = 0;
            Inputs = new List<Wire>();
            Outputs = new List<Wire>();
        }
        public Wire(double voltage, double current)
        {
            preFirstSimCurrent = current;
            preFirstSimVoltage = voltage;
            Inputs = new List<Wire>();
            Outputs = new List<Wire>();
        }

        public void ResetState()
        {
            Voltage = preFirstSimVoltage;
            Current = preFirstSimCurrent;
        }
        public virtual void Flow()
        {
            ResetState();
            DefaultFlow(Voltage, Current);
        }

        public void DefaultFlow(double voltage, double current)
        {
            if (Outputs.Count == 1)
            {
                var @out = Outputs[0];

                @out.Voltage = voltage;
                @out.Current = current;

                @out.Flow();
            }
            else
            {
                var totalResistance = GetCircuitResistance();
                foreach (var output in Outputs)
                {
                    output.Voltage = voltage;

                    output.Current = current * (totalResistance / output.GetCircuitResistance());

                    output.Flow();
                }
            }
        }

        public double GetCircuitResistance()
        {
            double result = Resistance;

            if (Outputs.Count == 0)
            {
                return result;
            }

            if (Outputs.Count == 1)
            {
                result += Outputs[0].GetCircuitResistance();
            }
            else
            {
                double inverseTotalResistance = 0;
                foreach (var wire in Outputs)
                {
                    var res = wire.GetCircuitResistance();
                    if (res != 0)
                    {
                        inverseTotalResistance += 1 / res;
                    }
                }
                result += (inverseTotalResistance != 0) ? 1 / inverseTotalResistance : 0;
            }

            return result;
        }

        public override string ToString()
        {
            return $"{Voltage}V - {Current}A - {Resistance}Ohm";
        }
    }
}
