using CircuitSim.Core;
using CircuitSim.Core.Common;
using CircuitSim.Core.Components;
using Raylib_cs;
using System.ComponentModel.Design;
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
        /// Gets the circuit currently being rendered
        /// </summary>
        public Circuit Circuit { get; private set; }

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
            Circuit = new();
        }

        private double maxVoltage = 1;
        private bool isDrawing = false;
        private Vector2 wireStart;
        private const float GridSize = 20.0f; // Define the grid size
        public Wire? Hovered = null;
        private Wire drawPreview = new();
        public Type WireType { get; set; } = typeof(Wire);

        /// <summary>
        /// Changes the circuit being rendered.
        /// </summary>
        /// <param name="circuit">The new circuit to render</param>
        public void UseCircuit(Circuit circuit)
        {
            this.Circuit = circuit;
        }

        /// <summary>
        /// Updates the simulation state.
        /// </summary>
        public void Update()
        {
            if (IsKeyPressed(KeyboardKey.W))
            {
                WireType = typeof(Wire);
            }
            if (IsKeyPressed(KeyboardKey.R))
            {
                WireType = typeof(Resistor);
            }
            if (IsKeyPressed(KeyboardKey.S))
            {
                WireType = typeof(VoltageSource);
            }
            if (IsKeyPressed(KeyboardKey.G))
                WireType = typeof(Ground);
            if (IsKeyPressed(KeyboardKey.L))
                WireType = typeof(LED);
            if (IsKeyPressed(KeyboardKey.A))
                WireType = typeof(Ammeter);
            if (IsKeyPressed(KeyboardKey.V))
                WireType = typeof(Voltmeter);
            if (IsKeyPressed(KeyboardKey.O))
                WireType = typeof(Ohmeter);
            if(IsKeyPressed(KeyboardKey.Delete))
            {
                if (Hovered != null)
                    Circuit.RemoveWire(Hovered);
            }

            if (IsKeyPressed(KeyboardKey.F))
            {
                foreach (var wire in Circuit.Wires)
                    wire.Reset();

                foreach (var wire in Circuit.Wires)
                    if (wire.GetType() == typeof(VoltageSource))
                        wire.Flow();
            }
            if (IsKeyPressed(KeyboardKey.J))
                Console.WriteLine(Circuit.SerializeToJson());

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
            }
            else
            {
                Hovered = null;
                foreach (var wire in Circuit.Wires)
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
        }

        /// <summary>
        /// Draws the simulation.
        /// </summary>
        public void Draw()
        {

            for (int i = 0; i < GetScreenWidth(); i += (int)GridSize)
            {
                DrawLine(i, 0, i, GetScreenHeight(), Constants.GridColor);
            }
            for (int i = 0; i < GetScreenHeight(); i += (int)GridSize)
            {
                DrawLine(0, i, GetScreenWidth(), i, Constants.GridColor);
            }

            if (Hovered != null)
            {
                DrawText($"Voltage: {Hovered.Voltage:0.00000}V", 10, 10, 20, Color.RayWhite);
                DrawText($"Current: {Hovered.preFlowCurrent:0.00000}A", 10, 40, 20, Color.RayWhite);
            }
            DrawText("1 - Wire", 10, 70, 20, Color.RayWhite);
            DrawText("2 - Resistor", 10, 100, 20, Color.RayWhite);
            DrawText("3 - Voltage Source", 10, 130, 20, Color.RayWhite);
            DrawText("F - Flow", 10, 160, 20, Color.RayWhite);
            DrawText("R - Reset", 10, 190, 20, Color.RayWhite);
            DrawText($"Selected = {WireType.Name}", 10, 220, 20, Color.RayWhite);
            DrawText($"Max: {maxVoltage}V", 10, 250, 20, Color.RayWhite);
            DrawText("J - Serialize", 10, 280, 20, Color.RayWhite);
            foreach (var wire in Circuit.Wires)
            {
                WireRenderer.Render(wire, GetColor(wire));
            }
            if(isDrawing)
                WireRenderer.Render(drawPreview, Color.RayWhite);
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
                newWire = new Resistor { Start = wireStart, End = wireEnd};
            }
            else if (WireType == typeof(VoltageSource))
            {
                newWire = new VoltageSource { SupplyVoltage = 10, Start = wireStart, End = wireEnd };
            }
            else
            {
                newWire = (Wire) Activator.CreateInstance(WireType)!;
                newWire.Start = wireStart;
                newWire.End = wireEnd;
            }
            Circuit.AddWire(newWire);

        }
    }
}
