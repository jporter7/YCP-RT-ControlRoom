using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Entities;
using System.Threading;

namespace ControlRoomApplicationTest.EntityControllersTests
{
    [TestClass]
    public class RadioTelescopeControllerTest
    {
        private static string ip = PLCConstants.LOCAL_HOST_IP;
        private static int port = 8086;

        private static RadioTelescopeController TestRadioTelescopeController;
        private static TestPLCDriver TestRTPLC;

        [ClassInitialize]
        public static void SetUp(TestContext context)
        {
            //PLCClientCommunicationHandler PLCClientCommHandler = new PLCClientCommunicationHandler(ip, port);
            TestRTPLC = new TestPLCDriver(ip, ip, port, port);

            SpectraCyberSimulatorController SCSimController = new SpectraCyberSimulatorController(new SpectraCyberSimulator());
            Location location = MiscellaneousConstants.JOHN_RUDY_PARK;
            RadioTelescope TestRT = new RadioTelescope(SCSimController, TestRTPLC, location, new Orientation(0, 0));
            TestRadioTelescopeController = new RadioTelescopeController(TestRT);


            TestRTPLC.StartAsyncAcceptingClients();
            //TestRT.PLCClient.ConnectToServer();

        }

        [TestMethod]
        public void TestTestCommunication()
        {
            var test_result = TestRadioTelescopeController.TestCommunication();
            Assert.IsTrue(test_result);
        }

        [TestMethod]
        public void TestGetCurrentOrientation()
        {
            // Create an Orientation object with an azimuth of 311 and elevation of 42
            Orientation Orientation = new Orientation(311.0, 42.0);
            // Set the RadioTelescope's CurrentOrientation field
            var response = TestRadioTelescopeController.MoveRadioTelescopeToOrientation(Orientation);
            // Call the GetCurrentOrientationMethod
            Orientation CurrentOrientation = TestRadioTelescopeController.GetCurrentOrientation();
            // Ensure the objects are identical
            Assert.IsTrue(response);
            Assert.AreEqual(Orientation.Azimuth, CurrentOrientation.Azimuth,0.001);
            Assert.AreEqual(Orientation.Elevation, CurrentOrientation.Elevation, 0.001);


            Orientation = new Orientation(28.0, 42.0);
            // Set the RadioTelescope's CurrentOrientation field
            response = TestRadioTelescopeController.MoveRadioTelescopeToOrientation(Orientation);
            // Call the GetCurrentOrientationMethod
            CurrentOrientation = TestRadioTelescopeController.GetCurrentOrientation();
            Assert.IsTrue(response);
            Assert.AreEqual(Orientation.Azimuth, CurrentOrientation.Azimuth, 0.001);
            Assert.AreEqual(Orientation.Elevation, CurrentOrientation.Elevation, 0.001);


            Orientation = new Orientation(310.0, 42.0);
            // Set the RadioTelescope's CurrentOrientation field
            response = TestRadioTelescopeController.MoveRadioTelescopeToOrientation(Orientation);
            // Call the GetCurrentOrientationMethod
            CurrentOrientation = TestRadioTelescopeController.GetCurrentOrientation();
            // Ensure the objects are identical
            Assert.IsTrue(response);
            Assert.AreEqual(Orientation.Azimuth, CurrentOrientation.Azimuth, 0.001);
            Assert.AreEqual(Orientation.Elevation, CurrentOrientation.Elevation, 0.001);

        }

        [TestMethod]
        public void TestGetCurrentLimitSwitchStatuses()
        {
            // Test the limit switch statuses
            var responses = TestRadioTelescopeController.GetCurrentLimitSwitchStatuses();
            
            // Make sure each limit switch is online
            foreach(var response in responses)
            {
                Assert.IsFalse(response);
            }
        }

        [TestMethod]
        public void TestGetCurrentSafetyInterlockStatus()
        {
            Thread.Sleep( 1000 );
            //estRTPLC.setSaftyInterlock();

            // Test the safety interlock status
            var response = TestRadioTelescopeController.GetCurrentSafetyInterlockStatus();

            // Make sure safety interlock is online
            Assert.IsTrue(response);
        }

        [TestMethod]
        public void TestCancelCurrentMoveCommand()
        {
            // Test canceling the the current move command
            TestRadioTelescopeController.MoveRadioTelescopeToOrientation(new Orientation(0, 0));
            var response = TestRadioTelescopeController.CancelCurrentMoveCommand();

            // Make sure it was successful
            Assert.IsTrue(response);
        }

        [TestMethod]
        public void TestShutdownRadioTelescope()
        {
            // Call the ShutdownRadioTelescope method
            var response = TestRadioTelescopeController.ShutdownRadioTelescope();
            Orientation orientation = TestRadioTelescopeController.GetCurrentOrientation();

            // The Radio Telescope should now have a CurrentOrientation of (0, -90)
            Assert.IsTrue(response);
            Assert.AreEqual(orientation.Azimuth, 0.0 ,0.001);
            Assert.AreEqual(orientation.Elevation, 0.0, 0.001 );
        }

        [TestMethod]
        public void TestCalibrateRadioTelescope()
        {
            // Call the CalibrateRadioTelescope method
            var response = TestRadioTelescopeController.CalibrateRadioTelescope();

            // The Radio Telescope should now have a CurrentOrienation of (0,0)
            Assert.IsTrue(response);
            Assert.AreEqual(TestRadioTelescopeController.RadioTelescope.CurrentOrientation.Azimuth, 0.0);
            Assert.AreEqual(TestRadioTelescopeController.RadioTelescope.CurrentOrientation.Elevation, 0.0);
        }

