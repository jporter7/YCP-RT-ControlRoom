using System;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;

namespace ControlRoomApplication.Controllers
{
    public class YCPBaseTCPIPCommunicationHandler : BaseTCPIPCommunicationHandler
    {
        private static readonly byte[] OUTGOING_MESSAGE_TEMPLATE =
        {
            0x13, 0x0,
            0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0
        };

        private static readonly int EXPECTED_FULL_RESPONSE_SIZE = 19;
        private static readonly int EXPECTED_MINOR_RESPONSE_SIZE = 3;

        public YCPBaseTCPIPCommunicationHandler(string ip, int port) : base(ip, port)
        {
            // Does nothing
        }

        public YCPBaseTCPIPCommunicationHandler(int port) : this(MiscellaneousConstants.LOCAL_HOST_IP, port)
        {
            // Does nothing
        }

        private byte[] GetNewOutgoingMessageTemplate(HardwareMessageTypeEnum MessageType)
        {
            byte[] template = (byte[])(OUTGOING_MESSAGE_TEMPLATE.Clone());
            template[2] = HardwareMessageTypeEnumConversionHelper.ConvertToByte(MessageType);
            return template;
        }

        private byte[] ExchangeSimpleMessage(HardwareMessageTypeEnum MessageType, bool ExpectsResponse)
        {
            return QueueOutgoingMessageAndReadResponse(GetNewOutgoingMessageTemplate(MessageType), ExpectsResponse, ExpectsResponse ? EXPECTED_FULL_RESPONSE_SIZE : EXPECTED_MINOR_RESPONSE_SIZE);
        }

        private static bool ResponseMetBasicExpectations(byte[] ResponseBytes, int ExpectedSize)
        {
            return ((ResponseBytes[0] + (ResponseBytes[1] * 256)) == ExpectedSize) && (ResponseBytes[2] == 0x1);
        }

        private static bool MinorResponseIsValid(byte[] MinorResponseBytes)
        {
            return ResponseMetBasicExpectations(MinorResponseBytes, EXPECTED_MINOR_RESPONSE_SIZE);
        }

        //
        // Start overloaded definitions
        //

        public override bool CalibrateRadioTelescope()
        {
            return MinorResponseIsValid(ExchangeSimpleMessage(HardwareMessageTypeEnum.CALIBRATE, false));
        }

        public override bool CancelCurrentMoveCommand()
        {
            return MinorResponseIsValid(ExchangeSimpleMessage(HardwareMessageTypeEnum.CANCEL_ACTIVE_OBJECTIVE_AZEL_POSITION, false));
        }
        
        public override bool ConfigureRadioTelescope(double startSpeedDPSAzimuth, double startSpeedDPSElevation, int homeTimeoutSecondsAzimuth, int homeTimeoutSecondsElevation)
        {
            byte[] NetOutgoingMessage = GetNewOutgoingMessageTemplate(HardwareMessageTypeEnum.SET_CONFIGURATION);

            int StartSpeedAzimuth = ConversionHelper.DPSToSPS(startSpeedDPSAzimuth, MotorConstants.GEARING_RATIO_AZIMUTH);
            int StartSpeedElevation = ConversionHelper.DPSToSPS(startSpeedDPSElevation, MotorConstants.GEARING_RATIO_ELEVATION);

            NetOutgoingMessage[3] = 0x84;
            NetOutgoingMessage[4] = 0x00;
            NetOutgoingMessage[5] = 0x00;
            NetOutgoingMessage[6] = 0x00;

            NetOutgoingMessage[7] = 0x0;
            NetOutgoingMessage[8] = (byte)(StartSpeedAzimuth / 0xFFFF);
            NetOutgoingMessage[9] = (byte)((StartSpeedAzimuth >> 8) & 0xFF);
            NetOutgoingMessage[10] = (byte)(StartSpeedAzimuth & 0xFF);

            NetOutgoingMessage[11] = 0x0;
            NetOutgoingMessage[12] = (byte)(StartSpeedElevation / 0xFFFF);
            NetOutgoingMessage[13] = (byte)((StartSpeedElevation >> 8) & 0xFF);
            NetOutgoingMessage[14] = (byte)(StartSpeedElevation & 0xFF);

            NetOutgoingMessage[15] = (byte)(homeTimeoutSecondsAzimuth >> 8);
            NetOutgoingMessage[16] = (byte)(homeTimeoutSecondsAzimuth & 0xFF);

            NetOutgoingMessage[17] = (byte)(homeTimeoutSecondsElevation >> 8);
            NetOutgoingMessage[18] = (byte)(homeTimeoutSecondsElevation & 0xFF);

            return MinorResponseIsValid(QueueOutgoingMessageAndReadResponse(NetOutgoingMessage, false, EXPECTED_MINOR_RESPONSE_SIZE));
        }

