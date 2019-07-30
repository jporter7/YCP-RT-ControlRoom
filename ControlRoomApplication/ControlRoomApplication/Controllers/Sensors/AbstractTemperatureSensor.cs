using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.Sensors
{
    /// <summary>
    /// Abstract class for the temperature sensor
    /// </summary>
    /// 
    public abstract class AbstractTemperatureSensor
    {
        /// <summary>
        /// get temperature of elevation motor
        /// </summary>
        /// 
        public abstract double GetElevationTemperature();

        /// <summary>
        /// get temperature of azimuth temperature
        /// </summary>
        /// 
        public abstract double GetAzimuthTemperature();

        /// <summary>
        /// 
        /// </summary>
        /// 
       // public abstract void SetElevationTemperature();

        /// <summary>
        /// 
        /// </summary>
        /// 
        //public abstract void SetAzimuthTemperature();

       

       



    }
}
