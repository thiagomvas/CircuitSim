using CircuitSim.Core;
using CircuitSim.Core.Common;
using CircuitSim.Core.Components;
using CircuitSim.Desktop;
using Raylib_cs;
using static Raylib_cs.Raylib;

var json = """
{
  "Wires": [
    {
      "Type": "CircuitSim.Core.Components.VoltageSource, CircuitSim.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
      "Data": {
        "SupplyVoltage": 10.0,
        "Resistance": 0.0,
        "Start": {
          "X": 520.0,
          "Y": 340.0
        },
        "End": {
          "X": 700.0,
          "Y": 340.0
        }
      }
    },
    {
      "Type": "CircuitSim.Core.Common.Wire, CircuitSim.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
      "Data": {
        "Resistance": 0.0,
        "Start": {
          "X": 700.0,
          "Y": 340.0
        },
        "End": {
          "X": 700.0,
          "Y": 260.0
        }
      }
    },
    {
      "Type": "CircuitSim.Core.Common.Wire, CircuitSim.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
      "Data": {
        "Resistance": 0.0,
        "Start": {
          "X": 700.0,
          "Y": 340.0
        },
        "End": {
          "X": 700.0,
          "Y": 420.0
        }
      }
    },
    {
      "Type": "CircuitSim.Core.Components.Resistor, CircuitSim.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
      "Data": {
        "Resistance": 1.0,
        "Start": {
          "X": 700.0,
          "Y": 260.0
        },
        "End": {
          "X": 840.0,
          "Y": 260.0
        }
      }
    },
    {
      "Type": "CircuitSim.Core.Components.Resistor, CircuitSim.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
      "Data": {
        "Resistance": 1.0,
        "Start": {
          "X": 700.0,
          "Y": 420.0
        },
        "End": {
          "X": 840.0,
          "Y": 420.0
        }
      }
    },
    {
      "Type": "CircuitSim.Core.Common.Wire, CircuitSim.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
      "Data": {
        "Resistance": 0.0,
        "Start": {
          "X": 840.0,
          "Y": 420.0
        },
        "End": {
          "X": 840.0,
          "Y": 340.0
        }
      }
    },
    {
      "Type": "CircuitSim.Core.Common.Wire, CircuitSim.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
      "Data": {
        "Resistance": 0.0,
        "Start": {
          "X": 840.0,
          "Y": 260.0
        },
        "End": {
          "X": 840.0,
          "Y": 340.0
        }
      }
    },
    {
      "Type": "CircuitSim.Core.Common.Wire, CircuitSim.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
      "Data": {
        "Resistance": 0.0,
        "Start": {
          "X": 840.0,
          "Y": 340.0
        },
        "End": {
          "X": 920.0,
          "Y": 340.0
        }
      }
    },
    {
      "Type": "CircuitSim.Core.Components.Resistor, CircuitSim.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
      "Data": {
        "Resistance": 1.0,
        "Start": {
          "X": 920.0,
          "Y": 340.0
        },
        "End": {
          "X": 1060.0,
          "Y": 340.0
        }
      }
    }
  ]
}
""";

var manager = SimulationManager.Instance;
manager.UseCircuit(Circuit.DeserializeFromJson(json));
SetConfigFlags(ConfigFlags.ResizableWindow);
SetConfigFlags(ConfigFlags.FullscreenMode);
InitWindow(1920, 1080, "CircuitSim");
SetTargetFPS(60);

while (!WindowShouldClose())
{
    manager.Update();
    BeginDrawing();
    ClearBackground(Constants.BackgroundColor);
    manager.Draw();
    EndDrawing();
}
CloseWindow();