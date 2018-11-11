using ControlRoomApplication.Controllers.SpectraCyberController;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//
// Changes made here that need to be reflected in the UML (if agreed upon):
//  - RadioTelescopeStatusEnum has Unknown, Moving, Integrating, and MovingAndIntegrating now
//  - RadioTelescope has coordinates (InstallLocation) for where it physically is, so calculations that need that position have it available
//  - Added the Simulated and FullRadioTelescope classes
//

namespace ControlRoomApplication.Entities
{
    public enum RadioTelescopeStatusEnum { Unknown, ShutDown, Idle, Moving, Integrating, MovingAndIntegrating };

    [Table("RadioTelescope")]
    public abstract class RadioTelescope
    {
        public RadioTelescope()
        {
            CurrentStatus = RadioTelescopeStatusEnum.Unknown;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        public RadioTelescopeStatusEnum CurrentStatus { get; set; }

        // Add when Coordinate entity is ready
        //public virtual Coordinate InstallLocation { get; set; }

        public abstract Orientation GetCurrentReferenceOrientation();
        public abstract bool SendReferenceVelocityCommand(double velocityAzimuth, double velocityElevation);
    }

    public class SimulatedTelescope : RadioTelescope
    {
        public SimulatedTelescope()
        {
            // Nothing extra at the moment
        }

        public override Orientation GetCurrentReferenceOrientation()
        {
            throw new System.NotImplementedException();
        }

        public override bool SendReferenceVelocityCommand(double velocityAzimuth, double velocityElevation)
        {
            throw new System.NotImplementedException();
        }
    }

    public class FullTelescope : RadioTelescope
    {
        private AbstractSpectraCyberController FTSpectraCyberController;

        public FullTelescope()
        {
            FTSpectraCyberController = new AbstractSpectraCyberController(new AbstractSpectraCyber());
            FTSpectraCyberController.BringUp();
        }

        public override Orientation GetCurrentReferenceOrientation()
        {
            throw new System.NotImplementedException();
        }

        public override bool SendReferenceVelocityCommand(double velocityAzimuth, double velocityElevation)
        {
            throw new System.NotImplementedException();
        }
    }
}
