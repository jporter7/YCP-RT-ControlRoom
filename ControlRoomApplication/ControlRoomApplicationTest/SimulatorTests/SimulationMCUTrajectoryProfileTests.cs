using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Simulators.Hardware.AbsoluteEncoder;
using ControlRoomApplication.Simulators.Hardware.MCU;

namespace ControlRoomApplicationTest.SimulatorTests
{
    [TestClass]
    public class SimulationMCUTrajectoryProfileTests
    {
        private static double DOUBLE_EPSILON = 0.000000001;

        private static SimulationAbsoluteEncoder Encoder;

        private static SimulationMCUTrajectoryProfile ProfileNegligible;
        private static SimulationMCUTrajectoryProfile ProfileSCurveTriangularPartial;
        private static SimulationMCUTrajectoryProfile ProfileSCurveTriangularFull;
        private static SimulationMCUTrajectoryProfile ProfileSCurveTrapezoidal;

        [ClassInitialize]
        public static void BuildUp(TestContext testContext)
        {
            Encoder = new SimulationAbsoluteEncoder(12);

            ProfileNegligible = SimulationMCUTrajectoryProfile.CalculateInstance(
                Encoder,
                0.0,
                0.0,
                HardwareConstants.SIMULATION_MCU_PEAK_VELOCITY,
                HardwareConstants.SIMULATION_MCU_PEAK_ACCELERATION,
                HardwareConstants.NEGLIGIBLE_POSITION_CHANGE_DEGREES / 2
            );

            ProfileSCurveTriangularFull = SimulationMCUTrajectoryProfile.CalculateInstance(
                Encoder,
                0.0,
                0.0,
                HardwareConstants.SIMULATION_MCU_PEAK_VELOCITY,
                HardwareConstants.SIMULATION_MCU_PEAK_ACCELERATION,
                1.85
            );

            ProfileSCurveTriangularPartial = SimulationMCUTrajectoryProfile.CalculateInstance(
                Encoder,
                60.0,
                0.0,
                HardwareConstants.SIMULATION_MCU_PEAK_VELOCITY,
                HardwareConstants.SIMULATION_MCU_PEAK_ACCELERATION,
                60.0 + (HardwareConstants.NEGLIGIBLE_POSITION_CHANGE_DEGREES * 2)
            );

            ProfileSCurveTrapezoidal = SimulationMCUTrajectoryProfile.CalculateInstance(
                Encoder,
                0.0,
                0.0,
                HardwareConstants.SIMULATION_MCU_PEAK_VELOCITY,
                HardwareConstants.SIMULATION_MCU_PEAK_ACCELERATION,
                HardwareConstants.NEGLIGIBLE_POSITION_CHANGE_DEGREES / 2
            );
        }

        [TestMethod]
        public void TestNegligibleProfileConstructorAndProperties()
        {
            Assert.AreEqual(SimulationMCUTrajectoryProfileTypeEnum.NEGLIGIBLE, ProfileNegligible.ProfileType);
            Assert.AreEqual(0.0, ProfileNegligible.InitialVelocity, DOUBLE_EPSILON);
            Assert.AreEqual(HardwareConstants.SIMULATION_MCU_PEAK_VELOCITY, ProfileNegligible.TrajectoryPeakVelocity, DOUBLE_EPSILON);
            Assert.AreEqual(HardwareConstants.SIMULATION_MCU_PEAK_ACCELERATION, ProfileNegligible.TrajectoryPeakAcceleration, DOUBLE_EPSILON);
            Assert.AreEqual(0.0, ProfileNegligible.TotalTime, DOUBLE_EPSILON);
            Assert.AreEqual(Encoder.GetEquivalentEncoderTicksFromDegrees(0.0), ProfileNegligible.InitialStep);
            Assert.AreEqual(Encoder.GetEquivalentEncoderTicksFromDegrees(HardwareConstants.NEGLIGIBLE_POSITION_CHANGE_DEGREES / 2), ProfileNegligible.ObjectiveStep);
        }

