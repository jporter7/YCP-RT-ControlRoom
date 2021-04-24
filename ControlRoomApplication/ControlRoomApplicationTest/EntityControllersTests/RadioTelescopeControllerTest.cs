using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Database;
using ControlRoomApplication.Simulators.Hardware.WeatherStation;
using System.Threading;
using System.Threading.Tasks;
using ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager;
using ControlRoomApplication.Controllers.SensorNetwork;
using System.Net;
using ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager.Enumerations;

namespace ControlRoomApplicationTest.EntityControllersTests {
    [TestClass]
    public class RadioTelescopeControllerTest {
        private static string ip = PLCConstants.LOCAL_HOST_IP;
        private static int port = 8086;

        private static RadioTelescopeController TestRadioTelescopeController;
        private static TestPLCDriver TestRTPLC;

        private static SensorNetworkServer SensorNetworkServer;

        [TestCleanup]
        public void testClean() {
            try {
                // Make sure all overrides are false
                TestRadioTelescopeController.overrides.setAzimuthAbsEncoder(false);
                TestRadioTelescopeController.overrides.setElevationAbsEncoder(false);
                TestRadioTelescopeController.overrides.setAzimuthMotTemp(false);
                TestRadioTelescopeController.overrides.setElevationMotTemp(false);
                TestRadioTelescopeController.overrides.setCounterbalanceAccelerometer(false);
                TestRadioTelescopeController.overrides.setAzimuthAccelerometer(false);
                TestRadioTelescopeController.overrides.setElevationAccelerometer(false);
                TestRadioTelescopeController.overrides.setElProx0Override(false);
                TestRadioTelescopeController.overrides.setElProx90Override(false);

                TestRadioTelescopeController.RadioTelescope.PLCDriver.publicKillHeartbeatComponent();
                TestRadioTelescopeController.RadioTelescope.SensorNetworkServer.EndSensorMonitoringRoutine();
            } catch { }
        }

        [TestInitialize]
        public void testInit() {
            TestRTPLC = new TestPLCDriver(ip, ip, 15001, 15003, true);
            //     TestRTPLC = new ProductionPLCDriver("192.168.0.70", "192.168.0.50" , 502 , 502 );
            SpectraCyberSimulatorController SCSimController = new SpectraCyberSimulatorController(new SpectraCyberSimulator());
            Location location = MiscellaneousConstants.JOHN_RUDY_PARK;
            SensorNetworkServer = new SensorNetworkServer(IPAddress.Parse("127.0.0.1"), 3000, "127.0.0.1", 3001, 500, false);
            RadioTelescope TestRT = new RadioTelescope(SCSimController, TestRTPLC, location, new Orientation(0, 0));
            TestRT.SensorNetworkServer = SensorNetworkServer;
            //TestRT.SensorNetworkServer.StartSensorMonitoringRoutine();
            TestRT.WeatherStation = new SimulationWeatherStation(1000);
            TestRadioTelescopeController = new RadioTelescopeController(TestRT);

            // Override motor temperature sensors
            TestRadioTelescopeController.overrides.overrideAzimuthMotTemp = true;
            TestRadioTelescopeController.overrides.overrideElevatMotTemp = true;
            TestRTPLC.setTelescopeType(RadioTelescopeTypeEnum.HARD_STOPS);

            TestRTPLC.StartAsyncAcceptingClients();

            TestRTPLC.Configure_MCU(.06, .06, 300, 300);
        }

        [TestMethod]
        public void TestTestCommunication() {
            var test_result = TestRadioTelescopeController.TestCommunication();
            Assert.IsTrue(test_result);
        }

        [TestMethod]
        public void TestGetCurrentOrientation() {
            // Create an Orientation object with an azimuth of 311 and elevation of 42
            Orientation Orientation = new Orientation(311.0, 42.0);
            // Set the RadioTelescope's CurrentOrientation field
            MovementResult response = TestRadioTelescopeController.MoveRadioTelescopeToOrientation(Orientation, MovePriority.Appointment);
            // Call the GetCurrentOrientationMethod
            Orientation CurrentOrientation = TestRadioTelescopeController.GetCurrentOrientation();
            // Ensure the objects are identical
            Assert.AreEqual(response, MovementResult.Success);
            Assert.AreEqual(Orientation.Azimuth, CurrentOrientation.Azimuth, 0.001);
            Assert.AreEqual(Orientation.Elevation, CurrentOrientation.Elevation, 0.001);


            Orientation = new Orientation(28.0, 42.0);
            // Set the RadioTelescope's CurrentOrientation field
            response = TestRadioTelescopeController.MoveRadioTelescopeToOrientation(Orientation, MovePriority.Appointment);
            // Call the GetCurrentOrientationMethod
            CurrentOrientation = TestRadioTelescopeController.GetCurrentOrientation();
            Assert.AreEqual(response, MovementResult.Success);
            Assert.AreEqual(Orientation.Azimuth, CurrentOrientation.Azimuth, 0.001);
            Assert.AreEqual(Orientation.Elevation, CurrentOrientation.Elevation, 0.001);


            Orientation = new Orientation(310.0, 42.0);
            // Set the RadioTelescope's CurrentOrientation field
            response = TestRadioTelescopeController.MoveRadioTelescopeToOrientation(Orientation, MovePriority.Appointment);
            // Call the GetCurrentOrientationMethod
            CurrentOrientation = TestRadioTelescopeController.GetCurrentOrientation();
            // Ensure the objects are identical
            Assert.AreEqual(response, MovementResult.Success);
            Assert.AreEqual(Orientation.Azimuth, CurrentOrientation.Azimuth, 0.001);
            Assert.AreEqual(Orientation.Elevation, CurrentOrientation.Elevation, 0.001);

        }

