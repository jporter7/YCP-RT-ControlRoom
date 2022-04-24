﻿using ControlRoomApplication.Entities;

namespace ControlRoomApplicationTest.EntitiesTests.HeartbeatInterfaceTests
{
    public class ConcreteHeartbeatTestClass : HeartbeatInterface
    {
        public ConcreteHeartbeatTestClass()
        {
        }

        protected override bool KillHeartbeatComponent()
        {
            // kill imaginary heartbeat
            return true;
        }

        public override bool TestIfComponentIsAlive()
        {
            throw new System.NotImplementedException();
        }
    }
}
