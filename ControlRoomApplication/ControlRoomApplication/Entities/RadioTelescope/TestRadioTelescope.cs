using ControlRoomApplication.Controllers.PLCCommunication;
using ControlRoomApplication.Controllers.SpectraCyberController;

namespace ControlRoomApplication.Entities.RadioTelescope
{
    public class TestRadioTelescope : AbstractRadioTelescope
    {
        public TestRadioTelescope()
        {

        }

        public TestRadioTelescope(AbstractSpectraCyberController spectraCyberController, PLCCommunicationHandler plcController)
        {
            PlcController = plcController;
            SpectraCyberController = spectraCyberController;
            CalibrationOrientation = new Orientation();
            Status = RadioTelescopeStatusEnum.UNKNOWN;
            //CurrentOrientation = PlcController.GetOrientation();
        }
    }
}
