using CircuitSim.Core;
using CircuitSim.Core.Common;
using CircuitSim.Core.Components;
using Raylib_cs;
using System.Numerics;
using System.Reflection;
using static Raylib_cs.Raylib;

namespace CircuitSim.Desktop;

/// <summary>
/// Utility class for rendering different components using <see cref="Raylib"/>
/// </summary>
public static class WireRenderer
{
    private static readonly Dictionary<Type, Action<Wire, Color>> RenderActions;

    static WireRenderer()
    {
        RenderActions = new Dictionary<Type, Action<Wire, Color>>();

        // Get all methods that match the rendering pattern and add them to the dictionary
        var methods = typeof(WireRenderer).GetMethods(BindingFlags.NonPublic | BindingFlags.Static);
        foreach (var method in methods)
        {
            if (method.Name.StartsWith("Render") && method.GetParameters().Length == 2 &&
                method.GetParameters()[0].ParameterType == typeof(Wire) &&
                method.GetParameters()[1].ParameterType == typeof(Color))
            {
                var action = (Action<Wire, Color>)Delegate.CreateDelegate(typeof(Action<Wire, Color>), method);
                Type? wireType = method.Name == "RenderWire" ? typeof(Wire) : Assembly.GetAssembly(typeof(Wire))
                    .GetTypes()
                    .FirstOrDefault(t => t.Name == method.Name[6..]);


                if (wireType != null)
                {
                    System.Console.WriteLine($"Assigning {method.Name} to {wireType.Name}");
                    RenderActions[wireType] = action;
                }
            }
        }

        // Ensure default rendering for Wire
        if (!RenderActions.ContainsKey(typeof(Wire)))
        {
            RenderActions[typeof(Wire)] = RenderWire;
        }
    }

    /// <summary>
    /// Renders a wire on screen. 
    /// </summary>
    /// <param name="wire">The wire to render</param>
    /// <param name="color">The color of the wire</param>
    public static void Render(Wire wire, Color color)
    {
        var type = wire.GetType();
        if (RenderActions.TryGetValue(type, out var renderAction))
        {
            renderAction(wire, color);
        }
        else
        {
            RenderWire(wire, color);
        }
    }

    private static void RenderWire(Wire wire, Color color)
    {
        DrawLineEx(wire.Start, wire.End, Constants.WireWidth, color);
        Utils.DrawCurrent(wire.Start, wire.End, wire);
    }

    private static void RenderResistor(Wire wire, Color color)
    {
        DrawLineEx(wire.Start, wire.End, Constants.WireWidth, color);

        Utils.DrawCurrent(wire.Start, wire.End, wire);
        string txt = $"{Utils.FormatValue(wire.Resistance)} Ohms";
        Utils.DrawTextBox(txt, wire.Center, wire.AngleDeg, color, Color.White);
    }

    private static void RenderVoltageSource(Wire wire, Color color)
    {
        Vector2 pos = wire.Start + wire.Direction * wire.Length * 0.5f;
        DrawLineEx(wire.Center, wire.End, Constants.WireWidth, color);

        Utils.DrawCurrent(pos, wire.End, wire);
        string txt = $"{Utils.FormatValue(((VoltageSource)wire).SupplyVoltage)}V";
        Utils.DrawTextBox(txt, pos, wire.AngleDeg, color, Color.White);
    }

    private static void RenderLED(Wire wire, Color color)
    {
        LED led = (LED)wire;
        DrawLineEx(wire.Start, wire.Center - wire.Direction * 20, Constants.WireWidth, color);
        DrawLineEx(wire.Center + wire.Direction * 20, wire.End, Constants.WireWidth, color);

        Utils.DrawCurrent(wire.Start, wire.End, wire);

        DrawCircleV(wire.Center, 20, Utils.SystemDrawingColorToRaylib(led.LitColor));
        if (!led.IsOn)
            DrawCircleV(wire.Center, 20 - Constants.WireWidth, Constants.BackgroundColor);
    }

    private static void RenderAmmeter(Wire wire, Color color)
    {
        var normal = new Vector2(wire.Direction.Y, -wire.Direction.X);
        Utils.DrawLineStrip(color, wire.Start,
            wire.Center,
            wire.Center + normal * 10,
            wire.Center + wire.Direction * 10,
            wire.Center - normal * 10,
            wire.Center,
            wire.End);
        Utils.DrawCurrent(wire.Start, wire.End, wire);

        string txt = $"{Utils.FormatValue(wire.Current)}A";
        DrawTextPro(GetFontDefault(),
                    txt,
                    wire.Center + normal * 16,
                    MeasureTextEx(GetFontDefault(), txt, 16, 1) * 0.5f,
                    wire.AngleDeg,
                    16,
                    1,
                    Color.White);
    }

    private static void RenderGround(Wire wire, Color color)
    {
        var normal = new Vector2(wire.Direction.Y, -wire.Direction.X);

        DrawLineEx(wire.Start, wire.End - wire.Direction * 20, Constants.WireWidth, color);
        Utils.DrawCurrent(wire.Start, wire.End - wire.Direction * 20, wire);

        DrawLineEx(wire.End - wire.Direction * 20 + normal * 20, wire.End - wire.Direction * 20 - normal * 20, Constants.WireWidth, color);

        DrawLineEx(wire.End - wire.Direction * 10 + normal * 12, wire.End - wire.Direction * 10 - normal * 12, Constants.WireWidth, color);

        DrawLineEx(wire.End + normal * 4, wire.End - normal * 4, Constants.WireWidth, color);

    }

