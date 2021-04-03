using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Database;
using ControlRoomApplication.Simulators.Hardware.WeatherStation;
using System.Threading;
using System.Threading.Tasks;

namespace ControlRoomApplicationTest.EntityControllersTests {
    [TestClass]
    public class RadioTelescopeControllerTest {
        private static string ip = PLCConstants.LOCAL_HOST_IP;
        private static int port = 8086;

        private static RadioTelescopeController TestRadioTelescopeController;
        private static TestPLCDriver TestRTPLC;
        /*
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

        [ClassCleanup]
        public static void BringDown()
        {
            //TestRadioTelescopeController.RadioTelescope.PLCClient.TerminateTCPServerConnection();
            //TestRTPLC.RequestStopAsyncAcceptingClientsAndJoin();
            TestRadioTelescopeController.RadioTelescope.PLCDriver.Bring_down();
        }
        */
        [TestCleanup]
        public void testClean() {
            try {
                TestRadioTelescopeController.RadioTelescope.PLCDriver.publicKillHeartbeatComponent();
            //TestRadioTelescopeController.RadioTelescope.PLCDriver.Bring_down();
            }catch { }
        }

        [TestInitialize]
        public void testInit() {
            TestRTPLC = new TestPLCDriver( ip, ip, 15001, 15003, true );
       //     TestRTPLC = new ProductionPLCDriver("192.168.0.70", "192.168.0.50" , 502 , 502 );
            SpectraCyberSimulatorController SCSimController = new SpectraCyberSimulatorController( new SpectraCyberSimulator() );
            Location location = MiscellaneousConstants.JOHN_RUDY_PARK;
            RadioTelescope TestRT = new RadioTelescope( SCSimController , TestRTPLC , location , new Orientation( 0 , 0 ) );
            TestRT.WeatherStation = new SimulationWeatherStation(1000);
            TestRadioTelescopeController = new RadioTelescopeController( TestRT );
            
            // Override motor temperature sensors
            TestRadioTelescopeController.overrides.overrideAzimuthMotTemp = true;
            TestRadioTelescopeController.overrides.overrideElevatMotTemp = true;
            TestRTPLC.setTelescopeType(RadioTelescopeTypeEnum.HARD_STOPS);
            TestRTPLC.SetParent(TestRT);
            TestRTPLC.driver.SetParent(TestRT);

            TestRTPLC.StartAsyncAcceptingClients();

            TestRTPLC.Configure_MCU( .06 , .06 , 300 , 300 );
        }

        [TestMethod]
        public void TestTestCommunication() {
            var test_result = TestRadioTelescopeController.TestCommunication();
            Assert.IsTrue( test_result );
        }

        [TestMethod]
        public void TestGetCurrentOrientation() {
            // Create an Orientation object with an azimuth of 311 and elevation of 42
            Orientation Orientation = new Orientation( 311.0 , 42.0 );
            // Set the RadioTelescope's CurrentOrientation field
            bool response = TestRadioTelescopeController.MoveRadioTelescopeToOrientation( Orientation ).GetAwaiter().GetResult();
            // Call the GetCurrentOrientationMethod
            Orientation CurrentOrientation = TestRadioTelescopeController.GetCurrentOrientation();
            // Ensure the objects are identical
            Assert.IsTrue( response );
            Assert.AreEqual( Orientation.Azimuth , CurrentOrientation.Azimuth , 0.001 );
            Assert.AreEqual( Orientation.Elevation , CurrentOrientation.Elevation , 0.001 );


            Orientation = new Orientation( 28.0 , 42.0 );
            // Set the RadioTelescope's CurrentOrientation field
            response = TestRadioTelescopeController.MoveRadioTelescopeToOrientation( Orientation ).GetAwaiter().GetResult();
            // Call the GetCurrentOrientationMethod
            CurrentOrientation = TestRadioTelescopeController.GetCurrentOrientation();
            Assert.IsTrue( response);
            Assert.AreEqual( Orientation.Azimuth , CurrentOrientation.Azimuth , 0.001 );
            Assert.AreEqual( Orientation.Elevation , CurrentOrientation.Elevation , 0.001 );


            Orientation = new Orientation( 310.0 , 42.0 );
            // Set the RadioTelescope's CurrentOrientation field
            response = TestRadioTelescopeController.MoveRadioTelescopeToOrientation( Orientation ).GetAwaiter().GetResult();
            // Call the GetCurrentOrientationMethod
            CurrentOrientation = TestRadioTelescopeController.GetCurrentOrientation();
            // Ensure the objects are identical
            Assert.IsTrue( response );
            Assert.AreEqual( Orientation.Azimuth , CurrentOrientation.Azimuth , 0.001 );
            Assert.AreEqual( Orientation.Elevation , CurrentOrientation.Elevation , 0.001 );

        }

