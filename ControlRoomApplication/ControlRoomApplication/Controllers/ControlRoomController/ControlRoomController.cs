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
                Sensor currentSensor = ControlRoom.RTControllerManagementThreads[0].Sensors.Find(i => i.Item == SensorItemEnum.HIGH_WIND_SPEED);
                if (!ControlRoom.WeatherStation.CurrentWindSpeedIsAllowable)
                {
                    logger.Info("[ControlRoomController] Wind speeds were too high: " + ControlRoom.WeatherStation.CurrentWindSpeedMPH);

                    //shut down telescopes
                    foreach (RadioTelescopeController telescopeController in ControlRoom.RadioTelescopeControllers)
                    {
                        // change the sensor status in our Radio Telescope Controller Management Thread
                        int id = ControlRoom.RadioTelescopes.Find(i => i.WeatherStation == ControlRoom.WeatherStation).Id;
                        ControlRoom.RTControllerManagementThreads[0].Sensors.Add(new Sensor(SensorItemEnum.HIGH_WIND_SPEED, SensorStatus.ALARM));
                        ControlRoom.RTControllerManagementThreads[0].checkCurrentSensorAndOverrideStatus();
                    }
                }
                // there exists an alarm for this sensor but we are in a safe state now
                else if(currentSensor != null && currentSensor.Status == SensorStatus.ALARM) {

                    //change the status
                    currentSensor.Status = SensorStatus.NORMAL;
                    logger.Info("Wind speed sensor back in normal range.");
                }

                logger.Info("Current wind speed is: " + ControlRoom.WeatherStation.GetWindSpeed());

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