        public override bool ExecuteRadioTelescopeControlledStop()
        {
            return MinorResponseIsValid(ExchangeSimpleMessage(HardwareMessageTypeEnum.CONTROLLED_STOP, false));
        }

        public override bool ExecuteRadioTelescopeImmediateStop()
        {
            return MinorResponseIsValid(ExchangeSimpleMessage(HardwareMessageTypeEnum.IMMEDIATE_STOP, false));
        }

        public override bool ExecuteRelativeMove(double speedDPSAzimuth, double translationDegreesAzimuth, double speedDPSElevation, double translationDegreesElevation)
        {
            //byte[] NetOutgoingMessage = GetNewOutgoingMessageTemplate(HardwareMessageTypeEnum.TRANSLATE_AZEL_POSITION);

            //int AxisPeakSpeed = (int)((speedDPSAzimuth * MotorConstants.GEARED_STEPS_PER_REVOLUTION) / 60);
            //int AxisTranslation = (int)(translationDegreesAzimuth * MotorConstants.GEARED_STEPS_PER_REVOLUTION / 360);

            //switch (axis)
            //{
            //    case RadioTelescopeAxisEnum.AZIMUTH:
            //        {
            //            NetOutgoingMessage[3] = 0x1;
            //            break;
            //        }

            //    case RadioTelescopeAxisEnum.ELEVATION:
            //        {
            //            NetOutgoingMessage[3] = 0x2;
            //            break;
            //        }

            //    default:
            //        {
            //            throw new ArgumentException("Invalid RadioTelescopeAxisEnum value seen while preparing relative movement bytes: " + axis.ToString());
            //        }
            //}

            //NetOutgoingMessage[4] = 0x0;
            //NetOutgoingMessage[5] = (byte)(AxisPeakSpeed / 0xFFFF);
            //NetOutgoingMessage[6] = (byte)((AxisPeakSpeed >> 8) & 0xFF);
            //NetOutgoingMessage[7] = (byte)(AxisPeakSpeed & 0xFF);

            //if (AxisTranslation > 0)
            //{
            //    NetOutgoingMessage[8] = 0x0;
            //    NetOutgoingMessage[9] = (byte)(AxisTranslation / 0xFFFF);
            //    NetOutgoingMessage[10] = (byte)((AxisTranslation >> 8) & 0xFF);
            //    NetOutgoingMessage[11] = (byte)(AxisTranslation & 0xFF);
            //}
            //else
            //{
            //    NetOutgoingMessage[8] = 0xFF;
            //    NetOutgoingMessage[9] = (byte)((AxisTranslation / 0xFFFF) - 1);
            //    NetOutgoingMessage[10] = (byte)((AxisTranslation >> 8) & 0xFF);
            //    NetOutgoingMessage[11] = (byte)(AxisTranslation & 0xFF);
            //}

            //return MinorResponseIsValid(QueueOutgoingMessageAndReadResponse(NetOutgoingMessage, false, EXPECTED_MINOR_RESPONSE_SIZE));

            throw new NotImplementedException("This is unsupported after recent refactoring and due to time constraints.");
        }

        public override bool[] GetCurrentLimitSwitchStatuses()
        {
            byte[] result = ExchangeSimpleMessage(HardwareMessageTypeEnum.GET_CURRENT_SAFETY_INTERLOCK_STATUS, true);

            if (!ResponseMetBasicExpectations(result, EXPECTED_FULL_RESPONSE_SIZE))
            {
                return null;
            }

            bool[] Statuses = new bool[4];

            byte DataByte = result[3];
            for (int i = 0; i < 4; i++)
            {
                switch (LimitSwitchStatusConversionHelper.GetFromByte((byte)((DataByte >> (2 * (3 - i))) & 0x3)))
                {
                    case LimitSwitchStatusEnum.WITHIN_WARNING_LIMITS:
                        {
                            Statuses[i] = true;
                            break;
                        }

                    case LimitSwitchStatusEnum.WITHIN_SAFE_LIMITS:
                        {
                            Statuses[i] = false;
                            break;
                        }

                    default:
                        {
                            throw new ArgumentException("Unrecognized/Invalid response for byte-casted limit switch status.");
                        }
                }
            }

            return Statuses;
        }

