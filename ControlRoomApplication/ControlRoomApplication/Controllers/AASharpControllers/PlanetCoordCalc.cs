using System;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Constants;
using AASharp;

namespace ControlRoomApplication.Controllers.AASharpControllers
{
    class PlanetCoordCalc
    {
        public PlanetCoordCalc()
        {

        }
        

        private static double DayFraction(System.DateTime date)
        {
            if (date.Hour > 12)
                return (double)date.Hour / 24.0 + date.Minute / 60.0 / 24.0 + date.Second / 3600.0 / 24.0;
            else
                return (double)date.Hour / 24.0 + date.Minute / 60.0 / 24.0 + date.Second / 3600.0 / 24.0 - .5;
        }

        //                      The Calculations for Az, El
        //                      
        // Elevation = arctan((cos(G)cos(L) - .1512) / sqrt(1- cos^2(G)cos^2(L)))
        //
        // Azimuth = 180 + arctan(tan(G) / sin(L))
        //
        // S = Celestial Body Longitude
        // N = RT Long
        // L = RT Lat
        // G = S - N
        private static Orientation SunAzEle(System.DateTime date)
        {
            double S = CalculateSunPos(date).RA;
            //RadioTelescope Longitude
            double N = RadioTelescopeConstants.RT_LONG;
            //RaduiTelescope Latitude
            double L = Constants.RadioTelescopeConstants.RT_LAT;
            // SunLong - RT_LONG
            double G = S - N;

            double elevation = System.Math.Atan((System.Math.Cos(G) * System.Math.Cos(L) - .1512) / System.Math.Sqrt(1 - System.Math.Pow(System.Math.Cos(G), 2) * System.Math.Pow(System.Math.Cos(L), 2)));
            elevation = AASCoordinateTransformation.RadiansToDegrees(elevation);

            double azimuth = 180 + System.Math.Atan(System.Math.Tan(G) / System.Math.Sin(L));
            azimuth = AASCoordinateTransformation.RadiansToDegrees(azimuth);

            return new Orientation(azimuth, elevation);
        }

        private static Orientation MoonAzEle(System.DateTime date)
        {
            double JD = AASDate.DateToJD(date.Year, date.Month, date.Day, true);

            double S = CalculateMoonPos(date).RA;
            double N = Constants.RadioTelescopeConstants.RT_LONG;
            double L = Constants.RadioTelescopeConstants.RT_LAT;
            double G = S - N;

            double elevation = System.Math.Atan((System.Math.Cos(G) * System.Math.Cos(L) - .1512) / System.Math.Sqrt(1 - System.Math.Pow(System.Math.Cos(G), 2) * System.Math.Pow(System.Math.Cos(L), 2)));
            double azimuth = 180 + System.Math.Atan(System.Math.Tan(G) / System.Math.Sin(L));

            return new Orientation(azimuth, elevation);
        }

