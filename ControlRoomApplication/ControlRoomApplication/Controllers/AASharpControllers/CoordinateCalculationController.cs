using ControlRoomApplication.Entities;
using System;

namespace ControlRoomApplication.Controllers.AASharpControllers
{
    public class CoordinateCalculationController
    {
        public CoordinateCalculationController()
        {

        }

        public Coordinate CalculateCoordinates(string celestialBody, DateTime date)
        {
            Coordinate coordinate = new Coordinate(0.0, 0.0);
            switch(celestialBody)
            {
                case "sun":
                    SunCoordinateCalculator sunCoordinateCalculator = new SunCoordinateCalculator(date);
                    coordinate.RightAscension = sunCoordinateCalculator.GetEquatorialElevation();
                    coordinate.Declination = sunCoordinateCalculator.GetEquatorialAzimuth();

                    return coordinate;
                default:
                    return new Coordinate(0.0,0.0);
            }
        }
    }
}
