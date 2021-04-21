using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using System.Threading;
using ControlRoomApplication.Controllers.Sensors;
using ControlRoomApplication.Database;
using ControlRoomApplication.Controllers.Communications;
using ControlRoomApplication.Util;
using System.Timers;
using System.Diagnostics;
using ControlRoomApplication.Controllers.PLCCommunication.PLCDrivers.MCUManager;

namespace ControlRoomApplication.Controllers
{
    public class RadioTelescopeController
    {
        public RadioTelescope RadioTelescope { get; set; }
        public CoordinateCalculationController CoordinateController { get; set; }
        public OverrideSwitchData overrides;

        // Thread that monitors database current temperature
        private Thread SensorMonitoringThread;
        private bool MonitoringSensors;
        private bool AllSensorsSafe;

        private double MaxElTempThreshold;
        private double MaxAzTempThreshold;

        // Previous snow dump azimuth -- we need to keep track of this in order to add 45 degrees each time we dump
        private double previousSnowDumpAzimuth;

        // Snow dump timer
        private static System.Timers.Timer snowDumpTimer;

        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Constructor that takes an AbstractRadioTelescope object and sets the
        /// corresponding field.
        /// </summary>
        /// <param name="radioTelescope"></param>
        public RadioTelescopeController(RadioTelescope radioTelescope)
        {
            RadioTelescope = radioTelescope;
            CoordinateController = new CoordinateCalculationController(radioTelescope.Location);

            overrides = new OverrideSwitchData(radioTelescope);

            SensorMonitoringThread = new Thread(SensorMonitor);
            SensorMonitoringThread.Start();
            MonitoringSensors = true;
            AllSensorsSafe = true;

            MaxAzTempThreshold = DatabaseOperations.GetThresholdForSensor(SensorItemEnum.AZ_MOTOR_TEMP);
            MaxElTempThreshold = DatabaseOperations.GetThresholdForSensor(SensorItemEnum.ELEV_MOTOR_TEMP);

            previousSnowDumpAzimuth = 0;

            snowDumpTimer = new System.Timers.Timer(DatabaseOperations.FetchWeatherThreshold().SnowDumpTime * 1000 * 60);
            snowDumpTimer.Elapsed += AutomaticSnowDumpInterval;
            snowDumpTimer.AutoReset = true;
            snowDumpTimer.Enabled = true;
        }

        /// <summary>
        /// Gets the status of whether this RT is responding.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        /// <returns> Whether or not the RT responded. </returns>
        public bool TestCommunication()
        {
            return RadioTelescope.PLCDriver.Test_Connection();
        }

