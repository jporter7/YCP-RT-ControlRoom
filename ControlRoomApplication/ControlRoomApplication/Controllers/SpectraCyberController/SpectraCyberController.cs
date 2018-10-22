using ControlRoomApplication.Entities;
using ControlRoomApplication.Main;
using ControlRoomApplication.Constants;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;

namespace ControlRoomApplication.Controllers.SpectraCyberController
{
    public class SpectraCyberController
    {
        public SpectraCyber SpectraCyber { get; set; }
        public RTDbContext Context { get; set; }

        public SpectraCyberController(SpectraCyber spectraCyber)
        {
            SpectraCyber = spectraCyber;
        }

        public void SetSpectraCyberModeType(SpectraCyberModeTypeEnum type)
        {
            SpectraCyber.CurrentModeType = type;
        }

        public bool BringUp()
        {
            try
            {
                SpectraCyber.SerialPort = new SerialPort(
                SpectraCyber.CommPort,
                GenericConstants.SPECTRA_CYBER_BAUD_RATE,
                GenericConstants.SPECTRA_CYBER_PARITY_BITS,
                GenericConstants.SPECTRA_CYBER_DATA_BITS,
                GenericConstants.SPECTRA_CYBER_STOP_BITS
                )
                {
                    ReadTimeout = GenericConstants.SPECTRA_CYBER_TIMEOUT_MS,
                    WriteTimeout = GenericConstants.SPECTRA_CYBER_TIMEOUT_MS
                };
            }
            catch (Exception e)
            {
                if (e is System.IO.IOException)
                {
                    Console.WriteLine("Failed creating serial port connection.");
                    return false;
                }
                else
                {
                    // Unexpected exception type
                    throw;
                }
            }

            try
            {
                SpectraCyber.SerialPort.Open();
            }
            catch (Exception e)
            {
                if (e is System.IO.IOException
                    || e is InvalidOperationException
                    || e is ArgumentOutOfRangeException
                    || e is ArgumentException
                    || e is UnauthorizedAccessException)
                {
                    Console.WriteLine("Failed opening serial communication.");
                    return false;
                }
                else
                {
                    // Unexpected exception type
                    throw;
                }
            }

            try
            {
                // Initialize thread and start it
                SpectraCyber.CommunicationThread = new Thread(new ThreadStart(RunCommunicationThread));
                SpectraCyber.CommunicationThread.Start();
            }
            catch (Exception e)
            {
                if (e is ArgumentNullException)
                {
                    Console.WriteLine("Failed creating communication thread.");
                    return false;
                }
                else if (e is ThreadStartException || e is OutOfMemoryException)
                {
                    Console.WriteLine("Failed starting communication thread.");
                    return false;
                }
                else
                {
                    // Unexpected exception type
                    throw;
                }
            }

            Console.WriteLine("Successfully started SpectraCyber communication and communication thread.");
            return true;
        }

        public bool BringDown()
        {
            try
            {
                if (SpectraCyber.SerialPort != null)
                {
                    SpectraCyber.SerialPort.Close();
                }
            }
            catch (Exception e)
            {
                if (e is System.IO.IOException)
                {
                    return false;
                }
                else
                {
                    // Unexpected exception type
                    throw;
                }
            }

            KillCommunicationThreadAndWait();

            Console.WriteLine("Successfully killed SpectraCyber communication and communication thread.");
            return true;
        }

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

        // Submit a command and return a response
        private SpectraCyberResponse SendCommand(SpectraCyberRequest request)
        {
            SpectraCyberResponse response = new SpectraCyberResponse();

            // If the request is empty, don't process
            if (request.IsEmpty())
            {
                return response;
            }

            try
            {
                // Attempt to write the command to the serial port
                SpectraCyber.SerialPort.Write(request.CommandString);
            }
            catch (Exception)
            {
                // Something went wrong, return the response
                return response;
            }

            // Command was successfully sent through serial communication
            response.RequestSuccessful = true;

            // Give the SpectraCyber some time to process the command
            Thread.Sleep(GenericConstants.SPECTRA_CYBER_WAIT_TIME_MS);

            // Check for any significant cases
            switch (request.CommandType)
            {
                // Termination, safely end communication
                case SpectraCyberCommandTypeEnum.Terminate:
                    BringDown();
                    break;

                // TODO: implement this case further probably
                case SpectraCyberCommandTypeEnum.ScanStop:
                    break;

                // Purge the serial buffer
                case SpectraCyberCommandTypeEnum.DataDiscard:
                    SpectraCyber.SerialPort.DiscardInBuffer();
                    break;
                
                //
                // Do nothing by default
                //
            }

            // If the request expects a reply back, capture the data and attach it to the response
            if (request.WaitForReply)
            {
                // Reponse's data is valid (assuming no exceptions are thrown)
                response.Valid = true;

                try
                {
                    // Create a character array in which to store the buffered characters
                    string hexString;

                    // Read a number of characters in the buffer
                    char[] charInBuffer = new char[GenericConstants.SPECTRA_CYBER_BUFFER_SIZE];
                    int length = SpectraCyber.SerialPort.Read(charInBuffer, 0, request.CharsToRead);

                    // Clip the string to the exact number of bytes read
                    if (GenericConstants.SPECTRA_CYBER_CLIP_BUFFER_RESPONSE && (length != GenericConstants.SPECTRA_CYBER_BUFFER_SIZE))
                    {
                        char[] actual = new char[length];

                        for (int i = 0; i < length; i++)
                            actual[i] = charInBuffer[i];

                        hexString = new string(actual);
                    }

                    // Leave the string how it is, with the possibility of chars being 0
                    else
                    {
                        hexString = new string(charInBuffer);
                    }

                    // Check to see that replyString's first character is what was expected
                    if (hexString[0] != request.ResponseIdentifier)
                    {
                        throw new Exception();
                    }

                    // Convert the hex string into an int
                    response.DecimalData = HexStringToInt(hexString.Substring(1));
                }
                catch (Exception e)
                {
                    // Something went wrong, the response isn't valid
                    Console.WriteLine("Failed to send a command: " + e.ToString());
                    response.Valid = false;
                }
            }

            // Clear the input buffer
            SpectraCyber.SerialPort.DiscardInBuffer();

            // Return the response, no matter what happened
            return response;
        }

        // Generate a SpectraCyberRequest based on currentMode
        private SpectraCyberRequest GenerateCurrentDataRequest(bool waitForReply = true, int numChars = -1)
        {
            if (numChars <= 0)
            {
                numChars = GenericConstants.SPECTRA_CYBER_BUFFER_SIZE;
            }
            
            // Based on the current mode, create the proper command string
            string commandString;

            if (SpectraCyber.CurrentModeType == SpectraCyberModeTypeEnum.Continuum)
            {
                commandString = "!D000";
            }
            else if (SpectraCyber.CurrentModeType == SpectraCyberModeTypeEnum.Spectral)
            {
                commandString = "!D001";
            }
            else
            {
                // Unknown current mode type
                throw new Exception();
            }

            return new SpectraCyberRequest(
                SpectraCyberCommandTypeEnum.DataRequest,
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
        private static int HexCharToInt(char ch)
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

        private static int HexStringToInt(string hex, int length)
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

        private static int HexStringToInt(string hex)
        {
            return HexStringToInt(hex, hex.Length);
        }

        private static string IntToHexString(int value)
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