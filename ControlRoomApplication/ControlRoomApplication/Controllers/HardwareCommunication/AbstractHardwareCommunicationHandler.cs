using ControlRoomApplication.Entities;

namespace ControlRoomApplication.Controllers
{
    public abstract class AbstractHardwareCommunicationHandler : HeartbeatInterface
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public abstract bool StartHandler();
        public abstract bool DisposeHandler();

        public abstract bool TestCommunication();

        public abstract Orientation GetCurrentOrientation();

        public abstract bool[] GetCurrentLimitSwitchStatuses();

        public abstract bool GetCurrentSafetyInterlockStatus();

        public abstract bool CancelCurrentMoveCommand();

        public abstract bool ShutdownRadioTelescope();

        public abstract bool CalibrateRadioTelescope();

        public abstract bool ConfigureRadioTelescope(double startSpeedDPSAzimuth, double startSpeedDPSElevation, int homeTimeoutSecondsAzimuth, int homeTimeoutSecondsElevation);

        public abstract bool MoveRadioTelescopeToOrientation(Orientation orientation);

        public abstract bool StartRadioTelescopeJog(RadioTelescopeAxisEnum axis, double speedDPS, bool clockwise);

        public abstract bool ExecuteRadioTelescopeControlledStop();

        public abstract bool ExecuteRadioTelescopeImmediateStop();

        public abstract bool ExecuteRelativeMove(double speedDPSAzimuth, double speedDPSElevation, double translationDegreesAzimuth, double translationDegreesElevation);

        protected override bool TestIfComponentIsAlive()
        {
            return TestCommunication();
        }

        protected override bool KillHeartbeatComponent()
        {
            return DisposeHandler();
        }
    }
}