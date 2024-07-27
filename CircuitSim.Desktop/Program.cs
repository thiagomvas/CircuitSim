using Raylib_cs;
using static Raylib_cs.Raylib;

InitWindow(800, 450, "CircuitSim");
SetTargetFPS(60);

while (!WindowShouldClose())
{
    BeginDrawing();
    ClearBackground(Color.DarkGray);
    DrawText("Congrats! You created your first window!", 190, 200, 20, Color.RayWhite);
    EndDrawing();
}
CloseWindow();