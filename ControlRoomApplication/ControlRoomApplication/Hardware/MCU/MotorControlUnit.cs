using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Hardware.MCU
{
    public class MotorControlUnit
    {
        public MotorControlUnit()
        {

        }

        public MotorControlUnit(AzimuthEncoder azEncoder, ElevationEncoder elEncoder)
        {
            AzEncoder = azEncoder;
            ElEncoder = elEncoder;
        }

        public AzimuthEncoder AzEncoder { get; set; }
        public ElevationEncoder ElEncoder { get; set; }
    }
}