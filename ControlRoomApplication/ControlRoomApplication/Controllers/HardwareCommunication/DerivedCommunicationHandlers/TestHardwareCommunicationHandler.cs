using ControlRoomApplication.Entities;

namespace ControlRoomApplication.Controllers
{
    public class TestHardwarCommunicationHandler : AbstractHardwareCommunicationHandler
    {
        public TestHardwarCommunicationHandler()
        {
            // Does nothing
        }

        public override bool StartHandler()
        {
            return true;
        }

        public override bool DisposeHandler()
        {
            return true;
        }

        public override bool CalibrateRadioTelescope()
        {
            return true;
        }

        public override bool CancelCurrentMoveCommand()
        {
            return true;
        }

        public override bool ConfigureRadioTelescope(double startSpeedDPSAzimuth, double startSpeedDPSElevation, int homeTimeoutSecondsAzimuth, int homeTimeoutSecondsElevation)
        {
            return true;
        }

        public override bool ExecuteRadioTelescopeControlledStop()
        {
            return true;
        }

        public override bool ExecuteRadioTelescopeImmediateStop()
        {
            return true;
        }

        public override bool ExecuteRelativeMove(double speedDPSAzimuth, double positionDegreesAzimuth, double speedDPSElevation, double positionDegreesElevation)
        {
            return false;
        }

        public override bool[] GetCurrentLimitSwitchStatuses()
        {
            return new bool[] { true, true, false, true };
        }

        public override Orientation GetCurrentOrientation()
        {
            return new Orientation(45, 42.18);
        }

        public override bool GetCurrentSafetyInterlockStatus()
        {
            return true;
        }

        public override bool MoveRadioTelescopeToOrientation(Orientation orientation)
        {
            return false;
        }

        public override bool ShutdownRadioTelescope()
        {
            return true;
        }

        public override bool StartRadioTelescopeJog(RadioTelescopeAxisEnum axis, double speedDPS, bool clockwise)
        {
            return false;
        }

        public override bool TestCommunication()
        {
            return true;
        }
    }
}
