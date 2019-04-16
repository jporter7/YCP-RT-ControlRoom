using System;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Simulators.Hardware.AbsoluteEncoder;

namespace ControlRoomApplication.Simulators.Hardware.MCU
{
    public class SimulationMCUTrajectoryProfile
    {
        // This describes the type of profile that this trajectory is abiding by
        public SimulationMCUTrajectoryProfileTypeEnum ProfileType { get; }

        // This is the initial velocity of the move
        // This value is in steps/seconds
        public double InitialVelocity { get; }

        // This is the maximum velocity, the speed that the profile will have during its constant velocity sequence
        // This value is in steps/second, where "steps" is in a domain like encoder tick positions
        public double TrajectoryPeakVelocity { get; }

        // This is the maximum acceleration and deceleration that the profile will abide by
        // This value is in steps/second^2, where "steps" is in a domain like encoder tick positions
        public double TrajectoryPeakAcceleration { get; }

        // This is the jerk throughout the move that the profile will abide by
        // This value is in steps/second^3, where "steps" is in a domain like encoder tick positions
        public double TrajectoryJerk { get; }

        // This is the calculated amount of time that the move should take, according to the kinematic characteristics of the profile
        // This value is in seconds
        public double TotalTime { get; }

        // This is the initial absolute step position in this axis
        public int InitialStep { get; }

        // This is the objective absolute step position in this axis
        public int ObjectiveStep { get; }

        // Private constructor, so the factory function has to be used
        private SimulationMCUTrajectoryProfile(
            SimulationMCUTrajectoryProfileTypeEnum type,
            double initialVelocity,
            double peakVelocity,
            double peakAcceleration,
            double accelerationTime,
            double totalTime,
            int initialStep,
            int objectiveStep)
        {
            ProfileType = type;
            InitialVelocity = initialVelocity;
            TrajectoryPeakVelocity = peakVelocity;
            TrajectoryPeakAcceleration = peakAcceleration;
            TrajectoryJerk = TrajectoryPeakAcceleration / (accelerationTime / 2);
            TotalTime = totalTime;
            InitialStep = initialStep;
            ObjectiveStep = objectiveStep;
        }

