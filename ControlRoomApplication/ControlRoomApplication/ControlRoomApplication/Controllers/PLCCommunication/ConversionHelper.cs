using ControlRoomApplication.Constants;

namespace ControlRoomApplication.Controllers
{
    public static class ConversionHelper
    {
        //
        // Definitions:
        //  - RPM = revolutions per minute
        //  - DPS = degrees per second
        //  - SPS = steps per second
        //

        public static double RevolutionsToDegrees(double revolutions)
        {
            return revolutions * 360;
        }

        public static int DegreesToSteps(double degrees, int gearingRatio)
        {
            return (int)(degrees * MotorConstants.STEPS_PER_REVOLUTION_BEFORE_GEARING * gearingRatio / 360);
        }

        public static double StepsToDegrees(int steps, int gearingRatio)
        {
            return steps * 360.0 / (MotorConstants.STEPS_PER_REVOLUTION_BEFORE_GEARING * gearingRatio);
        }

        public static double RPMToDPS(double rpms)
        {
            return rpms * 6;
        }

        public static int DPSToSPS(double dpss, int gearingRatio)
        {
            return (int)(dpss * MotorConstants.STEPS_PER_REVOLUTION_BEFORE_GEARING * gearingRatio / 360);
        }

        public static int RPMToSPS(double rpms, int gearingRatio)
        {
            return DPSToSPS(RPMToDPS(rpms), gearingRatio);
        }
    }
}
