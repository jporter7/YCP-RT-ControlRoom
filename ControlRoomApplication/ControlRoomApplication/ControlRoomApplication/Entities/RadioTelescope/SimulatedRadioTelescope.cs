using ControlRoomApplication.Controllers.SpectraCyberController;

namespace ControlRoomApplication.Entities
{
    public class SimulatedTelescope : AbstractRadioTelescope
    {
        public SimulatedTelescope(AbstractSpectraCyberController spectraCyberController) : base(spectraCyberController)
        {
            // Nothing extra at the moment
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
