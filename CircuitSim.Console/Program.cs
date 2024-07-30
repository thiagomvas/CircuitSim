using CircuitSim.Core.Common;
using System.Reflection;

var types = Assembly.GetAssembly(typeof(Wire))
    .GetTypes()
    .Where(t => t.IsAssignableTo(typeof(Wire)));

foreach (var type in types)
    Console.WriteLine(type.Name);