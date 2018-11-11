using ControlRoomApplication.Entities.PLC;

namespace ControlRoomApplication.Controllers.PLCController
{
    public class PLCController
    {
        public PLCController()
        {

        }

        public string CalibrateRT(PLC plc)
        {
            return plc.SendMessage("calibrate");
        }

        /// <summary>
        /// Tells the specified PLC to shutdown.
        /// </summary>
        /// <param name="plc"> The PLC to communicate with.</param>
        /// <returns></returns>
        public string ShutdownRT(PLC plc)
        {
            return plc.SendMessage("shutdown");
        }

        /// <summary>
        /// Tells the specified PLC to move the radiotelescope to the specified azimuth.
        /// </summary>
        /// <param name="plc"> The PLC to communicate with. </param>
        /// <param name="azimuth"> The azimuth that the PLC should move the radiotelescope to. </param>
        /// <returns> A string that indicates the state of the operation. </returns>
        public string MoveTelescope(PLC plc, double azimuth) //long azimuthOffset)
        {
            if (azimuth < 0 || azimuth > 359.99)
            {
                return "ERROR";
            }            

            return plc.SendMessage($"azimuth {azimuth}");
        }
    }
}
