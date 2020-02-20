using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;

namespace ControlRoomApplication.Controllers
{
    public class RadioTelescopeController
    {
        public RadioTelescope RadioTelescope { get; set; }
        public CoordinateCalculationController CoordinateController { get; set; }

        /// <summary>
        /// Constructor that takes an AbstractRadioTelescope object and sets the
        /// corresponding field.
        /// </summary>
        /// <param name="radioTelescope"></param>
        public RadioTelescopeController(RadioTelescope radioTelescope)
        {
            RadioTelescope = radioTelescope;
            CoordinateController = new CoordinateCalculationController(radioTelescope.Location);
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

            return RadioTelescope.PLCDriver.read_Position();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Orientation GetAbsoluteOrientation()
        {
            return RadioTelescope.Encoders.GetCurentOrientation();
        }


        /// <summary>
        /// Gets the current status of the four limit switches.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        /// <returns>
        ///     An array of four booleans, where "true" means that the limit switch was triggered, and "false" means otherwise.
        ///     The order of the limit switches are as follows:
        ///         0: Under limit    azimuth
        ///         1: Under warning  azimuth
        ///         2: Over  warning  azimuth
        ///         3: Over  limit    azimuth
        ///         4: Under limit    elevation
        ///         5: Under warning  elevation
        ///         6: Over  warning  elevation
        ///         7: Over  limit    elevation
        /// </returns>
        public bool[] GetCurrentLimitSwitchStatuses()
        {
            //throw new NotImplementedException();
            return RadioTelescope.PLCDriver.Get_Limit_switches();
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
        public bool CancelCurrentMoveCommand()
        {
            return RadioTelescope.PLCDriver.Cancel_move();
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
            return RadioTelescope.PLCDriver.Shutdown_PLC_MCU();
        }

        /// <summary>
        /// Method used to calibrate the Radio Telescope before each observation.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public Task<bool> ThermalCalibrateRadioTelescope()
        {
            return RadioTelescope.PLCDriver.Thermal_Calibrate();
        }

        /// <summary>
        /// Method used to request to set configuration of elements of the RT.
        /// 
        /// takes the starting speed of the motor in degrees per second (speed of tellescope after gearing)
        /// and home timeout which is currently unused
        /// </summary>
        public bool ConfigureRadioTelescope(double startSpeedAzimuth, double startSpeedElevation, int homeTimeoutAzimuth, int homeTimeoutElevation)
        {
            return RadioTelescope.PLCDriver.Configure_MCU(startSpeedAzimuth, startSpeedElevation, homeTimeoutAzimuth, homeTimeoutElevation);
        }

        /// <summary>
        /// Method used to request to move the Radio Telescope to an objective
        /// azimuth/elevation orientation.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public Task<bool> MoveRadioTelescopeToOrientation(Orientation orientation)
        {
            return RadioTelescope.PLCDriver.Move_to_orientation(orientation, RadioTelescope.PLCDriver.read_Position());
        }

        /// <summary>
        /// Method used to request to move the Radio Telescope to an objective
        /// right ascension/declination coordinate pair.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public Task<bool> MoveRadioTelescopeToCoordinate(Coordinate coordinate)
        {
            return MoveRadioTelescopeToOrientation(CoordinateController.CoordinateToOrientation(coordinate, DateTime.UtcNow));
        }

        /// <summary>
        /// Method used to request to start jogging one of the Radio Telescope's axes
        /// at a speed, in either the clockwise or counter-clockwise direction.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public bool StartRadioTelescopeJog(RadioTelescopeAxisEnum axis, double speed, bool clockwise)
        {
            return RadioTelescope.PLCDriver.Start_jog(axis, speed , clockwise);
        }

        /// <summary>
        /// Method used to request to start jogging the Radio Telescope's azimuth
        /// at a speed, in either the clockwise or counter-clockwise direction.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public bool StartRadioTelescopeAzimuthJog(double speed, bool clockwise)
        {
            return StartRadioTelescopeJog(RadioTelescopeAxisEnum.AZIMUTH, speed, clockwise);
        }

        /// <summary>
        /// Method used to request to start jogging the Radio Telescope's elevation
        /// at a speed, in either the clockwise or counter-clockwise direction.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public bool StartRadioTelescopeElevationJog(double speed, bool clockwise)
        {
            return StartRadioTelescopeJog(RadioTelescopeAxisEnum.ELEVATION, speed, clockwise);
        }

        /// <summary>
        /// Method used to request that all of the Radio Telescope's movement comes
        /// to a controlled stop.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public bool ExecuteRadioTelescopeControlledStop()
        {
            return RadioTelescope.PLCDriver.Controled_stop();
        }

        /// <summary>
        /// Method used to request that all of the Radio Telescope's movement comes
        /// to an immediate stop.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public bool ExecuteRadioTelescopeImmediateStop()
        {
            return RadioTelescope.PLCDriver.Immediade_stop();
        }


        /// <summary>
        /// return true if the RT has finished the previous move comand
        /// </summary>
        public bool finished_exicuting_move( RadioTelescopeAxisEnum axis )//[7]
        {
             
            var Taz = RadioTelescope.PLCDriver.GET_MCU_Status( RadioTelescopeAxisEnum.AZIMUTH );  //Task.Run( async () => { await  } );
            var Tel = RadioTelescope.PLCDriver.GET_MCU_Status( RadioTelescopeAxisEnum.ELEVATION );

            Taz.Wait();
            bool azFin = Taz.Result[(int)MCUConstants.MCUStutusBitsMSW.Move_Complete];
            bool elFin = Tel.GetAwaiter().GetResult()[(int)MCUConstants.MCUStutusBitsMSW.Move_Complete];
            if(axis == RadioTelescopeAxisEnum.BOTH) {
                return elFin && azFin;
            } else if(axis == RadioTelescopeAxisEnum.AZIMUTH) {
                return azFin;
            } else if(axis == RadioTelescopeAxisEnum.ELEVATION) {
                return elFin;
            }
            return false;
        }


        private static bool ResponseMetBasicExpectations(byte[] ResponseBytes, int ExpectedSize)
        {
            return ((ResponseBytes[0] + (ResponseBytes[1] * 256)) == ExpectedSize) && (ResponseBytes[2] == 0x1);
            //TODO: throws object is not instance of object when the  PLCClientCommunicationHandler.ReadResponse() retuns null usually due to time out

         }

        private static bool MinorResponseIsValid(byte[] MinorResponseBytes)
        {
            
            System.Diagnostics.Debug.WriteLine(MinorResponseBytes);
            return ResponseMetBasicExpectations(MinorResponseBytes, 0x3);
        }

        private static RFData GenerateRFData(SpectraCyberResponse spectraCyberResponse)
        {
            RFData rfData = new RFData();
            rfData.TimeCaptured = spectraCyberResponse.DateTimeCaptured;
            rfData.Intensity = spectraCyberResponse.DecimalData;
            return rfData;
        }

        private static List<RFData> GenerateRFDataList(List<SpectraCyberResponse> spectraCyberResponses)
        {
            List<RFData> rfDataList = new List<RFData>();
            foreach (SpectraCyberResponse response in spectraCyberResponses)
            {
                rfDataList.Add(GenerateRFData(response));
            }

            return rfDataList;
        }
    }
}