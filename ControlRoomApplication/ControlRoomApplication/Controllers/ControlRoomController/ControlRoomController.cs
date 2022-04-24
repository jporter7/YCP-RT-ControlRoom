﻿using System;
using System.Threading;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Database;
using ControlRoomApplication.Controllers.Communications;
using ControlRoomApplication.Util;


namespace ControlRoomApplication.Controllers
{
    public class ControlRoomController
    {
        public ControlRoom ControlRoom { get; set; }
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Thread WeatherMonitoringThread;
        private bool KeepWeatherMonitoringThreadAlive;

        // Weather Station override
        //public bool weatherStationOverride = false;

        public ControlRoomController(ControlRoom controlRoom)
        {
            ControlRoom = controlRoom;
            WeatherMonitoringThread = new Thread( new ThreadStart( WeatherMonitoringRoutine ) ) { Name = "Weather Monitoring Routine" };
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
                Sensor currentSensor = ControlRoom.RTControllerManagementThreads[0].Sensors.Find(i => i.Item == SensorItemEnum.WIND);
                int windSpeedStatus = ControlRoom.WeatherStation.CurrentWindSpeedStatus;

                // The Wind Speed has triggered an Alarm Status
                if (windSpeedStatus == 2)
                {
                    logger.Info(Utilities.GetTimeStamp() + ": [ControlRoomController] Wind speeds were too high: " + ControlRoom.WeatherStation.CurrentWindSpeedMPH);

                    // Overriding the status warning if override is true
                    if (!ControlRoom.weatherStationOverride)
                    {
                        currentSensor.Status = SensorStatusEnum.ALARM;

                        pushNotification.sendToAllAdmins("WARNING: WEATHER STATION", "Wind speeds are too high: " + ControlRoom.WeatherStation.CurrentWindSpeedMPH);
                        EmailNotifications.sendToAllAdmins("WARNING: WEATHER STATION", "Wind speeds are too high: " + ControlRoom.WeatherStation.CurrentWindSpeedMPH);
                    }
                    DatabaseOperations.AddSensorStatusData(SensorStatus.Generate(SensorStatusEnum.WARNING, SensorStatusEnum.NORMAL, SensorStatusEnum.NORMAL, SensorStatusEnum.ALARM, currentSensor.Status));
                    //ControlRoom.RTControllerManagementThreads[0].checkCurrentSensorAndOverrideStatus();

                    /*
                    //shut down telescopes
                    foreach (RadioTelescopeController telescopeController in ControlRoom.RadioTelescopeControllers)
                    {
                        // change the sensor status in our Radio Telescope Controller Management Thread
                        //int id = ControlRoom.RadioTelescopes.Find(i => i.WeatherStation == ControlRoom.WeatherStation).Id;
                        //ControlRoom.RTControllerManagementThreads[0].Sensors.Add(new Sensor(SensorItemEnum.WIND_SPEED, SensorStatus.ALARM));
                        //ControlRoom.RTControllerManagementThreads[0].checkCurrentSensorAndOverrideStatus();
                    }
                    */
                }
                // The Wind Speed has triggered a Warning Status
                else if(windSpeedStatus == 1)
                {
                    logger.Info(Utilities.GetTimeStamp() + ": [ControlRoomController] Wind speeds are in Warning Range: " + ControlRoom.WeatherStation.CurrentWindSpeedMPH);


                    // Overriding the status warning if override is true
                    if (!ControlRoom.weatherStationOverride)
                    {
                        currentSensor.Status = SensorStatusEnum.WARNING;

                        pushNotification.sendToAllAdmins("WARNING: WEATHER STATION", "Wind speeds are in Warning Range: " + ControlRoom.WeatherStation.CurrentWindSpeedMPH);
                        EmailNotifications.sendToAllAdmins("WARNING: WEATHER STATION", "Wind speeds are in Warning Range: " + ControlRoom.WeatherStation.CurrentWindSpeedMPH);
                    }
                    DatabaseOperations.AddSensorStatusData(SensorStatus.Generate(SensorStatusEnum.WARNING, SensorStatusEnum.NORMAL, SensorStatusEnum.NORMAL, SensorStatusEnum.ALARM, currentSensor.Status));
                }
                else if (windSpeedStatus == 0)
                {
                    //logger.Info(Utilities.GetTimeStamp() + ": [ControlRoomController] Wind speeds are in a Safe State: " + ControlRoom.WeatherStation.CurrentWindSpeedMPH);
                    currentSensor.Status = SensorStatusEnum.NORMAL;
                    DatabaseOperations.AddSensorStatusData(SensorStatus.Generate(SensorStatusEnum.WARNING, SensorStatusEnum.NORMAL, SensorStatusEnum.NORMAL, SensorStatusEnum.ALARM, currentSensor.Status));
                }

                /*
                else if(currentSensor != null && currentSensor.Status == SensorStatus.ALARM) {

                    //change the status
                    currentSensor.Status = SensorStatus.NORMAL;
                    logger.Info(Utilities.GetTimeStamp() + ": Wind speed sensor back in normal range.");
                }*/

                //logger.Info(Utilities.GetTimeStamp() + ": Current wind speed is: " + ControlRoom.WeatherStation.GetWindSpeed());

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