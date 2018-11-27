using AASharp;
using ControlRoomApplication.Entities;
using System;
using System.Collections.Generic;
using ControlRoomApplication.Constants;

namespace ControlRoomApplication.Controllers.AASharpControllers
{
    public class OrientationCalculationController
    {
        public OrientationCalculationController()
        {

        }

        public Dictionary<DateTime, Orientation> CalculateOrientations(DateTime startTime, DateTime endTime, Double rightAscension, Double declination)
        {
            Dictionary<DateTime, Orientation> dictionary = new Dictionary<DateTime, Orientation>();

            // Calculate the timespan of the observation
            TimeSpan span = endTime - startTime;

            // Calculate the Orientation for every four minutes of the observation's
            // total span
            for (int i = 0; i < span.Minutes; i += 4)
            {
                // Instantiate the DateTime for this iteration
                DateTime dateTime = startTime.AddMinutes(i);

                // Adapt the new DateTime object into an AASDate, which will be used in calculations
                AASDate aasDate = new AASDate(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, true);

                // Convert the AASDate into Dynamical Time (represented as a Julian Day)
                double julianDay = aasDate.Julian + AASDynamicalTime.DeltaT(aasDate.Julian) / 86400.0;

                // NOTE: This AASharp method uses right ascension and declination, but expects the right
                // ascension to be passed in in the form of hours (i.e 285 degrees = (285 / 15) = 19)
                AAS2DCoordinate topocentricCoordinate = AASParallax.Equatorial2Topocentric(
                    rightAscension / 15.0,
                    declination,
                    1,
                    RadioTelescopeConstants.OBSERVATORY_LONGITUDE,
                    RadioTelescopeConstants.OBSERVATORY_LATITUDE,
                    RadioTelescopeConstants.OBSERVATORY_HEIGHT,
                    julianDay
                );

                // The azimuth will be the AAS2DCoordinate's X value, and the elevation the Y value
                Orientation orientation = new Orientation(topocentricCoordinate.X, topocentricCoordinate.Y);

                // Add this value to our dictionary
                dictionary.Add(dateTime, orientation);
            }

            return dictionary;
        }
    }
}
