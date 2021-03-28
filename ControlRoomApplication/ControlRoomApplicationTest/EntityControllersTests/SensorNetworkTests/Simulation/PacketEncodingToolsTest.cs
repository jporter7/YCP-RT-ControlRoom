using ControlRoomApplication.Controllers.SensorNetwork;
using EmbeddedSystemsTest.SensorNetworkSimulation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplicationTest.EntityControllersTests.SensorNetworkTests.Simulation
{
    [TestClass]
    public class PacketEncodingToolsTest
    {
        PacketEncodingTools tools = new PacketEncodingTools();

        [TestMethod]
        public void TestAdd32BitValueToByteArray_Max32BitUnsignedValue_ConvertsToByteArray()
        {
            uint max = 4294967295;

            byte[] resultBytes = new byte[4];
            int counter = 0;

            PacketEncodingTools.Add32BitValueToByteArray(ref resultBytes, ref counter, max);

            // Create expected byte array
            byte[] expectedBytes = new byte[4];
            expectedBytes[0] = 255;
            expectedBytes[1] = 255;
            expectedBytes[2] = 255;
            expectedBytes[3] = 255;

            Assert.IsTrue(resultBytes.SequenceEqual(expectedBytes));
        }

        [TestMethod]
        public void TestAdd32BitValueToByteArray_Min32BitUnsignedValue_ConvertsToByteArray()
        {
            uint min = 0;

            byte[] resultBytes = new byte[4];
            int counter = 0;

            PacketEncodingTools.Add32BitValueToByteArray(ref resultBytes, ref counter, min);

            // Create expected byte array
            byte[] expectedBytes = new byte[4];
            expectedBytes[0] = 0;
            expectedBytes[1] = 0;
            expectedBytes[2] = 0;
            expectedBytes[3] = 0;

            Assert.IsTrue(resultBytes.SequenceEqual(expectedBytes));
        }

        [TestMethod]
        public void TestAdd16BitValueToByteArray_Max16BitUnsignedValue_ConvertsToByteArray()
        {
            short max = 32767;

            byte[] resultBytes = new byte[2];
            int counter = 0;

            PacketEncodingTools.Add16BitValueToByteArray(ref resultBytes, ref counter, max);

            // Create expected byte array
            byte[] expectedBytes = new byte[2];
            expectedBytes[0] = 127; // 127 because it is positive
            expectedBytes[1] = 255;

            Assert.IsTrue(resultBytes.SequenceEqual(expectedBytes));
        }

        [TestMethod]
        public void TestAdd16BitValueToByteArray_Min16BitUnsignedValue_ConvertsToByteArray()
        {
            short min = -32768;

            byte[] resultBytes = new byte[2];
            int counter = 0;

            PacketEncodingTools.Add16BitValueToByteArray(ref resultBytes, ref counter, min);

            // Create expected byte array
            byte[] expectedBytes = new byte[2];
            expectedBytes[0] = 128;
            expectedBytes[1] = 0;

            Assert.IsTrue(resultBytes.SequenceEqual(expectedBytes));
        }

        [TestMethod]
        public void TestConvertTempCToRawData_AnyDoubleData_MultipliesCorrectly()
        {
            double input = 1.1;

            short result = PacketEncodingTools.ConvertTempCToRawData(input);

            short expected = 17;

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestConvertDegreesToRawAzData_AnyDoubleData_CalculatesCorrectly()
        {
            double input = 1.1;

            short result = PacketEncodingTools.ConvertDegreesToRawAzData(input);

            short expected = (short)((SensorNetworkConstants.AzimuthEncoderScaling * input) / 360);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestConvertDegreesToRawElData_AnyDoubleData_CalculatesCorrectly()
        {
            double input = 1.1;

            short result = PacketEncodingTools.ConvertDegreesToRawElData(input);

            short expected = (short)Math.Round((input + 20.375) / 0.25);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestCalcDataSize_AllZero_CalculatesSizeCorrectly()
        {
            uint result = PacketEncodingTools.CalcDataSize(0, 0, 0, 0, 0, 0, 0);

            uint expected = 19; // Default data size with no sensors

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestCalcDataSize_ElAccelerometer_CalculatesSizeCorrectly()
        {
            int elAcc = 1; // 6 bytes

            uint result = PacketEncodingTools.CalcDataSize(elAcc, 0, 0, 0, 0, 0, 0);

            uint expected = 19 + 6; // Default data size plus size of sensor data

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestCalcDataSize_AzAccelerometer_CalculatesSizeCorrectly()
        {
            int azAcc = 1; // 6 bytes

            uint result = PacketEncodingTools.CalcDataSize(0, azAcc, 0, 0, 0, 0, 0);

            uint expected = 19 + 6; // Default data size plus size of sensor data

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestCalcDataSize_CbAccelerometer_CalculatesSizeCorrectly()
        {
            int cbAcc = 1; // 6 bytes

            uint result = PacketEncodingTools.CalcDataSize(0, 0, cbAcc, 0, 0, 0, 0);

            uint expected = 19 + 6; // Default data size plus size of sensor data

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestCalcDataSize_ElTemperature_CalculatesSizeCorrectly()
        {
            int elTemp = 1; // 2 bytes

            uint result = PacketEncodingTools.CalcDataSize(0, 0, 0, elTemp, 0, 0, 0);

            uint expected = 19 + 2; // Default data size plus size of sensor data

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestCalcDataSize_AzTemperature_CalculatesSizeCorrectly()
        {
            int azTemp = 1; // 2 bytes

            uint result = PacketEncodingTools.CalcDataSize(0, 0, 0, 0, azTemp, 0, 0);

            uint expected = 19 + 2; // Default data size plus size of sensor data

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestCalcDataSize_ElEncoder_CalculatesSizeCorrectly()
        {
            int elEnc = 1; // 2 bytes

            uint result = PacketEncodingTools.CalcDataSize(0, 0, 0, 0, 0, elEnc, 0);

            uint expected = 19 + 2; // Default data size plus size of sensor data

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestCalcDataSize_AzEncoder_CalculatesSizeCorrectly()
        {
            int azEnc = 1; // 2 bytes

            uint result = PacketEncodingTools.CalcDataSize(0, 0, 0, 0, 0, 0, azEnc);

            uint expected = 19 + 2; // Default data size plus size of sensor data

            Assert.AreEqual(expected, result);
        }
    }
}
