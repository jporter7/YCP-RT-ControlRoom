using System;
using System.Threading;
using ControlRoomApplication.Constants;

namespace ControlRoomApplication.Entities
{
    public abstract class AbstractWeatherStation : HeartbeatInterface
    {
        protected Mutex OperatingMutex;
        protected Thread OperatingThread;
        protected bool KeepOperatingThreadAlive;

        public Thread ReloadWeatherDataThread;
        public bool KeepReloadWeatherDataThreadAlive;

        private double _CurrentWindSpeedMPH;

        public double CurrentWindSpeedMPH
        {
            get
            {
                OperatingMutex.WaitOne();
                double read = GetWindSpeed();
                OperatingMutex.ReleaseMutex();

                return read;
            }
        }

        public int CurrentWindSpeedScanDelayMS { get; }

        public bool CurrentWindSpeedIsAllowable
        {
            get
            {
                return CurrentWindSpeedMPH <= MiscellaneousHardwareConstants.WEATHER_STATION_MAXIMUM_ALLOWABLE_WIND_SPEED_MPH;
            }
        }

        public int CurrentWindSpeedStatus
        {
            get
            {
                if (CurrentWindSpeedMPH < MiscellaneousHardwareConstants.WEATHER_STATION_WARNING_WIND_SPEED_MPH)
                    return 0; // Safe State of the Wind Speed
                else if (CurrentWindSpeedMPH >= MiscellaneousHardwareConstants.WEATHER_STATION_WARNING_WIND_SPEED_MPH
                    && CurrentWindSpeedMPH <= MiscellaneousHardwareConstants.WEATHER_STATION_MAXIMUM_ALLOWABLE_WIND_SPEED_MPH)
                    return 1; // Warning State of the Wind Speed
                else
                    return 2; // Alarm State of the Wind Speed
            }
        }



        public AbstractWeatherStation(int currentWindSpeedScanDelayMS)
        {
            CurrentWindSpeedScanDelayMS = currentWindSpeedScanDelayMS;
            _CurrentWindSpeedMPH = 0.0;
            OperatingMutex = new Mutex();
            OperatingThread = new Thread(new ThreadStart(OperationLoop));
            KeepOperatingThreadAlive = false;

            ReloadWeatherDataThread = null;
            KeepReloadWeatherDataThreadAlive = false;
    }

        public bool Start()
        {
            KeepOperatingThreadAlive = true;

            try
            {
                OperatingThread.Start();
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

        public bool RequestKillAndJoin()
        {
            try
            {
                OperatingMutex.WaitOne();
                KeepOperatingThreadAlive = false;
                OperatingMutex.ReleaseMutex();

                KeepReloadWeatherDataThreadAlive = false;
                ReloadWeatherDataThread.Join();
                OperatingThread.Join();
            }
            catch (Exception e)
            {
                if ((e is ObjectDisposedException) || (e is AbandonedMutexException) || (e is InvalidOperationException)
                    || (e is ApplicationException) || (e is ThreadStateException) | (e is ThreadInterruptedException))
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

        public void OperationLoop()
        {
            OperatingMutex.WaitOne();
            bool KeepAlive = KeepOperatingThreadAlive;
            OperatingMutex.ReleaseMutex();

            while (KeepAlive)
            {
                OperatingMutex.WaitOne();
                _CurrentWindSpeedMPH = GetWindSpeed();
                KeepAlive = KeepOperatingThreadAlive;
                OperatingMutex.ReleaseMutex();

                Thread.Sleep(CurrentWindSpeedScanDelayMS);
            }
        }

        protected override bool KillHeartbeatComponent()
        {
            return RequestKillAndJoin();
        }
        
        public abstract float GetBarometricPressure();
        public abstract float GetOutsideTemp();
        public abstract float GetInsideTemp();
        public abstract float GetDewPoint();
        public abstract float GetWindChill();
        public abstract int GetHumidity();
        public abstract float GetTotalRain();
        public abstract float GetDailyRain();
        public abstract float GetMonthlyRain();
        public abstract float GetWindSpeed();
        public abstract String GetWindDirection();
        public abstract float GetRainRate();
        public abstract int GetHeatIndex();

        // An abstract method that will get all of the information
        // to be able to put into the database cleanly
        //protected abstract short GetAllRecords();
    }
}
