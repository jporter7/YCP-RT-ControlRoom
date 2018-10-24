using AASharp;
using System;

namespace ControlRoomApplication.Controllers.AASharpControllers
{
    public class SunCoordinateCalculator
    {
        public SunCoordinateCalculator(DateTime date)
        {
            Date = date;
            CalculateEquatorialCoordinates();
        }

        public double GetEquatorialAzimuth()
        {
            return TopologicalCoordinate.X;
        }

        public double GetEquatorialElevation()
        {
            return TopologicalCoordinate.Y;
        }

        public void CalculateEquatorialCoordinates()
        {
            var bHighPrecision = false;

            //Calculate the topocentric horizontal position of the Sun for Palomar Observatory on midnight UTC for the 21st of September 2007
            AASDate dateSunCalc = new AASDate(Date.Year, Date.Month, Date.Day, true);
            double JDSun = dateSunCalc.Julian + AASDynamicalTime.DeltaT(dateSunCalc.Julian) / 86400.0;
            double SunLong = AASSun.ApparentEclipticLongitude(JDSun, bHighPrecision);
            double SunLat = AASSun.ApparentEclipticLatitude(JDSun, bHighPrecision);
            AAS2DCoordinate Equatorial = AASCoordinateTransformation.Ecliptic2Equatorial(SunLong, SunLat, AASNutation.TrueObliquityOfEcliptic(JDSun));
            double SunRad = AASEarth.RadiusVector(JDSun, bHighPrecision);
            double Longitude = AASCoordinateTransformation.DMSToDegrees(116, 51, 45); //West is considered positive
            double Latitude = AASCoordinateTransformation.DMSToDegrees(33, 21, 22);
            double Height = 1706;
            AAS2DCoordinate SunTopo = AASParallax.Equatorial2Topocentric(Equatorial.X, Equatorial.Y, SunRad, Longitude, Latitude, Height, JDSun);
            double AST = AASSidereal.ApparentGreenwichSiderealTime(dateSunCalc.Julian);
            double LongtitudeAsHourAngle = AASCoordinateTransformation.DegreesToHours(Longitude);
            double LocalHourAngle = AST - LongtitudeAsHourAngle - SunTopo.X;
            AAS2DCoordinate SunHorizontal = AASCoordinateTransformation.Equatorial2Horizontal(LocalHourAngle, SunTopo.Y, Latitude);
            SunHorizontal.Y += AASRefraction.RefractionFromTrue(SunHorizontal.Y, 1013, 10);

            TopologicalCoordinate = SunHorizontal;
        }

        public AAS2DCoordinate TopologicalCoordinate { get; set; }

        public DateTime Date { get; set; }
    }
}
