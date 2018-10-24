using ControlRoomApplication.Entities;
using ControlRoomApplication.Main;
using ControlRoomApplication.Constants;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ControlRoomApplication.Controllers.SpectraCyberController
{
    public abstract class AbstractSpectraCyberController
    {
        public AbstractSpectraCyber SpectraCyber { get; set; }
        public RTDbContext Context { get; set; }

        public AbstractSpectraCyberController(AbstractSpectraCyber spectraCyber)
        {
            SpectraCyber = spectraCyber;
        }

        public void SetSpectraCyberModeType(SpectraCyberModeTypeEnum type)
        {
            SpectraCyber.CurrentModeType = type;
        }

        public abstract bool BringUp();

        public abstract bool BringDown();

        // Submit a command and return a response
        protected abstract SpectraCyberResponse SendCommand(SpectraCyberRequest request);

        // Scan once, based on current mode
        public SpectraCyberResponse ScanOnce()
        {
            return SendCommand(GenerateCurrentDataRequest());
        }

        // Start scan
        // TODO: implement a start time and stop time option
        public void StartScan()
        {
            SpectraCyber.CurrentSpectraCyberRequest = GenerateCurrentDataRequest();
            SpectraCyber.ResponseList.Clear();
            SpectraCyber.CommunicationThreadActive = true;
        }

        // Stop scan (return scan info)
        public List<SpectraCyberResponse> StopScan()
        {
            SpectraCyber.CommunicationThreadActive = false;
            return SpectraCyber.ResponseList;
        }

        public List<SpectraCyberResponse> ScanFor(int durationMilliseconds, int startDelayMillseconds = 0)
        {
            if (startDelayMillseconds > 0)
            {
                Thread.Sleep(startDelayMillseconds);
            }

            StartScan();
            Thread.Sleep(durationMilliseconds);
            return StopScan();
        }

        // Generate a SpectraCyberRequest based on currentMode
        protected SpectraCyberRequest GenerateCurrentDataRequest(bool waitForReply = true, int numChars = -1)
        {
            if (numChars <= 0)
            {
                numChars = AbstractSpectraCyberConstants.SPECTRA_CYBER_BUFFER_SIZE;
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
                throw new Exception();
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
            // Loop until the thread is attempting to be shutdown
            while (!SpectraCyber.KillCommunicationThreadFlag)
            {
                // Process if the thread is set to be active, otherwise don't send commands
                if (SpectraCyber.CommunicationThreadActive)
                {
                    SpectraCyber.ResponseList.Add(SendCommand(SpectraCyber.CurrentSpectraCyberRequest));
                }
            }
        }

        // Implicitly kills the processing thread and waits for it to join before returning
        public void KillCommunicationThreadAndWait()
        {
            SpectraCyber.KillCommunicationThreadFlag = true;
            SpectraCyber.CommunicationThread.Join();
        }

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

        protected static string IntToHexString(int value)
        {
            string strHex = "0123456789ABCDEF";
            string strTemp = "";

            // First, encode the integer into hex (but backward)
            while (value > 0)
            {
                strTemp = strHex[value % 16] + strTemp;
                value /= 16;
            }

            // Now, pad the string with zeros to make it the correct length
            for (int i = strTemp.Length; i < 4; i++)
            {
                strTemp = "0" + strTemp;
            }

            // Finally, reverse the direction of the string
            string strOutput = "";
            foreach (char ch in strTemp.ToCharArray())
                strOutput = ch + strOutput;

            // Return it
            return strOutput;
        }
    }
}