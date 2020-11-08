using ControlRoomApplication.Controllers.Communications;
using ControlRoomApplication.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ControlRoomApplicationTest.CommunicationTests
{
    [TestClass]
    public class DataToCSVTest
    {
        [TestMethod]
        public void TestConvertDataToCSV()
        {
            RFData junk1 = new RFData();
            junk1.Id = 0;
            junk1.appointment_id = 0;
            junk1.TimeCaptured = new System.DateTime();
            junk1.Intensity = 10000;

            RFData junk2 = new RFData();
            junk2.Id = 1;
            junk2.appointment_id = 0;
            junk2.TimeCaptured = new System.DateTime();
            junk2.Intensity = 20000;

            List<RFData> JunkRFData = new List<RFData>();
            JunkRFData.Add(junk1);
            JunkRFData.Add(junk2);

            Assert.IsTrue(DataToCSV.ExportToCSV(JunkRFData, $"test_out-{System.DateTime.Now.ToString("yyyyMMddHHmmss")}"));
        }
    }
}
