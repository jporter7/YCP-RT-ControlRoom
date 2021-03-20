using ControlRoomApplication.Controllers.SensorNetwork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Entities
{

    /// <summary>
    /// This is the conviguration that the SensorNetworkServer and Client use.
    /// This contains values that we want to be retained when the control
    /// room is closed and then reopened, such as initialization information.
    /// </summary>
    [Table("sensor_network_config")]
    public class SensorNetworkConfig
    {
        /// <summary>
        /// Constructor to initialize a new SensorNetworkConfig for a RadioTelescope with default values.
        /// </summary>
        /// <param name="telescopeId">The RadioTelescope.Id that this configuration corresponds to.</param>
        public SensorNetworkConfig(int telescopeId)
        {
            TelescopeId = telescopeId;
            TimeoutDataRetrieval = SensorNetworkConstants.DefaultDataRetrievalTimeout;
            TimeoutInitialization = SensorNetworkConstants.DefaultInitializationTimeout;

            // Initialize all sensors to enabled by default
            ElevationTemp1Init = true;
            ElevationTemp2Init = true;
            AzimuthTemp1Init = true;
            AzimuthTemp2Init = true;
            AzimuthAccelerometerInit = true;
            ElevationAccelerometerInit = true;
            CounterbalanceAccelerometerInit = true;
            AzimuthEncoderInit = true;
            ElevationEncoderInit = true;
        }


        /// <summary>
        /// The Radio Telescope ID that this configuration is for. This should not be
        /// a foreign key, because a SensorNetworkConfig does not have a whole RadioTelescope object.
        /// </summary>
        [Column("telescope_id")]
        public int TelescopeId { get; set; }

        /// <summary>
        /// This will tell the Sensor Network whether or not to initialize the primary elevation motor temp sensor.
        /// We will not receive data for this sensor if it is not initialized.
        /// true = initialize;
        /// false = do not initialize
        /// </summary>
        [Column("elevation_temp_1_init")]
        public bool ElevationTemp1Init { get; set; }

        /// <summary>
        /// This will tell the Sensor Network whether or not to initialize the second (redundant) elevation motor temp sensor.
        /// We will not receive data for this sensor if it is not initialized.
        /// true = initialize;
        /// false = do not initialize
        /// </summary>
        [Column("elevation_temp_2_init")]
        public bool ElevationTemp2Init { get; set; }

        /// <summary>
        /// This will tell the Sensor Network whether or not to initialize the primary azimuth motor temp sensor.
        /// We will not receive data for this sensor if it is not initialized.
        /// true = initialize;
        /// false = do not initialize
        /// </summary>
        [Column("azimuth_temp_1_init")]
        public bool AzimuthTemp1Init { get; set; }

        /// <summary>
        /// This will tell the Sensor Network whether or not to initialize the second (redundant) azimuth motor temp sensor.
        /// We will not receive data for this sensor if it is not initialized.
        /// true = initialize;
        /// false = do not initialize
        /// </summary>
        [Column("azimuth_temp_2_init")]
        public bool AzimuthTemp2Init { get; set; }

        /// <summary>
        /// This will tell the Sensor Network whether or not to initialize the azimuth accelerometer.
        /// We will not receive data for this sensor if it is not initialized.
        /// true = initialize;
        /// false = do not initialize
        /// </summary>
        [Column("azimuth_accelerometer_init")]
        public bool AzimuthAccelerometerInit { get; set; }

        /// <summary>
        /// This will tell the Sensor Network whether or not to initialize the elevation accelerometer.
        /// We will not receive data for this sensor if it is not initialized.
        /// true = initialize;
        /// false = do not initialize
        /// </summary>
        [Column("elevation_accelerometer_init")]
        public bool ElevationAccelerometerInit { get; set; }

        /// <summary>
        /// This will tell the Sensor Network whether or not to initialize the counterbalance accelerometer.
        /// We will not receive data for this sensor if it is not initialized.
        /// true = initialize;
        /// false = do not initialize
        /// </summary>
        [Column("counterbalance_accelerometer_init")]
        public bool CounterbalanceAccelerometerInit { get; set; }

        /// <summary>
        /// This will tell the Sensor Network whether or not to initialize the azimuth encoder.
        /// We will not receive data for this sensor if it is not initialized.
        /// true = initialize;
        /// false = do not initialize
        /// </summary>
        [Column("azimuth_encoder_init")]
        public bool AzimuthEncoderInit { get; set; }

        /// <summary>
        /// This will tell the Sensor Network whether or not to initialize the elevation encoder.
        /// We will not receive data for this sensor if it is not initialized.
        /// true = initialize;
        /// false = do not initialize
        /// </summary>
        [Column("elevation_encoder_init")]
        public bool ElevationEncoderInit { get; set; }

        /// <summary>
        /// The timeout interval, in seconds, that the telescope will go into a state of <seealso cref="SensorNetworkStatusEnum.TimedOutDataRetrieval"/>.
        /// </summary>
        [Column("timeout_data_retrieval")]
        public int TimeoutDataRetrieval { get; set; }

        /// <summary>
        /// The timeout interval, in seconds, that the telescope will go into a state of <seealso cref="SensorNetworkStatusEnum.TimedOutInitialization"/>.
        /// </summary>
        [Column("timeout_initialization")]
        public int TimeoutInitialization { get; set; }
    }
    

}
