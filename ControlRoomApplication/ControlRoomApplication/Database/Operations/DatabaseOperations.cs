﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Main;

namespace ControlRoomApplication.Database
{
    public static class DatabaseOperations
    {
        private static bool USING_REMOTE_DATABASE = true;
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Updates the appointment status by saving the appt passed in.
        /// </summary>
        /// <param name="Context"> The Context that is being saved. </param>
        private static void SaveContext(RTDbContext Context)
        {
            bool saveFailed;
            do
            {
                saveFailed = false;

                try
                {
                    Context.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        logger.Error($"Entity of type {eve.Entry.Entity.GetType().Name} in state {eve.Entry.State} has the following validation errors:");
                        foreach (var ve in eve.ValidationErrors)
                        {
                            logger.Error($"- Property: {ve.PropertyName}, Error: {ve.ErrorMessage}");
                        }
                    }
                    throw;
                }
                catch (DbUpdateException ex)
                {
                    // Update the values of the entity that failed to save from the store
                    if (ex.Entries.ToList().Count > 0)
                    {
                        saveFailed = true;
                        ex.Entries.Single().Reload();
                    }
                }

            } while (saveFailed);
        }

        /// <summary>
        /// Return the appropriate database context
        /// </summary>
        private static RTDbContext InitializeDatabaseContext()
        {
            if (USING_REMOTE_DATABASE)
            {
                return new RTDbContext(AWSConstants.REMOTE_CONNECTION_STRING);
            }
            else
            {
                RTDbContext LocalContext = new RTDbContext();
                LocalContext.Database.CreateIfNotExists();
                SaveContext(LocalContext);
                return LocalContext;
            }
        }