        [TestMethod]
        public void TestGetCurrentSafetyInterlockStatus() {
            Task.Delay( 1000 ).Wait();
            //estRTPLC.setSaftyInterlock();

            // Test the safety interlock status
            var response = TestRadioTelescopeController.GetCurrentSafetyInterlockStatus();

            // Make sure safety interlock is online
            Assert.IsTrue( response );
        }

        [TestMethod]
        public void TestCancelCurrentMoveCommand() {
            // Test canceling the the current move command
            TestRadioTelescopeController.MoveRadioTelescopeToOrientation( new Orientation( 0 , 0 ) );
            var response = TestRadioTelescopeController.CancelCurrentMoveCommand();

            // Make sure it was successful
            Assert.IsTrue( response );
        }

        [TestMethod]
        public void TestShutdownRadioTelescope() {
            // Call the ShutdownRadioTelescope method
            var response = TestRadioTelescopeController.ShutdownRadioTelescope();
            Orientation orientation = TestRadioTelescopeController.GetCurrentOrientation();

            // The Radio Telescope should now have a CurrentOrientation of (0, -90)
            Assert.IsTrue( response );
            Assert.AreEqual( 0.0, orientation.Azimuth, 0.001 );
            Assert.AreEqual( 90.0 , orientation.Elevation, 0.001 );
        }

        [TestMethod]
        public void TestThermalCalibrateRadioTelescope() {
            Orientation before = TestRadioTelescopeController.GetCurrentOrientation();

            // Call the CalibrateRadioTelescope method
            var response = TestRadioTelescopeController.ThermalCalibrateRadioTelescope();
            // this var response is going to be null because there is no reading. It means that the 
            // telescope isn't calibrated. At this time, all we care about is the movement

            // The Radio Telescope should now have a CurrentOrienation of what it had before the call
            Assert.AreEqual( TestRadioTelescopeController.RadioTelescope.CurrentOrientation.Azimuth , before.Azimuth );
            Assert.AreEqual( TestRadioTelescopeController.RadioTelescope.CurrentOrientation.Elevation , before.Elevation );
        }

        [TestMethod]
        public void TestMoveRadioTelescope_HardStops()
        {
            // Set the Telescope Type to HARD STOPS
            TestRTPLC.setTelescopeType(RadioTelescopeTypeEnum.HARD_STOPS);

            // Create an Orientation object with an azimuth of 311 and elevation of 42
            Orientation Orientation = new Orientation( 311.0 , 42.0 );

            // Set the RadioTelescope's CurrentOrientation field
            bool response = TestRadioTelescopeController.MoveRadioTelescopeToOrientation( Orientation ).GetAwaiter().GetResult();

            // Call the GetCurrentOrientationMethod
            Orientation CurrentOrientation = TestRadioTelescopeController.GetCurrentOrientation();

            // Ensure the objects are identical
            Assert.IsTrue( response );
            Assert.AreEqual( Orientation.Azimuth , CurrentOrientation.Azimuth , 0.001 );
            Assert.AreEqual( Orientation.Elevation , CurrentOrientation.Elevation , 0.001 );
        }

