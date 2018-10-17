using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.IO.Ports;

//
// Changes made here that need to be reflected in the UML (if agreed upon):
//  - Add the entirety of the SpectraCyberCommunication class
//  - SpecraCyberModeType was renamed to SpecraCyberModeTypeEnum
//  - SpecraCyberModeTypeEnum has Unknown instead of Unspecified
//  - SpectraCyber's comm field changed to SCComms
//

namespace ControlRoomApplication.Entities
{
    public enum SpecraCyberModeTypeEnum { Unknown, Continuum, Spectral };

    public class SpectraCyber
    {
        private SpecraCyberModeTypeEnum CurrentModeType { get; set; }
        private string CommPort { get; }
        private SerialPort SCSerialPort;

        public SpectraCyber(string commPort)
        {
            CurrentModeType = SpecraCyberModeTypeEnum.Unknown;
            CommPort = commPort;

            SCSerialPort = new SerialPort(
                CommPort,
                Constants.SPECTRA_CYBER_BAUD_RATE,
                Constants.SPECTRA_CYBER_PARITY_BITS,
                Constants.SPECTRA_CYBER_DATA_BITS,
                Constants.SPECTRA_CYBER_STOP_BITS
            )
            {
                ReadTimeout = Constants.SPECTRA_CYBER_TIMEOUT_MS,
                WriteTimeout = Constants.SPECTRA_CYBER_TIMEOUT_MS
            };
        }

        public SpectraCyber() : this(Constants.SPECTRA_CYBER_DEFAULT_COMM_PORT) { }
    }
}
