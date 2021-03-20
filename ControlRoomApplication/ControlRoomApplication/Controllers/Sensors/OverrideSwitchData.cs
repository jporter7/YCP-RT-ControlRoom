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
        public bool overrideGate { get; set; }

        public bool overrideAzimuthProx1 { get; set; }
        public bool overrideAzimuthProx2 { get; set; }


        public bool overrideElevatProx1 { get; set; }
        public bool overrideElevatProx2 { get; set; }

        // Does not use PLC
        public bool overrideAzimuthMotTemp { get; set; }

        public bool overrideElevatMotTemp { get; set; }

        public OverrideSwitchData()
        {
            overrideGate = DatabaseOperations.GetOverrideStatusForSensor(SensorItemEnum.GATE);

            // Azimuth overrides are no longer needed because we are using the slip ring
            overrideAzimuthProx1 = true;//DatabaseOperations.GetOverrideStatusForSensor(SensorItemEnum.PROXIMITY);
            overrideAzimuthProx2 = true;//DatabaseOperations.GetOverrideStatusForSensor(SensorItemEnum.PROXIMITY);

            overrideElevatProx1 = DatabaseOperations.GetOverrideStatusForSensor(SensorItemEnum.PROXIMITY);
            overrideElevatProx2 = DatabaseOperations.GetOverrideStatusForSensor(SensorItemEnum.PROXIMITY);

            overrideAzimuthMotTemp = DatabaseOperations.GetOverrideStatusForSensor(SensorItemEnum.AZIMUTH_MOTOR);
            overrideElevatMotTemp = DatabaseOperations.GetOverrideStatusForSensor(SensorItemEnum.ELEVATION_MOTOR);
        }

        public void setGatesOverride(bool doOverride)
        {
            DatabaseOperations.SetOverrideForSensor(SensorItemEnum.GATE, doOverride);
            overrideGate = doOverride;
        }

        public void setAzimuthMotTemp(bool doOverride)
        {
            DatabaseOperations.SetOverrideForSensor(SensorItemEnum.AZIMUTH_MOTOR, doOverride);
            overrideAzimuthMotTemp = doOverride;
        }

        public void setElevationMotTemp(bool doOverride)
        {
            DatabaseOperations.SetOverrideForSensor(SensorItemEnum.ELEVATION_MOTOR, doOverride);
            overrideElevatMotTemp = doOverride;
        }
    }
}
