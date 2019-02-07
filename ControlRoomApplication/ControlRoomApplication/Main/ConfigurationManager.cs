using ControlRoomApplication.Entities;
using ControlRoomApplication.Entities.Plc;
using ControlRoomApplication.Entities.RadioTelescope;

namespace ControlRoomApplication.Main
{
    public class ConfigurationManager
    {
        public ConfigurationManager()
        {

        }

        /// <summary>
        /// Configures the type of PLC to be used in the rest of the application based on
        /// the first program argument passed in.
        /// </summary>
        /// <param name="arg"> The first program argument passed in. </param>
        /// <returns> A concrete instance of a PLC. </returns>
        public AbstractPLC ConfigurePLC(string arg0)
        {
            switch(arg0.Substring(0, 3).ToUpper())
            {
                case "/SP":
                    return new ScaleModelPLC();
               
                // Will be used later on and should be uncommented then
                //case "/PP":
                    //return new ProductionPLC();

                case "/TP":
                    return new TestPLC();

                case "/VP":
                    return new VRPLC();

                default:
                    // If none of the switches match or there wasn't one declared
                    // for the PLC, assume we are using the simulated/testing one.
                    return new TestPLC();
            }
        }

        /// <summary>
        /// Configures the type of SpectraCyber to be used in the rest of the 
        /// application based on the second program argument passed in.
        /// </summary>
        /// <param name="arg1"> The second program argument passed in. </param>
        /// <returns> A concrete instance of a SpectraCyber. </returns>
        public AbstractSpectraCyber ConfigureSpectraCyber(string arg1)
        {
            switch(arg1.Substring(0, 3).ToUpper())
            {
                case "/PS":
                    return new SpectraCyber();

                case "/SS":
                    return new SpectraCyberSimulator();

                default:
                    // If none of the switches match or there wasn't one declared
                    // for the PLC, assume we are using the simulated/testing one.
                    return new SpectraCyberSimulator();
            }
        }

        /// <summary>
        /// Configures the type of radiotelescope that will be used in the rest
        /// of the application based on the third command line argument passed in.
        /// </summary>
        /// <param name="arg2"> The third program argument passed in. </param>
        /// <returns> A concrete instance of a radio telescope. </returns>
        public AbstractRadioTelescope ConfigureRadioTelescope(string arg2)
        {
            switch(arg2.Substring(0, 3).ToUpper())
            {
                case "/SR":
                    return new ScaleRadioTelescope();

                case "/PR":
                    return new ProductionRadioTelescope();

                case "/TR":
                    // Case for the test/simulated radiotelescope.
                    // Will need to be uncommented later.
                    // return new TestRadioTelescope();
                default:
                    // Should be changed once we have a simulated
                    // radiotelescope class implemented
                    return new ScaleRadioTelescope();
            }
        }
    }
}
