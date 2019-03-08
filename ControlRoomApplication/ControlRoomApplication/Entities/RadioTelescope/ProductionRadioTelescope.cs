using ControlRoomApplication.Controllers.PLCController;
using ControlRoomApplication.Controllers.SpectraCyberController;
using ControlRoomApplication.Main;

namespace ControlRoomApplication.Entities.RadioTelescope
{
    public class ProductionRadioTelescope : AbstractRadioTelescope
    {
        public ProductionRadioTelescope()
        {
            
        }

        public ProductionRadioTelescope(AbstractSpectraCyberController spectraCyberController, PLCController plcController)
        {
            PlcController = plcController;
            SpectraCyberController = spectraCyberController;
            CalibrationOrientation = new Orientation();
            Status = RadioTelescopeStatusEnum.UNKNOWN;
            CurrentOrientation = PlcController.GetOrientation();
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