using System;
using ControlRoomApplication.Entities.Plc;

namespace ControlRoomApplication.Main
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            // Create an instance of the DbContext (it does all of the data accessing
            // and manipulation). Then, add the user to the user table. Lastly, save the 
            // changes that were automatically checked by dbContext.ChangeTracker
            //RTDbContext dbContext = new RTDbContext();

            //foreach(Appointment app in dbContext.Appointments)
            //{
            //    Console.WriteLine(string.Join(Environment.NewLine,
            //        $"Appointment {app.Id} has a start time of {app.StartTime} and an end time of {app.EndTime}",
            //        ""));
            //}

            while(true)
            {
                PLC plc = new PLC();

                Console.WriteLine("Enter an elevation: ");
                var ele = Convert.ToInt32(Console.ReadLine());
                plc.MoveScaleModelElevation(ele);


                Console.WriteLine("Enter an azimuth: ");
                var num = Convert.ToInt32(Console.ReadLine());

                plc.MoveScaleModelAzimuth(num);


            }

            //}
            //else
            //{
            //    var num = Convert.ToInt32(command);
            //    Console.WriteLine(num);
            //    plc.MoveScaleModelElevation(num);
            //}


            Console.ReadKey(true);
        }
    }
}
