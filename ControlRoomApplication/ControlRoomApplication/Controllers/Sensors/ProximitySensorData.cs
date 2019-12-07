using ControlRoomApplication.Controllers.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlRoomApplication.Constants;

namespace ControlRoomApplication.Simulators.Hardware
{
    public struct ProximitySensorData
    {
        public bool Azimuth_CW_Prox_Sensor;
        public bool Azimuth_CCW_Prox_Sensor;
        public bool Elevation_Lower_Prox_Sensor;
        public bool Elevation_Upper_Prox_Sensor;
    }
}
