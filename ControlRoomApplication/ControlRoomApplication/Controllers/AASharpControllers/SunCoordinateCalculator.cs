using AASharp;
using System;

namespace ControlRoomApplication.Controllers.AASharpControllers
{
    public class SunCoordinateCalculator
    {
        public SunCoordinateCalculator(DateTime date)
        {
            Date = date;
            CalculateCoordinates();
        }

        public double GetEquatorialAzimuth()
        {
            return TopologicalCoordinate.X;
        }

        public double GetEquatorialElevation()
        {
            return TopologicalCoordinate.Y;
        }

        public void CalculateCoordinates()
        {
            var bHighPrecision = false;

            //Calculate the topocentric horizontal position of the Sun for Palomar Observatory on midnight UTC for the 21st of September 2007
            AASDate dateSunCalc = new AASDate(Date.Year, Date.Month, Date.Day, Date.Hour, Date.Minute, Date.Second, true);
            double JDSun = dateSunCalc.Julian + AASDynamicalTime.DeltaT(dateSunCalc.Julian) / 86400.0;
            double SunLong = AASSun.ApparentEclipticLongitude(JDSun, bHighPrecision);
            double SunLat = AASSun.ApparentEclipticLatitude(JDSun, bHighPrecision);


            AAS2DCoordinate equatorial = AASCoordinateTransformation.Ecliptic2Equatorial(SunLong, SunLat, AASNutation.TrueObliquityOfEcliptic(JDSun));
            double SunRad = AASEarth.RadiusVector(JDSun, bHighPrecision);
            double Longitude = AASCoordinateTransformation.DMSToDegrees(40, 1, 27.872); //West is considered positive
            double Latitude = AASCoordinateTransformation.DMSToDegrees(76, 42, 16.430, false);
            double Height = 395;

            // This line gives us RA & Declination.
            AAS2DCoordinate SunTopo = AASParallax.Equatorial2Topocentric(equatorial.X, equatorial.Y, SunRad, Longitude, Latitude, Height, JDSun);
            double AST = AASSidereal.ApparentGreenwichSiderealTime(dateSunCalc.Julian);
            double LongtitudeAsHourAngle = AASCoordinateTransformation.DegreesToHours(Longitude);
            double LocalHourAngle = AST - LongtitudeAsHourAngle - SunTopo.X;

            // This is supposed to give us azimuth & elevation
            AAS2DCoordinate SunHorizontal = AASCoordinateTransformation.Equatorial2Horizontal(LocalHourAngle, SunTopo.Y, Latitude);
            SunHorizontal.Y += AASRefraction.RefractionFromTrue(SunHorizontal.Y, 1013, 10);

            TopologicalCoordinate = SunHorizontal;
        }

        public AAS2DCoordinate TopologicalCoordinate { get; set; }
        public DateTime Date { get; set; }
    }
}
