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
        private static double DOUBLE_EPSILON = 0.000001;

        private static SimulationAbsoluteEncoder Encoder;

        private static SimulationMCUTrajectoryProfile ProfileNegligible;
        private static SimulationMCUTrajectoryProfile ProfileSCurveTriangularPartial;
        private static SimulationMCUTrajectoryProfile ProfileSCurveTriangularFull;
        private static SimulationMCUTrajectoryProfile ProfileSCurveTrapezoidal;
        private static SimulationMCUTrajectoryProfile ProfileLinearFull;
        private static SimulationMCUTrajectoryProfile ProfileLinearPartial;

        [ClassInitialize]
        public static void BuildUp(TestContext testContext)
        {
            Encoder = new SimulationAbsoluteEncoder(12);

            ProfileNegligible = SimulationMCUTrajectoryProfile.ForceLinearInstance(
                Encoder,
                0.0,
                0.0,
                MCUConstants.SIMULATION_MCU_PEAK_VELOCITY,
                MCUConstants.SIMULATION_MCU_PEAK_ACCELERATION,
                MiscellaneousConstants.NEGLIGIBLE_POSITION_CHANGE_DEGREES / 2
            );

            ProfileSCurveTriangularFull = SimulationMCUTrajectoryProfile.ForceLinearInstance(
                Encoder,
                0.0,
                0.0,
                MCUConstants.SIMULATION_MCU_PEAK_VELOCITY,
                MCUConstants.SIMULATION_MCU_PEAK_ACCELERATION,
                1.85
            );

            ProfileSCurveTriangularPartial = SimulationMCUTrajectoryProfile.ForceLinearInstance(
                Encoder,
                60.0,
                0.0,
                MCUConstants.SIMULATION_MCU_PEAK_VELOCITY,
                MCUConstants.SIMULATION_MCU_PEAK_ACCELERATION,
                60.0 + (MiscellaneousConstants.NEGLIGIBLE_POSITION_CHANGE_DEGREES * 2)
            );

            ProfileSCurveTrapezoidal = SimulationMCUTrajectoryProfile.ForceLinearInstance(
                Encoder,
                0.0,
                0.0,
                MCUConstants.SIMULATION_MCU_PEAK_VELOCITY,
                MCUConstants.SIMULATION_MCU_PEAK_ACCELERATION,
                MiscellaneousConstants.NEGLIGIBLE_POSITION_CHANGE_DEGREES / 2
            );

            ProfileLinearFull = SimulationMCUTrajectoryProfile.ForceLinearInstance(
                Encoder,
                0.0,
                0.0,
                HardwareConstants.SIMULATION_MCU_PEAK_VELOCITY,
                HardwareConstants.SIMULATION_MCU_PEAK_ACCELERATION,
                Encoder.GetEquivalentDegreesFromEncoderTicks(20)
            );

            ProfileLinearPartial = SimulationMCUTrajectoryProfile.ForceLinearInstance(
                Encoder,
                0.0,
                0.0,
                HardwareConstants.SIMULATION_MCU_PEAK_VELOCITY,
                HardwareConstants.SIMULATION_MCU_PEAK_ACCELERATION,
                Encoder.GetEquivalentDegreesFromEncoderTicks(10)
            );
        }

        [TestMethod]
        public void TestNegligibleProfileConstructorAndProperties()
        {
            Assert.AreEqual(SimulationMCUTrajectoryProfileTypeEnum.NEGLIGIBLE, ProfileNegligible.ProfileType);
            Assert.AreEqual(0.0, ProfileNegligible.InitialVelocity, DOUBLE_EPSILON);
            Assert.AreEqual(MCUConstants.SIMULATION_MCU_PEAK_VELOCITY, ProfileNegligible.TrajectoryPeakVelocity, DOUBLE_EPSILON);
            Assert.AreEqual(MCUConstants.SIMULATION_MCU_PEAK_ACCELERATION, ProfileNegligible.TrajectoryPeakAcceleration, DOUBLE_EPSILON);
            Assert.AreEqual(0.0, ProfileNegligible.TotalTime, DOUBLE_EPSILON);
            Assert.AreEqual(Encoder.GetEquivalentEncoderTicksFromDegrees(0.0), ProfileNegligible.InitialStep);
            Assert.AreEqual(Encoder.GetEquivalentEncoderTicksFromDegrees(MiscellaneousConstants.NEGLIGIBLE_POSITION_CHANGE_DEGREES / 2), ProfileNegligible.ObjectiveStep);
        }

        [TestMethod]
        public void TestNegligibleProfileGoodInterpretation()
        {
            DateTime StartTime = DateTime.UtcNow;

            Assert.AreEqual(ProfileNegligible.ObjectiveStep, ProfileNegligible.InterpretEncoderTicksAt(StartTime, StartTime.AddSeconds(10)));
            Assert.AreEqual(ProfileNegligible.ObjectiveStep, ProfileNegligible.InterpretEncoderTicksAt(StartTime, StartTime.AddMilliseconds(1)));
            Assert.AreEqual(ProfileNegligible.ObjectiveStep, ProfileNegligible.InterpretEncoderTicksAt(StartTime, StartTime.AddMinutes(2)));
            Assert.AreEqual(ProfileNegligible.ObjectiveStep, ProfileNegligible.InterpretEncoderTicksAt(StartTime, StartTime.AddYears(10)));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestNegligibleProfileBadInterpretation()
        {
            DateTime StartTime = DateTime.UtcNow;
            ProfileNegligible.InterpretEncoderTicksAt(StartTime, StartTime.AddMilliseconds(-100));
        }

        //[TestMethod]
        //public void TestFullTriangularSCurveProfileConstructorAndProperties()
        //{
        //    Assert.AreEqual(SimulationMCUTrajectoryProfileTypeEnum.S_CURVE_TRIANGULAR_FULL, ProfileSCurveTriangularFull.ProfileType);
        //    Assert.AreEqual(0.0, ProfileSCurveTriangularFull.InitialVelocity, DOUBLE_EPSILON);
        //    Assert.AreEqual(HardwareConstants.SIMULATION_MCU_PEAK_VELOCITY, ProfileSCurveTriangularFull.TrajectoryPeakVelocity, DOUBLE_EPSILON);
        //    Assert.AreEqual(HardwareConstants.SIMULATION_MCU_PEAK_ACCELERATION, ProfileSCurveTriangularFull.TrajectoryPeakAcceleration, DOUBLE_EPSILON);
        //    Assert.AreEqual(1.875, ProfileSCurveTriangularFull.TotalTime, DOUBLE_EPSILON);
        //    Assert.AreEqual(Encoder.GetEquivalentEncoderTicksFromDegrees(0.0), ProfileSCurveTriangularFull.InitialStep);
        //    Assert.AreEqual(Encoder.GetEquivalentEncoderTicksFromDegrees(1.85), ProfileSCurveTriangularFull.ObjectiveStep);
        //}

        //[TestMethod]
        //public void TestFullTriangularSCurveProfileGoodInterpretation()
        //{
        //    DateTime StartTime = DateTime.UtcNow;
        //    double initialPosition = Encoder.GetEquivalentEncoderTicksFromDegrees(00.0);

            
        //}

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
        //    DateTime StartTime = DateTime.UtcNow;
        //    double initialPosition = Encoder.GetEquivalentEncoderTicksFromDegrees(60.0);

        //    //Assert.AreEqual(initialPosition, ProfileSCurveTriangularPartial.InterpretAt(StartTime, StartTime));
        //    //Assert.AreEqual(0, ProfileSCurveTriangularPartial.InterpretAt(StartTime, StartTime.AddMilliseconds(100)));
        //    //Assert.AreEqual(0, ProfileSCurveTriangularPartial.InterpretAt(StartTime, StartTime.AddMilliseconds(300)));
        //    //Assert.AreEqual(0, ProfileSCurveTriangularPartial.InterpretAt(StartTime, StartTime.AddMilliseconds(600)));
        //    //Assert.AreEqual(0, ProfileSCurveTriangularPartial.InterpretAt(StartTime, StartTime.AddMilliseconds(930)));
        //    //Assert.AreEqual(0, ProfileSCurveTriangularPartial.InterpretAt(StartTime, StartTime.AddMilliseconds(1200)));
        //}

        //[TestMethod]
        //[ExpectedException(typeof(ArgumentException))]
        //public void TestPartialTriangularSCurveProfileBadInterpretation()
        //{
        //    DateTime StartTime = DateTime.Now;
        //    ProfileSCurveTriangularPartial.InterpretEncoderTicksAt(StartTime, StartTime.AddSeconds(-1));
        //}

        [TestMethod]
        public void TestFullLinearProfileConstructorAndProperties()
        {
            Assert.AreEqual(SimulationMCUTrajectoryProfileTypeEnum.LINEAR_FULL, ProfileLinearFull.ProfileType);
            Assert.AreEqual(0.0, ProfileLinearFull.InitialVelocity, DOUBLE_EPSILON);
            Assert.AreEqual(HardwareConstants.SIMULATION_MCU_PEAK_VELOCITY, ProfileLinearFull.TrajectoryPeakVelocity, DOUBLE_EPSILON);
            Assert.AreEqual(HardwareConstants.SIMULATION_MCU_PEAK_ACCELERATION, ProfileLinearFull.TrajectoryPeakAcceleration, DOUBLE_EPSILON);
            Assert.AreEqual(1.6729167, ProfileLinearFull.TotalTime, DOUBLE_EPSILON);
            Assert.AreEqual(0, ProfileLinearFull.InitialStep);
            Assert.AreEqual(20, ProfileLinearFull.ObjectiveStep);
        }

        [TestMethod]
        public void TestFullLinearProfileGoodInterpretation()
        {
            DateTime StartTime = DateTime.Now;

            Assert.AreEqual(ProfileLinearFull.InitialStep + 7, ProfileLinearFull.InterpretEncoderTicksAt(StartTime, StartTime.AddSeconds(0.703125)));
            Assert.AreEqual(ProfileLinearFull.InitialStep + 11, ProfileLinearFull.InterpretEncoderTicksAt(StartTime, StartTime.AddSeconds(0.883125)));
            Assert.AreEqual(ProfileLinearFull.InitialStep + 13, ProfileLinearFull.InterpretEncoderTicksAt(StartTime, StartTime.AddSeconds(0.96979167)));
            Assert.AreEqual(ProfileLinearFull.InitialStep + 18, ProfileLinearFull.InterpretEncoderTicksAt(StartTime, StartTime.AddSeconds(1.25)));
            Assert.AreEqual(ProfileLinearFull.ObjectiveStep, ProfileLinearFull.InterpretEncoderTicksAt(StartTime, StartTime.AddYears(10)));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestFullLinearProfileBadInterpretation()
        {
            DateTime StartTime = DateTime.Now;
            ProfileLinearFull.InterpretEncoderTicksAt(StartTime, StartTime.AddMilliseconds(-100));
        }

        [TestMethod]
        public void TestPartialLinearProfileConstructorAndProperties()
        {
            Assert.AreEqual(SimulationMCUTrajectoryProfileTypeEnum.LINEAR_PARTIAL, ProfileLinearPartial.ProfileType);
            Assert.AreEqual(0.0, ProfileLinearPartial.InitialVelocity, DOUBLE_EPSILON);
            Assert.AreEqual(17.8885438, ProfileLinearPartial.TrajectoryPeakVelocity, DOUBLE_EPSILON);
            Assert.AreEqual(HardwareConstants.SIMULATION_MCU_PEAK_ACCELERATION, ProfileLinearPartial.TrajectoryPeakAcceleration, DOUBLE_EPSILON);
            Assert.AreEqual(1.1180339, ProfileLinearPartial.TotalTime, DOUBLE_EPSILON);
            Assert.AreEqual(0, ProfileLinearPartial.InitialStep);
            Assert.AreEqual(10, ProfileLinearPartial.ObjectiveStep);
        }

        [TestMethod]
        public void TestPartialLinearProfileGoodInterpretation()
        {
            DateTime StartTime = DateTime.Now;

            Assert.AreEqual(ProfileLinearPartial.InitialStep + 1, ProfileLinearPartial.InterpretEncoderTicksAt(StartTime, StartTime.AddSeconds(0.25)));
            Assert.AreEqual(ProfileLinearPartial.InitialStep + 4, ProfileLinearPartial.InterpretEncoderTicksAt(StartTime, StartTime.AddSeconds(0.5)));
            Assert.AreEqual(ProfileLinearPartial.InitialStep + 5, ProfileLinearPartial.InterpretEncoderTicksAt(StartTime, StartTime.AddSeconds(0.56)));
            Assert.AreEqual(ProfileLinearPartial.InitialStep + 7, ProfileLinearPartial.InterpretEncoderTicksAt(StartTime, StartTime.AddSeconds(0.76)));
            Assert.AreEqual(ProfileLinearPartial.ObjectiveStep, ProfileLinearPartial.InterpretEncoderTicksAt(StartTime, StartTime.AddYears(10)));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestPartiallLinearProfileBadInterpretation()
        {
            DateTime StartTime = DateTime.UtcNow;
            ProfileLinearPartial.InterpretEncoderTicksAt(StartTime, StartTime.AddTicks(-10));
        }
    }
}
