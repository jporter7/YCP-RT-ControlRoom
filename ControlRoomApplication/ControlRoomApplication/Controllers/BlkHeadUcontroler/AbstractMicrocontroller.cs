using ControlRoomApplication.Database;
using ControlRoomApplication.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ControlRoomApplication.Controllers.BlkHeadUcontroler {

    /// <summary>
    /// Abstract Class for the Microcontroller
    /// </summary>
    /// 
    public abstract class AbstractMicrocontroller {

        public AbstractMicrocontroller() { }

        //FourierTransform Class
        /// <summary>
        /// 
        /// </summary>
        /// 



        /// <summary>
        /// Start listetning for TCP Connection
        /// </summary>
        ///  
        public abstract bool BringUp();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        protected void interpretData( dynamic data ) {
            double threshold = 0;
            try {
                List<Temperature> temps = new List<Temperature>();
                List<Acceleration> accs = new List<Acceleration>();
                foreach(dynamic element in data.data) {
                    if(data.type == "temp") {
                        temps.Add( Temperature.Generate( element.time , element.val , SensorLocationEnumTypeConversionHelper.FromInt( element.loc ) ) );
                        threshold = 80;

                    } else if(data.type == "acc") {
                        accs.Add( Acceleration.Generate( element.time , element.val , element.x , element.y , element.z , SensorLocationEnumTypeConversionHelper.FromInt( element.loc ) ) );
                        threshold = 1.65;
                    } else {
                        Console.WriteLine( "Datatype not found" );
                        return;
                    }
                    if(element.val > threshold) {
                       // Console.WriteLine( element.val );
                      //  Console.WriteLine( element.time - DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() );
                    }

                }
                DatabaseOperations.AddSensorData( temps );
                DatabaseOperations.AddSensorData( accs );
            } catch(Exception e) {
                Console.WriteLine( e + "line 229" );
            }
        }
    }
}
