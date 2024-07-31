using CircuitSim.Core.Common;
using CircuitSim.Desktop;
using Raylib_cs;
using static Raylib_cs.Raylib;

var json = File.ReadAllText(@"Templates/OhmsLaw.json");
var circuit = Circuit.DeserializeFromJson(json);
var manager = SimulationManager.Instance;
manager.UseCircuit(circuit);
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