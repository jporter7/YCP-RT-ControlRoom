using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//
// Changes made here that need to be reflected in the UML (if agreed upon):
//  - RadioTelescopeStatusEnum has Unknown, Moving, Integrating, and MovingAndIntegrating now
//  - RadioTelescope has coordinates (InstallLocation) for where it physically is, so calculations that need that position have it available
//

namespace ControlRoomApplication.Entities
{
    public enum RadioTelescopeStatusEnum { Unknown, ShutDown, Idle, Moving, Integrating, MovingAndIntegrating };

    [Table("radio_telescopes")]
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

        public virtual Orientation CurrentOrientation { get; set; }

        // Add when Coordinate entity is ready
        //public virtual Coordinate.Coordinate InstallLocation { get; set; }
    }

    public class FullTelescope : RadioTelescope
    {
        // Add when SpectraCyber entity is ready
        //private SpectraCyber FTSpectraCyber;
        //
        //public FullTelescope()
        //{
        //    FTSpectraCyber = new SpectraCyber();
        //}
    }
}
