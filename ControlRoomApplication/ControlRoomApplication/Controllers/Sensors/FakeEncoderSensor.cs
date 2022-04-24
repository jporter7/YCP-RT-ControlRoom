using ControlRoomApplication.Controllers.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlRoomApplication.Constants;

namespace ControlRoomApplication.Simulators.Hardware
{

    class FakeEncoderSensor
    {
        double _elAngle = 0.0;
        double _azAngle = 0.0;
        bool upOrDown = true; // Up is true, down is false
        bool leftOrRight = true; // Right is true, left is false
        DateTime currentElevationTime = DateTime.Now;
        DateTime currentAzimuthTime = DateTime.Now;

        public double GetElevationAngle()
        {
            if (_elAngle < SimulationConstants.MIN_ELEVATION_ANGLE)
                upOrDown = true;
            else if (_elAngle > SimulationConstants.MAX_ELEVATION_ANGLE)
                upOrDown = false;

            return ReadElevationAngleDemo();
        }

        public double GetAzimuthAngle()
        {
            if (_azAngle < SimulationConstants.MIN_AZIMUTH_ANGLE)
                leftOrRight = true;
            else if (_azAngle > SimulationConstants.MAX_AZIMUTH_ANGLE)
                leftOrRight = false;

            return ReadAzimuthAngleDemo();
        }

        public double ReadElevationAngleDemo()
        {
            TimeSpan elapsedElevationTime = DateTime.Now - currentElevationTime;
            if (elapsedElevationTime.TotalSeconds > 1)
            {
                if (upOrDown)
                    _elAngle += SimulationConstants.ELEVATION_UPDATE_RATE;
                else
                    _elAngle -= SimulationConstants.ELEVATION_UPDATE_RATE;

                currentElevationTime = DateTime.Now;
            }

            return _elAngle;
            
        }

        public double ReadAzimuthAngleDemo()
        {
            TimeSpan elapsedAzimuthTime = DateTime.Now - currentAzimuthTime;
            if (elapsedAzimuthTime.TotalSeconds > 1)
            {
                if (leftOrRight)
                    _azAngle += SimulationConstants.AZIMUTH_UPDATE_RATE;
                else
                    _azAngle -= SimulationConstants.AZIMUTH_UPDATE_RATE;

                currentAzimuthTime = DateTime.Now;
            }
            return _azAngle;
        }

        public void SetElevationAngle(double elAngle)
        {
            _elAngle = elAngle;
        }

        public void SetAzimuthAngle(double azAngle)
        {
            _azAngle = azAngle;
        }

        public bool getUpOrDown()
        {
            return upOrDown;
        }

        public bool getLeftOrRight()
        {
            return leftOrRight;
        }


    }
}
