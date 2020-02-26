using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Simulators.Hardware.PLC_MCU;

namespace ControlRoomApplication.Controllers
{
    public class SimulationPLCDriver : AbstractPLCDriver
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Simulation_control_pannel SimMCU;
        public ProductionPLCDriver driver; 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="local_ip"></param>
        /// <param name="MCU_ip"></param>
        /// <param name="MCU_port"></param>
        /// <param name="PLC_port"></param>
        /// <param name="startPLC"></param>
        /// <param name="is_Test">if this is true the sim telescope will teleport to its final position</param>
        public SimulationPLCDriver(string local_ip, string MCU_ip, int MCU_port, int PLC_port, bool startPLC,bool is_Test ) : base(local_ip, MCU_ip, MCU_port, PLC_port )
        {
            SimMCU = new Simulation_control_pannel(local_ip, MCU_ip, MCU_port, PLC_port, is_Test);
            Thread.Sleep(1000);//wait for server in simMcu to come up
            driver = new ProductionPLCDriver(local_ip, MCU_ip, MCU_port, PLC_port);
            if(startPLC) {
                driver.StartAsyncAcceptingClients();
            }
            SimMCU.startPLC();
            driver.set_is_test( is_Test );
        }



        public override bool RequestStopAsyncAcceptingClientsAndJoin()
        {
            return driver.RequestStopAsyncAcceptingClientsAndJoin();
        }

        public override bool StartAsyncAcceptingClients()
        {
            return driver.StartAsyncAcceptingClients();
        }

        public override void HandleClientManagementThread()
        {
            driver.HandleClientManagementThread();
        }

        public override void Bring_down()
        {
            SimMCU.Bring_down();
            driver.Bring_down();
        }

        public override bool Test_Connection()
        {
            return driver.Test_Connection();
        }

        public override Orientation read_Position()
        {
            return driver.read_Position();
        }

        public override bool Cancel_move()
        {
            return driver.Cancel_move();
        }

        public override bool Shutdown_PLC_MCU()
        {
            return driver.Shutdown_PLC_MCU();
        }

        public override Task<bool> Thermal_Calibrate()
        {
            driver.SetParent(Parent);
            return driver.Thermal_Calibrate();
        }

        public override Task<bool> SnowDump()
        {
            return driver.SnowDump();
        }

        public override Task<bool> Stow()
        {
            return driver.Stow();
        }

        public override Task<bool> HitAzimuthLeftLimitSwitch()
        {
            return driver.HitAzimuthLeftLimitSwitch();
        }

        public override Task<bool> HitAzimuthRightLimitSwitch()
        {
            return driver.HitAzimuthRightLimitSwitch();
        }

        public override Task<bool> HitElevationLowerLimitSwitch()
        {
            return driver.HitElevationLowerLimitSwitch();
        }

        public override Task<bool> HitElevationUpperLimitSwitch()
        {
            return driver.HitElevationUpperLimitSwitch();
        }

        public override Task<bool> RecoverFromLimitSwitch()
        {
            return driver.RecoverFromLimitSwitch();
        }

        public override Task<bool> FullElevationMove()
        {
            return driver.FullElevationMove();
        }

        public override Task<bool> Full_360_CCW_Rotation()
        {
            return driver.Full_360_CCW_Rotation();
        }

        public override Task<bool> Full_360_CW_Rotation()
        {
            return driver.Full_360_CW_Rotation();
        }

        public override Task<bool> Hit_CW_Hardstop()
        {
            return driver.Hit_CW_Hardstop();
        }

        public override Task<bool> Hit_CCW_Hardstop()
        {
            return driver.Hit_CCW_Hardstop();
        }

        public override Task<bool> Recover_CW_Hardstop()
        {
            return driver.Recover_CW_Hardstop();
        }

        public override Task<bool> Recover_CCW_Hardstop()
        {
            return driver.Recover_CCW_Hardstop();
        }

        public override Task<bool> Hit_Hardstops()
        {
            return driver.Hit_Hardstops();
        }

        public override bool Configure_MCU(double startSpeedAzimuth, double startSpeedElevation, int homeTimeoutAzimuth, int homeTimeoutElevation)
        {
            return driver.Configure_MCU(startSpeedAzimuth, startSpeedElevation, homeTimeoutAzimuth, homeTimeoutElevation);
        }

        public override bool Controled_stop()
        {
            return driver.Controled_stop();
        }

        public override bool Immediade_stop()
        {
            return driver.Immediade_stop();
        }

        public override bool relative_move(int programmedPeakSpeedAZInt, ushort ACCELERATION, int positionTranslationAZ, int positionTranslationEL)
        {
            return driver.relative_move(programmedPeakSpeedAZInt, ACCELERATION, positionTranslationAZ, positionTranslationEL);
        }

        public override Task<bool> Move_to_orientation(Orientation target_orientation, Orientation current_orientation)
        {
            return driver.Move_to_orientation(target_orientation, current_orientation);
        }

        public override bool Start_jog(double AZspeed, bool AZ_CW, double ELspeed, bool EL_CW) {
            return driver.Start_jog( AZspeed, AZ_CW, ELspeed, EL_CW);
        }

        public override bool Get_interlock_status()
        {
            return driver.Get_interlock_status();
        }

        public override Task<bool[]> GET_MCU_Status( RadioTelescopeAxisEnum axis )
        {
            return driver.GET_MCU_Status( axis );
        }

        protected override bool TestIfComponentIsAlive() {
            return driver.workaroundAlive();
        }

        public override Task<bool> Home() {
            return driver.Home();
        }

        public override bool Stop_Jog() {
            return driver.Stop_Jog();
        }

        public override Task<bool> JogOffLimitSwitches() {
            return driver.JogOffLimitSwitches();
        }

        public override void setregvalue(ushort adr, ushort value)
        {
            driver.setregvalue(adr, value);
        }

        public override ushort getregvalue(ushort adr)
        {
            return driver.getregvalue(adr);
        }
    }
}
