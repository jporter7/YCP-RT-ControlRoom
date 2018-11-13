using ControlRoomApplication.Constants;
using ControlRoomApplication.Controllers.PLCController;
using System;

namespace ControlRoomApplication.Entities.Plc
{
    public class PLC
    {
        /// <summary>
        /// Constructor for a PLC object.
        /// </summary>
        /// <param name="ip"> The IP address of the PLC to connect to. </param>
        /// <param name="port"> The port that the PLC should be connected through. </param>
        public PLC(string ip, int port)
        {
            PLCConnector = new PLCConnector(ip, port);
        }

        /// <summary>
        /// Empty constructor for the PLC object.
        /// </summary>
        public PLC()
        {

        }

        /// <summary>
        /// Sends a message over TCP connection.
        /// </summary>
        /// <param name="message"> The message to be sent over TCP connection. </param>
        /// <returns> A message indicating the state of the operation.</returns>
        public string SendMessage(string message)
        {
            PLCConnector.WriteMessage(message);

            return GetMessage();
        }

        /// <summary>
        /// Method to receive a message over TCP connection from the PLC we're connected to.
        /// </summary>
        /// <returns> A string that represents the state of the operation. </returns>
        public string GetMessage()
        {
            // I have this inside the while loop because I believe that
            // we will need to perform the call multiple times while the PLC 
            // software/fhardware perform their tasks so this is a form of waiting.
            string result = null;
            while(result == null)
            {
                result = PLCConnector.ReceiveMessage();
            }

            return result;
        }

        /// <summary>
        /// Method to move the azimuth of the scale model.
        /// </summary>
        /// <param name="azimuth"> The azimuth in degrees that the scale model should move to. </param>
        /// <returns> A bool representing the state of the operation. </returns>
        public string MoveScaleModelAzimuth(int azimuth)
        {
            PLCConnector = new PLCConnector(PLCConstants.COM3);

            var sent = PLCConnector.SendSerialPortMessage(azimuth);

            string state = string.Empty;
            while(state == string.Empty)
            {
                state = PLCConnector.GetSerialPortMessage();
            }
            Console.WriteLine(state);

            PLCConnector.CloseSerialPort();

            return state;
        }

        /// <summary>
        /// Method to move the elevation of the scale model.
        /// </summary>
        /// <param name="elevation"> The elevation, in degrees, that the scale model should move to. </param>
        /// <returns> A bool representing the state of the operation. </returns>
        public string MoveScaleModelElevation(int elevation)
        {
            PLCConnector = new PLCConnector(PLCConstants.COM4);

            var sent = PLCConnector.SendSerialPortMessage(elevation);

            string state = string.Empty;
            while (state == string.Empty)
            {
                state = PLCConnector.GetSerialPortMessage();
            }
            Console.WriteLine(state);

            PLCConnector.CloseSerialPort();

            return state;
        }

        // Getter/Setter for the Connector that connects
        // the control room application to the PLC software.
        public PLCConnector PLCConnector { get; set; }
    }
}