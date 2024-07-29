using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircuitSim.Core.Common;

namespace CircuitSim.Core.Components
{
    public class bateriaCC : Wire
    {

        public double VoltageOutput { get; set; }
        public double Capacity { get; set; }

        public bool IsActive => Capacity > 0;

        public override void Flow()
        {
            if (IsActive)
            {
                Capacity -= VoltageOutput * 0.1;
                if (Capacity < 0) Capacity = 0;

                Voltage = VoltageOutput;

                base.Flow();
            }
        }
    }
}

