using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using ControlRoomApplication.Constants;

namespace ControlRoomApplication.Validation {
    
    public class Validator
    {
     
        /// <summary>
        /// Used to determine if a port number is in the acceptable range. Helpful for user validation
        /// </summary>
        /// <param name="port"> port number, as an integer </param>
        /// <returns> true if the port is valid (within acceptable range), otherwise false</returns>
        public static bool ValidatePort(int port)
        {
            return (port <= MiscellaneousConstants.MAX_PORT_VALUE 
                && port >= MiscellaneousConstants.MIN_PORT_VALUE);

            
        }

        /// <summary>
        /// Alternate method to handle the case where the input is coming from a text box
        /// </summary>
        /// <param name="text"> text from the textbox to parse</param>
        /// <returns> true if the string is a valid port number, false otherwise </returns>
        public static bool ValidatePort(string text)
        {
            int port;
            int.TryParse(text, out port);
            // luckily, since port 0 is reserved, we don't need to check if
            // port would be 0 from the parse failing. It handles both cases!
            return (port <= MiscellaneousConstants.MAX_PORT_VALUE
                && port >= MiscellaneousConstants.MIN_PORT_VALUE);
        }


       /// <summary>
       /// Validating that a string input by user into IP field is a valid IP Address
       /// </summary>
       /// <param name="ip"> string to be converted </param>
       /// <returns> true if the IP is valid, false if the string throws an exception
       /// meaning it was invalid.</returns>
        public static bool ValidateIPAddress(string ip)
        {
            IPAddress address;
            try {
               address = IPAddress.Parse(ip);
            }catch (FormatException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
            
        }

      
        /// <summary>
        ///  Validating speed from user input in RTControlForm
        /// </summary>
        /// <param name="speed"> speed value, given by user in RPMs </param>
        /// <returns> true if the value is between the accepted range, false otherwise</returns>
        public static bool ValidateSpeed(double speed)
        {
            return (speed <= MiscellaneousConstants.MAX_SPEED_RPM 
                && speed >= MiscellaneousConstants.MIN_SPEED_RPM);
        }

        /// <summary>
        /// Helper method to validate via a text box
        /// </summary>
        /// <param name="text"> user text to validate </param>
        /// <returns> returns true, if the textbox value is a valid double. False otherwise</returns>
        public static bool ValidateSpeed(string text)
        {
            double speed;
            try
            {
                speed = Double.Parse(text);
            }
            catch(FormatException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return (speed <= MiscellaneousConstants.MAX_SPEED_RPM
                && speed >= MiscellaneousConstants.MIN_SPEED_RPM);
        }


        public static bool ValidateOffsetVoltage(double volts)
        {
            return (volts >= 0 && volts <= 4.095);
        }

        public static bool ValidateOffsetVoltage(string text)
        {
            double volts;
            try
            {
                volts = Double.Parse(text);
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return (volts >= 0 && volts <= 4.095);
        }

        public static bool ValidateIFGain(string text)
        {
            double decibles;
            try
            {
                decibles = Double.Parse(text);
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return (decibles >= 10.00 && decibles <= 25.75);
        }

        public static bool ValidateIFGain(double decibles)
        {
            return (decibles >= 10.00 && decibles <= 25.75);
        }

        public static bool ValidateFrequency(string text)
        {
            double hertz;
            try
            {
                hertz = Double.Parse(text);
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return (hertz >= 0.0);
        }

        public static bool ValidateFrequency(double hertz)
        {
            return (hertz >= 0.0);
        }


    }

}

