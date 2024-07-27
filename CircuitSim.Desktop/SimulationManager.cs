using CircuitSim.Core.Common;
using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;

namespace CircuitSim.Desktop
{
    internal class SimulationManager
    {
        private bool isDrawing = false;
        private Vector2 wireStart;
        public List<Wire> Wires { get; set; } = new();
        public Wire? Hovered = null;

        public void Update()
        {
            if (IsMouseButtonPressed(MouseButton.Left))
            {
                isDrawing = true;
                wireStart = GetMousePosition();
            }
            if (IsMouseButtonReleased(MouseButton.Left))
            {
                isDrawing = false;
                var wireEnd = GetMousePosition();
                Wires.Add(new Wire { Start = wireStart, End = wireEnd });
            }

            if (isDrawing)
            {
                var mousePos = GetMousePosition();
                DrawLineEx(wireStart, mousePos, 2, Color.RayWhite);
            }
            else
            {
                Hovered = null;
                foreach (var wire in Wires)
                {
                    if (CheckCollisionPointLine(GetMousePosition(), wire.Start, wire.End, 4))
                    {
                        Hovered = wire;
                        break;
                    }

                }
            }
        }

        public void Draw()
        {
            foreach (var wire in Wires)
            {
                DrawLineEx(wire.Start, wire.End, 2, wire == Hovered ? Color.Green : Color.RayWhite);
            }
        }
    }
}
