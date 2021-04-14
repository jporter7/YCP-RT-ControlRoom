using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager
{
    public class MCUPositonStore
    {
        public int AzSteps;
        public int AzEncoder;

        public int ElSteps;
        public int ElEncoder;

        public MCUPositonStore()
        {

        }

        public MCUPositonStore(MCUPositonRegs mCUpositon)
        {
            this.AzSteps = mCUpositon.AzSteps;
            this.AzEncoder = mCUpositon.AzEncoder;

            this.ElSteps = mCUpositon.ElSteps;
            this.ElEncoder = mCUpositon.ElEncoder;
        }

        public MCUPositonStore(MCUPositonStore current, MCUPositonStore previous)
        {
            this.AzSteps = previous.AzSteps - current.AzSteps;
            this.AzEncoder = previous.AzEncoder - current.AzEncoder;

            this.ElSteps = previous.ElSteps - current.ElSteps;
            this.ElEncoder = previous.ElEncoder - current.ElEncoder;
        }

        public void SUM(MCUPositonStore current, MCUPositonStore previous)
        {
            this.AzSteps += current.AzSteps - previous.AzSteps;
            this.AzEncoder += current.AzEncoder - previous.AzEncoder;

            this.ElSteps += current.ElSteps - previous.ElSteps;
            this.ElEncoder += current.ElEncoder - previous.ElEncoder;
        }

        public void SUMAbsolute(MCUPositonStore current, MCUPositonStore previous)
        {
            this.AzSteps += Math.Abs(current.AzSteps - previous.AzSteps);
            this.AzEncoder += Math.Abs(current.AzEncoder - previous.AzEncoder);

            this.ElSteps += Math.Abs(current.ElSteps - previous.ElSteps);
            this.ElEncoder += Math.Abs(current.ElEncoder - previous.ElEncoder);
        }
    }
}
