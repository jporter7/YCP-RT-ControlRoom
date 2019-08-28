using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Constants {
    class TIME {
        /// <summary>
        /// C# is stupid and dosnt suport this nateively because it sucks
        /// </summary>
        public static readonly DateTime UnixEpoch =new DateTime( 1970 , 1 , 1 , 0 , 0 , 0 , DateTimeKind.Utc );
    }
}
