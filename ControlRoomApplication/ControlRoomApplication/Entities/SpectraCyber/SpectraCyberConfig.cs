using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlRoomApplication.Entities
{
    [Table("spectracyber_config")]
    [Serializable]
    public class SpectraCyberConfig
    {
        public SpectraCyberConfig(SpectraCyberModeTypeEnum mode, SpectraCyberIntegrationTimeEnum integration_time, 
                                  double offset_voltage, double if_gain, double dc_gain, double bandwidth)
        {
            Mode = mode;
            IntegrationTime = integration_time;
            OffsetVoltage = offset_voltage;
            IFGain = if_gain;
            DCGain = dc_gain;
            Bandwidth = bandwidth;
        }

        public SpectraCyberConfig()
        {
            Mode = SpectraCyberModeTypeEnum.UNKNOWN;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("mode")]
        public SpectraCyberModeTypeEnum Mode { get; set; }

        [Required]
        [Column("integration_time")]
        public SpectraCyberIntegrationTimeEnum IntegrationTime { get; set; }

        [Required]
        [Column("offset_voltage")]
        public double OffsetVoltage { get; set; }

        [Required]
        [Column("if_gain")]
        public double IFGain { get; set; }

        [Required]
        [Column("dc_gain")]
        public double DCGain { get; set; }

        [Required]
        [Column("bandwidth")]
        public double Bandwidth { get; set; }

    }
}