        /// <summary>
        /// Gets the current orientation of the radiotelescope in azimuth and elevation.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        /// <returns> An orientation object that holds the current azimuth/elevation of the scale model. </returns>
        public Orientation GetCurrentOrientation()
        {
            return RadioTelescope.PLCDriver.GetMotorEncoderPosition();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Orientation GetAbsoluteOrientation()
        {
            return RadioTelescope.SensorNetworkServer.CurrentAbsoluteOrientation;
        }

        /// <summary>
        /// Gets the status of the interlock system associated with this Radio Telescope.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        /// <returns> Returns true if the safety interlock system is still secured, false otherwise. </returns>
        public bool GetCurrentSafetyInterlockStatus()
        {
            return RadioTelescope.PLCDriver.Get_interlock_status();
        }

        /// <summary>
        /// Method used to cancel this Radio Telescope's current attempt to change orientation.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public bool CancelCurrentMoveCommand(MovePriority priority)
        {
            bool result = false;

            if(priority > RadioTelescope.PLCDriver.CurrentMovementPriority)
            {
                result = RadioTelescope.PLCDriver.Cancel_move();
                RadioTelescope.PLCDriver.CurrentMovementPriority = MovePriority.None;
            }

            return result;
        }

        /// <summary>
        /// Method used to shutdown the Radio Telescope in the case of inclement
        /// weather, maintenance, etcetera.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public bool ShutdownRadioTelescope()
        {
            //MoveRadioTelescopeToOrientation(MiscellaneousConstants.Stow);
            snowDumpTimer.Stop();
            snowDumpTimer.Dispose();

            return RadioTelescope.PLCDriver.RequestStopAsyncAcceptingClientsAndJoin();
        }

        /// <summary>
        /// Method used to calibrate the Radio Telescope before each observation.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public bool ThermalCalibrateRadioTelescope(MovePriority priority)
        {
            bool successMove1 = false;
            bool successMove2 = false;
            bool successMove3 = false;

            if (priority > RadioTelescope.PLCDriver.CurrentMovementPriority)
            {
                // We only want to do this if it is safe to do so. Return false if not
                if (!AllSensorsSafe) return false;

                Orientation current = GetCurrentOrientation();
                successMove1 = RadioTelescope.PLCDriver.Move_to_orientation(MiscellaneousConstants.THERMAL_CALIBRATION_ORIENTATION, current);

                // start a timer so we can have a time variable
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                // temporarily set spectracyber mode to continuum
                RadioTelescope.SpectraCyberController.SetSpectraCyberModeType(SpectraCyberModeTypeEnum.CONTINUUM);

                // read data
                SpectraCyberResponse response = RadioTelescope.SpectraCyberController.DoSpectraCyberScan();

                // end the timer
                stopWatch.Stop();
                double time = stopWatch.Elapsed.TotalSeconds;

                RFData rfResponse = RFData.GenerateFrom(response);

                // move back to previous location
                if (successMove1)
                {
                    successMove2 = RadioTelescope.PLCDriver.Move_to_orientation(current, MiscellaneousConstants.THERMAL_CALIBRATION_ORIENTATION);
                }

                // analyze data
                // temperature (Kelvin) = (intensity * time * wein's displacement constant) / (Planck's constant * speed of light)
                double weinConstant = 2.8977729;
                double planckConstant = 6.62607004 * Math.Pow(10, -34);
                double speedConstant = 299792458;
                double temperature = (rfResponse.Intensity * time * weinConstant) / (planckConstant * speedConstant);

                // convert to fahrenheit
                temperature = temperature * (9 / 5) - 459.67;

                // check against weather station reading
                double weatherStationTemp = RadioTelescope.WeatherStation.GetOutsideTemp();

                // Set SpectraCyber mode back to UNKNOWN
                RadioTelescope.SpectraCyberController.SetSpectraCyberModeType(SpectraCyberModeTypeEnum.UNKNOWN);

                // return true if working correctly, false if not
                if ((Math.Abs(weatherStationTemp - temperature) < MiscellaneousConstants.THERMAL_CALIBRATION_OFFSET) && successMove2 && successMove1)
                {
                    successMove3 = RadioTelescope.PLCDriver.Move_to_orientation(MiscellaneousConstants.Stow, current);
                }

                if (RadioTelescope.PLCDriver.CurrentMovementPriority != MovePriority.Critical) RadioTelescope.PLCDriver.CurrentMovementPriority = MovePriority.None;
            }

            return successMove1 && successMove2 && successMove3;
        }

        /// <summary>
        /// Method used to request to set configuration of elements of the RT.
        /// takes the starting speed of the motor in RPM (speed of tellescope after gearing)
        /// </summary>
        /// <param name="startSpeedAzimuth">RPM</param>
        /// <param name="startSpeedElevation">RPM</param>
        /// <param name="homeTimeoutAzimuth">SEC</param>
        /// <param name="homeTimeoutElevation">SEC</param>
        /// <returns></returns>
        public bool ConfigureRadioTelescope(double startSpeedAzimuth, double startSpeedElevation, int homeTimeoutAzimuth, int homeTimeoutElevation)
        {
            return RadioTelescope.PLCDriver.Configure_MCU(startSpeedAzimuth, startSpeedElevation, homeTimeoutAzimuth, homeTimeoutElevation); // NO MOVE
        }

        /// <summary>
        /// Method used to request to move the Radio Telescope to an objective
        /// azimuth/elevation orientation.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public bool MoveRadioTelescopeToOrientation(Orientation orientation, MovePriority priority)//TODO: once its intagrated use the microcontrole to get the current opsition 
        {
            bool success = false;

            if (priority > RadioTelescope.PLCDriver.CurrentMovementPriority)
            {
                if (!AllSensorsSafe) return false;

                RadioTelescope.PLCDriver.CurrentMovementPriority = priority;
                success = RadioTelescope.PLCDriver.Move_to_orientation(orientation, RadioTelescope.PLCDriver.GetMotorEncoderPosition());
                if(RadioTelescope.PLCDriver.CurrentMovementPriority != MovePriority.Critical) RadioTelescope.PLCDriver.CurrentMovementPriority = MovePriority.None;
            }

            return success;
        }

        /// <summary>
        /// Method used to request to move the Radio Telescope to an objective
        /// right ascension/declination coordinate pair.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public bool MoveRadioTelescopeToCoordinate(Coordinate coordinate, MovePriority priority)
        {
            bool success = false;

            if (priority > RadioTelescope.PLCDriver.CurrentMovementPriority)
            {
                if (!AllSensorsSafe) return false;

                RadioTelescope.PLCDriver.CurrentMovementPriority = priority;
                success = RadioTelescope.PLCDriver.Move_to_orientation(CoordinateController.CoordinateToOrientation(coordinate, DateTime.UtcNow), RadioTelescope.PLCDriver.GetMotorEncoderPosition()); // MOVE
                if (RadioTelescope.PLCDriver.CurrentMovementPriority != MovePriority.Critical) RadioTelescope.PLCDriver.CurrentMovementPriority = MovePriority.None;
            }

            return success;
        }

        /// <summary>
        /// This is used to home the telescope. Immediately after homing, the telescope will move to "Stow" position.
        /// This will also zero out the absolute encoders and account for the true north offset.
        /// </summary>
        /// <param name="priority">The priority of this movement.</param>
        /// <returns>True if homing was successful; false if homing failed.</returns>
        public bool HomeTelescope(MovePriority priority)
        {
            bool success = false;

            if(priority > RadioTelescope.PLCDriver.CurrentMovementPriority)
            {
                if (!AllSensorsSafe) return false;

                RadioTelescope.PLCDriver.CurrentMovementPriority = priority;
                success = RadioTelescope.PLCDriver.HomeTelescope();

                // Zero out absolute encoders
                RadioTelescope.SensorNetworkServer.AbsoluteOrientationOffset = (Orientation)RadioTelescope.SensorNetworkServer.CurrentAbsoluteOrientation.Clone();

                if (RadioTelescope.PLCDriver.CurrentMovementPriority != MovePriority.Critical) RadioTelescope.PLCDriver.CurrentMovementPriority = MovePriority.None;
            }

            return success;
        }

        /// <summary>
        /// A demonstration script that moves the elevation motor to its maximum and minimum.
        /// </summary>
        /// <param name="priority">Movement priority.</param>
        /// <returns></returns>
        public bool FullElevationMove(MovePriority priority)
        {
            bool successMove1 = false;
            bool successMove2 = false;
            bool successReturnToOriginalPos = false;

            if (priority > RadioTelescope.PLCDriver.CurrentMovementPriority)
            {
                Orientation origOrientation = GetCurrentOrientation();

                Orientation move1 = new Orientation(origOrientation.Azimuth, 0);
                Orientation move2 = new Orientation(origOrientation.Azimuth, 90);

                // Move to a low elevation point
                successMove1 = RadioTelescope.PLCDriver.Move_to_orientation(move1, origOrientation);

                if(successMove1)
                {
                    // Move to a high elevation point
                    successMove2 = RadioTelescope.PLCDriver.Move_to_orientation(move2, move1);

                    if(successMove2)
                    {
                        // Move back to the original orientation
                        successReturnToOriginalPos = RadioTelescope.PLCDriver.Move_to_orientation(origOrientation, move2);
                    }
                }
            }

            return successMove1 && successMove2 && successReturnToOriginalPos;
        }

        /// <summary>
        /// Method used to request to start jogging the Radio Telescope's azimuth
        /// at a speed (in RPM), in either the clockwise or counter-clockwise direction.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public bool StartRadioTelescopeAzimuthJog(double speed, RadioTelescopeDirectionEnum direction, MovePriority priority)
        {
            bool success = false;

            if(priority > RadioTelescope.PLCDriver.CurrentMovementPriority)
            {
                if (!AllSensorsSafe) return false;

                RadioTelescope.PLCDriver.CurrentMovementPriority = priority;

                // Elevation direction is a "don't care" because its speed is 0, so it won't move anyway
                success = RadioTelescope.PLCDriver.StartBothAxesJog(speed, direction, 0, direction);
            }

            return success;
        }

        /// <summary>
        /// Method used to request to start jogging the Radio Telescope's elevation
        /// at a speed (in RPM), in either the clockwise or counter-clockwise direction.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public bool StartRadioTelescopeElevationJog(double speed, RadioTelescopeDirectionEnum direction, MovePriority priority)
        {
            bool success = false;

            if (priority > RadioTelescope.PLCDriver.CurrentMovementPriority)
            {
                if (!AllSensorsSafe) return false;

                RadioTelescope.PLCDriver.CurrentMovementPriority = priority;

                // Azimuth direction is a "don't care" because its speed is 0, so it won't move anyway
                success = RadioTelescope.PLCDriver.StartBothAxesJog(0, direction, speed, direction);
            }

            return success;
        }


        /// <summary>
        /// send a clear move to the MCU to stop a jog
        /// </summary>
        public bool ExecuteRadioTelescopeStopJog(MovePriority priority)
        {
            bool success = false;

            if (priority > RadioTelescope.PLCDriver.CurrentMovementPriority)
            {
                success = RadioTelescope.PLCDriver.Stop_Jog();
                RadioTelescope.PLCDriver.CurrentMovementPriority = MovePriority.None;
            }

            return success;
        }

        /// <summary>
        /// Method used to request that all of the Radio Telescope's movement comes
        /// to a controlled stop. this will not work for jog moves use 
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public bool ExecuteRadioTelescopeControlledStop(MovePriority priority)
        {
            bool success = false;

            if (priority > RadioTelescope.PLCDriver.CurrentMovementPriority)
            {
                success = RadioTelescope.PLCDriver.ControlledStop();
                RadioTelescope.PLCDriver.CurrentMovementPriority = MovePriority.None;
            }

            return success;
        }

        /// <summary>
        /// Method used to request that all of the Radio Telescope's movement comes
        /// to an immediate stop.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public bool ExecuteRadioTelescopeImmediateStop(MovePriority priority)
        {
            bool success = false;

            if (priority > RadioTelescope.PLCDriver.CurrentMovementPriority)
            {
                success = RadioTelescope.PLCDriver.ImmediateStop();
                RadioTelescope.PLCDriver.CurrentMovementPriority = MovePriority.None;
            }

            return success;
        }


        /// <summary>
        /// return true if the RT has finished the previous move comand
        /// </summary>
        public bool finished_exicuting_move( RadioTelescopeAxisEnum axis )//[7]
        {
             
            var Taz = RadioTelescope.PLCDriver.GET_MCU_Status( RadioTelescopeAxisEnum.AZIMUTH );
            var Tel = RadioTelescope.PLCDriver.GET_MCU_Status( RadioTelescopeAxisEnum.ELEVATION );
            
            bool azFin = Taz[(int)MCUConstants.MCUStatusBitsMSW.Move_Complete];
            bool elFin = Tel[(int)MCUConstants.MCUStatusBitsMSW.Move_Complete];
            if(axis == RadioTelescopeAxisEnum.BOTH) {
                return elFin && azFin;
            } else if(axis == RadioTelescopeAxisEnum.AZIMUTH) {
                return azFin;
            } else if(axis == RadioTelescopeAxisEnum.ELEVATION) {
                return elFin;
            }
            return false;
        }

        // Checks the motor temperatures against acceptable ranges every second
        private void SensorMonitor()
        {
            // Getting initial current temperatures
            Temperature currAzTemp = RadioTelescope.SensorNetworkServer.CurrentAzimuthMotorTemp[RadioTelescope.SensorNetworkServer.CurrentAzimuthMotorTemp.Length - 1];
            Temperature currElTemp = RadioTelescope.SensorNetworkServer.CurrentElevationMotorTemp[RadioTelescope.SensorNetworkServer.CurrentElevationMotorTemp.Length - 1];
            bool elTempSafe = checkTemp(currElTemp, true);
            bool azTempSafe = checkTemp(currAzTemp, true);

            // Sensor overrides must be taken into account
            bool currentAZOveride = overrides.overrideAzimuthMotTemp;
            bool currentELOveride = overrides.overrideElevatMotTemp;

            // Loop through every one second to get new sensor data
            while (MonitoringSensors)
            {
                azTempSafe = checkTemp(RadioTelescope.SensorNetworkServer.CurrentAzimuthMotorTemp[RadioTelescope.SensorNetworkServer.CurrentAzimuthMotorTemp.Length - 1], azTempSafe);
                elTempSafe = checkTemp(RadioTelescope.SensorNetworkServer.CurrentElevationMotorTemp[RadioTelescope.SensorNetworkServer.CurrentElevationMotorTemp.Length - 1], elTempSafe);
                
                // Determines if the telescope is in a safe state
                if (azTempSafe && elTempSafe) AllSensorsSafe = true;
                else AllSensorsSafe = false;
                
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        ///  Checks that the motor temperatures are within acceptable ranges. If the temperature exceeds
        ///  the corresponding value in SimulationConstants.cs, it will return false, otherwise
        ///  it will return true if everything is good.
        ///  Tl;dr:
        ///  False - bad
        ///  True - good
        /// </summary>
        /// <returns>override bool</returns>
        public bool checkTemp(Temperature t, bool lastIsSafe)
        {
            // get maximum temperature threshold
            double max;

            // Determine whether azimuth or elevation
            String s;
            bool isOverridden;
            if (t.location_ID == (int)SensorLocationEnum.AZ_MOTOR)
            {
                s = "Azimuth";
                isOverridden = overrides.overrideAzimuthMotTemp;
                max = MaxAzTempThreshold;
            }
            else
            {
                s = "Elevation";
                isOverridden = overrides.overrideElevatMotTemp;
                max = MaxElTempThreshold;
            }

            // Check temperatures
            if (t.temp < SimulationConstants.MIN_MOTOR_TEMP)
            {
                if (lastIsSafe)
                {
                    logger.Info(Utilities.GetTimeStamp() + ": " + s + " motor temperature BELOW stable temperature by " + Math.Truncate(SimulationConstants.STABLE_MOTOR_TEMP - t.temp) + " degrees Fahrenheit.");

                    pushNotification.sendToAllAdmins("MOTOR TEMPERATURE", s + " motor temperature BELOW stable temperature by " + Math.Truncate(SimulationConstants.STABLE_MOTOR_TEMP - t.temp) + " degrees Fahrenheit.");
                    EmailNotifications.sendToAllAdmins("MOTOR TEMPERATURE", s + " motor temperature BELOW stable temperature by " + Math.Truncate(SimulationConstants.STABLE_MOTOR_TEMP - t.temp) + " degrees Fahrenheit.");
                }
                    
                // Only overrides if switch is true
                if (!isOverridden) return false;
                else return true;
            }
            else if (t.temp > SimulationConstants.OVERHEAT_MOTOR_TEMP)
            {
                if (lastIsSafe)
                {
                    logger.Info(Utilities.GetTimeStamp() + ": " + s + " motor temperature OVERHEATING by " + Math.Truncate(t.temp - max) + " degrees Fahrenheit.");

                    pushNotification.sendToAllAdmins("MOTOR TEMPERATURE", s + " motor temperature OVERHEATING by " + Math.Truncate(t.temp - max) + " degrees Fahrenheit.");
                    EmailNotifications.sendToAllAdmins("MOTOR TEMPERATURE", s + " motor temperature OVERHEATING by " + Math.Truncate(t.temp - max) + " degrees Fahrenheit.");
                }

                // Only overrides if switch is true
                if (!isOverridden) return false;
                else return true;
            }
            else if (t.temp <= SimulationConstants.MAX_MOTOR_TEMP && t.temp >= SimulationConstants.MIN_MOTOR_TEMP && !lastIsSafe) {
                logger.Info(Utilities.GetTimeStamp() + ": " + s + " motor temperature stable.");

                pushNotification.sendToAllAdmins("MOTOR TEMPERATURE", s + " motor temperature stable.");
                EmailNotifications.sendToAllAdmins("MOTOR TEMPERATURE", s + " motor temperature stable.");
            }

            return true;
        }

        /// <summary>
        /// This will set the overrides based on input. Takes in the sensor that it will be changing,
        /// and then the status, true or false.
        /// true = overriding
        /// false = enabled
        /// </summary>
        /// <param name="sensor"></param>
        /// <param name="set"></param>
        public void setOverride(String sensor, bool set)
        {
            if      (sensor.Equals("azimuth motor temperature"))    overrides.setAzimuthMotTemp(set);
            else if (sensor.Equals("elevation motor temperature"))  overrides.setElevationMotTemp(set);
            else if (sensor.Equals("main gate"))                    overrides.setGatesOverride(set);
            else if (sensor.Equals("elevation proximity (1)"))      overrides.setElProx0Override(set);
            else if (sensor.Equals("elevation proximity (2)"))      overrides.setElProx90Override(set);
            else if (sensor.Equals("azimuth absolute encoder")) overrides.setAzimuthAbsEncoder(set);
            else if (sensor.Equals("elevation absolute encoder")) overrides.setElevationAbsEncoder(set);
            else if (sensor.Equals("azimuth motor accelerometer")) overrides.setAzimuthAccelerometer(set);
            else if (sensor.Equals("elevation motor accelerometer")) overrides.setElevationAccelerometer(set);
            else if (sensor.Equals("counterbalance accelerometer")) overrides.setCounterbalanceAccelerometer(set);


            if (set)
            {
                logger.Info(Utilities.GetTimeStamp() + ": Overriding " + sensor + " sensor.");

                pushNotification.sendToAllAdmins("SENSOR OVERRIDES", "Overriding " + sensor + " sensor.");
                EmailNotifications.sendToAllAdmins("SENSOR OVERRIDES", "Overriding " + sensor + " sensor.");
            }
            else
            {
                logger.Info(Utilities.GetTimeStamp() + ": Enabled " + sensor + " sensor.");

                pushNotification.sendToAllAdmins("SENSOR OVERRIDES", "Enabled " + sensor + " sensor.");
                EmailNotifications.sendToAllAdmins("SENSOR OVERRIDES", "Enabled " + sensor + " sensor.");
            }
        }

        /// <summary>
        /// This is a script that is called when we want to dump snow out of the dish
        /// </summary>
        public bool SnowDump(MovePriority priority)
        {
            bool successMove1 = false;
            bool successMove2 = false;
            bool successMove3 = false;

            if (priority > RadioTelescope.PLCDriver.CurrentMovementPriority)
            {
                if (!AllSensorsSafe) return false;

                priority = RadioTelescope.PLCDriver.CurrentMovementPriority;

                // insert snow dump movements here
                // default is azimuth of 0 and elevation of 0
                Orientation dump = new Orientation(previousSnowDumpAzimuth += 45, -5);
                Orientation current = GetCurrentOrientation();

                Orientation dumpAzimuth = new Orientation(dump.Azimuth, current.Elevation);
                Orientation dumpElevation = new Orientation(dump.Azimuth, dump.Elevation);

                // move to dump snow
                successMove1 = RadioTelescope.PLCDriver.Move_to_orientation(dumpAzimuth, current);
                successMove2 = RadioTelescope.PLCDriver.Move_to_orientation(dumpElevation, dumpAzimuth);

                // move back to initial orientation
                successMove3 = RadioTelescope.PLCDriver.Move_to_orientation(current, dumpElevation);

                if (RadioTelescope.PLCDriver.CurrentMovementPriority != MovePriority.Critical) RadioTelescope.PLCDriver.CurrentMovementPriority = MovePriority.None;
            }

            return successMove1 && successMove2 && successMove3;
        }

        private void AutomaticSnowDumpInterval(Object source, ElapsedEventArgs e)
        {
            double DELTA = 0.01;
            Orientation currentOrientation = GetCurrentOrientation();
            
            // Check if we need to dump the snow off of the telescope
            if (RadioTelescope.WeatherStation.GetOutsideTemp() <= 30.00 && RadioTelescope.WeatherStation.GetTotalRain() > 0.00)
            {
                // We want to check stow position precision with a 0.01 degree margin of error
                if(Math.Abs(currentOrientation.Azimuth - MiscellaneousConstants.Stow.Azimuth) <= DELTA && Math.Abs(currentOrientation.Elevation - MiscellaneousConstants.Stow.Elevation) <= DELTA)
                {
                    Console.WriteLine("Time threshold reached. Running snow dump...");

                    SnowDump(MovePriority.Appointment);

                    Console.WriteLine("Snow dump completed");
                }
                
            }
        }

    }
}