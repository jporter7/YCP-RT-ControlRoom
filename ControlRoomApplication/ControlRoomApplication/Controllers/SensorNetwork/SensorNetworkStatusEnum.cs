using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.SensorNetwork
{
    /// <summary>
    /// This enum will tell you what status the Sensor Network is in. This can change depending on
    /// whether it just started, or if it receives any error codes from the Sensor Network itself.
    /// </summary>
    public enum SensorNetworkStatusEnum
    {
        /// <summary>
        /// Default value if a status has not yet been initialized.
        /// </summary>
        None,

        /// <summary>
        /// This is set when the SensorNetworkServer receives a request to send an initialization to the
        /// Sensor Network.
        /// </summary>
        Initializing,

        /// <summary>
        /// This is set as soon as the SensorNetworkServer receives the first packet of sensor data from
        /// the Sensor Network.
        /// </summary>
        ReceivingData,

        /// <summary>
        /// This is set if it takes too long to receive a data packet.
        /// </summary>
        TimedOutDataRetrieval,

        /// <summary>
        /// This is set if the Sensor Network takes too long to start sending data after the SensorNetworkClient
        /// successfully sends the initialization.
        /// </summary>
        TimedOutInitialization,

        /// <summary>
        /// This is set if the SensorNetworkClient fails to send the initialization to the Sensor Network. This can
        /// happen if the Sensor Network is not on, not plugged in (via eithernet), or the user may have typed the
        /// incorrect Client IP address/Port.
        /// </summary>
        InitializationSendingFailed,

        /// <summary>
        /// This is used if the SensorNetworkServer fails to start for any reason. Reasons could be that the IP address
        /// is already taken, or there is some other error with the network. The server doesn't depend on making a connection
        /// (it waits until something connects to it), so as long as the IP address is valid and reachable, this status should
        /// not occur.
        /// </summary>
        ErrorStartingServer,
        
        /// <summary>
        /// This should never be reached, but if it is, then some troubleshooting needs to happen. The only way this can be set is
        /// if the timer is elapsed and the status is not ReceivingData or Initializing.
        /// </summary>
        UnknownError
    }
}