        [TestMethod]
        public void TestGetCurrentSafetyInterlockStatus() {
            Task.Delay(1000).Wait();
            //estRTPLC.setSaftyInterlock();

            // Test the safety interlock status
            var response = TestRadioTelescopeController.GetCurrentSafetyInterlockStatus();

            // Make sure safety interlock is online
            Assert.IsTrue(response);
        }

        [TestMethod]
        public void TestCancelCurrentMoveCommand() {
            // Test canceling the the current move command
            TestRadioTelescopeController.MoveRadioTelescopeToOrientation(new Orientation(0, 0), MovePriority.Appointment);
            var response = TestRadioTelescopeController.CancelCurrentMoveCommand(MovePriority.GeneralStop);

            // Make sure it was successful
            Assert.IsTrue(response);
        }

        [TestMethod]
        public void TestShutdownRadioTelescope() {
            // Call the ShutdownRadioTelescope method
            var response = TestRadioTelescopeController.ShutdownRadioTelescope();
            Orientation orientation = TestRadioTelescopeController.GetCurrentOrientation();

            // The Radio Telescope should now have a CurrentOrientation of (0, -90)
            Assert.IsTrue(response);

            //Assert.AreEqual( 0.0, orientation.Azimuth, 0.001 );
            //Assert.AreEqual( 90.0 , orientation.Elevation, 0.001 );
        }

        [TestMethod]
        public void TestThermalCalibrateRadioTelescope() {
            Orientation before = TestRadioTelescopeController.GetCurrentOrientation();

            // Call the CalibrateRadioTelescope method
            var response = TestRadioTelescopeController.ThermalCalibrateRadioTelescope(MovePriority.Appointment);
            // this var response is going to be null because there is no reading. It means that the 
            // telescope isn't calibrated. At this time, all we care about is the movement

            // The Radio Telescope should now have a CurrentOrienation of what it had before the call
            Assert.AreEqual(TestRadioTelescopeController.RadioTelescope.CurrentOrientation.Azimuth, before.Azimuth);
            Assert.AreEqual(TestRadioTelescopeController.RadioTelescope.CurrentOrientation.Elevation, before.Elevation);
        }

        [TestMethod]
        public void TestMoveRadioTelescope_HardStops()
        {
            // Set the Telescope Type to HARD STOPS
            TestRTPLC.setTelescopeType(RadioTelescopeTypeEnum.HARD_STOPS);

            // Create an Orientation object with an azimuth of 311 and elevation of 42
            Orientation Orientation = new Orientation(311.0, 42.0);

            // Set the RadioTelescope's CurrentOrientation field
            MovementResult response = TestRadioTelescopeController.MoveRadioTelescopeToOrientation(Orientation, MovePriority.Appointment);

            // Call the GetCurrentOrientationMethod
            Orientation CurrentOrientation = TestRadioTelescopeController.GetCurrentOrientation();

            // Ensure the objects are identical
            Assert.AreEqual(response, MovementResult.Success);
            Assert.AreEqual(Orientation.Azimuth, CurrentOrientation.Azimuth, 0.001);
            Assert.AreEqual(Orientation.Elevation, CurrentOrientation.Elevation, 0.001);
        }

        [TestMethod]
        public void TestMoveRadioTelescope_SlipRing_311degrees()
        {
            // Set the Telescope Type to SLIP RING
            TestRTPLC.setTelescopeType(RadioTelescopeTypeEnum.SLIP_RING);

            // Create an Orientation object with an azimuth of 311 and elevation of 0
            Orientation TargetOrientation = new Orientation(311.0, 0);

            // Set the RadioTelescope's CurrentOrientation field
            MovementResult response = TestRadioTelescopeController.MoveRadioTelescopeToOrientation(TargetOrientation, MovePriority.Appointment);

            // Call the GetCurrentOrientationMethod
            Orientation CurrentOrientation = TestRadioTelescopeController.GetCurrentOrientation();

            // Ensure the objects are identical
            Assert.AreEqual(response, MovementResult.Success);
            Assert.AreEqual(TargetOrientation.Azimuth, CurrentOrientation.Azimuth, 0.001);
            Assert.AreEqual(TargetOrientation.Elevation, CurrentOrientation.Elevation, 0.001);
        }

        [TestMethod]
        public void TestMoveRadioTelescope_SlipRing_359degrees()
        {
            // Set the Telescope Type to SLIP RING
            TestRTPLC.setTelescopeType(RadioTelescopeTypeEnum.SLIP_RING);

            // Create an Orientation object with an azimuth of 311 and elevation of 0
            Orientation TargetOrientation = new Orientation(359.0, 0);

            // Set the RadioTelescope's CurrentOrientation field
            MovementResult response = TestRadioTelescopeController.MoveRadioTelescopeToOrientation(TargetOrientation, MovePriority.Appointment);

            // Call the GetCurrentOrientationMethod
            Orientation CurrentOrientation = TestRadioTelescopeController.GetCurrentOrientation();

            // Ensure the objects are identical
            Assert.AreEqual(response, MovementResult.Success);
            Assert.AreEqual(TargetOrientation.Azimuth, CurrentOrientation.Azimuth, 0.001);
            Assert.AreEqual(TargetOrientation.Elevation, CurrentOrientation.Elevation, 0.001);
        }

