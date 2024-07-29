using CircuitSim.Core.Components;
using CircuitSim.Desktop;
using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;

var manager = new SimulationManager();

SetConfigFlags(ConfigFlags.ResizableWindow);
SetConfigFlags(ConfigFlags.FullscreenMode);
InitWindow(1920, 1080, "CircuitSim");
SetTargetFPS(60);

while (!WindowShouldClose())
{
    manager.Update();
    BeginDrawing();
    ClearBackground(Color.DarkGray);
    manager.Draw();
    EndDrawing();
}
CloseWindow();