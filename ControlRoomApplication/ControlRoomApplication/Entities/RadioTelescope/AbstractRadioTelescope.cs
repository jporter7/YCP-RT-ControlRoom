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

        public AbstractRadioTelescope(AbstractSpectraCyberController spectraCyberController)
        {
            CurrentStatus = RadioTelescopeStatusEnum.Unknown;
            AbstractSpectraCyberController = spectraCyberController;
            AbstractSpectraCyberController.SetParent(this);
        }

        public void IntegrateNow()
        {
            AbstractSpectraCyberController.SingleScan();
        }

        public void StartContinuousIntegration()
        {
            AbstractSpectraCyberController.StartScan();
        }

        public void StopContinuousIntegration()
        {
            AbstractSpectraCyberController.StopScan();
        }

        public void StartScheduledIntegration(int intervalMS, int delayMS, bool startAfterScan)
        {
            AbstractSpectraCyberController.ScheduleScan(intervalMS, delayMS, startAfterScan);
        }

        public abstract Orientation GetCurrentReferenceOrientation();
        public abstract bool SendReferenceVelocityCommand(double velocityAzimuth, double velocityElevation);
    }
}
