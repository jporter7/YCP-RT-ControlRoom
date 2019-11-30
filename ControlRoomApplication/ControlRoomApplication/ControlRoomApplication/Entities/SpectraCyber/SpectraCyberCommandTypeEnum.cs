namespace ControlRoomApplication.Entities
{
    public enum SpectraCyberCommandTypeEnum
    {
        // Default value is unknown
        UNKNOWN,

        // Specifies an empty command that should not be processed, null-like behavior
        EMPTY,

        // Actual commands, in a general order of priority
        // Nothing below here has to be permanent, this is what I just put for the time being
        TERMINATE,
        RESET,
        CHANGE_SETTING,
        SCAN_START,
        SCAN_STOP,
        RESCAN,
        DATA_REQUEST,
        FREQUENCY_SET,
        DATA_DISCARD,
        GENERAL_COMMUNICATION
    }
}