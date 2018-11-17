using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Entities.Plc;
using Newtonsoft.Json;
using System;
using System.Threading;

namespace ControlRoomApplication.Controllers.PLCController
{
    public class PLCController
    {
        public PLCController()
        {

        }

        public PLCController(PLC plc)
        {
            Plc = plc;
        }

        /// <summary>
        /// Method that will send commands to the plc to calibrate the telescope.
        /// </summary>
        /// <param name="plc"> The plc that the commands should be sent to. </param>
        /// <returns> A string representing the state of the operation. </returns>
        public string CalibrateRT(PLC plc)
        {
            PlcConnector.WriteMessage(PLCConstants.CALIBRATE);
            return PlcConnector.ReceiveMessage();
        }

        /// <summary>
        /// Tells the specified PLC to shutdown.
        /// </summary>
        /// <param name="plc"> The PLC to communicate with.</param>
        /// <returns> A string representing the state of the operation. </returns>
        public string ShutdownRT(PLC plc)
        {
            PlcConnector.WriteMessage(PLCConstants.SHUTDOWN);
            return PlcConnector.ReceiveMessage();
        }

        /// <summary>
        /// Tells the specified PLC to move the radiotelescope to the specified azimuth.
        /// </summary>
        /// <param name="plc"> The PLC to communicate with. </param>
        /// <param name="azimuth"> The azimuth that the PLC should move the radiotelescope to. </param>
        /// <returns> A string that indicates the state of the operation. </returns>
        public string MoveTelescope(PLC plc, Coordinate coordinate) //long azimuthOffset)
        {
            if (coordinate.RightAscension < PLCConstants.RIGHT_ASCENSION_LOWER_LIMIT || coordinate.RightAscension > PLCConstants.RIGHT_ASCENSION_UPPER_LIMIT)
            {
                throw new System.Exception();
            }
            else if (coordinate.Declination < PLCConstants.DECLINATION_LOWER_LIMIT || coordinate.Declination > PLCConstants.DECLINATION_UPPER_LIMIT)
            {
                throw new System.Exception();
            }

            PlcConnector.WriteMessage($"right_ascension {coordinate.RightAscension}, declination {coordinate.Declination}");
            return PlcConnector.ReceiveMessage();
        }

        /// <summary>
        /// Method to move the scale model to a certain set of coordinates.
        /// </summary>
        /// <param name="plc"> The plc that the commands should be sent to. </param>
        /// <param name="coordinate"> A set of coordinates that the telescope should be moved to.</param>
        public string MoveScaleModel(PLC plc, string comPort)
        {
            if (plc.OutgoingOrientation.Azimuth < PLCConstants.RIGHT_ASCENSION_LOWER_LIMIT || plc.OutgoingOrientation.Azimuth > PLCConstants.RIGHT_ASCENSION_UPPER_LIMIT)
            {
                throw new System.Exception();
            }
            else if (plc.OutgoingOrientation.Elevation < PLCConstants.DECLINATION_LOWER_LIMIT || plc.OutgoingOrientation.Elevation > PLCConstants.DECLINATION_UPPER_LIMIT)
            {
                throw new System.Exception();
            }

            // Convert orientation object to a json string
            string jsonOrientation = JsonConvert.SerializeObject(plc.OutgoingOrientation);

            // Move the scale model's azimuth motor on com3 and its elevation on com4
            // make sure there is a delay in this thread for enough time to have the arduino
            // move the first motor (azimuth)
            PlcConnector = new PLCConnector(comPort);
            PlcConnector.SendSerialPortMessage(jsonOrientation);
            Thread.Sleep(5000);

            // Wait for the arduinos to send back a response 
            // in the arduino code, as of milestone 2, the response is a string: "finished"
            var state = string.Empty;
            while (state == string.Empty)
            {
                state = PlcConnector.GetSerialPortMessage();
            }

            // Print the state of the move operation to the console.
            Console.WriteLine(state);

            return state;
        }

        public PLC Plc { get; set; }
        public PLCConnector PlcConnector { get; set; }
    }
}