    private static void RenderVoltmeter(Wire wire, Color color)
    {
        string txt = $"{Utils.FormatValue(wire.Voltage)}V";
        var textSize = MeasureTextEx(GetFontDefault(), txt, 16, 1);
        DrawLineEx(wire.Start, wire.Center - wire.Direction * textSize.X / 2, Constants.WireWidth, color);
        DrawLineEx(wire.Center + wire.Direction * textSize.X / 2, wire.End, Constants.WireWidth, color);
        Utils.DrawCurrent(wire.Start, wire.End, wire);
        DrawRectanglePro(new Rectangle(wire.Center, textSize), textSize * 0.5f, wire.AngleDeg, Constants.BackgroundColor);

        DrawTextPro(GetFontDefault(),
                    txt,
                    wire.Center,
                    textSize * 0.5f,
                    wire.AngleDeg,
                    16,
                    1,
                    Color.White);
    }

    private static void RenderOhmeter(Wire wire, Color color)
    {
        var ohmeter = (Ohmeter)wire;
        string txt = $"{Utils.FormatValue(((Ohmeter) wire).CircuitResistance)} Ohms";
        var textSize = MeasureTextEx(GetFontDefault(), txt, 16, 1);
        DrawLineEx(wire.Start, wire.Center - wire.Direction * textSize.X / 2, Constants.WireWidth, color);
        DrawLineEx(wire.Center + wire.Direction * textSize.X / 2, wire.End, Constants.WireWidth, color);
        Utils.DrawCurrent(wire.Start, wire.End, wire);
        DrawRectanglePro(new Rectangle(wire.Center, textSize), textSize * 0.5f, wire.AngleDeg, Constants.BackgroundColor);
        DrawTextPro(GetFontDefault(),
                    txt,
                    wire.Center,
                    textSize * 0.5f,
                    wire.AngleDeg,
                    16,
                    1,
                    Color.White);
    }

    private static void RenderSwitch(Wire wire, Color color)
    {
        var s = (Switch)wire;
        float switchLength = 0;
        if (wire.Length > Constants.GridSize * 1.5f)
            switchLength = Constants.GridSize * 1.5f;
        else
            switchLength = wire.Length / 2;

        var normal = new Vector2(wire.Direction.Y, -wire.Direction.X);

        DrawLineEx(wire.Start, wire.Center - wire.Direction * switchLength, Constants.WireWidth, color);
        DrawLineEx(wire.Center + wire.Direction * switchLength, wire.End, Constants.WireWidth, color);

        if (s.State)
            DrawLineEx(wire.Center - wire.Direction * switchLength, wire.Center + wire.Direction * switchLength, Constants.WireWidth, Color.Gray);
        else
            DrawLineEx(wire.Center - wire.Direction * switchLength, wire.Center + wire.Direction * switchLength + normal * Constants.GridSize, Constants.WireWidth, Color.Gray);

    }

    private static void RenderNPNTransistor(Wire wire, Color color)
    {
        if(wire.Start != wire.End)
        {
            var npn = (NPNTransistor)wire;
            Wire baseWire = npn.Base, emitterWire = npn.Emitter, collectorWire = npn.Collector;
            // Render the base wire
            var baseNormal = new Vector2(baseWire.Direction.Y, -baseWire.Direction.X);
            DrawLineEx(baseWire.Start, baseWire.End, Constants.WireWidth, color);
            DrawLineEx(baseWire.End + baseNormal * 40, baseWire.End - baseNormal * 40, Constants.WireWidth, color);
            Utils.DrawCurrent(baseWire.Start, baseWire.End, baseWire);

            // Render the emitter wire
            DrawLineEx(emitterWire.Start, emitterWire.Center, Constants.WireWidth, color);
            var emitterNormal = new Vector2(emitterWire.Direction.Y, -emitterWire.Direction.X);
            Utils.DrawLineStrip(color, emitterWire.Center, emitterWire.Center + emitterNormal * 10, emitterWire.Center + emitterWire.Direction * 20, emitterWire.Center - emitterNormal * 10, emitterWire.Center);
            DrawLineEx(emitterWire.Center + emitterWire.Direction * 20, emitterWire.End, Constants.WireWidth, color);
            Utils.DrawCurrent(emitterWire.Start, emitterWire.End, emitterWire);

            // Render the collector wire
            DrawLineEx(collectorWire.Start, collectorWire.End, Constants.WireWidth, color);
            Utils.DrawCurrent(collectorWire.Start, collectorWire.End, collectorWire);
        }
    }

    private static void RenderNPNBase(Wire wire, Color color)
    {
        var normal = new Vector2(wire.Direction.Y, -wire.Direction.X);
        DrawLineEx(wire.Start, wire.End, Constants.WireWidth, color);
        DrawLineEx(wire.End + normal * 40, wire.End - normal * 40, Constants.WireWidth, color);
        Utils.DrawCurrent(wire.Start, wire.End, wire);
    }

    private static void RenderNPNEmitter(Wire wire, Color color)
    {
        DrawLineEx(wire.Start, wire.Center, Constants.WireWidth, color);
        var normal = new Vector2(wire.Direction.Y, -wire.Direction.X);
        Utils.DrawLineStrip(color, wire.Center, wire.Center + normal * 10, wire.Center + wire.Direction * 20, wire.Center - normal * 10, wire.Center);
        DrawLineEx(wire.Center + wire.Direction * 20, wire.End, Constants.WireWidth, color);
        Utils.DrawCurrent(wire.Start, wire.End, wire);
    }

    private static void RenderNPNCollector(Wire wire, Color color)
    {
        DrawLineEx(wire.Start, wire.End, Constants.WireWidth, color);
        Utils.DrawCurrent(wire.Start, wire.End, wire);
    }
}
