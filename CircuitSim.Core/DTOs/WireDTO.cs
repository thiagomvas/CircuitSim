using CircuitSim.Core.Common;

namespace CircuitSim.Core.DTOs
{
    internal class WireDTO
    {
        public required Type Type { get; set; }
        public required Wire Data { get; set; }
    }
}
