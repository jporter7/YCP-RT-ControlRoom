using System;
using System.Collections.Generic;
using System.Linq;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Controllers.ScheduleController;
using ControlRoomApplication.Entities;

namespace ControlRoomApplication.Main
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create an instance of the DbContext (it does all of the data accessing
            // and manipulation). Then, add the user to the user table. Lastly, save the 
            // changes that were automatically checked by dbContext.ChangeTracker
            try
            {
                RTDbContext dbContext = new RTDbContext(AWSConstants.REMOTE_CONNECTION_STRING);

                Console.WriteLine("Appointments table: \n");
                var schedule = new Schedule();
                var appts = dbContext.Appointments.OrderBy(x => x.StartTime).ToList();
                var appt = appts[0];

                Console.WriteLine($"The next appointment for the radio-telescope starts at {appt.StartTime}.\n");

                foreach(Appointment app in dbContext.Appointments)
                {
                    Console.WriteLine(string.Join(Environment.NewLine,
                        $"Appointment {app.Id} has a start time of {app.StartTime} and an end time of {app.EndTime}. ",
                        ""));
                }

                Console.WriteLine("Would you like to add some RFData? (y/n)");
                var input = Console.ReadLine();

                if (input.ToLower().Equals("y"))
                {
                    RFData rf = new RFData();
                    rf.TimeCaptured = DateTime.Now;

                    rf.Intensity = 4245345;
                    rf.AppointmentId = 1;

                    //dbContext.RFDatas.Add(rf);
                    //dbContext.SaveChanges();

                }

                foreach(RFData rfData in dbContext.RFDatas)
                {
                    Console.WriteLine(string.Join(Environment.NewLine,
                        $"RFdata {rfData.Id} has an intensity of {rfData.Intensity} and a timestamp of {rfData.TimeCaptured}.",
                        ""));
                }

                dbContext.Database.Connection.Close();
                Console.ReadKey(true);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey(true);
            }
        }
    }
}
