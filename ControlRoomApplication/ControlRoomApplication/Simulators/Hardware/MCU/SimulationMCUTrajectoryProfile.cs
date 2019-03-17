using System;
using ControlRoomApplication.Constants;

namespace ControlRoomApplication.Simulators.Hardware.MCU
{
    public class SimulationMCUTrajectoryProfile
    {
        // This describes the type of profile that this trajectory is abiding by
        private SimulationMCUTrajectoryProfileTypeEnum ProfileType;

        // This is the initial velocity of the move
        // This value is in steps/seconds
        private double InitialVelocity;

        // This is the maximum velocity, the speed that the profile will have during its constant velocity sequence
        // This value is in steps/second, where "steps" is in a domain like encoder tick positions
        private double TrajectoryPeakVelocity;

        // This is the maximum acceleration and deceleration that the profile will abide by
        // This value is in steps/second^2, where "steps" is in a domain like encoder tick positions
        private double TrajectoryPeakAcceleration;

        // This is the jerk throughout the move that the profile will abide by
        // This value is in steps/second^3, where "steps" is in a domain like encoder tick positions
        private double TrajectoryJerk;

        // This is the calculated amount of time that the move should take, according to the kinematic characteristics of the profile
        // This value is in seconds
        private double TotalTime;

        // These is the objective absolute step position in this axis
        private int ObjectiveStep;

        // Private constructor, so the factory function has to be used
        private SimulationMCUTrajectoryProfile(
            SimulationMCUTrajectoryProfileTypeEnum type,
            double initialVelocity,
            double peakVelocity,
            double peakAcceleration,
            double jerk,
            double totalTime,
            int objectiveStep)
        {
            ProfileType = type;
            InitialVelocity = initialVelocity;
            TrajectoryPeakVelocity = peakVelocity;
            TrajectoryPeakAcceleration = peakAcceleration;
            TrajectoryJerk = jerk;
            TotalTime = totalTime;
            ObjectiveStep = objectiveStep;
        }

        public int InterpretAt(DateTime startTime, DateTime evaluationTime)
        {
            //double timeElapsed = (evaluationTime - startTime).TotalSeconds;
            //double displacement = 0.0;
            //
            //switch (ProfileType)
            //{
            //    case SimulationMCUTrajectoryProfileTypeEnum.NEGLIGIBLE:
            //    {
            //        return ObjectiveStep;
            //    }
            //
            //    case SimulationMCUTrajectoryProfileTypeEnum.S_CURVE_TRAPEZOIDAL:
            //    {
            //        
            //    }
            //}

            throw new NotImplementedException("Hold your horses!");
        }

        private static int GetTickTranslation(double j, double a, double v, double dt)
        {
            return (int)(((((j * dt / 6) + (a / 2)) * dt) + v) * dt);
        }

        // This factory function utilizes the equations on page 20 and 21 of the 2-axis MCU docuemtation to calculate the type of
        // trajectory profile associated with the possible move type, as well as the characteristics of said profile
        public static SimulationMCUTrajectoryProfile calculateInstance(
            SimulationAbsoluteEncoder axisEncoder,
            double initialVelocity,
            double peakVelocity,
            double peakAcceleration,
            double jerk,
            double axisObjectiveDegrees)
        {
            int axisObjectiveSteps= axisEncoder.GetEquivalentEncoderTicksFromDegrees(axisObjectiveDegrees);
            int changeInAxisSteps = axisObjectiveSteps - axisEncoder.GetCurrentPositionTicks();

            SimulationMCUTrajectoryProfile resultingProfile;

            // See if this move is even worth the effort of TRYING to move
            if (Math.Abs(axisEncoder.GetEquivalentDegreesFromEncoderTicks(changeInAxisSteps)) < HardwareConstants.NEGLIGIBLE_POSITION_CHANGE_DEGREES)
            {
                resultingProfile = new SimulationMCUTrajectoryProfile(SimulationMCUTrajectoryProfileTypeEnum.NEGLIGIBLE, 0.0, 0.0, 0.0, 0.0, 0.0, 0);
            }
            else
            {
                // The move is substantial enough for us to want to move...
                // Try to make the most optimal types of trajectories work
                double timeRequiredForAcceleration = 4.0 / 3 * (peakVelocity - initialVelocity) / peakAcceleration;
                double distanceRequiredForAcceleration = timeRequiredForAcceleration * (peakVelocity + initialVelocity) / 2;
                int actualDistanceRequired = (int)(2 * distanceRequiredForAcceleration);

                // See if it can be a trapezoidal s-curve
                if (actualDistanceRequired < changeInAxisSteps)
                {
                    double remainingDistanceForConstantVelocity = changeInAxisSteps - (2 * distanceRequiredForAcceleration);
                    double remainingTimeForConstantVelocity = peakVelocity / remainingDistanceForConstantVelocity;

                    resultingProfile = new SimulationMCUTrajectoryProfile(
                        SimulationMCUTrajectoryProfileTypeEnum.S_CURVE_TRAPEZOIDAL,
                        axisEncoder.GetCurrentInstantaneousVelocity(),
                        peakVelocity,
                        peakAcceleration,
                        jerk,
                        (2 * timeRequiredForAcceleration) + remainingTimeForConstantVelocity,
                        axisObjectiveSteps
                    );
                }

                // Did not fit the full trapezoidal s-curve requirements, try a triangular s-curve
                // Because floating points are weird, look for a threshold that we can reasonably justify equality for
                else if (Math.Abs(changeInAxisSteps - actualDistanceRequired) < 0.001)
                {
                    resultingProfile = new SimulationMCUTrajectoryProfile(
                        SimulationMCUTrajectoryProfileTypeEnum.S_CURVE_TRIANGULAR,
                        axisEncoder.GetCurrentInstantaneousVelocity(),
                        peakVelocity,
                        peakAcceleration,
                        jerk,
                        2 * timeRequiredForAcceleration,
                        axisObjectiveSteps
                    );
                }

                // Did not fit the full trapezoidal or triangular s-curve requirements, it's a partial trapezoidal s-curve
                else
                {
                    double fixedPeakVelocity = Math.Sqrt((0.75 * peakAcceleration * distanceRequiredForAcceleration) + (initialVelocity * initialVelocity));
                    double fixedTimeRequiredForAcceleration = 4.0 / 3 * (fixedPeakVelocity - initialVelocity) / peakAcceleration;

                    resultingProfile = new SimulationMCUTrajectoryProfile(
                        SimulationMCUTrajectoryProfileTypeEnum.S_CURVE_TRAPEZOIDAL_PARTIAL,
                        axisEncoder.GetCurrentInstantaneousVelocity(),
                        fixedPeakVelocity,
                        peakAcceleration,
                        jerk,
                        2 * fixedTimeRequiredForAcceleration,
                        axisObjectiveSteps
                    );
                }
            }

            return resultingProfile;
        }
    }
}
