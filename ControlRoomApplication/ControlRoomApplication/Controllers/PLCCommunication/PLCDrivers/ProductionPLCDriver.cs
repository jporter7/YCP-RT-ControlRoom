﻿using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using Modbus.Data;
using Modbus.Device;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using ControlRoomApplication.Entities.Configuration;
using ControlRoomApplication.Controllers.Communications;
using ControlRoomApplication.Controllers.PLCCommunication;
using ControlRoomApplication.Util;
using System.Collections.Generic;
using static ControlRoomApplication.Constants.MCUConstants;
using ControlRoomApplication.Database;
using System.Timers;

namespace ControlRoomApplication.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductionPLCDriver : AbstractPLCDriver
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private TcpListener PLCTCPListener;
        private ModbusSlave PLC_Modbusserver;
        private long PLC_last_contact = 0;
        private bool keep_modbus_server_alive = true;
        private bool is_test = false;
        private int temp =0;
        private MCUManager MCU;
        private RadioTelescopeTypeEnum telescopeType = RadioTelescopeTypeEnum.NONE;


        // Previous snow dump azimuth -- we need to keep track of this in order to add 45 degrees each time we dump
        private double previousSnowDumpAzimuth;

        // Snow dump timer
        private static System.Timers.Timer snowDumpTimer;

        // Used for the Unity simulation to keep track of its position
        // This is ONLY TEMPORARY until we can find the simulation source code and fix
        // this bug the correct way
        private bool IsSimulation;
        private Orientation CurrentSimOrientation;

        /// <summary>
        /// set this ONLY if using test driver, removes timouts and delays
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public bool set_is_test(bool val) { is_test = val; return is_test; }
        /// <summary>
        /// starts a modbus server to comunicate with the PLC on PLC_port and local_ip
        /// then sets up a modbus client to comunicate with the MCU located at MCU_ip, MCU_port (192.168.0.50 , 502) for actual hardware
        /// </summary>
        /// <param name="local_ip"></param>
        /// <param name="MCU_ip"></param>
        /// <param name="MCU_port"></param>
        /// <param name="PLC_port"></param>
        /// <param name="isSimulation">This wil tell us whether we are running a simulation or not.</param>
        public ProductionPLCDriver(string local_ip, string MCU_ip, int MCU_port, int PLC_port, bool isSimulation = false) : base(local_ip, MCU_ip, MCU_port, PLC_port)
        {
            if (isSimulation)
            {
                IsSimulation = isSimulation;
                CurrentSimOrientation = new Orientation();
            }

            limitSwitchData = new Simulators.Hardware.LimitSwitchData();
            homeSensorData = new Simulators.Hardware.HomeSensorData();
            pLCEvents = new PLCEvents();
            MCU = new MCUManager( MCU_ip , MCU_port  );

            try {
                PLCTCPListener = new TcpListener(new IPEndPoint(IPAddress.Parse(local_ip), PLC_port));
                ClientManagmentThread = new Thread( new ThreadStart( HandleClientManagementThread ) ) { Name = "PLC server Thread"};
            }
            catch (Exception e)
            {
                if ((e is ArgumentNullException) || (e is ArgumentOutOfRangeException))
                {
                    logger.Info(Utilities.GetTimeStamp() + ": [AbstractPLCDriver] ERROR: failure creating PLC TCP server or management thread: " + e.ToString());
                    return;
                }
                else { throw e; }// Unexpected exception
            }
            try
            {
                PLCTCPListener.Start(1);
            }
            catch (Exception e)
            {
                if ((e is SocketException) || (e is ArgumentOutOfRangeException) || (e is InvalidOperationException))
                {
                    logger.Info(Utilities.GetTimeStamp() + ": [AbstractPLCDriver] ERROR: failure starting PLC TCP server: " + e.ToString());
                    return;
                }
            }


            previousSnowDumpAzimuth = 0;

            snowDumpTimer = new System.Timers.Timer(DatabaseOperations.FetchWeatherThreshold().SnowDumpTime * 1000 * 60);
            snowDumpTimer.Elapsed += AutomaticSnowDumpInterval;
            snowDumpTimer.AutoReset = true;
            snowDumpTimer.Enabled = true;
        }



        /// <summary>
        /// runs the modbus server to interface with the plc
        /// </summary>
        public override void HandleClientManagementThread() {
            byte slaveId = 1;
            // create and start the TCP slave
            PLC_Modbusserver = ModbusTcpSlave.CreateTcp(slaveId, PLCTCPListener);
            //coils, inputs, holdingRegisters, inputRegisters
            PLC_Modbusserver.DataStore = DataStoreFactory.CreateDefaultDataStore(0, 0, 256, 0);
            // PLC_Modbusserver.DataStore.SyncRoot.ToString();

            PLC_Modbusserver.ModbusSlaveRequestReceived += new EventHandler<ModbusSlaveRequestEventArgs>(Server_Read_handler);
            PLC_Modbusserver.DataStore.DataStoreWrittenTo += new EventHandler<DataStoreEventArgs>(Server_Written_to_handler);

            PLC_Modbusserver.Listen();

            //PLC_Modbusserver.ListenAsync().GetAwaiter().GetResult();

            // prevent the main thread from exiting
            Task.Delay(2000).Wait();

            PLCEvents.setDefaultLimitHandler( DefaultLimitSwitchHandle );

            while (keep_modbus_server_alive) {
                Thread.Sleep(1000);
                PLC_Modbusserver.DataStore.HoldingRegisters[(int)PLC_modbus_server_register_mapping.CTRL_HEART_BEAT]++;
            }
        }

        private async void DefaultLimitSwitchHandle( object sender , limitEventArgs e ) {
            if(e.Value) {
                switch(e.ChangedValue) {
                    case PLC_modbus_server_register_mapping.AZ_0_LIMIT : {
                            MCU.SendSingleAxisJog(RadioTelescopeAxisEnum.AZIMUTH, RadioTelescopeDirectionEnum.Clockwise, 0.25);
                            break;
                        }
                    case PLC_modbus_server_register_mapping.AZ_375_LIMIT: {
                            MCU.SendSingleAxisJog(RadioTelescopeAxisEnum.AZIMUTH, RadioTelescopeDirectionEnum.Counterclockwise, 0.25);
                            break;
                        }
                    case PLC_modbus_server_register_mapping.EL_10_LIMIT: {
                            MCU.SendSingleAxisJog(RadioTelescopeAxisEnum.ELEVATION, RadioTelescopeDirectionEnum.Counterclockwise, 0.25);
                            break;
                        }
                    case PLC_modbus_server_register_mapping.EL_90_LIMIT: {
                            MCU.SendSingleAxisJog(RadioTelescopeAxisEnum.ELEVATION, RadioTelescopeDirectionEnum.Clockwise, 0.25);
                            break;
                        }

                }
            }
            if(!e.Value) {
                switch(e.ChangedValue) {
                    case PLC_modbus_server_register_mapping.AZ_0_LIMIT: {
                            MCU.StopSingleAxisJog(RadioTelescopeAxisEnum.AZIMUTH);
                            break;
                        }
                    case PLC_modbus_server_register_mapping.AZ_375_LIMIT: {
                            MCU.StopSingleAxisJog(RadioTelescopeAxisEnum.AZIMUTH);
                            break;
                        }
                    case PLC_modbus_server_register_mapping.EL_10_LIMIT: {
                            MCU.StopSingleAxisJog(RadioTelescopeAxisEnum.ELEVATION);
                            break;
                        }
                    case PLC_modbus_server_register_mapping.EL_90_LIMIT: {
                            MCU.StopSingleAxisJog(RadioTelescopeAxisEnum.ELEVATION);
                            break;
                        }
                }
            }
        }


        public override bool StartAsyncAcceptingClients() {
            MCU.StartAsyncAcceptingClients();
            keep_modbus_server_alive = true;
            try {
                ClientManagmentThread.Start();
            } catch (Exception e) {
                if ((e is ThreadStateException) || (e is OutOfMemoryException)) {
                    logger.Error(Utilities.GetTimeStamp() + ": failed to start prodi=uction plc and mcu threads err:____    {0}", e);
                    return false;
                } else { throw e; }// Unexpected exception
            }
            return true;
        }

        /// <summary>
        /// Waits for the PLC to contact the control room 
        /// </summary>
        /// <remarks>
        /// the PLC is constantly trying to send data to the controll room if the control room is not present it will retry again until it sucessfully connects
        /// this method will return a task that completes when that first contact occurs or when it times out
        /// </remarks>
        /// <param name="timeoutS"></param>
        /// <returns></returns>
        public async Task WaitForPLCConnection(int timeoutS) {
            var timout = new CancellationTokenSource( timeoutS*1000 );
            bool PLC_alive = false;
            while(!timout.Token.IsCancellationRequested && !PLC_alive) {
                PLC_alive = (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - PLC_last_contact) < 3000;
                Task.Delay( 50 ).Wait();
                if(PLC_alive) {
                    timout.Dispose();
                    logger.Info(Utilities.GetTimeStamp() + ": sucsefully conected to the PLC");
                    return;
                }
            }
            timout.Dispose();
            return;
        }

        public override bool RequestStopAsyncAcceptingClientsAndJoin() {
            MCU.RequestStopAsyncAcceptingClientsAndJoin();
            keep_modbus_server_alive = false;
            try
            {
                PLC_Modbusserver.Dispose();
                PLCTCPListener.Stop();
                ClientManagmentThread.Join();
            } catch (Exception e) {
                if ((e is ThreadStateException) || (e is ThreadStartException)) {
                    logger.Error(e);
                    return false;
                } else { throw e; }// Unexpected exception
            }
            return true;
        }


        public override void Bring_down() {
            RequestStopAsyncAcceptingClientsAndJoin();
            snowDumpTimer.Stop();
            snowDumpTimer.Dispose();
        }

        /// <summary>
        /// this can be used as a heart beat tracker as the plc will poll the ctrl room every ~100 ms
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Server_Read_handler(object sender, ModbusSlaveRequestEventArgs e) {
            if (is_test) {
                logger.Info(Utilities.GetTimeStamp() + ": PLC Red data from the the control room");
            }
            PLC_last_contact = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            // Console.WriteLine(e.Message);
            return;
            /*
            Regex rx = new Regex(@"\b(?:Read )([0-9]+)(?:.+)(?:address )([0-9]+)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection matches = rx.Matches(e.Message.ToString());
            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                Console.WriteLine("'{0}' repeated at positions {1} and {2}", groups["word"].Value, groups[0].Index, groups[1].Index);
            }
            //*/
        }


        /// <summary>
        /// fires whenever the data on the modbus server changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Server_Written_to_handler(object sender, DataStoreEventArgs e) {
            //e.Data.B //array representing data   
            if (is_test) {
                logger.Info(Utilities.GetTimeStamp() + ": recived message from PLC");
            }
            PLC_last_contact = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            switch (e.StartAddress) {
                case (ushort)PLC_modbus_server_register_mapping.AZ_0_LIMIT: {
                        bool previous = limitSwitchData.Azimuth_CCW_Limit;
                        limitSwitchData.Azimuth_CCW_Limit = !Int_to_bool( PLC_Modbusserver.DataStore.HoldingRegisters[(int)PLC_modbus_server_register_mapping.AZ_0_LIMIT] );
                        if(previous != limitSwitchData.Azimuth_CCW_Limit) {
                            pLCEvents.PLCLimitChanged( limitSwitchData , PLC_modbus_server_register_mapping.AZ_0_LIMIT , limitSwitchData.Azimuth_CCW_Limit );
                            if (limitSwitchData.Azimuth_CCW_Limit)
                            {
                                logger.Info(Utilities.GetTimeStamp() + ": Azimuth CCW Limit Switch Hit");

                                pushNotification.sendToAllAdmins("LIMIT SWITCH", "Azimuth CCW limit switch hit");
                                EmailNotifications.sendToAllAdmins("LIMIT SWITCH", "Azimuth CCW limit switch hit");
                            }
                            else
                            {
                                logger.Info(Utilities.GetTimeStamp() + ": Azimuth CCW Limit Switch Not Hit");

                                pushNotification.sendToAllAdmins("LIMIT SWITCH", "Azimuth CCW limit switch NOT hit");
                                EmailNotifications.sendToAllAdmins("LIMIT SWITCH", "Azimuth CCW limit switch NOT hit");
                            }
                        }
                        break;
                    }
                case (ushort)PLC_modbus_server_register_mapping.AZ_0_HOME: {
                        bool previous = homeSensorData.Azimuth_Home_One;
                        homeSensorData.Azimuth_Home_One = Int_to_bool( PLC_Modbusserver.DataStore.HoldingRegisters[(int)PLC_modbus_server_register_mapping.AZ_0_HOME] );
                        if(previous != homeSensorData.Azimuth_Home_One) {
                            if (homeSensorData.Azimuth_Home_One)
                            {
                                logger.Info(Utilities.GetTimeStamp() + ": Azimuth_Home_One Sensor Hit");

                                pushNotification.sendToAllAdmins("LIMIT SWITCH", "Azimuth_Home_One sensor hit");
                                EmailNotifications.sendToAllAdmins("LIMIT SWITCH", "Azimuth_Home_One sensor hit");
                            }
                            else
                            {
                                logger.Info(Utilities.GetTimeStamp() + ": Azimuth_Home_One Sensor Not Hit");

                                pushNotification.sendToAllAdmins("LIMIT SWITCH", "Azimuth_Home_One sensor NOT hit");
                                EmailNotifications.sendToAllAdmins("LIMIT SWITCH", "Azimuth_Home_One sensor NOT hit");
                            }
                        }
                        break;
                    }
                case (ushort)PLC_modbus_server_register_mapping.AZ_0_SECONDARY: {
                        bool previous = homeSensorData.Azimuth_Home_Two;
                        homeSensorData.Azimuth_Home_Two = Int_to_bool( PLC_Modbusserver.DataStore.HoldingRegisters[(int)PLC_modbus_server_register_mapping.AZ_0_SECONDARY] );
                        if(previous != homeSensorData.Azimuth_Home_Two) {
                            if (homeSensorData.Azimuth_Home_Two)
                            {
                                logger.Info(Utilities.GetTimeStamp() + ": Azimuth_Home_Two Sensor Hit");

                                pushNotification.sendToAllAdmins("LIMIT SWITCH", "Azimuth_Home_Two sensor hit");
                                EmailNotifications.sendToAllAdmins("LIMIT SWITCH", "Azimuth_Home_Two sensor hit");
                            }
                            else
                            {
                                logger.Info(Utilities.GetTimeStamp() + ": Azimuth_Home_Two Sensor Not Hit");;

                                pushNotification.sendToAllAdmins("LIMIT SWITCH", "Azimuth_Home_Two sensor NOT hit");
                                EmailNotifications.sendToAllAdmins("LIMIT SWITCH", "Azimuth_Home_Two sensor NOT hit");
                            }
                        }
                        break;
                    }
                case (ushort)PLC_modbus_server_register_mapping.AZ_375_LIMIT: {
                        bool previous = limitSwitchData.Azimuth_CW_Limit;
                        limitSwitchData.Azimuth_CW_Limit = !Int_to_bool( PLC_Modbusserver.DataStore.HoldingRegisters[(int)PLC_modbus_server_register_mapping.AZ_375_LIMIT] );
                        if(previous != limitSwitchData.Azimuth_CW_Limit) {
                            pLCEvents.PLCLimitChanged( limitSwitchData , PLC_modbus_server_register_mapping.AZ_375_LIMIT , limitSwitchData.Azimuth_CW_Limit );
                            if (limitSwitchData.Azimuth_CW_Limit)
                            {
                                logger.Info(Utilities.GetTimeStamp() + ": Azimuth CW Limit Switch Hit");

                                pushNotification.sendToAllAdmins("LIMIT SWITCH", "Azimuth CW limit switch hit");
                                EmailNotifications.sendToAllAdmins("LIMIT SWITCH", "Azimuth CW limit switch hit");
                            }
                            else
                            {
                                logger.Info(Utilities.GetTimeStamp() + ": Azimuth CW Limit Switch Not Hit");

                                pushNotification.sendToAllAdmins("LIMIT SWITCH", "Azimuth CW limit switch NOT hit");
                                EmailNotifications.sendToAllAdmins("LIMIT SWITCH", "Azimuth CW limit switch NOT hit");
                            }
                        }
                        break;
                    }
                case (ushort)PLC_modbus_server_register_mapping.EL_10_LIMIT: {
                        bool previous = limitSwitchData.Elevation_Lower_Limit;
                        limitSwitchData.Elevation_Lower_Limit = !Int_to_bool( PLC_Modbusserver.DataStore.HoldingRegisters[(int)PLC_modbus_server_register_mapping.EL_10_LIMIT]);
                        if(previous != limitSwitchData.Elevation_Lower_Limit) {
                            pLCEvents.PLCLimitChanged( limitSwitchData , PLC_modbus_server_register_mapping.EL_10_LIMIT , limitSwitchData.Elevation_Lower_Limit );
                            if (limitSwitchData.Elevation_Lower_Limit)
                            {
                                logger.Info(Utilities.GetTimeStamp() + ": Elevation Lower Limit Switch Hit");

                                pushNotification.sendToAllAdmins("LIMIT SWITCH", "Elevation lower limit switch hit");
                                EmailNotifications.sendToAllAdmins("LIMIT SWITCH", "Elevation lower limit switch hit");
                            }
                            else
                            {
                                logger.Info(Utilities.GetTimeStamp() + ": Elevation Lower Limit Switch Not Hit");

                                pushNotification.sendToAllAdmins("LIMIT SWITCH", "Elevation lower limit switch NOT hit");
                                EmailNotifications.sendToAllAdmins("LIMIT SWITCH", "Elevation lower limit switch NOT hit");
                            }
                        }
                        break;
                    }
                case (ushort)PLC_modbus_server_register_mapping.EL_0_HOME: {
                        bool previous = homeSensorData.Elevation_Home;
                        homeSensorData.Elevation_Home = Int_to_bool( PLC_Modbusserver.DataStore.HoldingRegisters[(int)PLC_modbus_server_register_mapping.EL_0_HOME] );
                        if(previous != homeSensorData.Elevation_Home) {
                            if (homeSensorData.Elevation_Home)
                            {
                                logger.Info(Utilities.GetTimeStamp() + ": Elevation Home Sensor Hit");

                                pushNotification.sendToAllAdmins("LIMIT SWITCH", "Elevation home sensor hit");
                                EmailNotifications.sendToAllAdmins("LIMIT SWITCH", "Elevation home sensor hit");
                            }
                            else
                            {
                                logger.Info(Utilities.GetTimeStamp() + ": Elevation Home Sensor Not Hit");

                                pushNotification.sendToAllAdmins("LIMIT SWITCH", "Elevation home sensor NOT hit");
                                EmailNotifications.sendToAllAdmins("LIMIT SWITCH", "Elevation home sensor NOT hit");
                            }
                        }
                        break;
                    }
                case (ushort)PLC_modbus_server_register_mapping.EL_90_LIMIT: {
                        bool previous = limitSwitchData.Elevation_Upper_Limit;
                        limitSwitchData.Elevation_Upper_Limit = !Int_to_bool( PLC_Modbusserver.DataStore.HoldingRegisters[(int)PLC_modbus_server_register_mapping.EL_90_LIMIT] );
                        if(previous != limitSwitchData.Elevation_Upper_Limit) {
                            pLCEvents.PLCLimitChanged( limitSwitchData , PLC_modbus_server_register_mapping.EL_90_LIMIT , limitSwitchData.Elevation_Upper_Limit );
                            if (limitSwitchData.Elevation_Upper_Limit)
                            {
                                logger.Info(Utilities.GetTimeStamp() + ": Elevation Upper Limit Switch Hit");

                                pushNotification.sendToAllAdmins("LIMIT SWITCH", "Elevation upper limit switch hit");
                                EmailNotifications.sendToAllAdmins("LIMIT SWITCH", "Elevation upper limit switch hit");
                            }
                            else
                            {
                                logger.Info(Utilities.GetTimeStamp() + ": Elevation Upper Limit Switch Not Hit");

                                pushNotification.sendToAllAdmins("LIMIT SWITCH", "Elevation upper limit switch NOT hit");
                                EmailNotifications.sendToAllAdmins("LIMIT SWITCH", "Elevation upper limit switch NOT hit");
                            }
                        }
                        break;
                    }
                case (ushort)PLC_modbus_server_register_mapping.Gate_Safety_INTERLOCK: {
                        bool previous = plcInput.Gate_Sensor;
                        plcInput.Gate_Sensor = !Int_to_bool( PLC_Modbusserver.DataStore.HoldingRegisters[(int)PLC_modbus_server_register_mapping.Gate_Safety_INTERLOCK] );
                        if(previous != plcInput.Gate_Sensor) {
                            if (plcInput.Gate_Sensor)
                            {
                                logger.Info(Utilities.GetTimeStamp() + ": gate opened");

                                pushNotification.sendToAllAdmins("GATE ACTIVITY", "Gate has been opened.");
                                EmailNotifications.sendToAllAdmins("GATE ACTIVITY", "Gate has been opened.");
                            }
                            else
                            {
                                logger.Info(Utilities.GetTimeStamp() + ": gate closed");

                                pushNotification.sendToAllAdmins("GATE ACTIVITY", "Gate has been closed.");
                                EmailNotifications.sendToAllAdmins("GATE ACTIVITY", "Gate has been closed.");
                            }
                        }
                        break;
                    }
                case (ushort)PLC_modbus_server_register_mapping.E_STOP: {
                        bool previous = plcInput.Estop;
                        plcInput.Estop = !Int_to_bool( PLC_Modbusserver.DataStore.HoldingRegisters[(int)PLC_modbus_server_register_mapping.E_STOP] );
                        if(previous != plcInput.Estop) {
                            if (plcInput.Estop)
                            {
                                logger.Info(Utilities.GetTimeStamp() + ": Estop Hit");

                                pushNotification.sendToAllAdmins("E-STOP ACTIVITY", "E-stop has been hit.");
                                EmailNotifications.sendToAllAdmins("E-STOP ACTIVITY", "E-stop has been hit.");
                            }
                            else
                            {
                                logger.Info(Utilities.GetTimeStamp() + ": Estop released");

                                pushNotification.sendToAllAdmins("E-STOP ACTIVITY", "E-stop has been released.");
                                EmailNotifications.sendToAllAdmins("E-STOP ACTIVITY", "E-stop has been released.");
                            }
                        }
                        break;
                    }
                case (ushort)PLC_modbus_server_register_mapping.EL_SLIP_CAPTURE: {
                        bool previous = plcInput.EL_Slip_CAPTURE;
                        plcInput.EL_Slip_CAPTURE = Int_to_bool( PLC_Modbusserver.DataStore.HoldingRegisters[(int)PLC_modbus_server_register_mapping.EL_SLIP_CAPTURE] );
                        break;
                    }
            }
        }

        private void set_Local_registers(ushort[] data, ushort starting_adress) {
            logger.Info(data.Length + " dsv " + starting_adress);
            for (int i = 1; i < (data.Length - 1); i++) {
                PLC_Modbusserver.DataStore.HoldingRegisters[i + starting_adress] = data[i];
                logger.Info( " " + PLC_Modbusserver.DataStore.HoldingRegisters[i + starting_adress] + ",");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="adr"></param>
        /// <param name="value"></param>
        public override void setregvalue(ushort adr, ushort value) {
            PLC_Modbusserver.DataStore.HoldingRegisters[adr] = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="adr"></param>
        public override ushort getregvalue(ushort adr)
        {
            return PLC_Modbusserver.DataStore.HoldingRegisters[adr];
        }

        /// <summary>
        /// see   ControlRoomApplication.Entities.PLC_modbus_server_register_mapping
        /// for register maping
        /// </summary>
        /// <param name="adr"></param>
        /// <returns></returns>
        public ushort readregval(ushort adr) {
            return PLC_Modbusserver.DataStore.HoldingRegisters[adr];
        }



        public override bool Get_interlock_status() {
            return plcInput.Gate_Sensor;
            //return Int_to_bool( PLC_Modbusserver.DataStore.HoldingRegisters[(ushort)PLC_modbus_server_register_mapping.Gate_Safety_INTERLOCK] );
        }

        private bool Int_to_bool(int val) {
            if (val == 0) {
                return false;
            } else { return true; }
        }


        //above PLC modbus server ___below MCU comands






        public override bool Test_Connection() {
            return TestIfComponentIsAlive();
        }




        /// <summary>
        /// This is a script that is called when we want to check the current thermal calibration of the telescope
        /// Moves to point to the tree, reads in data, gets data from weather station, and compares
        /// Postcondition: return true if the telescope data IS within 0.001 degrees fahrenheit
        ///                return false if the telescope data IS NOT within 0.001 degrees fahrenheit
        /// </summary>
        public override bool Thermal_Calibrate() {
            Orientation current = read_Position();
            Move_to_orientation(MiscellaneousConstants.THERMAL_CALIBRATION_ORIENTATION, current);

            // start a timer so we can have a time variable
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            // temporarily set spectracyber mode to continuum
            Parent.SpectraCyberController.SetSpectraCyberModeType(SpectraCyberModeTypeEnum.CONTINUUM);

            // read data
            SpectraCyberResponse response = Parent.SpectraCyberController.DoSpectraCyberScan();

            // end the timer
            stopWatch.Stop();
            double time = stopWatch.Elapsed.TotalSeconds;

            RFData rfResponse = RFData.GenerateFrom(response);

            // move back to previous location
            Move_to_orientation(current, MiscellaneousConstants.THERMAL_CALIBRATION_ORIENTATION);

            // analyze data
            // temperature (Kelvin) = (intensity * time * wein's displacement constant) / (Planck's constant * speed of light)
            double weinConstant = 2.8977729;
            double planckConstant = 6.62607004 * Math.Pow(10, -34);
            double speedConstant = 299792458;
            double temperature = (rfResponse.Intensity * time * weinConstant) / (planckConstant * speedConstant);

            // convert to fahrenheit
            temperature = temperature * (9 / 5) - 459.67;

            // check against weather station reading
            double weatherStationTemp = Parent.WeatherStation.GetOutsideTemp();

            // Check if we need to dump the snow off of the telescope
            if (Parent.WeatherStation.GetOutsideTemp() <= 40.00 && Parent.WeatherStation.GetTotalRain() > 0.00)
            {
                SnowDump();
            }

            // Set SpectraCyber mode back to UNKNOWN
            Parent.SpectraCyberController.SetSpectraCyberModeType(SpectraCyberModeTypeEnum.UNKNOWN);

            // return true if working correctly, false if not
            if (Math.Abs(weatherStationTemp - temperature) < MiscellaneousConstants.THERMAL_CALIBRATION_OFFSET)
            {
                return Move_to_orientation(MiscellaneousConstants.Stow, MiscellaneousConstants.THERMAL_CALIBRATION_ORIENTATION);
            }

            return false;
        }

        /// <summary>
        /// This is a script that is called when we want to dump snow out of the dish
        /// </summary>
        public override bool SnowDump()
        {
            // insert snow dump movements here
            // default is azimuth of 0 and elevation of 0
            Orientation dump = new Orientation(previousSnowDumpAzimuth += 45, -5);
            Orientation current = read_Position();

            Orientation dumpAzimuth = new Orientation(dump.Azimuth, current.Elevation);
            Orientation dumpElevation = new Orientation(dump.Azimuth, dump.Elevation);

            // move to dump snow
            Move_to_orientation(dumpAzimuth, current);
            Move_to_orientation(dumpElevation, dumpAzimuth);
           

            // move back to initial orientation
            return Move_to_orientation(current, dump);
        }

        private void AutomaticSnowDumpInterval(Object source, ElapsedEventArgs e)
        {

            // Check if we need to dump the snow off of the telescope
            if (Parent.WeatherStation.GetOutsideTemp() <= 30.00 && Parent.WeatherStation.GetTotalRain() > 0.00)
            {
                Console.WriteLine("Time threshold reached. Running snow dump...");

                SnowDump();

                Console.WriteLine("Snow dump completed");
            }
        }

        /// <summary>
        /// Moves the telescope to the left azimuth switch
        /// </summary>
        public override bool HitAzimuthLeftLimitSwitch()
        {
            Orientation AZLeftLimit = new Orientation(-9, 0);

            return Move_to_orientation(AZLeftLimit, read_Position());
        }

        /// <summary>
        /// Moves the telescope to the right azimuth switch
        /// </summary>
        public override bool HitAzimuthRightLimitSwitch()
        {
            Orientation AZRightLimit = new Orientation(369, 0);

            return Move_to_orientation(AZRightLimit, read_Position());
        }

        /// <summary>
        /// Moves the telescope to the lower elevation switch
        /// </summary>
        public override bool HitElevationLowerLimitSwitch()
        {
            Orientation ELLowerLimit = new Orientation(0, -14);

            return Move_to_orientation(ELLowerLimit, read_Position());
        }

        /// <summary>
        /// Moves the telescope to the upper elevation switch
        /// </summary>
        public override bool HitElevationUpperLimitSwitch()
        {
            Orientation ELUpperLimit = new Orientation(0, 92);

            return Move_to_orientation(ELUpperLimit, read_Position());
        }

        /// <summary>
        /// Recovers the telescope when a limit switch is hit
        /// </summary>
        public override bool RecoverFromLimitSwitch()
        {
            Orientation currentPos = read_Position();

            Orientation safe;

            bool safeAz = false;
            bool safeEl = false;

            bool azTask;
            bool elTask;

            // Loops through just in case the move fails or if it as hit two limit switches
            while (true)
            {
                // Checks to see if the left az switch has been hit
                /// TODO: Update to also use limit switch sensors
                if (currentPos.Azimuth <= -8 && !safeAz)
                {
                    safe = new Orientation(0, currentPos.Elevation);

                    azTask = Move_to_orientation(safe, currentPos);
                    safeAz = azTask;
                }
                // Checks to see if the right az switch has been hit
                /// TODO: Update to also use limit switch sensors
                else if (currentPos.Azimuth >= 368 && !safeAz)
                {
                    safe = new Orientation(360, currentPos.Elevation);

                    azTask = Move_to_orientation(safe, currentPos);
                    safeAz = azTask;
                }
                else
                {
                    safeAz = true;
                    azTask = false;
                }

                // Checks to see if the lower el switch has been hit
                /// TODO: Update to also use limit switch sensors
                if (currentPos.Elevation <= -13 && !safeEl)
                {
                    safe = new Orientation(currentPos.Azimuth, 0);

                    elTask = Move_to_orientation(safe, currentPos);
                    safeEl = elTask;
                }
                // Checks to see if the upper el switch has been hit
                /// TODO: Update to also use limit switch sensors
                else if (currentPos.Elevation >= 91 && !safeEl)
                {
                    safe = new Orientation(currentPos.Azimuth, 85);

                    elTask = Move_to_orientation(safe, currentPos);
                    safeEl = elTask;
                }
                else
                {
                    safeEl = true;
                    elTask = false;
                }

                // Check to see if the telescope is in a safe state
                if (safeAz && safeEl)
                    return elTask;
            }
        }

        /// <summary>
        /// Moves the telescope from its current position to a start position at
        /// 0 degrees elevation, then moves to 90 degrees, then returns to its
        /// initial position
        /// </summary>
        public override bool FullElevationMove()
        {
            Orientation currentPos = read_Position();

            bool elStartFlag;
            bool elFinishFlag;

            Orientation elStart = new Orientation(currentPos.Azimuth, 0); ;
            Orientation elFinish = new Orientation(currentPos.Azimuth, 90);

            elStartFlag = Move_to_orientation(elStart, currentPos);

            elFinishFlag = Move_to_orientation(elFinish, elStart);

            if(!elStartFlag || !elFinishFlag)
            {
                throw new Exception();
            }

            return Move_to_orientation(currentPos, elFinish);
        }
        
        /// <summary>
        /// This is a script that is called when we want to move the telescope in a full 360 degree azimuth rotation
        /// The counter clockwise direction
        /// </summary>
        public override bool Full_360_CCW_Rotation()
        {
            Orientation current = read_Position();
            Orientation finish;

            if (current.Azimuth == 359)
            {
                finish = new Orientation(0, current.Elevation);
                return Move_to_orientation(finish, current);
            }
            else
            {
                finish = new Orientation(current.Azimuth + 1, current.Elevation);
                return Move_to_orientation(finish, current);
            }

        }

        /// <summary>
        /// This is a script that is called when we want to move the telescope in a full 360 degree azimuth rotation
        /// The clockwise direction
        /// </summary>
        public override bool Full_360_CW_Rotation()
        {
            Orientation current = read_Position();
            Orientation start = new Orientation(0, 0);
            Orientation finish = new Orientation(360, 0);

            if (!Move_to_orientation(start, current) || !Move_to_orientation(finish, start))
            {
                throw new Exception();
            }
            return Move_to_orientation(current, finish);
        }

        /// <summary>
        /// This is a script that is called when we want to move the telescope to the CW hardware stop
        /// </summary>
        public override bool Hit_CW_Hardstop()
        {
            Orientation current = read_Position();
            Orientation hardstop = new Orientation(370, current.Elevation);

            return Move_to_orientation(hardstop, current);
        }

        /// <summary>
        /// This is a script that is called when we want to move the telescope to the CCW hardware stop
        /// </summary>
        public override bool Hit_CCW_Hardstop()
        {
            Orientation current = read_Position();
            Orientation hardstop = new Orientation(-10, current.Elevation);

            return Move_to_orientation(hardstop, current);
        }

        /// <summary>
        /// This is a script that is called when we want to move the telescope from the current position
        /// to a safe position away from the hardstop
        /// Precondition: The telescope just hit the clockwise hardstop
        /// Postcondition: The telescope will be placed at 360 degrees azimuth (safe spot away from hard stop)
        /// </summary>
        public override bool Recover_CW_Hardstop()
        {
            Orientation current = read_Position();
            Orientation recover = new Orientation(360, current.Elevation);

            return Move_to_orientation(recover, current);
        }

        /// <summary>
        /// This is a script that is called when we want to move the telescope from the current position
        /// to a safe position away from the hardstop
        /// Precondition: The telescope just hit the counter clockwise hardstop
        /// Postcondition: The telescope will be placed at 0 degrees azimuth (safe spot away from hard stop)
        /// </summary>
        public override bool Recover_CCW_Hardstop()
        {
            Orientation current = read_Position();
            Orientation recover = new Orientation(0, current.Elevation);

            return Move_to_orientation(recover, current);
        }

        /// <summary>
        /// send basic configuration to MCU
        /// </summary>
        /// <param name="startSpeedRPMAzimuth"></param>
        /// <param name="startSpeedRPMElevation"></param>
        /// <param name="homeTimeoutSecondsAzimuth"></param>
        /// <param name="homeTimeoutSecondsElevation"></param>
        /// <returns></returns>
        public override bool Configure_MCU(double startSpeedRPMAzimuth, double startSpeedRPMElevation, int homeTimeoutSecondsAzimuth, int homeTimeoutSecondsElevation) {
            return MCU.Configure_MCU(
                    new MCUConfigurationAxys() { StartSpeed = startSpeedRPMAzimuth, HomeTimeoutSec = homeTimeoutSecondsAzimuth },
                    new MCUConfigurationAxys() { StartSpeed = startSpeedRPMElevation, HomeTimeoutSec = homeTimeoutSecondsElevation,UseCapture =true,CaptureActive_High =true }
                );
        }

        /// <summary>
        /// this gets the position stored in the MCU which is based of the number of steps the MCU has taken since it was last 0ed out
        /// </summary>
        /// <returns></returns>
        public override Orientation read_Position(){

            if (IsSimulation) return CurrentSimOrientation;
            else return MCU.read_Position();
        }

        /// <summary>
        /// get an array of boolens representiing the register described on pages 76 -79 of the mcu documentation 
        /// does not suport RadioTelescopeAxisEnum.BOTH
        /// see <see cref="MCUConstants.MCUStatusBitsMSW"/> for description of each bit
        /// </summary>
        public override bool[] GET_MCU_Status( RadioTelescopeAxisEnum axis ) {
            ushort start = 0;
            if(axis == RadioTelescopeAxisEnum.ELEVATION) {
                start = 10;
            }
            ushort[] data = MCU.ReadMCURegisters(start , 2);
            bool[] target = new bool[32];
            for(int i = 0; i < 16; i++) {
                target[i] = ((data[0] >> i) & 1) == 1;
                target[i + 16] = ((data[1] >> i) & 1) == 1;

            }
            return target;
        }

        /// <summary>
        /// clears the previos move comand from mthe PLC, only works for jog moves
        /// </summary>
        /// <returns></returns>
        public override bool Cancel_move() {
            return MCU.Cancel_move();
        }

        /// <summary>
        /// send a hold move command to the MCu
        /// </summary>
        /// <returns></returns>
        public override bool ControlledStop(  ) {
            return MCU.ControlledStop();
        }

        public override bool ImmediateStop() {
            return MCU.ImmediateStop();
        }

        /// <summary>
        /// move a set number of steps at the specified steps / second *intended for debuging
        /// </summary>
        /// <param name="programmedPeakSpeedAZInt"></param>
        /// <param name="ACCELERATION"></param>
        /// <param name="positionTranslationAZ"></param>
        /// <param name="positionTranslationEL"></param>
        /// <returns></returns>
        public override bool relative_move( int programmedPeakSpeedAZInt , ushort ACCELERATION , int positionTranslationAZ , int positionTranslationEL ) {
            if(IsSimulation)
            {
                CurrentSimOrientation = new Orientation(
                    ConversionHelper.StepsToDegrees(positionTranslationAZ, MotorConstants.GEARING_RATIO_AZIMUTH),
                    ConversionHelper.StepsToDegrees(positionTranslationEL, MotorConstants.GEARING_RATIO_ELEVATION)
                );
            }

            return send_relative_move( programmedPeakSpeedAZInt , programmedPeakSpeedAZInt , ACCELERATION , positionTranslationAZ , positionTranslationEL );
        }


        public override bool Move_to_orientation(Orientation target_orientation, Orientation current_orientation)
        {
            int positionTranslationAZ, positionTranslationEL;

            // Calculates the hard stop calculation by default
            double azimuthOrientationMovement = target_orientation.Azimuth - current_orientation.Azimuth;

            // If the type is a slip ring, checks to see if it needs to calculate a new route
            if(telescopeType == RadioTelescopeTypeEnum.SLIP_RING)
            {
                if(azimuthOrientationMovement > 180)
                {
                    azimuthOrientationMovement = (target_orientation.Azimuth - 360) - current_orientation.Azimuth;
                }
                else if(azimuthOrientationMovement < -180)
                {
                    azimuthOrientationMovement = (360 - current_orientation.Azimuth) + target_orientation.Azimuth;
                }
            }
            else if(telescopeType == RadioTelescopeTypeEnum.NONE)
            {
                logger.Info(Utilities.GetTimeStamp() + ": ERROR: Invalid Telescope Type!");
                return false;
            }

            positionTranslationAZ = ConversionHelper.DegreesToSteps(azimuthOrientationMovement, MotorConstants.GEARING_RATIO_AZIMUTH);
            positionTranslationEL = ConversionHelper.DegreesToSteps((target_orientation.Elevation - current_orientation.Elevation), MotorConstants.GEARING_RATIO_ELEVATION);

            int EL_Speed = ConversionHelper.DPSToSPS( ConversionHelper.RPMToDPS( 0.6 ), MotorConstants.GEARING_RATIO_ELEVATION);
            int AZ_Speed = ConversionHelper.DPSToSPS( ConversionHelper.RPMToDPS( 0.6 ), MotorConstants.GEARING_RATIO_AZIMUTH);

            //(ObjectivePositionStepsAZ - CurrentPositionStepsAZ), (ObjectivePositionStepsEL - CurrentPositionStepsEL)
            logger.Info(Utilities.GetTimeStamp() + ": degrees target az " + target_orientation.Azimuth + " el " + target_orientation.Elevation);
            logger.Info(Utilities.GetTimeStamp() + ": degrees curren az " + current_orientation.Azimuth + " el " + current_orientation.Elevation);

            // Set the simulation's current position
            if(IsSimulation) CurrentSimOrientation = target_orientation;

            //return sendmovecomand( EL_Speed * 20 , 50 , positionTranslationAZ , positionTranslationEL ).GetAwaiter().GetResult();
            return send_relative_move( AZ_Speed , EL_Speed ,50, positionTranslationAZ , positionTranslationEL );
        }

        /// <summary>
        /// speed in RPM
        /// </summary>
        /// <param name="AZspeed"></param>
        /// <param name="AZ_CW"></param>
        /// <param name="ELspeed"></param>
        /// <param name="ELPositive"></param>
        /// <returns></returns>
        public override bool Start_jog(double AZspeed, bool AZ_CW, double ELspeed, bool ELPositive) {
            return MCU.Send_Jog_command( Math.Abs( AZspeed ), AZ_CW, Math.Abs( ELspeed ), ELPositive);
        }

        public override bool Stop_Jog() {
            return MCU.Cancel_move();
        }

        public bool send_relative_move( int SpeedAZ , int SpeedEL , ushort ACCELERATION , int positionTranslationAZ , int positionTranslationEL ) {
            return MCU.MoveAndWaitForCompletion( SpeedAZ , SpeedEL , ACCELERATION , positionTranslationAZ , positionTranslationEL);
        }

        /// <summary>
        /// Moves both axes to where the homing sensors are. After this is run, the position offset needs applied to the motors, and then
        /// the absolute encoders.
        /// </summary>
        /// <returns>True if homing was successful, false if it failed</returns>
        public override bool HomeTelescope() {
            // We only want to perform this movement if the telescope has hard stops, because the range of motion is 0-375. That gives
            // us 15 degrees of overlap, so we want to make sure we aren't hitting a limit switch
            if (telescopeType == RadioTelescopeTypeEnum.HARD_STOPS)
            {
                int EL_Speed = ConversionHelper.DPSToSPS(ConversionHelper.RPMToDPS(0.1), MotorConstants.GEARING_RATIO_ELEVATION);
                int AZ_Speed = ConversionHelper.DPSToSPS(ConversionHelper.RPMToDPS(0.1), MotorConstants.GEARING_RATIO_AZIMUTH);
                int EL_Fast = ConversionHelper.DPSToSPS(ConversionHelper.RPMToDPS(0.6), MotorConstants.GEARING_RATIO_ELEVATION);
                int AZ_Fast = ConversionHelper.DPSToSPS(ConversionHelper.RPMToDPS(0.6), MotorConstants.GEARING_RATIO_AZIMUTH);

                bool ZeroOne = Int_to_bool(PLC_Modbusserver.DataStore.HoldingRegisters[(ushort)PLC_modbus_server_register_mapping.AZ_0_HOME]);  //active between 350 to 360 and -10 to 0 //primary home sensor for MCU
                bool ZeroTwo = Int_to_bool(PLC_Modbusserver.DataStore.HoldingRegisters[(ushort)PLC_modbus_server_register_mapping.AZ_0_SECONDARY]);//active between -1 to 10   and 359 to 370
                                                                                                                                                   // comented out for testing
                PLCEvents.OverrideLimitHandlers((object sender, limitEventArgs e) =>
                {
                    logger.Debug("limit hit durring homing, default handler disabled");
                });
                if (ZeroOne & ZeroTwo)
                {//very close to 0 degrees 
                 //  move 15 degrees ccw slowly to ensure that we arent near a limit switch then home
                    MCU.MoveAndWaitForCompletion(AZ_Speed, EL_Speed, 50, ConversionHelper.DegreesToSteps(20, MotorConstants.GEARING_RATIO_AZIMUTH), 0);
                    if (limitSwitchData.Azimuth_CW_Limit)
                    {// we were actually at 360 and need to go back towards 0
                        JogOffLimitSwitches().GetAwaiter().GetResult();
                        MCU.MoveAndWaitForCompletion(AZ_Fast, EL_Fast, 50, ConversionHelper.DegreesToSteps(-250, MotorConstants.GEARING_RATIO_AZIMUTH), 0);
                    }
                }
                else if (ZeroOne & !ZeroTwo)
                {//350 to 360 or -10 to 0
                 //  move 15 degrees cw slowly to ensure that we arent near a limit switch then home
                    MCU.MoveAndWaitForCompletion(AZ_Speed, EL_Speed, 50, ConversionHelper.DegreesToSteps(-20, MotorConstants.GEARING_RATIO_AZIMUTH), 0);
                    if (limitSwitchData.Azimuth_CCW_Limit)
                    {// we were actually less than 0 and need to go back past 0
                        JogOffLimitSwitches().GetAwaiter().GetResult();
                        MCU.MoveAndWaitForCompletion(AZ_Fast, EL_Fast, 50, ConversionHelper.DegreesToSteps(25, MotorConstants.GEARING_RATIO_AZIMUTH), 0);
                    }
                }
                else if (!ZeroOne & ZeroTwo)
                {//0 to 10   or 360 to 370
                 //  move 15 degrees ccw slowly to ensure that we arent near a limit switch then home
                    MCU.MoveAndWaitForCompletion(AZ_Speed, EL_Speed, 50, ConversionHelper.DegreesToSteps(20, MotorConstants.GEARING_RATIO_AZIMUTH), 0);
                    if (limitSwitchData.Azimuth_CW_Limit)
                    {// we were actually at 360 and need to go back towards 0
                        JogOffLimitSwitches().GetAwaiter().GetResult();
                        MCU.MoveAndWaitForCompletion(AZ_Fast, EL_Fast, 50, ConversionHelper.DegreesToSteps(-250, MotorConstants.GEARING_RATIO_AZIMUTH), 0);
                    }
                }
                else
                {
                    //we know our position is valid and can imedeatly perform a cw home
                }

                PLCEvents.ResetOverrides();
            }

            bool ELHome = Int_to_bool(PLC_Modbusserver.DataStore.HoldingRegisters[(ushort)PLC_modbus_server_register_mapping.EL_0_HOME]);
            MCU.HomeBothAxyes( true , ELHome , 0.25);

            return true;
        }

        public override async Task<bool> JogOffLimitSwitches() {
            var AZTAsk = Task.Run( () => {
               int AZstepSpeed = ConversionHelper.RPMToSPS( 0.2 , MotorConstants.GEARING_RATIO_AZIMUTH );
               var timeoutMS = MCUManager.EstimateMovementTime( AZstepSpeed , 50 ,ConversionHelper.DegreesToSteps(30, MotorConstants.GEARING_RATIO_AZIMUTH ));
               var timeout = new CancellationTokenSource( timeoutMS );
                if (limitSwitchData.Azimuth_CCW_Limit && !limitSwitchData.Azimuth_CW_Limit) {
                    MCU.Send_Jog_command(0.2, true , 0, false);
                   while(!timeout.IsCancellationRequested) {
                        Task.Delay( 33 ).Wait();
                        if(!limitSwitchData.Azimuth_CCW_Limit) {
                            Cancel_move();
                            return true;
                        }
                   }
                   Cancel_move();
                   return false;
               } else if (!limitSwitchData.Azimuth_CCW_Limit && limitSwitchData.Azimuth_CW_Limit) {
                    MCU.Send_Jog_command(0.2, false , 0, false);
                   while(!timeout.IsCancellationRequested) {
                        Task.Delay( 33 ).Wait();
                        if(!limitSwitchData.Azimuth_CW_Limit) {
                            Cancel_move();
                            return true;
                        }
                   }
                   Cancel_move();
                   return false;
               } else if(!limitSwitchData.Azimuth_CCW_Limit && !limitSwitchData.Azimuth_CW_Limit) { return true; } else {
                   throw new ArgumentException("both the CW and CCW limit switch in the Azimuth were true only one limit Swithc can be active at once");
                }
            } );
            var ELTAsk = Task<bool>.Run( ()  => {
                int ELstepSpeed = ConversionHelper.RPMToSPS( 0.2 , MotorConstants.GEARING_RATIO_ELEVATION );
                var timeoutMS = MCUManager.EstimateMovementTime( ELstepSpeed , 50 , ConversionHelper.DegreesToSteps( 30 , MotorConstants.GEARING_RATIO_ELEVATION ) );
                var timeout = new CancellationTokenSource( timeoutMS );
                if(limitSwitchData.Elevation_Lower_Limit && !limitSwitchData.Elevation_Upper_Limit) {
                    MCU.Send_Jog_command( 0 , false , 0.2 , false);
                    while(!timeout.IsCancellationRequested) {
                        Task.Delay( 33 ).Wait();
                        if(!limitSwitchData.Elevation_Lower_Limit) {
                            Cancel_move();
                            return true;
                        }
                    }
                    Cancel_move();
                    return false;
                } else if(!limitSwitchData.Elevation_Lower_Limit && limitSwitchData.Elevation_Upper_Limit) {
                    MCU.Send_Jog_command( 0 , false , 0.2 , true);
                    while(!timeout.IsCancellationRequested) {
                        Task.Delay( 33 ).Wait();
                        if(!limitSwitchData.Elevation_Upper_Limit) {
                            Cancel_move();
                            return true;
                        }
                    }
                    Cancel_move();
                    return false;
                } else if(!limitSwitchData.Elevation_Lower_Limit && !limitSwitchData.Elevation_Upper_Limit) { return true; }else {
                    throw new ArgumentException( "both the CW and CCW limit switch in the Elevation were true only one limit Swithc can be active at once" );
                }
            } );

            AZTAsk.GetAwaiter().GetResult();
            ELTAsk.GetAwaiter().GetResult();
            return true;
        }
        
        protected override bool TestIfComponentIsAlive() {

            bool PLC_alive, MCU_alive;
            try {
                long MCU_last_contact = MCU.getLastContact();
                PLC_alive = (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - PLC_last_contact) < 3000;
                MCU_alive = (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - MCU_last_contact) < 3000;
                if(is_test) {
                    //return true;
                    Console.WriteLine( "{0}   {1} " , (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - PLC_last_contact) , (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - MCU_last_contact) );
                }
                return PLC_alive && MCU_alive;
            } catch { return false; }
        }

        /// <summary>
        /// public version of TestIfComponentIsAlive
        /// </summary>
        /// <returns></returns>
        public bool workaroundAlive() {
            return TestIfComponentIsAlive();
        }

        public override void setTelescopeType(RadioTelescopeTypeEnum type)
        {
            telescopeType = type;

            // Also set the MCU's telescope type
            MCU.setTelescopeType(type);
        }

        /// <summary>
        /// Resets any errors the MCU encounters. This could be for either of the motors.
        /// </summary>
        public override void ResetMCUErrors()
        {
            MCU.ResetMCUErrors();
        }

        /// <summary>
        /// This will check for any errors present in the MCU's registers.
        /// </summary>
        /// <returns>A list of errors present in the MCU's registers</returns>
        public override List<Tuple<MCUOutputRegs, MCUStatusBitsMSW>> CheckMCUErrors()
        {
            return MCU.CheckMCUErrors();
        }
    }
}
