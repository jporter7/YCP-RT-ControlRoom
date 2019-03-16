using System;

namespace ControlRoomApplication.Entities.ProximitySensor
{
    class AbstractProximitySensor
    {
        public AbstractProximitySensor()
        {

        }

        public int SensorId { get; set; }

        public bool IsActive { get; set; }

        public DateTime TimeActivated { get; set; } 

        public DateTime TimeDeactivated { get; set; } 
    }
}
