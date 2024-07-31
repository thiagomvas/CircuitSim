using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;
namespace CircuitSim.Desktop
{
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
    }
}
