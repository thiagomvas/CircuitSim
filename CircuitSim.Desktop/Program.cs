﻿using CircuitSim.Core.Common;
using CircuitSim.Core.Components;
using CircuitSim.Desktop;
using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;

var manager = new SimulationManager();

var source = new VoltageSource() { Voltage = 10, Start = new(400, 100), End = new(500, 100) };
var resistor = new Resistor() {  Resistance = 1, Start = new(500, 100), End =new(600, 100) };
var wire = new Wire() { Start = new(600, 100), End = new(700, 100) };
source.Outputs.Add(resistor);
resistor.Inputs.Add(source);
resistor.Outputs.Add(wire);
wire.Inputs.Add(resistor);
manager.Wires.Add(source);
manager.Wires.Add(resistor);
manager.Wires.Add(wire);


SetConfigFlags(ConfigFlags.ResizableWindow);
SetConfigFlags(ConfigFlags.FullscreenMode);
InitWindow(1920, 1080, "CircuitSim");
SetTargetFPS(60);

while (!WindowShouldClose())
{
    manager.Update();
    source.Flow();
    source.Voltage = Math.Sin(GetTime()) * 10 + 10;
    BeginDrawing();
    ClearBackground(Constants.BackgroundColor);
    manager.Draw();
    EndDrawing();
}
CloseWindow();