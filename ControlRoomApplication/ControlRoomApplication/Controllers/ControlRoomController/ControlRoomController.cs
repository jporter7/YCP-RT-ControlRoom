using System;
using System.Threading;
using ControlRoomApplication.Entities;

namespace ControlRoomApplication.Controllers
{
    /// <summary>
    /// Controller for the control room class.
    /// </summary>
    public class ControlRoomController
    {
        /// <summary>
        /// The control room that is being controller by this controller.
        /// </summary>
        public ControlRoom ControlRoom { get; set; }
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Thread WeatherMonitoringThread;
        private bool KeepWeatherMonitoringThreadAlive;

        /// <summary>
        /// Constructor for the control room controller.
        /// </summary>
        /// <param name="controlRoom"> The control room class to be controlled. </param>
        public ControlRoomController(ControlRoom controlRoom)
        {
            ControlRoom = controlRoom;
            WeatherMonitoringThread = new Thread(new ThreadStart(WeatherMonitoringRoutine));
            KeepWeatherMonitoringThreadAlive = false;
        }

        /// <summary>
        /// Starts the weather station monitor and weather station thread. 
        /// </summary>
        /// <returns> A boolean indicating if the weather routine started. </returns>
        public bool StartWeatherMonitoringRoutine()
        {
            KeepWeatherMonitoringThreadAlive = true;

            try
            {
                if (!ControlRoom.WeatherStation.Start())
                {
                    return false;
                }

                WeatherMonitoringThread.Start();
            }
            catch (Exception e)
            {
                if ((e is ThreadStateException) || (e is OutOfMemoryException))
                {
                    return false;
                }
                else
                {
                    // Unexpected exception
                    throw e;
                }
            }

            return true;
        }

        /// <summary>
        /// Sends a request for the weather monitoring routine to stop.
        /// </summary>
        /// <returns> A boolean indicating if the weather monitoring routine stopped. </returns>
        public bool RequestToKillWeatherMonitoringRoutine()
        {
            KeepWeatherMonitoringThreadAlive = false;

            try
            {
                WeatherMonitoringThread.Join();
            }
            catch (Exception e)
            {
                if ((e is ThreadStateException) || (e is ThreadInterruptedException))
                {
                    return false;
                }
                else
                {
                    // Unexpected exception
                    throw e;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks the weather station attributes and determines if they are allowable.
        /// </summary>
        public void WeatherMonitoringRoutine()
        {
            while (KeepWeatherMonitoringThreadAlive)
            {
                // logger.Info("[ControlRoomController] Weather station reading: " + ControlRoom.WeatherStation.CurrentWindSpeedMPH.ToString() + " MPH wind speeds.");
                if (!ControlRoom.WeatherStation.CurrentWindSpeedIsAllowable)
                {
                    logger.Info("[ControlRoomController] Wind speeds were too high: " + ControlRoom.WeatherStation.CurrentWindSpeedMPH);
                }

                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Adds a radiotelescope controller and a management thread to the control room.
        /// </summary>
        /// <param name="rtController"> The radiotelescope controller to be added. </param>
        /// <returns> A boolean indicating if the controller was added to the control room. </returns>
        public bool AddRadioTelescopeController(RadioTelescopeController rtController)
        {
            if (ControlRoom.RadioTelescopes.Contains(rtController.RadioTelescope))
            {
                return false;
            }

            ControlRoom.RTControllerManagementThreads.Add(new RadioTelescopeControllerManagementThread(rtController));
            return true;
        }

        /// <summary>
        /// Adds a radiotelescope controller and starts its management thread.
        /// </summary>
        /// <param name="rtController"> The radiotelescope controller to be added and started. </param>
        /// <returns> A boolean indicating if the controller was added and started correctly. </returns>
        public bool AddRadioTelescopeControllerAndStart(RadioTelescopeController rtController)
        {
            if (AddRadioTelescopeController(rtController))
            {
                return ControlRoom.RTControllerManagementThreads[ControlRoom.RTControllerManagementThreads.Count - 1].Start();
            }
            else
            {
                return false;
            }
        }


        public bool RemoveRadioTelescopeControllerAt(int rtControllerIndex, bool waitForAnyTasks)
        {
            if ((rtControllerIndex < 0) || (rtControllerIndex >= ControlRoom.RTControllerManagementThreads.Count))
            {
                return false;
            }

            RadioTelescopeControllerManagementThread ToBeRemovedRTMT = ControlRoom.RTControllerManagementThreads[rtControllerIndex];

            if (!waitForAnyTasks)
            {
                ToBeRemovedRTMT.KillWithHardInterrupt();
            }
            else
            {
                ToBeRemovedRTMT.RequestToKill();
            }

            if (ToBeRemovedRTMT.WaitToJoin())
            {
                return ControlRoom.RTControllerManagementThreads.Remove(ToBeRemovedRTMT);
            }
            else
            {
                return false;
            }
        }

        public bool RemoveRadioTelescopeController(RadioTelescopeController rtController, bool waitForAnyTasks)
        {
            return RemoveRadioTelescopeControllerAt(ControlRoom.RadioTelescopeControllers.IndexOf(rtController), waitForAnyTasks);
        }
    }
}