using ControlRoomApplication.Controllers.SensorNetwork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlRoomApplication.Entities;
using EmbeddedSystemsTest.SensorNetworkSimulation;

namespace ControlRoomApplicationTest.EntityControllersTests.SensorNetworkTests
{
    [TestClass]
    public class PacketDecodingToolsTest
    {
        [TestMethod]
        public void TestGetAccelerationFromBytes_BytesToAcceleration_ReturnsAcceleration()
        {
            // The byte size for one acceleration is 6 bytes, because each axis takes up 2 bytes, 
            // and there are 3 axes.
            byte[] oneAcceleration = new byte[6];

            // This will create an acceleration with x, y and z axes as 1, 2 and 3 respectively
            oneAcceleration[0] = 0;
            oneAcceleration[1] = 1;
            oneAcceleration[2] = 0;
            oneAcceleration[3] = 2;
            oneAcceleration[4] = 0;
            oneAcceleration[5] = 3;

            // Skipping the timestamp because we aren't concerned with that in this test
            Acceleration[] expected = new Acceleration[1];
            expected[0] = Acceleration.Generate(0, 1, 2, 3, SensorLocationEnum.COUNTERBALANCE);

            // This is only used for the counter, becuase it needs a variable to be passed by reference
            int i = 0;

            var result = PacketDecodingTools.GetAccelerationFromBytes(ref i, oneAcceleration, 1, SensorLocationEnum.COUNTERBALANCE);

            Assert.AreEqual(1, result.Length); // Only expecting one result

            Assert.AreEqual(expected[0].x, result[0].x);
            Assert.AreEqual(expected[0].y, result[0].y);
            Assert.AreEqual(expected[0].z, result[0].z);
            Assert.AreEqual(expected[0].location_ID, result[0].location_ID);
        }

        [TestMethod]
        public void TestGetAccelerationFromBytes_BytesToMultipleAcceleration_ReturnsMultipleAcceleration()
        {
            // The byte size for one acceleration is 6 bytes, because each axis takes up 2 bytes, 
            // and there are 3 axes.
            byte[] twoAcceleration = new byte[12];

            // This will create two acceleration results with x, y and z axes as 1, 2 and 3 respectively
            twoAcceleration[0] = 0;
            twoAcceleration[1] = 1;
            twoAcceleration[2] = 0;
            twoAcceleration[3] = 2;
            twoAcceleration[4] = 0;
            twoAcceleration[5] = 3;
            twoAcceleration[6] = 0;
            twoAcceleration[7] = 1;
            twoAcceleration[8] = 0;
            twoAcceleration[9] = 2;
            twoAcceleration[10] = 0;
            twoAcceleration[11] = 3;

            // Skipping the timestamp because we aren't concerned with that in this test
            Acceleration[] expected = new Acceleration[2];
            expected[0] = Acceleration.Generate(0, 1, 2, 3, SensorLocationEnum.COUNTERBALANCE);
            expected[1] = Acceleration.Generate(0, 1, 2, 3, SensorLocationEnum.COUNTERBALANCE);

            // This is only used for the counter, becuase it needs a variable to be passed by reference
            int i = 0;

            var result = PacketDecodingTools.GetAccelerationFromBytes(ref i, twoAcceleration, 2, SensorLocationEnum.COUNTERBALANCE);

            Assert.AreEqual(2, result.Length); // Expecting two results

            Assert.AreEqual(expected[0].x, result[0].x);
            Assert.AreEqual(expected[0].y, result[0].y);
            Assert.AreEqual(expected[0].z, result[0].z);
            Assert.AreEqual(expected[0].location_ID, result[0].location_ID);

            Assert.AreEqual(expected[1].x, result[1].x);
            Assert.AreEqual(expected[1].y, result[1].y);
            Assert.AreEqual(expected[1].z, result[1].z);
            Assert.AreEqual(expected[1].location_ID, result[1].location_ID);
        }