        [TestMethod]
        public void TestMoveRadioTelescope_SlipRing_MoveFrom350To90()
        {
            // Set the Telescope Type to SLIP RING
            TestRTPLC.setTelescopeType(RadioTelescopeTypeEnum.SLIP_RING);

            // Initial movement from 0 to 350
            Orientation InitialOrientation = new Orientation(350, 0);
            TestRadioTelescopeController.MoveRadioTelescopeToOrientation(InitialOrientation, MovePriority.Appointment);

            // Move from 350 to 90
            Orientation TargetOrientation = new Orientation(90, 0);
            MovementResult response = TestRadioTelescopeController.MoveRadioTelescopeToOrientation(TargetOrientation, MovePriority.Appointment);

            // Call the GetCurrentOrientationMethod
            Orientation CurrentOrientation = TestRadioTelescopeController.GetCurrentOrientation();

            // Ensure the objects are identical
            Assert.AreEqual(response, MovementResult.Success);
            Assert.AreEqual(TargetOrientation.Azimuth, CurrentOrientation.Azimuth, 0.001);
            Assert.AreEqual(TargetOrientation.Elevation, CurrentOrientation.Elevation, 0.001);
        }

        [TestMethod]
        public void TestMoveRadioTelescope_SlipRing_MoveFrom90To350()
        {
            // Set the Telescope Type to SLIP RING
            TestRTPLC.setTelescopeType(RadioTelescopeTypeEnum.SLIP_RING);

            // Initial movement from 0 to 90
            Orientation InitialOrientation = new Orientation(90, 0);
            TestRadioTelescopeController.MoveRadioTelescopeToOrientation(InitialOrientation, MovePriority.Appointment);

            // Move from 90 to 350
            Orientation TargetOrientation = new Orientation(350, 0);
            MovementResult response = TestRadioTelescopeController.MoveRadioTelescopeToOrientation(TargetOrientation, MovePriority.Appointment);

            // Call the GetCurrentOrientationMethod
            Orientation CurrentOrientation = TestRadioTelescopeController.GetCurrentOrientation();

            // Ensure the objects are identical
            Assert.AreEqual(response, MovementResult.Success);
            Assert.AreEqual(TargetOrientation.Azimuth, CurrentOrientation.Azimuth, 0.001);
            Assert.AreEqual(TargetOrientation.Elevation, CurrentOrientation.Elevation, 0.001);
        }

        [TestMethod]
        public void test_conversions() {
            Random random = new Random();
            double degaz = 83.33, degel = 43.34;
            int az = ConversionHelper.DegreesToSteps(degaz, MotorConstants.GEARING_RATIO_AZIMUTH);
            double testaz = ConversionHelper.StepsToDegrees(az, MotorConstants.GEARING_RATIO_AZIMUTH);
            Assert.AreEqual(degaz, testaz, 0.001);

            int el = ConversionHelper.DegreesToSteps(degel, MotorConstants.GEARING_RATIO_ELEVATION);
            double testel = ConversionHelper.StepsToDegrees(el, MotorConstants.GEARING_RATIO_ELEVATION);
            Assert.AreEqual(degel, testel, 0.001);

            for (int i = 0; i < 360; i++) {
                double val = i + random.NextDouble();
                az = ConversionHelper.DegreesToSteps(val, MotorConstants.GEARING_RATIO_AZIMUTH);
                testaz = ConversionHelper.StepsToDegrees(az, MotorConstants.GEARING_RATIO_AZIMUTH);
                Assert.AreEqual(val, testaz, 0.001);

                el = ConversionHelper.DegreesToSteps(val, MotorConstants.GEARING_RATIO_ELEVATION);
                testel = ConversionHelper.StepsToDegrees(el, MotorConstants.GEARING_RATIO_ELEVATION);
                Assert.AreEqual(val, testel, 0.001);
            }

            double RPM = .5;
            int STEPSperSECOND = 83_333;

            int xpx = ConversionHelper.RPMToSPS(RPM, MotorConstants.GEARING_RATIO_AZIMUTH);
            Assert.AreEqual(STEPSperSECOND, xpx);

            double RPMOUT = ConversionHelper.SPSToRPM(xpx, MotorConstants.GEARING_RATIO_AZIMUTH);
            Assert.AreEqual(RPM, RPMOUT, 0.01);

            double dps = 3;
            xpx = ConversionHelper.DPSToSPS(dps, MotorConstants.GEARING_RATIO_AZIMUTH);
            Assert.AreEqual(STEPSperSECOND, xpx);

            double dpsOUT = ConversionHelper.SPSToDPS(xpx, MotorConstants.GEARING_RATIO_AZIMUTH);
            Assert.AreEqual(dps, dpsOUT, 0.01);


        }

        [TestMethod]
        public void test_MCU_Time_Estimate() {
            int vel = 170_000, dist = 10_000_000;
            int time = MCUManager.EstimateMovementTime(vel, dist);
            Assert.AreEqual(61800, time);
        }

