using System;
using System.Collections.Generic;
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
            return RadioTelescope.HardwareCommsHandler.TestCommunication();
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
            return RadioTelescope.HardwareCommsHandler.GetCurrentOrientation();
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
        ///         0: Under rotation for azimuth
        ///         1: Over rotation for azimuth
        ///         2: Under rotation for elevation
        ///         3: Over rotation for elevation
        /// </returns>
        public bool[] GetCurrentLimitSwitchStatuses()
        {
            return RadioTelescope.HardwareCommsHandler.GetCurrentLimitSwitchStatuses();
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
            return RadioTelescope.HardwareCommsHandler.GetCurrentSafetyInterlockStatus();
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
            return RadioTelescope.HardwareCommsHandler.CancelCurrentMoveCommand();
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
            return RadioTelescope.HardwareCommsHandler.ShutdownRadioTelescope();
        }

        /// <summary>
        /// Method used to calibrate the Radio Telescope before each observation.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public bool CalibrateRadioTelescope()
        {
            return RadioTelescope.HardwareCommsHandler.CalibrateRadioTelescope();
        }

        /// <summary>
        /// Method used to request to set configuration of elements of the RT.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public bool ConfigureRadioTelescope(double startSpeedAzimuth, double startSpeedElevation, int homeTimeoutAzimuth, int homeTimeoutElevation)
        {
            return RadioTelescope.HardwareCommsHandler.ConfigureRadioTelescope(startSpeedAzimuth, startSpeedElevation, homeTimeoutAzimuth, homeTimeoutElevation);
        }

        /// <summary>
        /// Method used to request to move the Radio Telescope to an objective
        /// azimuth/elevation orientation.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public bool MoveRadioTelescopeToOrientation(Orientation orientation)
        {
            return RadioTelescope.HardwareCommsHandler.MoveRadioTelescopeToOrientation(orientation);
        }

        /// <summary>
        /// Method used to request to move the Radio Telescope to an objective
        /// right ascension/declination coordinate pair.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public bool MoveRadioTelescopeToCoordinate(Coordinate coordinate)
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
            return RadioTelescope.HardwareCommsHandler.StartRadioTelescopeJog(axis, speed, clockwise);
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
            return RadioTelescope.HardwareCommsHandler.ExecuteRadioTelescopeControlledStop();
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
            return RadioTelescope.HardwareCommsHandler.ExecuteRadioTelescopeImmediateStop();
        }

        /// <summary>
        /// Method used to request that one of the Radio Telescope's axes moves to a
        /// position relative to the current one, in either the clockwise or
        /// counter-clockwise direction.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public bool ExecuteRelativeMove(RadioTelescopeAxisEnum axis, double speed, double position)
        {
            return RadioTelescope.HardwareCommsHandler.ExecuteRelativeMove(axis, speed, position);
        }

        /// <summary>
        /// Method used to request that the Radio Telescope's azimuth axis moves to a
        /// position relative to the current one, in either the clockwise or
        /// counter-clockwise direction.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public bool ExecuteRelativeMoveAzimuth(double speed, double position)
        {
            return ExecuteRelativeMove(RadioTelescopeAxisEnum.AZIMUTH, speed, position);
        }

        /// <summary>
        /// Method used to request that the Radio Telescope's elevation axis moves to a
        /// position relative to the current one, in either the clockwise or
        /// counter-clockwise direction.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public bool ExecuteRelativeMoveElevation(double speed, double position)
        {
            return ExecuteRelativeMove(RadioTelescopeAxisEnum.AZIMUTH, speed, position);
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