using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Controllers.PLCCommunication;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Entities;

namespace ControlRoomApplicationTest.EntityControllersTests
{
	[TestClass]
	public class VRPLCDriverTest
	{
		public static PLCClientCommunicationHandler CommsHandler;
		public static VRPLCDriver TestDriver;

		[ClassInitialize]
		public static void BuildUp(TestContext testContext)
		{
			string ip = PLCConstants.LOCAL_HOST_IP;
			int port = PLCConstants.PORT_8080;

			CommsHandler = new PLCClientCommunicationHandler(ip, port);
			TestDriver = new VRPLCDriver(ip, port);

			TestDriver.StartAsyncAcceptingClients();

			CommsHandler.ConnectToServer();
		}

		[TestMethod]
		public void TestConnectionTest()
		{
			byte[] expectedResponse = new byte[] { 0x13, 0x0, 0x1, 0x1, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
			byte[] actualResponse = CommsHandler.RequestMessageSend(PLCCommandAndQueryTypeEnum.TEST_CONNECTION);

			Console.WriteLine("Test Connection (actual): [{0}]", string.Join(", ", actualResponse));
			Console.WriteLine("Test Connection (expected): [{0}]", string.Join(", ", expectedResponse));

			CollectionAssert.AreEqual(expectedResponse, actualResponse);
		}

		[TestMethod]
		public void TestSendAzeEle()
		{
			double NewAzimuth = 90;
			double NewElevation = 45;

			byte[] expectedResponse = new byte[] { 0x3, 0x0, 0x1 };
			byte[] actualResponse = CommsHandler.RequestMessageSend(PLCCommandAndQueryTypeEnum.SET_OBJECTIVE_AZEL_POSITION, new Orientation(NewAzimuth, NewElevation));

			Console.WriteLine("Test Set Active Objective AZ/EL (actual): [{0}]", string.Join(", ", actualResponse));
			Console.WriteLine("Test Set Active Objective AZ/EL (expected): [{0}]", string.Join(", ", expectedResponse));

			CollectionAssert.AreEqual(expectedResponse, actualResponse);
			
		}
		/*
		[TestMethod]
		public void TestDemo()
		{
			sendcoords(90, 45);
			sleep(17);
			sendcoords(0,0);
			sleep(17);
			sendcoords(45, 45);
			sleep(15);
			sendcoords(90, 45);
			sleep(7);
			sendcoords(90, 55);
			sleep(4);
			sendcoords(45, 55);
			sleep(7);
			sendcoords(45, 60);
			sleep(4);
			sendcoords(90, 60);
			sleep(7);
			sendcoords(90, 70);
			sleep(4);
			sendcoords(45, 70);
			sleep(7);
			sendcoords(0, 90);
		}
		

		private void sendcoords(Double NewAzimuth, Double NewElevation)
		{
			CommsHandler.RequestMessageSend(PLCCommandAndQueryTypeEnum.SET_OBJECTIVE_AZEL_POSITION, new Orientation(NewAzimuth, NewElevation));
		}

		private void sleep(double seconds)
		{

			System.Threading.Thread.Sleep((int)seconds * 1000);
		}*/
	}
}