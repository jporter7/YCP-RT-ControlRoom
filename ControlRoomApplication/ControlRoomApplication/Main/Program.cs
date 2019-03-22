using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ControlRoomApplication.Controllers;
using ControlRoomApplication.Controllers.PLCCommunication;
using ControlRoomApplication.Controllers.RadioTelescopeControllers;
using ControlRoomApplication.Database.Operations;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Entities.RadioTelescope;

namespace ControlRoomApplication.Main
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Application.Run(new MainForm());
        }
        
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}