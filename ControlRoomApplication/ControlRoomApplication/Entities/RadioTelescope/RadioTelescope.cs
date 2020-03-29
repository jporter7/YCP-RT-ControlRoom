using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Controllers.BlkHeadUcontroler;
using System;

namespace ControlRoomApplication.Entities
{
    [Table("radio_telescope")]
    public class RadioTelescope
    {

        public RadioTelescope()
        {

        }

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
        public int Id { get; set; }

        [Column("online")]
        public int online { get; set; }

        [Column("current_orientation_id")]
        public int current_orientation { get; set; }
    //    [ForeignKey("current_orientation_id")]
        [NotMapped]
        public Orientation CurrentOrientation { get; set; }

        [Column("calibration_orientation_id")]
        public int calibration_orientation { get; set; }
    //    [ForeignKey("calibration_orientation_id")]
        [NotMapped]
        public Orientation CalibrationOrientation { get; set; }

        [NotMapped]
        public Location Location { get; set; }

        [NotMapped]
        public AbstractPLCDriver PLCDriver { get; set; }

        [NotMapped]
        public AbstractSpectraCyberController SpectraCyberController { get; set; }

        [NotMapped]
        public AbstractMicrocontroller Micro_controler { get; set; }

        [NotMapped]
        public AbstractEncoderReader Encoders { get; set; }

        [NotMapped]
        protected RadioTelescopeController Parent;

        public RadioTelescopeController GetParent()
        {
            return Parent;
        }

        public void SetParent(RadioTelescopeController rt)
        {
            Parent = rt;
        }

        [NotMapped]
        public AbstractWeatherStation WeatherStation { get; set; }
    }
}