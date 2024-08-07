using CircuitSim.Core.Common;
using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;
namespace CircuitSim.Desktop;

internal static class Utils
{
    public static Raylib_cs.Color SystemDrawingColorToRaylib(System.Drawing.Color color)
        => new Raylib_cs.Color(color.R, color.G, color.B, color.A);

    public static void DrawLineStrip(Color color, params Vector2[] points)
    {
        for (int i = 1; i < points.Length; i++)
        {
            DrawLineEx(points[i - 1], points[i], Constants.WireWidth, color);
        }
    }

    public static void DrawCurrent(Vector2 start, Vector2 end, Wire wire)
    {
        var dir = end - start;
        var len = dir.Length();
        if (wire.Current > 0)
        {
            var ballCount = (int)(len / 20);
            for (int i = 0; i < ballCount; i++)
            {
                var pos = start + Vector2.Normalize(dir) * (((float)GetTime() * (float)wire.Current * Constants.CurrentSpeedScale + i * 20) % len);
                DrawCircleV(pos, Constants.CurrentBallRadius, Color.Yellow);
            }
        }
    }

    public static void DrawCurrent(Wire wire)
    {
        if (wire.Current > 0)
        {
            var ballCount = (int)(wire.Length / 20);
            for (int i = 0; i < ballCount; i++)
            {
                var pos = wire.Start + wire.Direction * (((float)GetTime() * (float)wire.Current * Constants.CurrentSpeedScale + i * 20) % wire.Length);
                DrawCircleV(pos, Constants.CurrentBallRadius + 1, Color.Yellow);
            }
        }
    }
    public static void DrawTextBox(string text, Vector2 center, float angle, Color boxColor, Color textColor, float fontSize = 16)
    {
        var textSize = MeasureTextEx(GetFontDefault(), text, fontSize, 1);
        var rectSize = textSize + new Vector2(10 + Constants.WireWidth);
        var innerRectSize = rectSize - new Vector2(Constants.WireWidth);
        DrawRectanglePro(new Rectangle(center, rectSize), rectSize * 0.5f, angle, boxColor);
        DrawRectanglePro(new Rectangle(center, innerRectSize), innerRectSize * 0.5f, angle, Constants.BackgroundColor);
        DrawTextPro(GetFontDefault(),
                    text,
                    center,
                    textSize * 0.5f,
                    angle,
                    fontSize,
                    1,
                    textColor);
    }
}
