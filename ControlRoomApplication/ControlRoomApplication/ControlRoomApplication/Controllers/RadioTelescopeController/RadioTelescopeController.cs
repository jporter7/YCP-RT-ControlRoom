using ControlRoomApplication.Entities;

namespace ControlRoomApplication.Controllers.RadioTelescopeController
{
    public class RadioTelescopeController
    {
        public AbstractRadioTelescope RadioTelescope;

        public RadioTelescopeController(AbstractRadioTelescope radioTelescope)
        {
            RadioTelescope = radioTelescope;
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
    }
}