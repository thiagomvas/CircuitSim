﻿using CircuitSim.Core.DTOs;
using Newtonsoft.Json;
using System.Numerics;

namespace CircuitSim.Core.Common
{
    /// <summary>
    /// Represents a wire in a circuit.
    /// </summary>
    public class Wire
    {
        /// <summary>
        /// Gets or sets the pre-flow voltage of the wire.
        /// </summary>
        [JsonIgnore] public double PreFlowVoltage = 0;

        /// <summary>
        /// Gets or sets the pre-flow current of the wire.
        /// </summary>
        [JsonIgnore] public double PreFlowCurrent = 0;

        /// <summary>
        /// Gets the voltage across the wire after flow computation.
        /// </summary>
        [JsonIgnore] public double Voltage { get; private set; } = 0;

        /// <summary>
        /// Gets the current flowing through the wire after flow computation.
        /// </summary>
        [JsonIgnore] public double Current { get; private set; } = 0;

        /// <summary>
        /// Gets or sets the resistance of the wire.
        /// </summary>
        public double Resistance { get; set; } = 0;

        private Vector2 start, end;

        /// <summary>
        /// Gets or sets the start position of the wire.
        /// </summary>
        public Vector2 Start { get => start; set { start = value; UpdateValues(); } }

        /// <summary>
        /// Gets or sets the end position of the wire.
        /// </summary>
        public Vector2 End { get => end; set { end = value; UpdateValues(); } }

        /// <summary>
        /// Gets the center position of the wire.
        /// </summary>
        [JsonIgnore] public Vector2 Center { get; private set; }

        /// <summary>
        /// Gets the direction of the wire.
        /// </summary>
        [JsonIgnore] public Vector2 Direction { get; private set; }

        /// <summary>
        /// Gets the length of the wire.
        /// </summary>
        [JsonIgnore] public float Length { get; private set; }

        /// <summary>
        /// Gets the angle of the wire in degrees.
        /// </summary>
        [JsonIgnore] public float AngleDeg { get; private set; }

        /// <summary>
        /// Gets or sets the list of input wires connected to this wire.
        /// </summary>
        [JsonIgnore] public List<Wire> Inputs { get; set; }

        /// <summary>
        /// Gets or sets the list of output wires connected to this wire.
        /// </summary>
        [JsonIgnore] public List<Wire> Outputs { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Wire"/> class.
        /// </summary>
        public Wire()
        {
            Inputs = new List<Wire>();
            Outputs = new List<Wire>();
        }

        /// <summary>
        /// Adds the specified voltage to the pre-flow voltage of the wire.
        /// </summary>
        /// <param name="v">The voltage to add.</param>
        public void AddVoltage(double v) => PreFlowVoltage += v;

        /// <summary>
        /// Adds the specified current to the pre-flow current of the wire.
        /// </summary>
        /// <param name="a">The current to add.</param>
        public void AddCurrent(double a) => PreFlowCurrent += a;

        /// <summary>
        /// Sets the pre-flow voltage of the wire.
        /// </summary>
        /// <param name="v">The voltage to set.</param>
        public void SetVoltage(double v) => PreFlowVoltage = v;

        /// <summary>
        /// Sets the pre-flow current of the wire.
        /// </summary>
        /// <param name="a">The current to set.</param>
        public void SetCurrent(double a) => PreFlowCurrent = a;

        private void UpdateValues()
        {
            Center = (End + Start) / 2;
            Length = (End - Start).Length();
            Direction = Vector2.Normalize(End - Start);

            var dir = (End - Start);
            AngleDeg = MathF.Atan2(dir.Y, dir.X) * 180 / MathF.PI;
        }

        /// <summary>
        /// Resets the state of the wire.
        /// </summary>
        public void ResetState()
        {

        }

        /// <summary>
        /// Performs the flow operation on the wire.
        /// </summary>
        public virtual void Flow()
        {
            DefaultFlow();
        }

        protected void DefaultFlow(double voltageAddition = 0, double currentAddition = 0)
        {
            Voltage = PreFlowVoltage;
            Current = PreFlowCurrent;
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
                var totalResistance = GetCircuitResistance();
                foreach (var output in Outputs)
                {
                    output.AddVoltage(Voltage + voltageAddition);

                    output.AddCurrent(Current * (totalResistance / output.GetCircuitResistance()) + currentAddition);

                    output.Flow();
                }
            }
        }

        protected double GetCircuitResistance()
        {
            double result = Resistance;

            if (Outputs.Count == 0)
            {
                return result;
            }

            if (Outputs.Count == 1)
            {
                result += Outputs[0].GetCircuitResistance();
            }
            else
            {
                double inverseTotalResistance = 0;
                foreach (var wire in Outputs)
                {
                    var res = wire.GetCircuitResistance();
                    if (res != 0)
                    {
                        inverseTotalResistance += 1 / res;
                    }
                }
                result += (inverseTotalResistance != 0) ? 1 / inverseTotalResistance : 0;
            }

            return result;
        }

        /// <summary>
        /// Returns a string representation of the wire.
        /// </summary>
        /// <returns>A string representation of the wire.</returns>
        public override string ToString()
        {
            return $"{Voltage}V - {Current}A - {Resistance}Ohm";
        }

        internal WireDTO ToDTO()
        {
            return new()
            {
                Type = this.GetType(),
                Data = this
            };
        }
    }
}
