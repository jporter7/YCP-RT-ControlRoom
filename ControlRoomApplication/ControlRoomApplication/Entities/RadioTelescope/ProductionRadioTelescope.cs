using ControlRoomApplication.Controllers.SpectraCyberController;

namespace ControlRoomApplication.Entities.RadioTelescope
{
    public class ProductionRadioTelescope : AbstractRadioTelescope
    {
        private AbstractSpectraCyberController FTSpectraCyberController;

        public ProductionRadioTelescope()
        {
            FTSpectraCyberController = new SpectraCyberController(new SpectraCyber());
            FTSpectraCyberController.BringUp();
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
