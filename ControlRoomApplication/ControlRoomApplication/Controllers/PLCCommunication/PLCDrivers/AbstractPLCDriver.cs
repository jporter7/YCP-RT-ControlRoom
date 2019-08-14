using Modbus.Data;
using Modbus.Device;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ControlRoomApplication.Controllers
{
    public abstract class AbstractPLCDriver
    {
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected TcpListener PLCTCPListener;
        protected Thread ClientManagmentThread;
        protected volatile bool KillClientManagementThreadFlag;
        protected ModbusSlave Modbusserver;
        /// <summary>
        /// this ip shoud be the ip of the controll rooom box
        /// starts a tcp server on the specified port 
        /// </summary>
        /// <param name="ip_address"></param>
        /// <param name="port"></param>
        public AbstractPLCDriver(IPAddress ip_address, int port)
        {
            try
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[ipHostInfo.AddressList.Length - 1];//this get the ip of this machine
                for(int i =0;i< ipHostInfo.AddressList.Length; i++)
                {
                    Console.WriteLine(ipHostInfo.AddressList[i]);
                }
                Console.WriteLine("############################################");
                Console.WriteLine(ip_address);
                //PLCTCPListener = new TcpListener(new IPEndPoint(IPAddress.Parse("127.0.0.1"), port));//this starts a tcp server pointed to listen to that ip
                //PLCTCPListener = new TcpListener(new IPEndPoint(IPAddress.Parse("192.168.0.70"), port));
                PLCTCPListener = new TcpListener(new IPEndPoint(ip_address, port));
                ClientManagmentThread = new Thread(new ThreadStart(HandleClientManagementThread));
            }
            catch (Exception e)
            {
                if ((e is ArgumentNullException) || (e is ArgumentOutOfRangeException))
                {
                    logger.Info("[AbstractPLCDriver] ERROR: failure creating PLC TCP server or management thread: " + e.ToString());
                    return;
                }
                else
                {
                    // Unexpected exception
                    throw e;
                }
            }
            try
            {
               PLCTCPListener.Start(1);//start listing for tcp requests
            }
            catch (Exception e)
            {
                if ((e is SocketException) || (e is ArgumentOutOfRangeException) || (e is InvalidOperationException))
                {
                    logger.Info("[AbstractPLCDriver] ERROR: failure starting PLC TCP server: " + e.ToString());
                    return;
                }
            }
        }

        public AbstractPLCDriver(string ip, int port) : this(IPAddress.Parse(ip), port) { }

        public bool StartAsyncAcceptingClients()
        {
            KillClientManagementThreadFlag = false;

            try
            {
                ClientManagmentThread.Start();
            }
            catch (Exception e)
            {
                if ((e is ThreadStateException) || (e is OutOfMemoryException))
                {
                    return false;
                }
                else
                {
                    // Unexpected exception
                    throw e;
                }
            }

            return true;
        }

        public bool RequestStopAsyncAcceptingClientsAndJoin()
        {
            KillClientManagementThreadFlag = true;

            try
            {
                ClientManagmentThread.Join();
            }
            catch (Exception e)
            {
                if ((e is ThreadStateException) || (e is ThreadStartException))
                {
                    return false;
                }
                else
                {
                    // Unexpected exception
                    throw e;
                }
            }

            return true;
        }

        protected bool AttemptToWriteDataToServer(NetworkStream ActiveClientStream, byte[] ResponseData)
        {
            try
            {
                ActiveClientStream.Write(ResponseData, 0, ResponseData.Length);
            }
            catch (Exception e)
            {
                if ((e is ArgumentNullException) || (e is ArgumentOutOfRangeException) || (e is System.IO.IOException) || (e is ObjectDisposedException))
                {
                    logger.Info("[AbstractPLCDriver] ERROR: writing back to client with the PLC's response {" + ResponseData.ToString() + "}");
                    return false;
                }
            }

            return true;
        }

       // public delegate void ReadReghandler<ModbusSlaveRequestEventArgs>(object sender, ModbusSlaveRequestEventArgs e);


        /// <summary>
        /// modbuss server implamentation specific to each device
        /// </summary>
        protected abstract void HandleClientManagementThread();


        /// <summary>
        /// processes requests from the clientmanagementthread
        /// !not used in the production PLC driver
        ///is used in simulation although it may not be later
        /// </summary>
        /// <param name="ActiveClientStream"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        protected abstract bool ProcessRequest(NetworkStream ActiveClientStream, byte[] query);
    }
}