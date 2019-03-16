using System;
using System.Collections.Generic;
using ControlRoomApplication.Controllers.AASharpControllers;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Entities.RadioTelescope;

namespace ControlRoomApplication.Controllers.RadioTelescopeControllers
{
    public class RadioTelescopeController
    {
        public AbstractRadioTelescope RadioTelescope { get; set; }
        public CoordinateCalculationController CoordinateController { get; set; }

        /// <summary>
        /// Constructor that takes an AbstractRadioTelescope object and sets the
        /// corresponding field.
        /// </summary>
        /// <param name="radioTelescope"></param>
        public RadioTelescopeController(AbstractRadioTelescope radioTelescope)
        {
            RadioTelescope = radioTelescope;
            CoordinateController = new CoordinateCalculationController(radioTelescope.Location);
        }

        /// <summary>
        /// A simple getter for the underlying abstract RT model.
        /// </summary>
        /// <returns> The abstarct RT that this instance is controlling </returns>
        public AbstractRadioTelescope GetAbstractRadioTelescopeModel()
        {
            return RadioTelescope;
        }

        /// <summary>
        /// Gets the status of whether this RT is responding.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        /// <returns> Whether or not the RT responded. </returns>
        public bool TestRadioTelescopeCommunication()
        {
            byte[] ByteResponse = RadioTelescope.PLCClient.RequestMessageSend(PLCCommandAndQueryTypeEnum.TEST_CONNECTION);
            return (ByteResponse[2] == 0x1) && (ByteResponse[3] == 0x1);
        }

        /// <summary>
        /// Gets the current orientation of the radiotelescope in azimuth and elevation.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        /// <returns> An orientation object that holds the current azimuth/elevation of the scale model. </returns>
        public Orientation GetCurrentOrientation()
        {
            byte[] ByteResponse = RadioTelescope.PLCClient.RequestMessageSend(PLCCommandAndQueryTypeEnum.GET_CURRENT_AZEL_POSITIONS);

            if (ByteResponse[2] != 0x1)
            {
                return null;
            }

            return new Orientation(BitConverter.ToDouble(ByteResponse, 3), BitConverter.ToDouble(ByteResponse, 11));
        }

        /// <summary>
        /// Gets the current status of the four limit switches.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        /// <returns> An orientation object that holds the current azimuth/elevation of the scale model. </returns>
        public Orientation GetCurrentLimitSwitchStatuses()
        {
            return null;
        }

        /// <summary>
        /// Method used to shutdown the Radio Telescope in the case of inclement
        /// weather, maintenance, etcetera.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public void ShutdownRadioTelescope()
        {
            RadioTelescope.PLCClient.RequestMessageSend(PLCCommandAndQueryTypeEnum.SHUTDOWN);
            /*
            switch (RadioTelescope.GetType().Name)
            {
                case "ScaleRadioTelescope":
                    // Move the telescope to the "shutdown" position
                    Orientation ShutdownOrientation = new Orientation(0.0, -90.0);
                    RadioTelescope.PlcController.Plc.OutgoingOrientation = ShutdownOrientation;
                    RadioTelescope.PlcController.MoveScaleModel(PLCConstants.COM3, true);
                    RadioTelescope.PlcController.MoveScaleModel(PLCConstants.COM4, true);
                    RadioTelescope.CurrentOrientation = ShutdownOrientation;

                    // Set the status to shutdown
                    RadioTelescope.Status = RadioTelescopeStatusEnum.SHUTDOWN;

                    break;
                case "ProductionRadioTelescope":
                    // Add Code for production radiotelescope later
                    break;
                case "TestRadioTelescope":
                    // Add Code for test radiotelescope later
                default:
                    break;
            }
            */
        }

