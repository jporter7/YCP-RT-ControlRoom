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

        //Struct to hold the necessary temp, limit switch, and proximity sensor data before being sent to database
        public struct MicroControllerData
        {
            public long azimuthTempTime;
            public double azimuthTemp;

            public long elevationTempTime;
            public double elevationTemp;

            public long azimuthAccTime;
            public double azimuthAcc;
            public double azimuthX;
            public double azimuthY;
            public double azimuthZ;

            public long elevationAccTime;
            public double elevationAcc;
            public double elevationX;
            public double elevationY;
            public double elevationZ;
        }

        MicroControllerData myData;

        public AbstractMicrocontroller()
        {
            myData = new MicroControllerData();
        }

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

                foreach(Temperature temp in temps)
                {
                    Console.WriteLine("Motor int val is: " + temp.location_ID);
                    Console.WriteLine("Motor type is: " + SensorLocationEnumTypeConversionHelper.FromInt(temp.location_ID));
                    SensorLocationEnum location = SensorLocationEnumTypeConversionHelper.FromInt(temp.location_ID);

                    if(location == SensorLocationEnum.AZ_MOTOR) // SensorLocationEnum.AZ_MOTOR
                    {
                        Console.WriteLine("Adding in temp for the azimuth motors");
                        myData.azimuthTemp = temp.temp;
                        myData.azimuthTempTime = temp.TimeCapturedUTC;
                    }
                    else if (location == SensorLocationEnum.EL_MOTOR) // SensorLocationEnum.AZ_MOTOR
                    {
                        Console.WriteLine("Adding in temp for the elevation motors");
                        myData.elevationTemp = temp.temp;
                        myData.elevationTempTime = temp.TimeCapturedUTC;
                    }
                    else
                    {
                        Console.WriteLine("Uknown motor type for temperature");
                    }
                }

                foreach (Acceleration acc in accs)
                {
                    if (acc.location_ID == 0) // SensorLocationEnum.AZ_MOTOR
                    {
                        myData.azimuthAcc = acc.acc;
                        myData.azimuthAccTime = acc.TimeCaptured;
                        myData.azimuthX = acc.x;
                        myData.azimuthY = acc.y;
                        myData.azimuthZ = acc.z;
                    }
                    else if (acc.location_ID == 1) // SensorLocationEnum.AZ_MOTOR
                    {
                        myData.elevationAcc = acc.acc;
                        myData.elevationAccTime = acc.TimeCaptured;
                        myData.elevationX = acc.x;
                        myData.elevationY = acc.y;
                        myData.elevationZ = acc.z;
                    }
                    else
                    {
                        Console.WriteLine("Uknown motor type for temperature");
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
