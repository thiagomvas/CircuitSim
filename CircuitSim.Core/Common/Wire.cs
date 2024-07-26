namespace CircuitSim.Core.Common
{
    public class Wire
    {
        public double Voltage { get; set; }
        public double Current { get; set; }
        public double Resistance { get; set; }

        public List<Wire> Inputs { get; set; }
        public List<Wire> Outputs { get; set; }
        public Wire()
        {
            Inputs = new List<Wire>();
            Outputs = new List<Wire>();
        }

        public virtual void Flow()
        {
            if (Outputs.Count == 1)
            {
                var @out = Outputs[0];

                @out.Voltage += Voltage;
                @out.Current += Current;

                @out.Flow();
            }
            else
            {
                foreach (var output in Outputs)
                {
                    output.Voltage += Voltage;

                    output.Flow();
                }
            }
        }

        public override string ToString()
        {
            return $"{Voltage}V - {Current}A - {Resistance}Ohm";
        }
    }
}
