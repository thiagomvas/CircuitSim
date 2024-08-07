using Raylib_cs;

namespace CircuitSim.Desktop.Input;

public class Keymap
{
    public string Name { get; set; }
    public KeyboardKey Key { get; set; }
    public Action Action { get; set; }

    public Keymap(string name, KeyboardKey key, Action action)
    {
        Key = key;
        Name = name;
        Action = action;
    }
}