        private static Orientation MercAzEle(System.DateTime date)
        {
            var bHighPrecision = false;
            double JD = AASDate.DateToJD(date.Year, date.Month, date.Day, true);
            double Lambda = AASMercury.EclipticLongitude(JD, bHighPrecision);
            double Beta = AASMercury.EclipticLatitude(JD, bHighPrecision);
            double Epsilon = AASNutation.TrueObliquityOfEcliptic(JD);
            AAS2DCoordinate coord = AASCoordinateTransformation.Ecliptic2Equatorial(Lambda, Beta, Epsilon);

            double N = Constants.RadioTelescopeConstants.RT_LONG;
            double L = Constants.RadioTelescopeConstants.RT_LAT;
            double Height = Constants.RadioTelescopeConstants.RT_ALT;

            // Calculate S
            AASDate aDate = new AASDate(date.Year, date.Month, date.Day, true);
            AAS2DCoordinate Topo = AASParallax.Equatorial2Topocentric(coord.X, coord.Y, AASMercury.RadiusVector(JD, false), N, L, Height, JD);
            double AST = AASSidereal.ApparentGreenwichSiderealTime(aDate.Julian);
            double LongtitudeAsHourAngle = AASCoordinateTransformation.DegreesToHours(N);
            double LocalHourAngle = AST - LongtitudeAsHourAngle - Topo.X;
            AAS2DCoordinate Horizontal = AASCoordinateTransformation.Equatorial2Horizontal(LocalHourAngle, Topo.Y, L);

            double S = Horizontal.X;
            double G = S - N;

            System.Console.WriteLine("Mercury Az/Ele = " + S + ", " + Horizontal.Y);

            double elevation = System.Math.Atan((System.Math.Cos(G) * System.Math.Cos(L) - .1512) / System.Math.Sqrt(1 - System.Math.Pow(System.Math.Cos(G), 2) * System.Math.Pow(System.Math.Cos(L), 2)));
            double azimuth = 180 + System.Math.Atan(System.Math.Tan(G) / System.Math.Sin(L));

            return new Orientation(azimuth, elevation);
        }

        private static Orientation VenusAzEle(System.DateTime date)
        {
            var bHighPrecision = true;

            double JD = AASDate.DateToJD(date.Year, date.Month, date.Day, true);
            double S = AASVenus.EclipticLongitude(JD, bHighPrecision);

            double N = Constants.RadioTelescopeConstants.RT_LONG;
            double L = Constants.RadioTelescopeConstants.RT_LAT;
            double G = S - N;

            double elevation = System.Math.Atan((System.Math.Cos(G) * System.Math.Cos(L) - .1512) / System.Math.Sqrt(1 - System.Math.Pow(System.Math.Cos(G), 2) * System.Math.Pow(System.Math.Cos(L), 2)));
            double azimuth = 180 + System.Math.Atan(System.Math.Tan(G) / System.Math.Sin(L));

            return new Orientation(azimuth, elevation);
        }

        private static Orientation MarsAzEle(System.DateTime date)
        {
            var bHighPrecision = true;

            double JD = AASDate.DateToJD(date.Year, date.Month, date.Day, true);
            double S = AASMars.EclipticLongitude(JD, bHighPrecision);

            double N = Constants.RadioTelescopeConstants.RT_LONG;
            double L = Constants.RadioTelescopeConstants.RT_LAT;
            double G = S - N;

            double elevation = System.Math.Atan((System.Math.Cos(G) * System.Math.Cos(L) - .1512) / System.Math.Sqrt(1 - System.Math.Pow(System.Math.Cos(G), 2) * System.Math.Pow(System.Math.Cos(L), 2)));
            double azimuth = 180 + System.Math.Atan(System.Math.Tan(G) / System.Math.Sin(L));

            return new Orientation(azimuth, elevation);
        }

        private static Orientation JupAzEle(System.DateTime date)
        {
            var bHighPrecision = true;

            double JD = AASDate.DateToJD(date.Year, date.Month, date.Day, true);
            double S = AASJupiter.EclipticLongitude(JD, bHighPrecision);

            double N = Constants.RadioTelescopeConstants.RT_LONG;
            double L = Constants.RadioTelescopeConstants.RT_LAT;
            double G = S - N;

            double elevation = System.Math.Atan((System.Math.Cos(G) * System.Math.Cos(L) - .1512) / System.Math.Sqrt(1 - System.Math.Pow(System.Math.Cos(G), 2) * System.Math.Pow(System.Math.Cos(L), 2)));
            double azimuth = 180 + System.Math.Atan(System.Math.Tan(G) / System.Math.Sin(L));

            return new Orientation(azimuth, elevation);
        }

