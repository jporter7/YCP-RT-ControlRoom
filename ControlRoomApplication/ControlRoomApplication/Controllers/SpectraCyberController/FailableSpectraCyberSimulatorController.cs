using ControlRoomApplication.Main;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Constants;

namespace ControlRoomApplication.Controllers.SpectraCyberController
{
    public class FailableSpectraCyberSimulatorController : SpectraCyberSimulatorController
    {
        private double ProbabilityOfFailure;

        public FailableSpectraCyberSimulatorController(SpectraCyberSimulator spectraCyberSimulator, RTDbContext context, int appId, double probability)
            : base(spectraCyberSimulator, context, appId)
        {
            ProbabilityOfFailure = probability;
        }

        public FailableSpectraCyberSimulatorController(SpectraCyberSimulator spectraCyberSimulator, RTDbContext context, int appId)
            : this(spectraCyberSimulator, context, appId, SpectraCyberConstants.FAILABLE_SPECTRA_CYBER_FAILURE_DEFAULT_PROBABILITY) { }

        protected override bool TestIfComponentIsAlive()
        {
            return random.NextDouble() >= ProbabilityOfFailure;
        }
    }
}
