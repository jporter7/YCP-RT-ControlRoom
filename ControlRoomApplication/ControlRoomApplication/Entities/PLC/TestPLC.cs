using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Entities.Plc
{
    /// <summary>
    /// This implementation of the AbstractPLC is strictly used in unit tests
    /// where the test itself does not care what the PLC does, but needs to ensure
    /// the functionality of the component is as expected. This is necessary since the
    /// other implementation of the AbstractPLC, the ScaleModelPLC requires being hooked up
    /// to the actual ScaleModel
    /// </summary>
    public class TestPLC : AbstractPLC
    {
        public TestPLC() { }
    }
}
