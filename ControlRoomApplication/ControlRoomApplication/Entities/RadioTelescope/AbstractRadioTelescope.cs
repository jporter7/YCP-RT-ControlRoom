using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ControlRoomApplication.Controllers.PLCCommunication;
using ControlRoomApplication.Controllers.SpectraCyberController;

namespace ControlRoomApplication.Entities.RadioTelescope
{
    [Table("radio_telescope")]
    public abstract class AbstractRadioTelescope
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; }

        [Column("status")]
        public RadioTelescopeStatusEnum Status { get; set; }

        [Column("current_orientation")]
        public Orientation CurrentOrientation { get; set; }

        [Column("location")]
        public Location Location;

        public PLCClientCommunicationHandler PlcController { get; set; }
        public AbstractSpectraCyberController SpectraCyberController { get; set; }
        public Orientation CalibrationOrientation { get; set; }
    }
}