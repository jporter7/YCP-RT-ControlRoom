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
            CharsToRead = (charsToRead <= 0) ? AbstractSpectraCyberConstants.BUFFER_SIZE : charsToRead;
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
                case SpectraCyberCommandTypeEnum.UNKNOWN:
                    return 6;

                // Then, the empty case
                case SpectraCyberCommandTypeEnum.EMPTY:
                    return 5;

                //
                // Finally, the standard cases...
                //

                case SpectraCyberCommandTypeEnum.DATA_DISCARD:
                case SpectraCyberCommandTypeEnum.DATA_REQUEST:
                case SpectraCyberCommandTypeEnum.FREQUENCY_SET:
                case SpectraCyberCommandTypeEnum.GENERAL_COMMUNICATION:
                case SpectraCyberCommandTypeEnum.RESCAN:
                    return 4;

                case SpectraCyberCommandTypeEnum.SCAN_START:
                case SpectraCyberCommandTypeEnum.SCAN_STOP:
                    return 3;

                case SpectraCyberCommandTypeEnum.CHANGE_SETTING:
                    return 2;

                case SpectraCyberCommandTypeEnum.RESET:
                    return 1;

                case SpectraCyberCommandTypeEnum.TERMINATE:
                    return 0;

                // Could not find the case, return lowest priority
                default:
                    return 6;
            }
        }

        // Check if the request is of type Empty
        public bool IsEmpty()
        {
            return CommandType == SpectraCyberCommandTypeEnum.EMPTY;
        }

        // Create a new Empty request
        public static SpectraCyberRequest GetNewEmpty()
        {
            return new SpectraCyberRequest(
                SpectraCyberCommandTypeEnum.EMPTY,
                "EMPTY",
                false,
                0
            );
        }
    }
}
