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

        public Orientation CalculateOrientation(DateTime datetime, Coordinate coordinate)
        {
            double lat = 39.96;
            double JD = AASharp.AASDate.DateToJD(datetime.Year,datetime.Month, datetime.Day,true);
            double ST = AASharp.AASSidereal.ApparentGreenwichSiderealTime(JD);
            //Greenwich hour angle
            double GHA = ST - coordinate.RightAscension;
            //use for caluclation local hour angle;
            double LHADelta = 76.72 / 15;
            double LHA = GHA - LHADelta;

            //Calculate Azimuth and Elevation
            // Reference - aa.usno.navy.mil/faq/docs/Alt_Az.php
            Orientation calc = new Orientation();
            calc.Azimuth = Math.Cos(LHA)*Math.Cos(lat)+Math.Sin(coordinate.Declination)*Math.Sin(lat);
            calc.Elevation = (0-Math.Sin(LHA))/(Math.Tan(coordinate.Declination)*Math.Cos(lat) - Math.Sin(lat)*Math.Cos(LHA));
            
            return calc;
        }

    }
}
