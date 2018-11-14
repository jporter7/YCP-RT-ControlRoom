using ControlRoomApplication.Controllers.SpectraCyberController;

namespace ControlRoomApplication.Entities
{
    public class FullTelescope : AbstractRadioTelescope
    {
        public FullTelescope(AbstractSpectraCyberController spectraCyberController) : base(spectraCyberController)
        {
            
        }

        public override Orientation GetCurrentReferenceOrientation()
        {
            throw new System.NotImplementedException();
        }

        public override bool SendReferenceVelocityCommand(double velocityAzimuth, double velocityElevation)
        {
            throw new System.NotImplementedException();
        }
    }
}
