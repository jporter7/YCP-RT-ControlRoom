using ControlRoomApplication.Constants;
using ControlRoomApplication.Controllers.PLCController;
using ControlRoomApplication.Controllers.SpectraCyberController;
using ControlRoomApplication.Entities.Plc;
using ControlRoomApplication.Main;

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
            PlcController = PLCController;
            var context = new RTDbContext();
            var spectraCyber = new SpectraCyber();
            SpectraCyberController = new SpectraCyberController(spectraCyber, context);
            CalibrationOrientation = new Orientation();
            Status = RadioTelescopeStatusEnum.UNKNOWN;
            CurrentOrientation = CalibrationOrientation;
        }

        public ScaleRadioTelescope()
        {
            PlcController = new PLCController(new ScaleModelPLC());
            var context = new RTDbContext();
            var spectraCyber = new SpectraCyber();
            SpectraCyberController = new SpectraCyberController(spectraCyber, context);
            CalibrationOrientation = new Orientation();
            Status = RadioTelescopeStatusEnum.UNKNOWN;
            CurrentOrientation = CalibrationOrientation;
        }
    }
}