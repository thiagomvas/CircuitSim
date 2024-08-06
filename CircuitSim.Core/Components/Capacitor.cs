using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircuitSim.Core.Common;

namespace CircuitSim.Core.Components
{
    internal class Capacitor : Wire
    {
        public Capacitor(double capacitance)
        {
            Capacitance = capacitance;
            preFlowCurrent = 0;
            PreFlowVoltage = 0;
            Resistance = Resistance;
        }
        public void ApplyVoltage(double PreflowVoltage, double timeStep)
        {
            // Calcula a mudança de tensão usando a fórmula: dV = (I / C) * dt
            Voltage = preFlowCurrent / Capacitance;

            // Calcula a mudança de corrente usando a fórmula: dI = C * dV / dt
            double deltaCurrent = Capacitance * (PreflowVoltage - Voltage) / timeStep;
            preFlowCurrent += deltaCurrent;

            // Atualiza a tensão de pré-fluxo
            PreFlowVoltage = PreflowVoltage;

            // Atualiza a corrente de pré-fluxo
            PreFlowCurrent = preFlowCurrent;

        }
        public void Charge(double current, double timeStep)
        {
            // Calcula a mudança de tensão usando a fórmula: dV = (I / C) * dt
            double deltaVoltage = (current / Capacitance) * timeStep;
            Voltage += deltaVoltage;
        }

        public void Discharge(double timeStep)
        {
            // Calcula a mudança de tensão usando a fórmula: dV = -(V / (R * C)) * dt
            double deltaVoltage = -(Voltage / (Resistance * Capacitance)) * timeStep;
            Voltage += deltaVoltage;
        }
    }
}

