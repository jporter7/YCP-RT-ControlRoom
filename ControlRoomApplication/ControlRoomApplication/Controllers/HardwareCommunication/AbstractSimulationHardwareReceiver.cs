namespace ControlRoomApplication.Controllers
{
    public abstract class AbstractSimulationHardwareReceiver
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public abstract bool StartReceiver();
        public abstract bool DisposeReceiver();
    }
}