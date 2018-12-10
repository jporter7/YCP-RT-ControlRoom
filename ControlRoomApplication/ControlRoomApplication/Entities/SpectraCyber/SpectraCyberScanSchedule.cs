using System;

namespace ControlRoomApplication.Entities
{
    public class SpectraCyberScanSchedule
    {
        private SpectraCyberScanScheduleMode Mode { get; set; }
        private double ScanDelayMS { get; set; }
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
            LastConsumeTick = DateTime.Now;
            StartScanAfterDelay = startAfterDelay;
        }

        public void Consume()
        {
            LastConsumeTick = DateTime.Now;

            if (Mode == SpectraCyberScanScheduleMode.SINGLE_SCAN)
            {
                SetModeOff();
            }
        }

        public bool PollReadiness()
        {
            DateTime now = DateTime.Now;
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
