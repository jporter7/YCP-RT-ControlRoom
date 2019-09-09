using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Entities {
    public enum SensorLocationEnum : int {
        UNDEFINED = -1,
        AZ_MOTOR = 0,
        EL_MOTOR = 1,
        END_OF_COUNTER_BALLENCE = 2,
        MICRO_CTRL = 3,
        MICRO_CTRL_BOX = 4
        
    }

    public class SensorLocationEnumTypeConversionHelper {
        /// <summary>
        /// if input is not in SensorLocationEnum returns undefined
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static SensorLocationEnum FromInt( int input ) {
            if(!Enum.IsDefined( typeof( SensorLocationEnum ) , input )) {
                return SensorLocationEnum.UNDEFINED;
            } else {
                return (SensorLocationEnum)input;
            }
        }
    }
}