        /// <summary>
        /// Method used to request to move the Radio Telescope to an objective
        /// azimuth/elevation orientation.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public void MoveRadioTelescope(Orientation orientation)
        {
            RadioTelescope.PLCClient.RequestMessageSend(PLCCommandAndQueryTypeEnum.SET_OBJECTIVE_AZEL_POSITION, orientation);
            /*
            // Switch based on the type of radiotelescope that is being controlled by this controller.
            switch(RadioTelescope.GetType().Name)
            {
                case "ScaleRadioTelescope":
                    // Move the telescope to the orientation that it is supposed to be at
                    RadioTelescope.Status = RadioTelescopeStatusEnum.RUNNING;

                    RadioTelescope.PlcController.Plc.OutgoingOrientation = orientation;
                    List<Orientation> cords = new List<Orientation>();
                    Orientation computedO = new Orientation();
                    for (int i = 0; i < 2; i++)
                    {
                        Orientation ord = new Orientation();

                        ord.Azimuth = 75 + (60 * i);
                        ord.Elevation = 75 - (60 * i);
                        cords.Add(ord);
                    }

                    int x = 0;
                    foreach(Orientation ord in cords)
                    {
                        if (x > 0)
                        {
                            computedO.Azimuth = cords[x].Azimuth - cords[x - 1].Azimuth;
                            computedO.Elevation = cords[x].Elevation - cords[x - 1].Elevation;
                        } else
                        {
                            computedO.Azimuth = cords[x].Azimuth;
                            computedO.Elevation = cords[x].Elevation;
                        }


                        RadioTelescope.PlcController.Plc.OutgoingOrientation = computedO;
                        Thread t1, t2;
                        if (x > 0)
                        {
                            t1 = new Thread(() => RadioTelescope.PlcController.MoveScaleModel(PLCConstants.COM3, true));
                            t2 = new Thread(() => RadioTelescope.PlcController.MoveScaleModel(PLCConstants.COM4, true));
                        }
                        else
                        {
                            t1 = new Thread(() => RadioTelescope.PlcController.MoveScaleModel(PLCConstants.COM3, true));
                            t2 = new Thread(() => RadioTelescope.PlcController.MoveScaleModel(PLCConstants.COM4, true));
                        }
                        t1.Start();
                        t2.Start();
                        Console.WriteLine("Moving model.");
                        t1.Join();
                        t2.Join();
                        Thread.Sleep(3000);
                        HardwareFlags.COM3 = true;
                        HardwareFlags.COM4 = true;
                        x++;
                    }
                    
                    RadioTelescope.CurrentOrientation = orientation;

                    RadioTelescope.Status = RadioTelescopeStatusEnum.IDLE;
                    return;

                case "ProductionRadioTelescope":
                    // Add Code for production radiotelescope later
                    return;
                case "TestRadioTelescope":
                    // Add Code for test radiotelescope later
                    RadioTelescope.Status = RadioTelescopeStatusEnum.RUNNING;
                    RadioTelescope.CurrentOrientation = orientation;
                    RadioTelescope.Status = RadioTelescopeStatusEnum.IDLE;
                    return;
                default:
                    break;
            }
            */
        }

        /// <summary>
        /// Method used to request to move the Radio Telescope to an objective
        /// right ascension/declination coordinate pair.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public void MoveRadioTelescope(Coordinate coordinate)
        {
            MoveRadioTelescope(CoordinateController.CoordinateToOrientation(coordinate, DateTime.Now));
        }

        /// <summary>
        /// Method used to calibrate the Radio Telescope before each observation.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public void CalibrateRadioTelescope()
        {
            RadioTelescope.PLCClient.RequestMessageSend(PLCCommandAndQueryTypeEnum.CALIBRATE);
            /*
            switch (RadioTelescope.GetType().Name)
            {
                case "ScaleRadioTelescope":
                    // Move the telescope to the orientation that it is supposed to calibrate at
                    RadioTelescope.Status = RadioTelescopeStatusEnum.RUNNING;

                    // For the Scale Model, just align the orientation at (0, 0)
                    Orientation orientation = new Orientation();
                    orientation.Azimuth = 0;
                    orientation.Elevation = 0;
                    RadioTelescope.PlcController.Plc.OutgoingOrientation = orientation;
                    RadioTelescope.PlcController.MoveScaleModel(PLCConstants.COM3, true);
                    RadioTelescope.PlcController.MoveScaleModel(PLCConstants.COM4, true);

                    RadioTelescope.Status = RadioTelescopeStatusEnum.IDLE;
                    break;
                case "ProductionRadioTelescope":
                    // Add Code for production radio-telescope later
                    break;
                case "TestRadioTelescope":
                    // Add Code for test radiotelescope later
                default:
                    break;
            }
            */
        }

        private static RFData GenerateRFData(SpectraCyberResponse spectraCyberResponse)
        {
            RFData rfData = new RFData();
            rfData.TimeCaptured = spectraCyberResponse.DateTimeCaptured;
            rfData.Intensity = spectraCyberResponse.DecimalData;
            return rfData;
        }

        private static List<RFData> GenerateRFDataList(List<SpectraCyberResponse> spectraCyberResponses)
        {
            List<RFData> rfDataList = new List<RFData>();
            foreach (SpectraCyberResponse response in spectraCyberResponses)
            {
                rfDataList.Add(GenerateRFData(response));
            }

            return rfDataList;
        }
    }
}