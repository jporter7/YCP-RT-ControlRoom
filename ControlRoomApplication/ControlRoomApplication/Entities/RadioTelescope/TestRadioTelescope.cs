using ControlRoomApplication.Constants;
using ControlRoomApplication.Controllers.PLCController;
using ControlRoomApplication.Controllers.SpectraCyberController;
using ControlRoomApplication.Entities.Plc;
using ControlRoomApplication.Main;

namespace ControlRoomApplication.Entities.RadioTelescope
{
    public class TestRadioTelescope : AbstractRadioTelescope
    {
        public TestRadioTelescope()
        {

        }

        public TestRadioTelescope(AbstractSpectraCyberController spectraCyberController, PLCController plcController)
        {
            PlcController = plcController;
            SpectraCyberController = spectraCyberController;
            CalibrationOrientation = new Orientation();
            Status = RadioTelescopeStatusEnum.UNKNOWN;
            CurrentOrientation = PlcController.GetOrientation();
        }
    }
}
