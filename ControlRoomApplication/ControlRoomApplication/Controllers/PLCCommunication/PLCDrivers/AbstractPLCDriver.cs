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
using ControlRoomApplication.Controllers.PLCCommunication;

namespace ControlRoomApplication.Controllers
{
    public abstract class AbstractPLCDriver : HeartbeatInterface {
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected Thread ClientManagmentThread;

        protected RadioTelescope Parent;

        public LimitSwitchData limitSwitchData;
        public HomeSensorData homeSensorData;
        public MiscPlcInput plcInput;
        protected PLCEvents pLCEvents;


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

        public abstract bool Test_Connection();

        public abstract Orientation read_Position();

        public abstract bool Cancel_move();

        public abstract bool Shutdown_PLC_MCU();

        // All of the "scripts" are here all the way to....
        // Control Scripts

        public abstract Task<bool> Thermal_Calibrate();

        public abstract Task<bool> FullElevationMove();

        public abstract Task<bool> Full_360_CCW_Rotation();

        public abstract Task<bool> Full_360_CW_Rotation();

        public abstract Task<bool> Stow();

        public abstract Task<bool> SnowDump();

        // Diagnostics Scripts

        public abstract Task<bool> HitAzimuthLeftLimitSwitch();

        public abstract Task<bool> HitAzimuthRightLimitSwitch();

        public abstract Task<bool> HitElevationLowerLimitSwitch();

        public abstract Task<bool> HitElevationUpperLimitSwitch();

        public abstract Task<bool> RecoverFromLimitSwitch();

        public abstract Task<bool> Hit_CW_Hardstop();

        public abstract Task<bool> Hit_CCW_Hardstop();
        public abstract Task<bool> Recover_CW_Hardstop();

        public abstract Task<bool> Recover_CCW_Hardstop();

        public abstract Task<bool> Hit_Hardstops();

        // ... to here

        public abstract bool Configure_MCU(double startSpeedAzimuth, double startSpeedElevation, int homeTimeoutAzimuth, int homeTimeoutElevation);

        public abstract bool Controled_stop();

        public abstract bool Immediade_stop();

        public abstract bool relative_move(int programmedPeakSpeedAZInt, ushort ACCELERATION, int positionTranslationAZ, int positionTranslationEL);

        public abstract Task<bool> Move_to_orientation(Orientation target_orientation, Orientation current_orientation);

        public abstract bool Start_jog( double AZspeed ,bool AZ_CW, double ELspeed ,bool EL_CW);

        public abstract bool Stop_Jog();

        public abstract bool Get_interlock_status();

        public abstract Task<bool> JogOffLimitSwitches();

        public abstract void setregvalue(ushort adr, ushort value);

        public abstract ushort getregvalue(ushort adr);


        /// <summary>
        /// send home command to the tellescope, will move the telescope to 0 , 0  degrees 
        /// after calling this method we should zero out the apsolute encoders
        /// </summary>
        /// <returns>sucsess bool</returns>
        public abstract Task<bool> Home();
        /// <summary>
        /// get an array of boolens representiing the register described on pages 76 -79 of the mcu documentation 
        /// does not suport RadioTelescopeAxisEnum.BOTH
        /// see <see cref="MCUConstants.MCUStutusBitsMSW"/> for description of each bit
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public abstract Task<bool[]> GET_MCU_Status( RadioTelescopeAxisEnum axis );

    }
}