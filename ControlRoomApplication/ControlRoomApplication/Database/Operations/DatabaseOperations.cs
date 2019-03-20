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
        private static readonly bool USING_REMOTE_DATABASE = false;
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Return the appropriate database context
        /// </summary>
        public static RTDbContext InitializeDatabaseContext()
        {
            if (USING_REMOTE_DATABASE)
            {
                return new RTDbContext(AWSConstants.REMOTE_CONNECTION_STRING);
            }
            else
            {
                RTDbContext LocalContext = new RTDbContext();
                LocalContext.Database.CreateIfNotExists();
                LocalContext.SaveChanges();
                return LocalContext;
            }
        }

        /// <summary>
        /// Populates the local database with 4 appointments for testing purposes.
        /// </summary>
        public static void PopulateLocalDatabase()
        {
            if (!USING_REMOTE_DATABASE)
            {
                using (RTDbContext Context = InitializeDatabaseContext())
                {
                    DateTime date = DateTime.Now;

                    Appointment appt0 = new Appointment();
                    Appointment appt1 = new Appointment();
                    Appointment appt2 = new Appointment();
                    Appointment appt3 = new Appointment();

                    Coordinate coordinate0 = new Coordinate();
                    Coordinate coordinate1 = new Coordinate();
                    Coordinate coordinate2 = new Coordinate();
                    Coordinate coordinate3 = new Coordinate();

                    coordinate0.RightAscension = 10.3;
                    coordinate0.Declination = 50.8;

                    coordinate1.RightAscension = 22.0;
                    coordinate1.Declination = 83.63;

                    coordinate2.RightAscension = 16.0;
                    coordinate2.Declination = 71.5;

                    coordinate3.RightAscension = 26.3;
                    coordinate3.Declination = 85.12;

                    appt0.StartTime = date.AddMinutes(1);
                    appt0.EndTime = date.AddMinutes(2);
                    appt0.Status = AppointmentConstants.REQUESTED;
                    appt0.Type = AppointmentTypeConstants.ORIENTATION;
                    appt0.Orientation = new Orientation(30, 30);
                    appt0.SpectraCyberModeType = SpectraCyberModeTypeEnum.CONTINUUM;
                    appt0.TelescopeId = 1;
                    appt0.UserId = 1;

                    appt1.StartTime = date.AddMinutes(3);
                    appt1.EndTime = date.AddMinutes(4);
                    appt1.Status = AppointmentConstants.REQUESTED;
                    appt1.Type = AppointmentTypeConstants.CELESTIAL_BODY;
                    appt1.CelestialBody = CelestialBodyConstants.SUN;
                    appt1.SpectraCyberModeType = SpectraCyberModeTypeEnum.SPECTRAL;
                    appt1.TelescopeId = 1;
                    appt1.UserId = 1;

                    appt2.StartTime = date.AddMinutes(5);
                    appt2.EndTime = date.AddMinutes(6);
                    appt2.Status = AppointmentConstants.REQUESTED;
                    appt2.Type = AppointmentTypeConstants.POINT;
                    appt2.Coordinates.Add(coordinate2);
                    appt2.SpectraCyberModeType = SpectraCyberModeTypeEnum.CONTINUUM;
                    appt2.TelescopeId = 1;
                    appt2.UserId = 1;

                    appt3.StartTime = date.AddMinutes(7);
                    appt3.EndTime = date.AddMinutes(480);
                    appt3.Status = AppointmentConstants.REQUESTED;
                    appt3.Type = AppointmentTypeConstants.RASTER;
                    appt3.Coordinates.Add(coordinate0);
                    appt3.Coordinates.Add(coordinate1);
                    appt3.SpectraCyberModeType = SpectraCyberModeTypeEnum.CONTINUUM;
                    appt3.TelescopeId = 1;
                    appt3.UserId = 1;

                    List<Appointment> appts = new List<Appointment>()
                    {
                        appt0,
                        appt1,
                        appt2,
                        appt3,
                    };

                    Context.Appointments.AddRange(appts);
                    Context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Deletes the local database, if it exists.
        /// </summary>
        public static void DeleteLocalDatabase()
        {
            if (!USING_REMOTE_DATABASE)
            {
                using (RTDbContext Context = InitializeDatabaseContext())
                {
                    Context.Database.Delete();
                    Context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Returns the list of Appointments from the database.
        /// </summary>
        public static List<Appointment> GetListOfAppointments()
        {
            List<Appointment> appts = new List<Appointment>();
            using (RTDbContext Context = InitializeDatabaseContext())
            {
                foreach (var appt in Context.Appointments.ToList())
                {
                    // Copy Coordinates over to a new appointment object
                    // so appt list is valid after context is disposed
                    var coords = appt.Coordinates;
                    var new_appt = appt;
                    new_appt.Coordinates = coords;
                    appts.Add(appt);
                }
            }
            return appts;
        }

        /// <summary>
        /// Creates and stores and RFData reading in the local database.
        /// </summary>
        /// <param name="data">The RFData reading to be created/stored.</param>
        public static void CreateRFData(RFData data)
        {
            
            if (VerifyRFData(data))
            {
                using (RTDbContext Context = InitializeDatabaseContext())
                {
                    Context.RFDatas.Add(data);
                    Context.SaveChanges();
                }
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
                using (RTDbContext Context = InitializeDatabaseContext())
                {
                    // Update database appt with new status
                    var db_appt = Context.Appointments.Find(appt.Id);
                    db_appt.Status = appt.Status;
                    Context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Gets the next appointment from the local database context
        /// </summary>
        /// <returns></returns>
        public static Appointment GetNextAppointment()
        {
            Appointment appointment = null;
            using (RTDbContext Context = InitializeDatabaseContext())
            {
                logger.Debug("Retrieving list of appointments.");
                List<Appointment> appointments = GetListOfAppointments();

                if (appointments.Count > 0)
                {
                    appointments.RemoveAll(x => x.StartTime < DateTime.Now);
                    appointments.RemoveAll(x => x.Status == AppointmentConstants.COMPLETED);
                    appointments.Sort();
                    logger.Debug("Appointment list sorted. Starting to retrieve the next chronological appointment.");
                    appointment = appointments.Count > 0 ? appointments[0] : null;
                }
                else
                {
                    logger.Debug("No appointments found");
                }
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
    }
}