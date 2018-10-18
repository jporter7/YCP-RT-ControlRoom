using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.IO.Ports;

//
// Changes made here that need to be reflected in the UML (if agreed upon):
//  - SpectraCyberModeType was renamed to SpectraCyberModeTypeEnum
//  - SpectraCyberModeTypeEnum has Unknown instead of Unspecified
//

namespace ControlRoomApplication.Entities
{
    public enum SpectraCyberModeTypeEnum { Unknown, Continuum, Spectral };

    public class SpectraCyber
    {
        public SpectraCyberModeTypeEnum CurrentModeType { get; set; }
        public string CommPort { get; }
        public SerialPort SCSerialPort;

        public SpectraCyber(string commPort)
        {
            CurrentModeType = SpectraCyberModeTypeEnum.Unknown;
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
