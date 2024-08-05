using CircuitSim.Core.Common;
using CircuitSim.Desktop;
using Raylib_cs;
using static Raylib_cs.Raylib;



var manager = SimulationManager.Instance;
manager.UseCircuit(Circuit.FromTemplate("Displays"));
SetConfigFlags(ConfigFlags.ResizableWindow | ConfigFlags.FullscreenMode);
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