using System;
using ControlRoomApplication.Entities;

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
            return new Orientation(AzEncoder.GetCurrentPositionInDegrees(), ElEncoder.GetCurrentPositionInDegrees());
        }

        public void ConsumeRequestedStop()
        {
            RequestedStopType = SimulationStopTypeEnum.NONE;
        }

        public void ExecuteControlledStop()
        {
            if (HasActiveMove())
            {
                RequestedStopType = SimulationStopTypeEnum.CONTROLLED;
            }
            else
            {
                ConsumeRequestedStop();
            }
        }

        public void ExecuteImmediateStop()
        {
            if (HasActiveMove())
            {
                RequestedStopType = SimulationStopTypeEnum.IMMEDIATE;
            }
            else
            {
                ConsumeRequestedStop();
            }
        }

        public void SetActiveObjectiveOrientationAndStartMove(Orientation orientationDegrees)
        {
            ConsumeRequestedStop();
            ActiveObjectiveOrientation = orientationDegrees;
        }

        public void HandleMoveRoutine()
        {
            throw new NotImplementedException("Hold your horses!");
        }
    }
}