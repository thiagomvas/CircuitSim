using CircuitSim.Core.Common;

namespace CircuitSim.Core.Components;

/// <summary>
/// Represents a resistor component in a circuit.
/// </summary>
public class Resistor : Wire
{
    public Resistor()
    {
        Resistance = 1000;
    }
    /// <inheritdoc/>
    public override void Flow()
    {
        Voltage = PreFlowVoltage;

        if (Inputs.Count > 1)
            Current = Inputs.Sum(w => w.Current); // If multiple inputs, use the sum regardless
        else
            Current = PreFlowCurrent;

        double voltageAddition = -Current * Resistance;
        double currentAddition = 0;

        PreFlowVoltage = 0;
        PreFlowCurrent = 0;
        Voltage = Math.Max(0, Voltage);
        Current = Math.Max(0, Current);

        if (Outputs.Count == 1)
        {
            var @out = Outputs[0];

            @out.AddVoltage(Voltage + voltageAddition);
            @out.AddCurrent(Current + currentAddition);

            @out.Flow();
        }
        else
        {
            double totalResistance = GetCircuitResistance();
            if (totalResistance <= 0)
            {
                // Handle case where total resistance is zero or negative which might occur in ideal cases
                foreach (var output in Outputs)
                {
                    output.AddVoltage(Voltage + voltageAddition);
                    output.AddCurrent(Current / Outputs.Count + currentAddition);
                    output.Flow();
                }
            }
            else
            {
                // Calculate current division based on individual resistances
                double totalInverseResistance = 0;
                foreach (var output in Outputs)
                {
                    var outputResistance = output.GetCircuitResistance();
                    if (outputResistance > 0)
                    {
                        totalInverseResistance += 1 / outputResistance;
                    }
                }

                foreach (var output in Outputs)
                {
                    double outputResistance = output.GetCircuitResistance();
                    double currentFraction = 0;
                    if (outputResistance > 0 && totalInverseResistance > 0)
                    {
                        currentFraction = (1 / outputResistance) / totalInverseResistance;
                    }
                    output.AddVoltage(Voltage + voltageAddition);
                    output.AddCurrent(Current * currentFraction + currentAddition);
                    output.Flow();
                }
            }
        }
    }

}
