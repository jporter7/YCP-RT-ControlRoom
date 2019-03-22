using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Simulators.Hardware.AbsoluteEncoder;

namespace ControlRoomApplicationTest.SimulatorTests
{
    [TestClass]
    public class SimulationAbsoluteEncoderTests
    {
        private static SimulationAbsoluteEncoder Encoder0 = null;
        private static SimulationAbsoluteEncoder Encoder1 = null;
        private static SimulationAbsoluteEncoder Encoder2 = null;

        // Because the order of execution of tests is not guaranteed, have this method called whenever the values are changed
        private static void Reset()
        {
            Encoder0 = new SimulationAbsoluteEncoder(10);
            Encoder1 = new SimulationAbsoluteEncoder(12, 47.5);
            Encoder2 = new SimulationAbsoluteEncoder(16, 185.12);
        }

        [ClassInitialize]
        public static void BuildUp(TestContext testContext)
        {
            Reset();
        }

        [TestMethod]
        public void TestConstructorsAndProperties()
        {
            Assert.AreEqual(10, Encoder0.BitsOfPrecision);
            Assert.AreEqual(0, Encoder0.ErrorStandardDeviation);
            Assert.AreEqual(0, Encoder0.CurrentPositionTicks);
            Assert.AreEqual(0, Encoder0.CurrentPositionDegrees, 360.0 / Encoder0.NumberOfEncoderTickPositions);

            Assert.AreEqual(12, Encoder1.BitsOfPrecision);
            Assert.AreEqual(0, Encoder1.ErrorStandardDeviation);
            Assert.AreEqual(540, Encoder1.CurrentPositionTicks);
            Assert.AreEqual(47.5, Encoder1.CurrentPositionDegrees, 360.0 / Encoder0.NumberOfEncoderTickPositions);

            Assert.AreEqual(16, Encoder2.BitsOfPrecision);
            Assert.AreEqual(0, Encoder2.ErrorStandardDeviation);
            Assert.AreEqual(33700, Encoder2.CurrentPositionTicks);
            Assert.AreEqual(185.12, Encoder2.CurrentPositionDegrees, 360.0 / Encoder0.NumberOfEncoderTickPositions);
        }

        [TestMethod]
        public void TestEncoderTicksToDegreesCalculationGetter()
        {
            Assert.AreEqual(0, Encoder0.GetEquivalentEncoderTicksFromDegrees(0.0));
            Assert.AreEqual(256, Encoder0.GetEquivalentEncoderTicksFromDegrees(90.0));
            Assert.AreEqual(384, Encoder0.GetEquivalentEncoderTicksFromDegrees(135.2));
            Assert.AreEqual(1023, Encoder0.GetEquivalentEncoderTicksFromDegrees(359.9));
            Assert.AreEqual(0, Encoder0.GetEquivalentEncoderTicksFromDegrees(360.0));
            Assert.AreEqual(275, Encoder0.GetEquivalentEncoderTicksFromDegrees(456.7));
            Assert.AreEqual(965, Encoder0.GetEquivalentEncoderTicksFromDegrees(-20.58));
            Assert.AreEqual(768, Encoder0.GetEquivalentEncoderTicksFromDegrees(-450.0));

            Assert.AreEqual(0, Encoder1.GetEquivalentEncoderTicksFromDegrees(0.0));
            Assert.AreEqual(1024, Encoder1.GetEquivalentEncoderTicksFromDegrees(90.0));
            Assert.AreEqual(1538, Encoder1.GetEquivalentEncoderTicksFromDegrees(135.2));
            Assert.AreEqual(4094, Encoder1.GetEquivalentEncoderTicksFromDegrees(359.9));
            Assert.AreEqual(0, Encoder1.GetEquivalentEncoderTicksFromDegrees(360.0));
            Assert.AreEqual(1100, Encoder1.GetEquivalentEncoderTicksFromDegrees(456.7));
            Assert.AreEqual(3861, Encoder1.GetEquivalentEncoderTicksFromDegrees(-20.58));
            Assert.AreEqual(3072, Encoder1.GetEquivalentEncoderTicksFromDegrees(-450.0));

            Assert.AreEqual(0, Encoder2.GetEquivalentEncoderTicksFromDegrees(0.0));
            Assert.AreEqual(16384, Encoder2.GetEquivalentEncoderTicksFromDegrees(90.0));
            Assert.AreEqual(24612, Encoder2.GetEquivalentEncoderTicksFromDegrees(135.2));
            Assert.AreEqual(65517, Encoder2.GetEquivalentEncoderTicksFromDegrees(359.9));
            Assert.AreEqual(0, Encoder2.GetEquivalentEncoderTicksFromDegrees(360.0));
            Assert.AreEqual(17603, Encoder2.GetEquivalentEncoderTicksFromDegrees(456.7));
            Assert.AreEqual(61789, Encoder2.GetEquivalentEncoderTicksFromDegrees(-20.58));
            Assert.AreEqual(49152, Encoder2.GetEquivalentEncoderTicksFromDegrees(-450.0));
        }

