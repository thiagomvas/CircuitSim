using CircuitSim.Core.DTOs;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CircuitSim.Core.Common
{
    public class Circuit
    {
        public List<Wire> Wires { get; set; } = new List<Wire>();

        public string SerializeToJson()
        {
            var entity = new JObject();
            var dtos = Wires.Select(w => w.ToDTO());
            var jarray = JArray.FromObject(dtos);
            entity["Wires"] = JArray.FromObject(dtos);
            return entity.ToString(Newtonsoft.Json.Formatting.Indented);
        }

        public static Circuit DeserializeFromJson(string json)
        {
            var entity = JObject.Parse(json);
            var circuit = new Circuit();

            // Converting the JSON to a list of WireDTOs
            var dtos = entity["Wires"].ToObject<List<WireDTO>>();
            foreach(var wire in entity["Wires"])
            {
                // Parsing the wire type
                var type = ByName(wire["Type"]?.Value<string>()?.Split(',').First() ?? string.Empty);
                var props = type.GetProperties();

                // Creating an instance of that type
                var instance = Activator.CreateInstance(type);

                // Defining the properties of the instance using Reflection
                foreach (var prop in wire["Data"] as JObject)
                {
                    if(props.Any(p => p.Name == prop.Key))
                    {
                        var propInfo = props.First(p => p.Name == prop.Key);
                        if(propInfo.CanWrite)
                            propInfo.SetValue(instance, prop.Value.ToObject(propInfo.PropertyType));
                    }
                }
                
                // After all that headache, add to the circuit.
                circuit.Wires.Add((Wire) instance);
            }
            return circuit;
        }

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
    }
}
