using CircuitSim.Core.Common;
using CircuitSim.Core.Components;

var root = new VoltageSource() { Voltage = 5 };
var resistor = new Resistor() { Resistance = 10 };
var resistor2 = new Resistor() { Resistance = 10 };
var wire1 = new Wire();
var wire2 = new Wire();
var wire3 = new Wire();
var wire4 = new Wire();
root.Outputs.Add(wire1);
wire1.Outputs.Add(resistor);
resistor.Outputs.Add(wire2);

root.Outputs.Add(wire3);
wire3.Outputs.Add(resistor2);
resistor2.Outputs.Add(wire4);

Console.WriteLine($"Source: {root}");
Console.WriteLine($"Resistor: {resistor}");
Console.WriteLine($"Wire: {wire1}");
Console.WriteLine($"Resistor2: {resistor2}");
Console.WriteLine($"Wire2: {wire2}");
Console.WriteLine($"Total Res: {root.GetCircuitResistance()}");
Console.WriteLine($"Wire1 Res: {wire1.GetCircuitResistance()}");
Console.WriteLine($"Wire2 Res: {wire2.GetCircuitResistance()}");


root.Flow();
Console.WriteLine("======================================");
Console.WriteLine($"Source: {root}");
Console.WriteLine($"Resistor: {resistor}");
Console.WriteLine($"Wire: {wire1}");
Console.WriteLine($"Wire2: {wire2}");
Console.WriteLine();
Console.WriteLine($"Wire3: {wire3}");
Console.WriteLine($"Resistor2: {resistor2}");
Console.WriteLine($"Wire4: {wire4}");
