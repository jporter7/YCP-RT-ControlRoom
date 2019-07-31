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
        private Thread ClientManagmentThread;
        protected volatile bool KillClientManagementThreadFlag;
        protected ModbusSlave Modbusserver;

        public AbstractPLCDriver(IPAddress ip_address, int port)
        {
            try
            {
                ip_address= IPAddress.Parse("192.168.0.70");
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
                PLCTCPListener.Start(1);
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
        /// modbuss server
        /// </summary>
        protected abstract void HandleClientManagementThread();


        /// <summary>
        /// processes requests from the clientmanagementthread
        /// !not used in the production PLC driver
        /// </summary>
        /// <param name="ActiveClientStream"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        protected abstract bool ProcessRequest(NetworkStream ActiveClientStream, byte[] query);
    }
}