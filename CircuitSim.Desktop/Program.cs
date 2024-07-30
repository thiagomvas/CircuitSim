using CircuitSim.Core.Common;
using CircuitSim.Core.Components;
using CircuitSim.Desktop;
using Raylib_cs;
using static Raylib_cs.Raylib;

var manager = SimulationManager.Instance;

SetConfigFlags(ConfigFlags.ResizableWindow);
SetConfigFlags(ConfigFlags.FullscreenMode);
InitWindow(1920, 1080, "CircuitSim");
SetTargetFPS(60);

while (!WindowShouldClose())
{
    manager.Update();
    BeginDrawing();
    ClearBackground(Constants.BackgroundColor);
    manager.Draw();
    EndDrawing();
}
CloseWindow();