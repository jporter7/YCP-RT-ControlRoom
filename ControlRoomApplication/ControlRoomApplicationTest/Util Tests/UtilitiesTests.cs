using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlRoomApplication.Util;

namespace ControlRoomApplication.UtilitiesTests
{

    [TestClass]
    public class UtilitiesTests { 
    
        
        [TestMethod]
        public void TestGetTimeStamp()
        {
            string timeStamp = Utilities.GetTimeStamp();
            bool timeZone = false;

            if(timeStamp.Contains("EST"))
            {
                timeZone = true;
            }

            //THIS WILL ONLY WORK ON COMPUTER SET TO EST
            Assert.IsTrue(timeZone);


        }
    }
}

