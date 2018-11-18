using ControlRoomApplication.Controllers.PLCController;
using ControlRoomApplication.Controllers.SpectraCyberController;
using ControlRoomApplication.Entities.Plc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//
// Changes made here that need to be reflected in the UML (if agreed upon):
//  - RadioTelescopeStatusEnum has Unknown, Moving, Integrating, and MovingAndIntegrating now
//  - RadioTelescope has coordinates (InstallLocation) for where it physically is, so calculations that need that position have it available
//  - Added the Simulated and FullRadioTelescope classes
//

namespace ControlRoomApplication.Entities.RadioTelescope
{
    [Table("radio_telescope")]
    public abstract class AbstractRadioTelescope
    {
        public AbstractRadioTelescope() { }

        public AbstractRadioTelescope(AbstractSpectraCyberController spectraCyberController)
        {
            AbstractSpectraCyberController = spectraCyberController;
        }

        public RFData IntegrateNow()
        {
            return GenerateRFData(AbstractSpectraCyberController.ScanOnce());
        }

        public bool StartContinuousIntegration()
        {
            return AbstractSpectraCyberController.StartScan();
        }

        public List<RFData> StopContinuousIntegration()
        {
            return GenerateRFDataList(AbstractSpectraCyberController.StopScan());
        }

        private static RFData GenerateRFData(SpectraCyberResponse spectraCyberResponse)
        {
            RFData rfData = new RFData();
            rfData.TimeCaptured = spectraCyberResponse.DateTimeCaptured;
            rfData.Intensity = spectraCyberResponse.DecimalData;
            // TODO: set ID
            return rfData;
        }

        private static List<RFData> GenerateRFDataList(List<SpectraCyberResponse> spectraCyberResponses)
        {
            List<RFData> rfDataList = new List<RFData>();
            foreach (SpectraCyberResponse response in spectraCyberResponses)
            {
                rfDataList.Add(GenerateRFData(response));
            }

            return rfDataList;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; }

        [Column("status")]
        public RadioTelescopeStatusEnum Status { get; set; }

        [Column("current_orientation")]
        public Orientation CurrentOrientation { get; set; }

        public PLC Plc { get; set; }
        public PLCController PlcController { get; set; }
        public AbstractSpectraCyberController AbstractSpectraCyberController { get; set; }
        public Orientation CalibrationOrientation { get; set; }
    }
}