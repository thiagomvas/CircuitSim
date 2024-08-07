using CircuitSim.Core.Common;

namespace CircuitSim.Core.Components;

public sealed class Ground : Wire
{
    public sealed override void Flow()
    {
        DefaultFlow(-PreFlowVoltage, -PreFlowCurrent);
    }
}