        public int InterpretAt(DateTime startTime, DateTime evaluationTime)
        {
            double timeElapsed = (evaluationTime - startTime).TotalSeconds;

            if (timeElapsed < 0)
            {
                throw new ArgumentException("Evaluation time was before the start time.");
            }

            switch (ProfileType)
            {
                case SimulationMCUTrajectoryProfileTypeEnum.NEGLIGIBLE:
                    {
                        return ObjectiveStep;
                    }

                case SimulationMCUTrajectoryProfileTypeEnum.S_CURVE_TRAPEZOIDAL:
                    {
                        int cumulativePosition = InitialStep;

                        double t1 = TrajectoryPeakAcceleration / TrajectoryJerk;
                        if (timeElapsed < t1)
                        {
                            return cumulativePosition + GetTickTranslation(TrajectoryJerk, 0, 0, timeElapsed);
                        }

                        cumulativePosition += GetTickTranslation(TrajectoryJerk, 0, 0, t1);
                        int cumulativeVelocity = GetTickVelocityChange(TrajectoryJerk, 0, t1);
                        double t2 = ((TrajectoryPeakVelocity - (2 * cumulativeVelocity)) / TrajectoryPeakAcceleration) + t1;
                        if (timeElapsed < t2)
                        {
                            return cumulativePosition + GetTickTranslation(0, TrajectoryPeakAcceleration, cumulativeVelocity, timeElapsed - t1);
                        }

                        cumulativePosition += GetTickTranslation(0, TrajectoryPeakAcceleration, cumulativeVelocity, t2 - t1);
                        cumulativeVelocity += GetTickVelocityChange(0, TrajectoryPeakAcceleration, t2 - t1);
                        double t3 = t2 + t1;
                        if (timeElapsed < t3)
                        {
                            return cumulativePosition + GetTickTranslation(-TrajectoryJerk, TrajectoryPeakAcceleration, cumulativeVelocity, timeElapsed - t1);
                        }

                        cumulativePosition += GetTickTranslation(-TrajectoryJerk, TrajectoryPeakAcceleration, cumulativeVelocity, timeElapsed - t2);
                        cumulativeVelocity += GetTickVelocityChange(-TrajectoryJerk, TrajectoryPeakAcceleration, timeElapsed - t2);
                        double t4 = TotalTime - (2 * t3);
                        if (timeElapsed < t4)
                        {
                            return cumulativePosition + GetTickTranslation(0, 0, cumulativeVelocity, timeElapsed - t3);
                        }

                        cumulativePosition += GetTickTranslation(0, 0, cumulativeVelocity, t4 - t3);
                        cumulativeVelocity += GetTickVelocityChange(0, 0, t4 - t3);
                        double t5 = t4 + t1;
                        if (timeElapsed < t5)
                        {
                            return cumulativePosition + GetTickTranslation(-TrajectoryJerk, 0, cumulativeVelocity, timeElapsed - t4);
                        }

                        cumulativePosition += GetTickTranslation(-TrajectoryJerk, 0, cumulativeVelocity, t5 - t4);
                        cumulativeVelocity += GetTickVelocityChange(-TrajectoryJerk, 0, t5 - t4);
                        double t6 = t4 + t2;
                        if (timeElapsed < t6)
                        {
                            return cumulativePosition + GetTickTranslation(0, -TrajectoryPeakAcceleration, cumulativeVelocity, timeElapsed - t5);
                        }

                        cumulativePosition += GetTickTranslation(0, -TrajectoryPeakAcceleration, cumulativeVelocity, t6 - t5);
                        cumulativeVelocity += GetTickVelocityChange(0, -TrajectoryPeakAcceleration, t6 - t5);
                        if (timeElapsed < TotalTime)
                        {
                            return cumulativePosition + GetTickTranslation(TrajectoryJerk, -TrajectoryPeakAcceleration, cumulativeVelocity, timeElapsed - t6);
                        }

                        return ObjectiveStep;
                    }

                case SimulationMCUTrajectoryProfileTypeEnum.S_CURVE_TRIANGULAR_FULL:
                case SimulationMCUTrajectoryProfileTypeEnum.S_CURVE_TRIANGULAR_PARTIAL:
                    {
                        double t1 = TotalTime / 4;
                        if (timeElapsed < t1)
                        {
                            return GetTickTranslation(TrajectoryJerk, 0, 0, timeElapsed);
                        }

                        int cumulativeDisplacement = GetTickTranslation(TrajectoryJerk, 0, 0, t1);
                        int cumulativeVelocity = GetTickVelocityChange(TrajectoryJerk, 0, t1);
                        double t2 = t1;

                        double t3 = 2 * t1;
                        if (timeElapsed < t3)
                        {
                            return cumulativeDisplacement + GetTickTranslation(-TrajectoryJerk, TrajectoryPeakAcceleration, cumulativeVelocity, timeElapsed - t1);
                        }

                        cumulativeDisplacement += GetTickTranslation(-TrajectoryJerk, TrajectoryPeakAcceleration, cumulativeVelocity, timeElapsed - t2);
                        cumulativeVelocity += GetTickVelocityChange(-TrajectoryJerk, TrajectoryPeakAcceleration, timeElapsed - t2);
                        double t4 = t3;

                        double t5 = 3 * t1;
                        if (timeElapsed < t5)
                        {
                            return cumulativeDisplacement + GetTickTranslation(-TrajectoryJerk, 0, cumulativeVelocity, timeElapsed - t4);
                        }

                        cumulativeDisplacement += GetTickTranslation(-TrajectoryJerk, 0, cumulativeVelocity, t5 - t4);
                        cumulativeVelocity += GetTickVelocityChange(-TrajectoryJerk, 0, t5 - t4);
                        double t6 = t5;

                        if (timeElapsed < TotalTime)
                        {
                            return cumulativeDisplacement + GetTickTranslation(TrajectoryJerk, -TrajectoryPeakAcceleration, cumulativeVelocity, timeElapsed - t6);
                        }

                        return ObjectiveStep;
                    }

                default:
                    {
                        throw new InvalidOperationException("This profile is of an unrecognized type: " + ProfileType.ToString());
                    }
            }
        }

        private static int GetTickTranslation(double j, double a, double v, double dt)
        {
            return (int)(((((j * dt / 6) + (a / 2)) * dt) + v) * dt);
        }

        private static int GetTickVelocityChange(double j, double a, double dt)
        {
            return (int)(((j * dt / 2) + a) * dt);
        }

