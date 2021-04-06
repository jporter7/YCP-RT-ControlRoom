using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager
{
    public class MCUPositonStore
    {
        public int AZ_Steps, EL_Steps;
        public int AZ_Encoder, EL_Encoder;
        public MCUPositonStore()
        {

        }

        public MCUPositonStore(MCUPositonRegs mCUpositon)
        {
            this.AZ_Encoder = mCUpositon.AZ_Encoder;
            this.AZ_Steps = mCUpositon.AZ_Steps;
            this.EL_Encoder = mCUpositon.EL_Encoder;
            this.EL_Steps = mCUpositon.EL_Steps;
        }
        public MCUPositonStore(MCUPositonStore current, MCUPositonStore previous)
        {
            this.AZ_Steps = previous.AZ_Steps - current.AZ_Steps;
            this.EL_Steps = previous.EL_Steps - current.EL_Steps;
            this.AZ_Encoder = previous.AZ_Encoder - current.AZ_Encoder;
            this.EL_Encoder = previous.EL_Encoder - current.EL_Encoder;
        }

        public void SUM(MCUPositonStore current, MCUPositonStore previous)
        {
            this.AZ_Steps += current.AZ_Steps - previous.AZ_Steps;
            this.EL_Steps += current.EL_Steps - previous.EL_Steps;
            this.AZ_Encoder += current.AZ_Encoder - previous.AZ_Encoder;
            this.EL_Encoder += current.EL_Encoder - previous.EL_Encoder;
        }

        public void SUMAbsolute(MCUPositonStore current, MCUPositonStore previous)
        {
            this.AZ_Steps += Math.Abs(current.AZ_Steps - previous.AZ_Steps);
            this.EL_Steps += Math.Abs(current.EL_Steps - previous.EL_Steps);
            this.AZ_Encoder += Math.Abs(current.AZ_Encoder - previous.AZ_Encoder);
            this.EL_Encoder += Math.Abs(current.EL_Encoder - previous.EL_Encoder);
        }
    }
}
