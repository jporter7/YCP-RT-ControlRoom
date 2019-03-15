using ControlRoomApplication.Entities;
using ControlRoomApplication.Main;

namespace ControlRoomApplication.Controllers.SpectraCyberController
{
    public class SpectraCyberTestController : AbstractSpectraCyberController
    {
        public SpectraCyberTestController(SpectraCyberSimulator spectraCyberSimulator) : base(spectraCyberSimulator)
        {

        }

        public override bool BringDown()
        {
            return true;
        }

        public override bool BringUp(int appId)
        {
            return true;
        }

        protected override void SendCommand(SpectraCyberRequest request, ref SpectraCyberResponse response)
        {
            // pass
        }

        protected override bool TestIfComponentIsAlive()
        {
            //throw new System.NotImplementedException();
            return true;
        }
    }
}
