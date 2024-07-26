using CircuitSim.Console;
using CircuitSim.Core.Common;
using CircuitSim.Core.Components;

var root = new Wire() { Voltage = 5, Current = 0.1 };
var led = new Resistor() { Resistance = 10 };
root.Outputs.Add(led);
var secondWire = new Wire();
led.Outputs.Add(secondWire);
var secondLed = new Resistor() { Resistance = 10 };
secondWire.Outputs.Add(secondLed);
var thirdWire = new Wire();
secondLed.Outputs.Add(thirdWire);

// List voltage of all wires
Console.WriteLine($"Source: {root}");
Console.WriteLine($"LED: {led}");
Console.WriteLine($"Wire2: {secondWire}");
Console.WriteLine($"LED2: {secondLed}");
Console.WriteLine($"Wire3: {thirdWire}");


Console.WriteLine("Root -> LED -> Wire2 -> LED2 -> Wire3");

Console.WriteLine("AFTER FLOW");
root.Flow();
Console.WriteLine($"Source: {root}");
Console.WriteLine($"LED: {led}");
Console.WriteLine($"Wire2: {secondWire}");
Console.WriteLine($"LED2: {secondLed}");
Console.WriteLine($"Wire3: {thirdWire}");