        [TestMethod]
        public void TestMoveRadioTelescope()
        {
            // Create an Orientation object with an azimuth of 311 and elevation of 42
            Orientation Orientation = new Orientation(311.0, 42.0);

            // Set the RadioTelescope's CurrentOrientation field
            var response = TestRadioTelescopeController.MoveRadioTelescopeToOrientation(Orientation);

            // Call the GetCurrentOrientationMethod
            Orientation CurrentOrientation = TestRadioTelescopeController.GetCurrentOrientation();

            // Ensure the objects are identical
            Assert.IsTrue(response);
            Assert.AreEqual(Orientation.Azimuth, CurrentOrientation.Azimuth,0.001);
            Assert.AreEqual(Orientation.Elevation, CurrentOrientation.Elevation,0.001);
        }

        [TestMethod]
        public void test_conversions()
        {
            Random random = new Random();
            double degaz = 83.33, degel = 43.34;
            int az= ConversionHelper.DegreesToSteps(degaz, MotorConstants.GEARING_RATIO_AZIMUTH);
            double testaz = ConversionHelper.StepsToDegrees(az, MotorConstants.GEARING_RATIO_AZIMUTH);
            Assert.AreEqual(degaz, testaz, 0.001);

            int el = ConversionHelper.DegreesToSteps(degel, MotorConstants.GEARING_RATIO_ELEVATION);
            double testel = ConversionHelper.StepsToDegrees(el, MotorConstants.GEARING_RATIO_ELEVATION);
            Assert.AreEqual(degel, testel, 0.001);

            for (int i = 0; i < 360; i++)
            {
                double val=i+ random.NextDouble();
                az = ConversionHelper.DegreesToSteps(val, MotorConstants.GEARING_RATIO_AZIMUTH);
                testaz = ConversionHelper.StepsToDegrees(az, MotorConstants.GEARING_RATIO_AZIMUTH);
                Assert.AreEqual(val, testaz, 0.001);

                el = ConversionHelper.DegreesToSteps(val, MotorConstants.GEARING_RATIO_ELEVATION);
                testel = ConversionHelper.StepsToDegrees(el, MotorConstants.GEARING_RATIO_ELEVATION);
                Assert.AreEqual(val, testel, 0.001);
            }
        }

        [TestMethod]
        public void test_orientation_change()
        {
            Random random = new Random();
            
            Orientation current_orientation2;
            current_orientation2 = TestRadioTelescopeController.RadioTelescope.PLCDriver.read_Position();
            Orientation current_orientation = current_orientation2;

            int positionTranslationAZ, positionTranslationEL, current_stepsAZ, current_stepsEL;
            current_stepsAZ = ConversionHelper.DegreesToSteps(current_orientation.Azimuth, MotorConstants.GEARING_RATIO_AZIMUTH);
            current_stepsEL = ConversionHelper.DegreesToSteps(current_orientation.Elevation, MotorConstants.GEARING_RATIO_ELEVATION);
            for (int i = 0; i < 1500; i++)
            {
                Orientation target_orientation = new Orientation(random.NextDouble() * 360, random.NextDouble() * 360);
                positionTranslationAZ = ConversionHelper.DegreesToSteps((target_orientation.Azimuth - current_orientation.Azimuth), MotorConstants.GEARING_RATIO_AZIMUTH);
                positionTranslationEL = ConversionHelper.DegreesToSteps((target_orientation.Elevation - current_orientation.Elevation), MotorConstants.GEARING_RATIO_ELEVATION);


                Console.WriteLine("AZ_01 {0,16} EL_01 {1,16}", current_stepsAZ, current_stepsEL);
                current_stepsAZ = current_stepsAZ + positionTranslationAZ;
                current_stepsEL = current_stepsEL + positionTranslationEL;

                current_orientation = new Orientation(ConversionHelper.StepsToDegrees(current_stepsAZ, MotorConstants.GEARING_RATIO_AZIMUTH), ConversionHelper.StepsToDegrees(current_stepsEL, MotorConstants.GEARING_RATIO_ELEVATION));

                Assert.AreEqual(target_orientation.Azimuth, current_orientation.Azimuth, 0.001);
                Assert.AreEqual(target_orientation.Elevation, current_orientation.Elevation, 0.001);
                Console.WriteLine("AZ_finni0 {0,10} EL_finni0 {1,10}", current_stepsAZ, current_stepsEL);
                // current_orientation = new Orientation(ConversionHelper.StepsToDegrees(current_stepsAZ + positionTranslationAZ, MotorConstants.GEARING_RATIO_AZIMUTH), ConversionHelper.StepsToDegrees(current_stepsEL + positionTranslationEL, MotorConstants.GEARING_RATIO_ELEVATION));
                //Console.WriteLine(current_orientation.Elevation + "    "+ target_orientation.Elevation);
                // Console.WriteLine("AZ_step1 {0,10} EL_step1 {1,10}", positionTranslationAZ, positionTranslationEL);
                TestRadioTelescopeController.RadioTelescope.PLCDriver.relative_move(50, 50, positionTranslationAZ, positionTranslationEL);

                current_orientation2 = TestRadioTelescopeController.RadioTelescope.PLCDriver.read_Position();

                Assert.AreEqual(target_orientation.Azimuth, current_orientation2.Azimuth, 0.001);
                Assert.AreEqual(target_orientation.Elevation, current_orientation2.Elevation, 0.001);
            }
        }


        [ClassCleanup]
        public static void BringDown()
        {
            //TestRadioTelescopeController.RadioTelescope.PLCClient.TerminateTCPServerConnection();
            //TestRTPLC.RequestStopAsyncAcceptingClientsAndJoin();
            TestRadioTelescopeController.RadioTelescope.PLCDriver.Bring_down();
        }
    }
}