        [TestMethod]
        public void TestMoveRadioTelescope_SlipRing_311degrees()
        {
            // Set the Telescope Type to SLIP RING
            TestRTPLC.setTelescopeType(RadioTelescopeTypeEnum.SLIP_RING);

            // Create an Orientation object with an azimuth of 311 and elevation of 0
            Orientation TargetOrientation = new Orientation(311.0, 0);

            // Set the RadioTelescope's CurrentOrientation field
            bool response = TestRadioTelescopeController.MoveRadioTelescopeToOrientation(TargetOrientation).GetAwaiter().GetResult();

            // Call the GetCurrentOrientationMethod
            Orientation CurrentOrientation = TestRadioTelescopeController.GetCurrentOrientation();

            // Ensure the objects are identical
            Assert.IsTrue(response);
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
            bool response = TestRadioTelescopeController.MoveRadioTelescopeToOrientation(TargetOrientation).GetAwaiter().GetResult();

            // Call the GetCurrentOrientationMethod
            Orientation CurrentOrientation = TestRadioTelescopeController.GetCurrentOrientation();

            // Ensure the objects are identical
            Assert.IsTrue(response);
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
            TestRadioTelescopeController.MoveRadioTelescopeToOrientation(InitialOrientation).GetAwaiter().GetResult();

            // Move from 350 to 90
            Orientation TargetOrientation = new Orientation(90, 0);
            bool response = TestRadioTelescopeController.MoveRadioTelescopeToOrientation(TargetOrientation).GetAwaiter().GetResult();

            // Call the GetCurrentOrientationMethod
            Orientation CurrentOrientation = TestRadioTelescopeController.GetCurrentOrientation();

            // Ensure the objects are identical
            Assert.IsTrue(response);
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
            TestRadioTelescopeController.MoveRadioTelescopeToOrientation(InitialOrientation).GetAwaiter().GetResult();

            // Move from 90 to 350
            Orientation TargetOrientation = new Orientation(350, 0);
            bool response = TestRadioTelescopeController.MoveRadioTelescopeToOrientation(TargetOrientation).GetAwaiter().GetResult();

            // Call the GetCurrentOrientationMethod
            Orientation CurrentOrientation = TestRadioTelescopeController.GetCurrentOrientation();

            // Ensure the objects are identical
            Assert.IsTrue(response);
            Assert.AreEqual(TargetOrientation.Azimuth, CurrentOrientation.Azimuth, 0.001);
            Assert.AreEqual(TargetOrientation.Elevation, CurrentOrientation.Elevation, 0.001);
        }

        [TestMethod]
        public void test_conversions() {
            Random random = new Random();
            double degaz = 83.33, degel = 43.34;
            int az = ConversionHelper.DegreesToSteps( degaz , MotorConstants.GEARING_RATIO_AZIMUTH );
            double testaz = ConversionHelper.StepsToDegrees( az , MotorConstants.GEARING_RATIO_AZIMUTH );
            Assert.AreEqual( degaz , testaz , 0.001 );

            int el = ConversionHelper.DegreesToSteps( degel , MotorConstants.GEARING_RATIO_ELEVATION );
            double testel = ConversionHelper.StepsToDegrees( el , MotorConstants.GEARING_RATIO_ELEVATION );
            Assert.AreEqual( degel , testel , 0.001 );

            for(int i = 0; i < 360; i++) {
                double val = i + random.NextDouble();
                az = ConversionHelper.DegreesToSteps( val , MotorConstants.GEARING_RATIO_AZIMUTH );
                testaz = ConversionHelper.StepsToDegrees( az , MotorConstants.GEARING_RATIO_AZIMUTH );
                Assert.AreEqual( val , testaz , 0.001 );

                el = ConversionHelper.DegreesToSteps( val , MotorConstants.GEARING_RATIO_ELEVATION );
                testel = ConversionHelper.StepsToDegrees( el , MotorConstants.GEARING_RATIO_ELEVATION );
                Assert.AreEqual( val , testel , 0.001 );
            }

            double RPM = .5;
            int STEPSperSECOND = 83_333;

            int xpx =ConversionHelper.RPMToSPS( RPM , MotorConstants.GEARING_RATIO_AZIMUTH );
            Assert.AreEqual( STEPSperSECOND , xpx );

            double RPMOUT = ConversionHelper.SPSToRPM( xpx , MotorConstants.GEARING_RATIO_AZIMUTH );
            Assert.AreEqual( RPM , RPMOUT ,0.01);

            double dps = 3;
            xpx = ConversionHelper.DPSToSPS( dps , MotorConstants.GEARING_RATIO_AZIMUTH );
            Assert.AreEqual( STEPSperSECOND , xpx );

            double dpsOUT = ConversionHelper.SPSToDPS( xpx , MotorConstants.GEARING_RATIO_AZIMUTH );
            Assert.AreEqual( dps , dpsOUT , 0.01 );


        }

