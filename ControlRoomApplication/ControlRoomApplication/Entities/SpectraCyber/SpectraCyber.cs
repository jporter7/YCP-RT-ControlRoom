using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;

//
// Changes made here that need to be reflected in the UML (if agreed upon):
//  - SpectraCyberModeType was renamed to SpectraCyberModeTypeEnum
//  - SpectraCyberModeTypeEnum has Unknown instead of Unspecified
//  - SpectraCyberCommandTypeEnum, SpectraCyberRequest, and SpectraCyberResponse were added
//  - BringUnitOnline and BringUnitOffline are now of return type bool
//  - Add all of the attributes used for handling threading
//  - BringUnitOnline and BringUnitOffline were changed to BringUp and BringDown, respectively
//

namespace ControlRoomApplication.Entities
{
    public enum SpectraCyberModeTypeEnum
    {
        Unknown,
        Continuum,
        Spectral
    }

    public enum SpectraCyberCommandTypeEnum
    {
        // Default value is unknown
        Unknown,

        // Specifies an empty command that should not be processed, null-like behavior
        Empty,

        // Actual commands, in a general order of priority
        // Nothing below here has to be permanent, this is what I just put for the time being
        Terminate,
        Reset,
        ChangeSetting,
        ScanStart,
        ScanStop,
        Rescan,
        DataRequest,
        FrequencySet,
        DataDiscard,
        GeneralCommunication
    }

    public class SpectraCyber
    {
        public SpectraCyberModeTypeEnum CurrentModeType { get; set; }
        public string CommPort { get; }
        public SerialPort SerialPort { get; set; }

        // Attributes that handle inter-process communication
        // TODO: implement proper inter-process communication standards (i.e., a mutex for ResponseList and maybe CurrentSpectraCyberRequest)
        public bool CommunicationThreadActive { get; set; }
        public Thread CommunicationThread { get; set; }
        public bool KillCommunicationThreadFlag { get; set; }
        public SpectraCyberRequest CurrentSpectraCyberRequest { get; set; }
        public List<SpectraCyberResponse> ResponseList { get; set; }

        public SpectraCyber(string commPort)
        {
            CurrentModeType = SpectraCyberModeTypeEnum.Unknown;
            CommPort = commPort;

            CommunicationThreadActive = false;
            KillCommunicationThreadFlag = false;
            CurrentSpectraCyberRequest = SpectraCyberRequest.GetNewEmpty();
            ResponseList = new List<SpectraCyberResponse>();
        }

        public SpectraCyber() : this(Constants.SPECTRA_CYBER_DEFAULT_COMM_PORT) { }
    }
}
