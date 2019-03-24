using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ControlRoomApplication.Controllers.PLCCommunication;
using ControlRoomApplication.Controllers.SpectraCyberController;

namespace ControlRoomApplication.Entities.RadioTelescope
{
    [Table("radio_telescope")]
    public class RadioTelescope
    {
        public RadioTelescope(AbstractSpectraCyberController spectraCyberController, PLCClientCommunicationHandler plcCommsHandler, Location location)
        {
            PLCClient = plcCommsHandler;
            SpectraCyberController = spectraCyberController;
            CurrentOrientation = new Orientation();
            CalibrationOrientation = new Orientation();
            Location = location;
        }

        //
        // This is only to be used with a local DB instance!!
        //
        public RadioTelescope(AbstractSpectraCyberController spectraCyberController, PLCClientCommunicationHandler plcCommsHandler, Location location, int localDBID)
            : this(spectraCyberController, plcCommsHandler, location)
        {
            Id = localDBID;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; }

        [Column("current_orientation")]
        public Orientation CurrentOrientation { get; set; }

        [Column("calibration_orientation")]
        public Orientation CalibrationOrientation { get; set; }

        [Column("location")]
        public Location Location { get; set; }

        public PLCClientCommunicationHandler PLCClient { get; set; }
        public AbstractSpectraCyberController SpectraCyberController { get; set; }
    }
}