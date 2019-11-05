using ControlRoomApplication.Controllers.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlRoomApplication.Constants;

namespace ControlRoomApplication.Simulators.Hardware
{
    class FakeLimitSwitches
    {
        bool hitOrNot;

        public FakeLimitSwitches()
        {
            hitOrNot = false;
        }

        public bool GetLimitValue()
        {
            return hitOrNot;
        }

        public void HitsLimitSwitch()
        {
            hitOrNot = true;
        }

        public void notHittingSwitch()
        {
            hitOrNot = false;
        }
    }
}
