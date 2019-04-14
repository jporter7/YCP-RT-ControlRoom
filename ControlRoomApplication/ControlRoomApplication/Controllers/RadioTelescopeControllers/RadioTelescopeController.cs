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
            byte[] ByteResponse = RadioTelescope.PLCClient.RequestMessageSend(PLCCommandAndQueryTypeEnum.TEST_CONNECTION);
            return ResponseMetBasicExpectations(ByteResponse, 0x13) && (ByteResponse[3] == 0x1);
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
            byte[] ByteResponse = RadioTelescope.PLCClient.RequestMessageSend(PLCCommandAndQueryTypeEnum.GET_CURRENT_AZEL_POSITIONS);

            if (!ResponseMetBasicExpectations(ByteResponse, 0x13))
            {
                return null;
            }

            return new Orientation(BitConverter.ToDouble(ByteResponse, 3), BitConverter.ToDouble(ByteResponse, 11));
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
            byte[] ByteResponse = RadioTelescope.PLCClient.RequestMessageSend(PLCCommandAndQueryTypeEnum.GET_CURRENT_LIMIT_SWITCH_STATUSES);

            if (!ResponseMetBasicExpectations(ByteResponse, 0x13))
            {
                return null;
            }

            bool[] Statuses = new bool[4];

            byte DataByte = ByteResponse[3];
            for (int i = 0; i < 4; i++)
            {
                switch (PLCLimitSwitchStatusConversionHelper.GetFromByte((byte)((DataByte >> (2 * (3 - i))) & 0x3)))
                {
                    case PLCLimitSwitchStatusEnum.WITHIN_WARNING_LIMITS:
                        {
                            Statuses[i] = true;
                            break;
                        }

                    case PLCLimitSwitchStatusEnum.WITHIN_SAFE_LIMITS:
                        {
                            Statuses[i] = false;
                            break;
                        }

                    default:
                        {
                            throw new NotImplementedException("Unrecognized/Invalid response for byte-casted limit switch status.");
                        }
                }
            }

            return Statuses;
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
            byte[] ByteResponse = RadioTelescope.PLCClient.RequestMessageSend(PLCCommandAndQueryTypeEnum.GET_CURRENT_SAFETY_INTERLOCK_STATUS);
            return ResponseMetBasicExpectations(ByteResponse, 0x13) && (ByteResponse[3] == 0x1);
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
            return MinorResponseIsValid(RadioTelescope.PLCClient.RequestMessageSend(PLCCommandAndQueryTypeEnum.CANCEL_ACTIVE_OBJECTIVE_AZEL_POSITION));
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
            return MinorResponseIsValid(RadioTelescope.PLCClient.RequestMessageSend(PLCCommandAndQueryTypeEnum.SHUTDOWN));
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
            return MinorResponseIsValid(RadioTelescope.PLCClient.RequestMessageSend(PLCCommandAndQueryTypeEnum.CALIBRATE));
        }

        /// <summary>
        /// Method used to request to set configuration of elements of the RT.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public bool ConfigureRadioTelescope(int startSpeedAzimuth, int startSpeedElevation, int homeTimeoutAzimuth, int homeTimeoutElevation)
        {
            if ((startSpeedAzimuth < 1) || (startSpeedElevation < 1) || (homeTimeoutAzimuth < 0) || (homeTimeoutElevation < 0)
                || (startSpeedAzimuth > 1000000) || (startSpeedElevation > 1000000) || (homeTimeoutAzimuth > 300) || (homeTimeoutElevation > 300))
            {
                return false;
            }

            return MinorResponseIsValid(RadioTelescope.PLCClient.RequestMessageSend(PLCCommandAndQueryTypeEnum.SET_CONFIGURATION, startSpeedAzimuth, startSpeedElevation, homeTimeoutAzimuth, homeTimeoutElevation));
        }

        /// <summary>
        /// Method used to request to move the Radio Telescope to an objective
        /// azimuth/elevation orientation.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public bool MoveRadioTelescope(Orientation orientation)
        {
            return MinorResponseIsValid(RadioTelescope.PLCClient.RequestMessageSend(PLCCommandAndQueryTypeEnum.SET_OBJECTIVE_AZEL_POSITION, orientation));
        }

        /// <summary>
        /// Method used to request to move the Radio Telescope to an objective
        /// right ascension/declination coordinate pair.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public bool MoveRadioTelescope(Coordinate coordinate)
        {
            return MoveRadioTelescope(CoordinateController.CoordinateToOrientation(coordinate, DateTime.Now));
        }

        private static bool ResponseMetBasicExpectations(byte[] ResponseBytes, int ExpectedSize)
        {
            return ((ResponseBytes[0] + (ResponseBytes[1] * 256)) == ExpectedSize) && (ResponseBytes[2] == 0x1);
        }

        private static bool MinorResponseIsValid(byte[] MinorResponseBytes)
        {
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