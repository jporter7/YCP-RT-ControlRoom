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
        public CoordinateCalculationController()
        {

        }

        public Dictionary<DateTime, Orientation> CalculateCoordinates(Appointment appt)
        {
            Dictionary<DateTime, Orientation> orientations = new Dictionary<DateTime, Orientation>();
            TimeSpan length = appt.EndTime - appt.StartTime;
            for (int i = 0; i < length.TotalMinutes; i++)
            {
                DateTime datetime = DateTime.Now.AddMinutes(i);
                Coordinate coordinate = CalculateCoordinate(appt, datetime);
                if(coordinate != null)
                {
                    Orientation orientation = CoordinateToOrientation(coordinate, datetime);
                    orientations.Add(datetime, orientation);
                }
            }
            return orientations;
        }

        public Coordinate CalculateCoordinate(Appointment appt, DateTime datetime)
        {
            switch (appt.Type)
            {
                case ("POINT"):
                    var coordinates = appt.Coordinates.ToList();
                    return (coordinates.Count > 0) ? coordinates[0] : null;
                case ("CELESTIAL_BODY"):
                    return GetCelestialBodyCoordinate(appt.CelestialBody, datetime);
                case ("RASTER"):
                    throw new NotImplementedException();
                default:
                    return new Coordinate(0.0, 0.0);
            }
        }

        public Coordinate GetCelestialBodyCoordinate(string celestialBody, DateTime Date)
        {
            Coordinate coordinate = new Coordinate(0.0, 0.0);
            switch (celestialBody)
            {
                case "SUN":
                    SunCoordinateCalculator sunCoordinateCalculator = new SunCoordinateCalculator(Date);
                    coordinate.RightAscension = sunCoordinateCalculator.GetEquatorialElevation();
                    coordinate.Declination = sunCoordinateCalculator.GetEquatorialAzimuth();

                    return coordinate;
                case "MOON":
                    throw new NotImplementedException();
                default:
                    return new Coordinate(0.0, 0.0);
            }
        }

        public Orientation CoordinateToOrientation(Coordinate coordinate, DateTime Date)
        {
            AASDate date = new AASDate(Date.Year, Date.Month, Date.Day, Date.Hour, Date.Minute, Date.Second, true);
            double JD = date.Julian + AASDynamicalTime.DeltaT(date.Julian) / 86400.0;
            
            double AST = AASSidereal.ApparentGreenwichSiderealTime(date.Julian);
            double LongtitudeAsHourAngle = AASCoordinateTransformation.DegreesToHours(RadioTelescopeConstants.OBSERVATORY_LONGITUDE);
            double LocalHourAngle = AST - LongtitudeAsHourAngle - coordinate.RightAscension;
            AAS2DCoordinate Horizontal = AASCoordinateTransformation.Equatorial2Horizontal(LocalHourAngle, coordinate.Declination, RadioTelescopeConstants.OBSERVATORY_LATITUDE);

            Horizontal.X += 180;
            if(Horizontal.X > 360)
            {
                Horizontal.X -= 180;
            }

            return new Orientation(Horizontal.X, Horizontal.Y);
        }

        public Orientation SunCoordinateToOrientation(DateTime Date)
        {
            AASDate date = new AASDate(Date.Year, Date.Month, Date.Day, Date.Hour, Date.Minute, Date.Second, true);
            double JD = date.Julian + AASDynamicalTime.DeltaT(date.Julian) / 86400.0;
            double SunLong = AASSun.ApparentEclipticLongitude(JD, false);
            double SunLat = AASSun.ApparentEclipticLatitude(JD, false);
            
            AAS2DCoordinate equatorial = AASCoordinateTransformation.Ecliptic2Equatorial(SunLong, SunLat, AASNutation.TrueObliquityOfEcliptic(JD));
            double SunRad = AASEarth.RadiusVector(JD, false);
            
            // This line gives us RA & Declination.
            AAS2DCoordinate SunTopo = AASParallax.Equatorial2Topocentric(equatorial.X, equatorial.Y, SunRad, RadioTelescopeConstants.OBSERVATORY_LONGITUDE, RadioTelescopeConstants.OBSERVATORY_LATITUDE, RadioTelescopeConstants.OBSERVATORY_HEIGHT, JD);
            double AST = AASSidereal.ApparentGreenwichSiderealTime(date.Julian);
            double LongtitudeAsHourAngle = AASCoordinateTransformation.DegreesToHours(RadioTelescopeConstants.OBSERVATORY_LONGITUDE);
            double LocalHourAngle = AST - LongtitudeAsHourAngle - SunTopo.X;

            // This is supposed to give us azimuth & elevation
            AAS2DCoordinate SunHorizontal = AASCoordinateTransformation.Equatorial2Horizontal(LocalHourAngle, SunTopo.Y, RadioTelescopeConstants.OBSERVATORY_LATITUDE);
            SunHorizontal.Y += AASRefraction.RefractionFromTrue(SunHorizontal.Y, 1013, 10);

            // AASharp considers South as 0, instead of North
            SunHorizontal.X += 180;
            if(SunHorizontal.X > 360)
            {
                SunHorizontal.X -= 360;
            }

            return new Orientation(SunHorizontal.X, SunHorizontal.Y);
        }

        public Orientation MoonCoordinateToOrientation(DateTime Date)
        {

            AASDate date = new AASDate(Date.Year, Date.Month, Date.Day, Date.Hour, Date.Minute, Date.Second, true);
            double JD = date.Julian + AASDynamicalTime.DeltaT(date.Julian) / 86400.0;
            double MoonLong = AASMoon.EclipticLongitude(JD);
            double MoonLat = AASMoon.EclipticLatitude(JD);

            AAS2DCoordinate Equatorial = AASCoordinateTransformation.Ecliptic2Equatorial(MoonLong, MoonLat, AASNutation.TrueObliquityOfEcliptic(JD));
            double MoonRad = AASMoon.RadiusVector(JD);
            MoonRad /= 149597870.691; //Convert KM to AU

            AAS2DCoordinate MoonTopo = AASParallax.Equatorial2Topocentric(Equatorial.X, Equatorial.Y, MoonRad, RadioTelescopeConstants.OBSERVATORY_LONGITUDE, RadioTelescopeConstants.OBSERVATORY_LATITUDE, RadioTelescopeConstants.OBSERVATORY_HEIGHT, JD);
            double AST = AASSidereal.ApparentGreenwichSiderealTime(date.Julian);
            double LongtitudeAsHourAngle = AASCoordinateTransformation.DegreesToHours(RadioTelescopeConstants.OBSERVATORY_LONGITUDE);
            double LocalHourAngle = AST - LongtitudeAsHourAngle - MoonTopo.X;


            AAS2DCoordinate MoonHorizontal = AASCoordinateTransformation.Equatorial2Horizontal(LocalHourAngle, MoonTopo.Y, RadioTelescopeConstants.OBSERVATORY_LATITUDE);
            MoonHorizontal.Y += AASRefraction.RefractionFromTrue(MoonHorizontal.Y, 1013, 10);

            // South is considered 0 instead of North
            MoonHorizontal.X += 180;
            if(MoonHorizontal.X > 360)
            {
                MoonHorizontal.X -= 360;
            }

            return new Orientation(MoonHorizontal.X, MoonHorizontal.Y);
        }
    }
}