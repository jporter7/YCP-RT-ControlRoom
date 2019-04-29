using ControlRoomApplication.Entities;

namespace ControlRoomApplication.Controllers
{
    public abstract class AbstractHardwareCommunicationHandler : AbstractHardwareCommunicationHandlerBase
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public abstract void SendMessage(HardwareMessageTypeEnum MessageType, params object[] MessageParameters);
        public abstract byte[] ReadResponse();
        public abstract byte[] SendMessageAndReadResponse(HardwareMessageTypeEnum MessageType, params object[] MessageParameters);
    }
}