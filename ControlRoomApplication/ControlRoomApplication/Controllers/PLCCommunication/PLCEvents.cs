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

        private static PLCLimitChangedEvent defaultHandler;
        private static List<PLCLimitChangedEvent> handles = new List<PLCLimitChangedEvent>();
        private static object eventLock = new object();
        private static bool overriden = false;

        /// <summary>
        /// set the default event handler for limit switch events, idealy this should only be called once
        /// </summary>
        /// <param name="_defa"></param>
        public static void setDefaultLimitHandler( PLCLimitChangedEvent _defa ) {
            lock(eventLock) {
                defaultHandler = _defa;
                LimitEvent = defaultHandler;
            }
        }

        /// <summary>
        /// add a limit handle for limit events
        /// </summary>
        /// <param name="Handl"></param>
        public static void addLimitHandler( PLCLimitChangedEvent Handl ) {
            lock(eventLock) {
                if(!overriden) {
                    LimitEvent += Handl;
                }
                handles.Add( Handl );
            }
        }

        /// <summary>
        /// remove the passed event Handle
        /// </summary>
        /// <param name="Handl"></param>
        public static void RemoveLimitHandler( PLCLimitChangedEvent Handl ) {
            lock(eventLock) {
                try {
                    if(!overriden) {
                        LimitEvent -= Handl;
                    }
                } catch { }
                try {
                    handles.Remove( Handl );
                } catch { }
            }
        }

        /// <summary>
        /// override all exsisting limit events and replace them with Handl
        /// </summary>
        /// <param name="Handl"></param>
        public static void OverrideLimitHandlers( PLCLimitChangedEvent Handl ) {
            lock(eventLock) {
                overriden = true;
                LimitEvent = Handl;
            }
        }

        public static void DurringOverrideAddSecondary( PLCLimitChangedEvent Handl ) {
            lock(eventLock) {
                LimitEvent += Handl;
                handles.Add( Handl );
            }
        }

        public static void DurringOverrideRemoveSecondary( PLCLimitChangedEvent Handl ) {
            lock(eventLock) {
                try {
                    LimitEvent -= Handl;
                } catch { }
                try {
                    handles.Remove( Handl );
                } catch { }
            }
        }

        /// <summary>
        /// reset events to befor they were overriden
        /// </summary>
        public static void ResetOverrides() {
            lock(eventLock) {
                overriden = false;
                LimitEvent = defaultHandler;
                foreach(var evt in handles) {
                    LimitEvent += evt;
                }
            }
        }

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