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
                    coordinate.Latitude = sunCoordinateCalculator.GetEquatorialElevation();
                    coordinate.Longitude = sunCoordinateCalculator.GetEquatorialAzimuth();

                    return coordinate;
                case "moon":
                    
                default:
                    return new Coordinate(0.0,0.0);
            }
        }
    }
}
