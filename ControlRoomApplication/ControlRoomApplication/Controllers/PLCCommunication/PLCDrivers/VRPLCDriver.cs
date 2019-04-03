using System;
using System.Net;
using System.Net.Sockets;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;
using System.IO.Pipes;
namespace ControlRoomApplication.Controllers
{
	public class VRPLCDriver : AbstractPLCDriver
	{

		public VRPLCDriver(IPAddress ip_address, int port) : base(ip_address, port)
		{
		
		}

		public VRPLCDriver(string ip, int port) : this(IPAddress.Parse(ip), port) { }

		protected override bool ProcessRequest(NetworkStream ActiveClientStream, byte[] query)
		{
			Console.WriteLine("We made it here at least");
			byte[] FinalResponseContainer;
			//Keeping this here for error handling
			int ExpectedSize = query[0] + (16 * query[1]);
			if (query.Length != ExpectedSize)
			{
				throw new ArgumentException(
					"VRPLCDriverController read a package specifying a size [" + ExpectedSize.ToString() + "], but the actual size was different [" + query.Length + "]."
				);
			}
			byte CommandQueryTypeAndExpectedResponseStatus = query[2];
			byte CommandQueryTypeByte = (byte)(CommandQueryTypeAndExpectedResponseStatus & 0x3F);
			byte ExpectedResponseStatusByte = (byte)(CommandQueryTypeAndExpectedResponseStatus >> 6);

			PLCCommandAndQueryTypeEnum CommandQueryTypeEnum = PLCCommandAndQueryTypeConversionHelper.GetFromByte(CommandQueryTypeByte);
			PLCCommandResponseExpectationEnum ExpectedResponseStatusEnum = PLCCommandResponseExpectationConversionHelper.GetFromByte(ExpectedResponseStatusByte);

			//Passing the byte[] query to the Unity Engine
			NamedPipeServerStream namedPipeServer = new NamedPipeServerStream("VR-pipe");
			namedPipeServer.WaitForConnection();
			//The lenght of the query is currently 19 (the base sive of our packets)
			//not sure if this value will have to change in the future or not
			//so leaving at 19 for now
			namedPipeServer.Write(query, 0, 19);
			namedPipeServer.WaitForPipeDrain();
			//Now we get the response from the VR telescope
			if (ExpectedResponseStatusEnum == PLCCommandResponseExpectationEnum.FULL_RESPONSE)
			{
				FinalResponseContainer = new byte[19];
				Console.WriteLine("We made it here at least");
				namedPipeServer.Read(FinalResponseContainer, 0, 19);
				for(int i = 0; i < 19; i++)
				{
					Console.Write(FinalResponseContainer[i]+", ");
				}
				Console.Write("\n");
			} else if(ExpectedResponseStatusEnum == PLCCommandResponseExpectationEnum.MINOR_RESPONSE)
			{
				FinalResponseContainer = new byte[3];
				namedPipeServer.Read(FinalResponseContainer, 0, 3);
			}
			else
			{
				throw new ArgumentException("Invalid PLCCommandResponseExpectationEnum value seen while processing client request in ScaleModelPLCDriver: " + ExpectedResponseStatusEnum.ToString());
			}
				

			//Were dont communicating for now, so were disconnecting and distroying the named pipeline
			namedPipeServer.Disconnect();
			namedPipeServer.Close();

			return AttemptToWriteDataToServer(ActiveClientStream, FinalResponseContainer);
		}
	}
}