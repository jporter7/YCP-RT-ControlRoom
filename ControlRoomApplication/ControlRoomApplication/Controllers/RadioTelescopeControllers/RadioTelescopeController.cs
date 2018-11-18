using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Entities.RadioTelescope;

namespace ControlRoomApplication.Controllers.RadioTelescopeControllers
{
    public class RadioTelescopeController
    {
        public RadioTelescopeController()
        {

        }

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

        public void CalibrateRadioTelescope()
        {
            // Move the telescope to the orientation that it is supposed to calibrate at
            RadioTelescope.Status = RadioTelescopeStatusEnum.RUNNING;

            Orientation orientation = new Orientation();

            RadioTelescope.PlcController.MoveScaleModel(RadioTelescope.Plc, PLCConstants.COM3);
            RadioTelescope.PlcController.MoveScaleModel(RadioTelescope.Plc, PLCConstants.COM4);

            RadioTelescope.Status = RadioTelescopeStatusEnum.IDLE;
        }

        public AbstractRadioTelescope RadioTelescope { get; set; }
    }
}