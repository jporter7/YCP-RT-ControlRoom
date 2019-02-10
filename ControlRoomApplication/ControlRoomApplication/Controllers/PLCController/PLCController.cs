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

        public PLCController(AbstractPLC plc)
        {
            Plc = plc;
        }

        /// <summary>
        /// Method that will send commands to the plc to calibrate the telescope.
        /// Functions differently based on the plc type. If the PLC is a ScaleModelPLC,
        /// it will actually write and send a message, and receive a corresponding response.
        /// If it is a TestPLC, it will just return a boilerplate message, as the TestPLC is used
        /// to ensure the functionality of other components, while not requiring to actually be 
        /// hooked up with the Scale Model.
        /// If it is a VRPLC, it just returns the same boilerplate message since there is no 
        /// calibration for the VR Telescope at this moment.
        /// </summary>
        /// <returns> A string representing the state of the operation. </returns>
        public string CalibrateRT()
        {
            switch (Plc)
            {
                case ScaleModelPLC scaleModelPLC:
                    PlcConnector.WriteMessage(PLCConstants.CALIBRATE);

                    logger.Info("Scale model calibrated.");
                    return PlcConnector.ReceiveMessage();

                case TestPLC testPLC:
                    return "Radio Telescope successfully calibrated";

                case VRPLC vrPLC:
                    return "Radio Telescope sucessfully calibrated";

                default:
                    logger.Error("Radiotelescope type not defined.");
                    return null;
            }
        }

        /// <summary>
        /// Tells the specified PLC to shutdown.
        /// Functions differently based on the plc type. If the PLC is a ScaleModelPLC,
        /// it will actually write and send a shutdown message, and receive a corresponding response.
        /// If it is a TestPLC, it will just return a boilerplate message, as the TestPLC is used
        /// to ensure the functionality of other components, while not requiring that the application 
        /// actually be hooked up to the Scale Model.
        /// If it is vrPLC it shuts down the namedPipeServer.
        /// </summary>
        /// <returns> A string representing the state of the operation. </returns>
        public string ShutdownRT(AbstractPLC plc)
        {
            switch (Plc)
            {
                case ScaleModelPLC scaleModelPLC:
                    PlcConnector.WriteMessage(PLCConstants.SHUTDOWN);

                    logger.Info("Scale model shut down.");
                    return PlcConnector.ReceiveMessage();

                case TestPLC testPLC:
                    return "Radio Telescope successfully shut down";

                case VRPLC vrPLC:
                    vrPLC.pipeClient.Close();
                    return "Radio Telescope successfully shut down";

                default:
                    logger.Error("Radio-telescope type not defined.");
                    return null;
            }
            
        }

        /// <summary>
        /// Tells the specified PLC to move the radiotelescope to the specified azimuth.
        /// Functions differently based on the plc type. If the PLC is a ScaleModelPLC,
        /// it will actually write and send a message containing the azimuth and elevation, and
        /// will receive a corresponding response. If it is a TestPLC, it will just return a boilerplate 
        /// message, since the TestPLC is used to ensure the functionality of other components, while not requiring
        /// that the application actually be hooked up to the Scale Model.
        /// VRPLC sends coordinate to VR telescope through named pipe from VRPLC entity.
        /// VR telescope will return 1 on sucess.
        /// </summary>
        /// <param name="azimuth"> The azimuth that the PLC should move the radiotelescope to. </param>
        /// <returns> A string that indicates the state of the operation. </returns>
        public string MoveTelescope(AbstractPLC plc, Coordinate coordinate) //long azimuthOffset)
        {
            switch(Plc)
            {
                case ScaleModelPLC scaleModelPLC:
                    if (coordinate.RightAscension < PLCConstants.RIGHT_ASCENSION_LOWER_LIMIT || coordinate.RightAscension > PLCConstants.RIGHT_ASCENSION_UPPER_LIMIT)
                    {
                        throw new System.Exception();
                    }
                    else if (coordinate.Declination < PLCConstants.DECLINATION_LOWER_LIMIT || coordinate.Declination > PLCConstants.DECLINATION_UPPER_LIMIT)
                    {
                        throw new System.Exception();
                    }

                    PlcConnector.WriteMessage($"right_ascension {coordinate.RightAscension}, declination {coordinate.Declination}");

                    logger.Info($"Scale model moved to ({coordinate.RightAscension},{coordinate.Declination})");
                    return PlcConnector.ReceiveMessage();

                case TestPLC testPLC:
                    return "Oh yea we received a message from the Radio Telescope boyo";

                case VRPLC vrPLC:
                    vrPLC.pipeClient.WaitForConnection();
                    if(coordinate.RightAscension <= 255)
                    {
                        vrPLC.pipeClient.WriteByte(System.Convert.ToByte(coordinate.RightAscension));
                        vrPLC.pipeClient.WaitForPipeDrain();
                        vrPLC.pipeClient.WriteByte(0);
                        vrPLC.pipeClient.WaitForPipeDrain();
                    }
                    else if (coordinate.RightAscension <= 360)
                    {
                        vrPLC.pipeClient.WriteByte(255);
                        vrPLC.pipeClient.WaitForPipeDrain();
                        vrPLC.pipeClient.WriteByte(System.Convert.ToByte(coordinate.RightAscension-255));
                        vrPLC.pipeClient.WaitForPipeDrain();
                    }

                    if (coordinate.Declination <= 255)
                    {
                        vrPLC.pipeClient.WriteByte(System.Convert.ToByte(coordinate.Declination));
                        vrPLC.pipeClient.WaitForPipeDrain();
                        vrPLC.pipeClient.WriteByte(0);
                        vrPLC.pipeClient.WaitForPipeDrain();
                    }
                    else if (coordinate.Declination <= 360)
                    {
                        vrPLC.pipeClient.WriteByte(255);
                        vrPLC.pipeClient.WaitForPipeDrain();
                        vrPLC.pipeClient.WriteByte(System.Convert.ToByte(coordinate.Declination-255));
                        vrPLC.pipeClient.WaitForPipeDrain();
                    }
                    return "If this is a 1 we got good data from VR Telescope!!! -> "+vrPLC.pipeClient.ReadByte();


                default:
                    logger.Info("PLC type not defined.");
                    return null;
            }
            
        }

        /// <summary>
        /// Method to move the scale model to a certain set of coordinates.
        /// </summary>
        /// <param name="plc"> The plc that the commands should be sent to. </param>
        /// <param name="coordinate"> A set of coordinates that the telescope should be moved to.</param>
        public string MoveScaleModel(string comPort, bool flag)
        {
            switch (Plc)
            {
                case ScaleModelPLC scaleModelPLC:
                    if (Plc.OutgoingOrientation.Azimuth < PLCConstants.RIGHT_ASCENSION_LOWER_LIMIT || Plc.OutgoingOrientation.Azimuth > PLCConstants.RIGHT_ASCENSION_UPPER_LIMIT)
                    {
                        logger.Error($"Azimuth ({Plc.OutgoingOrientation.Azimuth}) was out of range.");
                        throw new System.Exception();
                    }
                    else if (Plc.OutgoingOrientation.Elevation < PLCConstants.DECLINATION_LOWER_LIMIT || Plc.OutgoingOrientation.Elevation > PLCConstants.DECLINATION_UPPER_LIMIT)
                    {
                        logger.Error($"Elevation ({Plc.OutgoingOrientation.Elevation} was out of range.)");
                        throw new System.Exception();
                    }

                    // Convert orientation object to a json string
                    string jsonOrientation = JsonConvert.SerializeObject(Plc.OutgoingOrientation);

                    // Move the scale model's azimuth motor on com3 and its elevation on com4
                    // make sure there is a delay in this thread for enough time to have the arduino
                    // move the first motor (azimuth)
                    if (flag)
                    {
                        PlcConnector = new PLCConnector(comPort);
                    }
                    PlcConnector.SendSerialPortMessage(jsonOrientation);

                    // Wait for the arduinos to send back a response 
                    // in the arduino code, as of milestone 2, the response is a string: "finished"
                    var state = string.Empty;
                    //state = PlcConnector.GetSerialPortMessage();

                    // Print the state of the move operation to the console.
                    //Console.WriteLine(state);

                    logger.Info($"Scale model moved to {Plc.OutgoingMessage}");
                    return state;

                case TestPLC testPLC:
                    return "Oh yea we received a message from the Radio Telescope boyo";

                case VRPLC vrPLC:
                    return "Oh yea we received a message from the Radio Telescope boyo";

                default:
                    logger.Info("PLC type not defined.");
                    return null;
            }
            
        }

        public AbstractPLC Plc { get; set; }
        public PLCConnector PlcConnector { get; set; }
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}