using System;
using System.Collections.Generic;

namespace ControlRoomApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create a user entity to store in the database later on.
            Console.WriteLine("Enter a username to create a user: ");
            string userName = Console.ReadLine();

            User user = new User();
            user.Appointments = new List<Appointment>();
            user.Username = userName;

            // Create an instance of the DbContext (it does all of the data accessing
            // and manipulation). Then, add the user to the user table. Lastly, save the 
            // changes that were automatically checked by dbContext.ChangeTracker
            RTDbContext dbContext = new RTDbContext();
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            Console.WriteLine("An appointment is being created for this user for the current time.");
            Appointment appointment = new Appointment();
            appointment.StartTime = DateTime.Now;
            appointment.EndTime = DateTime.Now;
            appointment.User = dbContext.Users.Find(user.Id);
            dbContext.Appointments.Add(appointment);
            dbContext.SaveChanges();

            foreach (User u in dbContext.Users)
            {
                Console.WriteLine(string.Join(Environment.NewLine, 
                    $"User {u.Id} has a username of {u.Username} and {u.Appointments.Count} appointments"
                    , ""));
                
            }

            foreach(Appointment app in dbContext.Appointments)
            {
                Console.WriteLine(string.Join(Environment.NewLine,
                    $"Appointment {app.Id} has a start time of {app.StartTime} and an end time of {app.EndTime}",
                    $"The user associated with this appointment is {app.User.Username}"),
                    "");
                
            }

            Console.ReadKey(true);
        }
    }
}
