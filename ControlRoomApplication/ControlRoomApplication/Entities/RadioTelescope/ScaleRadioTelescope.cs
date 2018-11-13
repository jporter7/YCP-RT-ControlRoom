using System.ComponentModel.DataAnnotations.Schema;
using ControlRoomApplication.Controllers.PLCController;
using ControlRoomApplication.Entities.Plc;

namespace ControlRoomApplication.Entities.RadioTelescope
{
    public class ScaleRadioTelescope : AbstractRadioTelescope
    {
        public ScaleRadioTelescope(PLC plc, PLCController plcController, Orientation currentOrientation)
        {
            Plc = plc;
            PlcController = plcController;
            CalibrationOrientation = new Orientation();
            Status = RadioTelescopeStatusEnum.UNKNOWN;
            CurrentOrientation = currentOrientation;
        }

        public ScaleRadioTelescope()
        {

        }

        /// <summary>
        /// Gets the current orientation of the radiotelescope in azimuth and elevation.
        /// </summary>
        /// <returns> An orientation object that holds the current azimuth/elevation of the scale model. </returns>
        public override Orientation GetCurrentOrientation()
        {
            throw new System.NotImplementedException();
        }

        public override void ShutdownRadioTelescope()
        {
            throw new System.NotImplementedException();
        }

        public override void MoveRadioTelescope(Orientation orientation)
        {
            // Move the telescope to the orientation that it is supposed to be at
            Status = RadioTelescopeStatusEnum.RUNNING;
            PlcController.MoveScaleModel(Plc, orientation);
            Status = RadioTelescopeStatusEnum.IDLE;
        }

        public void CalibrateRadioTelescope()
        {
            // Move the telescope to the orientation that it is supposed to calibrate at
            Status = RadioTelescopeStatusEnum.RUNNING;
            PlcController.MoveScaleModel(Plc, CalibrationOrientation);
            CurrentOrientation = CalibrationOrientation;
            Status = RadioTelescopeStatusEnum.IDLE;
        }

        public PLC Plc { get; set; }
        public PLCController PlcController { get; set; }
        public Orientation CalibrationOrientation { get; }
    }
}