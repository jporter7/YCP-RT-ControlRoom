using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Entities.PLC;

namespace ControlRoomApplication.Controllers.PLCController
{
    public class PLCController
    {
        public PLCController()
        {

        }

        /// <summary>
        /// Method that will send commands to the plc to calibrate the telescope.
        /// </summary>
        /// <param name="plc"> The plc that the commands should be sent to. </param>
        /// <returns> A string representing the state of the operation. </returns>
        public string CalibrateRT(PLC plc)
        {
            return plc.SendMessage(PLCConstants.CALIBRATE);
        }

        /// <summary>
        /// Tells the specified PLC to shutdown.
        /// </summary>
        /// <param name="plc"> The PLC to communicate with.</param>
        /// <returns> A string representing the state of the operation. </returns>
        public string ShutdownRT(PLC plc)
        {
            return plc.SendMessage(PLCConstants.SHUTDOWN);
        }

        /// <summary>
        /// Tells the specified PLC to move the radiotelescope to the specified azimuth.
        /// </summary>
        /// <param name="plc"> The PLC to communicate with. </param>
        /// <param name="azimuth"> The azimuth that the PLC should move the radiotelescope to. </param>
        /// <returns> A string that indicates the state of the operation. </returns>
        public string MoveTelescope(PLC plc, Coordinate coordinate) //long azimuthOffset)
        {
            if (coordinate.rightAscension < PLCConstants.RIGHT_ASCENSION_LOWER_LIMIT || coordinate.rightAscension > PLCConstants.RIGHT_ASCENSION_UPPER_LIMIT)
            {
                throw new System.Exception();
            }
            else if (coordinate.declination < PLCConstants.DECLINATION_LOWER_LIMIT || coordinate.declination > PLCConstants.DECLINATION_UPPER_LIMIT)
            {
                throw new System.Exception();
            }

            return plc.SendMessage($"right_ascension {coordinate.rightAscension}, declination {coordinate.declination}");
        }

        /// <summary>
        /// Method to move the scale model to a certain set of coordinates.
        /// </summary>
        /// <param name="plc"> The plc that the commands should be sent to. </param>
        /// <param name="coordinate"> A set of coordinates that the telescope should be moved to.</param>
        public void MoveScaleModel(PLC plc, Coordinate coordinate)
        {
            if (coordinate.rightAscension < PLCConstants.RIGHT_ASCENSION_LOWER_LIMIT || coordinate.rightAscension > PLCConstants.RIGHT_ASCENSION_UPPER_LIMIT)
            {
                throw new System.Exception();
            }
            else if (coordinate.declination < PLCConstants.DECLINATION_LOWER_LIMIT || coordinate.declination > PLCConstants.DECLINATION_UPPER_LIMIT)
            {
                throw new System.Exception();
            }

            plc.MoveScaleModelAzimuth((int)coordinate.rightAscension);
            plc.MoveScaleModelElevation((int)coordinate.declination);
        }

        public PLC Plc { get; set; }
    }
}