        [TestMethod]
        public void testOrientationChange_100_100Degrees()
        {
            // Acquire current orientation
            Orientation currOrientation;
            currOrientation = TestRadioTelescopeController.RadioTelescope.PLCDriver.GetMotorEncoderPosition();

            // Calculate motor steps from current orientation
            int currStepsAz, currStepsEl;
            currStepsAz = ConversionHelper.DegreesToSteps(currOrientation.Azimuth, MotorConstants.GEARING_RATIO_AZIMUTH);
            currStepsEl = ConversionHelper.DegreesToSteps(currOrientation.Elevation, MotorConstants.GEARING_RATIO_ELEVATION);

            // Set target orientation on both Azimuth and Elevation, respectively
            Orientation expectedOrientation = new Orientation(100, 100);

            // Calculate motor steps necessary for movement
            int posTransAz, posTransEl;
            posTransAz = ConversionHelper.DegreesToSteps(expectedOrientation.Azimuth - currOrientation.Azimuth, MotorConstants.GEARING_RATIO_AZIMUTH);
            posTransEl = ConversionHelper.DegreesToSteps(expectedOrientation.Elevation - currOrientation.Elevation, MotorConstants.GEARING_RATIO_ELEVATION);

            // Move telescope
            TestRadioTelescopeController.RadioTelescope.PLCDriver.RelativeMove(100_000, posTransAz, posTransEl);

            // Create result orientation
            Orientation resultOrientation = TestRadioTelescopeController.RadioTelescope.PLCDriver.GetMotorEncoderPosition();

            // Assert expected and result are identical
            Assert.AreEqual(expectedOrientation, resultOrientation);
        }

        [TestMethod]
        public void testOrientationChange_0_0Degrees()
        {
            // Acquire current orientation
            Orientation currOrientation;
            currOrientation = TestRadioTelescopeController.RadioTelescope.PLCDriver.GetMotorEncoderPosition();

            // Calculate motor steps from current orientation
            int currStepsAz, currStepsEl;
            currStepsAz = ConversionHelper.DegreesToSteps(currOrientation.Azimuth, MotorConstants.GEARING_RATIO_AZIMUTH);
            currStepsEl = ConversionHelper.DegreesToSteps(currOrientation.Elevation, MotorConstants.GEARING_RATIO_ELEVATION);

            // Set target orientation on both Azimuth and Elevation, respectively
            Orientation expectedOrientation = new Orientation(0, 0);

            // Calculate motor steps necessary for movement
            int posTransAz, posTransEl;
            posTransAz = ConversionHelper.DegreesToSteps(expectedOrientation.Azimuth - currOrientation.Azimuth, MotorConstants.GEARING_RATIO_AZIMUTH);
            posTransEl = ConversionHelper.DegreesToSteps(expectedOrientation.Elevation - currOrientation.Elevation, MotorConstants.GEARING_RATIO_ELEVATION);

            // Move telescope
            TestRadioTelescopeController.RadioTelescope.PLCDriver.RelativeMove(100_000, posTransAz, posTransEl);

            // Create result orientation
            Orientation resultOrientation = TestRadioTelescopeController.RadioTelescope.PLCDriver.GetMotorEncoderPosition();

            // Assert expected and result are identical
            Assert.AreEqual(expectedOrientation, resultOrientation);
        }

        [TestMethod]
        public void testOrientationChange_360_210Degrees()
        {
            // Set the Telescope Type to SLIP RING
            TestRTPLC.setTelescopeType(RadioTelescopeTypeEnum.HARD_STOPS);

            // Acquire current orientation
            Orientation currOrientation;
            currOrientation = TestRadioTelescopeController.RadioTelescope.PLCDriver.GetMotorEncoderPosition();

            // Calculate motor steps from current orientation
            int currStepsAz, currStepsEl;
            currStepsAz = ConversionHelper.DegreesToSteps(currOrientation.Azimuth, MotorConstants.GEARING_RATIO_AZIMUTH);
            currStepsEl = ConversionHelper.DegreesToSteps(currOrientation.Elevation, MotorConstants.GEARING_RATIO_ELEVATION);

            // Set target orientation on both Azimuth and Elevation, respectively
            Orientation expectedOrientation = new Orientation(360, 210);

            // Calculate motor steps necessary for movement
            int posTransAz, posTransEl;
            posTransAz = ConversionHelper.DegreesToSteps(expectedOrientation.Azimuth - currOrientation.Azimuth, MotorConstants.GEARING_RATIO_AZIMUTH);
            posTransEl = ConversionHelper.DegreesToSteps(expectedOrientation.Elevation - currOrientation.Elevation, MotorConstants.GEARING_RATIO_ELEVATION);

            // Move telescope
            TestRadioTelescopeController.RadioTelescope.PLCDriver.RelativeMove(100_000, posTransAz, posTransEl);

            // Create result orientation
            Orientation resultOrientation = TestRadioTelescopeController.RadioTelescope.PLCDriver.GetMotorEncoderPosition();

            // Assert expected and result are identical
            Assert.AreEqual(expectedOrientation.Azimuth, resultOrientation.Azimuth, 0.001);
            Assert.AreEqual(expectedOrientation.Elevation, resultOrientation.Elevation, 0.001);
        }

