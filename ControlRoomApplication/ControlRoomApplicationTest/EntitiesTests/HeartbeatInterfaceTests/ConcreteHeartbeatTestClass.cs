using ControlRoomApplication.Entities;

namespace ControlRoomApplicationTest.EntitiesTests.HeartbeatInterfaceTests
{
    public class ConcreteHeartbeatTestClass : HeartbeatInterface
    {
        public ConcreteHeartbeatTestClass()
        {
        }

        protected override bool KillHeartbeatComponent()
        {
            throw new System.NotImplementedException();
        }

        protected override bool TestIfComponentIsAlive()
        {
            throw new System.NotImplementedException();
        }
    }
}