        [TestMethod]
        public void TestGetTemperatureFromBytes_BytesToTemperature_ReturnsTemperature()
        {
            // The byte size for one temperature is 2 bytes
            byte[] oneTemperature = new byte[2];

            // This will create temperature value of 1, because the temperature is divided by 16
            oneTemperature[0] = 0;
            oneTemperature[1] = 16;

            // Skipping the timestamp because we aren't concerned with that in this test
            Temperature[] expected = new Temperature[1];
            expected[0] = Temperature.Generate(0, 1, SensorLocationEnum.COUNTERBALANCE);

            // This is only used for the counter, becuase it needs a variable to be passed by reference
            int i = 0;

            var result = PacketDecodingTools.GetTemperatureFromBytes(ref i, oneTemperature, 1, SensorLocationEnum.COUNTERBALANCE);

            Assert.AreEqual(1, result.Length); // Only expecting one result

            Assert.AreEqual(expected[0].temp, result[0].temp);
            Assert.AreEqual(expected[0].location_ID, result[0].location_ID);
        }

        [TestMethod]
        public void TestGetTemperatureFromBytes_BytesToMultipleTemperatures_ReturnsMultipleTemperatures()
        {
            // The byte size for one temperature is 2 bytes
            byte[] twoTemperature = new byte[4];

            // This will create temperature value of 1, because the temperature is divided by 16
            twoTemperature[0] = 0;
            twoTemperature[1] = 16;
            twoTemperature[2] = 0;
            twoTemperature[3] = 16;

            // Skipping the timestamp because we aren't concerned with that in this test
            Temperature[] expected = new Temperature[2];
            expected[0] = Temperature.Generate(0, 1, SensorLocationEnum.COUNTERBALANCE);
            expected[1] = Temperature.Generate(0, 1, SensorLocationEnum.COUNTERBALANCE);

            // This is only used for the counter, becuase it needs a variable to be passed by reference
            int i = 0;

            var result = PacketDecodingTools.GetTemperatureFromBytes(ref i, twoTemperature, 2, SensorLocationEnum.COUNTERBALANCE);

            Assert.AreEqual(2, result.Length); // Expecting two results

            Assert.AreEqual(expected[0].temp, result[0].temp);
            Assert.AreEqual(expected[0].location_ID, result[0].location_ID);

            Assert.AreEqual(expected[1].temp, result[1].temp);
            Assert.AreEqual(expected[1].location_ID, result[1].location_ID);
        }

        [TestMethod]
        public void TestGetAzimuthAxisPositionFromBytes_BytesToPosition_ReturnsPosition()
        {
            // byte size for an axis position is 2 bytes
            byte[] pos = new byte[2];

            // Encode
            int i = 0;
            double expected = 50;
            short encoded = PacketEncodingTools.ConvertDegreesToRawAzData(expected);
            PacketEncodingTools.Add16BitValueToByteArray(ref pos, ref i, encoded);

            // Decode
            i = 0;
            int offset = 0;

            double result = PacketDecodingTools.GetAzimuthAxisPositionFromBytes(ref i, pos, offset);

            Assert.AreEqual(expected, result, 0.16);
        }

        [TestMethod]
        public void TestGetAzimuthAxisPositionFromBytes_BytesToPositionWithOffset_ReturnsNormalizedPosition()
        {
            // byte size for an axis position is 2 bytes
            byte[] pos = new byte[2];

            // Encode
            int i = 0;
            double initialValue = 50;
            short encoded = PacketEncodingTools.ConvertDegreesToRawAzData(initialValue);
            PacketEncodingTools.Add16BitValueToByteArray(ref pos, ref i, encoded);

            // Decode
            i = 0;
            // With an offset of 60, that would make the origination originally -10, but with normalization, it should be 350
            int offset = 60;

            int expected = 350;

            double result = PacketDecodingTools.GetAzimuthAxisPositionFromBytes(ref i, pos, offset);

            Assert.AreEqual(expected, result, 0.16);
        }
    }
}
