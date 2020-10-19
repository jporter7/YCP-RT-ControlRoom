using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Controllers.BlkHeadUcontroler;
using ControlRoomApplication.Main;
using System.Reflection;
using System.Data.Entity.Core.Objects;

namespace ControlRoomApplication.Database
{
    public static class DatabaseOperations
    {
        private static readonly bool USING_REMOTE_DATABASE = false;
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // used to tell if we need to create a control room user
        // if control room user is already in the database, we do not want to push any users
        // if it is not in the database, we want to create one then no more
        private static bool createUser = false;

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
                RTDbContext LocalContext = new RTDbContext(AWSConstants.LOCAL_DATABASE_STRING);
                SaveContext(LocalContext);
                return LocalContext;
            }
        }

        /// <summary>
        /// Populates the local database with 4 appointments for testing purposes.
        /// </summary>
        public static void PopulateLocalDatabase(int RT_id)
        {
            if (!USING_REMOTE_DATABASE)
            {
                Random rand = new Random();

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

                    User controlRoomUser = DatabaseOperations.GetControlRoomUser();
                    RadioTelescope tele = new RadioTelescope(new SpectraCyberController(new SpectraCyber()), new TestPLCDriver(PLCConstants.LOCAL_HOST_IP, PLCConstants.LOCAL_HOST_IP, 8089, 8089, false), new Location(), new Orientation(), 0, new SimulatedMicrocontroller(0, 100, true), new EncoderReader("192.168.7.2", 1602));
                    RadioTelescopeController rtController = new RadioTelescopeController(tele);

                    // Add drift scan appointment
                    appt0.start_time = DateTimeUniversalStart.AddSeconds(20 + rand.Next(30));
                    appt0.end_time = appt0.start_time.AddSeconds(10 + rand.Next(90));
                    appt0._Status = AppointmentStatusEnum.REQUESTED;
                    appt0._Type = AppointmentTypeEnum.DRIFT_SCAN;
                    appt0._Priority = AppointmentPriorityEnum.MANUAL;
                    appt0.SpectraCyberConfig = new SpectraCyberConfig(SpectraCyberModeTypeEnum.CONTINUUM);
                    appt0.CelestialBody = new CelestialBody("control room");
                    appt0.CelestialBody.Coordinate = new Coordinate(0, 0);
                    appt0.Orientation = rtController.GetAbsoluteOrientation();
                    appt0.Telescope = tele;
                    appt0.User = controlRoomUser;

                    DatabaseOperations.AddAppointment(appt0);

                    // Add celesital body appointment
                    appt1.start_time = appt0.end_time.AddSeconds(20 + rand.Next(30));
                    appt1.end_time = appt1.start_time.AddSeconds(10 + rand.Next(90));
                    appt1._Status = AppointmentStatusEnum.REQUESTED;
                    appt1._Type = AppointmentTypeEnum.CELESTIAL_BODY;
                    appt1._Priority = AppointmentPriorityEnum.MANUAL;
                    appt1.SpectraCyberConfig = new SpectraCyberConfig(SpectraCyberModeTypeEnum.CONTINUUM);
                    appt1.CelestialBody = new CelestialBody("control room");
                    appt1.CelestialBody.Coordinate = new Coordinate(0, 0);
                    appt1.Orientation = rtController.GetAbsoluteOrientation();
                    appt1.Telescope = tele;
                    appt1.User = controlRoomUser;

                    DatabaseOperations.AddAppointment(appt1);

                    // Add point appointment
                    appt2.start_time = appt1.end_time.AddSeconds(20 + rand.Next(30));
                    appt2.end_time = appt2.start_time.AddSeconds(10 + rand.Next(90));
                    appt2._Status = AppointmentStatusEnum.REQUESTED;
                    appt2._Type = AppointmentTypeEnum.POINT;
                    appt2._Priority = AppointmentPriorityEnum.MANUAL;
                    appt2.SpectraCyberConfig = new SpectraCyberConfig(SpectraCyberModeTypeEnum.CONTINUUM);
                    appt2.CelestialBody = new CelestialBody("control room");
                    appt2.CelestialBody.Coordinate = new Coordinate(0, 0);
                    appt2.Orientation = rtController.GetAbsoluteOrientation();
                    appt2.Telescope = tele;
                    appt2.User = controlRoomUser;

                    DatabaseOperations.AddAppointment(appt2);

                    // Add raster appointment
                    appt3.start_time = appt2.end_time.AddSeconds(20 + rand.Next(30));
                    appt3.end_time = appt3.start_time.AddMinutes(10 + rand.Next(90));
                    appt3._Status = AppointmentStatusEnum.REQUESTED;
                    appt3._Type = AppointmentTypeEnum.RASTER;
                    appt3._Priority = AppointmentPriorityEnum.MANUAL;
                    appt3.Coordinates.Add(coordinate0);
                    appt3.Coordinates.Add(coordinate1);
                    appt3.SpectraCyberConfig = new SpectraCyberConfig(SpectraCyberModeTypeEnum.CONTINUUM);
                    appt3.CelestialBody = new CelestialBody("control room");
                    appt3.CelestialBody.Coordinate = new Coordinate(0, 0);
                    appt3.Orientation = rtController.GetAbsoluteOrientation();
                    appt3.Telescope = tele;
                    appt3.User = controlRoomUser;

                    DatabaseOperations.AddAppointment(appt3);

                    SaveContext(Context);
                }
            }
            else
            {
                logger.Error("Cannot populate a non-local database!");
            }
        }

        /// <summary>
        /// Adds an appointment to the database
        /// </summary>
        public static void AddAppointment(Appointment appt)
        {
        
            using (RTDbContext Context = InitializeDatabaseContext())
            {
                if (createUser == false)
                {
                    // we have this line so that we do not add another user
                    Context.Entry(appt.User).State = EntityState.Unchanged;
                }
                else
                {
                    Context.Users.Add(appt.User);
                    createUser = false;
                }
                    
                if (Context.CelestialBodies.Any(t => t.Id == appt.CelestialBody.Id) == false)
                {
                    if (Context.Coordinates.Any(t => t.Id == appt.CelestialBody.Coordinate.Id))
                        Context.Entry(appt.CelestialBody.Coordinate).State = EntityState.Unchanged;
                    
                    Context.CelestialBodies.Add(appt.CelestialBody);
                }
                if (Context.Orientations.Any(t => t.Id == appt.Orientation.Id) == false)
                    Context.Orientations.Add(appt.Orientation);
                if (Context.SpectraCyberConfigs.Any(t => t.Id == appt.SpectraCyberConfig.Id) == false)
                    Context.SpectraCyberConfigs.Add(appt.SpectraCyberConfig);

                Context.Entry(appt.Telescope).State = EntityState.Unchanged;
                
                Context.Appointments.Add(appt);
                SaveContext(Context);

                // Add coordinates
                List<Appointment> alist = Context.Appointments.ToList<Appointment>();
                foreach (Coordinate c in appt.Coordinates)
                {
                    c.apptId = alist[alist.Count - 1].Id;
                    Context.Coordinates.AddOrUpdate(c);
                }
                SaveContext(Context);
            }
        }

        /// <summary>
        /// Returns the list of Appointments from the database.
        /// </summary>
        public static List<Appointment> GetListOfAppointmentsForRadioTelescope(int radioTelescopeId)
        {
            List<Appointment> appts = new List<Appointment>();
            using (RTDbContext Context = InitializeDatabaseContext())
            {
                try
                {

                    // Use Include method to load related entities from the database
                    var appoints = Context.Appointments.Include(t => t.Telescope)
                                                        .Include(t => t.CelestialBody)
                                                        .Include(t => t.CelestialBody.Coordinate)
                                                        .Include(t => t.Orientation)
                                                        .Include(t => t.SpectraCyberConfig)
                                                        .Include(t => t.User)
                                                        .ToList<Appointment>();

                    appts = appoints.Where(x => x.telescope_id == radioTelescopeId).ToList();

                    // Add coordinates to the appointment
                    var coordsForAppt = Context.Coordinates.ToList<Coordinate>();
                
                    foreach(Appointment a in appts)
                    {
                        foreach(Coordinate c in coordsForAppt)
                        {
                            if(c.apptId == a.Id) a.Coordinates.Add(c);
                        }
                    }
                }
                catch (Exception e)
                {
                    ;
                }
                
            }
            return appts;
        }

        /// <summary>
        /// Returns the object of the control room user that we will use for all control room movements
        /// </summary>
        public static User GetControlRoomUser()
        {
            User controlRoomUser = null;
            using (RTDbContext Context = InitializeDatabaseContext())
            {
                // Use Include method to load related entities from the database
                List<User> users = Context.Users.SqlQuery("Select * from user").ToList<User>();

                users = users.Where(x => x.first_name == "control").ToList<User>();
                
                if(users.Count() == 0)
                {
                    users.Add(new User("control", "room", "controlroom@gmail.com", NotificationTypeEnum.SMS));
                    createUser = true;
                }
                if(users.Count() > 1)
                {
                    throw new System.InvalidOperationException("Too many control room users");
                }

                controlRoomUser = users[0];
            }
            return controlRoomUser;
        }

        /// <summary>
        /// Returns a list of all Users
        /// </summary>
        public static List<User> GetAllUsers()
        {
            List<User> AllUsers = new List<User>();

            using (RTDbContext Context = InitializeDatabaseContext())
            {
                AllUsers = Context.Users.SqlQuery("Select * from user").ToList<User>();

                if(AllUsers.Count() == 0)
                {
                    AllUsers.Add(new User("control", "room", "controlroom@gmail.com", NotificationTypeEnum.ALL));
                    createUser = true;
                }
            }
            return AllUsers;
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
        /// Returns the list of Appointments from the database.
        /// </summary>
        public static int GetTotalRTCount()
        {
            int count = -1;

            using (RTDbContext Context = InitializeDatabaseContext())
            {
                count = Context.RadioTelescope.Count();
            }

            return count;
        }

        /// <summary>
        /// Returns the updated Appointment from the database.
        /// </summary>
        public static Appointment GetUpdatedAppointment(Appointment appt)
        {
            using (RTDbContext Context = InitializeDatabaseContext())
            {
                Context.Entry(appt.CelestialBody).State = EntityState.Unchanged;
                if (appt.CelestialBody.Coordinate != null)
                    Context.Entry(appt.CelestialBody.Coordinate).State = EntityState.Unchanged;
                if (appt.Orientation != null)
                    Context.Entry(appt.Orientation).State = EntityState.Unchanged;
                Context.Entry(appt.SpectraCyberConfig).State = EntityState.Unchanged;
                Context.Entry(appt.Telescope).State = EntityState.Unchanged;
                Context.Entry(appt.User).State = EntityState.Unchanged;

                Context.SaveChanges();

                Context.Appointments.Attach(appt);

                Context.Entry(appt).Reload();

            }
            return appt;
        }

        /// <summary>
        /// Creates and stores and RFData reading in the local database.
        /// </summary>
        /// <param name="data">The RFData reading to be created/stored.</param>
        public static void AddRFData(RFData data)
        {
            
            if (VerifyRFData(data))
            {

                using (RTDbContext Context = InitializeDatabaseContext())
                {

                    // add the rf data to the list in appointment
                    data.Appointment.RFDatas.Add(data);

                    // add the rf data to the database
                    Context.RFDatas.AddOrUpdate(data);

                    Context.Entry(data.Appointment.User).State = EntityState.Unchanged;
                    Context.Entry(data.Appointment).State = EntityState.Unchanged;
                    if (data.Appointment.Orientation != null)
                        Context.Entry(data.Appointment.Orientation).State = EntityState.Unchanged;
                    if (data.Appointment.CelestialBody != null)
                    {
                        if (data.Appointment.CelestialBody.Coordinate != null)
                            Context.Entry(data.Appointment.CelestialBody.Coordinate).State = EntityState.Unchanged;
                        Context.Entry(data.Appointment.CelestialBody).State = EntityState.Unchanged;
                    }
                    Context.Entry(data.Appointment.SpectraCyberConfig).State = EntityState.Unchanged;
                    Context.Entry(data.Appointment.Telescope).State = EntityState.Unchanged;

                    Context.SaveChanges();
                }
            }
        }

        public static int GetTotalRFDataCount()
        {
            int count = -1;

            using (RTDbContext Context = InitializeDatabaseContext())
            {
                count = Context.RFDatas.Count();
            }

            return count;
        }

        /// <summary>
        /// Returns the list of Appointments from the database.
        /// </summary>
        public static List<RFData> GetListOfRFData()
        {
            List<RFData> appts = new List<RFData>();
            using (RTDbContext Context = InitializeDatabaseContext())
            {
                // Use Include method to load related entities from the database
                appts = Context.RFDatas.Include(t => t.Appointment).ToList<RFData>();

            }
            return appts;
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
                    // Add appointment
                    Context.Appointments.AddOrUpdate(appt);

                    // Retrieve all coordinates for appointment
                    var coordsForAppt = Context.Coordinates.ToList<Coordinate>().Where(coord => coord.apptId == appt.Id);

                    // Delete coordinates
                    foreach(Coordinate c in coordsForAppt)
                    {
                        if (!appt.Coordinates.Any(co => co.Id == c.Id))
                        {
                            Context.Coordinates.Remove(c);
                            Context.SaveChangesAsync();
                        }
                    }

                    // Add coordinates
                    foreach (Coordinate c in appt.Coordinates)
                    {
                        c.apptId = appt.Id;
                        Context.Coordinates.AddOrUpdate(c);
                    }

                    Context.SaveChangesAsync();

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
                    appointments.RemoveAll(x => x._Status == AppointmentStatusEnum.COMPLETED || x._Status == AppointmentStatusEnum.CANCELED);
                    appointments.Sort();
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
            return  appt._Status.Equals(AppointmentStatusEnum.CANCELED) || 
                    appt._Status.Equals(AppointmentStatusEnum.COMPLETED) ||
                    appt._Status.Equals(AppointmentStatusEnum.IN_PROGRESS) ||
                    appt._Status.Equals(AppointmentStatusEnum.REQUESTED) ||
                    appt._Status.Equals(AppointmentStatusEnum.SCHEDULED);
        }
        /// <summary>
        /// add an array of sensor data to the apropriat table
        /// </summary>
        /// <param name="temp"></param>
        public static void AddSensorData( List<Temperature> temp ) {
            if(temp.Count <= 0) { return; }
            if(!USING_REMOTE_DATABASE) {
                using(RTDbContext Context = InitializeDatabaseContext()) {
                    Context.Temperatures.AddRange( temp );
                    //foreach(Temperature tump in temp) {}
                    SaveContext( Context );
                }
            }
        }
        /// <summary>
        /// add an array of sensor data to the apropriat table
        /// </summary>
        /// <param name="acc"></param>
        public static void AddSensorData( List<Acceleration> acc ) {
            if(acc.Count <= 0) { return; }
            if(!USING_REMOTE_DATABASE) {
                using(RTDbContext Context = InitializeDatabaseContext()) {
                    Context.Accelerations.AddRange( acc );
                    //foreach(Temperature tump in temp) {}
                    SaveContext( Context );
                }
            }
        }
        /// <summary>
        /// get acc between starttime and now from sensor location loc
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endTime"> currently unused</param>
        /// <param name="loc"></param>
        /// <returns></returns>
        public static List<Acceleration> GetACCData( long starttime , long endTime, SensorLocationEnum loc ) {
            using(RTDbContext Context = InitializeDatabaseContext()) {//&& x.TimeCaptured < endTime
                return Context.Accelerations.Where( x => x.TimeCaptured > starttime && x.location_ID == (int)loc ).ToList();
            }
        }

        /// <summary>
        /// get temp between starttime and now from sensor location loc
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endTime"></param>
        /// <param name="loc"></param>
        /// <returns></returns>
        public static List<Temperature> GetTEMPData( long starttime , long endTime, SensorLocationEnum loc ) {
            using(RTDbContext Context = InitializeDatabaseContext()) {// && x.TimeCaptured < endTime) )   && x.TimeCaptured.Ticks < endTime.Ticks
                return Context.Temperatures.Where( x => x.TimeCapturedUTC > starttime && x.location_ID == (int)loc ).ToList();
            }
        }

        /// <summary>
        /// returns the most recent temerature for a given location
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public static Temperature GetCurrentTemp( SensorLocationEnum loc ) {
            using(RTDbContext Context = InitializeDatabaseContext()) {// && x.TimeCaptured < endTime) )   && x.TimeCaptured.Ticks < endTime.Ticks
                try {
                    return Context.Temperatures.Where( x => x.location_ID == (int)loc ).OrderByDescending( x => x.TimeCapturedUTC ).First();
                } catch {
                    return new Temperature();
                }
            }
        }

        /// <summary>
        /// Adds the weather data
        /// </summary>
        public static void AddWeatherData(WeatherData weather)
        {
           using (RTDbContext Context = InitializeDatabaseContext())
           {
               Context.Weather.Add(weather);
               SaveContext(Context);

               logger.Info("Added weather data to database");
           }
        }

        /// <summary>
        /// Adds the sensor status data
        /// </summary>
        public static void AddSensorStatusData(SensorStatus sensors)
        {
           using (RTDbContext Context = InitializeDatabaseContext())
           {
               Context.SensorStatus.Add(sensors);
               SaveContext(Context);

               //logger.Info("Added sensor status data to database");
           }
        }

        /// <summary>
        /// Grabs the current threshold value from the database for the specific sensor
        /// </summary>
        public static double GetThresholdForSensor(SensorItemEnum item)
        {
            double val;
            using (RTDbContext Context = InitializeDatabaseContext())
            {
                List<ThresholdValues> thresholds = Context.ThresholdValues.Where<ThresholdValues>(t => t.sensor_name == item.ToString()).ToList<ThresholdValues>();

                if (thresholds.Count() != 1)
                {
                    throw new InvalidOperationException();
                }

                val = Convert.ToDouble(thresholds[0].maxValue);
            }

            return val;
        }

        /// <summary>
        /// Updates the current override to the opposite of the given sensor name
        /// </summary>
        public static void SwitchOverrideForSensor(SensorItemEnum item)
        {
            using (RTDbContext Context = InitializeDatabaseContext())
            {
                var overrides = Context.Override.Where<Override>(t => t.sensor_name == item.ToString()).ToList<Override>();
                
                if(overrides.Count() != 1)
                {
                    throw new InvalidOperationException();
                }

                Override current = overrides[0];
                current.Overridden = current.Overridden == 1 ? Convert.ToSByte(0) : Convert.ToSByte(1);

                Context.Override.AddOrUpdate(current);

                Context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Gets the current status of the override
        /// </summary>
        public static bool GetOverrideStatusForSensor(SensorItemEnum item)
        {
            Override current;
            using (RTDbContext Context = InitializeDatabaseContext())
            {
                var overrides = Context.Override.Where<Override>(t => t.sensor_name == item.ToString()).ToList<Override>();

                if (overrides.Count() != 1)
                {
                    throw new InvalidOperationException();
                }

                current = overrides[0];
            }

            return current.Overridden == 1;
        }

        /// <summary>
        /// Adds the radio telescope
        /// </summary>
        public static void AddRadioTelescope(RadioTelescope telescope)
        {
            using (RTDbContext Context = InitializeDatabaseContext())
            {
                Context.RadioTelescope.Add(telescope);
                SaveContext(Context);

                logger.Info("Added radio telescope to database");
            }
        }

        /// <summary>
        /// Adds the radio telescope
        /// </summary>
        public static RadioTelescope FetchFirstRadioTelescope()
        {
            using (RTDbContext Context = InitializeDatabaseContext())
            {
                var telescopes = Context.RadioTelescope.ToList<RadioTelescope>();

                foreach(RadioTelescope rt in telescopes)
                {
                    logger.Info("Retrieved Radio Telescope from the database");
                    return rt;
                }

                logger.Info("No Radio Telescope found in the database");
                return null;
                
            }
        }

    }
}