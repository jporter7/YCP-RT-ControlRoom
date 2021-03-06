﻿using ControlRoomApplication.Entities;
using Modbus.Data;
using Modbus.Device;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ControlRoomApplication.Controllers
{
    public abstract class AbstractPLCDriver : HeartbeatInterface {
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected Thread ClientManagmentThread;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="local_ip_address"></param>
        /// <param name="MCU_ip_address"></param>
        /// <param name="MCU_port"></param>
        /// <param name="PLC_port"></param>
        public AbstractPLCDriver(IPAddress local_ip_address, IPAddress MCU_ip_address, int MCU_port, int PLC_port) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="local_ip"></param>
        /// <param name="MCU_ip"></param>
        /// <param name="MCU_port"></param>
        /// <param name="PLC_port"></param>
        public AbstractPLCDriver(string local_ip, string MCU_ip, int MCU_port, int PLC_port) : this(IPAddress.Parse(local_ip), IPAddress.Parse(MCU_ip), MCU_port, PLC_port) { }


        protected override bool KillHeartbeatComponent() {
             Bring_down();
            return true;
        }

        // public delegate void ReadReghandler<ModbusSlaveRequestEventArgs>(object sender, ModbusSlaveRequestEventArgs e);


        /// <summary>
        /// modbuss server implamentation specific to each device
        /// </summary>
        protected abstract void HandleClientManagementThread();


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

        public abstract void Bring_down();

        public abstract bool Test_Conection();

        public abstract Orientation read_Position();

        public abstract bool Cancle_move();

        public abstract bool Shutdown_PLC_MCU();

        public abstract bool Calibrate();

        public abstract bool Configure_MCU(int startSpeedAzimuth, int startSpeedElevation, int homeTimeoutAzimuth, int homeTimeoutElevation);

        public abstract bool Controled_stop(RadioTelescopeAxisEnum axis, bool both);

        public abstract bool Immediade_stop();

        public abstract bool relative_move(int programmedPeakSpeedAZInt, ushort ACCELERATION, int positionTranslationAZ, int positionTranslationEL);

        public abstract bool Move_to_orientation(Orientation target_orientation, Orientation current_orientation);

        public abstract bool Start_jog(RadioTelescopeAxisEnum axis, int speed, bool clockwise);

        public abstract bool Get_interlock_status();

        public abstract bool[] Get_Limit_switches();

        public abstract bool[] GET_MCU_Status();



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