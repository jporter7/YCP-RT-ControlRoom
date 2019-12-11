using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Entities
{
    public class Sensor
    {
        public SensorItemEnum Item { get; set; }
        public DateTime Time_Created { get; set; }
        public SensorStatusEnum Status { get; set; }

        public Sensor(SensorItemEnum INItem, SensorStatusEnum status)
        {
            Time_Created = DateTime.Now;
            Item = INItem;
            Status = status;
        }
    }
}
