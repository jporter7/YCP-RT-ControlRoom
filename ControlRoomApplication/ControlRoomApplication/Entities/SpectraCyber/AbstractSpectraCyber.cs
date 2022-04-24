namespace ControlRoomApplication.Entities
{
    public abstract class AbstractSpectraCyber
    {
        public SpectraCyberModeTypeEnum CurrentModeType { get; set; }
        public SpectraCyberRequest CurrentSpectraCyberRequest { get; set; }
        public Appointment ActiveAppointment { get; set; }

        public AbstractSpectraCyber()
        {
            CurrentModeType = SpectraCyberModeTypeEnum.UNKNOWN;
            CurrentSpectraCyberRequest = SpectraCyberRequest.GetNewEmpty();
        }
    }
}