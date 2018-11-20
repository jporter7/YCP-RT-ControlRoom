using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Entities.RadioTelescope;

namespace ControlRoomApplication.Controllers.RadioTelescopeControllers
{
    public class RadioTelescopeController
    {
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public RadioTelescopeController()
        {

        }

        /// <summary>
        /// Constructor that takes an AbstractRadioTelescope object and sets the
        /// corresponding field
        /// </summary>
        /// <param name="radioTelescope"></param>
        public RadioTelescopeController(AbstractRadioTelescope radioTelescope)
        {
            RadioTelescope = radioTelescope;
        }

        /// <summary>
        /// Gets the current orientation of the radiotelescope in azimuth and elevation.
        /// </summary>
        /// <returns> An orientation object that holds the current azimuth/elevation of the scale model. </returns>
        public Orientation GetCurrentOrientation()
        {
            return RadioTelescope.CurrentOrientation;
        }

        public void ShutdownRadioTelescope()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Method to move the Radio Telescope, which behaves differently based on the type
        /// of Radio Telescope in question. Currently, only the ScaleRadioTelescope scenario
        /// is designed with functionality, wherein it will use the PLCController to send 
        /// movement information to the scale model
        /// </summary>
        /// <param name="orientation"></param>
        public void MoveRadioTelescope(Orientation orientation)
        {
            // Switch based on the type of radiotelescope that is being controlled by this controller.
            switch(RadioTelescope)
            {
                case ScaleRadioTelescope scale:
                    // Move the telescope to the orientation that it is supposed to be at
                    scale.Status = RadioTelescopeStatusEnum.RUNNING;

                    scale.Plc.OutgoingOrientation = orientation;
                    scale.PlcController.MoveScaleModel(RadioTelescope.Plc, PLCConstants.COM3);
                    scale.PlcController.MoveScaleModel(RadioTelescope.Plc, PLCConstants.COM4);
                    scale.CurrentOrientation = orientation;

                    scale.Status = RadioTelescopeStatusEnum.IDLE;
                    break;

                case ProductionRadioTelescope prod:
                    // Add Code for production radiotelescope later

                default:

                    break;
            }
        }

        /// <summary>
        /// Method used to calibrate the Radio Telescope before each observation. It, like the 
        /// MoveRadioTelescope method, is designed to function differently based on the type of
        /// Radio Telescope in question. Currently, only the ScaleRadioTelescope scenario is designed 
        /// with functionality, wherein it will set the Orientation to (0,0)
        /// </summary>
        public void CalibrateRadioTelescope()
        {
            switch(RadioTelescope)
            {
                case ScaleRadioTelescope scale:
                    // Move the telescope to the orientation that it is supposed to calibrate at
                    scale.Status = RadioTelescopeStatusEnum.RUNNING;

                    // For the Scale Model, just align the orientation at (0, 0)
                    Orientation orientation = new Orientation();

                    scale.PlcController.MoveScaleModel(RadioTelescope.Plc, PLCConstants.COM3);
                    scale.PlcController.MoveScaleModel(RadioTelescope.Plc, PLCConstants.COM4);

                    scale.Status = RadioTelescopeStatusEnum.IDLE;
                    break;
                case ProductionRadioTelescope prod:
                    // Add Code for production radio-telescope later
                    break;
                default:
                    break;
            }
            
        }

        public AbstractRadioTelescope RadioTelescope { get; set; }
    }
}