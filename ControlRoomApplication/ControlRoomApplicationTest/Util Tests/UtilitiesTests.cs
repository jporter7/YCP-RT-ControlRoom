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

            // Allow for timestamps containing both EST or EDT, rather than just EST
            if(timeStamp.Contains("EST") || timeStamp.Contains("EDT"))
            {
                timeZone = true;
            }
            
            Assert.IsTrue(timeZone);

        }
    }
}

