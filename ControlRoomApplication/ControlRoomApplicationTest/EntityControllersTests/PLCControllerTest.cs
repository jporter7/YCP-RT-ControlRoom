using System;
using ControlRoomApplication.Controllers.PLCController;
using ControlRoomApplication.Entities.Plc;
using ControlRoomApplication.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControlRoomApplicationTest.EntityControllersTests
{
    [TestClass]
    public class PLCControllerTest
    {
        [TestInitialize]
        public void BuildUp()
        {
            
        }

        [TestMethod]
        public void TestTelescopeLimitsMethod()
        {
            PLCController plccontroller = new PLCController();
            AbstractPLC plc = new ScaleModelPLC("127.0.0.1", 8080);
            //Right Ascention Out of Bounds Tests
            //Bad RightAscension positive
            Boolean test = false;
            Coordinate badRighta = new Coordinate(360, 0);
            try
            {
                plccontroller.MoveTelescope(plc, badRighta);
            }
            catch (SystemException e)
            {
                test = true;
            }
            Assert.AreEqual(test, true);

            //Bad RightAscension negative
            badRighta.RightAscension = -360;
            test = false;
            try
            {
                plccontroller.MoveTelescope(plc, badRighta);
            }
            catch (SystemException e)
            {
                test = true;
            }
            Assert.AreEqual(test, true);

            //Declination Out of Bounds Tests
            //Bad Dec positive
            Coordinate badDec = new Coordinate(0, 91);
            test = false;
            try
            {
                plccontroller.MoveTelescope(plc, badDec);
            }
            catch (SystemException e)
            {
                test = true;
            }
            Assert.AreEqual(test, true);

            //Bad Dec negative
            badDec.Declination = -91;
            test = false;
            try
            {
                plccontroller.MoveTelescope(plc, badDec);
            }
            catch (SystemException e)
            {
                test = true;
            }
            Assert.AreEqual(test, true);

            //Need to Discuss with Jason Best Practice for Testing Positive Outcomes
        }

        [TestMethod]
        public void TestShutdownMethod()
        {

        }

        [TestMethod]
        public void TestCalibrateMethod()
        {

        }

        [TestMethod]
        public void TestMoveScaleModelMethod()
        {

        }

    }
}
