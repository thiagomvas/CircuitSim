using CircuitSim.Core.Common;
using static Raylib_cs.Raylib;
using Raylib_cs;
using CircuitSim.Core.Components;
using System.Numerics;

namespace CircuitSim.Desktop
{
    public static class WireRenderer
    {
        public static void Render(Wire wire, Color color)
        {
            var type = wire.GetType();
            if (type == typeof(Wire))
            {
                RenderWire(wire, color);
                return;
            }
            else if (type == typeof(Resistor))
            {
                RenderResistor(wire, color);
                return;
            }
            else if (type == typeof(VoltageSource))
            {
                RenderVoltageSource(wire, color);
                return;
            }
            else
                RenderWire(wire, color);
        }
        private static void RenderWire(Wire wire, Color color)
        {
            DrawLineEx(wire.Start, wire.End, Constants.WireWidth, color);
        }

        private static void RenderResistor(Wire wire, Color color)
        {
            var rectSize = new Vector2(wire.Length * 0.5f, 50);
            var innerRectSize = new Vector2(wire.Length * 0.5f - Constants.WireWidth, 50 - Constants.WireWidth);
            DrawLineEx(wire.Start, wire.End, Constants.WireWidth, color);
            DrawRectanglePro(new(wire.Center, rectSize), rectSize/2, wire.AngleDeg, color);
            DrawRectanglePro(new(wire.Center, innerRectSize), innerRectSize/2, wire.AngleDeg, Constants.BackgroundColor);
            string txt = $"{wire.Resistance} Ohms";
            DrawTextPro(GetFontDefault(),
                        txt,
                        wire.Center,
                        MeasureTextEx(GetFontDefault(), txt, 16, 1) * 0.5f,
                        wire.AngleDeg,
                        16,
                        1,
                        Color.White);
            
        }

        private static void RenderVoltageSource(Wire wire, Color color)
        {
            var rectSize = new Vector2(wire.Length * 0.5f, 50);
            var innerRectSize = new Vector2(wire.Length * 0.5f - Constants.WireWidth, 50 - Constants.WireWidth);

            Vector2 pos = wire.Start + wire.Direction * wire.Length * 0.5f;
            DrawLineEx(wire.Center, wire.End, Constants.WireWidth, color);
            DrawRectanglePro(new Rectangle(pos, rectSize), rectSize * 0.5f, wire.AngleDeg, color);
            DrawRectanglePro(new Rectangle(pos, innerRectSize), innerRectSize * 0.5f, wire.AngleDeg, Constants.BackgroundColor);
            string txt = $"{wire.Voltage:0.00}V";
            DrawTextPro(GetFontDefault(),
                        txt,
                        pos,
                        MeasureTextEx(GetFontDefault(), txt, 16, 1) * 0.5f,
                        wire.AngleDeg,
                        16,
                        1,
                        Color.White);

        }
    }
}