        [TestMethod]
        public void TestDegreesToEncoderTicksCalculationGetter()
        {
            Assert.AreEqual(0.0, Encoder0.GetEquivalentDegreesFromEncoderTicks(0), 360.0 / Encoder0.NumberOfEncoderTickPositions);
            Assert.AreEqual(90.0, Encoder0.GetEquivalentDegreesFromEncoderTicks(256), 360.0 / Encoder0.NumberOfEncoderTickPositions);
            Assert.AreEqual(135.2, Encoder0.GetEquivalentDegreesFromEncoderTicks(384), 360.0 / Encoder0.NumberOfEncoderTickPositions);
            Assert.AreEqual(359.9, Encoder0.GetEquivalentDegreesFromEncoderTicks(1023), 360.0 / Encoder0.NumberOfEncoderTickPositions);
            Assert.AreEqual(0.0, Encoder0.GetEquivalentDegreesFromEncoderTicks(0), 360.0 / Encoder0.NumberOfEncoderTickPositions);
            Assert.AreEqual(96.7, Encoder0.GetEquivalentDegreesFromEncoderTicks(275), 360.0 / Encoder0.NumberOfEncoderTickPositions);
            Assert.AreEqual(339.42, Encoder0.GetEquivalentDegreesFromEncoderTicks(965), 360.0 / Encoder0.NumberOfEncoderTickPositions);
            Assert.AreEqual(270.0, Encoder0.GetEquivalentDegreesFromEncoderTicks(768), 360.0 / Encoder0.NumberOfEncoderTickPositions);

            Assert.AreEqual(0.0, Encoder1.GetEquivalentDegreesFromEncoderTicks(0), 360.0 / Encoder1.NumberOfEncoderTickPositions);
            Assert.AreEqual(90.0, Encoder1.GetEquivalentDegreesFromEncoderTicks(1024), 360.0 / Encoder1.NumberOfEncoderTickPositions);
            Assert.AreEqual(135.2, Encoder1.GetEquivalentDegreesFromEncoderTicks(1538), 360.0 / Encoder1.NumberOfEncoderTickPositions);
            Assert.AreEqual(359.9, Encoder1.GetEquivalentDegreesFromEncoderTicks(4094), 360.0 / Encoder1.NumberOfEncoderTickPositions);
            Assert.AreEqual(0.0, Encoder1.GetEquivalentDegreesFromEncoderTicks(0), 360.0 / Encoder1.NumberOfEncoderTickPositions);
            Assert.AreEqual(96.7, Encoder1.GetEquivalentDegreesFromEncoderTicks(1100), 360.0 / Encoder1.NumberOfEncoderTickPositions);
            Assert.AreEqual(339.42, Encoder1.GetEquivalentDegreesFromEncoderTicks(3861), 360.0 / Encoder1.NumberOfEncoderTickPositions);
            Assert.AreEqual(270.0, Encoder1.GetEquivalentDegreesFromEncoderTicks(3072), 360.0 / Encoder1.NumberOfEncoderTickPositions);

            Assert.AreEqual(0.0, Encoder2.GetEquivalentDegreesFromEncoderTicks(0), 360.0 / Encoder2.NumberOfEncoderTickPositions);
            Assert.AreEqual(90.0, Encoder2.GetEquivalentDegreesFromEncoderTicks(16384), 360.0 / Encoder2.NumberOfEncoderTickPositions);
            Assert.AreEqual(135.2, Encoder2.GetEquivalentDegreesFromEncoderTicks(24612), 360.0 / Encoder2.NumberOfEncoderTickPositions);
            Assert.AreEqual(359.9, Encoder2.GetEquivalentDegreesFromEncoderTicks(65517), 360.0 / Encoder2.NumberOfEncoderTickPositions);
            Assert.AreEqual(0.0, Encoder2.GetEquivalentDegreesFromEncoderTicks(0), 360.0 / Encoder2.NumberOfEncoderTickPositions);
            Assert.AreEqual(96.7, Encoder2.GetEquivalentDegreesFromEncoderTicks(17603), 360.0 / Encoder2.NumberOfEncoderTickPositions);
            Assert.AreEqual(339.42, Encoder2.GetEquivalentDegreesFromEncoderTicks(61789), 360.0 / Encoder2.NumberOfEncoderTickPositions);
            Assert.AreEqual(270.0, Encoder2.GetEquivalentDegreesFromEncoderTicks(49152), 360.0 / Encoder2.NumberOfEncoderTickPositions);
        }

