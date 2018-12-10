using System;

namespace ControlRoomApplication.Entities
{
    public class SpectraCyberResponse
    {
        // Whether or not the command was successfully sent
        public bool RequestSuccessful { get; set; }

        // Whether or not the response is valid and populated
        public bool Valid { get; set; }

        // The identifier of the response string
        public char SerialIdentifier { get; set; }

        // The decimal value pertaining to this response
        public int DecimalData { get; set; }

        // The moment this data was captured
        public DateTime DateTimeCaptured { get; set; }

        public SpectraCyberResponse()
        {
            RequestSuccessful = false;
            Valid = false;
            SerialIdentifier = (char)0;
            DecimalData = 0;
            DateTimeCaptured = DateTime.MinValue;
        }
    }
}