        // This factory function utilizes the equations on page 20 and 21 of the 2-axis MCU docuemtation to calculate the type of
        // trajectory profile associated with the possible move type, as well as the characteristics of said profile
        // This assumes 0 initial velocity, for now.
        public static SimulationMCUTrajectoryProfile CalculateInstance(
            SimulationAbsoluteEncoder axisEncoder,
            double axisInitialDegrees,
            double initialVelocity,
            double peakVelocity,
            double peakAcceleration,
            double axisObjectiveDegrees)
        {
            int axisInitialSteps = axisEncoder.GetEquivalentEncoderTicksFromDegrees(axisInitialDegrees);
            int axisObjectiveSteps = axisEncoder.GetEquivalentEncoderTicksFromDegrees(axisObjectiveDegrees);
            int changeInAxisSteps = axisObjectiveSteps - axisInitialSteps;

            SimulationMCUTrajectoryProfile resultingProfile;

            // See if this move is even worth the effort of TRYING to move
            if (Math.Abs(axisEncoder.GetEquivalentDegreesFromEncoderTicks(changeInAxisSteps)) < MiscellaneousConstants.NEGLIGIBLE_POSITION_CHANGE_DEGREES)
            {
                resultingProfile = new SimulationMCUTrajectoryProfile(SimulationMCUTrajectoryProfileTypeEnum.NEGLIGIBLE, initialVelocity, peakVelocity, peakAcceleration, 0.0, 0.0, axisInitialSteps, axisObjectiveSteps);
            }
            else
            {
                // The move is substantial enough for us to want to move...
                // Try to make the most optimal types of trajectories work
                double timeRequiredForAcceleration = 4 * (peakVelocity - initialVelocity) / (3 * peakAcceleration);
                double distanceRequiredForAcceleration = timeRequiredForAcceleration * (peakVelocity + initialVelocity) / 2;
                int actualDistanceRequired = (int)(2 * distanceRequiredForAcceleration);

                // See if it can be a trapezoidal s-curve
                if (actualDistanceRequired < changeInAxisSteps)
                {
                    double remainingDistanceForConstantVelocity = changeInAxisSteps - (2 * distanceRequiredForAcceleration);
                    double remainingTimeForConstantVelocity = peakVelocity / remainingDistanceForConstantVelocity;

                    resultingProfile = new SimulationMCUTrajectoryProfile(
                        SimulationMCUTrajectoryProfileTypeEnum.S_CURVE_TRAPEZOIDAL,
                        0.0,
                        peakVelocity,
                        peakAcceleration,
                        timeRequiredForAcceleration,
                        (2 * timeRequiredForAcceleration) + remainingTimeForConstantVelocity,
                        axisInitialSteps,
                        axisObjectiveSteps
                    );
                }

                // Did not fit the full trapezoidal s-curve requirements, try a triangular s-curve
                else if (changeInAxisSteps == actualDistanceRequired)
                {
                    resultingProfile = new SimulationMCUTrajectoryProfile(
                        SimulationMCUTrajectoryProfileTypeEnum.S_CURVE_TRIANGULAR_FULL,
                        0.0,
                        peakVelocity,
                        peakAcceleration,
                        timeRequiredForAcceleration,
                        2 * timeRequiredForAcceleration,
                        axisInitialSteps,
                        axisObjectiveSteps
                    );
                }

                // Did not fit the full trapezoidal or triangular s-curve requirements, it's a partial trapezoidal s-curve
                else
                {
                    double fixedPeakVelocity = Math.Sqrt((0.75 * distanceRequiredForAcceleration * peakAcceleration) + (initialVelocity * initialVelocity));
                    double fixedTimeRequiredForAcceleration = 4 * (fixedPeakVelocity - initialVelocity) / (3 * peakAcceleration);

                    resultingProfile = new SimulationMCUTrajectoryProfile(
                        SimulationMCUTrajectoryProfileTypeEnum.S_CURVE_TRIANGULAR_PARTIAL,
                        0.0,
                        fixedPeakVelocity,
                        peakAcceleration,
                        fixedTimeRequiredForAcceleration,
                        2 * fixedTimeRequiredForAcceleration,
                        axisInitialSteps,
                        axisObjectiveSteps
                    );
                }
            }

            return resultingProfile;
        }
    }
}
