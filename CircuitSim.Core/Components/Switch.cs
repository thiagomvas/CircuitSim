using CircuitSim.Core.Common;

namespace CircuitSim.Core.Components;

public class Switch : Wire
{
    public bool State { get; set; }
    public override void Flow()
    {
        if (State)
            base.Flow();
        else
        {
            Voltage = PreFlowVoltage;
            Current = PreFlowCurrent;
        }
    }

    public override sealed void Interact()
    {
        State = !State;
    }
}
