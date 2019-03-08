using System;
using System.Collections.Generic;
using ControlRoomApplication.Controllers.PLCCommunication;
using ControlRoomApplication.Controllers.SpectraCyberController;
using ControlRoomApplication.Database.Operations;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Entities.RadioTelescope;

namespace ControlRoomApplication.Main
{
    public static class ConfigurationManager
    {
        /// <summary>
        /// Configures the type of SpectraCyberController to be used in the rest of the 
        /// application based on the second program argument passed in.
        /// </summary>
        /// <param name="arg1"> The second program argument passed in. </param>
        /// <param name="dbContext"> The database context </param>
        /// <returns> A concrete instance of a SpectraCyberController. </returns>
        public static AbstractSpectraCyberController ConfigureSpectraCyberController(string arg1, RTDbContext dbContext)
        {
            switch (arg1.ToUpper())
            {
                case "/PS":
                    return new SpectraCyberController(new SpectraCyber(), dbContext);

                case "/SS":
                    return new SpectraCyberSimulatorController(new SpectraCyberSimulator(), dbContext);

                case "/TS":
                    return new SpectraCyberTestController(new SpectraCyberSimulator(), dbContext);

                default:
                    // If none of the switches match or there wasn't one declared
                    // for the spectraCyber, assume we are using the simulated/testing one.
                    return new SpectraCyberSimulatorController(new SpectraCyberSimulator(), dbContext);
            }
        }

        /// <summary>
        /// Configures the type of radiotelescope that will be used in the rest
        /// of the application based on the third command line argument passed in.
        /// </summary>
        /// <param name="arg2"> The third program argument passed in. </param>
        /// <returns> A concrete instance of a radio telescope. </returns>
        public static AbstractRadioTelescope ConfigureRadioTelescope(string arg2, AbstractSpectraCyberController spectraCyberController, string ip, string port)
        {
            PLCCommunicationHandler PLCCommsHandler = new PLCCommunicationHandler(ip, int.Parse(port));
            switch (arg2.ToUpper())
            {
                case "/SR":
                    return new ScaleRadioTelescope(spectraCyberController, PLCCommsHandler);

                case "/PR":
                    return new ProductionRadioTelescope(spectraCyberController, PLCCommsHandler);

                case "/TR":
                    // Case for the test/simulated radiotelescope.
                    return new TestRadioTelescope(spectraCyberController, PLCCommsHandler);

                default:
                    // Should be changed once we have a simulated
                    // radiotelescope class implemented
                    return new ScaleRadioTelescope(spectraCyberController, PLCCommsHandler);
            }
        }

        /// <summary>
        /// Configures the type of PLC driver to simulate, based on the type of radio telescope being used.
        /// </summary>
        /// <returns> An instance of the proper PLC driver. </returns>
        public static AbstractPLCDriver ConfigureSimulatedPLCDriver(string arg2, string ip, string port)
        {
            switch (arg2.ToUpper())
            {
                case "/PR":
                    // The production telescope
                    return null;

                case "/TR":
                    // Case for the test/simulated radiotelescope.
                    throw new NotImplementedException("There is not yet a Test/Simulation PLC.");

                case "/SR":
                default:
                    // Should be changed once we have a simulated
                    // radiotelescope class implemented
                    return new ScaleModelPLCDriver(ip, int.Parse(port));
            }
        }

        /// <summary>
        /// Constructs a series of radio telescopes, given input parameters.
        /// </summary>
        /// <returns> A list of built instances of a radio telescope. </returns>
        public static List<KeyValuePair<AbstractRadioTelescope, AbstractPLCDriver>> BuildRadioTelescopeSeries(string[] args, RTDbContext dbContext)
        {
            int NumRTs;
            try
            {
                NumRTs = int.Parse(args[0]);
            }
            catch(Exception)
            {
                Console.WriteLine("Invalid first input argument, an int was expected.");
                return null;
            }

            if (NumRTs != args.Length- 1)
            {
                Console.WriteLine("The requested number of RTs doesn't match the number of input arguments.");
                return null;
            }

            List<KeyValuePair<AbstractRadioTelescope, AbstractPLCDriver>> RTDriverPairList = new List<KeyValuePair<AbstractRadioTelescope, AbstractPLCDriver>>(NumRTs);
            for (int i = 0; i < NumRTs; i++)
            {
                string[] RTArgs = args[i + 1].Split(',');

                if (RTArgs.Length != 3)
                {
                    Console.WriteLine("Unexpected format for input #" + i.ToString() + "[" + args[i + 1] + "], skipping...");
                    continue;
                }
                
                AbstractRadioTelescope ARadioTelescope = ConfigureRadioTelescope(RTArgs[0], ConfigureSpectraCyberController(RTArgs[0], dbContext), RTArgs[1], RTArgs[2]);
                AbstractPLCDriver APLCDriver = ConfigureSimulatedPLCDriver(RTArgs[0], RTArgs[1], RTArgs[2]);

                RTDriverPairList.Add(new KeyValuePair<AbstractRadioTelescope, AbstractPLCDriver>(ARadioTelescope, APLCDriver));
            }

            return RTDriverPairList;
        }

        public static void ConfigureLocalDatabase()
        {
            DatabaseOperations.InitializeLocalConnectionOnly();
            DatabaseOperations.PopulateLocalDatabase();
        }
    }
}
