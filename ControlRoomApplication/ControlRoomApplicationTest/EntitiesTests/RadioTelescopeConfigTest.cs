using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlRoomApplication.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ControlRoomApplication.EntitiesTests {

   [TestClass]
   public class RadioTelescopeConfigTest {
        private string validJson1 = "{\"telescopeID\":134,\"newTelescope\":false}";
        private string validJson2 = "{\"telescopeID\":1,\"newTelescope\":true}";
        private string validJson3 = "{\"telescopeID\":100,\"newTelescope\":false}";
        private string invalidJson1 = "{\"telescopeID\":,\"newTelescope\":}";
        private string invalidJson2 = "{\"telescopeID\":adasd,\"newTelescope\":dff}";
        private string invalidJson3 = "";
        private string invalidJson4 = "\"telescopeID\":adasd\"newTelescope\":dff}";
        public RadioTelescopeConfig rtc1 = new RadioTelescopeConfig(134, false);
        public  RadioTelescopeConfig rtc2 = new RadioTelescopeConfig(1, true);
        public  RadioTelescopeConfig rtc3 = new RadioTelescopeConfig(100, false);

        [TestMethod]
        public void TestDeserializeAndSerialize()
        {
            // Each one of these tests will attempt to serialize a given RTConfig instance, write it to the JSON file, and then 
            // retrieve the contents by deserializing the file. 
            // IF things work as they should, the test object which is being pulled from the JSON file should
            // match the one placed in it by SerializeRTConfig
            RadioTelescopeConfig.SerializeRTConfig(rtc1, true);
            RadioTelescopeConfig rtcTest1 = RadioTelescopeConfig.DeserializeRTConfig(true);
            Assert.AreEqual(rtcTest1.telescopeID, rtc1.telescopeID);
            Assert.AreEqual(rtcTest1.newTelescope, rtc1.newTelescope);

            RadioTelescopeConfig.SerializeRTConfig(rtc2, true);
            RadioTelescopeConfig rtcTest2 = RadioTelescopeConfig.DeserializeRTConfig(true);
            Assert.AreEqual(rtcTest2.telescopeID, rtc2.telescopeID);
            Assert.AreEqual(rtcTest2.newTelescope, rtc2.newTelescope);

            RadioTelescopeConfig.SerializeRTConfig(rtc3, true);
            RadioTelescopeConfig rtcTest3 = RadioTelescopeConfig.DeserializeRTConfig(true);
            Assert.AreEqual(rtcTest3.telescopeID, rtc3.telescopeID);
            Assert.AreEqual(rtcTest3.newTelescope, rtc3.newTelescope);

        }

        [TestMethod]
        public void TestCreateAndWriteToJSONFile()
        {
            // In these tests, we are passing invalid and valid JSON strings to the file. The expected behavior
            // is that the DeserializeRTConfig should return null if a JsonReaderException occurs (which, for an invalid JSON string, will occur).

            // In the valid JSON instances, the resulting objects should match the values of the validJsonX strings.
            RadioTelescopeConfig.CreateAndWriteToNewJSONFile(validJson1, true);
            RadioTelescopeConfig rtcTest1 = RadioTelescopeConfig.DeserializeRTConfig(true);
            Assert.AreEqual(rtcTest1.telescopeID, 134);
            Assert.AreEqual(rtcTest1.newTelescope, false);

            RadioTelescopeConfig.CreateAndWriteToNewJSONFile(validJson2, true);
            RadioTelescopeConfig rtcTest2 = RadioTelescopeConfig.DeserializeRTConfig(true);
            Assert.AreEqual(rtcTest2.telescopeID, 1);
            Assert.AreEqual(rtcTest2.newTelescope, true);

            RadioTelescopeConfig.CreateAndWriteToNewJSONFile(validJson3, true);
            RadioTelescopeConfig rtcTest3 = RadioTelescopeConfig.DeserializeRTConfig(true);
            Assert.AreEqual(rtcTest3.telescopeID, 100);
            Assert.AreEqual(rtcTest3.newTelescope, false);


            // For the invalid instances, the DeserializeRTConfig method will return null due to the JsonReaderException
            RadioTelescopeConfig.CreateAndWriteToNewJSONFile(invalidJson1, true);
            RadioTelescopeConfig rtcTest4 = RadioTelescopeConfig.DeserializeRTConfig(true);
            Assert.IsNull(rtcTest4);

            RadioTelescopeConfig.CreateAndWriteToNewJSONFile(invalidJson2, true);
            RadioTelescopeConfig rtcTest5 = RadioTelescopeConfig.DeserializeRTConfig(true);
            Assert.IsNull(rtcTest5);

            RadioTelescopeConfig.CreateAndWriteToNewJSONFile(invalidJson3, true);
            RadioTelescopeConfig rtcTest6 = RadioTelescopeConfig.DeserializeRTConfig(true);
            Assert.IsNull(rtcTest6);

            RadioTelescopeConfig.CreateAndWriteToNewJSONFile(invalidJson4, true);
            RadioTelescopeConfig rtcTest7 = RadioTelescopeConfig.DeserializeRTConfig(true);
            Assert.IsNull(rtcTest7);
        }

    }
}
