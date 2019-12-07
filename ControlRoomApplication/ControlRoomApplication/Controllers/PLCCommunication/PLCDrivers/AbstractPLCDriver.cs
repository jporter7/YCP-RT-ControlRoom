﻿using ControlRoomApplication.Entities;
using Modbus.Data;
using Modbus.Device;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ControlRoomApplication.Simulators.Hardware;

namespace ControlRoomApplication.Controllers
{
    public abstract class AbstractPLCDriver : HeartbeatInterface {
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected Thread ClientManagmentThread;

        protected RadioTelescope Parent;

        public Simulators.Hardware.LimitSwitchData limitSwitchData;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="local_ip"></param>
        /// <param name="MCU_ip"></param>
        /// <param name="MCU_port"></param>
        /// <param name="PLC_port"></param>
        public AbstractPLCDriver(string local_ip, string MCU_ip, int MCU_port, int PLC_port, bool autoStartPLCThread) { }


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
        /// <summary>
        ///  get an array of boolens representiing the register described on pages 76 -79 of the mcu documentation, 
        /// also see MCUConstants.MCUStutusBits
        /// does not suport RadioTelescopeAxisEnum.BOTH
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public abstract Task<bool[]> GET_MCU_Status( RadioTelescopeAxisEnum axis );



        /// <summary>
        /// processes requests from the clientmanagementthread
        /// !not used in the production PLC driver
        ///is used in simulation although it may not be later
        /// </summary>
        /// <param name="ActiveClientStream"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        //protected abstract bool ProcessRequest(NetworkStream ActiveClientStream, byte[] query);



    }
}