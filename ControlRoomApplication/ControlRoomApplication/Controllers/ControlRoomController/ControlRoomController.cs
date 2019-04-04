using System;
using System.Threading;
using ControlRoomApplication.Entities;

namespace ControlRoomApplication.Controllers
{
    public class ControlRoomController
    {
        public ControlRoom ControlRoom { get; set; }
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Thread WeatherMonitoringThread;
        private bool KeepWeatherMonitoringThreadAlive;

        public ControlRoomController(ControlRoom controlRoom)
        {
            ControlRoom = controlRoom;
            WeatherMonitoringThread = new Thread(new ThreadStart(WeatherMonitoringRoutine));
            KeepWeatherMonitoringThreadAlive = false;
        }

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

        public bool AddRadioTelescopeController(RadioTelescopeController rtController)
        {
            if (ControlRoom.RadioTelescopes.Contains(rtController.RadioTelescope))
            {
                return false;
            }

            ControlRoom.RTControllerManagementThreads.Add(new RadioTelescopeControllerManagementThread(rtController));
            return true;
        }

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