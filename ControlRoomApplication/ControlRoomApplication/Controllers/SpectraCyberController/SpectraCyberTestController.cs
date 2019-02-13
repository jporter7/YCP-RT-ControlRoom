﻿using ControlRoomApplication.Entities;
using ControlRoomApplication.Main;

namespace ControlRoomApplication.Controllers.SpectraCyberController
{
    public class SpectraCyberTestController : AbstractSpectraCyberController
    {
        public SpectraCyberTestController(SpectraCyberSimulator spectraCyberSimulator, RTDbContext context) : base(spectraCyberSimulator, context)
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
    }
}
