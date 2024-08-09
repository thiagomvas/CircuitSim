using CircuitSim.Core.Common;
using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;
namespace CircuitSim.Desktop;

internal static class Utils
{
    public static Raylib_cs.Color SystemDrawingColorToRaylib(System.Drawing.Color color)
        => new Raylib_cs.Color(color.R, color.G, color.B, color.A);
    public static double ParseValue(string formattedValue)
    {
        if (string.IsNullOrWhiteSpace(formattedValue))
            return 0;

        // Trim any leading or trailing whitespace
        formattedValue = formattedValue.Trim();

        // Regex to handle scientific notation and SI prefixes
        var match = System.Text.RegularExpressions.Regex.Match(formattedValue, @"^([+-]?\d*\.?\d+([eE][+-]?\d+)?)([a-zA-Zµ]+)?$");
        if (!match.Success)
            return double.NaN;

        // Parse the numeric part with scientific notation
        if (!double.TryParse(match.Groups[1].Value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double number))
            return double.NaN;

        // Handle SI prefixes
        var prefix = match.Groups[3].Value;
        int exponent = GetSiExponent(prefix);
        if (exponent != int.MinValue)
        {
            number *= Math.Pow(10, exponent);
        }
        else if (!string.IsNullOrEmpty(prefix))
        {
            return double.NaN;
        }

        return number;
    }

    private static int GetSiExponent(string prefix)
    {
        return prefix switch
        {
            "y" => -24,
            "z" => -21,
            "a" => -18,
            "f" => -15,
            "p" => -12,
            "n" => -9,
            "µ" => -6,
            "m" => -3,
            "k" => 3,
            "M" => 6,
            "G" => 9,
            "T" => 12,
            "P" => 15,
            "E" => 18,
            "Z" => 21,
            "Y" => 24,
            _ => int.MinValue
        };
    }


    public static string FormatValue(double value)
    {
        if (value == 0)
            return "0";

        var absValue = Math.Abs(value);
        int exponent = (int)Math.Floor(Math.Log10(absValue));
        int siExponent = exponent / 3 * 3;
        double normalizedValue = value / Math.Pow(10, siExponent);

        if (normalizedValue % 1 == 0)
        {
            return $"{(int)normalizedValue}{GetSiPrefix(siExponent)}";
        }

        string result = $"{normalizedValue:F3}".TrimEnd('0').TrimEnd('.');
        return $"{result}{GetSiPrefix(siExponent)}";
    }

    private static string GetSiPrefix(int siExponent)
    {
        return siExponent switch
        {
            -24 => "y",
            -21 => "z",
            -18 => "a",
            -15 => "f",
            -12 => "p",
            -9 => "n",
            -6 => "µ",
            -3 => "m",
            0 => "",
            3 => "k",
            6 => "M",
            9 => "G",
            12 => "T",
            15 => "P",
            18 => "E",
            21 => "Z",
            24 => "Y",
            _ => ""
        };
    }

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
