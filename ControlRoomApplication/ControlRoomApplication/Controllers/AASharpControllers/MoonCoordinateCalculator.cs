using AASharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.AASharpControllers
{
    public class MoonCoordinateCalculator
    {
        public MoonCoordinateCalculator(DateTime date)
        {
            Date = date;
            CalculateCoordinates();
        }

        public void CalculateCoordinates()
        {
            //Calculate the topocentric horizontal position of the Moon for Palomar Observatory on midnight UTC for the 21st of September 2007
            AASDate dateMoonCalc = new AASDate(2007, 9, 21, true);
            double JDMoon = dateMoonCalc.Julian + AASDynamicalTime.DeltaT(dateMoonCalc.Julian) / 86400.0;
            double MoonLong = AASMoon.EclipticLongitude(JDMoon);
            double MoonLat = AASMoon.EclipticLatitude(JDMoon);

            AAS2DCoordinate Equatorial = AASCoordinateTransformation.Ecliptic2Equatorial(MoonLong, MoonLat, AASNutation.TrueObliquityOfEcliptic(JDMoon));
            double MoonRad = AASMoon.RadiusVector(JDMoon);
            MoonRad /= 149597870.691; //Convert KM to AU
            double Longitude = AASCoordinateTransformation.DMSToDegrees(116, 51, 45); //West is considered positive
            double Latitude = AASCoordinateTransformation.DMSToDegrees(33, 21, 22);
            double Height = 395;

            AAS2DCoordinate MoonTopo = AASParallax.Equatorial2Topocentric(Equatorial.X, Equatorial.Y, MoonRad, Longitude, Latitude, Height, JDMoon);
            double AST = AASSidereal.ApparentGreenwichSiderealTime(dateMoonCalc.Julian);
            double LongtitudeAsHourAngle = AASCoordinateTransformation.DegreesToHours(Longitude);
            double LocalHourAngle = AST - LongtitudeAsHourAngle - MoonTopo.X;

            AAS2DCoordinate MoonHorizontal = AASCoordinateTransformation.Equatorial2Horizontal(LocalHourAngle, MoonTopo.Y, Latitude);
            MoonHorizontal.Y += AASRefraction.RefractionFromTrue(MoonHorizontal.Y, 750, 15);// 1013, 10);

            TopologicalCoordinate = MoonHorizontal;
        }

        public AAS2DCoordinate TopologicalCoordinate { get; set; }

        public DateTime Date { get; set; }
    }
}
