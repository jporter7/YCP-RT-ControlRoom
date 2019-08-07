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
    public class EncoderReader
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>current orentation</returns>
        static public Orientation GetCurentOrientation()
        {
            string message = "moshi moshi controlroom desu";
            try
            {
                Int32 port = 1602;
                TcpClient client = new TcpClient("192.168.7.2",port);

                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);

                data = new Byte[2048];
                String responseData = String.Empty;

                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);
                dynamic respobj = null;
                try
                {
                    respobj =JsonConvert.DeserializeObject(responseData);
                    Console.WriteLine(respobj.AZ+" "+ respobj.EL);
                    return new Orientation((double)respobj.AZ, (double)respobj.EL);
                    //return new Orientation(0, (double)0);
                }
                catch(Exception e)
                {
                    Console.WriteLine("parsing exception: {0}", e);
                    return null;
                }
                finally
                {
                    stream.Close();
                    client.Close();
                }
                // Close everything.
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
                return null;
            }
            catch (SocketException e)
            {
                //conection refused
                //Console.WriteLine("SocketException: {0}", e);
                return null;
            }

        }
    }


}
