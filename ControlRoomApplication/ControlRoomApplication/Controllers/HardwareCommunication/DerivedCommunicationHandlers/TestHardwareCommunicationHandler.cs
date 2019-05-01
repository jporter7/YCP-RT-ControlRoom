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

        public override bool ConfigureRadioTelescope(double startSpeedAzimuth, double startSpeedElevation, int homeTimeoutAzimuth, int homeTimeoutElevation)
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

        public override bool ExecuteRelativeMove(RadioTelescopeAxisEnum axis, double speed, double position)
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

        public override bool StartRadioTelescopeJog(RadioTelescopeAxisEnum axis, double speed, bool clockwise)
        {
            return false;
        }

        public override bool TestCommunication()
        {
            return true;
        }
    }
}