        [TestMethod]
        public void TestGoodSetters()
        {
            Assert.AreEqual(0, Encoder0.CurrentPositionTicks);
            Encoder0.SetPositionFromEncoderTicks(750);
            Assert.AreEqual(750, Encoder0.CurrentPositionTicks);
            Encoder0.SetPositionFromDegrees(45.0);
            Assert.AreEqual(128, Encoder0.CurrentPositionTicks);
            Encoder0.SetPositionFromDegrees(405.0);
            Assert.AreEqual(128, Encoder0.CurrentPositionTicks);
            Encoder0.SetPositionFromDegrees(-315.0);
            Assert.AreEqual(128, Encoder0.CurrentPositionTicks);

            Assert.AreEqual(540, Encoder1.CurrentPositionTicks);
            Encoder1.SetPositionFromEncoderTicks(1024);
            Assert.AreEqual(1024, Encoder1.CurrentPositionTicks);
            Encoder1.SetPositionFromDegrees(36.0);
            Assert.AreEqual(409, Encoder1.CurrentPositionTicks);
            Encoder1.SetPositionFromDegrees(370.0);
            Assert.AreEqual(113, Encoder1.CurrentPositionTicks);
            Encoder1.SetPositionFromDegrees(-10.0);
            Assert.AreEqual(3982, Encoder1.CurrentPositionTicks);

            Reset();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestBadSetterUnderflow()
        {
            Encoder2.SetPositionFromEncoderTicks(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestBadSetterOverflow()
        {
            Encoder1.SetPositionFromEncoderTicks(Encoder1.NumberOfEncoderTickPositions);
        }

        [TestMethod]
        public void TestTranslationSetters()
        {
            Encoder2.SetPositionFromEncoderTicks(0);
            Assert.AreEqual(0, Encoder2.CurrentPositionTicks);
            Encoder2.TranslateEncoderTicks(-1);
            Assert.AreEqual(Encoder2.NumberOfEncoderTickPositions - 1, Encoder2.CurrentPositionTicks);
            Encoder2.TranslateEncoderTicks(1);
            Assert.AreEqual(0, Encoder2.CurrentPositionTicks);
            Encoder2.TranslateEncoderTicks(Encoder2.NumberOfEncoderTickPositions);
            Assert.AreEqual(0, Encoder2.CurrentPositionTicks);

            Encoder2.TranslateDegrees(180.0);
            Assert.AreEqual(32768, Encoder2.CurrentPositionTicks);
            Encoder2.TranslateDegrees(180.0);
            Assert.AreEqual(0, Encoder2.CurrentPositionTicks);
            Encoder2.TranslateDegrees(-77.7);
            Assert.AreEqual(51391, Encoder2.CurrentPositionTicks);
            Encoder2.TranslateDegrees(360.0);
            Assert.AreEqual(51391, Encoder2.CurrentPositionTicks);

            Reset();
        }
    }
}
