using ControlRoomApplication.Entities;
using Modbus.Data;
using Modbus.Device;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ControlRoomApplication.Simulators.Hardware;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Controllers.Sensors;

namespace ControlRoomApplication.Controllers
{
    public abstract class AbstractPLCDriver : HeartbeatInterface {
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected Thread ClientManagmentThread;

        protected RadioTelescope Parent;

        public LimitSwitchData limitSwitchData;
        public ProximitySensorData proximitySensorData;
        public OverrideSwitchData overrides;

        /// <summary>
        /// the PLC will look for the server that we create in the control room, the control room will look for the remote server that the MCU has setup
        /// </summary>
        /// <param name="local_ip">IP adress to start the local modbus server on</param>
        /// <param name="MCU_ip">IP adress of the MCU</param>
        /// <param name="MCU_port">port the MCU is using</param>
        /// <param name="PLC_port">port to start the local modbus server on</param>
        /// <param name="autoStartPLCThread"> if true will automaticly start the modbus server</param>
        public AbstractPLCDriver(string local_ip, string MCU_ip, int MCU_port, int PLC_port) { }


        protected override bool KillHeartbeatComponent() {
             Bring_down();
            return true;
        }

        public bool publicKillHeartbeatComponent() {
            return KillHeartbeatComponent();
        }

        /// <summary>
        /// modbuss server implamentation specific to each device
        /// </summary>
        public abstract void HandleClientManagementThread();


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract bool StartAsyncAcceptingClients();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract bool RequestStopAsyncAcceptingClientsAndJoin();

        public RadioTelescope GetParent()
        {
            return Parent;
        }

        public void SetParent(RadioTelescope rt)
        {
            Parent = rt;
        }

        public abstract void Bring_down();

        public abstract bool Test_Conection();

        public abstract Orientation read_Position();

        public abstract bool Cancel_move();

        public abstract bool Shutdown_PLC_MCU();

        // All of the "scripts" are here all the way to....
        // Control Scripts

        public abstract bool Thermal_Calibrate();

        public abstract bool FullElevationMove();

        public abstract bool Full_360_CCW_Rotation();

        public abstract bool Full_360_CW_Rotation();

        public abstract bool Stow();

        public abstract bool SnowDump();

        // Diagnostics Scripts

        public abstract bool HitAzimuthLeftLimitSwitch();

        public abstract bool HitAzimuthRightLimitSwitch();

        public abstract bool HitElevationLowerLimitSwitch();

        public abstract bool HitElevationUpperLimitSwitch();

        public abstract bool RecoverFromLimitSwitch();

        public abstract bool Hit_CW_Hardstop();

        public abstract bool Hit_CCW_Hardstop();
        public abstract bool Recover_CW_Hardstop();

        public abstract bool Recover_CCW_Hardstop();

        public abstract bool Hit_Hardstops();

        // ... to here

        public abstract bool Configure_MCU(double startSpeedAzimuth, double startSpeedElevation, int homeTimeoutAzimuth, int homeTimeoutElevation);

        public abstract bool Controled_stop(RadioTelescopeAxisEnum axis, bool both);

        public abstract bool Immediade_stop();

        public abstract bool relative_move(int programmedPeakSpeedAZInt, ushort ACCELERATION, int positionTranslationAZ, int positionTranslationEL);

        public abstract bool Move_to_orientation(Orientation target_orientation, Orientation current_orientation);

        public abstract bool Start_jog(RadioTelescopeAxisEnum axis, int speed, bool clockwise);

        public abstract bool Get_interlock_status();

        public abstract bool[] Get_Limit_switches();

        public abstract void setregvalue(ushort adr, ushort value);


        /// <summary>
        /// send home command to the tellescope, will move the telescope to 0 , 0  degrees 
        /// after calling this method we should zero out the apsolute encoders
        /// </summary>
        /// <returns>sucsess bool</returns>
        public abstract Task<bool> Home();
        /// <summary>
        /// get an array of boolens representiing the register described on pages 76 -79 of the mcu documentation 
        /// does not suport RadioTelescopeAxisEnum.BOTH
        /// see <see cref="MCUConstants.MCUStutusBits"/> for description of each bit
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public abstract Task<bool[]> GET_MCU_Status( RadioTelescopeAxisEnum axis );

        /// <summary>
        ///  Checks that the motor temperatures are within acceptable ranges. If the temperature exceeds
        ///  the corresponding value in SimulationConstants.cs, it will return false, otherwise
        ///  it will return true if everything is good.
        /// </summary>
        /// <returns></returns>
        public bool tempMonitor()
        {
            // Check azimuth motor
            if(Parent.Micro_controler.tempData.azimuthTemp < SimulationConstants.STABLE_MOTOR_TEMP)
            {
                logger.Info("Azimuth motor temperature BELOW stable temperature by " + (SimulationConstants.STABLE_MOTOR_TEMP - Parent.Micro_controler.tempData.azimuthTemp) + " degrees Fahrenheit.");
                
                // Only overrides if switch is true
                if (!overrides.overrideAzimuthMotTemp) return false;
            }
            else if(Parent.Micro_controler.tempData.azimuthTemp > SimulationConstants.OVERHEAT_MOTOR_TEMP)
            {
                logger.Info("Azimuth motor temperature OVERHEATING by " + (Parent.Micro_controler.tempData.azimuthTemp - SimulationConstants.OVERHEAT_MOTOR_TEMP) + " degrees Fahrenheit.");

                // Only overrides if switch is true
                if (!overrides.overrideAzimuthMotTemp) return false;
            }

            // Check elevation motor
            if (Parent.Micro_controler.tempData.elevationTemp < SimulationConstants.STABLE_MOTOR_TEMP)
            {
                logger.Info("Elevation motor temperature BELOW stable temperature by " + (SimulationConstants.STABLE_MOTOR_TEMP - Parent.Micro_controler.tempData.elevationTemp) + " degrees Fahrenheit.");

                // Only overrides if switch is true
                if (!overrides.overrideElevatMotTemp) return false;
            }
            else if (Parent.Micro_controler.tempData.elevationTemp > SimulationConstants.OVERHEAT_MOTOR_TEMP)
            {
                logger.Info("Elevation motor temperature OVERHEATING by " + (Parent.Micro_controler.tempData.elevationTemp - SimulationConstants.OVERHEAT_MOTOR_TEMP) + " degrees Fahrenheit.");

                // Only overrides if switch is true
                if (!overrides.overrideElevatMotTemp) return false;
            }

            logger.Info("All motor temperatures are stable.");
            return true;
        }
    }
}