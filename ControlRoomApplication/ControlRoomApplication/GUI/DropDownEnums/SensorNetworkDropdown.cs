using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.GUI.DropDownEnums
{
    /// <summary>
    /// This will contain every option of the Sensor Network combo box so we can more easily tell what each thing is
    /// </summary>
    public enum SensorNetworkDropdown
    {
        /// <summary>
        /// This gets selected if we are connecting to the production sensor network, or another simulation that is
        /// not running within the control room.
        /// </summary>
        ProductionSensorNetwork,

        /// <summary>
        /// This gets selected if we are running the simulated sensor network within the control room.
        /// </summary>
        SimulatedSensorNetwork
    }
}
