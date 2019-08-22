using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ControlRoomApplication.Entities;
using Newtonsoft.Json;

namespace ControlRoomApplication.Controllers.BlkHeadUcontroler {
    class SimulatedEncoder : AbstractEncoderReader {

        EncoderReader driver;
        AbstractPLCDriver plc;
        string micro_ctrl_IP;
        int port;
        public SimulatedEncoder( string micro_ctrl_IP , int port ) : base( micro_ctrl_IP , port ) {

        }

        public SimulatedEncoder( AbstractPLCDriver plc , string micro_ctrl_IP , int port ) : base( plc , micro_ctrl_IP , port ) {
            driver = new EncoderReader( micro_ctrl_IP , port );
            this.plc = plc;
            this.micro_ctrl_IP = micro_ctrl_IP;
            this.port = port;
            new Thread( new ThreadStart( run_simulation ) ).Start();
        }

        public override Orientation GetCurentOrientation() {
            return driver.GetCurentOrientation();
        }

        private void run_simulation() {
            TcpListener server = new TcpListener( IPAddress.Parse( micro_ctrl_IP ) , port );
            server.Start();
            while(true) {
                ClientWorking cw = new ClientWorking( server.AcceptTcpClient() , plc );
                cw.DoSomethingWithClient();
            }
            server.Stop();
        }

        class ClientWorking {
            private Stream ClientStream;
            private TcpClient Client;
            private AbstractPLCDriver PCL;
            public ClientWorking( TcpClient Client , AbstractPLCDriver plc ) {
                PCL = plc;
                this.Client = Client;
                ClientStream = Client.GetStream();
            }

            public void DoSomethingWithClient() {
                StreamWriter sw = new StreamWriter( ClientStream );
                StreamReader sr = new StreamReader( sw.BaseStream );
                Orientation or = PCL.read_Position();
                int az = (int)((or.Azimuth / 360.0) * 4096);
                int el = (int)((or.Elevation / 20.0) * 4096*10);
                var obj = new {
                    uuid = "xxxxxxxxxxxxx" ,
                    type = "position" ,
                    AZ = az ,
                    EL = el
                };
                //Console.WriteLine(az +" "+el);
                string json = JsonConvert.SerializeObject( obj );
                sw.WriteLine( json );
                sw.Flush();
                /*
                string data;
                try {
                    while((data = sr.ReadLine()) != "exit") {
                        Console.WriteLine( data );
                        sw.WriteLine( data );
                        sw.Flush();
                    }
                } finally {
                    sw.Close();
                }//*/
            }
        }

    }

}
