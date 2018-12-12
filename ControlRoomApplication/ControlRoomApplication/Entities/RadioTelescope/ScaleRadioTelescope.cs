using ControlRoomApplication.Constants;
using ControlRoomApplication.Controllers.PLCController;
using ControlRoomApplication.Controllers.SpectraCyberController;
using ControlRoomApplication.Entities.Plc;

namespace ControlRoomApplication.Entities.RadioTelescope
{
    public class ScaleRadioTelescope : AbstractRadioTelescope
    {
        public ScaleRadioTelescope(PLCController plcController, SpectraCyberController spectraCyberController, Orientation currentOrientation)
        {
            PlcController = plcController;
            SpectraCyberController = spectraCyberController;
            CalibrationOrientation = new Orientation();
            Status = RadioTelescopeStatusEnum.UNKNOWN;
            CurrentOrientation = currentOrientation;
        }

        public ScaleRadioTelescope(PLCController PLCController) 
        {
            Plc = PLCController.Plc;
            PlcController = PLCController;
            CalibrationOrientation = new Orientation();
            Status = RadioTelescopeStatusEnum.UNKNOWN;
            CurrentOrientation = CalibrationOrientation;
        }

        public ScaleRadioTelescope()
        {
            Plc = new ScaleModelPLC();
            PlcController = new PLCController(Plc);
            CalibrationOrientation = new Orientation();
            Status = RadioTelescopeStatusEnum.UNKNOWN;
            CurrentOrientation = CalibrationOrientation;
        }
    }
}