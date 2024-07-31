
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

        public static float CurrentSpeedScale = 1000;

        /// <summary>
        /// The background color.
        /// </summary>
        public static readonly Color BackgroundColor = Color.Black;

        public static readonly Color GridColor = new Color(0x0f, 0x0f, 0x0f, 0xff);
    }
}
