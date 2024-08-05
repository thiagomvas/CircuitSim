using CircuitSim.Core;
using CircuitSim.Core.Common;
using CircuitSim.Core.Components;
using Raylib_cs;

namespace CircuitSim.Desktop.Input
{
    public class InputSystem
    {
        public readonly Dictionary<KeyboardKey, Keymap> Keymappings = new()
        {
            { KeyboardKey.W, new(nameof(Wire), KeyboardKey.W, () => SimulationManager.Instance.WireType = typeof(Wire)) },
            { KeyboardKey.A, new(nameof(Ohmeter), KeyboardKey.A, () => SimulationManager.Instance.WireType = typeof(Ammeter)) },
            { KeyboardKey.G, new(nameof(Ground), KeyboardKey.G, () => SimulationManager.Instance.WireType = typeof(Ground)) },
            { KeyboardKey.L, new(nameof(LED), KeyboardKey.L, () => SimulationManager.Instance.WireType = typeof(LED)) },
            { KeyboardKey.O, new(nameof(Ohmeter), KeyboardKey.O, () => SimulationManager.Instance.WireType = typeof(Ohmeter)) },
            { KeyboardKey.R, new(nameof(Resistor), KeyboardKey.R, () => SimulationManager.Instance.WireType = typeof(Resistor)) },
            { KeyboardKey.V, new(nameof(Voltmeter), KeyboardKey.V, () => SimulationManager.Instance.WireType = typeof(Voltmeter)) },
            { KeyboardKey.S, new(nameof(VoltageSource), KeyboardKey.S, () => SimulationManager.Instance.WireType = typeof(VoltageSource)) },
            { KeyboardKey.Delete, new("Delete Selection", KeyboardKey.Delete, SimulationManager.Instance.DeleteHovered) },
            { KeyboardKey.F, new("Begin Flow", KeyboardKey.F, SimulationManager.Instance.BeginFlow) },
            { KeyboardKey.Tab, new("Toggle Controls", KeyboardKey.Tab, () => SimulationManager.Instance.ShowControls = !SimulationManager.Instance.ShowControls) },
        };


        public void CheckForInput(KeyboardKey key)
        {
            if (Keymappings.TryGetValue(key, out Keymap map))
                map.Action?.Invoke();
        }
    }
}