        [TestMethod]
        public void test_temperature_check()
        {
            // make sure neither override is set
            DatabaseOperations.SetOverrideForSensor(SensorItemEnum.AZIMUTH_MOTOR, false);
            DatabaseOperations.SetOverrideForSensor(SensorItemEnum.ELEVATION_MOTOR, false);

            // Enable motor temperature sensors
            TestRadioTelescopeController.overrides.overrideAzimuthMotTemp = false;
            TestRadioTelescopeController.overrides.overrideElevatMotTemp = false;

            /** Azimuth tests **/
            Temperature az = new Temperature(); az.location_ID = (int)SensorLocationEnum.AZ_MOTOR;

            // Stable
            az.temp = 100;
            Assert.IsTrue(TestRadioTelescopeController.checkTemp(az, true));

            // Stable
            az.temp = 50;
            Assert.IsTrue(TestRadioTelescopeController.checkTemp(az, true));

            // Overheating
            az.temp = 151;
            Assert.IsFalse(TestRadioTelescopeController.checkTemp(az, true));

            // Overheating
            az.temp = 151;
            Assert.IsFalse(TestRadioTelescopeController.checkTemp(az, true));

            // Too cold
            az.temp = -1;
            Assert.IsFalse(TestRadioTelescopeController.checkTemp(az, true));


            /** Elevation tests **/
            Temperature el = new Temperature(); el.location_ID = (int)SensorLocationEnum.EL_MOTOR;

            // Stable
            el.temp = 100;
            Assert.IsTrue(TestRadioTelescopeController.checkTemp(el, true));

            // Stable
            el.temp = 50;
            Assert.IsTrue(TestRadioTelescopeController.checkTemp(el, true));

            // Overheating
            el.temp = 151;
            Assert.IsFalse(TestRadioTelescopeController.checkTemp(el, true));

            // Too cold
            el.temp = -1;
            Assert.IsFalse(TestRadioTelescopeController.checkTemp(el, true));
        }

        [TestMethod]
        public void test_temperature_overrides()
        {
            // make sure both overrides are set
            TestRadioTelescopeController.overrides.overrideAzimuthMotTemp = true;
            TestRadioTelescopeController.overrides.overrideElevatMotTemp = true;

            /** Azimuth temperatures **/
            Temperature az = new Temperature(); az.location_ID = (int)SensorLocationEnum.AZ_MOTOR;

            // Stable
            az.temp = 50;
            Assert.IsTrue(TestRadioTelescopeController.checkTemp(az, true));

            // Stable
            az.temp = 150;
            Assert.IsTrue(TestRadioTelescopeController.checkTemp(az, true));

            // Overheating
            az.temp = 151;
            Assert.IsTrue(TestRadioTelescopeController.checkTemp(az, true));

            // To cold
            az.temp = 49;
            Assert.IsTrue(TestRadioTelescopeController.checkTemp(az, true));


            /** Elevation temperatures **/
            Temperature el = new Temperature(); el.location_ID = (int)SensorLocationEnum.EL_MOTOR;

            // Stable
            el.temp = 50;
            Assert.IsTrue(TestRadioTelescopeController.checkTemp(el, true));

            // Stable
            el.temp = 150;
            Assert.IsTrue(TestRadioTelescopeController.checkTemp(el, true));

            // Overheating
            el.temp = 151;
            Assert.IsTrue(TestRadioTelescopeController.checkTemp(el, true));

            // To cold
            el.temp = 49;
            Assert.IsTrue(TestRadioTelescopeController.checkTemp(el, true));
        }

        [TestMethod]
        public void test_plc_overrides()
        {
            // Default override settings (no override)
            Assert.IsTrue(0 == (int)TestRadioTelescopeController.RadioTelescope.PLCDriver.getregvalue((ushort)PLC_modbus_server_register_mapping.AZ_0_LIMIT));
            Assert.IsTrue(0 == (int)TestRadioTelescopeController.RadioTelescope.PLCDriver.getregvalue((ushort)PLC_modbus_server_register_mapping.AZ_375_LIMIT));
            Assert.IsTrue(0 == (int)TestRadioTelescopeController.RadioTelescope.PLCDriver.getregvalue((ushort)PLC_modbus_server_register_mapping.EL_10_LIMIT));
            Assert.IsTrue(0 == (int)TestRadioTelescopeController.RadioTelescope.PLCDriver.getregvalue((ushort)PLC_modbus_server_register_mapping.EL_90_LIMIT));
            Assert.IsTrue(0 == (int)TestRadioTelescopeController.RadioTelescope.PLCDriver.getregvalue((ushort)PLC_modbus_server_register_mapping.GATE_OVERRIDE));

            // Flip switches
            TestRadioTelescopeController.RadioTelescope.PLCDriver.setregvalue((ushort)PLC_modbus_server_register_mapping.AZ_0_LIMIT, 1);
            TestRadioTelescopeController.RadioTelescope.PLCDriver.setregvalue((ushort)PLC_modbus_server_register_mapping.AZ_375_LIMIT, 1);
            TestRadioTelescopeController.RadioTelescope.PLCDriver.setregvalue((ushort)PLC_modbus_server_register_mapping.EL_10_LIMIT, 1);
            TestRadioTelescopeController.RadioTelescope.PLCDriver.setregvalue((ushort)PLC_modbus_server_register_mapping.EL_90_LIMIT, 1);
            TestRadioTelescopeController.RadioTelescope.PLCDriver.setregvalue((ushort)PLC_modbus_server_register_mapping.GATE_OVERRIDE, 1);

            // Now overriding
            Assert.IsTrue(1 == (int)TestRadioTelescopeController.RadioTelescope.PLCDriver.getregvalue((ushort)PLC_modbus_server_register_mapping.AZ_0_LIMIT));
            Assert.IsTrue(1 == (int)TestRadioTelescopeController.RadioTelescope.PLCDriver.getregvalue((ushort)PLC_modbus_server_register_mapping.AZ_375_LIMIT));
            Assert.IsTrue(1 == (int)TestRadioTelescopeController.RadioTelescope.PLCDriver.getregvalue((ushort)PLC_modbus_server_register_mapping.EL_10_LIMIT));
            Assert.IsTrue(1 == (int)TestRadioTelescopeController.RadioTelescope.PLCDriver.getregvalue((ushort)PLC_modbus_server_register_mapping.EL_90_LIMIT));
            Assert.IsTrue(1 == (int)TestRadioTelescopeController.RadioTelescope.PLCDriver.getregvalue((ushort)PLC_modbus_server_register_mapping.GATE_OVERRIDE));
        }

