using System;
using System.Net.Sockets;
using System.Threading;
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

        public SimulationPLCDriver(string local_ip, string MCU_ip, int MCU_port, int PLC_port) : base(local_ip, MCU_ip, MCU_port, PLC_port)
        {
            // Create the Simulation Motor Controller Unit to have absolute encoders with:
            //   1.) 12 bits of precision on the azimuth
            //   2.) 10 bits of precision on the elevation
            //SimMCU = new SimulationMCU(12, 10);
            //

            
            if (MCU_port== PLC_port)
            {
                MCU_port++;
            }
            SimMCU = new Simulation_control_pannel(local_ip, MCU_ip, MCU_port, PLC_port);
            Thread.Sleep(3000);
            driver = new ProductionPLCDriver(local_ip, MCU_ip, MCU_port, PLC_port);
            driver.StartAsyncAcceptingClients();
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

        public override bool Configure_MCU(int startSpeedAzimuth, int startSpeedElevation, int homeTimeoutAzimuth, int homeTimeoutElevation)
        {
            return driver.Configure_MCU(startSpeedAzimuth, startSpeedElevation, homeTimeoutAzimuth, homeTimeoutElevation);
        }

        public override bool Controled_stop()
        {
            throw new NotImplementedException();
        }

        public override bool Immediade_stop()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
