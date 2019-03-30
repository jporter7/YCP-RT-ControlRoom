﻿using System;
using System.Threading;
using ControlRoomApplication.Constants;

namespace ControlRoomApplication.Entities
{
    public abstract class AbstractWeatherStation : HeartbeatInterface
    {
        protected Mutex OperatingMutex;
        protected Thread OperatingThread;
        protected bool KeepOperatingThreadAlive;

        private double _CurrentWindSpeedMPH;

        public double CurrentWindSpeedMPH
        {
            get
            {
                OperatingMutex.WaitOne();
                double read = _CurrentWindSpeedMPH;
                OperatingMutex.ReleaseMutex();

                return read;
            }
        }

        public int CurrentWindSpeedScanDelayMS { get; }

        public bool CurrentWindSpeedIsAllowable
        {
            get
            {
                return CurrentWindSpeedMPH <= HardwareConstants.WEATHER_STATION_MAXIMUM_ALLOWABLE_WIND_SPEED_MPH;
            }
        }

        public AbstractWeatherStation(int currentWindSpeedScanDelayMS)
        {
            CurrentWindSpeedScanDelayMS = currentWindSpeedScanDelayMS;
            _CurrentWindSpeedMPH = 0.0;
            OperatingMutex = new Mutex();
            OperatingThread = new Thread(new ThreadStart(OperationLoop));
            KeepOperatingThreadAlive = false;
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

        public bool RequestKill()
        {
            try
            {
                OperatingMutex.WaitOne();
                KeepOperatingThreadAlive = false;
                OperatingMutex.ReleaseMutex();
            }
            catch (Exception e)
            {
                if ((e is ObjectDisposedException) || (e is AbandonedMutexException) || (e is InvalidOperationException) | (e is ApplicationException))
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
                _CurrentWindSpeedMPH = ReadCurrentWindSpeedMPH();
                KeepAlive = KeepOperatingThreadAlive;
                OperatingMutex.ReleaseMutex();

                Thread.Sleep(CurrentWindSpeedScanDelayMS);
            }
        }

        protected override bool KillHeartbeatComponent()
        {
            try
            {
                OperatingMutex.WaitOne();
                KeepOperatingThreadAlive = false;
                OperatingMutex.ReleaseMutex();

                OperatingThread.Join();
            }
            catch (Exception e)
            {
                if ((e is ObjectDisposedException) || (e is AbandonedMutexException) || (e is InvalidOperationException)
                    || (e is ApplicationException) || (e is ThreadStateException) || (e is ThreadInterruptedException))
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

        protected abstract double ReadCurrentWindSpeedMPH();
    }
}