        [TestMethod]
        public void TestHomeTelescope_BothAbsolutePositionsOK_Success()
        {
            MovementResult result = TestRadioTelescopeController.HomeTelescope(MovePriority.Manual);

            Assert.AreEqual(MovementResult.Success, result);
        }

        [TestMethod]
        public void TestHomeTelescope_ElevationAbsoluteEncoderOff_IncorrectPosition()
        {
            SensorNetworkServer.CurrentAbsoluteOrientation.Elevation = 1;

            MovementResult result = TestRadioTelescopeController.HomeTelescope(MovePriority.Manual);

            Assert.AreEqual(MovementResult.IncorrectPosition, result);
        }

        [TestMethod]
        public void TestHomeTelescope_AzimuthAbsoluteEncoderOff_IncorrectPosition()
        {
            SensorNetworkServer.CurrentAbsoluteOrientation.Azimuth = 1;

            MovementResult result = TestRadioTelescopeController.HomeTelescope(MovePriority.Manual);

            Assert.AreEqual(MovementResult.IncorrectPosition, result);
        }

        [TestMethod]
        public void TestHomeTelescope_BothAbsoluteEncodersOff_IncorrectPosition()
        {
            SensorNetworkServer.CurrentAbsoluteOrientation.Azimuth = 1;
            SensorNetworkServer.CurrentAbsoluteOrientation.Elevation = 1;

            MovementResult result = TestRadioTelescopeController.HomeTelescope(MovePriority.Manual);

            Assert.AreEqual(MovementResult.IncorrectPosition, result);
        }

        [TestMethod]
        public void TestHomeTelescope_AzimuthAbsoluteEncoderOffButOverridden_Success()
        {
            SensorNetworkServer.CurrentAbsoluteOrientation.Azimuth = 1;
            TestRadioTelescopeController.overrides.setAzimuthAbsEncoder(true);

            MovementResult result = TestRadioTelescopeController.HomeTelescope(MovePriority.Manual);

            Assert.AreEqual(MovementResult.Success, result);
        }

        [TestMethod]
        public void TestHomeTelescope_ElevationAbsoluteEncoderOffButOverridden_Success()
        {
            SensorNetworkServer.CurrentAbsoluteOrientation.Elevation = 1;
            TestRadioTelescopeController.overrides.setElevationAbsEncoder(true);

            MovementResult result = TestRadioTelescopeController.HomeTelescope(MovePriority.Manual);

            Assert.AreEqual(MovementResult.Success, result);
        }

        [TestMethod]
        public void TestHomeTelescope_BothAbsoluteEncodersOffButOverridden_Success()
        {
            SensorNetworkServer.CurrentAbsoluteOrientation.Elevation = 1;
            SensorNetworkServer.CurrentAbsoluteOrientation.Azimuth = 1;
            TestRadioTelescopeController.overrides.setElevationAbsEncoder(true);
            TestRadioTelescopeController.overrides.setAzimuthAbsEncoder(true);

            MovementResult result = TestRadioTelescopeController.HomeTelescope(MovePriority.Manual);

            Assert.AreEqual(MovementResult.Success, result);
        }

        [TestMethod]
        public void TestHomeTelescope_AzimuthEncoderOKAndOverridden_Success()
        {
            TestRadioTelescopeController.overrides.setAzimuthAbsEncoder(true);

            MovementResult result = TestRadioTelescopeController.HomeTelescope(MovePriority.Manual);

            Assert.AreEqual(MovementResult.Success, result);
        }

        [TestMethod]
        public void TestHomeTelescope_ElevationEncoderOKAndOverridden_Success()
        {
            TestRadioTelescopeController.overrides.setElevationAbsEncoder(true);

            MovementResult result = TestRadioTelescopeController.HomeTelescope(MovePriority.Manual);

            Assert.AreEqual(MovementResult.Success, result);
        }

        [TestMethod]
        public void TestHomeTelescope_BothEncodersOKAndOverridden_Success()
        {
            TestRadioTelescopeController.overrides.setElevationAbsEncoder(true);
            TestRadioTelescopeController.overrides.setAzimuthAbsEncoder(true);

            MovementResult result = TestRadioTelescopeController.HomeTelescope(MovePriority.Manual);

            Assert.AreEqual(MovementResult.Success, result);
        }

