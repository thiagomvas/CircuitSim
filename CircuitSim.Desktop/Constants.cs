
using Raylib_cs;

namespace CircuitSim.Desktop
{
    internal static class Constants
    {
        /// <summary>
        /// The width of the wire.
        /// </summary>
        public const float WireWidth = 4;

        public const float CurrentBallRadius = 5;
        public const float Margin = 50;
        public static float CurrentSpeedScale = 100;
        public static float GridSize = 20.0f; // Define the grid size
        public const float GridLineWidth = 1.0f; // Define the grid line width

        /// <summary>
        /// The background color.
        /// </summary>
        public static readonly Color BackgroundColor = new Color(0x11, 0x83, 0xa5, 0xff);

        public static readonly Color GridColor = Color.White;
        public static readonly Color FaintGridColor = new Color(0xff, 0xff, 0xff, 0x80);

        public static readonly Color InputHoveredColor = Color.Red;
        public static readonly Color OutputHoveredColor = Color.DarkGreen;
        public static readonly Color HoveredColor = Color.Yellow;
    }
}
