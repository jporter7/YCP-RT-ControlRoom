using ControlRoomApplication.Controllers.SensorNetwork;
using ControlRoomApplication.Controllers.SensorNetwork.Simulation;
using ControlRoomApplication.Entities.DiagnosticData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbeddedSystemsTest.SensorNetworkSimulation
{
    /// <summary>
    /// This contains helper functions to allow us to encode data in the same way the Sensor Network would before sending it to us.
    /// The data it encodes will be data from CSVs, but it will "send" raw data to the SensorNetworkServer.
    /// </summary>
    public class PacketEncodingTools
    {
        /// <summary>
        /// This is important for the simulation. This will convert the data arrays we get from CSV files
        /// to bytes that we can send to the SensorNetworkServer.
        /// </summary>
        /// <param name="elAccl">Array of RAW elevation accelerometer samples.</param>
        /// <param name="azAccl">Array of RAW azimuth accelerometer samples.</param>
        /// <param name="cbAccl">Array of RAW counterbalance accelerometer samples.</param>
        /// <param name="elTemps">Array of elevation temperature samples.</param>
        /// <param name="azTemps">Array of azimuth temperature samples.</param>
        /// <param name="elEnc">Array of elevation encoder samples.</param>
        /// <param name="azEnc">Array of azimuth encoder samples.</param>
        /// <param name="statuses">All the sensor statuses and errors that come from the sensor network.</param>
        /// <returns></returns>
        public static byte[] ConvertDataArraysToBytes(RawAccelerometerData[] elAccl, RawAccelerometerData[] azAccl, RawAccelerometerData[] cbAccl, double[] elTemps, double[] azTemps, double[] elEnc, double[] azEnc, SensorStatuses statuses)
        {
            uint dataSize = CalcDataSize(elAccl.Length, azAccl.Length, cbAccl.Length, elTemps.Length, azTemps.Length, elEnc.Length, azEnc.Length);

            // If you want to input raw data instead, just comment out the next few loops.
            // They exist so that we can input data into our CSV files that make sense to us, since
            // raw data values are not very readable

            // Convert elevation temperature to raw data
            short[] rawElTemps = new short[elTemps.Length];
            for (int i = 0; i < elTemps.Length; i++)
            {
                rawElTemps[i] = ConvertTempCToRawData(elTemps[i]);
            }

            // Convert azimuth temperature to raw data
            short[] rawAzTemps = new short[azTemps.Length];
            for (int i = 0; i < azTemps.Length; i++)
            {
                rawAzTemps[i] = ConvertTempCToRawData(azTemps[i]);
            }

            // Convert elevation position to raw data
            short[] rawElEnc = new short[elEnc.Length];
            for(int i = 0; i < elEnc.Length; i++)
            {
                rawElEnc[i] = ConvertDegreesToRawElData(elEnc[i]);
            }

            // Convert azimuth position to raw data
            short[] rawAzEnc = new short[azEnc.Length];
            for (int i = 0; i < azEnc.Length; i++)
            {
                rawAzEnc[i] = ConvertDegreesToRawAzData(azEnc[i]);
            }

            bool[] sensorStatusBoolArray = new bool[] {
                statuses.ElevationAccelerometerStatus == SensorNetworkSensorStatus.Okay,
                statuses.AzimuthAccelerometerStatus == SensorNetworkSensorStatus.Okay,
                statuses.CounterbalanceAccelerometerStatus == SensorNetworkSensorStatus.Okay,
                statuses.ElevationTemperature1Status == SensorNetworkSensorStatus.Okay,
                statuses.ElevationTemperature2Status == SensorNetworkSensorStatus.Okay,
                statuses.AzimuthTemperature1Status == SensorNetworkSensorStatus.Okay,
                statuses.AzimuthTemperature2Status == SensorNetworkSensorStatus.Okay,
                statuses.AzimuthAbsoluteEncoderStatus == SensorNetworkSensorStatus.Okay
            };

            int errors = 0; // TODO: implement conversion

            return EncodeRawData(dataSize, elAccl, azAccl, cbAccl, rawElTemps, rawAzTemps, rawElEnc, rawAzEnc, sensorStatusBoolArray, errors);
        }
        
        /// <summary>
        /// This will take each RAW data array and add it to its proper location in the byte array
        /// </summary>
        /// <param name="dataSize">The total size of the byte array we are adding data to.</param>
        /// <param name="elAcclData">Array of RAW elevation accelerometer samples.</param>
        /// <param name="azAcclData">Array of RAW azimuth accelerometer samples.</param>
        /// <param name="cbAcclData">Array of RAW counterbalance accelerometer samples.</param>
        /// <param name="elTemp">Array of RAW elevation temperature samples.</param>
        /// <param name="azTemp">Array of RAW azimuth temperature samples.</param>
        /// <param name="elEnc">Array of RAW elevation encoder samples. (Should only ever be a size of 1)</param>
        /// <param name="azEnc">Array of RAW azimuth encoder samples. (Should only ever be a size of 1)</param>
        /// <returns></returns>
        public static byte[] EncodeRawData(uint dataSize, RawAccelerometerData[] elAcclData, RawAccelerometerData[] azAcclData, RawAccelerometerData[] cbAcclData, short[] elTemp, short[] azTemp, short[] elEnc, short[] azEnc, bool[] statuses, int errors)
        {
            byte[] data = new byte[dataSize];

            int i = 0;
            data[i++] = SensorNetworkConstants.TransitIdSuccess;

            // Store the total data size in 4 bytes
            Add32BitValueToByteArray(ref data, ref i, dataSize);

            // Store the sensor statuses
            data[i++] = ConvertBoolArrayToByte(statuses);

            // Store the sensor errors in 3 bytes
            Add24BitValueToByteArray(ref data, ref i, errors);

            // Store elevation accelerometer size in 2 bytes
            Add16BitValueToByteArray(ref data, ref i, (short)elAcclData.Length);

            // Store azimuth accelerometer size in 2 bytes
            Add16BitValueToByteArray(ref data, ref i, (short)azAcclData.Length);

            // Store counterbalance accelerometer size in 2 bytes
            Add16BitValueToByteArray(ref data, ref i, (short)cbAcclData.Length);

            // Store elevation temperature size in 2 bytes
            Add16BitValueToByteArray(ref data, ref i, (short)elTemp.Length);

            // Store azimuth temperature size in 2 bytes
            Add16BitValueToByteArray(ref data, ref i, (short)azTemp.Length);

            // Store elevation encoder size in 2 bytes
            Add16BitValueToByteArray(ref data, ref i, (short)elEnc.Length);

            // Store azimuth encoder size in 2 bytes
            Add16BitValueToByteArray(ref data, ref i, (short)azEnc.Length);

            // Store elevation accelerometer data in a variable number of bytes
            // Each axis occupies 2 bytes, making a total of 6 bytes for each accelerometer data
            for (uint j = 0; j < elAcclData.Length; j++)
            {
                Add16BitValueToByteArray(ref data, ref i, (short)elAcclData[j].X);
                Add16BitValueToByteArray(ref data, ref i, (short)elAcclData[j].Y);
                Add16BitValueToByteArray(ref data, ref i, (short)elAcclData[j].Z);
            }

            // Store azimuth accelerometer data in a variable number of bytes
            // Each axis occupies 2 bytes, making a total of 6 bytes for each accelerometer data
            for (uint j = 0; j < azAcclData.Length; j++)
            {
                Add16BitValueToByteArray(ref data, ref i, (short)azAcclData[j].X);
                Add16BitValueToByteArray(ref data, ref i, (short)azAcclData[j].Y);
                Add16BitValueToByteArray(ref data, ref i, (short)azAcclData[j].Z);
            }

            // Store counterbalance accelerometer data in a variable number of bytes
            // Each axis occupies 2 bytes, making a total of 6 bytes for each accelerometer data
            for (uint j = 0; j < cbAcclData.Length; j++)
            {
                Add16BitValueToByteArray(ref data, ref i, (short)cbAcclData[j].X);
                Add16BitValueToByteArray(ref data, ref i, (short)cbAcclData[j].Y);
                Add16BitValueToByteArray(ref data, ref i, (short)cbAcclData[j].Z);
            }

            // Store elevation temperature data in a variable number of bytes
            // Each temperature occupies 2 bytes
            for (uint j = 0; j < elTemp.Length; j++)
            {
                Add16BitValueToByteArray(ref data, ref i, (short)elTemp[j]);
            }

            // Store azimuth temperature data in a variable number of bytes
            // Each temperature occupies 2 bytes
            for (uint j = 0; j < azTemp.Length; j++)
            {
                Add16BitValueToByteArray(ref data, ref i, (short)azTemp[j]);
            }

            // Store elevation encoder data in a variable number of bytes
            // Each position occupies 2 bytes
            for (uint j = 0; j < elEnc.Length; j++)
            {
                Add16BitValueToByteArray(ref data, ref i, (short)elEnc[j]);
            }

            // Store azimuth encoder data in a variable number of bytes
            // Each position occupies 2 bytes
            for (uint j = 0; j < azEnc.Length; j++)
            {
                Add16BitValueToByteArray(ref data, ref i, (short)azEnc[j]);
            }

            return data;
        }

        /// <summary>
        /// Calculates the size of the packet that will be sent to the sensor network.
        /// This value will be used to create the byte array.
        /// </summary>
        /// <param name="elAccSize">The number of elevation accelerometer samples.</param>
        /// <param name="azAccSize">The number of azimuth accelerometer samples.</param>
        /// <param name="cbAccSize">The number of counterbalance accelerometer samples.</param>
        /// <param name="elTempSize">The number of elevation temperature samples.</param>
        /// <param name="azTempSize">The number of azimuth temperature samples.</param>
        /// <param name="elEncSize">The number of elevation encoder samples.</param>
        /// <param name="azEncSize">The number of azimuth encoder samples.</param>
        /// <returns></returns>
        public static uint CalcDataSize(int elAccSize, int azAccSize, int cbAccSize, int elTempSize, int azTempSize, int elEncSize, int azEncSize)
        {
            // 1 for the transmit ID
            // 4 for the total data size
            // 4 for the sensor statuses and errors
            // 14 for each sensor's data size (each sensor size is 2 bytes, with 7 sensors total)
            uint length = 1 + 4 + 4 + 14;

            // Each accelerometer axis is 2 bytes each. With three axes, that's 6 bytes per accelerometer
            length += (uint)elAccSize * 6;
            length += (uint)azAccSize * 6;
            length += (uint)cbAccSize * 6;

            // Each temp and encoder value is 2 bytes
            length += (uint)elTempSize * 2;
            length += (uint)azTempSize * 2;

            // Encoder arrays should always be of size 1, so they should only ever be two bytes each
            length += (uint)elEncSize * 2;
            length += (uint)azEncSize * 2;

            return length;
        }

        /// <summary>
        /// This is so we can give the simulation "real" data, where it will be converted to raw
        /// before being encoded. This is approximate, and may not be exact.
        /// </summary>
        /// <param name="dataToConvert">The data we are converting to a raw elevation position.</param>
        /// <returns>The raw elevation position.</returns>
        public static short ConvertDegreesToRawElData(double dataToConvert)
        {
            return (short)Math.Round((dataToConvert - 104.375) / -0.25);
        }

        /// <summary>
        /// This is so we can give the simulation "real" data, where it will be converted to raw
        /// before being encoded. This is approximate, and may not be exact.
        /// </summary>
        /// <param name="dataToConvert">The data we are converting to a raw azimuth position.</param>
        /// <returns>The raw elevation position</returns>
        public static short ConvertDegreesToRawAzData(double dataToConvert)
        {
            return (short)((SensorNetworkConstants.AzimuthEncoderScaling * dataToConvert / 360) * -1);
        }

        /// <summary>
        /// This converts the degrees from celsius into raw data. This is approximate.
        /// </summary>
        /// <param name="dataToConvert">The data we are converting to a raw value (in Celsius)</param>
        /// <returns>Raw temperature data.</returns>
        public static short ConvertTempCToRawData(double dataToConvert)
        {
            return (short)(dataToConvert * 16);
        }

        /// <summary>
        /// A helper function to add 16-bit values to the byte array so we don't have to do this every single time.
        /// </summary>
        /// <param name="dataToAddTo">The byte array we are modifying.</param>
        /// <param name="counter">The counter to tell us where in the byte array we are modifying.</param>
        /// <param name="dataBeingAdded">The data we are adding to the byte array.</param>
        public static void Add16BitValueToByteArray(ref byte[] dataToAddTo, ref int counter, short dataBeingAdded)
        {
            dataToAddTo[counter++] = (byte)((((short)dataBeingAdded) & 0xFF00) >> 8);
            dataToAddTo[counter++] = (byte)((((short)dataBeingAdded & 0x00FF)));
        }

        /// <summary>
        /// A helper function to add 24-bit values to the byte array so we don't have to do this every single time.
        /// </summary>
        /// <param name="dataToAddTo">The byte array we are modifying.</param>
        /// <param name="counter">The counter to tell us where in the byte array we are modifying.</param>
        /// <param name="dataBeingAdded">The data we are adding to the byte array.</param>
        public static void Add24BitValueToByteArray(ref byte[] dataToAddTo, ref int counter, int dataBeingAdded)
        {
            dataToAddTo[counter++] = (byte)((((short)dataBeingAdded) & 0xFF0000) >> 16);
            dataToAddTo[counter++] = (byte)((((short)dataBeingAdded) & 0x00FF00) >> 8);
            dataToAddTo[counter++] = (byte)((short)dataBeingAdded & 0x0000FF);
        }

        /// <summary>
        /// A helper function to add 32-bit values to the byte array so we don't have to do this every single time.
        /// </summary>
        /// <param name="dataToAddTo">The byte array we are modifying.</param>
        /// <param name="counter">The counter to tell us where in the byte array we are modifying.</param>
        /// <param name="dataBeingAdded">The data we are adding to the byte array.</param>
        public static void Add32BitValueToByteArray(ref byte[] dataToAddTo, ref int counter, uint dataBeingAdded)
        {
            dataToAddTo[counter++] = (byte)((((short)dataBeingAdded) & 0xFF000000) >> 24);
            dataToAddTo[counter++] = (byte)((((short)dataBeingAdded) & 0x00FF0000) >> 16);
            dataToAddTo[counter++] = (byte)((((short)dataBeingAdded) & 0x0000FF00) >> 8);
            dataToAddTo[counter++] = (byte)((((short)dataBeingAdded & 0x000000FF)));
        }


        private static byte ConvertBoolArrayToByte(bool[] source)
        {
            if (source.Length > 8) throw new ArgumentOutOfRangeException("There can only be 8 bits in a byte array.");

            byte result = 0;

            int index = 8 - source.Length;

            // Loop through the array
            foreach (bool b in source)
            {
                // if the element is 'true' set the bit at that position
                if (b)
                    result |= (byte)(1 << (7 - index));

                index++;
            }

            return result;
        }
    }
}