        [TestMethod]
        public void test_MCU_Time_Estimate() {
            int vel = 170_000, acc = 50, dist = 10_000_000;
            int time = MCUManager.estimateTime(vel, acc, dist);
            Assert.AreEqual(61800, time);
        }

        [TestMethod]
        public void testOrientationChange_100_100Degrees()
        {
            // Acquire current orientation
            Orientation currOrientation;
            currOrientation = TestRadioTelescopeController.RadioTelescope.PLCDriver.read_Position();

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
            TestRadioTelescopeController.RadioTelescope.PLCDriver.relative_move(100_000, 50, posTransAz, posTransEl);

            // Create result orientation
            Orientation resultOrientation = TestRadioTelescopeController.RadioTelescope.PLCDriver.read_Position();

            // Assert expected and result are identical
            Assert.AreEqual(expectedOrientation, resultOrientation);
        }

        [TestMethod]
        public void testOrientationChange_0_0Degrees()
        {
            // Acquire current orientation
            Orientation currOrientation;
            currOrientation = TestRadioTelescopeController.RadioTelescope.PLCDriver.read_Position();

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
            TestRadioTelescopeController.RadioTelescope.PLCDriver.relative_move(100_000, 50, posTransAz, posTransEl);

            // Create result orientation
            Orientation resultOrientation = TestRadioTelescopeController.RadioTelescope.PLCDriver.read_Position();

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
            currOrientation = TestRadioTelescopeController.RadioTelescope.PLCDriver.read_Position();

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
            TestRadioTelescopeController.RadioTelescope.PLCDriver.relative_move(100_000, 50, posTransAz, posTransEl);

            // Create result orientation
            Orientation resultOrientation = TestRadioTelescopeController.RadioTelescope.PLCDriver.read_Position();

            // Assert expected and result are identical
            Assert.AreEqual(expectedOrientation.Azimuth, resultOrientation.Azimuth, 0.001);
            Assert.AreEqual(expectedOrientation.Elevation, resultOrientation.Elevation, 0.001);
        }

