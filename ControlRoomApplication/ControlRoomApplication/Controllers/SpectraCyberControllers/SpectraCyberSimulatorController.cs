using System;
using System.IO;
using System.Threading;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Constants;

namespace ControlRoomApplication.Controllers.SpectraCyberController
{
    public class SpectraCyberSimulatorController : AbstractSpectraCyberController
    {
        private Random random;

/*        private int[][] ReadGrid()
        {
            int[][] grid = null;

            var reader = new StreamReader("ControlRoom")

            return grid;
        }
        */

        public SpectraCyberSimulatorController(SpectraCyberSimulator spectraCyberSimulator) : base(spectraCyberSimulator)
        {
            random = new Random();
        }

        public override bool BringUp()
        {
            try
            {
                // Initialize thread and start it
                CommunicationThread = new Thread(() => RunCommunicationThread());
                CommunicationThread.Start();
            }
            catch (Exception e)
            {
                if (e is ArgumentNullException)
                {
                    Console.WriteLine("[SpectraCyberSimulatorController] Failed creating communication thread.");
                    return false;
                }
                else if (e is ThreadStartException || e is OutOfMemoryException)
                {
                    Console.WriteLine("[SpectraCyberSimulatorController] Failed starting communication thread.");
                    return false;
                }
                else
                {
                    // Unexpected exception type
                    throw;
                }
            }

            Console.WriteLine("[SpectraCyberSimulatorController] Successfully started SpectraCyber communication and communication thread.");
            return true;
        }

        public override bool BringDown()
        {
            KillCommunicationThreadAndWait();

            Console.WriteLine("[SpectraCyberSimulatorController] Successfully killed SpectraCyber communication and communication thread.");
            return true;
        }

        // Submit a command and return a response
        protected override void SendCommand(SpectraCyberRequest request, ref SpectraCyberResponse response)
        {
            // Here is where the request would be sent through serial if this were a physical device

            // Assume it is successfully sent
            response.RequestSuccessful = true;

            // Give the simulated SpectraCyber some time to process the command
            Thread.Sleep(AbstractSpectraCyberConstants.WAIT_TIME_MS);

            // Check for any significant cases
            switch (request.CommandType)
            {
                // Termination, safely end communication
                case SpectraCyberCommandTypeEnum.TERMINATE:
                    BringDown();
                    break;
                
                //
                // Do nothing by default
                //
            }

            // If the request expects a reply back, capture the data and attach it to the response
            if (request.WaitForReply)
            {
                // Reponse's data is valid
                response.Valid = true;

                // Set the SerialIdentifier, assuming the correct type of response is heard back
                response.SerialIdentifier = request.ResponseIdentifier;

                // Generate random data
                int minIntensityScaled = (int)(AbstractSpectraCyberConstants.SIMULATED_RF_INTENSITY_MINIMUM / AbstractSpectraCyberConstants.SIMULATED_RF_INTENSITY_DISCRETIZATION);
                int maxIntensityScaled = (int)(AbstractSpectraCyberConstants.SIMULATED_RF_INTENSITY_MAXIMUM / AbstractSpectraCyberConstants.SIMULATED_RF_INTENSITY_DISCRETIZATION);
                response.DecimalData = random.Next(minIntensityScaled, maxIntensityScaled + 1);

                // Set the time captured to be as close to the (simulated) read as possible
                response.DateTimeCaptured = DateTime.Now;
            }

            // Do nothing to purge a simulated buffer
        }

        protected override bool TestIfComponentIsAlive()
        {
            return random.NextDouble() < 0.02;
        }
    }
}