        [TestMethod]
        public void TestHomeTelescope_SensorDataUnsafe_SensorsNotSafe()
        {
            SensorNetworkServer.CurrentElevationMotorTemp[0].temp = 3000;
            SensorNetworkServer.CurrentElevationMotorTemp[0].location_ID = (int)SensorLocationEnum.EL_MOTOR;
            TestRadioTelescopeController.overrides.setElevationMotTemp(false);

            Thread.Sleep(2000);

            MovementResult result = TestRadioTelescopeController.HomeTelescope(MovePriority.Manual);

            Assert.AreEqual(MovementResult.SensorsNotSafe, result);
        }

        [TestMethod]
        public void TestHomeTelescope_IsHoming_PriorityPopulated()
        {
            MovePriority priority = MovePriority.Manual;

            // Run async so that we can check for priority change during the movement
            Task.Run(() =>
            {
                MovementResult result = TestRadioTelescopeController.HomeTelescope(priority);
            });

            Thread.Sleep(50);

            Assert.AreEqual(priority, TestRadioTelescopeController.RadioTelescope.PLCDriver.CurrentMovementPriority);
        }

        [TestMethod]
        public void TestHomeTelescope_TriesToHomeWithAnotherCommandRunning_AlreadyMoving()
        {
            MovementResult result1 = MovementResult.None;
            MovementResult result2 = MovementResult.None;
            MovePriority priority = MovePriority.Manual;

            // This is running two commands at the same time. One of them should succeed, while
            // the other is rejected

            Task.Run(() =>
            {
                result1 = TestRadioTelescopeController.HomeTelescope(priority);
            });

            result2 = TestRadioTelescopeController.HomeTelescope(priority);

            Assert.IsTrue(result1 == MovementResult.Success || result2 == MovementResult.Success);
            Assert.IsTrue(result1 == MovementResult.AlreadyMoving || result2 == MovementResult.AlreadyMoving);
        }

        [TestMethod]
        public void TestSnowDump_AllStatusesOK_Success()
        {
            MovementResult result = TestRadioTelescopeController.SnowDump(MovePriority.Manual);

            Assert.AreEqual(MovementResult.Success, result);
        }

        [TestMethod]
        public void TestSnowDump_SensorDataUnsafe_SensorsNotSafe()
        {
            SensorNetworkServer.CurrentElevationMotorTemp[0].temp = 3000;
            SensorNetworkServer.CurrentElevationMotorTemp[0].location_ID = (int)SensorLocationEnum.EL_MOTOR;
            TestRadioTelescopeController.overrides.setElevationMotTemp(false);

            Thread.Sleep(2000);

            MovementResult result = TestRadioTelescopeController.SnowDump(MovePriority.Manual);

            Assert.AreEqual(MovementResult.SensorsNotSafe, result);
        }

        [TestMethod]
        public void TestSnowDump_TriesSnowDumpWithAnotherCommandRunning_AlreadyMoving()
        {
            MovementResult result1 = MovementResult.None;
            MovementResult result2 = MovementResult.None;
            MovePriority priority = MovePriority.Manual;

            // This is running two commands at the same time. One of them should succeed, while
            // the other is rejected

            Task.Run(() =>
            {
                result1 = TestRadioTelescopeController.SnowDump(priority);
            });

            result2 = TestRadioTelescopeController.SnowDump(priority);

            Assert.IsTrue(result1 == MovementResult.Success || result2 == MovementResult.Success);
            Assert.IsTrue(result1 == MovementResult.AlreadyMoving || result2 == MovementResult.AlreadyMoving);
        }

        [TestMethod]
        public void TestFullElevationMove_AllStatusesOK_Success()
        {
            MovementResult result = TestRadioTelescopeController.FullElevationMove(MovePriority.Manual);

            Assert.AreEqual(MovementResult.Success, result);
        }

        [TestMethod]
        public void TestFullElevationMove_SensorDataUnsafe_SensorsNotSafe()
        {
            TestRadioTelescopeController.overrides.setElevationMotTemp(false);
            SensorNetworkServer.CurrentElevationMotorTemp[0].temp = 3000;
            SensorNetworkServer.CurrentElevationMotorTemp[0].location_ID = (int)SensorLocationEnum.EL_MOTOR;

            Thread.Sleep(2000);

            MovementResult result = TestRadioTelescopeController.FullElevationMove(MovePriority.Manual);

            Assert.AreEqual(MovementResult.SensorsNotSafe, result);
        }

        [TestMethod]
        public void TestFullElevationMove_TriesFullElevationMoveWithAnotherCommandRunning_AlreadyMoving()
        {
            MovementResult result1 = MovementResult.None;
            MovementResult result2 = MovementResult.None;
            MovePriority priority = MovePriority.Manual;

            // This is running two commands at the same time. One of them should succeed, while
            // the other is rejected

            Task.Run(() =>
            {
                result1 = TestRadioTelescopeController.FullElevationMove(priority);
            });

            result2 = TestRadioTelescopeController.FullElevationMove(priority);

            Assert.IsTrue(result1 == MovementResult.Success || result2 == MovementResult.Success);
            Assert.IsTrue(result1 == MovementResult.AlreadyMoving || result2 == MovementResult.AlreadyMoving);
        }

        [TestMethod]
        public void TestMoveRadioTelescopeToCoordinate_AllStatusesOK_Success()
        {
            Coordinate c = new Coordinate();

            MovementResult result = TestRadioTelescopeController.MoveRadioTelescopeToCoordinate(c, MovePriority.Manual);

            Assert.AreEqual(MovementResult.Success, result);
        }

