using ControlRoomApplication.Controllers.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlRoomApplication.Constants;

namespace ControlRoomApplication.Simulators.Hardware
{
    class FakeProximitySensors
    {
        bool closeToLimit;

        public FakeProximitySensors()
        {
            closeToLimit = false;
        }

        public bool getCloseToLimit()
        {
            return closeToLimit;
        }

        public void gettingCloseToLimit()
        {
            closeToLimit = true;
        }

        public void notCloseToLimit()
        {
            closeToLimit = false;
        }

    }
}
