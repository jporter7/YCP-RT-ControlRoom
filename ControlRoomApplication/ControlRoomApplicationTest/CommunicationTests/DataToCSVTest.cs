using ControlRoomApplication.Controllers.Communications;
using ControlRoomApplication.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace ControlRoomApplicationTest.CommunicationTests
{
    [TestClass]
    public class DataToCSVTest
    {
        public static string testpath = $"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"test-out-{System.DateTime.Now.ToString("yyyyMMddHHmmss")}")}";

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

            Assert.IsTrue(DataToCSV.ExportToCSV(JunkRFData, testpath));
        }

        [TestMethod]
        public void TestDeleteCSV()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string testfile = Path.Combine(path, "testfile.csv");

            FileStream file = File.Create(testfile);
            file.Close();

            Assert.IsTrue(DataToCSV.DeleteCSVFileWhenDone(testfile));
        }

        [TestCleanup]
        public void Cleanup()
        {
            DataToCSV.DeleteCSVFileWhenDone(testpath);
        }
    }
}
