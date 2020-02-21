using ControlRoomApplication.Entities;
using ControlRoomApplication.Simulators.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.PLCCommunication {

    public class limitEventArgs {
        public limitEventArgs( LimitSwitchData _AllLimits , PLC_modbus_server_register_mapping _ChangedValue , bool _Value ) { AllLimits = _AllLimits; ChangedValue = _ChangedValue; Value = _Value; }
        public LimitSwitchData AllLimits { get; } // readonly
        public PLC_modbus_server_register_mapping ChangedValue { get; } // readonly
        public bool Value { get; } // readonly
    }
    

    public delegate void PLCLimitChangedEvent( object sender , limitEventArgs e );

    /// <summary>
    /// this class stores the static events used to signal Limitswitches and other events
    /// </summary>
    public class PLCEvents {
        // Our static event  
        public static event PLCLimitChangedEvent LimitEvent;

        /// <summary>
        /// call from the PLC driver when a switch value is changed, this should not be called elsewhere
        /// </summary>
        /// <param name="_AllLimits"></param>
        /// <param name="_ChangedValue"></param>
        /// <param name="_Value"></param>        
        public void PLCLimitChanged( LimitSwitchData _AllLimits , PLC_modbus_server_register_mapping _ChangedValue , bool _Value ) {
            if(LimitEvent != null) {
                LimitEvent( this , new limitEventArgs( _AllLimits , _ChangedValue , _Value ) );
            }
        }
    }
}