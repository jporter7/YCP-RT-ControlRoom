namespace ControlRoomApplication.Entities
{
    public class SpectraCyberResponse
    {
        public SpectraCyberResponse()
        {
            RequestSuccessful = false;
            Valid = false;
            DecimalData = 0;
        }

        // Whether or not the command was successfully sent
        public bool RequestSuccessful { get; set; }

        // Whether or not the response is valid and populated
        public bool Valid { get; set; }

        // The decimal value pertaining to this response
        public int DecimalData { get; set; }
    }
}