        [TestMethod]
        public void TestMoveRadioTelescopeToCoordinate_SensorDataUnsafe_SensorsNotSafe()
        {
            TestRadioTelescopeController.overrides.setElevationMotTemp(false);
            SensorNetworkServer.CurrentElevationMotorTemp[0].temp = 3000;
            SensorNetworkServer.CurrentElevationMotorTemp[0].location_ID = (int)SensorLocationEnum.EL_MOTOR;

            Coordinate c = new Coordinate();

            Thread.Sleep(2000);

            MovementResult result = TestRadioTelescopeController.MoveRadioTelescopeToCoordinate(c, MovePriority.Manual);

            Assert.AreEqual(MovementResult.SensorsNotSafe, result);
        }

        [TestMethod]
        public void TestMoveRadioTelescopeToCoordinate_TriesMoveRadioTelescopeToCoordinateWithAnotherCommandRunning_AlreadyMoving()
        {
            MovementResult result1 = MovementResult.None;
            MovementResult result2 = MovementResult.None;
            MovePriority priority = MovePriority.Manual;

            Coordinate c = new Coordinate();

            // This is running two commands at the same time. One of them should succeed, while
            // the other is rejected

            Task.Run(() =>
            {
                result1 = TestRadioTelescopeController.MoveRadioTelescopeToCoordinate(c, priority);
            });

            result2 = TestRadioTelescopeController.MoveRadioTelescopeToCoordinate(c, priority);

            Assert.IsTrue(result1 == MovementResult.Success || result2 == MovementResult.Success);
            Assert.IsTrue(result1 == MovementResult.AlreadyMoving || result2 == MovementResult.AlreadyMoving);
        }

        [TestMethod]
        public void TestMoveRadioTelescopeToOrientation_AllStatusesOK_Success()
        {
            Orientation o = new Orientation();

            MovementResult result = TestRadioTelescopeController.MoveRadioTelescopeToOrientation(o, MovePriority.Manual);

            Assert.AreEqual(MovementResult.Success, result);
        }

        [TestMethod]
        public void TestMoveRadioTelescopeToOrientation_SensorDataUnsafe_SensorsNotSafe()
        {
            TestRadioTelescopeController.overrides.setElevationMotTemp(false);
            SensorNetworkServer.CurrentElevationMotorTemp[0].temp = 3000;
            SensorNetworkServer.CurrentElevationMotorTemp[0].location_ID = (int)SensorLocationEnum.EL_MOTOR;

            Orientation o = new Orientation();

            Thread.Sleep(2000);

            MovementResult result = TestRadioTelescopeController.MoveRadioTelescopeToOrientation(o, MovePriority.Manual);

            Assert.AreEqual(MovementResult.SensorsNotSafe, result);
        }

        [TestMethod]
        public void TestMoveRadioTelescopeToOrientation_TriesMoveRadioTelescopeToOrientationWithAnotherCommandRunning_AlreadyMoving()
        {
            MovementResult result1 = MovementResult.None;
            MovementResult result2 = MovementResult.None;
            MovePriority priority = MovePriority.Manual;

            Orientation o = new Orientation();

            // This is running two commands at the same time. One of them should succeed, while
            // the other is rejected

            Task.Run(() =>
            {
                result1 = TestRadioTelescopeController.MoveRadioTelescopeToOrientation(o, priority);
            });

            result2 = TestRadioTelescopeController.MoveRadioTelescopeToOrientation(o, priority);

            Assert.IsTrue(result1 == MovementResult.Success || result2 == MovementResult.Success);
            Assert.IsTrue(result1 == MovementResult.AlreadyMoving || result2 == MovementResult.AlreadyMoving);
        }

        [TestMethod]
        public void TestThermalCalibrateRadioTelescope_AllStatusesOK_Success()
        {
            MovementResult result = TestRadioTelescopeController.ThermalCalibrateRadioTelescope(MovePriority.Manual);

            Assert.AreEqual(MovementResult.Success, result);
        }

        [TestMethod]
        public void TestThermalCalibrateRadioTelescope_SensorDataUnsafe_SensorsNotSafe()
        {
            TestRadioTelescopeController.overrides.setElevationMotTemp(false);
            SensorNetworkServer.CurrentElevationMotorTemp[0].temp = 3000;
            SensorNetworkServer.CurrentElevationMotorTemp[0].location_ID = (int)SensorLocationEnum.EL_MOTOR;
            
            Thread.Sleep(2000);

            MovementResult result = TestRadioTelescopeController.ThermalCalibrateRadioTelescope(MovePriority.Manual);

            Assert.AreEqual(MovementResult.SensorsNotSafe, result);
        }

        [TestMethod]
        public void TestThermalCalibrateRadioTelescope_TriesThermalCalibrateRadioTelescopeWithAnotherCommandRunning_AlreadyMoving()
        {
            MovementResult result1 = MovementResult.None;
            MovementResult result2 = MovementResult.None;
            MovePriority priority = MovePriority.Manual;

            // This is running two commands at the same time. One of them should succeed, while
            // the other is rejected

            Task.Run(() =>
            {
                result1 = TestRadioTelescopeController.ThermalCalibrateRadioTelescope(priority);
            });

            result2 = TestRadioTelescopeController.ThermalCalibrateRadioTelescope(priority);

            Assert.IsTrue(result1 == MovementResult.Success || result2 == MovementResult.Success);
            Assert.IsTrue(result1 == MovementResult.AlreadyMoving || result2 == MovementResult.AlreadyMoving);
        }
    }
}

