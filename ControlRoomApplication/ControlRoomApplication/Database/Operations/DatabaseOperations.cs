using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Main;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ControlRoomApplication.Database.Operations
{
    public static class DatabaseOperations
    {
        /// <summary>
        /// Initialize the local and remote database contexts/connections.
        /// </summary>
        public static void InitializeConnections()
        {
            LocalContext = new RTDbContext();
            if (LocalContext.Database.Exists())
            {
                LocalContext.Database.Delete();
                LocalContext.SaveChanges();
            }
            LocalContext.Database.CreateIfNotExists();
            LocalContext.SaveChanges();

            RemoteContext = new RTDbContext(AWSConstants.REMOTE_CONNECTION_STRING);
        }

        /// <summary>
        /// Initialize only the local database, for testing purposes only.
        /// </summary>
        public static void InitializeLocalConnectionOnly()
        {
            LocalContext = new RTDbContext();
            if (LocalContext.Database.Exists())
            {
                LocalContext.Database.Delete();
                LocalContext.SaveChanges();
            }

            LocalContext.Database.CreateIfNotExists();
            LocalContext.SaveChanges();
        }

        /// <summary>
        /// Clean up the local database resources/connections, 
        /// for testing purposes only.
        /// </summary>
        public static void DisposeLocalDatabaseOnly()
        {
            LocalContext.Dispose();
        }

        /// <summary>
        /// Populates the local database with 3 appointments and 3 coordinates
        /// for testing purposes.
        /// </summary>
        public static void PopulateLocalDatabase()
        {
            if (LocalContext.Database.Exists())
            {
                DateTime date = DateTime.Now.AddMinutes(5);

                Appointment appt0 = new Appointment();
                Appointment appt1 = new Appointment();
                Appointment appt2 = new Appointment();
                Appointment appt3 = new Appointment();

                Coordinate coordinate0 = new Coordinate();
                Coordinate coordinate1 = new Coordinate();
                Coordinate coordinate2 = new Coordinate();
                Coordinate coordinate3 = new Coordinate();

                coordinate0.RightAscension = 50.8;
                coordinate0.Declination = 10.3;

                coordinate1.RightAscension = 83.63;
                coordinate1.Declination = 22.0;

                coordinate2.RightAscension = 71.5;
                coordinate2.Declination = 16.0;

                coordinate3.RightAscension = 85.12;
                coordinate3.Declination = 26.3;

                appt0.StartTime = date;
                appt0.EndTime = date.AddMinutes(1);
                appt0.Status = AppointmentConstants.IN_PROGRESS;
                appt0.Type = AppointmentTypeConstants.CELESTIAL_BODY;
                appt0.Coordinates = new List<Coordinate>();
                appt0.CelestialBody = CelestialBodyConstants.SUN;
                appt0.TelescopeId = 1;
                appt0.UserId = 1;

                appt1.StartTime = date;
                appt1.EndTime = date.AddMinutes(480);
                appt1.Status = AppointmentConstants.IN_PROGRESS;
                appt1.Type = AppointmentTypeConstants.RASTER;
                appt1.Coordinates = new List<Coordinate>();
                appt1.Coordinates.Add(coordinate0);
                appt1.Coordinates.Add(coordinate1);
                appt1.CelestialBody = CelestialBodyConstants.NONE;
                appt1.TelescopeId = 1;
                appt1.UserId = 1;

                appt2.StartTime = date.AddMinutes(2);
                appt2.EndTime = date.AddMinutes(3);
                appt2.Status = AppointmentConstants.REQUESTED;
                appt2.Type = AppointmentTypeConstants.POINT;
                appt2.Coordinates = new List<Coordinate>();
                appt2.Coordinates.Add(coordinate2);
                appt2.CelestialBody = CelestialBodyConstants.NONE;
                appt2.TelescopeId = 1;
                appt2.UserId = 1;

                appt3.StartTime = date.AddMinutes(3);
                appt3.EndTime = date.AddMinutes(4);
                appt3.Status = AppointmentConstants.REQUESTED;
                appt3.Type = AppointmentTypeConstants.POINT;
                appt3.Coordinates = new List<Coordinate>();
                appt3.Coordinates.Add(coordinate3);
                appt3.CelestialBody = CelestialBodyConstants.NONE;
                appt3.TelescopeId = 1;
                appt3.UserId = 1;

                List<Appointment> appts = new List<Appointment>()
                {
                    appt0,
                    appt1,
                    appt2,
                    appt3,
                };

                LocalContext.Appointments.AddRange(appts);
                LocalContext.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes the local database, if it exists.
        /// </summary>
        public static void DeleteLocalDatabase()
        {
            if (LocalContext.Database.Exists())
            {
                LocalContext.Database.Delete();
            }
        }

        public static List<Appointment> GetListOfAppointments()
        {
            return LocalContext.Appointments.ToList();
        }

        /// <summary>
        /// Creates and stores and RFData reading in the local database.
        /// </summary>
        /// <param name="data">The RFData reading to be created/stored.</param>
        public static void CreateRFData(RFData data)
        {
            if (VerifyRFData(data))
            {
                LocalContext.RFDatas.Add(data);
                LocalContext.SaveChanges();
            }
        }

        /// <summary>
        /// Updates the appointment status by saving the appt passed in.
        /// </summary>
        /// <param name="appt"> The appt that is being updated. </param>
        public static void UpdateAppointmentStatus(Appointment appt)
        {
            if (VerifyAppointmentStatus(appt))
            {
                LocalContext.SaveChanges();
            }
        }

        /// <summary>
        /// Gets the next appointment from the local database context
        /// </summary>
        /// <returns></returns>
        public static Appointment GetNextAppointment()
        {
            Appointment appointment = null;
            logger.Debug("Retrieving list of appointments.");
            List<Appointment> appointments = LocalContext.Appointments.ToList();

            if (appointments.Count > 0)
            {
                appointments.Sort();
                appointments.RemoveAll(x => x.StartTime < DateTime.Now);
                logger.Debug("Appointment list sorted. Starting to retrieve the next chronological appointment.");
                appointment = appointments[0];
            }
            else
            {
                logger.Debug("No appointments found");
            }

            return appointment;
        }

        /// <summary>
        /// Verifies that the RFData being created/stored in the database has an
        /// intensity greater than 0 and that the time it was captured is not in
        /// the future for any reason. (1 minute into the future to allow leeway
        /// for processing time).
        /// </summary>
        /// <param name="data"> The RFData that is being created/stored. </param>
        /// <returns> A boolean indicating whether or not the RFData is valid. </returns>
        private static bool VerifyRFData(RFData data)
        {
            if (data.Intensity <= 0)
            {
                return false;
            }
            else if (data.TimeCaptured > DateTime.Now.AddMinutes(1))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Verifies that the appointment status set for the appt is actually
        /// a valid appointment status.
        /// </summary>
        /// <param name="appt"> The appointment that's status is being updated. </param>
        /// <returns> A boolean indicating whether or not the status is valid.</returns>
        private static bool VerifyAppointmentStatus(Appointment appt)
        {
            return AppointmentConstants.AppointmentStatuses.Any(appt.Status.Contains);
        }

        private static RTDbContext LocalContext { get; set; }
        private static RTDbContext RemoteContext { get; set; }
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}