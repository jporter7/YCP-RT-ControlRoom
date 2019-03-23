using System;
using System.Collections.Generic;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Entities.RadioTelescope;
using ControlRoomApplication.Database.Operations;
using ControlRoomApplication.Controllers.PLCCommunication;
using ControlRoomApplication.Controllers.SpectraCyberController;
using ControlRoomApplication.Simulators.Hardware.WeatherStation;

namespace ControlRoomApplication.Main
{
    public static class ConfigurationManager
    {
        private static int NumLocalDBRTInstancesCreated = 1;

        /// <summary>
        /// Configures the type of SpectraCyberController to be used in the rest of the 
        /// application based on the program argument passed in for it.
        /// </summary>
        /// <param name="argS"> The program argument passed in for the SpectraCyber. </param>
        /// <returns> A concrete instance of a SpectraCyberController. </returns>
        public static AbstractSpectraCyberController ConfigureSpectraCyberController(string argS)
        {
            switch (argS.ToUpper())
            {
                case "/PS":
                    return new SpectraCyberController(new SpectraCyber());

                case "/SS":
                    return new SpectraCyberSimulatorController(new SpectraCyberSimulator());

                case "/TS":
                    return new SpectraCyberTestController(new SpectraCyberSimulator());

                default:
                    // If none of the switches match or there wasn't one declared
                    // for the spectraCyber, assume we are using the simulated/testing one.
                    return new SpectraCyberSimulatorController(new SpectraCyberSimulator());
            }
        }

        /// <summary>
        /// Configures the type of WeatherStation to be used in the rest of the 
        /// application based on the program argument passed in for it.
        /// </summary>
        /// <param name="argW"> The program argument passed in for the weather station. </param>
        /// <returns> A concrete instance of a WeatherStation. </returns>
        public static AbstractWeatherStation ConfigureWeatherStation(string argW)
        {
            switch (argW.ToUpper())
            {
                case "/PW":
                    throw new NotImplementedException("The production weather station is not yet supported.");

                case "/SW":
                    return new SimulationWeatherStation(1000);

                default:
                case "/TW":
                    throw new NotImplementedException("The test weather station is not yet supported.");
            }
        }

        /// <summary>
        /// Configures the type of radiotelescope that will be used in the rest
        /// of the application based on the third command line argument passed in.
        /// </summary>
        /// <returns> A concrete instance of a radio telescope. </returns>
        public static RadioTelescope ConfigureRadioTelescope(AbstractSpectraCyberController spectraCyberController, string ip, int port, bool usingLocalDB)
        {
            PLCClientCommunicationHandler PLCCommsHandler = new PLCClientCommunicationHandler(ip, port);

            // Create Radio Telescope Location
            Location location = new Location(76.7046, 40.0244, 395.0); // John Rudy Park hardcoded for now

            // Return Radio Telescope
            if (usingLocalDB)
            {
                return new RadioTelescope(spectraCyberController, PLCCommsHandler, location, NumLocalDBRTInstancesCreated++);
            }
            else
            {
                return new RadioTelescope(spectraCyberController, PLCCommsHandler, location);
            }
        }

        /// <summary>
        /// Configures the type of PLC driver to simulate, based on the type of radio telescope being used.
        /// </summary>
        /// <returns> An instance of the proper derived PLC driver. </returns>
        public static AbstractPLCDriver ConfigureSimulatedPLCDriver(string argP, string ip, int port)
        {
            switch (argP.ToUpper())
            {
                case "/PR":
                    // The production telescope
                    throw new NotImplementedException("There is not yet communication for the real PLC.");

                case "/SR":
                    // Case for the test/simulated radiotelescope.
                    return new ScaleModelPLCDriver(ip, port);

                case "/TR":
                default:
                    // Should be changed once we have a simulated radiotelescope class implemented
                    return new TestPLCDriver(ip, port);
            }
        }

        /// <summary>
        /// Constructs a series of radio telescopes and its RF integrators, as well as a weather station, given a list of inputs.
        /// </summary>
        /// <returns> A list of built instances of a radio telescope and its derived PLC driver, as well as a weather station. </returns>
        public static (List<(RadioTelescope, AbstractPLCDriver)>, AbstractWeatherStation) BuildRadioTelescopeSeries(string[] args, bool usingLocalDB)
        {
            int NumRTs;
            try
            {
                NumRTs = int.Parse(args[0]);
            }
            catch (Exception)
            {
                Console.WriteLine("[ConfigurationManager] Invalid first input argument, an int was expected.");
                return (null, null);
            }

            if (NumRTs != args.Length - 2)
            {
                Console.WriteLine("[ConfigurationManager] The requested number of RTs doesn't match the number of input arguments.");
                return (null, null);
            }

            List<(RadioTelescope, AbstractPLCDriver)> RTDerivedDriverPairList = new List<(RadioTelescope, AbstractPLCDriver)>();
            for (int i = 0; i < NumRTs; i++)
            {
                string[] RTArgs = args[i + 2].Split(',');

                if (RTArgs.Length != 4)
                {
                    Console.WriteLine("[ConfigurationManager] Unexpected format for input #" + i.ToString() + " [" + args[i + 2] + "], skipping...");
                    continue;
                }

                string ip = RTArgs[2];
                int port = int.Parse(RTArgs[3]);

                AbstractPLCDriver GeneratedPLCDriver = ConfigureSimulatedPLCDriver(RTArgs[0], ip, port);
                RadioTelescope GeneratedRadioTelescope = ConfigureRadioTelescope(ConfigureSpectraCyberController(RTArgs[1]), ip, port, usingLocalDB);

                RTDerivedDriverPairList.Add((GeneratedRadioTelescope, GeneratedPLCDriver));
            }

            return (RTDerivedDriverPairList, ConfigureWeatherStation(args[1]));
        }

        public static void ConfigureLocalDatabase(int NumRTInstances)
        {
            DatabaseOperations.DeleteLocalDatabase();
            DatabaseOperations.PopulateLocalDatabase(NumRTInstances);
        }
    }
}
