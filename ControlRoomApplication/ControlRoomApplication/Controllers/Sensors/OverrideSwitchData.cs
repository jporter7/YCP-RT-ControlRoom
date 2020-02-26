using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlRoomApplication.Controllers.Sensors;

namespace ControlRoomApplication.Controllers.Sensors
{
    /// <summary>
    /// A public struct to store the override switch data for
    /// use on DiagnosticsForm.cs and AbstractPLCDriver.cs
    /// This DOES NOT include weather station sensors.
    /// </summary>
    public struct OverrideSwitchData
    {
        // Uses PLC
        public bool overrideGate;
        public bool overrideAzimuthProx1;
        public bool overrideAzimuthProx2;
        public bool overrideElevatProx1;
        public bool overrideElevatProx2;

        // Does not use PLC
        public bool overrideAzimuthMotTemp;
        public bool overrideElevatMotTemp;
    }
}