        private static Orientation SatAzEle(System.DateTime date)
        {
            var bHighPrecision = true;

            double JD = AASDate.DateToJD(date.Year, date.Month, date.Day, true);
            double S = AASSaturn.EclipticLongitude(JD, bHighPrecision);

            double N = Constants.RadioTelescopeConstants.RT_LONG;
            double L = Constants.RadioTelescopeConstants.RT_LAT;
            double G = S - N;

            double elevation = System.Math.Atan((System.Math.Cos(G) * System.Math.Cos(L) - .1512) / System.Math.Sqrt(1 - System.Math.Pow(System.Math.Cos(G), 2) * System.Math.Pow(System.Math.Cos(L), 2)));
            double azimuth = 180 + System.Math.Atan(System.Math.Tan(G) / System.Math.Sin(L));

            return new Orientation(azimuth, elevation);
        }

        private static Orientation UraAzEle(System.DateTime date)
        {
            var bHighPrecision = true;

            double JD = AASDate.DateToJD(date.Year, date.Month, date.Day, true);
            double S = AASUranus.EclipticLongitude(JD, bHighPrecision);

            double N = Constants.RadioTelescopeConstants.RT_LONG;
            double L = Constants.RadioTelescopeConstants.RT_LAT;
            double G = S - N;

            double elevation = System.Math.Atan((System.Math.Cos(G) * System.Math.Cos(L) - .1512) / System.Math.Sqrt(1 - System.Math.Pow(System.Math.Cos(G), 2) * System.Math.Pow(System.Math.Cos(L), 2)));
            double azimuth = 180 + System.Math.Atan(System.Math.Tan(G) / System.Math.Sin(L));

            return new Orientation(azimuth, elevation);
        }

        private static Orientation NepAzEle(System.DateTime date)
        {
            var bHighPrecision = true;

            double JD = AASDate.DateToJD(date.Year, date.Month, date.Day, true);
            double S = AASNeptune.EclipticLongitude(JD, bHighPrecision);

            double N = Constants.RadioTelescopeConstants.RT_LONG;
            double L = Constants.RadioTelescopeConstants.RT_LAT;
            double G = S - N;

            double elevation = System.Math.Atan((System.Math.Cos(G) * System.Math.Cos(L) - .1512) / System.Math.Sqrt(1 - System.Math.Pow(System.Math.Cos(G), 2) * System.Math.Pow(System.Math.Cos(L), 2)));
            double azimuth = 180 + System.Math.Atan(System.Math.Tan(G) / System.Math.Sin(L));

            return new Orientation(azimuth, elevation);
        }

        private static Orientation PluAzEle(System.DateTime date)
        {
            double JD = AASDate.DateToJD(date.Year, date.Month, date.Day, true);
            double S = AASPluto.EclipticLongitude(JD);

            double N = Constants.RadioTelescopeConstants.RT_LONG;
            double L = Constants.RadioTelescopeConstants.RT_LAT;
            double G = S - N;

            double elevation = System.Math.Atan((System.Math.Cos(G) * System.Math.Cos(L) - .1512) / System.Math.Sqrt(1 - System.Math.Pow(System.Math.Cos(G), 2) * System.Math.Pow(System.Math.Cos(L), 2)));
            double azimuth = 180 + System.Math.Atan(System.Math.Tan(G) / System.Math.Sin(L));

            return new Orientation(azimuth, elevation);
        }




