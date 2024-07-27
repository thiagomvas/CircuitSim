using CircuitSim.Core.Common;
using CircuitSim.Core.Components;

namespace CircuitSim.Tests
{
    public class WireTests
    {
        [Test]
        public void Wire_InSeries_ShouldFlowDirectly()
        {
            // Arrange
            var root = new Wire() { Voltage = 5, Current = 0.1 };
            var wire1 = new Wire();
            var wire2 = new Wire();
            var wire3 = new Wire();
            root.Outputs.Add(wire1);
            wire1.Outputs.Add(wire2);
            wire2.Outputs.Add(wire3);

            // Act
            root.Flow();

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(wire1.Voltage, Is.EqualTo(root.Voltage));
                Assert.That(wire2.Voltage, Is.EqualTo(root.Voltage));
                Assert.That(wire3.Voltage, Is.EqualTo(root.Voltage));
                Assert.That(wire1.Current, Is.EqualTo(root.Current));
                Assert.That(wire2.Current, Is.EqualTo(root.Current));
                Assert.That(wire3.Current, Is.EqualTo(root.Current));
            });
        }


    }
}
