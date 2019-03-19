using System;

namespace ControlRoomApplication.Simulators.Hardware.AbsoluteEncoder
{
    public class SimulationAbsoluteEncoder
    {
        // This defines the number of bits that can be used to represent the position ticks
        // If this value was 10, the number of possible positions would be 2^10 = 1024
        public int BitsOfPrecision { get; }

        // This describes some simulated error between where the encoder should be and where it "actually" is
        // This value is assumed to be the magnitude of a standard deviation in a Gaussian distribution with mean of 0 (no error)
        // In other words, this means that about 63.0% of all samples of the error will be in the range [-ErrorStandardDeviation : +ErrorStandardDeviation]
        // ------------------------------------- 95.0% of all samples of the error will be in the range [-2 * ErrorStandardDeviation : +2 * ErrorStandardDeviation]
        // ------------------------------------- 99.7% of all samples of the error will be in the range [-3 * ErrorStandardDeviation : +3 * ErrorStandardDeviation]
        // For more information, for now, either Google a Gaussian distribution or ask Nick Vandemark
        public int ErrorStandardDeviation { get; }

        // This is the position that the simulation is saying that this encoder is reading, in encoder ticks
        public int CurrentPositionTicks { get; private set; }

        // This is the position that the simulation is saying that this encoder is reading, in degrees
        public double CurrentPositionDegrees
        {
            get
            {
                return GetEquivalentDegreesFromEncoderTicks(CurrentPositionTicks);
            }

            private set
            {
                CurrentPositionTicks = GetEquivalentEncoderTicksFromDegrees(value);
            }
        }

        // These are the number of values that this encoder could see, so the range of values are [0 : NumberOfEncoderTickPositions-1]
        public int NumberOfEncoderTickPositions { get; }

        // This input position is in degrees
        public SimulationAbsoluteEncoder(int bits, int error, double position)
        {
            BitsOfPrecision = bits;
            NumberOfEncoderTickPositions = (int)Math.Pow(2, BitsOfPrecision);
            ErrorStandardDeviation = error;
            CurrentPositionDegrees = position;
        }

        public SimulationAbsoluteEncoder(int bits, double position) : this(bits, 0, position) { }

        public SimulationAbsoluteEncoder(int bits, int error) : this(bits, error, 0.0) { }

        public SimulationAbsoluteEncoder(int bits) : this(bits, 0, 0.0) { }

        public int GetEquivalentEncoderTicksFromDegrees(double positionDegrees)
        {
            while (positionDegrees < 0)
            {
                positionDegrees += 360;
            }

            return (int)(((positionDegrees % 360) / 360.0) * NumberOfEncoderTickPositions);
        }

        public double GetEquivalentDegreesFromEncoderTicks(int positionTicks)
        {
            return (positionTicks / Math.Pow(2, BitsOfPrecision)) * 360;
        }

        private void SetPositionFromEncoderTicks(int newPosition, bool allowTurnover)
        {
            if ((newPosition < 0) || (newPosition >= NumberOfEncoderTickPositions))
            {
                if (allowTurnover)
                {
                    newPosition %= NumberOfEncoderTickPositions;
                    if (newPosition < 0)
                    {
                        newPosition += NumberOfEncoderTickPositions;
                    }
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Given " + BitsOfPrecision.ToString() + " bits of precision, the value " + newPosition.ToString() + " is impossible.");
                }
            }

            CurrentPositionTicks = newPosition;
        }

        public void SetPositionFromDegrees(double newPosition)
        {
            CurrentPositionDegrees = newPosition;
        }

        public void SetPositionFromEncoderTicks(int newPosition)
        {
            SetPositionFromEncoderTicks(newPosition, false);
        }

        public void TranslateEncoderTicks(int changeInTicks)
        {
            SetPositionFromEncoderTicks(CurrentPositionTicks + changeInTicks, true);
        }

        public void TranslateDegrees(double changeInDegrees)
        {
            TranslateEncoderTicks(GetEquivalentEncoderTicksFromDegrees(changeInDegrees));
        }
    }
}