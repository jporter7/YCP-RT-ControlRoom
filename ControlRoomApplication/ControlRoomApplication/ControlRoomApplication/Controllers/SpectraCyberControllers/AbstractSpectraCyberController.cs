using System;
using System.Threading;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Database;

namespace ControlRoomApplication.Controllers
{
    public abstract class AbstractSpectraCyberController : HeartbeatInterface
    {
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected RadioTelescope Parent { get; set; }
        protected AbstractSpectraCyber SpectraCyber { get; set; }
        protected SpectraCyberScanSchedule Schedule { get; set; }

        protected Thread CommunicationThread { get; set; }
        protected bool KillCommunicationThreadFlag { get; set; }
        protected Mutex CommunicationMutex;

        public AbstractSpectraCyberController(AbstractSpectraCyber spectraCyber) : base()
        {
            SpectraCyber = spectraCyber;
            Schedule = new SpectraCyberScanSchedule(SpectraCyberScanScheduleMode.OFF);
            KillCommunicationThreadFlag = false;
            CommunicationMutex = new Mutex();
            SpectraCyber.CurrentModeType = SpectraCyberModeTypeEnum.CONTINUUM;
        }

        public RadioTelescope GetParent()
        {
            return Parent;
        }

        public void SetParent(RadioTelescope rt)
        {
            Parent = rt;
        }

        public bool SetApptConfig(Appointment appt)
        {
            bool success = false;
            SetActiveAppointmentID(appt.Id);
            SpectraCyberConfig config = appt.SpectraCyberConfig;
            SetSpectraCyberModeType(config.Mode);
            if(config.Mode == SpectraCyberModeTypeEnum.CONTINUUM)
            {
                success = SetContinuumIntegrationTime(config.IntegrationTime) && SetContinuumOffsetVoltage(config.OffsetVoltage);
            }
            else if(config.Mode == SpectraCyberModeTypeEnum.SPECTRAL)
            {
                success = SetSpectralOffsetVoltage(config.OffsetVoltage) && SetSpectralIntegrationTime(config.IntegrationTime);
            }
            else
            {
                // Unknown current mode type
                logger.Info("[AbstractSpectraCyberController] ERROR: invalid SpectraCyber mode type: " + SpectraCyber.CurrentModeType.ToString());
            }
            return success;

        }

        public void SetSpectraCyberModeType(SpectraCyberModeTypeEnum type)
        {
            SpectraCyber.CurrentModeType = type;
        }

        public void SetActiveAppointmentID(int apptId)
        {
            CommunicationMutex.WaitOne();
            SpectraCyber.ActiveAppointmentID = apptId;
            CommunicationMutex.ReleaseMutex();
        }

        public void RemoveActiveAppointmentID()
        {
            SetActiveAppointmentID(-1);
        }

        public void TestCommunication()
        {
            SpectraCyberRequest Request = new SpectraCyberRequest(
                SpectraCyberCommandTypeEnum.RESET,
                "!R000",
                true,
                4
            );

            SpectraCyberResponse Response = new SpectraCyberResponse();
            SendCommand(Request, ref Response);

            string ResponseData = Response.SerialIdentifier + Response.DecimalData.ToString("X3");
            logger.Info("[AbstractSpectraCyberController] Attempted RESET with command \"!R000\", heard back: " + ResponseData);
        }

        private bool SetSomeOffsetVoltage(double offset, char identifier)
        {
            if ((offset < 0.0) || (offset > 4.095))
            {
                logger.Info("[AbstractSpectraCyberController] ERROR: input voltage outside of range [0, 4.095]");
                return false;
            }

            int Magnitude = (int)(offset * 1000);
            string Command = "!" + identifier + IntToHexString(Magnitude);

            SpectraCyberRequest Request = new SpectraCyberRequest(
                SpectraCyberCommandTypeEnum.CHANGE_SETTING,
                Command,
                false,
                4
            );

            SpectraCyberResponse Response = new SpectraCyberResponse();
            SendCommand(Request, ref Response);

            return Response.RequestSuccessful;
        }

        public bool SetContinuumOffsetVoltage(double offset)
        {
            return SetSomeOffsetVoltage(offset, 'O');
        }

        public bool SetSpectralOffsetVoltage(double offset)
        {
            return SetSomeOffsetVoltage(offset, 'J');
        }

        private bool SetSomeIntegrationTime(SpectraCyberIntegrationTimeEnum time, char identifier)
        {
            string Command = "!" + identifier + "00" + SpectraCyberIntegrationTimeEnumHelper.GetValue(time);

            SpectraCyberRequest Request = new SpectraCyberRequest(
                SpectraCyberCommandTypeEnum.CHANGE_SETTING,
                Command,
                false,
                4
            );

            SpectraCyberResponse Response = new SpectraCyberResponse();
            SendCommand(Request, ref Response);

            return Response.RequestSuccessful;
        }

        public bool SetContinuumIntegrationTime(SpectraCyberIntegrationTimeEnum time)
        {
            return SetSomeIntegrationTime(time, 'I');
        }

        public bool SetSpectralIntegrationTime(SpectraCyberIntegrationTimeEnum time)
        {
            return SetSomeIntegrationTime(time, 'L');
        }

        // Perform a single scan, based on current mode
        protected SpectraCyberResponse DoSpectraCyberScan()
        {
            SpectraCyberResponse Response = new SpectraCyberResponse();

            CommunicationMutex.WaitOne();
            SendCommand(GenerateCurrentDataRequest(), ref Response);
            CommunicationMutex.ReleaseMutex();

            return Response;
        }

