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
                                  double offset_voltage, double if_gain, SpectraCyberDCGainEnum dc_gain, SpectraCyberBandwidthEnum bandwidth)
        {
            Mode = mode;
            IntegrationTime = integration_time;
            OffsetVoltage = offset_voltage;
            IFGain = if_gain;
            DCGain = dc_gain;
            Bandwidth = bandwidth;
        }

        public SpectraCyberConfig(SpectraCyberModeTypeEnum mode)
        {
            Mode = mode;
            IntegrationTime = SpectraCyberIntegrationTimeEnum.MID_TIME_SPAN;
            OffsetVoltage = 0;
            IFGain = 10;
            DCGain = SpectraCyberDCGainEnum.X1;
            Bandwidth = SpectraCyberBandwidthEnum.SMALL_BANDWIDTH;
        }

        public SpectraCyberConfig() : this(SpectraCyberModeTypeEnum.UNKNOWN) { }

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
        public SpectraCyberDCGainEnum DCGain { get; set; }

        [Required]
        [Column("bandwidth")]
        public SpectraCyberBandwidthEnum Bandwidth { get; set; }

        /// <summary>
        /// Checks if the current SpectraCyberConfig is Equal to another SpectraCyberConfig  
        /// and it checks if the other SpectraCyberConfig is null
        /// </summary>
        public override bool Equals(object obj)
        {
            SpectraCyberConfig other = obj as SpectraCyberConfig; //avoid double casting
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            return  Mode == other.Mode && 
                    IntegrationTime == other.IntegrationTime && 
                    OffsetVoltage == other.OffsetVoltage && 
                    IFGain == other.IFGain && 
                    DCGain == other.DCGain && 
                    Bandwidth == other.Bandwidth;
        }

        /// <summary>
        /// Returns the HashCode of the Orientation's Id
        /// </summary>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
