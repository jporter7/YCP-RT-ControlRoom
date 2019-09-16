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
        private ProductionPLCDriver driver; 

        public SimulationPLCDriver(string local_ip, string MCU_ip, int MCU_port, int PLC_port, bool startPLC ) : base(local_ip, MCU_ip, MCU_port, PLC_port, startPLC )
        {
            if (MCU_port== PLC_port)//cant have 2 servers on the same port and ip 
            {
                MCU_port++;
            }
            SimMCU = new Simulation_control_pannel(local_ip, MCU_ip, MCU_port, PLC_port, false);
            Thread.Sleep(1000);//wait for server in simMcu to come up
            driver = new ProductionPLCDriver(local_ip, MCU_ip, MCU_port, PLC_port,false);
            if(startPLC) {
                driver.StartAsyncAcceptingClients();
            }
            SimMCU.startPLC();
        }



        public override bool RequestStopAsyncAcceptingClientsAndJoin()
        {
            return driver.RequestStopAsyncAcceptingClientsAndJoin();
        }

        public override bool StartAsyncAcceptingClients()
        {
            return driver.StartAsyncAcceptingClients();
        }

        protected override void HandleClientManagementThread()
        {
            throw new NotImplementedException();
        }

        public override void Bring_down()
        {
            SimMCU.Bring_down();
            driver.Bring_down();
        }

        public override bool Test_Conection()
        {
            return driver.Test_Conection();
        }

        public override Orientation read_Position()
        {
            return driver.read_Position();
        }

        public override bool Cancle_move()
        {
            return driver.Cancle_move();
        }

        public override bool Shutdown_PLC_MCU()
        {
            return driver.Shutdown_PLC_MCU();
        }

        public override bool Calibrate()
        {
            return driver.Calibrate();
        }

        public override bool Configure_MCU(double startSpeedAzimuth, double startSpeedElevation, int homeTimeoutAzimuth, int homeTimeoutElevation)
        {
            return driver.Configure_MCU(startSpeedAzimuth, startSpeedElevation, homeTimeoutAzimuth, homeTimeoutElevation);
        }

        public override bool Controled_stop(RadioTelescopeAxisEnum axis, bool both)
        {
            return driver.Controled_stop(axis , both );
        }

        public override bool Immediade_stop()
        {
            return driver.Immediade_stop();
        }

        public override bool relative_move(int programmedPeakSpeedAZInt, ushort ACCELERATION, int positionTranslationAZ, int positionTranslationEL)
        {
            return driver.relative_move(programmedPeakSpeedAZInt, ACCELERATION, positionTranslationAZ, positionTranslationEL);
        }

        public override bool Move_to_orientation(Orientation target_orientation, Orientation current_orientation)
        {
            return driver.Move_to_orientation(target_orientation, current_orientation);
        }

        public override bool Start_jog(RadioTelescopeAxisEnum axis, int speed, bool clockwise)
        {
            return driver.Start_jog(axis, speed, clockwise);
        }

        public override bool Get_interlock_status()
        {
            return driver.Get_interlock_status();
        }

        public override bool[] Get_Limit_switches()
        {
            return driver.Get_Limit_switches();
        }

        public override Task<bool[]> GET_MCU_Status( RadioTelescopeAxisEnum axis )
        {
            return driver.GET_MCU_Status( axis );
        }

        protected override bool TestIfComponentIsAlive() {
            return driver.workaroundAlive();
        }

    }
}