        private static Coordinate CalculateSunPos(System.DateTime date)
        {
            var bHighPrecision = false;

            // Calculate location of the center of the Sun as seen from a specific point on Earth
            AASDate dateCalc = new AASDate(date.Year, date.Month, date.Day, true);
            double JD = dateCalc.Julian + AASDynamicalTime.DeltaT(dateCalc.Julian) / 86400.0;
            double Long = AASSun.ApparentEclipticLongitude(JD, bHighPrecision);
            double Lat = AASSun.ApparentEclipticLatitude(JD, bHighPrecision);
            AAS2DCoordinate Equatorial = AASCoordinateTransformation.Ecliptic2Equatorial(Long, Lat, AASNutation.TrueObliquityOfEcliptic(JD));
            double Radius = AASEarth.RadiusVector(JD, bHighPrecision);

            // Establish position of RadioTelescope in use
            // TODO: This uses Global constants - Will need to be
            // altered to the specific RadioTelescope
            double Longitude = Constants.RadioTelescopeConstants.RT_LONG;
            double Latitude = Constants.RadioTelescopeConstants.RT_LAT;
            double Height = Constants.RadioTelescopeConstants.RT_ALT;

            AAS2DCoordinate Topo = AASParallax.Equatorial2Topocentric(Equatorial.X, Equatorial.Y, Radius, Longitude, Latitude, Height, JD);

            return new Coordinate(Topo.X, Topo.Y);
        }

        private static Coordinate CalculateMoonPos(System.DateTime date)
        {
            double JD = AASDate.DateToJD(date.Year, date.Month, date.Day, true);
            double Lambda = AASMoon.EclipticLongitude(JD);
            double Beta = AASMoon.EclipticLatitude(JD);
            double Epsilon = AASNutation.TrueObliquityOfEcliptic(JD);
            AAS2DCoordinate Equatorial = AASCoordinateTransformation.Ecliptic2Equatorial(Lambda, Beta, Epsilon);
            double radius = AASMoon.RadiusVector(JD);

            double Longitude = Constants.RadioTelescopeConstants.RT_LONG;
            double Latitude = Constants.RadioTelescopeConstants.RT_LAT;
            double Height = Constants.RadioTelescopeConstants.RT_ALT;

            AAS2DCoordinate Topo = AASParallax.Equatorial2Topocentric(Equatorial.X, Equatorial.Y, radius, Longitude, Latitude, Height, JD);

            return new Coordinate(Equatorial.X, Equatorial.Y);
        }

        private static Coordinate CalculateMercPos(System.DateTime date)
        {
            var bHighPrecision = false;
            double JD = AASDate.DateToJD(date.Year, date.Month, date.Day, true);
            double Lambda = AASMercury.EclipticLongitude(JD, bHighPrecision);
            double Beta = AASMercury.EclipticLatitude(JD, bHighPrecision);
            double Epsilon = AASNutation.TrueObliquityOfEcliptic(JD);
            AAS2DCoordinate Equatorial = AASCoordinateTransformation.Ecliptic2Equatorial(Lambda, Beta, Epsilon);

            double Radius = AASEarth.RadiusVector(JD, bHighPrecision);

            // Establish position of RadioTelescope in use
            // TODO: This uses Global constants - Will need to be
            // altered to the specific RadioTelescope
            double Longitude = Constants.RadioTelescopeConstants.RT_LONG;
            double Latitude = Constants.RadioTelescopeConstants.RT_LAT;
            double Height = Constants.RadioTelescopeConstants.RT_ALT;

            AAS2DCoordinate Topo = AASParallax.Equatorial2Topocentric(Equatorial.X, Equatorial.Y, Radius, Longitude, Latitude, Height, JD);

            return new Coordinate(Topo.X, Topo.Y);
        }

        private static Coordinate CalculateVenusPos(System.DateTime date)
        {
            var bHighPrecision = false;

            double JD = AASDate.DateToJD(date.Year, date.Month, date.Day, true);
            double Lambda = AASVenus.EclipticLongitude(JD, bHighPrecision);
            double Beta = AASVenus.EclipticLatitude(JD, bHighPrecision);
            double Epsilon = AASNutation.TrueObliquityOfEcliptic(JD);
            AAS2DCoordinate VenusCoord = AASCoordinateTransformation.Ecliptic2Equatorial(Lambda, Beta, Epsilon);

            return new Coordinate(VenusCoord.X, VenusCoord.Y);
        }

