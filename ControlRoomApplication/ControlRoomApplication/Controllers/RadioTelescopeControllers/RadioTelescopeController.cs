using System;
using System.Collections.Generic;
using ControlRoomApplication.Controllers.AASharpControllers;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Entities.RadioTelescope;

namespace ControlRoomApplication.Controllers.RadioTelescopeControllers
{
    public class RadioTelescopeController
    {
        public AbstractRadioTelescope RadioTelescope { get; set; }
        public CoordinateCalculationController CoordinateController { get; set; }

        /// <summary>
        /// Constructor that takes an AbstractRadioTelescope object and sets the
        /// corresponding field.
        /// </summary>
        /// <param name="radioTelescope"></param>
        public RadioTelescopeController(AbstractRadioTelescope radioTelescope)
        {
            RadioTelescope = radioTelescope;
            CoordinateController = new CoordinateCalculationController(radioTelescope.Location);
        }

        /// <summary>
        /// A simple getter for the underlying abstract RT model.
        /// </summary>
        /// <returns> The abstarct RT that this instance is controlling </returns>
        public AbstractRadioTelescope GetAbstractRadioTelescopeModel()
        {
            return RadioTelescope;
        }

        /// <summary>
        /// Gets the status of whether this RT is responding.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        /// <returns> Whether or not the RT responded. </returns>
        public bool TestRadioTelescopeCommunication()
        {
            byte[] ByteResponse = RadioTelescope.PLCClient.RequestMessageSend(PLCCommandAndQueryTypeEnum.TEST_CONNECTION);
            return (ByteResponse[2] == 0x1) && (ByteResponse[3] == 0x1);
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

            if (ByteResponse[2] != 0x1)
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
        /// <returns> An orientation object that holds the current azimuth/elevation of the scale model. </returns>
        public Orientation GetCurrentLimitSwitchStatuses()
        {
            return null;
        }

        /// <summary>
        /// Method used to shutdown the Radio Telescope in the case of inclement
        /// weather, maintenance, etcetera.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public void ShutdownRadioTelescope()
        {
            RadioTelescope.PLCClient.RequestMessageSend(PLCCommandAndQueryTypeEnum.SHUTDOWN);
        }

        /// <summary>
        /// Method used to request to move the Radio Telescope to an objective
        /// azimuth/elevation orientation.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public void MoveRadioTelescope(Orientation orientation)
        {
            RadioTelescope.PLCClient.RequestMessageSend(PLCCommandAndQueryTypeEnum.SET_OBJECTIVE_AZEL_POSITION, orientation);
        }

        /// <summary>
        /// Method used to request to move the Radio Telescope to an objective
        /// right ascension/declination coordinate pair.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public void MoveRadioTelescope(Coordinate coordinate)
        {
            MoveRadioTelescope(CoordinateController.CoordinateToOrientation(coordinate, DateTime.Now));
        }

        /// <summary>
        /// Method used to calibrate the Radio Telescope before each observation.
        /// 
        /// The implementation of this functionality is on a "per-RT" basis, as
        /// in this may or may not work, it depends on if the derived
        /// AbstractRadioTelescope class has implemented it.
        /// </summary>
        public void CalibrateRadioTelescope()
        {
            RadioTelescope.PLCClient.RequestMessageSend(PLCCommandAndQueryTypeEnum.CALIBRATE);
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