        public override Orientation GetCurrentOrientation()
        {
            byte[] result = ExchangeSimpleMessage(HardwareMessageTypeEnum.GET_CURRENT_AZEL_POSITIONS, true);
            if (!ResponseMetBasicExpectations(result, EXPECTED_FULL_RESPONSE_SIZE))
            {
                return null;
            }

            return new Orientation(BitConverter.ToDouble(result, 3), BitConverter.ToDouble(result, 11));
        }

        public override bool GetCurrentSafetyInterlockStatus()
        {
            byte[] result = ExchangeSimpleMessage(HardwareMessageTypeEnum.GET_CURRENT_SAFETY_INTERLOCK_STATUS, true);
            return ResponseMetBasicExpectations(result, EXPECTED_FULL_RESPONSE_SIZE);
        }

        public override bool MoveRadioTelescopeToOrientation(Orientation orientation)
        {
            byte[] NetOutgoingMessage = GetNewOutgoingMessageTemplate(HardwareMessageTypeEnum.SET_OBJECTIVE_AZEL_POSITION);

            Array.Copy(BitConverter.GetBytes(orientation.Azimuth), 0, NetOutgoingMessage, 3, 8);
            Array.Copy(BitConverter.GetBytes(orientation.Elevation), 0, NetOutgoingMessage, 11, 8);

            return MinorResponseIsValid(QueueOutgoingMessageAndReadResponse(NetOutgoingMessage, false, EXPECTED_MINOR_RESPONSE_SIZE));
        }

        public override bool ShutdownRadioTelescope()
        {
            return MinorResponseIsValid(ExchangeSimpleMessage(HardwareMessageTypeEnum.SHUTDOWN, false));
        }

        public override bool StartRadioTelescopeJog(RadioTelescopeAxisEnum axis, double speedDPS, bool clockwise)
        {
            byte[] NetOutgoingMessage = GetNewOutgoingMessageTemplate(HardwareMessageTypeEnum.SET_OBJECTIVE_AZEL_POSITION);

            int AxisJogSpeed;

            switch (axis)
            {
                case RadioTelescopeAxisEnum.AZIMUTH:
                    {
                        NetOutgoingMessage[3] = 0x1;
                        AxisJogSpeed = ConversionHelper.DPSToSPS(speedDPS, MotorConstants.GEARING_RATIO_AZIMUTH);
                        break;
                    }

                case RadioTelescopeAxisEnum.ELEVATION:
                    {
                        NetOutgoingMessage[3] = 0x2;
                        AxisJogSpeed = ConversionHelper.DPSToSPS(speedDPS, MotorConstants.GEARING_RATIO_ELEVATION);
                        break;
                    }

                default:
                    {
                        throw new ArgumentException("Invalid RadioTelescopeAxisEnum value seen while preparing jog movement bytes: " + axis.ToString());
                    }
            }

            NetOutgoingMessage[4] = 0x0;
            NetOutgoingMessage[5] = (byte)(AxisJogSpeed / 0xFFFF);
            NetOutgoingMessage[6] = (byte)((AxisJogSpeed >> 8) & 0xFF);
            NetOutgoingMessage[7] = (byte)(AxisJogSpeed & 0xFF);

            NetOutgoingMessage[8] = (byte)(clockwise ? 0x1 : 0x2);

            return MinorResponseIsValid(QueueOutgoingMessageAndReadResponse(NetOutgoingMessage, false, EXPECTED_MINOR_RESPONSE_SIZE));
        }

        public override bool TestCommunication()
        {
            byte[] result = ExchangeSimpleMessage(HardwareMessageTypeEnum.TEST_CONNECTION, true);
            return ResponseMetBasicExpectations(result, EXPECTED_FULL_RESPONSE_SIZE);
        }
    }
}
