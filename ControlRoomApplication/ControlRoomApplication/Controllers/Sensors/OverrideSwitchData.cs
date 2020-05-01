using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlRoomApplication.Controllers.Sensors;
using ControlRoomApplication.Database;
using ControlRoomApplication.Entities;

namespace ControlRoomApplication.Controllers.Sensors
{
    /// <summary>
    /// A public struct to store the override switch data for
    /// use on DiagnosticsForm.cs and AbstractPLCDriver.cs
    /// This DOES NOT include weather station sensors.
    /// </summary>
    public class OverrideSwitchData
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

        public OverrideSwitchData()
        {
            overrideGate = DatabaseOperations.GetOverrideStatusForSensor(SensorItemEnum.GATE);
            overrideAzimuthProx1 = DatabaseOperations.GetOverrideStatusForSensor(SensorItemEnum.PROXIMITY);
            overrideAzimuthProx2 = DatabaseOperations.GetOverrideStatusForSensor(SensorItemEnum.PROXIMITY);
            overrideElevatProx1 = DatabaseOperations.GetOverrideStatusForSensor(SensorItemEnum.PROXIMITY);
            overrideElevatProx2 = DatabaseOperations.GetOverrideStatusForSensor(SensorItemEnum.PROXIMITY);

            overrideAzimuthMotTemp = DatabaseOperations.GetOverrideStatusForSensor(SensorItemEnum.AZIMUTH_MOTOR);
            overrideElevatMotTemp = DatabaseOperations.GetOverrideStatusForSensor(SensorItemEnum.ELEVATION_MOTOR);
    }
    }
}
