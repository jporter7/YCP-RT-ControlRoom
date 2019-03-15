using ControlRoomApplication.Entities;
using System;
using AASharp;
using ControlRoomApplication.Constants;
using System.Linq;
using System.Collections.Generic;

namespace ControlRoomApplication.Controllers.AASharpControllers
{
    public class CoordinateCalculationController
    {
        private Location Location { get; set; }

        public CoordinateCalculationController(Location location)
        {
            Location = location;
        }

        public Dictionary<DateTime, Orientation> CalculateCoordinates(Appointment appt)
        {
            Dictionary<DateTime, Orientation> orientations = new Dictionary<DateTime, Orientation>();
            TimeSpan length = appt.EndTime - appt.StartTime;
            for (int i = 0; i < length.TotalMinutes; i++)
            {
                DateTime datetime = appt.StartTime.AddMinutes(i);
                Orientation orientation = CalculateOrientation(appt, datetime);
                if(orientation != null)
                {
                    orientations.Add(datetime, orientation);
                }
            }
            return orientations;
        }

        public Orientation CalculateOrientation(Appointment appt, DateTime datetime)
        {
            switch (appt.Type)
            {
                case ("POINT"):
                    var point_coord = GetPointCoordinate(appt, datetime);
                    return CoordinateToOrientation(point_coord, datetime);
                case ("CELESTIAL_BODY"):
                    var celestial_body_coord = GetCelestialBodyCoordinate(appt.CelestialBody, datetime);
                    return CoordinateToOrientation(celestial_body_coord, datetime);
                case ("RASTER"):
                    var raster_coord = GetRasterCoordinate(appt, datetime);
                    return CoordinateToOrientation(raster_coord, datetime);
                case ("ORIENTATION"):
                    return appt.Orientation;
                case ("FREE_CONTROL"):
                    throw new NotImplementedException();
                default:
                    throw new ArgumentException("Invalid Appt type");
            }
        }

        public Coordinate GetPointCoordinate(Appointment appt, DateTime datetime)
        {
            var coords = appt.Coordinates.ToList();
            if (coords.Count > 1)
            {
                throw new ArgumentException("Too many Coordinates for a Point");
            }
            else if (coords.Count < 1)
            {
                throw new ArgumentException("No Point Coordinate");
            }

            return coords[0];
        }

        public Coordinate GetRasterCoordinate(Appointment appt, DateTime datetime)
        {
            // Get start and end coordinates
            List<Coordinate> coords = appt.Coordinates.ToList();

            if (coords.Count > 2)
            {
                throw new ArgumentException("Too many Coordinates for a Raster Scan");
            }
            else if (coords.Count < 2)
            {
                throw new ArgumentException("Too few Coordinates for a Raster Scan");
            }
            Coordinate start_coord = coords[0];
            Coordinate end_coord = coords[1];

            // Make sure the coordinates do not overlap
            if( start_coord.RightAscension == end_coord.RightAscension ||
                start_coord.Declination == end_coord.Declination)
            {
                throw new ArgumentException("Coordinates cannot overlap");
            }

            // Find the width and the height of the square in points (minutes),
            // rounded down to an integer
            double num_points = (appt.EndTime - appt.StartTime).TotalMinutes;
            double point_width = Math.Floor(Math.Sqrt(num_points));
            double point_height = Math.Floor(Math.Sqrt(num_points));

            // Find the width and the height of the square in coordinates
            double coord_width = end_coord.RightAscension - start_coord.RightAscension;
            double coord_height = end_coord.Declination - start_coord.Declination;

            // Find the coordinate increment per point 
            // (x = RightAscension, y = Declination)
            double x_increment = coord_width / point_width;
            double y_increment = coord_height / point_height;

            // Check if the point index is outside the bounds of the square
            // (this can occur because of the rounding down that occurs when
            // finding the point_width and point_height)
            // If it is, just stay at the last point of the square
            double max_point = point_width * point_height;
            double point = (datetime - appt.StartTime).TotalMinutes;
            if (point >= max_point)
            {
                point = max_point - 1;
            }

            // Find the point's x, y location, rounded down
            // (x = RightAscension, y = Declination)
            double point_x = Math.Floor(point / point_width);
            double point_y = Math.Floor(point % point_height);

            // Find the coordinate change in x and y
            // (x = RightAscension, y = Declination)
            double dx = (point_x * x_increment) % coord_width;
            double dy = (point_y * y_increment) % coord_height;

            // Find the new coordinate x and y
            // (x = RightAscension, y = Declination)
            double x = start_coord.RightAscension + dx;
            double y = start_coord.Declination + dy;

            //Console.WriteLine(x + ", " + y); // (FOR TESTING)

            // Create the new coordinate
            // (x = RightAscension, y = Declination)
            return new Coordinate(x, y);
        }

