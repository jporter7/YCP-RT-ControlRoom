using ControlRoomApplication.Entities;

namespace ControlRoomApplication.Controllers
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

        public override bool BringUp()
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
