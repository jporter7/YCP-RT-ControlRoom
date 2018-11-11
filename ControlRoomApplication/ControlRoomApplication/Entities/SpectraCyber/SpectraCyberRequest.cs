using ControlRoomApplication.Constants;

namespace ControlRoomApplication.Entities
{
    public class SpectraCyberRequest
    {
        // Constructor with specified parameters
        public SpectraCyberRequest(
            SpectraCyberCommandTypeEnum commandType,
            string commandString,
            bool waitForReply,
            int charsToRead,
            char responseIdentifier)
        {
            CommandType = commandType;
            Priority = CalcPriority();
            CommandString = commandString;
            WaitForReply = waitForReply;
            CharsToRead = (charsToRead <= 0) ? GenericConstants.SPECTRA_CYBER_BUFFER_SIZE : charsToRead;
            ResponseIdentifier = responseIdentifier;
        }

        // Constructor with specified parameters, default response identier to the same as the command string
        public SpectraCyberRequest(
            SpectraCyberCommandTypeEnum commandType,
            string commandString,
            bool waitForReply,
            int charsToRead)
            : this(commandType, commandString, waitForReply, charsToRead, commandString[1]) { }

        // Get the command type
        public SpectraCyberCommandTypeEnum CommandType { get; }

        // Get the command's priority
        public int Priority { get; }

        // Get or Set the command string
        public string CommandString { get; set; }

        // Get the number of characters to read from the serial port
        public int CharsToRead { get; }

        // Get whether or not the command should expect a reply
        public bool WaitForReply { get; }

        // Get the expected character identifier for a response
        public char ResponseIdentifier { get; }

        // Set the priority of this command
        private int CalcPriority()
        {
            switch (CommandType)
            {
                // Cover the unknown case first
                case SpectraCyberCommandTypeEnum.Unknown:
                    return 6;

                // Then, the empty case
                case SpectraCyberCommandTypeEnum.Empty:
                    return 5;

                //
                // Finally, the standard cases...
                //

                case SpectraCyberCommandTypeEnum.DataDiscard:
                case SpectraCyberCommandTypeEnum.DataRequest:
                case SpectraCyberCommandTypeEnum.FrequencySet:
                case SpectraCyberCommandTypeEnum.GeneralCommunication:
                case SpectraCyberCommandTypeEnum.Rescan:
                    return 4;

                case SpectraCyberCommandTypeEnum.ScanStart:
                case SpectraCyberCommandTypeEnum.ScanStop:
                    return 3;

                case SpectraCyberCommandTypeEnum.ChangeSetting:
                    return 2;

                case SpectraCyberCommandTypeEnum.Reset:
                    return 1;

                case SpectraCyberCommandTypeEnum.Terminate:
                    return 0;

                // Could not find the case, return lowest priority
                default:
                    return 6;
            }
        }

        // Check if the request is of type Empty
        public bool IsEmpty()
        {
            return CommandType == SpectraCyberCommandTypeEnum.Empty;
        }

        // Create a new Empty request
        public static SpectraCyberRequest GetNewEmpty()
        {
            return new SpectraCyberRequest(
                SpectraCyberCommandTypeEnum.Empty,
                "EMPTY",
                false,
                0
            );
        }
    }
}
