using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlRoomApplication.Entities
{
    [Table("sensor_overrides")]
    public class Override
    { 

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [NotMapped]
        public SensorItemEnum Item
        {
            get
            {
                return (SensorItemEnum)Enum.Parse(typeof(SensorItemEnum), sensor_name);
            }
            set
            {
                this.sensor_name = value.ToString();
            }
        }

        private string backingName { get; set; }

        [Required]
        [Column("sensor_name")]
        public string sensor_name
        {
            get
            {
                return this.backingName;
            }
            set
            {
                if (value == null || Enum.IsDefined(typeof(SensorItemEnum), value))
                {
                    this.backingName = value;
                }
                else
                {
                    throw new InvalidCastException();
                }
            }
        }

        [NotMapped]
        public DateTime Time_Created { get; set; }

        [NotMapped]
        public String User_Created_By { get; set; }

        [Column("overridden")]
        public SByte Overridden { get; set; }

        public Override(SensorItemEnum INItem, String createdBy)
        {
            Time_Created = DateTime.Now;
            Item = INItem;
            User_Created_By = createdBy;
        }

        public Override()
        {

        }
    }
}
