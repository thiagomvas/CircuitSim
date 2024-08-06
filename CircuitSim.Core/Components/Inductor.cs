using CircuitSim.Core.Common;


namespace CircuitSim.Core.Components
{
    internal class Inductor : Wire
    {
        public double DissipatedEnergy { get; private set; } // Energia dissipada em Joules
        public Inductor(double inductance)
        {
            Inductance = inductance;
            preFlowCurrent = 0;
            PreFlowVoltage = 0;
            Resistance = 0;
        }

        public void ApplyVoltage(double PreflowVoltage, double timeStep)
        {
            // Calcula a mudança de corrente usando a fórmula: dI = (V / L) * dt
            double deltaCurrent = (PreflowVoltage / Inductance) * timeStep;
            preFlowCurrent += deltaCurrent;

            // Calcula a mudança de tensão usando a fórmula: dV = L * dI / dt
            Voltage = Inductance * (preFlowCurrent - Voltage) / timeStep;

            // Calcula a energia dissipada no resistor: P = I^2 * R
            double powerDissipated = Math.Pow(preFlowCurrent, 2) * Resistance;

            // Energia dissipada no passo de tempo: E = P * dt
            DissipatedEnergy += powerDissipated * timeStep;
            // Atualiza a tensão de pré-fluxo
            PreFlowVoltage = PreflowVoltage;

            // Atualiza a corrente de pré-fluxo
            PreFlowCurrent = preFlowCurrent;
        }
    }
}
