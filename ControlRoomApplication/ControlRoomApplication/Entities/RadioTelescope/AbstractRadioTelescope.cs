using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ControlRoomApplication.Controllers.SpectraCyberController;

//
// Changes made here that need to be reflected in the UML (if agreed upon):
//  - RadioTelescopeStatusEnum has Unknown, Moving, Integrating, and MovingAndIntegrating now
//  - RadioTelescope has coordinates (InstallLocation) for where it physically is, so calculations that need that position have it available
//  - Added the Simulated and FullRadioTelescope classes
//

namespace ControlRoomApplication.Entities
{
    [Table("RadioTelescope")]
    public abstract class AbstractRadioTelescope
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        public RadioTelescopeStatusEnum CurrentStatus { get; set; }
        public AbstractSpectraCyberController AbstractSpectraCyberController { get; set; }

        // Add when Coordinate entity is ready
        //public virtual Coordinate InstallLocation { get; set; }

        public AbstractRadioTelescope(AbstractSpectraCyberController spectraCyberController)
        {
            CurrentStatus = RadioTelescopeStatusEnum.Unknown;
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

        public abstract Orientation GetCurrentReferenceOrientation();
        public abstract bool SendReferenceVelocityCommand(double velocityAzimuth, double velocityElevation);

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
    }
}
