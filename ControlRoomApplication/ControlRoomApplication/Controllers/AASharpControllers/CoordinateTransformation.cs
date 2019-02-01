using System;
using AASharp;
using ControlRoomApplication.Entities;


namespace ControlRoomApplication.Controllers.AASharpControllers
{
    public class CoordinateTransformation
    {
        public static Orientation CoordinateToOrientation(Coordinate coordinate, double latRT, double longRT, double altRT, DateTime date)
        {
            //Julian Date
            double JD = UTCtoJulian(date);

            //Local Sidereal Time from AASharp
            double LST = AASSidereal.ApparentGreenwichSiderealTime(JD);

            //Local Hour Angle = dideReal - RA then multiply by 15 (24hrs to 360deg)
            double LHA = LST - coordinate.Declination;
            LHA *= 15;

            double elevation = Math.Asin(Math.Sin(coordinate.RightAscension) * Math.Sin(latRT) + Math.Cos(coordinate.RightAscension) * Math.Cos(latRT) * Math.Cos(LHA));
            double azimuth = Math.Asin(Math.Sin(LHA) * Math.Cos(coordinate.RightAscension) / Math.Cos(elevation));
            return new Orientation((long) azimuth, (long) elevation);
        }

        public static double UTCtoJulian(DateTime date)
        {
            // AASharp can get us to the nearest day
            double JD = AASDate.DateToJD(date.Year, date.Month, date.Day, true);
            JD += date.Hour / 24.0 + date.Minute / 60.0 / 24.0 + date.Second / 60.0 / 60.0 / 24.0 + date.Millisecond / 1000.0 / 60.0 / 60.0 / 24.0;

            if (date.Hour > 12)
                return JD;
            else
                return JD - .5;
        }
    }
}
