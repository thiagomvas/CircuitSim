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
            var root = new VoltageSource() { SupplyVoltage = 1 };
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
            });
        }

        [Test]
        public void Flow_InParallel_ShouldHaveCorrectValues()
        {
            // Arrange
            var root = new VoltageSource() { SupplyVoltage = 5 };
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
                Assert.That(root.Voltage, Is.EqualTo(5), "Root did not supply correct voltage");
                Assert.That(root.preFlowCurrent, Is.EqualTo(1), "Root did not supply correct current");
                Assert.That(wire1.Voltage, Is.EqualTo(5), "Top wire before first resistor did not get proper voltage");
                Assert.That(wire1.preFlowCurrent, Is.EqualTo(0.5), "Top wire before first resistor did not get proper current");
                Assert.That(resistor.Voltage, Is.EqualTo(5), "Top resistor did not get proper voltage");
                Assert.That(resistor.preFlowCurrent, Is.EqualTo(0.5), "Top resistor did not get proper current");
                Assert.That(wire2.Voltage, Is.EqualTo(0), "Top wire after resistor has voltage");
                Assert.That(wire2.preFlowCurrent, Is.EqualTo(0.5), "Top wire after resistor did not get proper current");
                Assert.That(wire3.Voltage, Is.EqualTo(5), "Bottom wire before resistor did not get proper voltage");
                Assert.That(wire3.preFlowCurrent, Is.EqualTo(0.5), "Bottom wire before resistor did not get proper current");
                Assert.That(resistor2.Voltage, Is.EqualTo(5), "Bottom resistor did not get proper voltage");
                Assert.That(resistor2.preFlowCurrent, Is.EqualTo(0.5), "Bottom resistor did not get proper current");
                Assert.That(wire4.Voltage, Is.EqualTo(0), "Bottom wire after resistor has voltage");
                Assert.That(wire4.preFlowCurrent, Is.EqualTo(0.5), "Bottom wire after resistor did not get proper current");
            });
        }
    }
}
