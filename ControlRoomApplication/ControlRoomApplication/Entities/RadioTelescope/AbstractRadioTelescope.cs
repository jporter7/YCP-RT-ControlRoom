using ControlRoomApplication.Controllers.PLCController;
using ControlRoomApplication.Entities.Plc;
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
        public Orientation CalibrationOrientation { get; set; }
    }
}