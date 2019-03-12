using ControlRoomApplication.Controllers.PLCCommunication;
using ControlRoomApplication.Controllers.SpectraCyberController;

namespace ControlRoomApplication.Entities.RadioTelescope
{
    public class ScaleRadioTelescope : AbstractRadioTelescope
    {
        public ScaleRadioTelescope(AbstractSpectraCyberController spectraCyberController, PLCClientCommunicationHandler plcController)
        {
            PlcController = plcController;
            SpectraCyberController = spectraCyberController;
            CalibrationOrientation = new Orientation();
            //Status = RadioTelescopeStatusEnum.UNKNOWN;
            //CurrentOrientation = PlcController.GetOrientation();
        }
    }
}