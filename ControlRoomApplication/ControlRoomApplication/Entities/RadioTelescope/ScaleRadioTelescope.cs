using ControlRoomApplication.Constants;
using ControlRoomApplication.Controllers.PLCController;
using ControlRoomApplication.Entities.Plc;

namespace ControlRoomApplication.Entities.RadioTelescope
{
    public class ScaleRadioTelescope : AbstractRadioTelescope
    {
        public ScaleRadioTelescope(PLCController plcController, Orientation currentOrientation)
        {
            PlcController = plcController;
            CalibrationOrientation = new Orientation();
            Status = RadioTelescopeStatusEnum.UNKNOWN;
            CurrentOrientation = currentOrientation;
        }

        public ScaleRadioTelescope()
        {
            Plc = new PLC();
            PlcController = new PLCController(Plc);
            CalibrationOrientation = new Orientation();
            Status = RadioTelescopeStatusEnum.UNKNOWN;
            CurrentOrientation = CalibrationOrientation;
        }
    }
}