        /// <summary>
        /// Populates the local database with 4 appointments for testing purposes.
        /// </summary>
        public static void PopulateLocalDatabase(int RT_id, bool local)
        {
            Random rand = new Random();

            if (local)
            {


                using (RTDbContext Context = InitializeDatabaseContext())
                {
                    List<Appointment> appts = new List<Appointment>();

                    DateTime DateTimeUniversalStart = DateTime.UtcNow.AddMinutes(1);

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

                    //List<Coordinate> coords = new List<Coordinate>();
                    //coords.AddRange(new Coordinate[] { coordinate0, coordinate1, coordinate2, coordinate3 } );
                    //Context.Coordinates.AddRange(coords);
                    //SaveContext(Context);

                    // Add drift scan appointment
                    appt0.id = 137;
                    appt0.StartTime = DateTimeUniversalStart.AddSeconds(20 + rand.Next(30));
                    appt0.EndTime = appt0.StartTime.AddSeconds(10 + rand.Next(90));
                    appt0.Status = "REQUESTED";//AppointmentStatusEnum.REQUESTED;
                    appt0.Type = "DRIFT_SCAN";//AppointmentTypeEnum.DRIFT_SCAN;
                    appt0.orientation = new Orientation(30, 30);
                    appt0.SpectraCyberConfig = new SpectraCyberConfig(SpectraCyberModeTypeEnum.CONTINUUM);
                    appt0.TelescopeId = RT_id;
                    appt0.UserId = 6;

                    // Add celesital body appointment
                    appt1.id = 137;
                    appt1.StartTime = appt0.EndTime.AddSeconds(20 + rand.Next(30));
                    appt1.EndTime = appt1.StartTime.AddSeconds(10 + rand.Next(90));
                    appt1.Status = "REQUESTED";//AppointmentStatusEnum.REQUESTED;
                    appt1.Type = "CELESTIAL_BODY";//AppointmentTypeEnum.CELESTIAL_BODY;
                    appt1.celestial_body = new CelestialBody(CelestialBodyConstants.SUN);
                    appt1.SpectraCyberConfig = new SpectraCyberConfig(SpectraCyberModeTypeEnum.SPECTRAL);
                    appt1.TelescopeId = RT_id;
                    appt1.UserId = 6;

                    // Add point appointment
                    appt2.id = 137;
                    appt2.StartTime = appt1.EndTime.AddSeconds(20 + rand.Next(30));
                    appt2.EndTime = appt2.StartTime.AddSeconds(10 + rand.Next(90));
                    appt2.Status = "REQUESTED";//AppointmentStatusEnum.REQUESTED;
                    appt2.Type = "POINT";//AppointmentTypeEnum.POINT;
                    appt2.coordinates.Add(coordinate2);
                    appt2.SpectraCyberConfig = new SpectraCyberConfig(SpectraCyberModeTypeEnum.CONTINUUM);
                    appt2.TelescopeId = RT_id;
                    appt2.UserId = 6;

                    // Add raster appointment
                    appt3.id = 137;
                    appt3.StartTime = appt2.EndTime.AddSeconds(20 + rand.Next(30));
                    appt3.EndTime = appt3.StartTime.AddMinutes(10 + rand.Next(90));
                    appt3.Status = "REQUESTED";//AppointmentStatusEnum.REQUESTED;
                    appt3.Type = "RASTER"; //AppointmentTypeEnum.RASTER;
                    appt3.coordinates.Add(coordinate0);
                    appt3.coordinates.Add(coordinate1);
                    appt3.SpectraCyberConfig = new SpectraCyberConfig(SpectraCyberModeTypeEnum.CONTINUUM);
                    appt3.TelescopeId = RT_id;
                    appt3.UserId = 6;

                    appts.AddRange(new Appointment[] { appt0, appt1, appt2, appt3 });

                    Context.Appointments.AddRange(appts);
                    SaveContext(Context);
                }
            }
            else
            {
                USING_REMOTE_DATABASE = true;
                using (RTDbContext Context = InitializeDatabaseContext())
                {
                    DateTime DateTimeUniversalStart = DateTime.UtcNow.AddMinutes(1);

                    Appointment appt0 = new Appointment();
                    Coordinate coordinate0 = new Coordinate();

                    appt0.StartTime = DateTimeUniversalStart.AddSeconds(20 + rand.Next(30));
                    appt0.EndTime = appt0.StartTime.AddSeconds(10 + rand.Next(90));
                    appt0.Status = "REQUESTED";//AppointmentStatusEnum.REQUESTED;
                    appt0.Type = "DRIFT_SCAN";//AppointmentTypeEnum.DRIFT_SCAN;
                    appt0.orientation = new Orientation(30, 30);
                    appt0.SpectraCyberConfig = new SpectraCyberConfig(SpectraCyberModeTypeEnum.CONTINUUM);
                    appt0.TelescopeId = RT_id;
                    appt0.UserId = 6;

                    Context.Appointments.Add(appt0);
                    SaveContext(Context);
                }
                //USING_REMOTE_DATABASE = false;
                //using (RTDbContext Context = InitializeDatabaseContext())
                //{
                //    DateTime DateTimeUniversalStart = DateTime.UtcNow.AddMinutes(1);

                //    Appointment appt0 = new Appointment();
                //    Coordinate coordinate0 = new Coordinate();

                //    appt0.StartTime = DateTimeUniversalStart.AddSeconds(20 + rand.Next(30));
                //    appt0.EndTime = appt0.StartTime.AddSeconds(10 + rand.Next(90));
                //    appt0.Status = "REQUESTED";//AppointmentStatusEnum.REQUESTED;
                //    appt0.Type = "DRIFT_SCAN";//AppointmentTypeEnum.DRIFT_SCAN;
                //    appt0.orientation = new Orientation(30, 30);
                //    appt0.SpectraCyberConfig = new SpectraCyberConfig(SpectraCyberModeTypeEnum.CONTINUUM);
                //    appt0.TelescopeId = RT_id;
                //    appt0.UserId = 6;

                //    Context.Appointments.Add(appt0);
                //    SaveContext(Context);
                //}
            }
        }

        /// <summary>
        /// Adds an appointment to the database
        /// </summary>
        public static void AddAppointment(Appointment appt)
        {
            if (!USING_REMOTE_DATABASE)
            {
                using (RTDbContext Context = InitializeDatabaseContext())
                {
                    Context.Appointments.Add(appt);
                    SaveContext(Context);
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
                    SaveContext(Context);
                }
            }
        }

        /// <summary>
        /// Returns the list of Appointments from the given context.
        /// </summary>
        private static DbQuery<Appointment> QueryAppointments(RTDbContext Context)
        {
            // Use Include method to load related entities from the database
            return Context.Appointments.Include("coordinates")
                                        .Include("celestial_body.coordinate")
                                        .Include("orientation")
                                        .Include("RFDatas");
                                        //.Include("SpectraCyberConfig");
        }

