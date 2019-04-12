using System;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Simulators.Hardware.AbsoluteEncoder;

namespace ControlRoomApplication.Simulators.Hardware.MCU
{
    public class SimulationMCU
    {
        private SimulationAbsoluteEncoder AzEncoder;
        private SimulationAbsoluteEncoder ElEncoder;

        private DateTime ActiveObjectiveOrientationMoveStart;
        private Orientation ActiveObjectiveOrientation;

        private SimulationStopTypeEnum RequestedStopType;

        public SimulationMCU(SimulationAbsoluteEncoder azEncoder, SimulationAbsoluteEncoder elEncoder)
        {
            AzEncoder = azEncoder;
            ElEncoder = elEncoder;

            ActiveObjectiveOrientation = null;
            RequestedStopType = SimulationStopTypeEnum.NONE;
        }

        public SimulationMCU(int azEncoderPrecision, int elEncoderPrecision)
            : this(new SimulationAbsoluteEncoder(azEncoderPrecision), new SimulationAbsoluteEncoder(elEncoderPrecision)) {}

        public Orientation GetActiveObjectiveOrientation()
        {
            return ActiveObjectiveOrientation;
        }

        public bool HasActiveMove()
        {
            return ActiveObjectiveOrientation != null;
        }

        public Orientation GetCurrentOrientationInDegrees()
        {
            return new Orientation(AzEncoder.CurrentPositionDegrees, ElEncoder.CurrentPositionDegrees);
        }

        private void TryStop(SimulationStopTypeEnum stopType)
        {
            RequestedStopType = HasActiveMove() ? stopType : SimulationStopTypeEnum.NONE;
        }

        public void ExecuteControlledStop()
        {
            TryStop(SimulationStopTypeEnum.CONTROLLED);
        }

        public void ExecuteImmediateStop()
        {
            TryStop(SimulationStopTypeEnum.IMMEDIATE);
        }

        public void SetActiveObjectiveOrientationAndStartMove(Orientation orientationDegrees)
        {
            RequestedStopType = SimulationStopTypeEnum.NONE;
            ActiveObjectiveOrientation = orientationDegrees;
            ActiveObjectiveOrientationMoveStart = DateTime.UtcNow;
        }
    }
}