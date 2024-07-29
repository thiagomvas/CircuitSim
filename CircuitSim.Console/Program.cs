using CircuitSim.Core.Common;
using CircuitSim.Core.Components;

var source = new VoltageSource() { Voltage = 10 };
var wire = new Wire();
var resistor = new Resistor() { Resistance = 1 };
var wire2 = new Wire();
var resistor2 = new Resistor() { Resistance = 1 };
var wire3 = new Wire();

source.Outputs.Add(wire);
wire.Outputs.Add(resistor);
resistor.Outputs.Add(wire2);
wire2.Outputs.Add(resistor2);
resistor2.Outputs.Add(wire3);

source.Flow();

Console.WriteLine($"Source: {source}");
Console.WriteLine($"Wire: {wire}");
Console.WriteLine($"Resistor: {resistor}");
Console.WriteLine($"Wire2: {wire2}");
Console.WriteLine($"Resistor2: {resistor2}");
Console.WriteLine($"Wire3: {wire3}");