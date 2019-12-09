using ControlRoomApplication.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.BlkHeadUcontroler
{
    /// <summary>
    /// 
    /// </summary>
    public class EncoderReader : AbstractEncoderReader {
        static string message = "moshi moshi controlroom desu";

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        TcpClient client;
        int port;
        string ip;
        IPEndPoint ipEndPoint;
        NetworkStream stream;
        /// <summary>
        /// "192.168.7.2"
        /// </summary>
        /// <param name="micro_ctrl_IP"></param>
        /// <param name="port"></param>
        public EncoderReader( string micro_ctrl_IP , int port ) : base( micro_ctrl_IP,port ) {
            ip = micro_ctrl_IP;
            this.port = port;
            IPAddress ipAddress = IPAddress.Parse(micro_ctrl_IP);
            ipEndPoint = new IPEndPoint( ipAddress , port );




        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>current orentation</returns>
        public override Orientation GetCurentOrientation() {
            try {
                client = new TcpClient();
                client.Connect( ipEndPoint );
                stream = client.GetStream();
                Byte[] data = System.Text.Encoding.ASCII.GetBytes( message );
                stream.Write( data , 0 , data.Length );

                data = new Byte[2048];
                String responseData = String.Empty;

                Int32 bytes = stream.Read( data , 0 , data.Length );
                responseData = System.Text.Encoding.ASCII.GetString( data , 0 , bytes );
                dynamic respobj = null;
                try {

                    respobj = JsonConvert.DeserializeObject( responseData );
                    double AZ = respobj.AZ / (2048.0) * 180;
                    double EL = respobj.EL / (2048.0);
                    return new Orientation( AZ , EL );
                } catch(Exception e) {
                    logger.Info("parsing exception: {0}", e);
                    return null;
                } finally {
                    stream.Close();
                    client.Close();
                }
                // Close everything.
            } catch(ArgumentNullException e) {
                logger.Info("ArgumentNullException: {0}", e);
                return null;
            } catch(SocketException e) {
                //conection refused
                logger.Info("SocketException: {0}", e);
                return null;
            }

        }


    }


}
