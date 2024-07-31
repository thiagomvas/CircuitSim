using CircuitSim.Core.Common;
using CircuitSim.Core.Components;
using System.Numerics;

Circuit circuit = new();
circuit.Wires.Add(new VoltageSource() { SupplyVoltage = 10, Start = Vector2.Zero, End = new(1, 0) });
circuit.Wires.Add(new Resistor() { Resistance = 1000, Start = new(1, 0), End = new(1, 1) });
circuit.Wires.Add(new Wire() { Start = new(1, 1), End = new(2, 1) });

var json = circuit.SerializeToJson();

Console.WriteLine(json);

var deserialized = Circuit.DeserializeFromJson(json);

foreach (var wire in deserialized.Wires)
{
    var type = wire.GetType();
    if(type == typeof(VoltageSource))
    {
        var vs = (VoltageSource)wire;
        Console.WriteLine($"Voltage Source: {vs.SupplyVoltage}V");
    }
    else if(type == typeof(Resistor))
    {
        var res = (Resistor) wire;
        Console.WriteLine($"Resistor: {res.Resistance} Ohms");
    }
    else if(type == typeof(Wire))
    {
        Console.WriteLine("Wire");
    }
}