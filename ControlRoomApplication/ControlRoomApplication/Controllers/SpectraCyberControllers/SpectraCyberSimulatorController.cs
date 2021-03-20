using System;
using System.Threading;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Util;


namespace ControlRoomApplication.Controllers
{
    public class SpectraCyberSimulatorController : AbstractSpectraCyberController
    {
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Random random;

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
                    logger.Info(Utilities.GetTimeStamp() + ": [SpectraCyberSimulatorController] Failed creating communication thread.");
                    return false;
                }
                else if (e is ThreadStartException || e is OutOfMemoryException)
                {
                    logger.Info(Utilities.GetTimeStamp() + ": [SpectraCyberSimulatorController] Failed starting communication thread.");
                    return false;
                }
                else
                {
                    // Unexpected exception type
                    throw;
                }
            }

            logger.Info(Utilities.GetTimeStamp() + ": [SpectraCyberSimulatorController] Successfully started SpectraCyber communication and communication thread.");
            return true;
        }

        public override bool BringDown()
        {
            KillCommunicationThreadAndWait();

            logger.Info(Utilities.GetTimeStamp() + ": [SpectraCyberSimulatorController] Successfully killed SpectraCyber communication and communication thread.");
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
                /// <TODO>
                /// May need to update to more accurately match the data seen from the real spectra cyber
                int minIntensityScaled = (int)(AbstractSpectraCyberConstants.SIMULATED_RF_INTENSITY_MINIMUM / AbstractSpectraCyberConstants.SIMULATED_RF_INTENSITY_DISCRETIZATION);
                int maxIntensityScaled = (int)(AbstractSpectraCyberConstants.SIMULATED_RF_INTENSITY_MAXIMUM / AbstractSpectraCyberConstants.SIMULATED_RF_INTENSITY_DISCRETIZATION);
                response.DecimalData = random.Next(minIntensityScaled, maxIntensityScaled + 1);

                // Set the time captured to be as close to the (simulated) read as possible
                response.DateTimeCaptured = DateTime.UtcNow;
            }

            // Do nothing to purge a simulated buffer
        }

        protected override bool TestIfComponentIsAlive()
        {
            return true;
        }

    }
}