            /// <summary>
            /// Returns the list of Appointments from the database.
            /// </summary>
            public static List<Appointment> GetListOfAppointmentsForRadioTelescope(int radioTelescopeId)
        {
            List<Appointment> appts = new List<Appointment>();
            using (RTDbContext Context = InitializeDatabaseContext())
            {
                // Use Include method to load related entities from the database
                appts = QueryAppointments(Context).Where(x => x.TelescopeId == radioTelescopeId).ToList();
            }
            return appts;
        }

        /// <summary>
        /// Returns the updated appointment from the database.
        /// </summary>
        public static Appointment GetUpdatedAppointment(int appt_id)
        {
            Appointment appt;
            using (RTDbContext Context = InitializeDatabaseContext())
            {
                List<Appointment> appts = new List<Appointment>();
                // Use Include method to load related entities from the database
                appts = QueryAppointments(Context).ToList();
                appt = appts.Find(x => x.id == appt_id);
            }
            return appt;
        }

        /// <summary>
        /// Returns the list of Appointments from the database.
        /// </summary>
        public static int GetTotalAppointmentCount()
        {
            int count = -1;

            using (RTDbContext Context = InitializeDatabaseContext())
            {
                count = Context.Appointments.Count();
            }

            return count;
        }

        /// <summary>
        /// Creates and stores and RFData reading in the local database.
        /// </summary>
        /// <param name="data">The RFData reading to be created/stored.</param>
        public static void CreateRFData(int apptId, RFData data)
        {
            
            if (VerifyRFData(data))
            {
                USING_REMOTE_DATABASE = true;
                using (RTDbContext Context = InitializeDatabaseContext())
                {
                    var appt = Context.Appointments.Find(apptId);
                    appt.RFDatas.Add(data);
                    SaveContext(Context);
                }
                USING_REMOTE_DATABASE = false;
            }
        }

        /// <summary>
        /// Updates the appointment by saving the appt passed in.
        /// </summary>
        /// <param name="appt"> The appt that is being updated. </param>
        public static void UpdateAppointment(Appointment appt)
        { 
            if (VerifyAppointmentStatus(appt))
            {
                using (RTDbContext Context = InitializeDatabaseContext())
                {
                    // Update database appt with new status
                    var db_appt = QueryAppointments(Context).ToList().Find(x => x.id == appt.id);
                    if (db_appt != null)
                    {
                        db_appt.celestial_body = appt.celestial_body;
                        db_appt.coordinates = appt.coordinates;
                        db_appt.EndTime = appt.EndTime;
                        db_appt.orientation = appt.orientation;
                        db_appt.RFDatas = appt.RFDatas;
                        db_appt.SpectraCyberConfig = appt.SpectraCyberConfig;
                        db_appt.StartTime = appt.StartTime;
                        db_appt.Status = appt.Status;
                        db_appt.TelescopeId = appt.TelescopeId;
                        db_appt.Type = appt.Type;
                        db_appt.UserId = appt.UserId;
                        SaveContext(Context);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the next appointment from the local database context
        /// </summary>
        /// <returns></returns>
        public static Appointment GetNextAppointment(int radioTelescopeId)
        {
            Appointment appointment = null;
            using (RTDbContext Context = InitializeDatabaseContext())
            {
                //logger.Debug("Retrieving list of appointments.");
                List<Appointment> appointments = GetListOfAppointmentsForRadioTelescope(radioTelescopeId);

                if (appointments.Count > 0)
                {
                    appointments.RemoveAll(x => x.StartTime < DateTime.UtcNow || x.Status == "COMPLETED");//AppointmentStatusEnum.COMPLETED);
                    appointments.Sort();
                    logger.Debug("appointment list sorted. Starting to retrieve the next chronological appointment.");
                    appointment = appointments.Count > 0 ? appointments[0] : null;
                }
                else
                {
                    //logger.Debug("No appointments found");
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
            else if (data.TimeCaptured > DateTime.UtcNow.AddMinutes(1))
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
            return appt.Status.Equals("CANCELLED") || //AppointmentStatusEnum.CANCELLED) || 
                    appt.Status.Equals("COMPLETED") || //AppointmentStatusEnum.COMPLETED) ||
                    appt.Status.Equals("IN_PROGRESS") || //AppointmentStatusEnum.IN_PROGRESS) ||
                    appt.Status.Equals("REQUESTED") || //AppointmentStatusEnum.REQUESTED) ||
                    appt.Status.Equals("SCHEDULED");  //AppointmentStatusEnum.SCHEDULED);
        }
    }
}