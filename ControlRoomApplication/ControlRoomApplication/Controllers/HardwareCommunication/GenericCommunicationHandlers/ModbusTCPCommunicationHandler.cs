using System;
using ControlRoomApplication.Entities;
using Modbus.Device;

namespace ControlRoomApplication.Controllers
{
    //public class ModbusTCPCommunicationHandler : AbstractHardwareCommunicationHandler<uint>
    //{
    //    public override uint[] ReadResponse()
    //    {
    //        throw new NotImplementedException("" +
    //            "There's no meaning to this functionality in this context. Modbus slaves only communicate when requested, and the " +
    //            "NModbus4 library handles getting the responses when read requests are made."
    //        );
    //    }

    //    public override void SendMessage(HardwareMessageTypeEnum MessageType, params object[] MessageParameters)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override uint[] SendMessageAndReadResponse(HardwareMessageTypeEnum MessageType, params object[] MessageParameters)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    protected override void CommunicationRoutine()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    protected override void InitCommunicationThreadElements()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
