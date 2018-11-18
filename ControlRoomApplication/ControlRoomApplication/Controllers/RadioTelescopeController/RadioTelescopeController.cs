using ControlRoomApplication.Entities;
using System.Collections.Generic;

namespace ControlRoomApplication.Controllers.RadioTelescopeController
{
    public class RadioTelescopeController
    {
        public AbstractRadioTelescope RadioTelescope;

        public RadioTelescopeController(AbstractRadioTelescope radioTelescope)
        {
            RadioTelescope = radioTelescope;
        }

        public RFData GetCurrentRadioTelescopeRFData()
        {
            return RadioTelescope.IntegrateNow();
        }
        
        public bool StartRadioTelscopeRFDataScan()
        {
            return RadioTelescope.StartContinuousIntegration();
        }

        public List<RFData> StopRadioTelescopeRFDataScan()
        {
            return RadioTelescope.StopContinuousIntegration();
        }
    }
}