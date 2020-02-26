using ControlRoomApplication.Controllers.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlRoomApplication.Constants;

namespace ControlRoomApplication.Simulators.Hardware
{
    public struct HomeSensorData
    {
        public bool Azimuth_Home_One;
        public bool Azimuth_Home_Two;
        public bool Elevation_Home;
    }
}
