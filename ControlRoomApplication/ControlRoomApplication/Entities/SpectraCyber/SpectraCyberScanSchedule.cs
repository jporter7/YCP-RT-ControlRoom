using System;

namespace ControlRoomApplication.Entities
{
    public class SpectraCyberScanSchedule
    {
        private SpectraCyberScanScheduleMode Mode { get; set; }
        private double ScanDelayMS { get; set; }
        // If this is true, that means the scanning starts after ScanDelayMS time has passed
        // Otherwise, an amount of time equal to ScanIntervalMS has to pass again, then it starts scanning
        private bool StartScanAfterDelay { get; set; }
        private double ScanIntervalMS { get; set; }
        private DateTime LastConsumeTick { get; set; }

        public SpectraCyberScanSchedule(SpectraCyberScanScheduleMode initMode = SpectraCyberScanScheduleMode.UNKNOWN)
        {
            Mode = initMode;
            ScanDelayMS = -1;
            ScanIntervalMS = -1;
            LastConsumeTick = DateTime.MinValue;
        }

        public void SetModeOff()
        {
            Mode = SpectraCyberScanScheduleMode.OFF;
        }

        public void SetSingleMode()
        {
            Mode = SpectraCyberScanScheduleMode.SINGLE_SCAN;
        }

        public void SetContinuousMode()
        {
            Mode = SpectraCyberScanScheduleMode.CONTINUOUS_SCAN;
        }

        public void SetScheduledMode(int intervalMS, int delayMS, bool startAfterDelay)
        {
            ScanIntervalMS = (double)intervalMS;
            ScanDelayMS = (double)delayMS;
            Mode = SpectraCyberScanScheduleMode.SCHEDULED_SCAN;
            LastConsumeTick = DateTime.UtcNow;
            StartScanAfterDelay = startAfterDelay;
        }

        public SpectraCyberScanScheduleMode GetMode()
        {
            return Mode;
        }

        public int GetScanIntervalMS()
        {
            return (int)ScanIntervalMS;
        }

        public DateTime GetLastConsumeTick()
        {
            return LastConsumeTick;
        }

        public void Consume()
        {
            LastConsumeTick = DateTime.UtcNow;

            if (Mode == SpectraCyberScanScheduleMode.SINGLE_SCAN)
            {
                SetModeOff();
            }
        }

        // Note: For the cases of SINGLE_SCAN and CONTINUOUS_SCAN, the returned time of 0 does not take into account any time the SpectraCyber needs to "rest" between scans
        public int TimeUntilReadyMS()
        {
            switch (Mode)
            {
                case SpectraCyberScanScheduleMode.SINGLE_SCAN:
                case SpectraCyberScanScheduleMode.CONTINUOUS_SCAN:
                    return 0;

                case SpectraCyberScanScheduleMode.SCHEDULED_SCAN:
                    if (ScanDelayMS > 0)
                    {
                        double TotalTimeMS = ScanDelayMS;

                        if (!StartScanAfterDelay)
                        {
                            TotalTimeMS += ScanIntervalMS;
                        }

                        return (int)(TotalTimeMS);
                    }
                    else
                    {
                        return (int)(ScanIntervalMS - (DateTime.UtcNow - LastConsumeTick).TotalMilliseconds);
                    }

                default:
                    return -1;
            }
        }

        public bool PollReadiness()
        {
            DateTime now = DateTime.UtcNow;
            double totalMS = (now - LastConsumeTick).TotalMilliseconds;

            switch (Mode)
            {
                case SpectraCyberScanScheduleMode.SINGLE_SCAN:
                case SpectraCyberScanScheduleMode.CONTINUOUS_SCAN:
                    return true;

                case SpectraCyberScanScheduleMode.SCHEDULED_SCAN:
                    if (ScanDelayMS > 0)
                    {
                        if (ScanDelayMS <= totalMS)
                        {
                            if (StartScanAfterDelay)
                            {
                                ScanDelayMS = 0;
                                return true;
                            }
                            else
                            {
                                totalMS -= ScanDelayMS;
                                ScanDelayMS = 0;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }

                    return totalMS >= ScanIntervalMS;

                default:
                    return false;
            }
        }
    }
}
