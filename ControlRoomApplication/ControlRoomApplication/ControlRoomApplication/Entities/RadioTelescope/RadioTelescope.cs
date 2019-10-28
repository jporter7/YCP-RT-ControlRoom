using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Controllers.BlkHeadUcontroler;

namespace ControlRoomApplication.Entities
{
    [Table("radio_telescope")]
    public class RadioTelescope
    {
        public RadioTelescope(AbstractSpectraCyberController spectraCyberController, AbstractPLCDriver plcCommsHandler, Location location, Orientation calibrationOrientation)
        {
            PLCDriver = plcCommsHandler;
            SpectraCyberController = spectraCyberController;
            CalibrationOrientation = calibrationOrientation;
            Location = location;
            CurrentOrientation = new Orientation();
        }

        public RadioTelescope( AbstractSpectraCyberController spectraCyberController , AbstractPLCDriver plcCommsHandler , Location location , Orientation calibrationOrientation , int localDBID , AbstractMicrocontroller ctrler , AbstractEncoderReader encoder ) {
            PLCDriver = plcCommsHandler;
            SpectraCyberController = spectraCyberController;
            CalibrationOrientation = calibrationOrientation;
            Location = location;
            CurrentOrientation = new Orientation();
            Encoders = encoder;
            Micro_controler = ctrler;
            Id = localDBID;
        }

        //
        // This is only to be used with a local DB instance!!
        //
        public RadioTelescope(AbstractSpectraCyberController spectraCyberController, AbstractPLCDriver plcCommsHandler, Location location, Orientation calibrationOrientation, int localDBID)
            : this(spectraCyberController, plcCommsHandler, location, calibrationOrientation)
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

        public AbstractPLCDriver PLCDriver { get; set; }
        public AbstractSpectraCyberController SpectraCyberController { get; set; }
        public AbstractMicrocontroller Micro_controler { get; set; }
        public AbstractEncoderReader Encoders { get; set; }
    }
}