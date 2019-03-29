using System;
using System.Threading;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Controllers.RadioTelescopeControllers;

namespace ControlRoomApplication.Controllers
{
    public class ControlRoomController
    {
        public ControlRoom CRoom { get; set; }
        public CancellationToken Token { get; set; }
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Thread WeatherMonitoringThread;
        private bool KeepWeatherMonitoringThreadAlive;

        public ControlRoomController(ControlRoom controlRoom, CancellationToken token)
        {
            CRoom = controlRoom;
            WeatherMonitoringThread = new Thread(new ThreadStart(WeatherMonitoringRoutine));
            KeepWeatherMonitoringThreadAlive = false;
            Token = Token;
        }

        public bool StartWeatherMonitoringRoutine()
        {
            KeepWeatherMonitoringThreadAlive = true;

            try
            {
                if (!CRoom.WeatherStation.Start())
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
                // Console.WriteLine("[ControlRoomController] Weather station reading: " + ControlRoom.WeatherStation.CurrentWindSpeedMPH.ToString() + " MPH wind speeds.");
                if (!ControlRoom.WeatherStation.CurrentWindSpeedIsAllowable)
                {
                    Console.WriteLine("[ControlRoomController] Wind speeds were too high: " + CRoom.WeatherStation.CurrentWindSpeedMPH);
                }

                Thread.Sleep(1000);
            }
        }

        public bool AddRadioTelescopeController(RadioTelescopeController rtController)
        {
            if (CRoom.RadioTelescopes.Contains(rtController.RadioTelescope))
            {
                return false;
            }

            CRoom.RTControllerManagementThreads.Add(new RadioTelescopeControllerManagementThread(rtController, Token));
            return true;
        }

        public bool AddRadioTelescopeControllerAndStart(RadioTelescopeController rtController)
        {
            if (AddRadioTelescopeController(rtController))
            {
                return CRoom.RTControllerManagementThreads[CRoom.RTControllerManagementThreads.Count - 1].Start();
            }
            else
            {
                return false;
            }
        }

        public bool RemoveRadioTelescopeControllerAt(int rtControllerIndex, bool waitForAnyTasks)
        {
            if ((rtControllerIndex < 0) || (rtControllerIndex >= CRoom.RTControllerManagementThreads.Count))
            {
                return false;
            }

            RadioTelescopeControllerManagementThread ToBeRemovedRTMT = CRoom.RTControllerManagementThreads[rtControllerIndex];

            if (ToBeRemovedRTMT.Busy && (!waitForAnyTasks))
            {
                ToBeRemovedRTMT.KillWithHardInterrupt();
            }
            else
            {
                ToBeRemovedRTMT.RequestToKill();
            }

            if (ToBeRemovedRTMT.WaitToJoin())
            {
                return CRoom.RTControllerManagementThreads.Remove(ToBeRemovedRTMT);
            }
            else
            {
                return false;
            }
        }

        public bool RemoveRadioTelescopeController(RadioTelescopeController rtController, bool waitForAnyTasks)
        {
            return RemoveRadioTelescopeControllerAt(CRoom.RadioTelescopeControllers.IndexOf(rtController), waitForAnyTasks);
        }
    }
}