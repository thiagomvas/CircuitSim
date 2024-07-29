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
        private const float GridSize = 20.0f; // Define the grid size
        public List<Wire> Wires { get; set; } = new();
        public Wire? Hovered = null;
        public Type WireType { get; set; } = typeof(Wire);

        public void Update()
        {
            if (IsKeyPressed(KeyboardKey.One))
            {
                WireType = typeof(Wire);
            }
            if (IsKeyPressed(KeyboardKey.Two))
            {
                WireType = typeof(Resistor);
            }
            if(IsKeyPressed(KeyboardKey.Three))
            {
                WireType = typeof(VoltageSource);
            }

            if (IsMouseButtonPressed(MouseButton.Left))
            {
                isDrawing = true;
                wireStart = SnapToGrid(GetMousePosition());
            }
            if (IsMouseButtonReleased(MouseButton.Left))
            {
                isDrawing = false;
                var wireEnd = SnapToGrid(GetMousePosition());
                CreateWire(wireEnd);
            }

            if (isDrawing)
            {
                var mousePos = SnapToGrid(GetMousePosition());
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
            if (IsKeyPressed(KeyboardKey.F))
            {
                foreach (var wire in Wires)
                    if (wire.GetType() == typeof(VoltageSource))
                        wire.Flow();
            }
            if (IsKeyPressed(KeyboardKey.R))
            {
                Wires.Clear();
            }
        }

        public void Draw()
        {
            if(Hovered != null)
            {
                DrawText($"Voltage: {Hovered.Voltage}V", 10, 10, 20, Color.RayWhite);
                DrawText($"Current: {Hovered.Current}A", 10, 40, 20, Color.RayWhite);
            }
            DrawText("1 - Wire", 10, 70, 20, Color.RayWhite);
            DrawText("2 - Resistor", 10, 100, 20, Color.RayWhite);
            DrawText("3 - Voltage Source", 10, 130, 20, Color.RayWhite);
            DrawText("F - Flow", 10, 160, 20, Color.RayWhite);
            DrawText("R - Reset", 10, 190, 20, Color.RayWhite);
            DrawText($"Selected = {WireType.Name}", 10, 220, 20, Color.RayWhite);
            foreach (var wire in Wires)
            {
                DrawLineEx(wire.Start, wire.End, 2, GetColor(wire));
            }
        }

        private Color GetColor(Wire wire)
        {
            if(Hovered == wire)
                return Color.Yellow;
            if(wire.GetType() == typeof(VoltageSource))
                return Color.Red;
            if (wire.GetType() == typeof(Resistor))
                return Color.Blue;
            if (wire.GetType() == typeof(Wire))
                return Color.White;

            return Color.Gray;
        }
        private Vector2 SnapToGrid(Vector2 position)
        {
            float x = MathF.Round(position.X / GridSize) * GridSize;
            float y = MathF.Round(position.Y / GridSize) * GridSize;
            return new Vector2(x, y);
        }

        public void CreateWire(Vector2 wireEnd)
        {
            Wire newWire;
            if (WireType == typeof(Resistor))
            {
                newWire = new Resistor { Start = wireStart, End = wireEnd, Resistance = 1 };
            }
            else if(WireType == typeof(VoltageSource))
            {
                newWire = new VoltageSource { Voltage = 10, Start = wireStart, End = wireEnd };
            }
            else
            {
                newWire = new Wire { Start = wireStart, End = wireEnd };
            }

            Wires.Add(newWire);
            System.Console.WriteLine("Created Wire");

            // Connect to other wires if endpoints match
            foreach (var wire in Wires)
            {
                if (wire != newWire)
                {
                    if (wire.End == newWire.Start)
                    {
                        wire.Outputs.Add(newWire);
                        newWire.Inputs.Add(wire);
                        System.Console.WriteLine("Connected Wires: wire.End == newWire.Start");
                    }
                    if (wire.Start == newWire.End)
                    {
                        newWire.Outputs.Add(wire);
                        wire.Inputs.Add(newWire);
                        System.Console.WriteLine("Connected Wires: newWire.End == wire.Start");
                    }
                }
            }
        }
    }
}
