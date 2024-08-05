﻿using CircuitSim.Core;
using CircuitSim.Core.Common;
using CircuitSim.Core.Components;
using CircuitSim.Desktop.Input;
using Raylib_cs;
using System.ComponentModel.Design;
using System.Numerics;
using static CircuitSim.Desktop.UISystem;
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

        private InputSystem _inputSystem;
        private UISystem _uiSystem;

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
        public Wire? Selected = null;
        private Wire drawPreview = new();
        public Type WireType { get; set; } = typeof(Wire);
        public bool ShowControls = false;

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
            _uiSystem ??= new(GetScreenWidth(), GetScreenHeight());
            if (_inputSystem == null)
            {
                _inputSystem = new();
                _uiSystem.AddDrawer(_inputSystem.Keymappings.Select(kvp => new DrawerButton(kvp.Value.Name, () => _inputSystem.CheckForInput(kvp.Key))).ToArray());
            }
            var key = GetKeyPressed();
            if(key != 0)
            {
                _inputSystem.CheckForInput((KeyboardKey)key);
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
                if(wireStart != wireEnd)
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
            if(IsMouseButtonPressed(MouseButton.Right))
            {
                Selected = Hovered;
                _uiSystem.SelectWire(Selected);
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

            DrawText($"Selected: {WireType.Name}", 10, Raylib.GetScreenHeight() - 20, 20, Color.RayWhite);

            if (ShowControls)
            {
                int i = 0;
                foreach(var (_, mapping) in _inputSystem.Keymappings)
                {
                    DrawText($"[{mapping.Key}] - {mapping.Name}", 10, 40 + i * 30, 20, Color.RayWhite);
                    i++;
                }
            }

            if(Hovered != null)
            {
                var txt = $"{Hovered.GetType().Name}\n\nVoltage: {Hovered.Voltage:0.00000}V\n\nCurrent: {Hovered.Current:0.00000}A\n\nResistance: {Hovered.Resistance:0.00000} Ohms";
                var textSize = MeasureTextEx(GetFontDefault(), txt, 16, 1);
                Utils.DrawTextBox(txt,
                    new Vector2(10, GetScreenHeight()) + new Vector2(textSize.X, -textSize.Y), 
                    0, 
                    Color.RayWhite, 
                    Color.RayWhite,
                    20);

            }

            foreach (var wire in Circuit.Wires)
            {
                WireRenderer.Render(wire, GetColor(wire));
            }
            if(isDrawing)
                WireRenderer.Render(drawPreview, Color.RayWhite);

            _uiSystem.DrawUI();
            if (Selected != null)
            {
                _uiSystem.DrawPropertyInputFields();
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
        public void DeleteHovered()
        {
            if (Hovered != null)
                Circuit.RemoveWire(Hovered); 
        }

        public void BeginFlow()
        {
            foreach (var wire in Circuit.Wires)
                wire.Reset();

            foreach (var wire in Circuit.Wires)
                if (wire.GetType() == typeof(VoltageSource))
                    wire.Flow();
        }
    }
}
