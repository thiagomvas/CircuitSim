using CircuitSim.Core.Common;
using CircuitSim.Core.Components;

namespace CircuitSim.Tests
{
    public class WireTests
    {

        [Test]
        public void Flow_InSeries_ShouldFlowDirectly()
        {
            // Arrange
            var root = new Wire() { Voltage = 5, Current = 0.1 };
            var wire1 = new Wire();
            var wire2 = new Wire();
            var wire3 = new Wire();
            root.Outputs.Add(wire1);
            wire1.Outputs.Add(wire2);
            wire2.Outputs.Add(wire3);

            // fonte -> cabo1 -> cabo2 -> cabo3

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

        [Test]
        public void Flow_InParallel_ShouldHaveCorrectValues()
        {
            // Arrange
            var root = new VoltageSource() { Voltage = 5 };
            var resistor = new Resistor() { Resistance = 10 };
            var resistor2 = new Resistor() { Resistance = 10 };
            var wire1 = new Wire();
            var wire2 = new Wire();
            var wire3 = new Wire();
            var wire4 = new Wire();
            root.Outputs.Add(wire1);
            wire1.Outputs.Add(resistor);
            resistor.Outputs.Add(wire2);

            root.Outputs.Add(wire3);
            wire3.Outputs.Add(resistor2);
            resistor2.Outputs.Add(wire4);

            /*
             * fonte -> cabo1 -> resistor  -> cabo2
             *       -> cabo3 -> resistor2 -> cabo4
             */

            // Act
            root.Flow();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(root.Voltage, Is.EqualTo(5));
                Assert.That(root.Current, Is.EqualTo(1));
                Assert.That(wire1.Voltage, Is.EqualTo(5));
                Assert.That(wire1.Current, Is.EqualTo(0.5));
                Assert.That(resistor.Voltage, Is.EqualTo(5));
                Assert.That(resistor.Current, Is.EqualTo(0.5));
                Assert.That(wire2.Voltage, Is.EqualTo(0));
                Assert.That(wire2.Current, Is.EqualTo(0.5));
                Assert.That(wire3.Voltage, Is.EqualTo(5));
                Assert.That(wire3.Current, Is.EqualTo(0.5));
                Assert.That(resistor2.Voltage, Is.EqualTo(5));
                Assert.That(resistor2.Current, Is.EqualTo(0.5));
                Assert.That(wire4.Voltage, Is.EqualTo(0));
                Assert.That(wire4.Current, Is.EqualTo(0.5));
            });

        }


    }
}
