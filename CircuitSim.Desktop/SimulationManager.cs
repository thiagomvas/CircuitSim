using CircuitSim.Core.Common;
using CircuitSim.Core.Components;
using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;

namespace CircuitSim.Desktop
{
    /// <summary>
    /// Manages the simulation of the circuit.
    /// </summary>
    internal class SimulationManager
    {
        private static SimulationManager instance;

        /// <summary>
        /// Gets the singleton instance of the SimulationManager.
        /// </summary>
        public static SimulationManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new();
                return instance;
            }
        }

        private SimulationManager()
        {
        }

        private double maxVoltage = 1;
        private bool isDrawing = false;
        private Vector2 wireStart;
        private const float GridSize = 20.0f; // Define the grid size
        public List<Wire> Wires { get; set; } = new();
        public Wire? Hovered = null;
        private Wire drawPreview = new();
        public Type WireType { get; set; } = typeof(Wire);

        /// <summary>
        /// Updates the simulation state.
        /// </summary>
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
            if (IsKeyPressed(KeyboardKey.Three))
            {
                WireType = typeof(VoltageSource);
            }

            if (IsMouseButtonPressed(MouseButton.Left))
            {
                isDrawing = true;
                drawPreview = (Wire)Activator.CreateInstance(WireType);
                wireStart = SnapToGrid(GetMousePosition());
                drawPreview!.Start = SnapToGrid(GetMousePosition());
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
                drawPreview.End = mousePos;
                WireRenderer.Render(drawPreview, Color.RayWhite);
            }
            else
            {
                Hovered = null;
                foreach (var wire in Wires)
                {
                    if (wire.Voltage > maxVoltage)
                        maxVoltage = wire.Voltage;
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

        /// <summary>
        /// Draws the simulation.
        /// </summary>
        public void Draw()
        {
            if (Hovered != null)
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
            DrawText($"Max: {maxVoltage}V", 10, 250, 20, Color.RayWhite);
            foreach (var wire in Wires)
            {
                WireRenderer.Render(wire, GetColor(wire));
            }
        }

        private Color GetColor(Wire wire)
        {
            if (Hovered == wire)
                return Color.Blue;
            if (Hovered != null && Hovered.Inputs.Contains(wire))
                return Color.Red;
            if (Hovered != null && Hovered.Outputs.Contains(wire))
                return Color.Yellow;

            return new Color(25, (int)Math.Min((wire.Voltage / maxVoltage * 230) + 25, 255), 25, 255);
        }

        private Vector2 SnapToGrid(Vector2 position)
        {
            float x = MathF.Round(position.X / GridSize) * GridSize;
            float y = MathF.Round(position.Y / GridSize) * GridSize;
            return new Vector2(x, y);
        }

        /// <summary>
        /// Creates a new wire and adds it to the simulation.
        /// </summary>
        /// <param name="wireEnd">The end point of the wire.</param>
        public void CreateWire(Vector2 wireEnd)
        {
            Wire newWire;
            if (WireType == typeof(Resistor))
            {
                newWire = new Resistor { Start = wireStart, End = wireEnd, Resistance = 1 };
            }
            else if (WireType == typeof(VoltageSource))
            {
                newWire = new VoltageSource { SupplyVoltage = 10, Start = wireStart, End = wireEnd };
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
                    }
                    if (wire.Start == newWire.End)
                    {
                        newWire.Outputs.Add(wire);
                        wire.Inputs.Add(newWire);
                    }
                }
            }
        }
    }
}
