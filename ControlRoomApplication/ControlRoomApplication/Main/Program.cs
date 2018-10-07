using System;
using System.Collections.Generic;

namespace ControlRoomApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create an instance of the DbContext (it does all of the data accessing
            // and manipulation). Then, add the user to the user table. Lastly, save the 
            // changes that were automatically checked by dbContext.ChangeTracker
            RTDbContext dbContext = new RTDbContext();

            foreach(Appointment app in dbContext.Appointments)
            {
                Console.WriteLine(string.Join(Environment.NewLine,
                    $"Appointment {app.Id} has a start time of {app.StartTime} and an end time of {app.EndTime}",
                    ""));
            }

            Console.ReadKey(true);
        }
    }
}
