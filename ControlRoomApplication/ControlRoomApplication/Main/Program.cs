#define INCLUDE_SPECTRACYBER_ROUTINE
#define SIMULATED_SC

using System;
using ControlRoomApplication.Entities;

using System.Threading;
using System.Collections.Generic;
using ControlRoomApplication.Controllers.RadioTelescopeController;
using ControlRoomApplication.Controllers.SpectraCyberController;

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
                foreach (Appointment app in dbContext.Appointments)
                {
                    Console.WriteLine(string.Join(Environment.NewLine,
                        $"Appointment {app.Id} has a start time of {app.StartTime} and an end time of {app.EndTime}",
                        ""));
                }

                Console.ReadKey(true);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey(true);
            }

#if INCLUDE_SPECTRACYBER_ROUTINE
            int NumSingleScans = 5;
            int ReadContinuousMS = 2000;
            int NumElementsPerRow = 10;

#if SIMULATED_SC
            SpectraCyberSimulator sc = new SpectraCyberSimulator();
            SpectraCyberSimulatorController sc_cont = new SpectraCyberSimulatorController(sc);
#else
            SpectraCyber sc = new SpectraCyber();
            SpectraCyberController sc_cont = new SpectraCyberController(sc);
#endif
            sc_cont.SetSpectraCyberModeType(SpectraCyberModeTypeEnum.CONTINUUM);
            sc_cont.BringUp();

            SimulatedTelescope rt_sim = new SimulatedTelescope(sc_cont);
            RadioTelescopeController rt_cont = new RadioTelescopeController(rt_sim);

            Thread.Sleep(1000);

            RFData SingleData;
            Console.WriteLine("Single data scans:");
            for (int i = 0; i < NumSingleScans; i++)
            {
                SingleData = rt_cont.GetCurrentRadioTelescopeRFData();
                Console.WriteLine("\t#" + i + ": " + SingleData.Intensity);
            }

            if (rt_cont.StartRadioTelscopeRFDataScan())
            {
                
                Console.WriteLine("Starting scan for " + ReadContinuousMS + " milliseconds...");
                Thread.Sleep(ReadContinuousMS);

                List<RFData> ListData = rt_cont.StopRadioTelescopeRFDataScan();
                Console.WriteLine("List data [" + ListData.Count + " elements]:");

                int index = 0;
                string PrintStatement = "";
                foreach (RFData data in ListData)
                {
                    PrintStatement += ("\t" + data.Intensity);

                    if (++index % NumElementsPerRow == 0)
                    {
                        PrintStatement += "\n";
                    }
                }

                Console.WriteLine(PrintStatement);
            }
            else
            {
                Console.WriteLine("ERROR starting scan...");
            }

            Thread.Sleep(1000);

            sc_cont.BringDown();

            Console.WriteLine("Done. Exiting in 15 seconds...");
            Thread.Sleep(15000);
#endif
        }
    }
}