        [TestMethod]
        public void TestNegligibleProfileGoodInterpretation()
        {
            DateTime StartTime = DateTime.Now;

            Assert.AreEqual(ProfileNegligible.ObjectiveStep, ProfileNegligible.InterpretAt(StartTime, StartTime.AddSeconds(10)));
            Assert.AreEqual(ProfileNegligible.ObjectiveStep, ProfileNegligible.InterpretAt(StartTime, StartTime.AddMilliseconds(1)));
            Assert.AreEqual(ProfileNegligible.ObjectiveStep, ProfileNegligible.InterpretAt(StartTime, StartTime.AddMinutes(2)));
            Assert.AreEqual(ProfileNegligible.ObjectiveStep, ProfileNegligible.InterpretAt(StartTime, StartTime.AddYears(10)));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestNegligibleProfileBadInterpretation()
        {
            DateTime StartTime = DateTime.Now;
            ProfileNegligible.InterpretAt(StartTime, StartTime.AddMilliseconds(-100));
        }

        [TestMethod]
        public void TestFullTriangularSCurveProfileConstructorAndProperties()
        {
            Assert.AreEqual(SimulationMCUTrajectoryProfileTypeEnum.S_CURVE_TRIANGULAR_FULL, ProfileSCurveTriangularFull.ProfileType);
            Assert.AreEqual(0.0, ProfileSCurveTriangularFull.InitialVelocity, DOUBLE_EPSILON);
            Assert.AreEqual(HardwareConstants.SIMULATION_MCU_PEAK_VELOCITY, ProfileSCurveTriangularFull.TrajectoryPeakVelocity, DOUBLE_EPSILON);
            Assert.AreEqual(HardwareConstants.SIMULATION_MCU_PEAK_ACCELERATION, ProfileSCurveTriangularFull.TrajectoryPeakAcceleration, DOUBLE_EPSILON);
            Assert.AreEqual(1.875, ProfileSCurveTriangularFull.TotalTime, DOUBLE_EPSILON);
            Assert.AreEqual(Encoder.GetEquivalentEncoderTicksFromDegrees(0.0), ProfileSCurveTriangularFull.InitialStep);
            Assert.AreEqual(Encoder.GetEquivalentEncoderTicksFromDegrees(1.85), ProfileSCurveTriangularFull.ObjectiveStep);
        }

        [TestMethod]
        public void TestFullTriangularSCurveProfileGoodInterpretation()
        {
            DateTime StartTime = DateTime.Now;
            double initialPosition = Encoder.GetEquivalentEncoderTicksFromDegrees(00.0);

            
        }

        //[TestMethod]
        //public void TestPartialTriangularSCurveProfileConstructorAndProperties()
        //{
        //    Assert.AreEqual(SimulationMCUTrajectoryProfileTypeEnum.S_CURVE_TRIANGULAR_PARTIAL, ProfileSCurveTriangularPartial.ProfileType);
        //    Assert.AreEqual(0.0, ProfileSCurveTriangularPartial.InitialVelocity, DOUBLE_EPSILON);
        //    Assert.AreEqual(HardwareConstants.SIMULATION_MCU_PEAK_VELOCITY / Math.Sqrt(2), ProfileSCurveTriangularPartial.TrajectoryPeakVelocity, DOUBLE_EPSILON);
        //    Assert.AreEqual(HardwareConstants.SIMULATION_MCU_PEAK_ACCELERATION, ProfileSCurveTriangularPartial.TrajectoryPeakAcceleration, DOUBLE_EPSILON);
        //    Assert.AreEqual(0.3125, ProfileSCurveTriangularPartial.TotalTime, DOUBLE_EPSILON);
        //    Assert.AreEqual(Encoder.GetEquivalentEncoderTicksFromDegrees(60.0), ProfileSCurveTriangularPartial.InitialStep);
        //    Assert.AreEqual(Encoder.GetEquivalentEncoderTicksFromDegrees(60.0 + (HardwareConstants.NEGLIGIBLE_POSITION_CHANGE_DEGREES * 2)), ProfileSCurveTriangularPartial.ObjectiveStep);
        //}

        //[TestMethod]
        //public void TestPartialTriangularSCurveProfileGoodInterpretation()
        //{
        //    DateTime StartTime = DateTime.Now;
        //    double initialPosition = Encoder.GetEquivalentEncoderTicksFromDegrees(60.0);

        //    //Assert.AreEqual(initialPosition, ProfileSCurveTriangularPartial.InterpretAt(StartTime, StartTime));
        //    //Assert.AreEqual(0, ProfileSCurveTriangularPartial.InterpretAt(StartTime, StartTime.AddMilliseconds(100)));
        //    //Assert.AreEqual(0, ProfileSCurveTriangularPartial.InterpretAt(StartTime, StartTime.AddMilliseconds(300)));
        //    //Assert.AreEqual(0, ProfileSCurveTriangularPartial.InterpretAt(StartTime, StartTime.AddMilliseconds(600)));
        //    //Assert.AreEqual(0, ProfileSCurveTriangularPartial.InterpretAt(StartTime, StartTime.AddMilliseconds(930)));
        //    //Assert.AreEqual(0, ProfileSCurveTriangularPartial.InterpretAt(StartTime, StartTime.AddMilliseconds(1200)));
        //}

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestPartialTriangularSCurveProfileBadInterpretation()
        {
            DateTime StartTime = DateTime.Now;
            ProfileSCurveTriangularPartial.InterpretAt(StartTime, StartTime.AddSeconds(-1));
        }
    }
}
