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
            return (int)(degrees * MotorConstants.STEPS_PER_REVOLUTION_BEFORE_GEARING * gearingRatio / 360.0);
        }

        public static double StepsToDegrees(int steps, int gearingRatio)
        {
            return steps * 360.0 / (MotorConstants.STEPS_PER_REVOLUTION_BEFORE_GEARING * gearingRatio);
        }

        public static int DegreesToSteps_Encoder( double degrees , int gearingRatio ) {
            return (int)(degrees * MotorConstants.ENCODER_COUNTS_PER_REVOLUTION_BEFORE_GEARING * gearingRatio / 360.0);
        }

        public static double StepsToDegrees_Encoder( int steps , int gearingRatio ) {
            return steps * 360.0 / (MotorConstants.ENCODER_COUNTS_PER_REVOLUTION_BEFORE_GEARING * gearingRatio);
        }

        public static double RPMToDPS(double rpms)
        {
            return rpms * 6;
        }

        public static int DPSToSPS(double dpss, int gearingRatio)
        {
            return DegreesToSteps( dpss , gearingRatio );
            //return RPMToSPS( (dpss /6.0),gearingRatio);
        }

        public static double SPSToDPS(  int sps, int gearingRatio ) {
            return StepsToDegrees( sps , gearingRatio );
        }

        public static int RPMToSPS(double rpms, int gearingRatio)
        {
            return (int)((rpms * (double)(MotorConstants.STEPS_PER_REVOLUTION_BEFORE_GEARING * gearingRatio))/60.0);
        }

        public static double SPSToRPM( int sps , int gearingRatio ) {
            return (sps*60)/ (double)(MotorConstants.STEPS_PER_REVOLUTION_BEFORE_GEARING * gearingRatio);
        }
    }
}
