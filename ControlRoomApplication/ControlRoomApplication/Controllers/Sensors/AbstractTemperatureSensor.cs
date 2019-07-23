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
       // public abstract void SetElevationTemperatuer();

        /// <summary>
        /// 
        /// </summary>
        /// 
        //public abstract void SetAzimuthTemperature();

        /// <summary>
        /// 
        /// </summary>
        /// 
        //public abstract double GetAverageElevationTemperature();

        /// <summary>
        /// 
        /// </summary>
        /// 
       // public abstract double GetAverageAzimuthTemperature();

        /// <summary>
        /// 
        /// </summary>
        /// 
       // public abstract double GetMinimumElevationTemperature();

        /// <summary>
        /// 
        /// </summary>
        /// 
        //public abstract double GetMaximumElevationTemperature();

        /// <summary>
        /// 
        /// </summary>
        /// 
       // public abstract double GetMinimumAzimuthTemperature();

        /// <summary>
        /// 
        /// </summary>
        /// 
       // public abstract double GetMaximumAzimuthTemperature();
            

       



    }
}
