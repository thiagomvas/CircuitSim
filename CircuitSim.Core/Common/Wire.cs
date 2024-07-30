using System.Numerics;

namespace CircuitSim.Core.Common
{
    public class Wire
    {
        public double PreFlowVoltage = 0;
        public double PreFlowCurrent = 0;

        public double Voltage { get; set; } = 0;
        public double Current { get; set; } = 0;
        public double Resistance { get; set; } = 0;

        private Vector2 start, end;
        public Vector2 Start { get => start; set { start = value; UpdateValues(); } }
        public Vector2 End { get => end; set { end = value; UpdateValues(); } }
        public Vector2 Center { get; private set; }
        public Vector2 Direction { get; private set; }
        public float Length { get; private set; }
        public float AngleDeg { get; private set; }
        public List<Wire> Inputs { get; set; }
        public List<Wire> Outputs { get; set; }
        public Wire()
        {
            Inputs = new List<Wire>();
            Outputs = new List<Wire>();
        }

        public void AddVoltage(double v) => PreFlowVoltage += v;
        public void AddCurrent(double a) => PreFlowCurrent += a;
        public void SetVoltage(double v) => PreFlowVoltage = v;
        public void SetCurrent(double a) => PreFlowCurrent = a;
        private void UpdateValues()
        {
            Center = (End + Start) / 2;
            Length = (End - Start).Length();
            Direction = Vector2.Normalize(End - Start);

            var dir = (End - Start);
            AngleDeg = MathF.Atan2(dir.Y, dir.X) * 180 / MathF.PI;
        }

        public void ResetState()
        {

        }
        public virtual void Flow()
        {
            DefaultFlow();
        }

        public void DefaultFlow(double voltageAddition = 0, double currentAddition = 0)
        {
            Voltage = PreFlowVoltage;
            Current = PreFlowCurrent;
            PreFlowVoltage = 0;
            PreFlowCurrent = 0;
            Voltage = Math.Max(0, Voltage);
            Current = Math.Max(0, Current);

            if (Outputs.Count == 1)
            {
                var @out = Outputs[0];

                @out.AddVoltage(Voltage + voltageAddition);
                @out.AddCurrent(Current + currentAddition);

                @out.Flow();
            }
            else
            {
                var totalResistance = GetCircuitResistance();
                foreach (var output in Outputs)
                {
                    output.AddVoltage(Voltage + voltageAddition);

                    output.AddCurrent(Current * (totalResistance / output.GetCircuitResistance()) + currentAddition);

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
