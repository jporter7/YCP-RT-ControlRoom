using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Entities
{
    public class Override
    {
        public SensorItemEnum Item { get; set; }
        public DateTime Time_Created { get; set; }
        public String User_Created_By { get; set; }

        public Override(SensorItemEnum INItem, String createdBy)
        {
            Time_Created = DateTime.Now;
            Item = INItem;
            User_Created_By = createdBy;
        }
    }
}
