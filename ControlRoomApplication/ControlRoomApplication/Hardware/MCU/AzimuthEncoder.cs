using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Hardware.MCU
{
    public class AzimuthEncoder
    {
        public AzimuthEncoder()
        {

        }

        public AzimuthEncoder(double degree)
        {
            Degree = degree;
        }

        public double Degree { get; set; }
    }
}