        // Start scanning, keep doing so until requested to stop
        public void SingleScan()
        {
            try
            {
                CommunicationMutex.WaitOne();
                Schedule.SetSingleMode();
                CommunicationMutex.ReleaseMutex();
            }
            catch
            {
                // ERROR
            }
        }

        // Start scanning, keep doing so until requested to stop
        public void StartScan()
        {
            try
            {
                CommunicationMutex.WaitOne();
                Schedule.SetContinuousMode();
                CommunicationMutex.ReleaseMutex();
            }
            catch
            {
                // ERROR
            }
        }

        // Stop scanning and return scan results
        public void StopScan()
        {
            try
            {
                CommunicationMutex.WaitOne();
                Schedule.SetModeOff();
                CommunicationMutex.ReleaseMutex();
            }
            catch
            {
                // ERROR
            }
        }

        // Start a scheduled scan interval
        public void ScheduleScan(int intervalMS, int delayMS, bool startAfterDelay)
        {
            try
            {
                CommunicationMutex.WaitOne();
                Schedule.SetScheduledMode(intervalMS, delayMS, startAfterDelay);
                CommunicationMutex.ReleaseMutex();
            }
            catch
            {
                // ERROR
            }
        }

        // Generate a SpectraCyberRequest based on currentMode
        protected SpectraCyberRequest GenerateCurrentDataRequest(bool waitForReply = true, int numChars = 0)
        {
            if (numChars <= 0)
            {
                numChars = AbstractSpectraCyberConstants.BUFFER_SIZE;
            }

            // Based on the current mode, create the proper command string
            string commandString;

            if (SpectraCyber.CurrentModeType == SpectraCyberModeTypeEnum.CONTINUUM)
            {
                commandString = "!D000";
            }
            else if (SpectraCyber.CurrentModeType == SpectraCyberModeTypeEnum.SPECTRAL)
            {
                commandString = "!D001";
            }
            else
            {
                // Unknown current mode type
                throw new ArgumentException("Invalid SpectraCyber mode type: " + SpectraCyber.CurrentModeType.ToString());
            }

            return new SpectraCyberRequest(
                SpectraCyberCommandTypeEnum.DATA_REQUEST,
                commandString,
                waitForReply,
                numChars
            );
        }

        public void RunCommunicationThread()
        {
            bool KeepRunningCommsThread = true;

            // Loop until the thread is attempting to be shutdown (don't directly reference SpectraCyber.KillCommunicationThreadFlag
            // because it can't be kept in the mutex's scope)
            while (KeepRunningCommsThread)
            {
                // Wait for the mutex to say it's safe to proceed
                CommunicationMutex.WaitOne();

                if (Schedule.PollReadiness())
                {
                    AddToRFDataDatabase(DoSpectraCyberScan(), SpectraCyber.ActiveAppointmentID);
                    Schedule.Consume();
                    //logger.Info("[AbstractSpectraCyberController] SC Scan");
                }

                // Tell the loop to break on its next pass (so the mutex is still released if the flag is high)
                KeepRunningCommsThread = !KillCommunicationThreadFlag;

                // Release the mutex
                CommunicationMutex.ReleaseMutex();
            }
        }

        // Implicitly kills the processing thread and waits for it to join before returning
        public void KillCommunicationThreadAndWait()
        {
            CommunicationMutex.WaitOne();
            KillCommunicationThreadFlag = true;
            CommunicationMutex.ReleaseMutex();

            CommunicationThread.Join();
        }

        protected override bool KillHeartbeatComponent()
        {
            return BringDown();
        }

        private RFData AddToRFDataDatabase(SpectraCyberResponse spectraCyberResponse, int appId)
        {
            RFData rfData = RFData.GenerateFrom(spectraCyberResponse);

            //
            // Add to database
            //
            DatabaseOperations.CreateRFData(appId, rfData);

            return rfData;
        }

        public abstract bool BringUp();
        public abstract bool BringDown();
        protected abstract void SendCommand(SpectraCyberRequest request, ref SpectraCyberResponse response);

        // TODO: implement proper error handling if ch is out of acceptable range
        protected static int HexCharToInt(char ch)
        {
            int baseVal = Convert.ToByte(ch);

            // Between [0-9]
            if (baseVal >= 48 && baseVal <= 57)
            {
                return baseVal - 48;
            }

            // Between [A-F]
            if (baseVal >= 65 && baseVal <= 70)
            {
                return baseVal - 55;
            }

            // Between [a-f]
            if (baseVal >= 97 && baseVal <= 102)
            {
                return baseVal - 87;
            }

            // Unknown
            return 0;
        }

        protected static int HexStringToInt(string hex, int length)
        {
            if (length == 0)
            {
                return 0;
            }

            if (length == 1)
            {
                return HexCharToInt(hex[0]);
            }

            return (int)(HexCharToInt(hex[0]) * Math.Pow(16, length - 1)) + HexStringToInt(hex.Substring(1), length - 1);
        }

        protected static int HexStringToInt(string hex)
        {
            return HexStringToInt(hex, hex.Length);
        }

        protected static string IntToHexString(int value, int minimumLength = 3)
        {
            string strHex = "0123456789ABCDEF";
            string strOutput = "";

            // First, encode the integer into hex
            while (value > 0)
            {
                strOutput = strHex[value % 16] + strOutput;
                value /= 16;
            }

            // Now, pad the string with zeros to make it the correct length
            for (int i = strOutput.Length; i < minimumLength; i++)
            {
                strOutput = "0" + strOutput;
            }

            // Return it
            return strOutput;
        }
    }
}