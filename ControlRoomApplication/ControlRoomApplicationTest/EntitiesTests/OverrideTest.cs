using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlRoomApplication.GUI;
using ControlRoomApplication.Main;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControlRoomApplicationTest.EntitiesTests
{
    [TestClass]
    public class OverrideTest
    {
        // Class being tested
        private DiagnosticsForm df;
        private MainForm m;

        [TestInitialize]
        public void BuildUp()
        {
            m = new MainForm();
            DiagnosticsForm diagnosticForm = new DiagnosticsForm(m.MainControlRoomController.ControlRoom, 0, m);
        }

        [TestMethod]
        public void AllOverridesFalse()
        {
            // PLC
            Assert.IsFalse(df.getRTController().RadioTelescope.PLCDriver.overrides.overrideGate);
            Assert.IsFalse(df.getRTController().RadioTelescope.PLCDriver.overrides.overrideAzimuthProx1);
            Assert.IsFalse(df.getRTController().RadioTelescope.PLCDriver.overrides.overrideAzimuthProx2);
            Assert.IsFalse(df.getRTController().RadioTelescope.PLCDriver.overrides.overrideElevatProx1);
            Assert.IsFalse(df.getRTController().RadioTelescope.PLCDriver.overrides.overrideElevatProx2);

            // No PLC
            Assert.IsFalse(df.getRTController().RadioTelescope.PLCDriver.overrides.overrideAzimuthMotTemp);
            Assert.IsFalse(df.getRTController().RadioTelescope.PLCDriver.overrides.overrideElevatMotTemp);

            // Weather Station
            Assert.IsFalse(m.getWSOverride());
        }

        [TestMethod]
        public void AllOverridesTrue()
        {
            // Change values
            // PLC
            df.getRTController().RadioTelescope.PLCDriver.overrides.overrideGate = true;
            df.getRTController().RadioTelescope.PLCDriver.overrides.overrideAzimuthProx1 = true;
            df.getRTController().RadioTelescope.PLCDriver.overrides.overrideAzimuthProx2 = true ;
            df.getRTController().RadioTelescope.PLCDriver.overrides.overrideElevatProx1 = true;
            df.getRTController().RadioTelescope.PLCDriver.overrides.overrideElevatProx2 = true;

            // No PLC
            df.getRTController().RadioTelescope.PLCDriver.overrides.overrideAzimuthMotTemp = true;
            df.getRTController().RadioTelescope.PLCDriver.overrides.overrideElevatMotTemp = true;

            // Weather Station
            m.setWSOverride(true);

            // Assertions
            // PLC
            Assert.IsTrue(df.getRTController().RadioTelescope.PLCDriver.overrides.overrideGate);
            Assert.IsTrue(df.getRTController().RadioTelescope.PLCDriver.overrides.overrideAzimuthProx1);
            Assert.IsTrue(df.getRTController().RadioTelescope.PLCDriver.overrides.overrideAzimuthProx2);
            Assert.IsTrue(df.getRTController().RadioTelescope.PLCDriver.overrides.overrideElevatProx1);
            Assert.IsTrue(df.getRTController().RadioTelescope.PLCDriver.overrides.overrideElevatProx2);

            // No PLC
            Assert.IsTrue(df.getRTController().RadioTelescope.PLCDriver.overrides.overrideAzimuthMotTemp);
            Assert.IsTrue(df.getRTController().RadioTelescope.PLCDriver.overrides.overrideElevatMotTemp);

            // Weather Station
            Assert.IsTrue(m.getWSOverride());
        }
    }
}
