using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            RemoteContext = new RTDbContext(AWSConstants.REMOTE_CONNECTION_STRING);
        }

        /// <summary>
        /// Initialize only the local database, for testing purposes only.
        /// </summary>
        public static void InitializeLocalConnectionOnly()
        {
            LocalContext = new RTDbContext();
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