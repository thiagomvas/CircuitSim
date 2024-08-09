namespace CircuitSim.Core.Common
{
    public abstract class MultiConnectionWire : Wire
    {
        internal abstract void AttachChildren(Circuit circuit);
        internal void RemoveSubWiresFromCircuit(Circuit circuit)
        {
            Console.WriteLine("Removing multiwire");

            // Create a list of wires to remove to avoid modifying the collection while iterating
            var wiresToRemove = new List<Wire>(Inputs.Concat(Outputs));

            foreach (var wire in wiresToRemove)
            {
                circuit.RemoveWireNoMult(wire);
            }

            // Remove the multiwire itself
            circuit.RemoveWireNoMult(this);
        }
    }
}