using CircuitSim.Core.Common;
using CircuitSim.Core.Components.Subcomponents;
using System.Numerics;

namespace CircuitSim.Core.Components
{
    public class NPNTransistor : MultiConnectionWire
    {
        public NPNBase Base { get; set; }
        public NPNEmitter Emitter { get; set; }
        public NPNCollector Collector { get; set; }

        public NPNTransistor()
        {
            Base = new();
            Emitter = new();
            Collector = new();

            // Add proper connections to inputs and outputs
            Inputs.Add(Base);
            Inputs.Add(Collector);
            Outputs.Add(Emitter);

            Base.Outputs.Add(this);
            Emitter.Inputs.Add(this);
            Collector.Inputs.Add(this);
        }


        protected sealed override void UpdateValues()
        {

            var dir = Vector2.Normalize(End - Start);
            var perp = new Vector2(dir.Y, -dir.X);

            Base.Start = Start ;
            Base.End = End - dir * 40;

            Collector.Start = End - dir * 40;
            Collector.End = End + perp * 40;

            Emitter.Start = End - dir * 40;
            Emitter.End = End - perp * 40; 

        }

        internal override void AttachChildren(Circuit circuit)
        {
            circuit.AddWire(Base);
            circuit.AddWire(Emitter);
            circuit.AddWire(Collector);
            SetPositionsNoUpdate(Start, Start);
        }
    }
}
