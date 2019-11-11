using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.Sensors
{
    /// <summary>
    /// A public struct to store the data from the temperature sensor
    /// for use in the diagnostics page
    /// </summary>
    /// 
    public struct TempSensorData
    {
        public long azimuthTempTime;
        public double azimuthTemp;

        public long elevationTempTime;
        public double elevationTemp;

        public long azimuthAccTime;
        public double azimuthAcc;
        public double azimuthX;
        public double azimuthY;
        public double azimuthZ;

        public long elevationAccTime;
        public double elevationAcc;
        public double elevationX;
        public double elevationY;
        public double elevationZ;
    }
}