        public Coordinate GetCelestialBodyCoordinate(string celestialBody, DateTime datetime)
        {
            switch (celestialBody)
            {
                case "SUN":
                    return GetSunCoordinate(datetime);
                case "MOON":
                    return GetMoonCoordinate(datetime);
                default:
                    throw new ArgumentException("Invalid Celestial Body");
            }
        }

        public Coordinate GetSunCoordinate(DateTime datetime)
        {
            AASDate date = new AASDate(datetime.Year, datetime.Month, datetime.Day, datetime.Hour, datetime.Minute, datetime.Second, true);
            double JD = date.Julian + AASDynamicalTime.DeltaT(date.Julian) / 86400.0;
            double SunLong = AASSun.ApparentEclipticLongitude(JD, false);
            double SunLat = AASSun.ApparentEclipticLatitude(JD, false);
            
            AAS2DCoordinate equatorial = AASCoordinateTransformation.Ecliptic2Equatorial(SunLong, SunLat, AASNutation.TrueObliquityOfEcliptic(JD));
            double SunRad = AASEarth.RadiusVector(JD, false);
            
            // This line gives us RA & Declination.
            AAS2DCoordinate SunTopo = AASParallax.Equatorial2Topocentric(equatorial.X, equatorial.Y, SunRad, Location.Longitude, Location.Latitude, Location.Altitude, JD);

           return new Coordinate(SunTopo.X, SunTopo.Y);
        }

        public Coordinate GetMoonCoordinate(DateTime datetime)
        {
            AASDate date = new AASDate(datetime.Year, datetime.Month, datetime.Day, datetime.Hour, datetime.Minute, datetime.Second, true);
            double JD = date.Julian + AASDynamicalTime.DeltaT(date.Julian) / 86400.0;
            double MoonLong = AASMoon.EclipticLongitude(JD);
            double MoonLat = AASMoon.EclipticLatitude(JD);

            AAS2DCoordinate Equatorial = AASCoordinateTransformation.Ecliptic2Equatorial(MoonLong, MoonLat, AASNutation.TrueObliquityOfEcliptic(JD));
            double MoonRad = AASMoon.RadiusVector(JD);
            MoonRad /= 149597870.691; //Convert KM to AU

            AAS2DCoordinate MoonTopo = AASParallax.Equatorial2Topocentric(Equatorial.X, Equatorial.Y, MoonRad, Location.Longitude, Location.Latitude, Location.Altitude, JD);

            return new Coordinate(MoonTopo.X, MoonTopo.Y);
        }

        public Orientation CoordinateToOrientation(Coordinate coordinate, DateTime datetime)
        {
            AASDate date = new AASDate(datetime.Year, datetime.Month, datetime.Day, datetime.Hour, datetime.Minute, datetime.Second, true);

            double ApparentGreenwichSiderealTime = AASSidereal.ApparentGreenwichSiderealTime(date.Julian);
            double LongtitudeAsHourAngle = AASCoordinateTransformation.DegreesToHours(Location.Longitude);
            double LocalHourAngle = ApparentGreenwichSiderealTime - LongtitudeAsHourAngle - coordinate.RightAscension;
            AAS2DCoordinate Horizontal = AASCoordinateTransformation.Equatorial2Horizontal(LocalHourAngle, coordinate.Declination, Location.Latitude);

            // Since AASharp considers south zero, flip the orientation 180 degrees
            Horizontal.X += 180;
            if (Horizontal.X > 360)
            {
                Horizontal.X -= 360;
            }

            return new Orientation(Horizontal.X, Horizontal.Y);
        }
    }
}