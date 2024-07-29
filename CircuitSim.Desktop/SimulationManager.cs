using CircuitSim.Core.Common;
using CircuitSim.Core.Components;
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
        public Type WireType { get; set; } = typeof(Wire);

        public void Update()
        {
            if(IsKeyPressed(KeyboardKey.One))
            {
                WireType = typeof(Wire);
            }
            if(IsKeyPressed(KeyboardKey.Two))
            {
                WireType = typeof(VoltageSource);
            }

            if (IsMouseButtonPressed(MouseButton.Left))
            {
                isDrawing = true;
                wireStart = GetMousePosition();
            }
            if (IsMouseButtonReleased(MouseButton.Left))
            {
                isDrawing = false;
                var wireEnd = GetMousePosition();
                CreateWire(wireEnd);
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
                        DrawText($"Voltage: {Hovered.Voltage}V", 10, 10, 20, Color.RayWhite);
                        DrawText($"Current: {Hovered.Current}A", 10, 40, 20, Color.RayWhite);
                        break;
                    }
                }
            }
            if (IsKeyPressed(KeyboardKey.F))
            {
                Wires[0].Flow();
            }
            if (IsKeyPressed(KeyboardKey.R))
            {
                Wires.Clear();
            }
        }

        public void Draw()
        {
            foreach (var wire in Wires)
            {
                DrawLineEx(wire.Start, wire.End, 2, wire == Hovered ? Color.Green : Color.RayWhite);
            }
        }

        public void CreateWire(Vector2 wireEnd)
        {
            if (WireType == typeof(Wire))
            {
                Wires.Add(new Resistor { Start = wireStart, End = wireEnd, Resistance = 1 });
                System.Console.WriteLine("Created Wire");
            }
            else
            {
                Wires.Add(new VoltageSource() { Voltage = 10, Start = wireStart, End = wireEnd });
                System.Console.WriteLine("Created VoltageSource");
            }
            if (Wires.Count >= 2)
            {
                Wires[^2].Outputs.Add(Wires[^1]);
                Wires[^1].Inputs.Add(Wires[^2]);
                System.Console.WriteLine("Connected Wires");
            }
        }
    }
}
