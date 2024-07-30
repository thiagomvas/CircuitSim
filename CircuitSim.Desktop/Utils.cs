namespace CircuitSim.Desktop
{
    internal static class Utils
    {
        public static Raylib_cs.Color SystemDrawingColorToRaylib(System.Drawing.Color color)
            => new Raylib_cs.Color(color.R, color.G, color.B, color.A);
    }
}
