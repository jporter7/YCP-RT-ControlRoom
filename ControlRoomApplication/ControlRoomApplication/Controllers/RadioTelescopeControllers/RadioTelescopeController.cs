﻿using ControlRoomApplication.Constants;
using ControlRoomApplication.Controllers.PLCController;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Entities.RadioTelescope;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;

namespace ControlRoomApplication.Controllers.RadioTelescopeControllers
{
    public class RadioTelescopeController
    {
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public RadioTelescopeController()
        {

        }

        /// <summary>
        /// Constructor that takes an AbstractRadioTelescope object and sets the
        /// corresponding field
        /// </summary>
        /// <param name="radioTelescope"></param>
        public RadioTelescopeController(AbstractRadioTelescope radioTelescope)
        {
            RadioTelescope = radioTelescope;
        }

        /// <summary>
        /// Gets the current orientation of the radiotelescope in azimuth and elevation.
        /// </summary>
        /// <returns> An orientation object that holds the current azimuth/elevation of the scale model. </returns>
        public Orientation GetCurrentOrientation()
        {
            return RadioTelescope.CurrentOrientation;
        }

        /// <summary>
        /// Method used to shutdown the Radio Telescope in the case of inclement
        /// weather, maintenance, etc.
        /// This method functions differently depending on the Radio Telescope type.
        /// Currently, only the ScaleModel scenario is implemented, and it will move
        /// the CurrentOrientation to a straight upward position and set the status to shutdown.
        /// </summary>
        public void ShutdownRadioTelescope()
        {
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
        }

        public void MoveRadioTelescope(Orientation orientation)
        {
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
                case "TestRadioTelescope":
                    // Add Code for test radiotelescope later
                default:
                    break;
            }
        }

        /// <summary>
        /// Method used to calibrate the Radio Telescope before each observation. It, like the 
        /// MoveRadioTelescope method, is designed to function differently based on the type of
        /// Radio Telescope in question. Currently, only the ScaleRadioTelescope scenario is designed 
        /// with functionality, wherein it will set the Orientation to (0,0)
        /// </summary>
        public void CalibrateRadioTelescope()
        {
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

        }

        private static RFData GenerateRFData(SpectraCyberResponse spectraCyberResponse)
        {
            RFData rfData = new RFData();
            rfData.TimeCaptured = spectraCyberResponse.DateTimeCaptured;
            rfData.Intensity = spectraCyberResponse.DecimalData;
            // TODO: set ID
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

        public AbstractRadioTelescope RadioTelescope { get; set; }
    }
}