        /*
        [TestMethod]
        public void test_orientation_change() {
            Random random = new Random();

            Orientation current_orientation2;
            current_orientation2 = TestRadioTelescopeController.RadioTelescope.PLCDriver.read_Position();
            Orientation current_orientation = current_orientation2;

            int positionTranslationAZ, positionTranslationEL, current_stepsAZ, current_stepsEL;
            current_stepsAZ = ConversionHelper.DegreesToSteps( current_orientation.Azimuth , MotorConstants.GEARING_RATIO_AZIMUTH );
            current_stepsEL = ConversionHelper.DegreesToSteps( current_orientation.Elevation , MotorConstants.GEARING_RATIO_ELEVATION );
            for(int i = 0; i < 100; i++) {
                Orientation target_orientation = new Orientation( random.NextDouble() * 360 , random.NextDouble() * 360 );

                positionTranslationAZ = ConversionHelper.DegreesToSteps( (target_orientation.Azimuth - current_orientation.Azimuth) , MotorConstants.GEARING_RATIO_AZIMUTH );
                positionTranslationEL = ConversionHelper.DegreesToSteps( (target_orientation.Elevation - current_orientation.Elevation) , MotorConstants.GEARING_RATIO_ELEVATION );


                Console.WriteLine( "AZ_01 {0,16} EL_01 {1,16}" , current_stepsAZ , current_stepsEL );
                current_stepsAZ = current_stepsAZ + positionTranslationAZ;
                current_stepsEL = current_stepsEL + positionTranslationEL;

                current_orientation = new Orientation( ConversionHelper.StepsToDegrees( current_stepsAZ , MotorConstants.GEARING_RATIO_AZIMUTH ) , ConversionHelper.StepsToDegrees( current_stepsEL , MotorConstants.GEARING_RATIO_ELEVATION ) );

                Assert.AreEqual( target_orientation.Azimuth , current_orientation.Azimuth , 0.1 );
                Assert.AreEqual( target_orientation.Elevation , current_orientation.Elevation , 0.1 );
                Console.WriteLine( "AZ_finni0 {0,10} EL_finni0 {1,10}" , current_stepsAZ , current_stepsEL );
                // current_orientation = new Orientation(ConversionHelper.StepsToDegrees(current_stepsAZ + positionTranslationAZ, MotorConstants.GEARING_RATIO_AZIMUTH), ConversionHelper.StepsToDegrees(current_stepsEL + positionTranslationEL, MotorConstants.GEARING_RATIO_ELEVATION));
                //Console.WriteLine(current_orientation.Elevation + "    "+ target_orientation.Elevation);
                // Console.WriteLine("AZ_step1 {0,10} EL_step1 {1,10}", positionTranslationAZ, positionTranslationEL);
                TestRadioTelescopeController.RadioTelescope.PLCDriver.relative_move( 100_000 , 50 , positionTranslationAZ , positionTranslationEL );

                current_orientation2 = TestRadioTelescopeController.RadioTelescope.PLCDriver.read_Position();

                Assert.AreEqual( target_orientation.Azimuth , current_orientation2.Azimuth , 0.1 );
                Assert.AreEqual( target_orientation.Elevation , current_orientation2.Elevation , 0.1 );
            }
        }
        */

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

            // Overheating
            az.temp = 100;
            Assert.IsFalse(TestRadioTelescopeController.checkTemp(az));

            // Stable
            az.temp = 50;
            Assert.IsTrue(TestRadioTelescopeController.checkTemp(az));

            // Overheating
            az.temp = 150;
            Assert.IsFalse(TestRadioTelescopeController.checkTemp(az));

            // Overheating
            az.temp = 151;
            Assert.IsFalse(TestRadioTelescopeController.checkTemp(az));

            // Too cold
            az.temp = 49;
            Assert.IsFalse(TestRadioTelescopeController.checkTemp(az));


            /** Elevation tests **/
            Temperature el = new Temperature(); el.location_ID = (int)SensorLocationEnum.EL_MOTOR;

            // Overheating
            el.temp = 100;
            Assert.IsFalse(TestRadioTelescopeController.checkTemp(el));

            // Stable
            el.temp = 50;
            Assert.IsTrue(TestRadioTelescopeController.checkTemp(el));

            // Overheating
            el.temp = 150;
            Assert.IsFalse(TestRadioTelescopeController.checkTemp(el));

            // Overheating
            el.temp = 151;
            Assert.IsFalse(TestRadioTelescopeController.checkTemp(el));

            // Too cold
            el.temp = 49;
            Assert.IsFalse(TestRadioTelescopeController.checkTemp(el));


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
            Assert.IsTrue(TestRadioTelescopeController.checkTemp(az));

            // Stable
            az.temp = 150;
            Assert.IsTrue(TestRadioTelescopeController.checkTemp(az));

            // Overheating
            az.temp = 151;
            Assert.IsTrue(TestRadioTelescopeController.checkTemp(az));

            // To cold
            az.temp = 49;
            Assert.IsTrue(TestRadioTelescopeController.checkTemp(az));


            /** Elevation temperatures **/
            Temperature el = new Temperature(); el.location_ID = (int)SensorLocationEnum.EL_MOTOR;

            // Stable
            el.temp = 50;
            Assert.IsTrue(TestRadioTelescopeController.checkTemp(el));

            // Stable
            el.temp = 150;
            Assert.IsTrue(TestRadioTelescopeController.checkTemp(el));

            // Overheating
            el.temp = 151;
            Assert.IsTrue(TestRadioTelescopeController.checkTemp(el));

            // To cold
            el.temp = 49;
            Assert.IsTrue(TestRadioTelescopeController.checkTemp(el));
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
    }
}

