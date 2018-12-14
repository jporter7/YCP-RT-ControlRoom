using ControlRoomApplication.Constants;
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
            switch (RadioTelescope)
            {
                case ScaleRadioTelescope scale:
                    // Move the telescope to the "shutdown" position
                    Orientation ShutdownOrientation = new Orientation(0.0, -90.0);
                    scale.Plc.OutgoingOrientation = ShutdownOrientation;
                    scale.PlcController.MoveScaleModel(RadioTelescope.Plc, PLCConstants.COM3);
                    scale.PlcController.MoveScaleModel(RadioTelescope.Plc, PLCConstants.COM4);
                    scale.CurrentOrientation = ShutdownOrientation;

                    // Set the status to shutdown
                    scale.Status = RadioTelescopeStatusEnum.SHUTDOWN;

                    break;
                case ProductionRadioTelescope prod:
                    // Add Code for production radiotelescope later
                    break;
                default:
                    break;
            }
        }

        public void MoveRadioTelescope(Orientation orientation)
        {
            // Switch based on the type of radiotelescope that is being controlled by this controller.
            switch(RadioTelescope)
            {
                case ScaleRadioTelescope scale:
                    // Move the telescope to the orientation that it is supposed to be at
                    RadioTelescope.Status = RadioTelescopeStatusEnum.RUNNING;

                    RadioTelescope.PlcController.Plc.OutgoingOrientation = orientation;
                    Orientation[] cords = new Orientation[15];
                    Orientation computedO = new Orientation();
                    for (int i = 0; i < 15; i++)
                    {
                        Orientation ord = new Orientation();

                        ord.Azimuth = 30 + (3 * i);
                        ord.Elevation = 40 - (3 * i);
                        cords[i] = ord;
                    }

                    int x = 0;
                    foreach(Orientation ord in cords)
                    {
                        computedO.Azimuth = cords[x].Azimuth - cords[x - 1].Azimuth;
                        computedO.Elevation = cords[x].Elevation - cords[x - 1].Elevation;

                        RadioTelescope.PlcController.Plc.OutgoingOrientation = computedO;
                        Thread t1 = new Thread(() => RadioTelescope.PlcController.MoveScaleModel(RadioTelescope.PlcController.Plc, PLCConstants.COM3));
                        Thread t2 = new Thread(() => RadioTelescope.PlcController.MoveScaleModel(RadioTelescope.PlcController.Plc, PLCConstants.COM4));
                        t1.Start();
                        t2.Start();

                        t1.Join();
                        t2.Join();

                    }
                    
                    RadioTelescope.CurrentOrientation = orientation;

                    RadioTelescope.Status = RadioTelescopeStatusEnum.IDLE;
                    return;

                case ProductionRadioTelescope prod:
                    // Add Code for production radiotelescope later
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
            switch (RadioTelescope)
            {
                case ScaleRadioTelescope scale:
                    // Move the telescope to the orientation that it is supposed to calibrate at
                    scale.Status = RadioTelescopeStatusEnum.RUNNING;

                    // For the Scale Model, just align the orientation at (0, 0)
                    Orientation orientation = new Orientation();
                    orientation.Azimuth = 0;
                    orientation.Elevation = 0;
                    scale.PlcController.Plc.OutgoingOrientation = orientation;
                    scale.PlcController.MoveScaleModel(RadioTelescope.Plc, PLCConstants.COM3);
                    scale.PlcController.MoveScaleModel(RadioTelescope.Plc, PLCConstants.COM4);

                    scale.Status = RadioTelescopeStatusEnum.IDLE;
                    break;
                case ProductionRadioTelescope prod:
                    // Add Code for production radio-telescope later
                    break;
                default:
                    break;
            }

        }

        public void GetCurrentRadioTelescopeRFData()
        {
            RadioTelescope.IntegrateNow();
        }

        public void StartRadioTelscopeRFDataScan()
        {
            RadioTelescope.StartContinuousIntegration();
        }

        public void StopRadioTelescopeRFDataScan()
        {
            RadioTelescope.StopContinuousIntegration();
        }

        public void ScheduleRadioTelescopeRFDataScan(int intervalMS, int delayMS = 0, bool startAfterDelay = false)
        {
            RadioTelescope.StartScheduledIntegration(intervalMS, delayMS, startAfterDelay);
        }

        public AbstractRadioTelescope RadioTelescope { get; set; }
    }
}