        private static Coordinate CalculateMarsPos(System.DateTime date)
        {
            var bHighPrecision = false;

            double JD = AASDate.DateToJD(date.Year, date.Month, date.Day, true);
            double Lambda = AASMars.EclipticLongitude(JD, bHighPrecision);
            double Beta = AASMars.EclipticLatitude(JD, bHighPrecision);
            double Epsilon = AASNutation.TrueObliquityOfEcliptic(JD);
            AAS2DCoordinate MarsCoord = AASCoordinateTransformation.Ecliptic2Equatorial(Lambda, Beta, Epsilon);

            return new Coordinate(MarsCoord.X, MarsCoord.Y);
        }

        private static Coordinate CalculateJupPos(System.DateTime date)
        {
            var bHighPrecision = false;

            double JD = AASDate.DateToJD(date.Year, date.Month, date.Day, true);
            double Lambda = AASJupiter.EclipticLongitude(JD, bHighPrecision);
            double Beta = AASJupiter.EclipticLatitude(JD, bHighPrecision);
            double Epsilon = AASNutation.TrueObliquityOfEcliptic(JD);
            AAS2DCoordinate JupiterCoord = AASCoordinateTransformation.Ecliptic2Equatorial(Lambda, Beta, Epsilon);

            return new Coordinate(JupiterCoord.X, JupiterCoord.Y);
        }

        private static Coordinate CalculateSatPos(System.DateTime date)
        {
            var bHighPrecision = false;

            double JD = AASDate.DateToJD(date.Year, date.Month, date.Day, true);
            double Lambda = AASSaturn.EclipticLongitude(JD, bHighPrecision);
            double Beta = AASSaturn.EclipticLatitude(JD, bHighPrecision);
            double Epsilon = AASNutation.TrueObliquityOfEcliptic(JD);
            AAS2DCoordinate SaturnCoord = AASCoordinateTransformation.Ecliptic2Equatorial(Lambda, Beta, Epsilon);

            return new Coordinate(SaturnCoord.X, SaturnCoord.Y);
        }

        private static Coordinate CalculateUraPos(System.DateTime date)
        {
            var bHighPrecision = false;

            double JD = AASDate.DateToJD(date.Year, date.Month, date.Day, true);
            double Lambda = AASUranus.EclipticLongitude(JD, bHighPrecision);
            double Beta = AASUranus.EclipticLatitude(JD, bHighPrecision);
            double Epsilon = AASNutation.TrueObliquityOfEcliptic(JD);
            AAS2DCoordinate UranusCoord = AASCoordinateTransformation.Ecliptic2Equatorial(Lambda, Beta, Epsilon);

            return new Coordinate(UranusCoord.X, UranusCoord.Y);
        }

        private static Coordinate CalculateNepPos(System.DateTime date)
        {
            var bHighPrecision = false;

            double JD = AASDate.DateToJD(date.Year, date.Month, date.Day, true);
            double Lambda = AASNeptune.EclipticLongitude(JD, bHighPrecision);
            double Beta = AASNeptune.EclipticLatitude(JD, bHighPrecision);
            double Epsilon = AASNutation.TrueObliquityOfEcliptic(JD);
            AAS2DCoordinate NeptuneCoord = AASCoordinateTransformation.Ecliptic2Equatorial(Lambda, Beta, Epsilon);

            return new Coordinate(NeptuneCoord.X, NeptuneCoord.Y);
        }

        private static Coordinate CalculatePluPos(System.DateTime date)
        {
            double JD = AASDate.DateToJD(date.Year, date.Month, date.Day, true);
            double Lambda = AASPluto.EclipticLongitude(JD);
            double Beta = AASPluto.EclipticLatitude(JD);
            double Epsilon = AASNutation.TrueObliquityOfEcliptic(JD);
            AAS2DCoordinate PlutoCoord = AASCoordinateTransformation.Ecliptic2Equatorial(Lambda, Beta, Epsilon);

            return new Coordinate(PlutoCoord.X, PlutoCoord.Y);
        }
    }
}
