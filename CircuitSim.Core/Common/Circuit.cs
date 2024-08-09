using CircuitSim.Core.DTOs;
using Newtonsoft.Json.Linq;

namespace CircuitSim.Core.Common;

/// <summary>
/// Represents a circuit composed of wires.
/// </summary>
public class Circuit
{
    /// <summary>
    /// Gets or sets the list of wires in the circuit.
    /// </summary>
    public List<Wire> Wires { get; set; } = new List<Wire>();

    /// <summary>
    /// Adds a new wire to the circuit and connects it to existing wires if applicable.
    /// </summary>
    /// <param name="newWire">The wire to add to the circuit.</param>
    public void AddWire(Wire newWire)
    {
        foreach (var wire in Wires)
        {
            if (wire != newWire)
            {
                if (wire.End == newWire.Start)
                {
                    wire.Outputs.Add(newWire);
                    newWire.Inputs.Add(wire);
                }
                if (wire.Start == newWire.End)
                {
                    newWire.Outputs.Add(wire);
                    wire.Inputs.Add(newWire);
                }
            }
        }
        Wires.Add(newWire);
        if(newWire is MultiConnectionWire multi)
        {
            multi.AttachChildren(this);
        }
    }

    /// <summary>
    /// Serializes the circuit to JSON format.
    /// </summary>
    /// <returns>The JSON representation of the circuit.</returns>
    public string SerializeToJson()
    {
        var entity = new JObject();
        var dtos = Wires.Select(w => w.ToDTO());
        var jarray = JArray.FromObject(dtos);
        entity["Wires"] = JArray.FromObject(dtos);
        return entity.ToString(Newtonsoft.Json.Formatting.Indented);
    }

    /// <summary>
    /// Deserializes a circuit from JSON format.
    /// </summary>
    /// <param name="json">The JSON representation of the circuit.</param>
    /// <returns>The deserialized circuit.</returns>
    public static Circuit DeserializeFromJson(string json)
    {
        var entity = JObject.Parse(json);
        var circuit = new Circuit();

        // Converting the JSON to a list of WireDTOs
        var dtos = entity["Wires"].ToObject<List<WireDTO>>();
        foreach (var wire in entity["Wires"])
        {
            // Parsing the wire type
            var type = ByName(wire["Type"]?.Value<string>()?.Split(',').First() ?? string.Empty);
            var props = type.GetProperties();

            // Creating an instance of that type
            var wireInstance = Activator.CreateInstance(type);

            // Defining the properties of the instance using Reflection
            foreach (var prop in wire["Data"] as JObject)
            {
                if (props.Any(p => p.Name == prop.Key))
                {
                    var propInfo = props.First(p => p.Name == prop.Key);
                    if (propInfo.CanWrite)
                        propInfo.SetValue(wireInstance, prop.Value.ToObject(propInfo.PropertyType));
                }
            }

            // After all that headache, add to the circuit.
            circuit.AddWire((Wire)wireInstance);
        }

        return circuit;
    }

    /// <summary>
    /// Gets the type by its name.
    /// </summary>
    /// <param name="name">The name of the type.</param>
    /// <returns>The type with the specified name.</returns>
    private static Type? ByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Reverse())
        {
            var tt = assembly.GetType(name);
            if (tt != null)
            {
                return tt;
            }
        }

        return null;
    }

    public void RemoveWire(Wire hovered)
    {
        // Check if the wire is connected to a multiwire as an input or output
        var multiWire = hovered.Inputs.OfType<MultiConnectionWire>().FirstOrDefault() ??
                        hovered.Outputs.OfType<MultiConnectionWire>().FirstOrDefault() ??
                        hovered as MultiConnectionWire;

        if (multiWire != null)
        {
            // Remove the multiwire and all its connected wires
            multiWire.RemoveSubWiresFromCircuit(this);
            return;
        }

        // Otherwise, remove the wire normally
        RemoveWireNoMult(hovered);
    }
    internal void RemoveWireNoMult(Wire hovered)
    {
        foreach (var wire in hovered.Inputs)
        {
            wire.Outputs.Remove(hovered);
        }

        foreach (var wire in hovered.Outputs)
        {
            wire.Inputs.Remove(hovered);
        }

        Wires.Remove(hovered);
    }
    public static Circuit FromTemplate(string templateName)
    {
        if (!File.Exists($"Templates/{templateName}.json"))
            return new();

        var template = File.ReadAllText($"Templates/{templateName}.json");
        return DeserializeFromJson(template);

    }
}
