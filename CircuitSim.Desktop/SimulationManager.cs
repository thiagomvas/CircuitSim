using CircuitSim.Core.Common;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace CircuitSim.Desktop
{
    internal class SimulationManager
    {
        private bool isDrawing = false;
        public List<Wire> Wires { get; set; } = new();

        public void Update()
        {
            foreach (var wire in Wires)
            {
                wire.Flow();
            }
        }

        public void Draw()
        {
            foreach (var wire in Wires)
            {
                DrawLineEx(wire.Start, wire.End, 2, Color.RayWhite);
            }
        }
    }
}
