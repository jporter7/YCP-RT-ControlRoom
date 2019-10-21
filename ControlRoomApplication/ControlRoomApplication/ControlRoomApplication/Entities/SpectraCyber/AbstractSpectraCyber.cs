namespace ControlRoomApplication.Entities
{
    public abstract class AbstractSpectraCyber
    {
        public SpectraCyberModeTypeEnum CurrentModeType { get; set; }
        public SpectraCyberRequest CurrentSpectraCyberRequest { get; set; }
        public int ActiveAppointmentID { get; set; }

        public AbstractSpectraCyber()
        {
            CurrentModeType = SpectraCyberModeTypeEnum.UNKNOWN;
            CurrentSpectraCyberRequest = SpectraCyberRequest.GetNewEmpty();
            ActiveAppointmentID = -1;
        }
    }
}