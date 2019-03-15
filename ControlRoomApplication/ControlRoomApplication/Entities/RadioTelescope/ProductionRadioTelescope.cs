using ControlRoomApplication.Controllers.PLCCommunication;
using ControlRoomApplication.Controllers.SpectraCyberController;

namespace ControlRoomApplication.Entities.RadioTelescope
{
    public class ProductionRadioTelescope : AbstractRadioTelescope
    {
        public ProductionRadioTelescope(AbstractSpectraCyberController spectraCyberController, PLCClientCommunicationHandler plcController, Location location)
        {
            PlcController = plcController;
            SpectraCyberController = spectraCyberController;
            CalibrationOrientation = new Orientation();
            Status = RadioTelescopeStatusEnum.UNKNOWN;
            Location = location;
        }

        public Orientation GetCurrentOrientation()
        {
            throw new System.NotImplementedException();
        }

        public bool SendReferenceVelocityCommand(double velocityAzimuth, double velocityElevation)
        {
            throw new System.NotImplementedException();
        }

        public void ShutdownRadioTelescope()
        {
            throw new System.NotImplementedException();
        }

        public void MoveRadioTelescope(Orientation orientation)
        {
            throw new System.NotImplementedException();
        }
    }
}
